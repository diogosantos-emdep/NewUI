using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Xpf.Core;
using System.Threading;
using System.Windows;
using Prism.Logging;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.PCM;
using System.Data;
using System.Globalization;
using Emdep.Geos.Modules.ERM.Views;
using System.ComponentModel;

namespace Emdep.Geos.Modules.ERM.CommonClasses
{
    public static class ERMTimeTrackingGetData
    {

        public static CancellationTokenSource tokenToLoadFullTimetrackingInAsync;
        public static System.Threading.Tasks.Task<List<TimeTracking>> taskToLoadTimetrackingInAsync;

        public static List<int> AppSettingData;//[GEOS2-6620][gulab lakade][19 12 2024]

        // CustomObservableCollection<TimeTracking>  List<TimeTracking>
        // [GEOS2-5098] [gulab lakade][30 11 2023]
        //public static async void GetERMTimeTrackingFromServiceAsync(
        // IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
        //   GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
        //   , List<GeosAppSetting> GeosAppSettingList, List<Site> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList)
        //public static async void GetERMTimeTrackingFromServiceAsync(
        //IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
        //  GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
        //  , List<GeosAppSetting> GeosAppSettingList, List<ERMTimetrackingSite> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList)
        // public static async void GetERMTimeTrackingFromServiceAsync(
        //IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
        //  GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
        //  , List<GeosAppSetting> GeosAppSettingList, List<ERMTimetrackingSite> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList, List<TimeTrackingProductionStage> TimeTrackingProductionStage)//[gulab lakade][11 03 2024][GEOS2-5466]

        //  public static async void GetERMTimeTrackingFromServiceAsync(
        //IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
        //  GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
        //  , List<GeosAppSetting> GeosAppSettingList, List<ERMTimetrackingSite> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList)// [Rupali Sarode][GEOS2-5522][21-03-2024] 

        //        public static async void GetERMTimeTrackingFromServiceAsync(
        //IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
        //GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
        //, List<GeosAppSetting> GeosAppSettingList, List<ERMTimetrackingSite> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList, List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList);//[GEOS2-5854][gulab lakade][19 07 2024]
        public static async void GetERMTimeTrackingFromServiceAsync(
IERMService ERMService, IPLMService PLMService, List<TimeTracking> TimeTrackingListold,
 GridControl GridControl1, string CurrencyNameFromSetting, string PlantName, DataTable DtTimetracking, DataTable DataTableForGridLayout
 , List<GeosAppSetting> GeosAppSettingList, List<ERMTimetrackingSite> PlantList, ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, string WarningFailedPlants, bool IsShowFailedPlantWarning, List<Site> AllPlantsList, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList, List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList, List<int> AppSettingData_old, string fromDate, string toDate)

        {
            const string methodNameWithBrackets = nameof(GetERMTimeTrackingFromServiceAsync) + "()";
            try
            {
                //List<TimeTrackingProductionStage> TimeTrackingProductionStage = TimetrackingStagesList.AllStagesList;
                AppSettingData = AppSettingData_old;//[GEOS2-6620][gulab lakade][19 12 2024]
                ERMCommon.Instance.UnloadAsyncTimetracking = true;
                ERMCommon.Instance.PlantVisibleFlag = false;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                foreach (string itemSelectedPlant in PlantName.Split(','))
                {

                    int count = 1;
                    //foreach (var item in PlantList.Where(i => i.Name != itemSelectedPlant.Trim()))  ////[GEOS2-5098] [gulab lakade][30 11 2023]
                    foreach (var item in PlantList.Where(i => i.ProductionSite != itemSelectedPlant.Trim()))  ////[GEOS2-5098] [gulab lakade][30 11 2023]
                    {
                        count++;
                        ERMCommon.Instance.PlantVisibleFlag = false;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                        //if (count < 2)
                        //{
                        if (ERMCommon.Instance.UnloadAsyncTimetracking == true)
                        {
                            ERMCommon.Instance.TimeTrackingLoadingMessage = string.Empty;
                            //ERMCommon.Instance.TimeTrackingLoadingMessage = "Please wait while getting data from the plant " + item.Name + "  in background...";
                            //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMCommon.Instance.TimeTrackingLoadingMessage = "Please wait while getting data from the plant " + item.ProductionSite + "  in background...";
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.ProductionSite).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            if (serviceurl != null)
                            {
                                ERMService = new ERMServiceController(serviceurl);



                                if (tokenToLoadFullTimetrackingInAsync != null
                                    //&& taskToLoadFullYearAttendanceInAsync.Status==TaskStatus.
                                    )
                                {
                                    try
                                    {
                                        tokenToLoadFullTimetrackingInAsync.Cancel();
                                        taskToLoadTimetrackingInAsync.Wait(tokenToLoadFullTimetrackingInAsync.Token);
                                        GeosApplication.Instance.Logger.Log($"In method {methodNameWithBrackets}, token to load time tracking is canceled", category: Category.Info, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        GeosApplication.Instance.Logger.Log($"Get an error in {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                                    }
                                }
                                tokenToLoadFullTimetrackingInAsync = new CancellationTokenSource();
                                ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Visible;

                                //[GEOS2-4173][Rupali Sarode][06-03-2023]
                                //var newTimeTracking = await GetERMTimeTrackingFromServiceAsyncTask(
                                //ERMService, PLMService, itemSelectedPlant, PlantList.Where(i => i.Name == itemSelectedPlant.Trim()).FirstOrDefault().IdSite, item.Name, GeosAppSettingList, TimeTrackingListold);

                                //var newTimeTracking = await GetERMTimeTrackingFromServiceAsyncTask(
                                //ERMService, PLMService, itemSelectedPlant, AllPlantsList.Where(i => i.Name == itemSelectedPlant.Trim()).FirstOrDefault().IdSite, item.Name, GeosAppSettingList, TimeTrackingListold);////[GEOS2-5098] [gulab lakade][30 11 2023]
                                var newTimeTracking = await GetERMTimeTrackingFromServiceAsyncTask(
                               ERMService, PLMService, itemSelectedPlant, AllPlantsList.Where(i => i.Name == itemSelectedPlant.Trim()).FirstOrDefault().IdSite, item, GeosAppSettingList, TimeTrackingListold, fromDate, toDate);  ////[GEOS2-5098] [gulab lakade][30 11 2023]

                                if (tokenToLoadFullTimetrackingInAsync != null &&
                                    tokenToLoadFullTimetrackingInAsync.IsCancellationRequested)
                                {
                                    return;
                                }

                                if (TimeTrackingListold == null)
                                {
                                    TimeTrackingListold = new List<Data.Common.ERM.TimeTracking>();
                                }

                                //else
                                //    timeTrackingList1.ClearWithTemporarySuppressedNotification();

                                //timeTrackingList1.AddRangeWithTemporarySuppressedNotification(newTimeTracking);
                                //List<TimeTracking> TimeTracking = new List<TimeTracking>();
                                //foreach (TimeTracking Trackingitem in newTimeTracking)
                                //{
                                //    if (!timeTrackingList1.Any(a=>a.Offer==Trackingitem.Offer))
                                //    {
                                //        timeTrackingList1.Add(Trackingitem);
                                //        TimeTracking.Add(Trackingitem);
                                //    }
                                //}

                                FillDeliveryweek(TemplateWithTimeTracking, newTimeTracking);
                                //FillDashboard(DtTimetracking, newTimeTracking, DataTableForGridLayout);
                                // FillDashboard(DtTimetracking, newTimeTracking, DataTableForGridLayout, TimeTrackingProductionStage);//[gulab lakade][11 03 2024][GEOS2-5466]
                                //FillDashboard(DtTimetracking, newTimeTracking, DataTableForGridLayout, TimeTrackingProductionStage, TimetrackingStagesList); // [Rupali Sarode][GEOS2-5522][21-03-2024]
                                FillDashboard(DtTimetracking, newTimeTracking, DataTableForGridLayout, TimeTrackingProductionStage, TimetrackingStagesList, CADCAMDesignTypeList); //[GEOS2-5854][gulab lakade][19 07 2024]
                                 //if (GridControl1 != null)
                                //{
                                //    GridControl1.RefreshData();
                                //}
                            }
                        }
                        else if (ERMCommon.Instance.UnloadAsyncTimetracking == false)
                        {
                            tokenToLoadFullTimetrackingInAsync.Cancel();
                            tokenToLoadFullTimetrackingInAsync.Dispose();
                            tokenToLoadFullTimetrackingInAsync.Token.ThrowIfCancellationRequested();
                            ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Hidden;
                            ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                        }
                    }
                    if (count == PlantList.Count())
                    {
                        //ERMCommon.Instance.UnloadAsyncTimetracking = false;
                        ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                    }
                    // }
                }
                ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Hidden;
                if (ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground == Visibility.Hidden)
                {
                    ERMCommon.Instance.PlantVisibleFlag = true;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                }
                else
                {
                    ERMCommon.Instance.PlantVisibleFlag = false;  //[gulab lakade][21 04 2023][Plant dropdown disable]
                }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;//[gulab lakade][03 05 2023][message display]
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;//[gulab lakade][03 05 2023][message display]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;//[gulab lakade][03 05 2023][message display]
                GeosApplication.Instance.SplashScreenMessage = string.Empty;//[gulab lakade][03 05 2023][message display]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;//[gulab lakade][03 05 2023][message display]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        #region ////[GEOS2-5098] [gulab lakade][30 11 2023]
        //public static async Task<List<TimeTracking>>
        // GetERMTimeTrackingFromServiceAsyncTask(
        //     IERMService ERMService, IPLMService PLMService, string plantName, UInt32 idSite, string IDPlantName, List<GeosAppSetting> GeosAppSettingList, List<TimeTracking> TimeTrackingListold)
        //{
        //    const string methodNameWithBrackets = nameof(GetERMTimeTrackingFromServiceAsyncTask) + "()";
        //    var TimeTrackingList = new List<TimeTracking>();
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
        //        string CurrencyNameFromSetting = null;
        //        if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
        //        {
        //            CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
        //        }
        //        ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Visible;
        //        taskToLoadTimetrackingInAsync =
        //            new Task<List<TimeTracking>>(() =>
        //            {
        //                return ReturnFullTimeTrackingList_V2340(ERMService, PLMService, ref TimeTrackingList, CurrencyNameFromSetting, plantName, idSite, IDPlantName, GeosAppSettingList, TimeTrackingListold);

        //            }, tokenToLoadFullTimetrackingInAsync.Token);
        //        taskToLoadTimetrackingInAsync.Start();
        //        GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
        //        return await taskToLoadTimetrackingInAsync;
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log($"Error in Method {methodNameWithBrackets} Method - ServiceUnexceptedException " + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
        //    }
        //    return null;
        //}

        public static async Task<List<TimeTracking>>
    GetERMTimeTrackingFromServiceAsyncTask(
        IERMService ERMService, IPLMService PLMService, string plantName, UInt32 idSite, ERMTimetrackingSite ProductionSiteDetail, List<GeosAppSetting> GeosAppSettingList, List<TimeTracking> TimeTrackingListold, string fromDate, string toDate)
        {
            const string methodNameWithBrackets = nameof(GetERMTimeTrackingFromServiceAsyncTask) + "()";
            var TimeTrackingList = new List<TimeTracking>();
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
                string CurrencyNameFromSetting = null;
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                {
                    CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                }
                ERMCommon.Instance.IsVisibleLabelTimetrackingInBackground = Visibility.Visible;
                taskToLoadTimetrackingInAsync =
                    new Task<List<TimeTracking>>(() =>
                    {
                        return ReturnFullTimeTrackingList_V2340(ERMService, PLMService, ref TimeTrackingList, CurrencyNameFromSetting, plantName, idSite, ProductionSiteDetail, GeosAppSettingList, TimeTrackingListold, fromDate, toDate);

                    }, tokenToLoadFullTimetrackingInAsync.Token);
                taskToLoadTimetrackingInAsync.Start();
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                return await taskToLoadTimetrackingInAsync;
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

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }
        #endregion
        //private static void FillDashboard(DataTable DtTimetracking, List<TimeTracking> TimeTrackingListnew, DataTable DataTableForGridLayout)
        // private static void FillDashboard(DataTable DtTimetracking, List<TimeTracking> TimeTrackingListnew, DataTable DataTableForGridLayout, List<TimeTrackingProductionStage> TimeTrackingProductionStage)//[gulab lakade][11 03 2024][GEOS2-5466]
        // private static void FillDashboard(DataTable DtTimetracking, List<TimeTracking> TimeTrackingListnew, DataTable DataTableForGridLayout, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList) // [Rupali Sarode][GEOS2-5522][21-03-2024]
        //private static void FillDashboard(DataTable DtTimetracking, List<TimeTracking> TimeTrackingListnew, DataTable DataTableForGridLayout, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList, List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList) //[GEOS2-5854][gulab lakade][19 07 2024]
        private static void FillDashboard(DataTable DtTimetracking, List<TimeTracking> TimeTrackingListnew, DataTable DataTableForGridLayout, List<TimeTrackingProductionStage> TimeTrackingProductionStage, StageByOTItemAndIDDrawing TimetrackingStagesList, List<ERM_CADCAMTimePerDesignType> CADCAMDesignTypeList)   //rajashri GEOS2-5988[22-08-2024]

        {
            try
            {
                // if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                int rowCounter = 0;
                double? totalsum = null;
                double totalsumConvertedAmount = 0;
                int bandvalue = 3;
                List<string> tempCurrentstage = new List<string>();
                var currentculter = CultureInfo.CurrentCulture;
                string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                // DataTableForGridLayout.Clear();
                //DtTimetracking = null;
                TimeTrackingListnew = TimeTrackingListnew.OrderBy(a => a.DeliveryDate).ToList();

             //   List<TimeTrackingProductionStage> TimeTrackingProductionStage = TimetrackingStagesList.AllStagesList;
                List<int> OtItemStagesList = TimetrackingStagesList.OTITemStagesList.Select(i => i.IdStage).ToList();
                List<int> DrawingIdStagesList = TimetrackingStagesList.DrawingIdStagesList.Select(i => i.IdStage).ToList();

                List<ReworkData> OTsWithIDOTItemList = new List<ReworkData>();
                List<ReworkData> OTsWithIDDrawingList = new List<ReworkData>();

                for (int i = 0; i < TimeTrackingListnew.Count; i++)
                {
                    List<ERM_Timetracking_rework_ows>  ReworkOWSList = new List<ERM_Timetracking_rework_ows>();////[GEOS2-6620][gulab lakade][12 12 2024]
                    DataRow dr = DataTableForGridLayout.NewRow();
                    if (Convert.ToDateTime(TimeTrackingListnew[i].DeliveryDate) < DateTime.Now)
                    {
                        dr["DeliveryDateColor"] = true;
                    }
                    dr["DeliveryWeek"] = Convert.ToString(TimeTrackingListnew[i].DeliveryWeek);
                    dr["DeliveryDate"] = Convert.ToString(TimeTrackingListnew[i].DeliveryDate);
                    dr["DeliveryDateHtmlColor"] = Convert.ToString(TimeTrackingListnew[i].DeliveryDateHtmlColor);

                    if (TimeTrackingListnew[i].PlannedDeliveryDate != null) //[GEOS2-4217] [Pallavi Jadhav] [27 02 2023]
                    {
                        //PlannedDatecolor = true;
                        dr["PlannedDeliveryDate"] = Convert.ToDateTime(TimeTrackingListnew[i].PlannedDeliveryDate);
                        dr["PlannedDeliveryDateHtmlColor"] = Convert.ToString(TimeTrackingListnew[i].PlannedDeliveryDateHtmlColor);
                    }
                    #region [GEOS2-4093][Rupali Sarode][26-12-2022]
                    if (TimeTrackingListnew[i].QuoteSendDate != null)
                        dr["QuoteSendDate"] = Convert.ToString(TimeTrackingListnew[i].QuoteSendDate);

                    if (TimeTrackingListnew[i].GoAheadDate != null)
                        dr["GoAheadDate"] = Convert.ToString(TimeTrackingListnew[i].GoAheadDate);

                    if (TimeTrackingListnew[i].PODate != null)
                        dr["PODate"] = Convert.ToString(TimeTrackingListnew[i].PODate);

                    #region [GEOS2-4145][Pallavi Jadhav][02-03-2023]
                    if (TimeTrackingListnew[i].Samples != null && TimeTrackingListnew[i].Samples != "")
                    {
                        dr["SamplesTemplate"] = Convert.ToString(TimeTrackingListnew[i].Samples);
                        if (TimeTrackingListnew[i].SamplesDate != null)
                            dr["SamplesDateTemplate"] = Convert.ToString(TimeTrackingListnew[i].SamplesDate);
                        string samplesColor = Convert.ToString(TimeTrackingListnew[i].SamplesColor);
                        switch (samplesColor)
                        {
                            case "True":
                                dr["SamplesColor"] = Convert.ToString("#008000");
                                break;
                            case "False":
                                dr["SamplesColor"] = Convert.ToString("#FF0000");
                                break;
                            default:
                                break;
                        }

                    }



                    #endregion

                    if (TimeTrackingListnew[i].AvailbleForDesignDate != null)
                        dr["AvailbleForDesignDate"] = Convert.ToString(TimeTrackingListnew[i].AvailbleForDesignDate);

                    #endregion

                    #region [GEOS2-4173][Rupali Sarode][13-03-2023]
                    if (TimeTrackingListnew[i].DrawingType != null)
                    {
                        dr["DrawingType"] = Convert.ToString(TimeTrackingListnew[i].DrawingType);
                        string DrawingTypecolor = Convert.ToString(TimeTrackingListnew[i].DrawingType);
                        switch (DrawingTypecolor)
                        {
                            case "M":
                                dr["DrawingTypeForColor"] = Convert.ToString("#A020F0");
                                break;
                            case "U":
                                dr["DrawingTypeForColor"] = Convert.ToString("#00FF00");
                                break;
                            case "N":
                                dr["DrawingTypeForColor"] = Convert.ToString("#FF0000");
                                break;
                            case "C":
                                dr["DrawingTypeForColor"] = Convert.ToString("#0000FF");
                                break;
                            case "X":
                                dr["DrawingTypeForColor"] = Convert.ToString("#000000");
                                break;
                            default:
                                break;
                        }
                    }
                    if (TimeTrackingListnew[i].TrayName != null)
                        dr["TrayName"] = Convert.ToString(TimeTrackingListnew[i].TrayName);
                    if (TimeTrackingListnew[i].TrayColor != null)
                        dr["TrayColor"] = Convert.ToString(TimeTrackingListnew[i].TrayColor);

                    if (TimeTrackingListnew[i].FirstDeliveryDate != null)
                        dr["FirstDeliveryDate"] = Convert.ToString(TimeTrackingListnew[i].FirstDeliveryDate);

                    #endregion


                    if (TimeTrackingListnew[i].POType != null)
                        dr["POType"] = Convert.ToString(TimeTrackingListnew[i].POType);
                    if (TimeTrackingListnew[i].Customer != null)
                        dr["Customer"] = Convert.ToString(TimeTrackingListnew[i].Customer);
                    if (TimeTrackingListnew[i].Project != null)
                        dr["Project"] = Convert.ToString(TimeTrackingListnew[i].Project);
                    if (TimeTrackingListnew[i].Offer != null)
                        dr["Offer"] = Convert.ToString(TimeTrackingListnew[i].Offer);
                    if (TimeTrackingListnew[i].OTCode != null)
                        dr["OTCode"] = Convert.ToString(TimeTrackingListnew[i].OTCode);

                    dr["OriginPlant"] = Convert.ToString(TimeTrackingListnew[i].OriginalPlant);
                    if (TimeTrackingListnew[i].ProductionPlant != null)
                        dr["ProductionPlant"] = Convert.ToString(TimeTrackingListnew[i].ProductionPlant);
                    if (TimeTrackingListnew[i].Reference != null)
                        dr["Reference"] = Convert.ToString(TimeTrackingListnew[i].Reference);
                    if (TimeTrackingListnew[i].Template != null)
                        dr["ReferenceTemplate"] = Convert.ToString(TimeTrackingListnew[i].Template);
                    if (TimeTrackingListnew[i].Type != null)
                        dr["Type"] = Convert.ToString(TimeTrackingListnew[i].Type);
                    if (TimeTrackingListnew[i].QTY != null)
                        dr["QTY"] = TimeTrackingListnew[i].QTY;
                    //if (TimeTrackingList[i].QTY != null && TimeTrackingList[i].Unit != null)
                    //    TimeTrackingList[i].TotalSalePrice =(float?) Math.Round(Convert.ToDecimal(TimeTrackingList[i].QTY) * Convert.ToDecimal(TimeTrackingList[i].Unit),2);
                    //double tempTotalPrice = 

                    if (TimeTrackingListnew[i].Unit != null)
                        dr["UnitPrice"] = Convert.ToDecimal(Convert.ToDouble(TimeTrackingListnew[i].Unit).ToString("N2", CultureInfo.CurrentCulture));
                    
                    //[Rupali Sarode][04-04-2024][GEOS2-5577]
                    if (TimeTrackingListnew[i].IdDrawing > 0)
                        dr["IdDrawing"] = Convert.ToString(TimeTrackingListnew[i].IdDrawing);

                    //[Aishwarya Ingale][09-08-2024][GEOS2-6034]
                    if (TimeTrackingListnew[i].Workbookdrawing != null)
                        dr["workbook_drawing"] = Convert.ToString(TimeTrackingListnew[i].Workbookdrawing);

                    if (TimeTrackingListnew[i].SerialNumber != null)
                        dr["SerialNumber"] = Convert.ToString(TimeTrackingListnew[i].SerialNumber);
                    if (TimeTrackingListnew[i].ItemStatus != null)
                        dr["ItemStatus"] = Convert.ToString(TimeTrackingListnew[i].ItemStatus);
                    if (TimeTrackingListnew[i].HtmlColor != null)
                        dr["HtmlColor"] = Convert.ToString(TimeTrackingListnew[i].HtmlColor);
                    if (TimeTrackingListnew[i].NumItem != null)
                    {
                        dr["ItemNumber"] = Convert.ToString(TimeTrackingListnew[i].NumItem);
                    }

                    if (TimeTrackingListnew[i].CurrentWorkStation != null)
                    {
                        dr["CurrentWorkStation"] = Convert.ToString(TimeTrackingListnew[i].CurrentWorkStation);
                    }                    
                  

                    if (TimeTrackingListnew[i].TotalSalePrice != null)
                        dr["TotalPrice"] = Convert.ToDecimal(Convert.ToDouble(TimeTrackingListnew[i].TotalSalePrice).ToString("N2", CultureInfo.CurrentCulture));// Math.Round(Convert.ToDouble(TimeTrackingList[i].TotalSalePrice),2).ToString("N", CultureInfo.CurrentCulture);
                                                                                                                                                            //dr["Total"] = Convert.ToDouble(tempTotalPrice).ToString("N", CultureInfo.CurrentCulture);

                    //if (TimeTrackingListnew[i].TRework != null)
                    //    dr["TRework"] = TimeTrackingListnew[i].TRework;

                    #region [Rupali Sarode][GEOS2-5522][21-03-2024] -- Rework As per New algorithm



                    List<TimeTrackingCurrentStage> StageReworksList = new List<TimeTrackingCurrentStage>();
                    
                    dr["TRework"] = TimeTrackingListnew[i].TRework;

                    if (TimeTrackingListnew[i].IsBatch == false && TimeTrackingListnew[i].TRework > 0)
                    {
                        if (TimeTrackingListnew[i].IdOTItem != 0)
                        {
                            var TempIdOTItem = OTsWithIDOTItemList.Where(j => j.IdOT == TimeTrackingListnew[i].IdOt && j.IdOTItem == TimeTrackingListnew[i].IdOTItem).FirstOrDefault();

                            if (TempIdOTItem == null)
                            {
                                ReworkData TempReworkOTItem = new ReworkData();

                                TempReworkOTItem.IdOT = TimeTrackingListnew[i].IdOt;
                                TempReworkOTItem.IdOTItem = TimeTrackingListnew[i].IdOTItem;
                                TempReworkOTItem.IdCounterpart = TimeTrackingListnew[i].IdCounterpart;
                                OTsWithIDOTItemList.Add(TempReworkOTItem);
                                //FlagNewAlgorithmCOM = false;
                            }
                            else
                            {
                                //FlagNewAlgorithmCOM = true;

                                // Update rework for COM stage in Time tracking Lists to display in Time tracking grid 
                                if (TimeTrackingListnew[i].TimeTrackingStage != null)
                                {
                                    StageReworksList = TimeTrackingListnew[i].TimeTrackingStage.Where(j => OtItemStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                    if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                    {
                                        if (TimeTrackingListnew[i].TRework > 0)
                                        {
                                            TimeTrackingListnew[i].TRework = TimeTrackingListnew[i].TRework - (ulong?)StageReworksList.Count;
                                            dr["TRework"] = TimeTrackingListnew[i].TRework;
                                        }

                                    }
                                }
                            }
                        }

                        if (TimeTrackingListnew[i].IdDrawing != 0 && TimeTrackingListnew[i].IdWorkbookOfCpProducts != 0) //Aishwarya Ingale[Geos2-6034])
                        {
                            var TempIdDrawing = OTsWithIDDrawingList.Where(j => j.IdOT == TimeTrackingListnew[i].IdOt && j.IdDrawing == TimeTrackingListnew[i].IdDrawing && j.IdWorkbookOfCpProducts == TimeTrackingListnew[i].IdWorkbookOfCpProducts).FirstOrDefault();
                            if (TempIdDrawing == null)
                            {
                                ReworkData TempReworkDrawing = new ReworkData();

                                TempReworkDrawing.IdOT = TimeTrackingListnew[i].IdOt;
                                TempReworkDrawing.IdOTItem = TimeTrackingListnew[i].IdOTItem;
                                TempReworkDrawing.IdCounterpart = TimeTrackingListnew[i].IdCounterpart;
                                TempReworkDrawing.IdDrawing = TimeTrackingListnew[i].IdDrawing;
                                TempReworkDrawing.IdWorkbookOfCpProducts = TimeTrackingListnew[i].IdWorkbookOfCpProducts; //Aishwarya Ingale[Geos2-6034]

                                OTsWithIDDrawingList.Add(TempReworkDrawing);
                            }
                            else
                            {
                                // Update rework for CAD & CAM stage in ProductionOutPutReport & AllPlantWeeklyReworksMail Lists to display in mail 

                                StageReworksList = new List<TimeTrackingCurrentStage>();
                                if (TimeTrackingListnew[i].TimeTrackingStage != null)
                                {
                                    StageReworksList = TimeTrackingListnew[i].TimeTrackingStage.Where(j => DrawingIdStagesList.Contains(j.NewIdStage) && j.Rework == 1).ToList();

                                    if (StageReworksList.Count > 0)  // Update rework only if rework is related to specific stage.
                                    {
                                        TimeTrackingListnew[i].TRework = TimeTrackingListnew[i].TRework - (ulong?)StageReworksList.Count;
                                        dr["TRework"] = TimeTrackingListnew[i].TRework;
                                    }
                                }
                            }
                        }
                    }

                    #endregion [Rupali Sarode][GEOS2-5522][21-03-2024] 

                    TimeSpan TempTotalReal = TimeSpan.Parse("0");
                    TimeSpan TempTotalExpected = TimeSpan.Parse("0");
                    TimeSpan TempTotalRemaianing = TimeSpan.Parse("0");
                    if (TimeTrackingListnew[i].TimeTrackingStage != null)
                    {

                        #region [gulab lakade][11 03 2024][GEOS2-5466]
                        //if (dr["SerialNumber"].ToString() == "R210008H006001")
                        //{

                        //}

                        try
                        {
                           
                            List<TimeTrackingCurrentStage> TempTimeTrackingByStageList = new List<TimeTrackingCurrentStage>();

                            var temprecord = TimeTrackingListnew[i].TimeTrackingStage.OrderBy(a => a.IdCounterparttracking).ToList();


                            var reorderedResult1 = temprecord
                                .OrderBy(r => r.Startdate)
                                .ThenBy(r => r.Enddate)
                                .ToList();
                            #region [GEOS2-6620][gulab lakade][05 12 2024]
                            int? checkIdstage_first = 0;
                            string Production = string.Empty;
                            //string Rework = string.Empty;

                            string Rework_First = string.Empty;
                            string Rework_Second = string.Empty;
                            string Rework_Third = string.Empty;
                            string Rework_Fourth = string.Empty;

                            string POWS_First = string.Empty;
                            string POWS_Second = string.Empty;
                            string POWS_Third = string.Empty;
                            string POWS_Fourth = string.Empty;


                            string ROWS_First = string.Empty;
                            string ROWS_Second = string.Empty;
                            string ROWS_Third = string.Empty;
                            string ROWS_Fourth = string.Empty;


                            TimeSpan ProductionTime = new TimeSpan();
                            TimeSpan ReworkTime = new TimeSpan();
                            double ReworkTimeIndouble = 0;
                            TimeSpan TotalProductiontime = new TimeSpan();
                            TimeSpan TotalReworktime = new TimeSpan();


                            int? DetectedIdStage_first = 0;

                            string Rework = string.Empty;
                            ////rajashri start GEOS2-6054
                            //double ProductionOWStime = 0;
                            string POWS = string.Empty;
                            string ROWS = string.Empty;
                            TimeSpan TotalProductionOwstime = new TimeSpan();
                            TimeSpan TotalReworkOwstime = new TimeSpan();
                            TimeSpan OWSprodandReworkTime = new TimeSpan();
                            //TimeSpan tempstoreOWSvalues = new TimeSpan();
                            #endregion

                            List<TimeTrackingCurrentStage> ReworkTimeStageList = new List<TimeTrackingCurrentStage>();

                            //end

                            #region [pallavi jadhav][GEOS2-7066][10-04-2025]
                            TimeSpan TempAddInPostServer = TimeSpan.Parse("0");
                            TimeSpan AddintotalTimeDifference = TimeSpan.Zero;
                            TimeSpan PostservertotalTimeDifference = TimeSpan.Zero;
                            #endregion

                            foreach (var item in reorderedResult1)
                            {
                                #region [GEOS2-6620][gulab lakade][12 12 2024]
                                if (ReworkOWSList == null)
                                {
                                    ReworkOWSList = new List<ERM_Timetracking_rework_ows>();
                                }
                                #endregion

                                bool FlagPresentInActivePlants = false;
                                var Stagerecord = TimeTrackingProductionStage.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                                if (Stagerecord != null)
                                {
                                    //TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                    #region [Rupali Sarode][GEOS2-4347][05-05-2023] -- Show stage only if present in ActiveInPlants

                                    string[] ArrActiveInPlants;

                                    if (Stagerecord.ActiveInPlants != null && Stagerecord.ActiveInPlants != "")
                                    {
                                        ArrActiveInPlants = Stagerecord.ActiveInPlants.Split(',');
                                        List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                                        FlagPresentInActivePlants = false;
                                        foreach (Site itemSelectedPlant in tmpSelectedPlant)
                                        {
                                            if (FlagPresentInActivePlants == true)
                                            {
                                                break;
                                            }

                                            foreach (var iPlant in ArrActiveInPlants)
                                            {
                                                if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                                                {
                                                    FlagPresentInActivePlants = true;
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                    #endregion
                                    if (FlagPresentInActivePlants == true || Stagerecord.ActiveInPlants == null || Stagerecord.ActiveInPlants == "")
                                    {
                                        if (TempTimeTrackingByStageList.Count() > 0)
                                        {
                                            var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                                            decimal? RealTime = 0;
                                            decimal? ExpectedTime = 0;
                                            if (Real != null)
                                            {
                                                if (Real.Real != null)
                                                {
                                                    RealTime = Real.Real;
                                                }
                                                if (Real.Expected == null)
                                                {
                                                    if (item.Expected == null || item.Expected == 0)
                                                    {
                                                        TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                                                    }
                                                    else
                                                    {
                                                        TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = item.Expected);
                                                    }

                                                }
                                                else
                                                {
                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = Real.Expected);
                                                }

                                            }
                                            else
                                            {
                                                TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                                //var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage && X.Expected!=null).FirstOrDefault();

                                                if (item.Expected != null)
                                                {
                                                    TempTimeTrackingByStage.Expected = item.Expected;
                                                }
                                                else
                                                {
                                                    TempTimeTrackingByStage.Expected = 0;
                                                }
                                                TempTimeTrackingByStage.IdStage = item.IdStage;
                                                TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                                                TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                                                TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                                                TempTimeTrackingByStage.Days = item.Days;

                                                TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                                            }
                                        }
                                        else
                                        {
                                            TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                                            // var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage).FirstOrDefault();

                                            if (item.Expected != null)
                                            {
                                                TempTimeTrackingByStage.Expected = item.Expected;
                                            }
                                            else
                                            {
                                                TempTimeTrackingByStage.Expected = 0;
                                            }
                                            TempTimeTrackingByStage.IdStage = item.IdStage;
                                            TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                                            TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                                            TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                                            TempTimeTrackingByStage.Days = item.Days;
                                            TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);

                                        }
                                        #region First Rework is true

                                        //if (reworkflag_First == true)
                                        if (ReworkOWSList.Count() > 0)
                                        {
                                            #region check reworkflag_Second 
                                            //if (reworkflag_Second == false)
                                            if (ReworkOWSList.Count() == 1)
                                            {
                                                var tempR_P_OWS = ReworkOWSList.Where(a => a.OWS_ID == 1).FirstOrDefault();
                                                if (tempR_P_OWS != null)
                                                {
                                                    checkIdstage_first = tempR_P_OWS.CheckStageId;
                                                    DetectedIdStage_first = tempR_P_OWS.DetectedStageId;
                                                }
                                                #region reworkflag_First data check and fill only
                                                #region checkIdstage_first == item.IdStage
                                                if (checkIdstage_first == item.IdStage)
                                                {
                                                    try
                                                    {
                                                        #region reworkflag_First completed
                                                        string production_first = "Production_" + checkIdstage_first;
                                                        if (!DataTableForGridLayout.Columns.Contains(Production))
                                                        {
                                                            dr[production_first] = TimeSpan.Zero;
                                                        }
                                                        if (DataTableForGridLayout.Columns.Contains(production_first))
                                                        {
                                                            if (Convert.ToString(dr[production_first]) != null)
                                                            {
                                                                string reworkowsInstring = Convert.ToString(dr["Production_" + checkIdstage_first]);
                                                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                {
                                                                    reworkowsInstring = "0.00:00:00";
                                                                }
                                                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                TimeSpan TempproductionTime = TimeSpan.ParseExact(timeString, format, null);

                                                                if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                {

                                                                    #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                                    //dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                    //TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    if (item.IdStage == 2 && TimeTrackingListnew[i].DesignSystem == "EDS")
                                                                    {
                                                                        var tempAddingPosServer = TimeTrackingListnew[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            TimeSpan tmptimespanvalue = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                            }
                                                                            dr[production_first] = TempproductionTime + tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dr[production_first] = (TempproductionTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));
                                                                        TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }
                                                                    #endregion 
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (DataTableForGridLayout.Columns.Contains(production_first))
                                                                {


                                                                    #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                                    //dr[production_first] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    //TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    if (item.IdStage == 2 && TimeTrackingListnew[i].DesignSystem == "EDS")
                                                                    {
                                                                        var tempAddingPosServer = TimeTrackingListnew[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229)).ToList();

                                                                        if (tempAddingPosServer.Count() > 0)
                                                                        {
                                                                            TimeSpan tmptimespanvalue = new TimeSpan();
                                                                            foreach (var ttime in tempAddingPosServer)
                                                                            {
                                                                                tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                            }
                                                                            dr[production_first] =  tmptimespanvalue;
                                                                            TotalProductiontime = TotalProductiontime + tmptimespanvalue;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dr[production_first] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        TotalProductiontime = TotalProductiontime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }
                                                                    #endregion 
                                                                }

                                                            }
                                                        }
                                                        // reworkflag_First = false;
                                                        checkIdstage_first = 0;
                                                        ReworkTimeIndouble = 0;
                                                        DetectedIdStage_first = 0;
                                                        //ProductionOWStime_First = 0;//GEOS2-6054
                                                        // reworkflag_First = false;
                                                        // checkIdstage_first = 0;
                                                        ReworkOWSList.RemoveAll(a => a.OWS_ID == 1);
                                                        #endregion


                                                        #region reworkflag_First again true
                                                        // gulab lakade 19 04 2024
                                                        if (item.Rework == 1)
                                                        {
                                                            ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                            selected_rework_ows.OWS_ID = 1;
                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                            //reworkflag_First = true;
                                                            checkIdstage_first = item.IdStage;
                                                            var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                            int lastindex = reorderedResult1.Count();
                                                            if (lastindex - 1 > DetectedStageIndex)
                                                            {
                                                                var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                if (Detectedrecord != null)
                                                                {
                                                                    DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage
                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                }
                                                                TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                ReworkTimeStageList.Add(TempReworkTime);

                                                            }

                                                            if (checkIdstage_first == DetectedIdStage_first)
                                                            {
                                                                // reworkflag_First = false;
                                                                checkIdstage_first = 0;
                                                                DetectedIdStage_first = 0;
                                                            }
                                                            else
                                                            {
                                                                ReworkOWSList.Add(selected_rework_ows);
                                                            }

                                                        }
                                                        #endregion
                                                        //end gulab lakade 19 04 2024
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {


                                                        #region only Add reworkflag_First data not other  
                                                        #region  if (item.Rework == 1) DetectedIdStage_Second
                                                        #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                        Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                        ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                        selected_rework_ows.OWS_ID = Max_owsID + 1;
                                                        bool sameCheck_Detect = false;
                                                        if (item.Rework == 1)
                                                        {
                                                            var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                            if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                            {
                                                                var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                if (Detectedrecord != null)
                                                                {
                                                                    //DetectedIdStage_Second = Detectedrecord.IdStage;
                                                                    selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                }
                                                            }
                                                            selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);

                                                            // checkIdstage_Second = item.IdStage;
                                                            if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                            {
                                                                sameCheck_Detect = true;
                                                                //checkIdstage_Second = 0;
                                                                //DetectedIdStage_Second = 0;
                                                                //reworkflag_Second = false;
                                                            }
                                                            else
                                                            {
                                                                ReworkOWSList.Add(selected_rework_ows);

                                                            }

                                                        }

                                                        #endregion

                                                        if (item.Rework == 1 && sameCheck_Detect == false)
                                                        {
                                                            //var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                            //if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                            //{
                                                            //    var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                            //    if (Detectedrecord != null)
                                                            //    {
                                                            //        DetectedIdStage_Second = Detectedrecord.IdStage;
                                                            //    }
                                                            //   // reworkflag_Second = true;
                                                            //    checkIdstage_Second = item.IdStage;
                                                            Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                            Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                            var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                            var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                            #region Rework==1 and DetectedIdStage_first == item.IdStage
                                                            //if (DetectedIdStage_first == item.IdStage)
                                                            if (R_P_OWSMax_minus.DetectedStageId == item.IdStage)
                                                            {

                                                                string Rework_second = "Rework_" + item.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }
                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }

                                                            }
                                                            #endregion
                                                            else
                                                            {
                                                                #region Rework OWS
                                                                string ROWS_second = "ROWS_" + R_P_OWSMax_minus.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[ROWS_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }
                                                                #endregion
                                                                #region Production OWS
                                                                string POWS_second = "POWS_" + item.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                {
                                                                    dr[POWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[POWS_second]) != null)
                                                                {
                                                                    string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                    {
                                                                        P_owsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }

                                                                #endregion
                                                            }


                                                            // }

                                                        }
                                                        #endregion
                                                        else
                                                        {


                                                            #region if (DetectedIdStage_first != item.IdStage)
                                                            Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                            //Int32 Max_owsID_Add_minus = Max_owsID_Add - 1;
                                                            var R_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                            //var R_P_OWSMax_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add_minus).FirstOrDefault();

                                                            //if (DetectedIdStage_first == item.IdStage)
                                                            if (R_P_OWSMax.DetectedStageId == item.IdStage)
                                                            {

                                                                string Rework_second = "Rework_" + item.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }
                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }

                                                            }
                                                            else
                                                            {
                                                                #region  rework ows and production ows


                                                                POWS_First = "POWS_" + item.IdStage;
                                                                ROWS_First = "ROWS_" + R_P_OWSMax.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                {
                                                                    dr[POWS_First] = TimeSpan.Zero;
                                                                }
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                {
                                                                    dr[ROWS_First] = TimeSpan.Zero;
                                                                }
                                                                if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                {
                                                                    if (Convert.ToString(dr[ROWS_First]) != null)
                                                                    {
                                                                        string ROWSInstring = Convert.ToString(dr[ROWS_First]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                        {
                                                                            ROWSInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                        TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        //tempstoreOWSvalues = timespan;
                                                                        // TotalProductionOwstime += timespan;
                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                        {
                                                                            dr[ROWS_First] = timespan;

                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_First))
                                                                        {
                                                                            dr[ROWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        }

                                                                    }
                                                                }
                                                                TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                {
                                                                    if (Convert.ToString(dr[POWS_First]) != null)
                                                                    {
                                                                        string POWInstring = Convert.ToString(dr[POWS_First]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                        {
                                                                            POWInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                        TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                        TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                        {
                                                                            dr[POWS_First] = timespan;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_First))
                                                                        {
                                                                            dr[POWS_First] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        }

                                                                    }
                                                                }

                                                                #endregion
                                                            }

                                                            #endregion
                                                        }
                                                        #endregion
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                                #endregion
                                                #endregion
                                            }
                                            else
                                            {
                                                try
                                                {



                                                    #region check reworkflag_Third 
                                                    //if (reworkflag_Third == false)
                                                    if (ReworkOWSList.Count() > 1)
                                                    {
                                                        Int32 Max_owsID = ReworkOWSList.Max(a => a.OWS_ID);
                                                        Int32 Max_owsID_minus = Max_owsID - 1;
                                                        var tempR_P_OWSMax = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID).FirstOrDefault();
                                                        var tempR_P_OWSMax_minusone = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_minus).FirstOrDefault();
                                                        #region reworkflag_Second data check and fill only
                                                        #region checkIdstage_Second  == item.IdStage
                                                        //if (checkIdstage_Second == item.IdStage)
                                                        if (tempR_P_OWSMax.CheckStageId == item.IdStage)
                                                        {
                                                            #region   if(checkIdstage_Second==DetectedIdStage_first)
                                                            //if (checkIdstage_Second == DetectedIdStage_first)
                                                            if (tempR_P_OWSMax.CheckStageId == tempR_P_OWSMax_minusone.DetectedStageId)
                                                            {
                                                                string Rework_second = "Rework_" + item.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                {
                                                                    dr[Rework_second] = TimeSpan.Zero;
                                                                }


                                                                if (Convert.ToString(dr[Rework_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[Rework_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(Rework_second))
                                                                    {
                                                                        dr[Rework_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[Rework_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }
                                                            }
                                                            #endregion
                                                            else
                                                            {
                                                                #region Rework OWS

                                                                //string ROWS_second = "ROWS_" + DetectedIdStage_first;
                                                                string ROWS_second = "ROWS_" + tempR_P_OWSMax_minusone.DetectedStageId;
                                                                if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[ROWS_second]) != null)
                                                                {
                                                                    string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                    {
                                                                        reworkowsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }
                                                                #endregion
                                                                #region Production OWS
                                                                string POWS_second = "POWS_" + item.IdStage;
                                                                if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                {
                                                                    dr[POWS_second] = TimeSpan.Zero;
                                                                }

                                                                if (Convert.ToString(dr[POWS_second]) != null)
                                                                {
                                                                    string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                    if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                    {
                                                                        P_owsInstring = "00:00:00";
                                                                    }
                                                                    string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                    TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                }

                                                                #endregion

                                                            }
                                                            #region reworkflag_Second completed
                                                            ReworkOWSList.RemoveAll(a => a.OWS_ID == Max_owsID);

                                                            //ReworkTimeIndouble = 0;
                                                            //DetectedIdStage_Second = 0;
                                                            //ProductionOWStime_Second = 0;//GEOS2-6054
                                                            //reworkflag_Second = false;
                                                            //checkIdstage_Second = 0;
                                                            #endregion


                                                            #region reworkflag_Second again true
                                                            // gulab lakade 19 04 2024
                                                            if (item.Rework == 1)
                                                            {
                                                                Int32 Max_owsID_Add = ReworkOWSList.Max(a => a.OWS_ID);
                                                                var tempR_P_OWSMax_add = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_Add).FirstOrDefault();
                                                                ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                selected_rework_ows.OWS_ID = Max_owsID_Add + 1;
                                                                selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                //reworkflag_Second = true;
                                                                //checkIdstage_Second = item.IdStage;
                                                                var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                int lastindex = reorderedResult1.Count();
                                                                if (lastindex - 1 > DetectedStageIndex)
                                                                {
                                                                    var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                    if (Detectedrecord != null)
                                                                    {
                                                                        //DetectedIdStage_Second = Detectedrecord.IdStage;// detected rework stage
                                                                        selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                    }
                                                                    TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                                                                    TempReworkTime.IdStage = Detectedrecord.IdStage;
                                                                    TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                                                                    TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                                                                    ReworkTimeStageList.Add(TempReworkTime);
                                                                }
                                                                ReworkOWSList.Add(selected_rework_ows);


                                                            }
                                                            #endregion
                                                            //end gulab lakade 19 04 2024
                                                        }
                                                        else
                                                        {
                                                            #region only Add reworkflag_Second data not other  
                                                            #region  if (item.Rework == 1) DetectedIdStage_Third
                                                            #region if(checkIdstage_Second==DetectedIdStage_Second)
                                                            Int32 Max_owsID_1 = ReworkOWSList.Max(a => a.OWS_ID);
                                                            var tempR_P_OWSMax_1 = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1).FirstOrDefault();
                                                            Int32 Max_owsID_1_minus = Max_owsID_1 - 1;
                                                            var tempR_P_OWSMax_1_minus = ReworkOWSList.Where(a => a.OWS_ID == Max_owsID_1_minus).FirstOrDefault();
                                                            bool sameCheck_Detect = false;
                                                            if (item.Rework == 1)
                                                            {
                                                                ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                                selected_rework_ows.OWS_ID = Max_owsID_1 + 1;
                                                                var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                                {
                                                                    var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                    if (Detectedrecord != null)
                                                                    {
                                                                        //DetectedIdStage_Third = Detectedrecord.IdStage;
                                                                        selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);
                                                                    }
                                                                }
                                                                //checkIdstage_Third = item.IdStage;
                                                                selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                                //if (checkIdstage_Third == DetectedIdStage_Third )
                                                                if (selected_rework_ows.CheckStageId == selected_rework_ows.DetectedStageId)
                                                                {
                                                                    sameCheck_Detect = true;
                                                                    //checkIdstage_Third = 0;
                                                                    //DetectedIdStage_Third = 0;
                                                                    //reworkflag_Third = false;
                                                                }
                                                                else
                                                                {
                                                                    ReworkOWSList.Add(selected_rework_ows);
                                                                }

                                                            }

                                                            #endregion

                                                            if (item.Rework == 1 && sameCheck_Detect == false)
                                                            {
                                                                //var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                                //if ((reorderedResult1.Count() - 1) > DetectedStageIndex)
                                                                //{
                                                                //var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                                //if (Detectedrecord != null)
                                                                //{
                                                                //    DetectedIdStage_Third = Detectedrecord.IdStage;
                                                                //}

                                                                //reworkflag_Third = true;
                                                                //checkIdstage_Third = item.IdStage;
                                                                #region item.Rework == 1 and  if (DetectedIdStage_Second != item.IdStage)
                                                                //if (DetectedIdStage_Second == item.IdStage)
                                                                if (tempR_P_OWSMax_1_minus.DetectedStageId == item.IdStage)
                                                                {

                                                                    string Rework_third = "Rework_" + item.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.Zero;
                                                                    }
                                                                    if (Convert.ToString(dr[Rework_third]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                        {
                                                                            dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                            TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    }

                                                                }
                                                                #endregion
                                                                else
                                                                {
                                                                    #region Rework OWS
                                                                    //string ROWS_second = "ROWS_" + DetectedIdStage_Second;
                                                                    string ROWS_second = "ROWS_" + tempR_P_OWSMax_1_minus.DetectedStageId;
                                                                    if (!DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                    {
                                                                        dr[ROWS_second] = TimeSpan.Zero;
                                                                    }

                                                                    if (Convert.ToString(dr[ROWS_second]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[ROWS_second]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(ROWS_second))
                                                                        {
                                                                            dr[ROWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                            TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[ROWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    }
                                                                    #endregion
                                                                    #region Production OWS
                                                                    string POWS_second = "POWS_" + item.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                    {
                                                                        dr[POWS_second] = TimeSpan.Zero;
                                                                    }

                                                                    if (Convert.ToString(dr[POWS_second]) != null)
                                                                    {
                                                                        string P_owsInstring = Convert.ToString(dr[POWS_second]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(P_owsInstring)) || P_owsInstring == "0" || P_owsInstring == "00:00:00")
                                                                        {
                                                                            P_owsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(P_owsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(POWS_second))
                                                                        {
                                                                            dr[POWS_second] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                            TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[POWS_second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    }

                                                                    #endregion
                                                                }



                                                                //}

                                                            }
                                                            #endregion
                                                            else
                                                            {

                                                                #region if (DetectedIdStage_Second != item.IdStage)
                                                                //if (DetectedIdStage_Second == item.IdStage)
                                                                if (tempR_P_OWSMax_1.DetectedStageId == item.IdStage)
                                                                {

                                                                    string Rework_third = "Rework_" + item.IdStage;
                                                                    if (!DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.Zero;
                                                                    }
                                                                    if (Convert.ToString(dr[Rework_third]) != null)
                                                                    {
                                                                        string reworkowsInstring = Convert.ToString(dr[Rework_third]);
                                                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                                                                        {
                                                                            reworkowsInstring = "00:00:00";
                                                                        }
                                                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };// Specify the format of the time string
                                                                        TimeSpan TempReworkTime = TimeSpan.ParseExact(timeString, format, null);

                                                                        if (DataTableForGridLayout.Columns.Contains(Rework_third))
                                                                        {
                                                                            dr[Rework_third] = (TempReworkTime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference)));

                                                                            TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr[Rework_third] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                        TotalReworktime = TotalReworktime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    #region  rework ows and production ows


                                                                    POWS_Second = "POWS_" + item.IdStage;
                                                                    //ROWS_Second = "ROWS_" + DetectedIdStage_Second;
                                                                    ROWS_Second = "ROWS_" + tempR_P_OWSMax_1.DetectedStageId;
                                                                    if (!DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                    {
                                                                        dr[POWS_Second] = TimeSpan.Zero;
                                                                    }
                                                                    if (!DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                    {
                                                                        dr[ROWS_Second] = TimeSpan.Zero;
                                                                    }
                                                                    if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                    {
                                                                        if (Convert.ToString(dr[ROWS_Second]) != null)
                                                                        {
                                                                            string ROWSInstring = Convert.ToString(dr[ROWS_Second]);
                                                                            if (string.IsNullOrEmpty(Convert.ToString(ROWSInstring)) || ROWSInstring == "0" || ROWSInstring == "00:00:00")
                                                                            {
                                                                                ROWSInstring = "00:00:00";
                                                                            }
                                                                            string timeString = Convert.ToString(ROWSInstring); // Example time format string
                                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                            TimeSpan tempROWSInstring = TimeSpan.ParseExact(timeString, format, null);
                                                                            TimeSpan timespan = tempROWSInstring + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                            //tempstoreOWSvalues = timespan;
                                                                            // TotalProductionOwstime += timespan;
                                                                            if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                            {
                                                                                dr[ROWS_Second] = timespan;

                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (DataTableForGridLayout.Columns.Contains(ROWS_Second))
                                                                            {
                                                                                dr[ROWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            }

                                                                        }
                                                                    }
                                                                    TotalReworkOwstime = TotalReworkOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    TotalProductionOwstime = TotalProductionOwstime + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                    if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                    {
                                                                        if (Convert.ToString(dr[POWS_Second]) != null)
                                                                        {
                                                                            string POWInstring = Convert.ToString(dr[POWS_Second]);
                                                                            if (string.IsNullOrEmpty(Convert.ToString(POWInstring)) || POWInstring == "0" || POWInstring == "00:00:00")
                                                                            {
                                                                                POWInstring = "00:00:00";
                                                                            }
                                                                            string timeString = Convert.ToString(POWInstring); // Example time format string
                                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                                            TimeSpan POWInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                                            TimeSpan timespan = POWInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                                                                            if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                            {
                                                                                dr[POWS_Second] = timespan;
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (DataTableForGridLayout.Columns.Contains(POWS_Second))
                                                                            {
                                                                                dr[POWS_Second] = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                                            }

                                                                        }
                                                                    }

                                                                    #endregion
                                                                }

                                                                #endregion
                                                            }
                                                            #endregion
                                                        }

                                                        #endregion
                                                        #endregion
                                                    }

                                                    #endregion
                                                }
                                                catch (Exception ex)
                                                {
                                                    GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        else
                                        {
                                            try
                                            {


                                                #region if (item.Rework == 1) Firstrework==true
                                                if (item.Rework == 1)
                                                {
                                                    ERM_Timetracking_rework_ows selected_rework_ows = new ERM_Timetracking_rework_ows();
                                                    selected_rework_ows.OWS_ID = 1;
                                                    var DetectedStageIndex = reorderedResult1.IndexOf(item);
                                                    int lastindex = reorderedResult1.Count();
                                                    if (lastindex - 1 > DetectedStageIndex)
                                                    {
                                                        try
                                                        {
                                                            var Detectedrecord = reorderedResult1[DetectedStageIndex + 1];
                                                            if (Detectedrecord != null)
                                                            {
                                                                // DetectedIdStage_first = Detectedrecord.IdStage;// detected rework stage

                                                                selected_rework_ows.DetectedStageId = Convert.ToInt32(Detectedrecord.IdStage);// detected rework stage

                                                            }

                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                        //end

                                                        // reworkflag_First = true;
                                                        // checkIdstage_first = item.IdStage;
                                                        selected_rework_ows.CheckStageId = Convert.ToInt32(item.IdStage);
                                                        ReworkOWSList.Add(selected_rework_ows);
                                                        Production = "Production_" + item.IdStage;
                                                        //Rework_First = "Rework_" + DetectedIdStage_first;
                                                        //POWS_First = "POWS_" + DetectedIdStage_first;
                                                        //ROWS_First = "ROWS_" + DetectedIdStage_first;
                                                        Rework_First = "Rework_" + selected_rework_ows.DetectedStageId;
                                                        POWS_First = "POWS_" + selected_rework_ows.DetectedStageId;
                                                        ROWS_First = "ROWS_" + selected_rework_ows.DetectedStageId;

                                                        #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                        // ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                        if (item.IdStage == 2 && TimeTrackingListnew[i].DesignSystem == "EDS")
                                                        {
                                                            var tempAddingPosServer = TimeTrackingListnew[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229)).ToList();

                                                            if (tempAddingPosServer.Count() > 0)
                                                            {
                                                                TimeSpan tmptimespanvalue = new TimeSpan();
                                                                foreach (var ttime in tempAddingPosServer)
                                                                {
                                                                    tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                                }
                                                                ProductionTime = tmptimespanvalue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                        }
                                                        #endregion
                                                        TotalProductiontime += ProductionTime;
                                                        //dr[Production] = ProductionTime;
                                                        if (DataTableForGridLayout.Columns.Contains(Production))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                        {
                                                            if (!string.IsNullOrEmpty(Convert.ToString(item.TimeDifference)) && Convert.ToString(item.TimeDifference) != "0")
                                                            {

                                                                if (AppSettingData.Count > 0)
                                                                {
                                                                    if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                                                    {
                                                                        if (TimeTrackingListnew[i].QTY == 1)
                                                                        {
                                                                            if (TimeTrackingListnew[i].SerialNumber.EndsWith("1"))
                                                                            {
                                                                                if (DataTableForGridLayout.Columns.Contains(Production))
                                                                                {
                                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                                    {
                                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                                        dr[Production] = ProductionTime;
                                                                                    }
                                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                ProductionTime = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                                                                if (DataTableForGridLayout.Columns.Contains(Production))
                                                                                {
                                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                                    {
                                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                                        dr[Production] = ProductionTime;
                                                                                    }
                                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = true;
                                                                                }
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework_First] == DBNull.Value)
                                                                                    dr[Rework_First] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                        {
                                                                            if (dr[Rework_First] == DBNull.Value)
                                                                                dr[Rework_First] = TimeSpan.Zero;
                                                                            dr[Production] = ProductionTime;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework_First] == DBNull.Value)
                                                                            dr[Rework_First] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                {
                                                                    if (dr[Rework_First] == DBNull.Value)
                                                                        dr[Rework_First] = TimeSpan.Zero;
                                                                    dr[Production] = ProductionTime;
                                                                }
                                                            }
                                                            //decimal? tempProduction = Convert.ToDecimal(item.TimeDifference);
                                                            //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                                                        }

                                                    }
                                                }
                                                #endregion
                                                else
                                                {
                                                    Production = "Production_" + item.IdStage;
                                                    Rework = "Rework_" + item.IdStage;


                                                    #region[GEOS2-7880][gulab lakade][16 04 2025]
                                                    //ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                    if (item.IdStage == 2 && TimeTrackingListnew[i].DesignSystem == "EDS")
                                                    {
                                                        var tempAddingPosServer = TimeTrackingListnew[i].TimeTrackingAddingPostServer.Where(x => x.TimeTrackIdCounterparttracking == item.IdCounterparttracking && (x.ProductionActivityTimeType == 0 || x.ProductionActivityTimeType == 2228 || x.ProductionActivityTimeType == 2229)).ToList();

                                                        if (tempAddingPosServer.Count() > 0)
                                                        {
                                                            TimeSpan tmptimespanvalue = new TimeSpan();
                                                            foreach (var ttime in tempAddingPosServer)
                                                            {
                                                                tmptimespanvalue = tmptimespanvalue + ttime.TimeTrackDifference;
                                                            }
                                                            ProductionTime = tmptimespanvalue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                                                    }
                                                    #endregion
                                                    ReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                    POWS = "POWS_" + item.IdStage;
                                                    ROWS = "ROWS_" + item.IdStage;
                                                    OWSprodandReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                                                    TotalProductiontime += ProductionTime;
                                                    #region old production time get
                                                    string oldproductiontime = Convert.ToString(dr[Production]);

                                                    if (string.IsNullOrEmpty(Convert.ToString(oldproductiontime)) || oldproductiontime == "0" || oldproductiontime == "00:00:00")
                                                    {
                                                        oldproductiontime = "0.00:00:00";
                                                    }
                                                    string oldprod_string = Convert.ToString(oldproductiontime); // Example time format string
                                                    string[] format_Prod = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                    TimeSpan OldproductionTime = TimeSpan.ParseExact(oldprod_string, format_Prod, null);
                                                    #endregion
                                                    if (!string.IsNullOrEmpty(Convert.ToString(item.TimeDifference)) && Convert.ToString(item.TimeDifference) != "0")
                                                    {
                                                        if (AppSettingData.Count > 0)
                                                        {
                                                            if (AppSettingData.Contains(Convert.ToInt32(item.IdStage)))
                                                            {
                                                                if (TimeTrackingListnew[i].QTY == 1)
                                                                {
                                                                    if (TimeTrackingListnew[i].SerialNumber.EndsWith("1"))
                                                                    {
                                                                        if (DataTableForGridLayout.Columns.Contains(Production))
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework] == DBNull.Value)
                                                                                    dr[Rework] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                                dr[POWS] = OWSprodandReworkTime;
                                                                                dr[ROWS] = OWSprodandReworkTime;
                                                                            }
                                                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        ProductionTime = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                                                        if (DataTableForGridLayout.Columns.Contains(Production))
                                                                        {
                                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                            {
                                                                                if (dr[Rework] == DBNull.Value)
                                                                                    dr[Rework] = TimeSpan.Zero;
                                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                                dr[POWS] = OWSprodandReworkTime;
                                                                                dr[ROWS] = OWSprodandReworkTime;
                                                                            }
                                                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = true;
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                    {
                                                                        if (dr[Rework] == DBNull.Value)
                                                                            dr[Rework] = TimeSpan.Zero;
                                                                        dr[Production] = ProductionTime + OldproductionTime;
                                                                        dr[POWS] = OWSprodandReworkTime;
                                                                        dr[ROWS] = OWSprodandReworkTime;
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                                {
                                                                    if (dr[Rework] == DBNull.Value)
                                                                        dr[Rework] = TimeSpan.Zero;
                                                                    dr[Production] = ProductionTime + OldproductionTime;
                                                                    dr[POWS] = OWSprodandReworkTime;
                                                                    dr[ROWS] = OWSprodandReworkTime;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                            {
                                                                if (dr[Rework] == DBNull.Value)
                                                                    dr[Rework] = TimeSpan.Zero;
                                                                dr[Production] = ProductionTime + OldproductionTime;
                                                                dr[POWS] = OWSprodandReworkTime;
                                                                dr[ROWS] = OWSprodandReworkTime;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                                                        {
                                                            if (dr[Rework] == DBNull.Value)
                                                                dr[Rework] = TimeSpan.Zero;
                                                            dr[Production] = ProductionTime + OldproductionTime;
                                                            dr[POWS] = OWSprodandReworkTime;
                                                            dr[ROWS] = OWSprodandReworkTime;
                                                        }
                                                    }
                                                    decimal? tempProduction = Convert.ToDecimal(item.TimeDifference);
                                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                                                    TotalReworktime += ReworkTime;
                                                    TotalProductionOwstime += OWSprodandReworkTime;
                                                    if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                    {
                                                        #region Aishwarya Ingale[Geos2-6069]
                                                        if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = ReworkTime;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = DBNull.Value;
                                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = ReworkTime;
                                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                        }
                                                        else if (ProductionTime == TimeSpan.Zero)
                                                        {
                                                            dr[Rework] = DBNull.Value;
                                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                        }

                                                        #endregion


                                                        if (Convert.ToString(dr[Rework]) != null)
                                                        {
                                                            string ReworkInstring = Convert.ToString(dr[Rework]);
                                                            if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)))
                                                            {
                                                                ReworkInstring = "00:00:00";
                                                            }
                                                            string timeString = Convert.ToString(ReworkInstring); // Example time format string
                                                            string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string
                                                            TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                            TimeSpan timespan = TempReworkInTimespan + ReworkTime;

                                                            if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                            {
                                                                #region Aishwarya Ingale[Geos2-6069]
                                                                if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = timespan;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = timespan;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }

                                                                #endregion
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                            {
                                                                #region Aishwarya Ingale[Geos2-6069]
                                                                if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = ReworkTime;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = ReworkTime;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }
                                                                else if (ProductionTime == TimeSpan.Zero)
                                                                {
                                                                    dr[Rework] = DBNull.Value;
                                                                    TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                                                                }

                                                                #endregion
                                                            }

                                                        }
                                                    }
                                                    //rajashri
                                                    if (DataTableForGridLayout.Columns.Contains(POWS))
                                                    {
                                                        string productionowsInstring = Convert.ToString(dr[POWS]);
                                                        if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)) || productionowsInstring == "0" || productionowsInstring == "00:00:00")
                                                        {
                                                            productionowsInstring = "00:00:00";
                                                        }
                                                        string timeString = Convert.ToString(productionowsInstring); // Example time format string
                                                        string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }; // Specify the format of the time string

                                                        TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                                                        TimeSpan timespan = OWSprodandReworkTime;
                                                        if (DataTableForGridLayout.Columns.Contains(POWS))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                                        {
                                                            dr[POWS] = timespan;
                                                            dr[ROWS] = timespan;

                                                        }
                                                    }
                                                    //end


                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDashboard() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //if (item.Rework == 1)
                                        //{
                                        //    reworkflag_First = true;
                                        //    checkIdstage_first = item.IdStage;
                                        //}
                                    }
                                }
                                else
                                {
                                    //if (item.Rework == 1)
                                    //{
                                    //    reworkflag_First = true;
                                    //    checkIdstage_first = item.IdStage;
                                    //}
                                }

                                #region  [pallavi jadhav][GEOS2-7066][10-04-2025]  
                                //if (item.IdStage == 2)
                                //{
                                //    string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };
                                //    if (Convert.ToString(TimeTrackingListnew[i].DesignSystem) == "EDS")
                                //    {
                                //        TimeSpan totalAddINPostServer = TimeSpan.Zero;
                                //        if (item.ProductionActivityTimeType == 2228)
                                //        {
                                //            AddintotalTimeDifference = TimeSpan.ParseExact(Convert.ToString(item.TimeTrackDifference), format, null);
                                //            // dr[Production] = AddintotalTimeDifference + PostservertotalTimeDifference;
                                //            totalAddINPostServer = totalAddINPostServer + AddintotalTimeDifference;
                                //        }
                                //        if (item.ProductionActivityTimeType == 2229)
                                //        {
                                //            PostservertotalTimeDifference = TimeSpan.ParseExact(Convert.ToString(item.TimeTrackDifference), format, null);
                                //            totalAddINPostServer = totalAddINPostServer + PostservertotalTimeDifference;
                                //        }


                                //        // totalAddINPostServer = AddintotalTimeDifference + PostservertotalTimeDifference;
                                //        if (Production == Convert.ToString("Production_" + item.IdStage))
                                //        {
                                //            dr[Production] = AddintotalTimeDifference + PostservertotalTimeDifference;
                                //        }
                                //        TotalProductiontime = TotalProductiontime + totalAddINPostServer;
                                //    }

                                //}
                                #endregion

                            }
                            foreach (var stageItem in TempTimeTrackingByStageList)
                            {
                                string real = "Real_" + Convert.ToString(stageItem.IdStage);
                                double TempExpectedTime = 0;//gulab lakade  mismatch total
                                TimeSpan Tempreal = TimeSpan.Parse("0");

                                #region Real value
                                string[] format = { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" };  // Specify the format of the time string
                                string tempproduction = Convert.ToString(dr["Production_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempproduction)) || tempproduction == "0" || tempproduction == "00:00:00")
                                {
                                    tempproduction = "00:00:00";
                                }
                                string PRO_timeString = Convert.ToString(tempproduction); // Example time format string

                                TimeSpan Pro_InTimespan = TimeSpan.ParseExact(PRO_timeString, format, null);

                                string tempRework = Convert.ToString(dr["Rework_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempRework)) || tempRework == "0" || tempRework == "00:00:00")
                                {
                                    tempRework = "00:00:00";
                                }
                                string R_timeString = Convert.ToString(tempRework); // Example time format string

                                TimeSpan R_InTimespan = TimeSpan.ParseExact(R_timeString, format, null);
                                string tempP_OWS = Convert.ToString(dr["POWS_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempP_OWS)) || tempP_OWS == "0" || tempP_OWS == "00:00:00")
                                {
                                    tempP_OWS = "00:00:00";
                                }
                                string POWS_timeString = Convert.ToString(tempP_OWS); // Example time format string

                                TimeSpan POWS_InTimespan = TimeSpan.ParseExact(POWS_timeString, format, null);
                                Tempreal = Pro_InTimespan + R_InTimespan + POWS_InTimespan;
                                if (DataTableForGridLayout.Columns.Contains(real))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                                {
                                    #region[GEOS2 - 6742][gulab lakade][30 12 2024]
                                    //dr[real] = Tempreal;
                                    if (Tempreal == TimeSpan.Zero)
                                    {
                                        dr[real] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[real] = Tempreal;
                                    }

                                    #endregion
                                }
                                #endregion

                                #region [GEOS2-6742][gulab lakade][30 12 2024]
                               
                                string tempR_OWS = Convert.ToString(dr["ROWS_" + stageItem.IdStage]);
                                if (string.IsNullOrEmpty(Convert.ToString(tempR_OWS)) || tempR_OWS == "0" || tempR_OWS == "00:00:00")
                                {
                                    tempR_OWS = "00:00:00";
                                }
                                string ROWS_timeString = Convert.ToString(tempR_OWS); // Example time format string

                                TimeSpan ROWS_InTimespan = TimeSpan.ParseExact(ROWS_timeString, format, null);
                                if (!string.IsNullOrEmpty(Convert.ToString(dr["Production_" + stageItem.IdStage])))
                                {
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])))
                                    {
                                        dr["Rework_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])) && Convert.ToString(dr["Rework_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["Rework_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])))
                                    {
                                        dr["POWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                    if (!string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])) && Convert.ToString(dr["POWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["POWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])))
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                    else
                                         if (!string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])) && Convert.ToString(dr["POWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = TimeSpan.Zero;
                                    }
                                }
                                else
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Production_" + stageItem.IdStage])))
                                {
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])))
                                    {
                                        dr["Rework_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                     if (!string.IsNullOrEmpty(Convert.ToString(dr["Rework_" + stageItem.IdStage])) && Convert.ToString(dr["Rework_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["Rework_" + stageItem.IdStage] = DBNull.Value;
                                    }

                                    if (string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])))
                                    {
                                        dr["POWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                     if (!string.IsNullOrEmpty(Convert.ToString(dr["POWS_" + stageItem.IdStage])) && Convert.ToString(dr["POWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["POWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])))
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                    else
                                         if (!string.IsNullOrEmpty(Convert.ToString(dr["ROWS_" + stageItem.IdStage])) && Convert.ToString(dr["ROWS_" + stageItem.IdStage]) == "00:00:00")
                                    {
                                        dr["ROWS_" + stageItem.IdStage] = DBNull.Value;
                                    }
                                }
                                #endregion


                                string expected = "Expected_" + Convert.ToString(stageItem.IdStage);
                                TimeSpan Tempexpected = TimeSpan.Parse("0");


                                if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Expected)) && Convert.ToString(stageItem.Expected) != "0")
                                {
                                    Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(stageItem.Expected));

                                }
                                else
                                {
                                    Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));

                                }

                                #region [GEOS2-5854][gulab lakade][18 07 2024]
                                if (CADCAMDesignTypeList.Count() > 0)
                                {
                                    var TempCADCAM = CADCAMDesignTypeList.Where(x => x.IdStage == stageItem.IdStage && x.DesignType == Convert.ToString(TimeTrackingListnew[i].DrawingType)).FirstOrDefault();
                                    if (TempCADCAM != null)
                                    {
                                        if (TempCADCAM.RoleValue == "C")
                                        {
                                            Tempexpected = TimeSpan.FromSeconds(TempCADCAM.DesignValue);
                                            TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);//gulab lakade  mismatch total
                                        }
                                        else
                                        {

                                            double doubleExpected = Tempexpected.TotalSeconds;
                                            doubleExpected = doubleExpected * Convert.ToDouble(Convert.ToDouble(TempCADCAM.DesignValue) / Convert.ToDouble(100));
                                            Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                                            TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes); ;//gulab lakade  mismatch total 14 08 2024
                                        }

                                    }
                                    else
                                    {

                                        double doubleExpected = Tempexpected.TotalSeconds;
                                        Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                                        TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);



                                    }
                                }
                                if (DataTableForGridLayout.Columns.Contains(expected))
                                {
                                    dr[expected] = Tempexpected;
                                    //gulab lakade  mismatch total 14 08 2024
                                    if (TempExpectedTime != 0)
                                    {
                                        stageItem.Expected = Convert.ToDecimal(TempExpectedTime);
                                    }
                                    //gulab lakade  mismatch total 14 08 2024
                                }
                                #endregion

                                string remaining = "Remaining_" + Convert.ToString(stageItem.IdStage);
                                #region [pallavi jadhav][GEOS2-5320][05 11 2024]
                                string plannedDeliveryDate = "PlannedDeliveryDate_" + Convert.ToString(stageItem.IdStage);
                                string plannedDeliveryDateHtmlColor = "PlannedDeliveryDateHtmlColor_" + Convert.ToString(stageItem.IdStage);
                                if (DataTableForGridLayout.Columns.Contains(plannedDeliveryDate))
                                {
                                    if (stageItem.PlannedDeliveryDateByStage != null)
                                    {
                                        dr[plannedDeliveryDateHtmlColor] = stageItem.PlannedDeliveryDateHtmlColor;
                                        dr[plannedDeliveryDate] = stageItem.PlannedDeliveryDateByStage;
                                    }
                                }
                                string days = "Days_" + Convert.ToString(stageItem.IdStage);
                                if (DataTableForGridLayout.Columns.Contains(days))
                                {
                                    if (stageItem.Days != null && stageItem.Days != 0)
                                    {
                                        dr[days] = stageItem.Days;
                                    }
                                }
                                #endregion
                                string tempcolor = "Tempcolor_" + Convert.ToString(stageItem.IdStage);
                                TimeSpan TempProduction = Pro_InTimespan;
                                //if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Production)) && Convert.ToString(stageItem.Production) != "0")
                                //{
                                //    TempProduction = TimeSpan.FromSeconds(Convert.ToDouble(stageItem.Production));
                                //}

                                //if (Tempreal <= Tempexpected)
                                if (TempProduction <= Tempexpected)
                                {

                                    if (DataTableForGridLayout.Columns.Contains(tempcolor))
                                    {
                                        dr[tempcolor] = true;
                                    }

                                    if (DataTableForGridLayout.Columns.Contains(remaining))
                                    {
                                        dr[remaining] = (Tempexpected - TempProduction);
                                    }
                                    TempTotalRemaianing += (Tempexpected - TempProduction);

                                }
                                else
                                {
                                    if (DataTableForGridLayout.Columns.Contains(tempcolor))
                                    {
                                        dr[tempcolor] = false;
                                    }
                                    if (DataTableForGridLayout.Columns.Contains(remaining))
                                    {
                                        dr[remaining] = (Tempexpected - TempProduction);
                                    }
                                    TempTotalRemaianing += (Tempexpected - TempProduction);
                                }
                            }

                            double TotalExpectedIndouble = Convert.ToDouble(TempTimeTrackingByStageList.Sum(a => a.Expected));
                            // DateTime PDD = TempTimeTrackingByStageList.Select(a=>a.PlannedDeliveryDateByStage).


                            //TempTotalReal = TimeSpan.FromSeconds(TotalRealIndouble);
                            TempTotalReal = TotalProductiontime + TotalProductionOwstime + TotalReworktime;
                            dr["Real"] = TempTotalReal;


                            if (TotalExpectedIndouble != 0)
                            {
                                //TempTotalExpected = TimeSpan.FromSeconds(TotalExpectedIndouble);
                                TempTotalExpected = TimeSpan.FromMinutes(TotalExpectedIndouble); //gulab lakade  mismatch total 14 08 2024
                                dr["Expected"] = TempTotalExpected;

                            }
                            if (TotalProductiontime != TimeSpan.Parse("0"))
                            {
                                dr["Production"] = TotalProductiontime;

                            }
                            if (TotalProductionOwstime != TimeSpan.Parse("0"))
                            {
                                dr["POWS"] = TotalProductionOwstime;

                            }
                            if (TotalReworktime != TimeSpan.Parse("0"))
                            {
                                dr["Rework"] = TotalReworktime;

                            }
                            if (TotalProductionOwstime != TimeSpan.Parse("0"))
                            {
                                dr["ROWS"] = TotalReworkOwstime;

                            }
                            if (TempTotalRemaianing != null)
                            {
                                if (TempTotalReal <= TempTotalExpected)
                                {

                                    dr["Tempcolor"] = true;
                                    dr["Remaining"] = TempTotalRemaianing;
                                }
                                else
                                {
                                    dr["Tempcolor"] = false;
                                    dr["Remaining"] = TempTotalRemaianing;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        //try
                        //{
                        //    List<TimeTrackingCurrentStage> TempTimeTrackingByStageList = new List<TimeTrackingCurrentStage>();

                        //    var temprecord = TimeTrackingListnew[i].TimeTrackingStage.OrderBy(a => a.IdCounterparttracking).ToList();
                        //    bool reworkflag = false;
                        //    int? checkIdstage = 0;
                        //    int? checkIdstageWithinRework = 0;
                        //    string Production = string.Empty;
                        //    string Rework = string.Empty;
                        //    TimeSpan ProductionTime = new TimeSpan();
                        //    TimeSpan ReworkTime = new TimeSpan();
                        //    double ReworkTimeIndouble = 0;
                        //    TimeSpan TotalProductiontime = new TimeSpan();
                        //    TimeSpan TotalReworktime = new TimeSpan();
                        //    int? DetectedIdStage = 0;
                        //    int? DetectedIdStagWithinRework = 0;
                        //    double ReworkTimeIndoubleWithinRework = 0;

                        //    //rajashri start GEOS2-6054
                        //    double ProductionOWStime = 0;
                        //    string POWS = string.Empty;
                        //    string ROWS = string.Empty;
                        //    TimeSpan TotalProductionOwstime = new TimeSpan();
                        //    TimeSpan OWSprodandReworkTime = new TimeSpan();
                        //    TimeSpan tempstoreOWSvalues = new TimeSpan();
                        //    List<TimeTrackingCurrentStage> ReworkTimeStageList = new List<TimeTrackingCurrentStage>();

                        //    //end
                        //    foreach (var item in temprecord)
                        //    {
                        //        bool FlagPresentInActivePlants = false;
                        //        var Stagerecord = TimeTrackingProductionStage.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                        //        if (Stagerecord != null)
                        //        {
                        //            //TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //            #region [Rupali Sarode][GEOS2-4347][05-05-2023] -- Show stage only if present in ActiveInPlants

                        //            string[] ArrActiveInPlants;

                        //            if (Stagerecord.ActiveInPlants != null && Stagerecord.ActiveInPlants != "")
                        //            {
                        //                ArrActiveInPlants = Stagerecord.ActiveInPlants.Split(',');
                        //                List<Site> tmpSelectedPlant = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                        //                FlagPresentInActivePlants = false;
                        //                foreach (Site itemSelectedPlant in tmpSelectedPlant)
                        //                {
                        //                    if (FlagPresentInActivePlants == true)
                        //                    {
                        //                        break;
                        //                    }

                        //                    foreach (var iPlant in ArrActiveInPlants)
                        //                    {
                        //                        if (Convert.ToUInt16(iPlant) == itemSelectedPlant.IdSite)
                        //                        {
                        //                            FlagPresentInActivePlants = true;
                        //                            break;
                        //                        }
                        //                    }

                        //                }
                        //            }
                        //            #endregion
                        //            if (FlagPresentInActivePlants == true || Stagerecord.ActiveInPlants == null || Stagerecord.ActiveInPlants == "")
                        //            {
                        //                if (TempTimeTrackingByStageList.Count() > 0)
                        //                {
                        //                    var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                        //                    decimal? RealTime = 0;
                        //                    decimal? ExpectedTime = 0;
                        //                    if (Real != null)
                        //                    {
                        //                        if (Real.Real != null)
                        //                        {
                        //                            RealTime = Real.Real;
                        //                        }
                        //                        if (Real.Expected == null)
                        //                        {
                        //                            if (item.Expected == null || item.Expected == 0)
                        //                            {
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                        //                            }
                        //                            else
                        //                            {
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = item.Expected);
                        //                            }

                        //                        }
                        //                        else
                        //                        {
                        //                            TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = Real.Expected);
                        //                        }

                        //                    }
                        //                    else
                        //                    {
                        //                        TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                        //var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage && X.Expected!=null).FirstOrDefault();

                        //                        if (item.Expected != null)
                        //                        {
                        //                            TempTimeTrackingByStage.Expected = item.Expected;
                        //                        }
                        //                        else
                        //                        {
                        //                            TempTimeTrackingByStage.Expected = 0;
                        //                        }
                        //                        TempTimeTrackingByStage.IdStage = item.IdStage;
                        //                        TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                        //                        TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                        //                        TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                        //                        TempTimeTrackingByStage.Days = item.Days;
                        //                        TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                    // var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage).FirstOrDefault();

                        //                    if (item.Expected != null)
                        //                    {
                        //                        TempTimeTrackingByStage.Expected = item.Expected;
                        //                    }
                        //                    else
                        //                    {
                        //                        TempTimeTrackingByStage.Expected = 0;
                        //                    }
                        //                    TempTimeTrackingByStage.IdStage = item.IdStage;
                        //                    TempTimeTrackingByStage.Real = 0;// Convert.ToDecimal(item.TimeDifference);
                        //                    TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                        //                    TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                        //                    TempTimeTrackingByStage.Days = item.Days;
                        //                    TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                }
                        //                if (reworkflag == true)
                        //                {
                        //                    if (checkIdstage == item.IdStage)
                        //                    {
                        //                        reworkflag = false;
                        //                        checkIdstage = 0;
                        //                        //Rework = "Rework_" + item.IdStage;
                        //                        Rework = "Rework_" + DetectedIdStage;
                        //                        POWS = "POWS_" + DetectedIdStage;
                        //                        ROWS = "ROWS_" + DetectedIdStage;
                        //                        //ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                        ReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(ReworkTimeIndouble));
                        //                        TotalReworktime += ReworkTime;
                        //                        ProductionOWStime = ProductionOWStime + item.TimeDifference;//rajashri GEOS2-6054
                        //                        OWSprodandReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(ProductionOWStime));//rajashri GEOS2-6054
                        //                        #region
                        //                        //rajashri start GEOS2-6054
                        //                        if (item.Rework > 0)
                        //                        {
                        //                            var DetectedStageIndex1 = temprecord.IndexOf(item);
                        //                            int lastindex1 = temprecord.Count();
                        //                            if (lastindex1 - 1 > DetectedStageIndex1)
                        //                            {
                        //                                var Detectedrecord1 = temprecord[DetectedStageIndex1 + 1];
                        //                                if (Detectedrecord1 != null)
                        //                                {
                        //                                    DetectedIdStage = Detectedrecord1.IdStage;// detected rework stage
                        //                                }
                        //                                TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                        //                                TempReworkTime.IdStage = Detectedrecord1.IdStage;
                        //                                TempReworkTime.IdCounterparttracking = Detectedrecord1.IdCounterparttracking;
                        //                                TempReworkTime.TimeDifference = Detectedrecord1.TimeDifference;
                        //                                ReworkTimeStageList.Add(TempReworkTime);
                        //                            }
                        //                        }

                        //                        if (DataTableForGridLayout.Columns.Contains(ROWS))
                        //                        {
                        //                            if (Convert.ToString(dr[ROWS]) != null)
                        //                            {
                        //                                string reworkowsInstring = Convert.ToString(dr[ROWS]);
                        //                                if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                        //                                {
                        //                                    reworkowsInstring = "00:00:00";
                        //                                }
                        //                                string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                        //                                string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                TimeSpan timespan = TempReworkInTimespan + OWSprodandReworkTime;
                        //                                //tempstoreOWSvalues = timespan;
                        //                                // TotalProductionOwstime += timespan;
                        //                                if (DataTableForGridLayout.Columns.Contains(ROWS))
                        //                                {
                        //                                    dr[ROWS] = timespan;

                        //                                }

                        //                            }
                        //                            else
                        //                            {
                        //                                if (DataTableForGridLayout.Columns.Contains(ROWS))
                        //                                {
                        //                                    dr[ROWS] = OWSprodandReworkTime;

                        //                                }

                        //                            }
                        //                        }
                        //                        if (DataTableForGridLayout.Columns.Contains(POWS))
                        //                        {
                        //                            if (Convert.ToString(dr[POWS]) != null)
                        //                            {
                        //                                string productionowsInstring = Convert.ToString(dr[POWS]);
                        //                                if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)) || productionowsInstring == "0" || productionowsInstring == "00:00:00")
                        //                                {
                        //                                    productionowsInstring = "00:00:00";
                        //                                }
                        //                                string timeString = Convert.ToString(productionowsInstring); // Example time format string
                        //                                string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                TimeSpan timespan = TempReworkInTimespan + OWSprodandReworkTime;
                        //                                tempstoreOWSvalues = timespan;
                        //                                //TotalProductionOwstime = new TimeSpan();
                        //                                TotalProductionOwstime += timespan;
                        //                                if (DataTableForGridLayout.Columns.Contains(POWS))
                        //                                {
                        //                                    dr[POWS] = timespan;
                        //                                }

                        //                            }
                        //                            else
                        //                            {
                        //                                if (DataTableForGridLayout.Columns.Contains(POWS))
                        //                                {
                        //                                    dr[POWS] = OWSprodandReworkTime;
                        //                                }

                        //                            }
                        //                        }
                        //                        #endregion

                        //                        if (DataTableForGridLayout.Columns.Contains(Rework))
                        //                        {
                        //                            if (Convert.ToString(dr[Rework]) != null)
                        //                            {
                        //                                string ReworkInstring = Convert.ToString(dr[Rework]);
                        //                                if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)) || ReworkInstring == "0" || ReworkInstring == "00:00:00")
                        //                                {
                        //                                    ReworkInstring = "00:00:00";
                        //                                }
                        //                                string timeString = Convert.ToString(ReworkInstring); // Example time format string
                        //                                string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                //TimeSpan timespan = TempReworkInTimespan + ReworkTime;
                        //                                TimeSpan timespan =  ReworkTime;
                        //                                if (DataTableForGridLayout.Columns.Contains(Rework))
                        //                                {
                        //                                    #region Aishwarya Ingale[Geos2-6069]
                        //                                    if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = timespan;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = timespan;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime != TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                    {
                        //                                        if (dr[Production] != DBNull.Value)
                        //                                            dr[Rework] = TimeSpan.Zero;
                        //                                    }
                        //                                    #endregion
                        //                                }

                        //                            }
                        //                            else
                        //                            {
                        //                                if (DataTableForGridLayout.Columns.Contains(Rework))
                        //                                {
                        //                                    #region Aishwarya Ingale[Geos2-6069]
                        //                                    if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = ReworkTime;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = ReworkTime;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    //else if (ProductionTime != TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                                    //{
                        //                                    //    if (dr[Production] != DBNull.Value)
                        //                                    //        dr[Rework] = TimeSpan.Zero;
                        //                                    //}
                        //                                    #endregion
                        //                                }

                        //                            }
                        //                        }
                        //                        if (TempTimeTrackingByStageList.Count() > 0)
                        //                        {
                        //                            //decimal ? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault().Real;
                        //                            //decimal? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).FirstOrDefault().Real;

                        //                            var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).FirstOrDefault();
                        //                            decimal? RealTime = 0;
                        //                            if (Real != null)
                        //                            {
                        //                                if (Real.Real != null)
                        //                                {
                        //                                    RealTime = Real.Real;
                        //                                }
                        //                                if (Real.Expected == null)
                        //                                {
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.Expected = 0);
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.Expected = 0);
                        //                            }
                        //                            //decimal? Realtime = RealTime + Convert.ToDecimal(ReworkTimeIndouble);
                        //                            decimal? Realtime = Real.Production + Convert.ToDecimal(ReworkTimeIndouble) + Convert.ToDecimal(tempstoreOWSvalues.TotalSeconds);//rajashri GEOS2-6054
                        //                            //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                            TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                            TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage);
                        //                            TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor);
                        //                            //dr["PlannedDeliveryDateHtmlColor"] = item.PlannedDeliveryDateHtmlColor;
                        //                            TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStage).ToList().ForEach(a => a.Days = item.Days);

                        //                        }
                        //                        else
                        //                        {
                        //                            TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                            var tempexpexcted = temprecord.Where(X => X.IdStage == DetectedIdStage).FirstOrDefault();

                        //                            if (tempexpexcted.Expected != null)
                        //                            {
                        //                                TempTimeTrackingByStage.Expected = tempexpexcted.Expected;
                        //                            }
                        //                            else
                        //                            {
                        //                                TempTimeTrackingByStage.Expected = 0;
                        //                            }
                        //                            TempTimeTrackingByStage.IdStage = DetectedIdStage;
                        //                            TempTimeTrackingByStage.Real = Convert.ToDecimal(item.TimeDifference);
                        //                            TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                        //                            TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                        //                            TempTimeTrackingByStage.Days = item.Days;
                        //                            TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                        }
                        //                        ReworkTimeIndouble = 0;
                        //                        DetectedIdStage = 0;
                        //                        ProductionOWStime = 0;//GEOS2-6054
                        //                        // gulab lakade 19 04 2024
                        //                        if (item.Rework == 1)
                        //                        {
                        //                            reworkflag = true;
                        //                            checkIdstage = item.IdStage;
                        //                            var DetectedStageIndex = temprecord.IndexOf(item);
                        //                            int lastindex = temprecord.Count();
                        //                            if (lastindex - 1 > DetectedStageIndex)
                        //                            {
                        //                                var Detectedrecord = temprecord[DetectedStageIndex + 1];
                        //                                if (Detectedrecord != null)
                        //                                {
                        //                                    DetectedIdStage = Detectedrecord.IdStage;// detected rework stage
                        //                                }
                        //                            }
                        //                        }

                        //                        //end gulab lakade 19 04 2024
                        //                    }
                        //                    else
                        //                    {
                        //                        if (checkIdstageWithinRework != null && checkIdstageWithinRework != 0)
                        //                        {
                        //                            //if (checkIdstageWithinRework == item.IdStage)
                        //                            //{
                        //                            //    checkIdstageWithinRework = 0;
                        //                            //}

                        //                            Rework = "Rework_" + DetectedIdStagWithinRework;
                        //                            POWS = "POWS_" + DetectedIdStagWithinRework;
                        //                            ROWS = "ROWS_" + DetectedIdStagWithinRework;
                        //                            if (ReworkTimeStageList.Any(ti => ti.IdStage == item.IdStage))
                        //                            {
                        //                                ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                            }
                        //                            else
                        //                            {
                        //                                ProductionOWStime = ProductionOWStime + item.TimeDifference;
                        //                            }
                        //                            if (Convert.ToString(dr[Rework]) != null || Convert.ToString(dr[POWS]) != null || Convert.ToString(dr[ROWS]) != null)
                        //                            {
                        //                                if (DataTableForGridLayout.Columns.Contains(Rework))
                        //                                {
                        //                                    string ReworkInstring = Convert.ToString(dr[Rework]);
                        //                                    if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)))
                        //                                    {
                        //                                        ReworkInstring = "00:00:00";
                        //                                    }
                        //                                    string timeString = Convert.ToString(ReworkInstring); // Example time format string
                        //                                    string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                    TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                    //TimeSpan timespan = TempReworkInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));

                        //                                    TimeSpan timespan = TimeSpan.FromSeconds(Convert.ToDouble(ReworkTimeIndouble));//rajashri GEOS2-6054
                        //                                    if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                                    {
                        //                                        #region Aishwarya Ingale[Geos2-6069]
                        //                                        if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                        {
                        //                                            dr[Rework] = timespan;
                        //                                        }
                        //                                        else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                        {
                        //                                            dr[Rework] = DBNull.Value;
                        //                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                        }
                        //                                        else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                        {
                        //                                            dr[Rework] = timespan;
                        //                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                        }
                        //                                        else if (ProductionTime == TimeSpan.Zero)
                        //                                        {
                        //                                            dr[Rework] = DBNull.Value;
                        //                                            TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                        }
                        //                                        //else if (ProductionTime != TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                        //{
                        //                                        //    if (dr[Production] != DBNull.Value)
                        //                                        //        dr[Rework] = TimeSpan.Zero;
                        //                                        //}
                        //                                        #endregion
                        //                                    }
                        //                                }
                        //                                #region 6054
                        //                                //rajashri
                        //                                if (DataTableForGridLayout.Columns.Contains(ROWS))
                        //                                {
                        //                                    if (Convert.ToString(dr[ROWS]) != null)
                        //                                    {
                        //                                        string reworkowsInstring = Convert.ToString(dr[ROWS]);
                        //                                        if (string.IsNullOrEmpty(Convert.ToString(reworkowsInstring)) || reworkowsInstring == "0" || reworkowsInstring == "00:00:00")
                        //                                        {
                        //                                            reworkowsInstring = "00:00:00";
                        //                                        }
                        //                                        string timeString = Convert.ToString(reworkowsInstring); // Example time format string
                        //                                        string format = "hh\\:mm\\:ss";
                        //                                        TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                        TimeSpan timespan = TempReworkInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(ProductionOWStime));
                        //                                        if (DataTableForGridLayout.Columns.Contains(ROWS))
                        //                                        {
                        //                                            dr[ROWS] = timespan;

                        //                                        }

                        //                                    }
                        //                                }
                        //                                if (DataTableForGridLayout.Columns.Contains(POWS))
                        //                                {
                        //                                    string productionowsInstring = Convert.ToString(dr[POWS]);
                        //                                    if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)))
                        //                                    {
                        //                                        productionowsInstring = "00:00:00";
                        //                                    }
                        //                                    string timeString = Convert.ToString(productionowsInstring); // Example time format string
                        //                                    string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                    TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                    //ProductionOWStime = ProductionOWStime + item.TimeDifference;
                        //                                    TimeSpan timespan = TempReworkInTimespan + TimeSpan.FromSeconds(Convert.ToDouble(ProductionOWStime));
                        //                                    if (DataTableForGridLayout.Columns.Contains(POWS))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                                    {

                        //                                        dr[POWS] = timespan;

                        //                                    }

                        //                                }
                        //                                //end
                        //                                #endregion
                        //                                if (TempTimeTrackingByStageList.Count() > 0)
                        //                                {
                        //                                    //decimal ? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault().Real;

                        //                                    var Real = TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).FirstOrDefault();
                        //                                    decimal? RealTime = 0;
                        //                                    if (Real != null)
                        //                                    {
                        //                                        if (Real.Real != null)
                        //                                        {
                        //                                            RealTime = Real.Real;
                        //                                        }
                        //                                        if (Real.Expected == null)
                        //                                        {
                        //                                            TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.Expected = 0);
                        //                                        }


                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.Expected = 0);
                        //                                    }
                        //                                    decimal? Realtime = RealTime + Convert.ToDecimal(item.TimeDifference);
                        //                                    //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.Real = Realtime);
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage);
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.Days = item.Days);
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == DetectedIdStagWithinRework).ToList().ForEach(a => a.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor);

                        //                                }
                        //                                else
                        //                                {
                        //                                    TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                                    var tempexpexcted = temprecord.Where(X => X.IdStage == DetectedIdStagWithinRework).FirstOrDefault();
                        //                                    if (tempexpexcted.Expected != null)
                        //                                    {
                        //                                        TempTimeTrackingByStage.Expected = tempexpexcted.Expected;
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        TempTimeTrackingByStage.Expected = 0;
                        //                                    }
                        //                                    TempTimeTrackingByStage.IdStage = DetectedIdStagWithinRework;
                        //                                    TempTimeTrackingByStage.Real = Convert.ToDecimal(item.TimeDifference);
                        //                                    TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                        //                                    TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                        //                                    TempTimeTrackingByStage.Days = item.Days;
                        //                                    TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                                }
                        //                                if (DetectedIdStagWithinRework == item.IdStage)
                        //                                {
                        //                                    ReworkTimeIndoubleWithinRework = ReworkTimeIndoubleWithinRework + item.TimeDifference;
                        //                                    checkIdstageWithinRework = 0;
                        //                                    DetectedIdStagWithinRework = 0;
                        //                                    ReworkTimeIndoubleWithinRework = 0;
                        //                                }
                        //                                else
                        //                                {
                        //                                    ReworkTimeIndoubleWithinRework = ReworkTimeIndoubleWithinRework + item.TimeDifference;
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                //dr[Rework] = ReworkTime;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            //if (item.Rework == 1)
                        //                            //{
                        //                            //    checkIdstageWithinRework = item.IdStage;
                        //                            //    ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                            //}
                        //                            //else
                        //                            //{
                        //                            //    ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                            //}

                        //                            if (item.Rework == 1)
                        //                            {
                        //                                var DetectedStageIndex = temprecord.IndexOf(item);
                        //                                if (temprecord.Count() - 1 > DetectedStageIndex)
                        //                                {
                        //                                    var Detectedrecord = temprecord[DetectedStageIndex + 1];
                        //                                    if (Detectedrecord != null)
                        //                                    {
                        //                                        DetectedIdStagWithinRework = Detectedrecord.IdStage;
                        //                                    }
                        //                                    checkIdstageWithinRework = item.IdStage;
                        //                                    // ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                                    #region GEOS2-6054 rajashri
                        //                                    TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                        //                                    TempReworkTime.IdStage = Detectedrecord.IdStage;
                        //                                    TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                        //                                    TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                        //                                    ReworkTimeStageList.Add(TempReworkTime);

                        //                                    if (ReworkTimeStageList.Any(ti => ti.IdStage == item.IdStage))
                        //                                    {
                        //                                        var timeDifference = ReworkTimeStageList.Where(x => x.IdStage == item.IdStage).Select(x => x.TimeDifference).FirstOrDefault();
                        //                                        var totalTimeDifference = ReworkTimeStageList.Where(x => x.IdStage == item.IdStage).Sum(x => x.TimeDifference);
                        //                                        ReworkTimeIndouble = ReworkTimeIndouble + totalTimeDifference;
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        ProductionOWStime = ProductionOWStime + item.TimeDifference;
                        //                                    }
                        //                                    #endregion
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                //ReworkTimeIndouble = ReworkTimeIndouble + item.TimeDifference;
                        //                                if (ReworkTimeStageList.Any(ti => ti.IdStage == item.IdStage))
                        //                                {
                        //                                    var timeDifference = ReworkTimeStageList.Where(x => x.IdStage == item.IdStage).Select(x => x.TimeDifference).FirstOrDefault();
                        //                                    var totalTimeDifference = ReworkTimeStageList.Where(x => x.IdStage == item.IdStage).Sum(x => x.TimeDifference);
                        //                                    ReworkTimeIndouble = ReworkTimeIndouble + totalTimeDifference;

                        //                                }
                        //                                else
                        //                                {
                        //                                    ProductionOWStime = ProductionOWStime + item.TimeDifference;
                        //                                }
                        //                            }

                        //                        }

                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    if (item.Rework == 1)
                        //                    {
                        //                        var DetectedStageIndex = temprecord.IndexOf(item);
                        //                        int lastindex = temprecord.Count();
                        //                        if (lastindex - 1 > DetectedStageIndex)
                        //                        {
                        //                            var Detectedrecord = temprecord[DetectedStageIndex + 1];
                        //                            if (Detectedrecord != null)
                        //                            {
                        //                                DetectedIdStage = Detectedrecord.IdStage;// detected rework stage
                        //                            }
                        //                            //rajashri 6054
                        //                            TimeTrackingCurrentStage TempReworkTime = new TimeTrackingCurrentStage();
                        //                            TempReworkTime.IdStage = Detectedrecord.IdStage;
                        //                            TempReworkTime.IdCounterparttracking = Detectedrecord.IdCounterparttracking;
                        //                            TempReworkTime.TimeDifference = Detectedrecord.TimeDifference;
                        //                            ReworkTimeStageList.Add(TempReworkTime);
                        //                            //end
                        //                            reworkflag = true;
                        //                            checkIdstage = item.IdStage;
                        //                            Production = "Production_" + item.IdStage;
                        //                            ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                        //                            TotalProductiontime += ProductionTime;
                        //                            //dr[Production] = ProductionTime;
                        //                            if (DataTableForGridLayout.Columns.Contains(Production))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                            {
                        //                                if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                        //                                {
                        //                                    if (dr[Rework] == DBNull.Value)
                        //                                        dr[Rework] = TimeSpan.Zero;
                        //                                    dr[Production] = ProductionTime;
                        //                                }
                        //                                decimal? tempProduction = Convert.ToDecimal(item.TimeDifference);
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                        //                            }
                        //                            if (TempTimeTrackingByStageList.Count() > 0)
                        //                            {
                        //                                //decimal ? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault().Real;
                        //                                var TempProductionRecord = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                        //                                if (TempProductionRecord != null)
                        //                                {
                        //                                    decimal? TempReal = 0;
                        //                                    if (TempProductionRecord.Real != null)
                        //                                    {
                        //                                        TempReal = TempProductionRecord.Real;
                        //                                    }
                        //                                    decimal? TempProduction = 0;
                        //                                    if (TempProductionRecord.Production != null)
                        //                                    {
                        //                                        TempProduction = TempProductionRecord.Production;
                        //                                    }
                        //                                    if (TempProductionRecord.Expected == null)
                        //                                    {
                        //                                        TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                        //                                    }


                        //                                    decimal? Realtime = TempReal + Convert.ToDecimal(item.TimeDifference);
                        //                                    decimal? Productiontime = TempProduction + Convert.ToDecimal(item.TimeDifference);
                        //                                    //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                                    //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = Productiontime);
                        //                                }
                        //                                else
                        //                                {
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                        //                                }

                        //                            }
                        //                            else
                        //                            {
                        //                                TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                                var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage).FirstOrDefault();
                        //                                if (tempexpexcted.Expected != null)
                        //                                {
                        //                                    TempTimeTrackingByStage.Expected = tempexpexcted.Expected;
                        //                                }
                        //                                else
                        //                                {
                        //                                    TempTimeTrackingByStage.Expected = 0;
                        //                                }
                        //                                TempTimeTrackingByStage.IdStage = item.IdStage;
                        //                                TempTimeTrackingByStage.Real = Convert.ToDecimal(item.TimeDifference);
                        //                               // TempTimeTrackingByStage.Production = Convert.ToDecimal(item.TimeDifference);
                        //                                TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        Production = "Production_" + item.IdStage;
                        //                        Rework = "Rework_" + item.IdStage;
                        //                        ProductionTime = TimeSpan.FromSeconds(Convert.ToDouble(item.TimeDifference));
                        //                        ReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));
                        //                        POWS = "POWS_" + item.IdStage;//rajashri GEOS2-6054
                        //                        ROWS = "ROWS_" + item.IdStage;//rajashri GEOS2-6054
                        //                        OWSprodandReworkTime = TimeSpan.FromSeconds(Convert.ToDouble(0));//rajashri GEOS2-6054
                        //                        TotalProductiontime += ProductionTime;
                        //                        if (ProductionTime != TimeSpan.Zero)//Aishwarya Ingale[Geos2-6069]
                        //                        {
                        //                            if (dr[Rework] == DBNull.Value)
                        //                                dr[Rework] = TimeSpan.Zero;
                        //                            dr[Production] = ProductionTime;
                        //                        }
                        //                        decimal? tempProduction = Convert.ToDecimal(item.TimeDifference);
                        //                        TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = tempProduction);
                        //                        TotalReworktime += ReworkTime;
                        //                        if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                        {
                        //                            #region Aishwarya Ingale[Geos2-6069]
                        //                            if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                            {
                        //                                dr[Rework] = ReworkTime;
                        //                            }
                        //                            else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                            {
                        //                                dr[Rework] = DBNull.Value;
                        //                                TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                            }
                        //                            else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                            {
                        //                                dr[Rework] = ReworkTime;
                        //                                TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                            }
                        //                            else if (ProductionTime == TimeSpan.Zero)
                        //                            {
                        //                                dr[Rework] = DBNull.Value;
                        //                                TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                            }
                        //                            //else if (ProductionTime != TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                            //{
                        //                            //    if (dr[Production] != DBNull.Value)
                        //                            //        dr[Rework] = TimeSpan.Zero;
                        //                            //}
                        //                            #endregion


                        //                            if (Convert.ToString(dr[Rework]) != null)
                        //                            {
                        //                                string ReworkInstring = Convert.ToString(dr[Rework]);
                        //                                if (string.IsNullOrEmpty(Convert.ToString(ReworkInstring)))
                        //                                {
                        //                                    ReworkInstring = "00:00:00";
                        //                                }
                        //                                string timeString = Convert.ToString(ReworkInstring); // Example time format string
                        //                                string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                                TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                                TimeSpan timespan = TempReworkInTimespan + ReworkTime;

                        //                                if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                                {
                        //                                    #region Aishwarya Ingale[Geos2-6069]
                        //                                    if (ProductionTime != TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = timespan;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && timespan != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = timespan;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    //else if (ProductionTime != TimeSpan.Zero && timespan == TimeSpan.Zero)
                        //                                    //{
                        //                                    //    if (dr[Production] != DBNull.Value)
                        //                                    //        dr[Rework] = TimeSpan.Zero;
                        //                                    //}
                        //                                    #endregion
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                if (DataTableForGridLayout.Columns.Contains(Rework))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                                {
                        //                                    #region Aishwarya Ingale[Geos2-6069]
                        //                                    if (ProductionTime != TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = ReworkTime;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero && ReworkTime != TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = ReworkTime;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    else if (ProductionTime == TimeSpan.Zero)
                        //                                    {
                        //                                        dr[Rework] = DBNull.Value;
                        //                                        TimeTrackingListnew[i].ProductionHtmlColorFlag = false;
                        //                                    }
                        //                                    //else if (ProductionTime != TimeSpan.Zero && ReworkTime == TimeSpan.Zero)
                        //                                    //{
                        //                                    //    if (dr[Production] != DBNull.Value)
                        //                                    //        dr[Rework] = TimeSpan.Zero;
                        //                                    //}
                        //                                    #endregion
                        //                                }

                        //                            }
                        //                        }
                        //                        //rajashri
                        //                        if (DataTableForGridLayout.Columns.Contains(POWS))
                        //                        {
                        //                            string productionowsInstring = Convert.ToString(dr[POWS]);
                        //                            if (string.IsNullOrEmpty(Convert.ToString(productionowsInstring)) || productionowsInstring == "0" || productionowsInstring == "00:00:00")
                        //                            {
                        //                                productionowsInstring = "00:00:00";
                        //                            }
                        //                            string timeString = Convert.ToString(productionowsInstring); // Example time format string
                        //                            string format = "hh\\:mm\\:ss"; // Specify the format of the time string
                        //                            TimeSpan TempReworkInTimespan = TimeSpan.ParseExact(timeString, format, null);
                        //                            TimeSpan timespan = OWSprodandReworkTime;
                        //                            if (DataTableForGridLayout.Columns.Contains(POWS))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                            {
                        //                                dr[POWS] = timespan;
                        //                                dr[ROWS] = timespan;

                        //                            }
                        //                        }
                        //                        //end
                        //                        if (TempTimeTrackingByStageList.Count() > 0)
                        //                        {
                        //                            //decimal ? Real = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault().Real;
                        //                            var TempProductionRecord = TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).FirstOrDefault();
                        //                            if (TempProductionRecord != null)
                        //                            {
                        //                                decimal? TempReal = TempProductionRecord.Real;
                        //                                decimal? TempProduction = TempProductionRecord.Production;

                        //                                decimal? Realtime = TempReal + Convert.ToDecimal(item.TimeDifference);
                        //                                decimal? Productiontime = TempProduction + Convert.ToDecimal(item.TimeDifference);
                        //                                if (TempProductionRecord.Expected == null)
                        //                                {
                        //                                    TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                        //                                }

                        //                                //TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Real = Realtime);
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage);
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor);
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Days = item.Days);
                        //                                // TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Production = Productiontime);
                        //                            }
                        //                            else
                        //                            {
                        //                                TempTimeTrackingByStageList.Where(x => x.IdStage == item.IdStage).ToList().ForEach(a => a.Expected = 0);
                        //                            }

                        //                        }
                        //                        else
                        //                        {
                        //                            TimeTrackingCurrentStage TempTimeTrackingByStage = new TimeTrackingCurrentStage();
                        //                            var tempexpexcted = temprecord.Where(X => X.IdStage == item.IdStage).FirstOrDefault();
                        //                            if (tempexpexcted != null)
                        //                            {
                        //                                if (tempexpexcted.Expected != null)
                        //                                {
                        //                                    TempTimeTrackingByStage.Expected = tempexpexcted.Expected;
                        //                                }
                        //                                else
                        //                                {
                        //                                    TempTimeTrackingByStage.Expected = 0;
                        //                                }

                        //                            }
                        //                            TempTimeTrackingByStage.IdStage = item.IdStage;
                        //                            TempTimeTrackingByStage.Real = Convert.ToDecimal(item.TimeDifference);
                        //                            TempTimeTrackingByStage.PlannedDeliveryDateByStage = item.PlannedDeliveryDateByStage;
                        //                            TempTimeTrackingByStage.PlannedDeliveryDateHtmlColor = item.PlannedDeliveryDateHtmlColor;
                        //                            TempTimeTrackingByStage.Days = item.Days;
                        //                            //TempTimeTrackingByStage.Production = Convert.ToDecimal(item.TimeDifference);
                        //                            TempTimeTrackingByStageList.Add(TempTimeTrackingByStage);
                        //                        }
                        //                    }
                        //                }

                        //            }
                        //            else
                        //            {
                        //                if (item.Rework == 1)
                        //                {
                        //                    reworkflag = true;
                        //                    checkIdstage = item.IdStage;
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (item.Rework == 1)
                        //            {
                        //                reworkflag = true;
                        //                checkIdstage = item.IdStage;
                        //            }
                        //        }
                        //    }
                        //    foreach (var stageItem in TempTimeTrackingByStageList)
                        //    {
                        //        string real = "Real_" + Convert.ToString(stageItem.IdStage);
                        //        double TempExpectedTime = 0;//gulab lakade  mismatch total
                        //        TimeSpan Tempreal = TimeSpan.Parse("0");

                        //        if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Real)) && Convert.ToString(stageItem.Real) != "0")
                        //        {
                        //            //Tempreal = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real));
                        //            Tempreal = TimeSpan.FromSeconds(Convert.ToDouble(stageItem.Real));
                        //            if (DataTableForGridLayout.Columns.Contains(real))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //            {
                        //                dr[real] = Tempreal;
                        //            }
                        //        }
                        //        string expected = "Expected_" + Convert.ToString(stageItem.IdStage);
                        //        TimeSpan Tempexpected = TimeSpan.Parse("0");
                        //        if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Expected)) && Convert.ToString(stageItem.Expected) != "0")
                        //        {
                        //            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(stageItem.Expected));
                        //            //[GEOS2-5854][gulab lakade][18 07 2024]
                        //            //if (DataTableForGridLayout.Columns.Contains(expected))
                        //            //{
                        //            //    dr[expected] = Tempexpected;
                        //            //}
                        //            //end [GEOS2-5854][gulab lakade][18 07 2024]
                        //        }
                        //        else
                        //        {
                        //            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                        //            //[GEOS2-5854][gulab lakade][18 07 2024]
                        //            //if (DataTableForGridLayout.Columns.Contains(expected))
                        //            //{
                        //            //    dr[expected] = Tempexpected;
                        //            //}
                        //            //end [GEOS2-5854][gulab lakade][18 07 2024]
                        //        }

                        //        #region [GEOS2-5854][gulab lakade][18 07 2024]
                        //        if (CADCAMDesignTypeList.Count() > 0)
                        //        {
                        //            var TempCADCAM = CADCAMDesignTypeList.Where(x => x.IdStage == stageItem.IdStage && x.DesignType == Convert.ToString(TimeTrackingListnew[i].DrawingType)).FirstOrDefault();
                        //            if (TempCADCAM != null)
                        //            {
                        //                if (TempCADCAM.RoleValue == "C")
                        //                {
                        //                    Tempexpected = TimeSpan.FromSeconds(TempCADCAM.DesignValue);
                        //                    TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);//gulab lakade  mismatch total
                        //                }
                        //                else
                        //                {


                        //                        double doubleExpected = Tempexpected.TotalSeconds;
                        //                        doubleExpected = doubleExpected * Convert.ToDouble(Convert.ToDouble(TempCADCAM.DesignValue) / Convert.ToDouble(100));
                        //                        Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                        //                        TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes); ;//gulab lakade  mismatch total 14 08 2024

                        //                }
                        //            }
                        //            else
                        //            {
                        //                        double doubleExpected = Tempexpected.TotalSeconds;
                        //                        Tempexpected = TimeSpan.FromSeconds(doubleExpected);
                        //                        TempExpectedTime = Convert.ToDouble(Tempexpected.TotalMinutes);

                        //            }
                        //        }
                        //        if (DataTableForGridLayout.Columns.Contains(expected))
                        //        {
                        //            dr[expected] = Tempexpected;
                        //            //gulab lakade  mismatch total 14 08 2024
                        //            if (TempExpectedTime != 0)
                        //            {
                        //                stageItem.Expected = Convert.ToDecimal(TempExpectedTime);
                        //            }
                        //            //gulab lakade  mismatch total 14 08 2024
                        //        }
                        //        #endregion
                        //        string remaining = "Remaining_" + Convert.ToString(stageItem.IdStage);
                        //        #region [pallavi jadhav][GEOS2-5320][05 11 2024]
                        //        string plannedDeliveryDate = "PlannedDeliveryDate_" + Convert.ToString(stageItem.IdStage);
                        //        string plannedDeliveryDateHtmlColor = "PlannedDeliveryDateHtmlColor_" + Convert.ToString(stageItem.IdStage);
                        //        if (DataTableForGridLayout.Columns.Contains(plannedDeliveryDate))
                        //        {
                        //            if (stageItem.PlannedDeliveryDateByStage != null)
                        //            {
                        //                dr[plannedDeliveryDateHtmlColor] = stageItem.PlannedDeliveryDateHtmlColor;
                        //                dr[plannedDeliveryDate] = stageItem.PlannedDeliveryDateByStage;
                        //            }
                        //        }
                        //        string days = "Days_" + Convert.ToString(stageItem.IdStage);
                        //        if (DataTableForGridLayout.Columns.Contains(days))
                        //        {
                        //            if (stageItem.Days != null && stageItem.Days != 0)
                        //            {
                        //                dr[days] = stageItem.Days;
                        //            }
                        //        }
                        //        #endregion
                        //        string tempcolor = "Tempcolor_" + Convert.ToString(stageItem.IdStage);
                        //        TimeSpan TempProduction = TimeSpan.Parse("0");
                        //        if (!string.IsNullOrEmpty(Convert.ToString(stageItem.Production)) && Convert.ToString(stageItem.Production) != "0")
                        //        {
                        //            TempProduction = TimeSpan.FromSeconds(Convert.ToDouble(stageItem.Production));
                        //        }
                        //        //if (Tempreal <= Tempexpected)
                        //        if (TempProduction <= Tempexpected)
                        //        {

                        //            if (DataTableForGridLayout.Columns.Contains(tempcolor))
                        //            {
                        //                dr[tempcolor] = true;
                        //            }

                        //            if (DataTableForGridLayout.Columns.Contains(remaining))
                        //            {
                        //                dr[remaining] = (Tempexpected - TempProduction);
                        //            }
                        //            TempTotalRemaianing += (Tempexpected - TempProduction);

                        //        }
                        //        else
                        //        {
                        //            if (DataTableForGridLayout.Columns.Contains(tempcolor))
                        //            {
                        //                dr[tempcolor] = false;
                        //            }
                        //            if (DataTableForGridLayout.Columns.Contains(remaining))
                        //            {
                        //                dr[remaining] = (Tempexpected - TempProduction);
                        //            }
                        //            TempTotalRemaianing += (Tempexpected - TempProduction);
                        //        }
                        //    }
                        //    double TotalRealIndouble = Convert.ToDouble(TempTimeTrackingByStageList.Sum(a => a.Real));

                        //    double TotalExpectedIndouble = Convert.ToDouble(TempTimeTrackingByStageList.Sum(a => a.Expected));

                        //    if (TotalRealIndouble != 0)
                        //    {
                        //        TempTotalReal = TimeSpan.FromSeconds(TotalRealIndouble);
                        //        dr["Real"] = TempTotalReal;

                        //    }
                        //    if (TotalExpectedIndouble != 0)
                        //    {
                        //        //TempTotalExpected = TimeSpan.FromSeconds(TotalExpectedIndouble);
                        //        TempTotalExpected = TimeSpan.FromMinutes(TotalExpectedIndouble); //gulab lakade  mismatch total 14 08 2024
                        //        dr["Expected"] = TempTotalExpected;

                        //    }
                        //    if (TotalProductiontime != TimeSpan.Parse("0"))
                        //    {
                        //        dr["Production"] = TotalProductiontime;

                        //    }
                        //    if (TotalReworktime != TimeSpan.Parse("0"))
                        //    {
                        //        dr["Rework"] = TotalReworktime;

                        //    }
                        //    if (TempTotalRemaianing != null)
                        //    {
                        //        if (TempTotalReal <= TempTotalExpected)
                        //        {

                        //            dr["Tempcolor"] = true;
                        //            dr["Remaining"] = TempTotalRemaianing;
                        //        }
                        //        else
                        //        {
                        //            dr["Tempcolor"] = false;
                        //            dr["Remaining"] = TempTotalRemaianing;
                        //        }
                        //    }
                        //}
                        //catch (Exception ex)
                        //{

                        //}

                        //    for (int iItem = 0; iItem < TimeTrackingList[i].TimeTrackingStage.GroupBy(x => x.IdStage).ToList().Count; iItem++)
                        //    {
                        //        try
                        //        {
                        //            string real = "Real_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            TimeSpan Tempreal = TimeSpan.Parse("0");

                        //            if (!string.IsNullOrEmpty(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real)) && Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real) != "0")
                        //            {
                        //                //Tempreal = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Real));
                        //                Tempreal = TimeSpan.FromSeconds(Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Real));
                        //                if (DataTableForGridLayout.Columns.Contains(real))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[real] = Tempreal;
                        //                }
                        //            }

                        //            TempTotalReal += Tempreal;// Convert.ToDouble(item1.Real);

                        //            string expected = "Expected_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            TimeSpan Tempexpected = TimeSpan.Parse("0");
                        //            if (!string.IsNullOrEmpty(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected)) && Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected) != "0")
                        //            {
                        //                //Tempexpected = ConvertfloattoTimespan(Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].Expected));
                        //                Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Expected));
                        //                if (DataTableForGridLayout.Columns.Contains(expected))  // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[expected] = Tempexpected;
                        //                }
                        //            }
                        //            else    //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                        //            {
                        //                Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                        //                if (DataTableForGridLayout.Columns.Contains(expected)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[expected] = Tempexpected;
                        //                }
                        //            }

                        //            TempTotalExpected += Tempexpected;// Convert.ToDouble(item1.Expected);

                        //            string remaining = "Remaining_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);

                        //            string tempcolor = "Tempcolor_" + Convert.ToString(TimeTrackingList[i].TimeTrackingStage[iItem].IdStage);
                        //            //if (Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Real) <= Convert.ToDouble(TimeTrackingList[i].TimeTrackingStage[iItem].Expected))
                        //            if (Tempreal <= Tempexpected)
                        //            {
                        //                //Remainingtimecolor = true;
                        //                //TimeTrackingList[i].Tempcolor = false;
                        //                if (DataTableForGridLayout.Columns.Contains(tempcolor)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[tempcolor] = true;
                        //                }

                        //                if (DataTableForGridLayout.Columns.Contains(remaining)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[remaining] = (Tempexpected - Tempreal);
                        //                }
                        //                TempTotalRemaianing += (Tempexpected - Tempreal);

                        //            }
                        //            else
                        //            {
                        //                //TimeTrackingList[i].Tempcolor = true;
                        //                if (DataTableForGridLayout.Columns.Contains(tempcolor)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[tempcolor] = false;
                        //                }
                        //                if (DataTableForGridLayout.Columns.Contains(remaining)) // [Rupali Sarode][GEOS2-4347][05-05-2023]
                        //                {
                        //                    dr[remaining] = (Tempexpected - Tempreal);
                        //                }
                        //                TempTotalRemaianing += (Tempexpected - Tempreal);
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }
                        //    }
                        //}
                        //if (TempTotalReal != null)
                        //{
                        //    dr["Real"] = TempTotalReal;

                        //}
                        //if (TempTotalExpected != null)
                        //{
                        //    dr["Expected"] = TempTotalExpected;

                        //}
                        //if (TempTotalRemaianing != null)
                        //{
                        //    if (TempTotalReal <= TempTotalExpected)
                        //    {

                        //        dr["Tempcolor"] = true;
                        //        dr["Remaining"] = TempTotalRemaianing;
                        //    }
                        //    else
                        //    {
                        //        dr["Tempcolor"] = false;
                        //        dr["Remaining"] = TempTotalRemaianing;
                        //    }
                        //}
                        #endregion


                    }
                    //if (TempTotalReal != null)
                    //{
                    //    dr["Real"] = TempTotalReal;

                    //}
                    //if (TempTotalExpected != null)
                    //{
                    //    dr["Expected"] = TempTotalExpected;

                    //}
                    //if (TempTotalRemaianing != null)
                    //{
                    //    if (TempTotalReal <= TempTotalExpected)
                    //    {

                    //        dr["Tempcolor"] = true;
                    //        dr["Remaining"] = TempTotalRemaianing;
                    //    }
                    //    else
                    //    {
                    //        dr["Tempcolor"] = false;
                    //        dr["Remaining"] = TempTotalRemaianing;
                    //    }
                    //}
                    //[GEOS2-4150] [Gulab Lakade] [30 01 2023]
                    if (TimeTrackingListnew[i].ExpectedHtmlColorFlag == true)
                    {
                        #region [pallavi.jadhav] [10 07 2025] [GEOS2-8868]
                        if (!DataTableForGridLayout.Columns.Contains("ExpectedHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("POWSHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ProductionHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ROWSHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ReworkHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RealHtmlColorFlag"))
                            DataTableForGridLayout.Columns.Add("RealHtmlColorFlag", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag2"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag2", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RemainingHtmlColorFlag6"))
                            DataTableForGridLayout.Columns.Add("RemainingHtmlColorFlag6", typeof(string));
                        #endregion
                        #region [rajashri.telvekar] [07 11 2025] [GEOS2-8309]
                        if (!DataTableForGridLayout.Columns.Contains("ExpectedHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ExpectedHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("POWSHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("POWSHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ProductionHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ProductionHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ROWSHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ROWSHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("ReworkHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("ReworkHtmlColorFlag1", typeof(string));
                        if (!DataTableForGridLayout.Columns.Contains("RealHtmlColorFlag1"))
                            DataTableForGridLayout.Columns.Add("RealHtmlColorFlag1", typeof(string));
                        #endregion
                        dr["ExpectedHtmlColorFlag"] = "#808080";
                        #region[rani dhamankar] [20 - 02 - 2025][GEOS2 - 6685]
                        dr["POWSHtmlColorFlag"] = "#808080";
                        dr["ProductionHtmlColorFlag"] = "#808080";
                        dr["ROWSHtmlColorFlag"] = "#808080";
                        dr["ReworkHtmlColorFlag"] = "#808080";
                        dr["RealHtmlColorFlag"] = "#808080";
                        dr["RemainingHtmlColorFlag1"] = "#808080";
                        dr["RemainingHtmlColorFlag2"] = "#808080";
                        dr["RemainingHtmlColorFlag6"] = "#808080";
                        #endregion
                        dr["ExpectedHtmlColorFlag1"] = "#808080";
                        //#region[rajashri telvekar] [07 - 11 - 2025][GEOS2 - 8309]
                        dr["POWSHtmlColorFlag1"] = "#808080";
                        dr["ProductionHtmlColorFlag1"] = "#808080";
                        dr["ROWSHtmlColorFlag1"] = "#808080";
                        dr["ReworkHtmlColorFlag1"] = "#808080";
                        dr["RealHtmlColorFlag1"] = "#808080";
                        //#endregion
                    }
                    if (TimeTrackingListnew[i].ProductionHtmlColorFlag == true)
                    {
                        dr["ProductionHtmlColorFlag"] = "#808080";
                    }
                
                    DataTableForGridLayout.Rows.Add(dr);
                    rowCounter += 1;

                }
                #region GEOS2[8309][7/11/2025 ] [pallavi.jadhav][rajashri.telvekar]
                // Allowed Expected columns
                var expectedColumns = DataTableForGridLayout.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .Where(c =>
                        c.StartsWith("Expected_", StringComparison.OrdinalIgnoreCase) &&
                        (c.EndsWith("_1") || c.EndsWith("_2") || c.EndsWith("_6")))
                    .ToList();

                foreach (DataRow row in DataTableForGridLayout.Rows)
                {
                    string serialNumber = row["SerialNumber"].ToString();
                    string itemNumber = row["ItemNumber"].ToString();
                    string otCode = row["OTCode"].ToString();
                    string idDrawing = row["IdDrawing"].ToString();


                    // Loop only Expected_1, Expected_2, Expected_6
                    foreach (string expectedColumn in expectedColumns)
                    {
                        if (string.IsNullOrEmpty(expectedColumn))
                            continue;

                        TimeSpan expectedTime = TimeSpan.Zero;
                        if (row[expectedColumn] != DBNull.Value)
                            TimeSpan.TryParse(row[expectedColumn].ToString(), out expectedTime);

                        int expectedIndex = 0;
                        if (expectedColumn.Contains("_"))
                            int.TryParse(expectedColumn.Split('_')[1], out expectedIndex);

                        if (expectedIndex != 1)
                        {
                            // ✅ Logic for Expected_2 and Expected_6
                            var sameOtSameDrawingList = DataTableForGridLayout.AsEnumerable()
                                .Where(r => r["OTCode"].ToString() == otCode &&
                                            r["IdDrawing"].ToString() == idDrawing)
                                .OrderBy(r => Convert.ToInt64(r["IdCounterpart"]))
                                .ToList();

                            var sameOtDiffDrawingList = DataTableForGridLayout.AsEnumerable()
                                .Where(r => r["OTCode"].ToString() == otCode &&
                                            r["IdDrawing"].ToString() != idDrawing)
                                .OrderBy(r => Convert.ToInt64(r["IdCounterpart"]))
                                .ToList();

                            if (sameOtSameDrawingList.Count > 1)
                            {
                                var firstRow = sameOtSameDrawingList
                                    .FirstOrDefault(r => r["SerialNumber"].ToString().EndsWith("001"))
                                    ?? sameOtSameDrawingList.First();

                                if (firstRow != null && serialNumber == firstRow["SerialNumber"].ToString())
                                {
                                    row[expectedColumn] = expectedTime;

                                    row["ExpectedHtmlColorFlag"] = "";
                                    row["POWSHtmlColorFlag"] = "";
                                    row["ProductionHtmlColorFlag"] = "";
                                    row["ROWSHtmlColorFlag"] = "";
                                    row["ReworkHtmlColorFlag"] = "";
                                    row["RealHtmlColorFlag"] = "";
                                    row["RemainingHtmlColorFlag2"] = "";
                                    row["RemainingHtmlColorFlag6"] = "";
                                }
                                else
                                {
                                    row[expectedColumn] = TimeSpan.Zero;
                                    row["ExpectedHtmlColorFlag"] = "#808080";
                                    row["POWSHtmlColorFlag"] = "#808080";
                                    row["ProductionHtmlColorFlag"] = "#808080";
                                    row["ROWSHtmlColorFlag"] = "#808080";
                                    row["ReworkHtmlColorFlag"] = "#808080";
                                    row["RealHtmlColorFlag"] = "#808080";

                                    row["RemainingHtmlColorFlag2"] = "#808080";
                                    row["RemainingHtmlColorFlag6"] = "#808080";
                                }
                            }
                            else if (sameOtDiffDrawingList.Any())
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                                row["POWSHtmlColorFlag"] = "";
                                row["ProductionHtmlColorFlag"] = "";
                                row["ROWSHtmlColorFlag"] = "";
                                row["ReworkHtmlColorFlag"] = "";
                                row["RealHtmlColorFlag"] = "";
                                row["RemainingHtmlColorFlag1"] = "";
                                row["RemainingHtmlColorFlag2"] = "";
                                row["RemainingHtmlColorFlag6"] = "";
                            }
                            else
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                                row["POWSHtmlColorFlag"] = "";
                                row["ProductionHtmlColorFlag"] = "";
                                row["ROWSHtmlColorFlag"] = "";
                                row["ReworkHtmlColorFlag"] = "";
                                row["RealHtmlColorFlag"] = "";
                                row["RemainingHtmlColorFlag1"] = "";
                                row["RemainingHtmlColorFlag2"] = "";
                                row["RemainingHtmlColorFlag6"] = "";
                            }
                        }
                        else
                        {
                            // ✅ Logic for Expected_1

                            if (expectedIndex == 1)
                            {
                                if (serialNumber.EndsWith("001"))
                                {
                                    row[expectedColumn] = expectedTime;
                                    row["ExpectedHtmlColorFlag1"] = "";
                                    row["POWSHtmlColorFlag1"] = "";
                                    row["ProductionHtmlColorFlag1"] = "";
                                    row["ROWSHtmlColorFlag1"] = "";
                                    row["ReworkHtmlColorFlag1"] = "";
                                    row["RealHtmlColorFlag1"] = "";
                                    row["RemainingHtmlColorFlag1"] = "";
                                }
                                else
                                {
                                    row[expectedColumn] = TimeSpan.Zero;
                                    row["ExpectedHtmlColorFlag1"] = "#808080";
                                    row["POWSHtmlColorFlag1"] = "#808080";
                                    row["ProductionHtmlColorFlag1"] = "#808080";
                                    row["ROWSHtmlColorFlag1"] = "#808080";
                                    row["ReworkHtmlColorFlag1"] = "#808080";
                                    row["RealHtmlColorFlag1"] = "#808080";
                                    row["RemainingHtmlColorFlag1"] = "#808080";
                                }
                            }
                            else
                            {
                                row[expectedColumn] = expectedTime;
                                row["ExpectedHtmlColorFlag"] = "";
                            }
                        }
                    }
                }
                #endregion

                DtTimetracking = DataTableForGridLayout;
                //foreach (var Rowitems in DataTableForGridLayout.Rows)
                //{
                //    DtTimetracking.Rows.Add(Rowitems);
                //}
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }

        #region ////[GEOS2-5098] [gulab lakade][30 11 2023]
        //private static List<TimeTracking> ReturnFullTimeTrackingList_V2340(IERMService ERMService, IPLMService PLMService, ref List<TimeTracking> TimeTrackingList, string CurrencyNameFromSetting, string plantName, UInt32 idSite, string IDPlantName, List<GeosAppSetting> GeosAppSettingList, List<TimeTracking> TimeTrackingListold)
        //{

        //    try
        //    {
        //        if (ERMCommon.Instance.FailedPlants == null)
        //        {
        //            ERMCommon.Instance.FailedPlants = new List<string>();
        //        }
        //        if (ERMCommon.Instance.WarningFailedPlants == null)
        //        {
        //            ERMCommon.Instance.WarningFailedPlants = "";
        //        }

        //        //[GEOS2-4093][Rupali Sarode][26-12-2022]
        //        //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2340(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
        //        //   ERMService = new ERMServiceController("localhost:6699");
        //        //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2360(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
        //        //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2370(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));


        //        //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2380(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));   //[Origin and Production][gulab lakade][17 04 2023]
        //        //  TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2400(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));   //  //[gulab lakade][GEOS2-4494-batch][26 05 2023]
        //        // TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2420(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
        //     //   TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2430(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList)); //[Pallavi Jadhav][14-09-2023][GEOS2-4818]
        //        TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2460(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList)); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]

        //        if (TimeTrackingListold != null && TimeTrackingList != null)
        //            {
        //                if (TimeTrackingList.Count > 0)
        //                {
        //                    TimeTrackingListold.AddRange(TimeTrackingList);
        //                }
        //            }

        //    }/*catch(Exception ex) { }*/
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        GeosApplication.Instance.SplashScreenMessage = string.Empty;
        //        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", IDPlantName, " Failed");
        //        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
        //            if (!ERMCommon.Instance.FailedPlants.Contains(IDPlantName))
        //            {
        //                ERMCommon.Instance.FailedPlants.Add(IDPlantName);
        //                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
        //                {
        //                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
        //                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
        //                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
        //                }
        //            }
        //        //System.Threading.Thread.Sleep(1000);
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", IDPlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        GeosApplication.Instance.SplashScreenMessage = string.Empty;
        //        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", IDPlantName, " Failed");
        //        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
        //            if (!ERMCommon.Instance.FailedPlants.Contains(IDPlantName))
        //            {
        //                ERMCommon.Instance.FailedPlants.Add(IDPlantName);
        //                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
        //                {
        //                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
        //                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
        //                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
        //                }
        //            }
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        //System.Threading.Thread.Sleep(1000);
        //        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", IDPlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.SplashScreenMessage = string.Empty;
        //        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", IDPlantName, " Failed");
        //        if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
        //            if (!ERMCommon.Instance.FailedPlants.Contains(IDPlantName))
        //            {
        //                ERMCommon.Instance.FailedPlants.Add(IDPlantName);

        //                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
        //                {
        //                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
        //                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
        //                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
        //                }

        //            }
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        //System.Threading.Thread.Sleep(1000);
        //        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", IDPlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //    // GeosApplication.Instance.SplashScreenMessage = string.Empty;
        //    return TimeTrackingList;
        //}

        private static List<TimeTracking> ReturnFullTimeTrackingList_V2340(IERMService ERMService, IPLMService PLMService, ref List<TimeTracking> TimeTrackingList, string CurrencyNameFromSetting, string plantName, UInt32 idSite, ERMTimetrackingSite ProductionSiteDetail, List<GeosAppSetting> GeosAppSettingList, List<TimeTracking> TimeTrackingListold, string fromDate, string toDate
)
        {

            try
            {
                if (ERMCommon.Instance.FailedPlants == null)
                {
                    ERMCommon.Instance.FailedPlants = new List<string>();
                }
                if (ERMCommon.Instance.WarningFailedPlants == null)
                {
                    ERMCommon.Instance.WarningFailedPlants = "";
                }
                UInt32 ProductionIdSite = Convert.ToUInt32(ProductionSiteDetail.ProductionIdSite);
                UInt32 OriginalPlantIdSite= Convert.ToUInt32(ProductionSiteDetail.OriginalIdSite);
                //[GEOS2-4093][Rupali Sarode][26-12-2022]
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2340(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
                // ERMService = new ERMServiceController("localhost:6699");
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2360(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2370(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));


                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2380(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));   //[Origin and Production][gulab lakade][17 04 2023]
                //  TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2400(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));   //  //[gulab lakade][GEOS2-4494-batch][26 05 2023]
                // TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2420(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList));
                //   TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2430(idSite, IDPlantName, CurrencyNameFromSetting, GeosAppSettingList)); //[Pallavi Jadhav][14-09-2023][GEOS2-4818]
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2460(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
                // TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2500(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); //[gulab lakade][11 03 2024][GEOS2-5466]
                //  TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2540(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); //[pallavi jadhav][18 07 2024][GEOS2-5907]

                //   ERMService = new ERMServiceController("localhost:6699");
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2550(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); //[pallavi jadhav][18 07 2024][GEOS2-5907]

                //   TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2560(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); //[pallavi jadhav][16 09 2024][GEOS2-6081]
                // TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2580(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList)); //[pallavi jadhav][24 10 2024][GEOS2-5320]

                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2590(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList));   // [GEOS2-6646]][Daivshala Vighne][13-10-2024]
                //   TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2600(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList));   // [GEOS2-6646][pallavi jadhav][04-02-2025]
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2630(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, DateTime.ParseExact(fromDate, "dd/MM/yyyy", null),
                //         DateTime.ParseExact(toDate, "dd/MM/yyyy", null)));  // [pallavi jadhav][GEOS2-7060][25-03-2025] 
                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2640(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, DateTime.ParseExact(fromDate, "dd/MM/yyyy", null),
                //        DateTime.ParseExact(toDate, "dd/MM/yyyy", null)));  //[Pallavi.jadhav][16 05 2025][GEOS2-8124]

                //TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2660(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, DateTime.ParseExact(fromDate, "dd/MM/yyyy", null),
                //      DateTime.ParseExact(toDate, "dd/MM/yyyy", null)));  //[pallavi.jadhav][11 07 2025][GEOS2-8868] 

                TimeTrackingList.AddRange(ERMService.GetTimeTrackingBYPlant_V2670(OriginalPlantIdSite, ProductionIdSite, CurrencyNameFromSetting, GeosAppSettingList, DateTime.ParseExact(fromDate, "dd/MM/yyyy", null),
                    DateTime.ParseExact(toDate, "dd/MM/yyyy", null)));  //[GEOS2-8309][rani dhamankar][29 08 2025]

                if (TimeTrackingListold != null && TimeTrackingList != null)
                {
                    if (TimeTrackingList.Count > 0)
                    {
                        TimeTrackingListold.AddRange(TimeTrackingList);
                    }
                }

            }/*catch(Exception ex) { }*/
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed");
                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                    if (!ERMCommon.Instance.FailedPlants.Contains(ProductionSiteDetail.ProductionSite))
                    {
                        ERMCommon.Instance.FailedPlants.Add(ProductionSiteDetail.ProductionSite);
                        if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                        {
                            ERMCommon.Instance.IsShowFailedPlantWarning = true;
                            ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                            ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                        }
                    }
                //System.Threading.Thread.Sleep(1000);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed");
                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                    if (!ERMCommon.Instance.FailedPlants.Contains(ProductionSiteDetail.ProductionSite))
                    {
                        ERMCommon.Instance.FailedPlants.Add(ProductionSiteDetail.ProductionSite);
                        if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                        {
                            ERMCommon.Instance.IsShowFailedPlantWarning = true;
                            ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                            ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                        }
                    }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //System.Threading.Thread.Sleep(1000);
                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed");
                if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                    if (!ERMCommon.Instance.FailedPlants.Contains(ProductionSiteDetail.ProductionSite))
                    {
                        ERMCommon.Instance.FailedPlants.Add(ProductionSiteDetail.ProductionSite);

                        if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                        {
                            ERMCommon.Instance.IsShowFailedPlantWarning = true;
                            ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                            ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                        }

                    }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //System.Threading.Thread.Sleep(1000);
                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", ProductionSiteDetail.ProductionSite, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            // GeosApplication.Instance.SplashScreenMessage = string.Empty;
            return TimeTrackingList;
        }
        #endregion
        //static ObservableCollection<TrackingAccordian> templateWithTimeTracking;
        //public static ObservableCollection<TrackingAccordian> TemplateWithTimeTracking
        //{
        //    get { return templateWithTimeTracking; }
        //    set
        //    {
        //        templateWithTimeTracking = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TemplateWithTimeTracking"));
        //    }
        //}
        
        private static void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        private static void FillDeliveryweek(ObservableCollection<TrackingAccordian> TemplateWithTimeTracking, List<TimeTracking> TimeTrackingListnew)
        {
            try
            {
                List<TimeTracking> DeliverWeekGroup = new List<TimeTracking>();
                var temp = TimeTrackingListnew.GroupBy(x => x.DeliveryWeek)
                    .Select(group => new
                    {
                        DeliveryWeek = TimeTrackingListnew.FirstOrDefault(a => a.DeliveryWeek == group.Key).DeliveryWeek,

                        Count = TimeTrackingListnew.Where(b => b.DeliveryWeek == null).Count(),
                    }).ToList().OrderBy(i => i.DeliveryWeek);     ////GEOS2-4045 Gulab lakade Order by CW ASC

                foreach (var item in temp)
                {

                    DeliverWeekGroup = TimeTrackingListnew.Where(x => x.DeliveryWeek == item.DeliveryWeek).ToList();

                    TrackingAccordian templateWithTimeTracking = new TrackingAccordian();
                    var currentculter = CultureInfo.CurrentCulture;
                    var tempdateByPlanningDeliveryDate = (from dw in DeliverWeekGroup   //[GEOS2-4217] [Pallavi Jadhav] [27 02 2023]
                                                          select new
                                                          {
                                                              FDeliveryDate = dw.DeliveryDate,
                                                              dw.PlannedDeliveryDate,
                                                              DeliveryDate = (dw.PlannedDeliveryDate != null ? dw.PlannedDeliveryDate : dw.DeliveryDate)
                                                          }
                          ).Distinct().OrderBy(a => a.DeliveryDate).ToList();
                    List<DateTime?> tempdate = tempdateByPlanningDeliveryDate.Select(i => i.DeliveryDate).Distinct().ToList();
                    //List<DateTime?> tempdate = DeliverWeekGroup.Select(i => i.DeliveryDate).Distinct().ToList();
                    List<DateTime?> tempDateorderBy = tempdate.OrderBy(a => a.Value).ToList();   //// Gulab lakade Order by CW ASC 04-05-2023
                    if (templateWithTimeTracking.TimeTracking == null)
                        templateWithTimeTracking.TimeTracking = new List<string>();
                    int tempcount = 0;
                    String tempdeliveryweek = string.Empty;
                    foreach (DateTime item1 in tempDateorderBy)
                    {
                        string TempDate = item1.ToString("d", currentculter);
                        if (!templateWithTimeTracking.TimeTracking.Contains(TempDate))
                        {
                            int DeliveryWeekcount = TemplateWithTimeTracking.Where(a => a.deliwaryWeek.Contains(item.DeliveryWeek) && a.TimeTracking.ToList().Contains(TempDate)).ToList().Count;
                            if (DeliveryWeekcount == 0)
                            {
                                if (TemplateWithTimeTracking.Where(a => a.deliwaryWeek.Contains(item.DeliveryWeek)).Count() == 0)
                                {
                                    tempdeliveryweek = string.Empty;
                                    templateWithTimeTracking.TimeTracking.Add(TempDate);
                                    //templateWithTimeTracking = new TrackingAccordian();
                                    //templateWithTimeTracking.deliwaryWeek = item.DeliveryWeek + " (" + Updateitem.TimeTracking.Count() + ")";
                                    //templateWithTimeTracking.TimeTracking = new List<string>();
                                    //templateWithTimeTracking.TimeTracking.AddRange(Updateitem.TimeTracking);
                                    //templateWithTimeTracking.TimeTracking.Sort();   //// Gulab lakade Order by CW ASC 04-05-2023

                                    tempcount++;
                                }
                                else
                                {
                                    foreach (var Updateitem in (TemplateWithTimeTracking.Where(a => a.deliwaryWeek.Contains(item.DeliveryWeek))))
                                    {
                                        tempdeliveryweek = Updateitem.deliwaryWeek;
                                        Updateitem.TimeTracking.Add(TempDate);
                                        List<DateTime?> tempDate = new List<DateTime?>();
                                        //// Gulab lakade Order by CW ASC 04-05-2023
                                        foreach (var item11 in Updateitem.TimeTracking.ToList())
                                        {
                                            tempDate.Add(Convert.ToDateTime(item11));

                                        }
                                        Updateitem.TimeTracking = new List<string>();
                                        List<DateTime?> tempTrackingDate = tempDate.OrderBy(a => a.Value).ToList();
                                        List<string> Stringlist = new List<string>();
                                        foreach (DateTime item11 in tempTrackingDate)
                                        {
                                            string Dateforstring = item11.ToString("d", currentculter);
                                            Stringlist.Add(Convert.ToString(Dateforstring));

                                        }
                                        Updateitem.TimeTracking.AddRange(Stringlist);
                                        //end
                                        templateWithTimeTracking = new TrackingAccordian();
                                        templateWithTimeTracking.deliwaryWeek = item.DeliveryWeek + " (" + Updateitem.TimeTracking.Count() + ")";
                                        templateWithTimeTracking.copyDeliwaryWeek = item.DeliveryWeek;
                                        templateWithTimeTracking.TimeTracking = new List<string>();
                                        templateWithTimeTracking.TimeTracking.AddRange(Updateitem.TimeTracking);
                                        //templateWithTimeTracking.TimeTracking.Sort();  //// Gulab lakade Order by CW ASC 04-05-2023
                                        tempcount++;

                                    }
                                }

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(tempdeliveryweek))
                    {
                        TemplateWithTimeTracking.Where(f => tempdeliveryweek.Contains(f.deliwaryWeek)).ToList().ForEach(i => TemplateWithTimeTracking.Remove(i));
                    }

                    if (tempcount > 0)
                    {
                        if (string.IsNullOrEmpty(tempdeliveryweek))
                        {
                            templateWithTimeTracking.deliwaryWeek = item.DeliveryWeek + " (" + templateWithTimeTracking.TimeTracking.Count() + ")";
                            templateWithTimeTracking.copyDeliwaryWeek = item.DeliveryWeek;
                        }
                        int tempoldcount = 0;

                        TemplateWithTimeTracking.Add(templateWithTimeTracking);
                        //TemplateWithTimeTracking.OrderBy(a => a.deliwaryWeek);

                    }
                }


                ObservableCollection<TrackingAccordian> TemplateWithTimeTracking1 = new ObservableCollection<TrackingAccordian>();
                //TemplateWithTimeTracking.OrderBy(a => a.copyDeliwaryWeek).ToList().Sort(x => x.copyDeliwaryWeek && x.deliwaryWeek && );
                //TemplateWithTimeTracking1 = (from t in TemplateWithTimeTracking orderby t.copyDeliwaryWeek select new { t }).ToList();


                TemplateWithTimeTracking1.AddRange(TemplateWithTimeTracking.OrderBy(a => a.copyDeliwaryWeek).ToList());
                TemplateWithTimeTracking = new ObservableCollection<TrackingAccordian>();
                TemplateWithTimeTracking.AddRange(TemplateWithTimeTracking1);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDeliveryweek() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDeliveryweek() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        private class ReworkData
        {
            public Int64 IdOT;
            public Int64 IdOTItem;
            public Int64 IdDrawing;
            public Int64 IdCounterpart;
            public Int64 IdWorkbookOfCpProducts;//Aishwarya Ingale[Geos2-6034]
        }
    }
  
}
