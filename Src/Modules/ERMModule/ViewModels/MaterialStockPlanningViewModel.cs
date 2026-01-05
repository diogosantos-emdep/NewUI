using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Data.Common.PCM;
using System.Linq;
using System.Windows;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.ERM;
using System.ServiceModel;
using System.Collections.Generic;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using System.Windows.Media;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.XtraPrinting;
using System.Data;
using DevExpress.Data;
using DevExpress.Xpf.Core.Serialization;
using System.Collections.ObjectModel;
using System.IO;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.CodeView;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
  public  class MaterialStockPlanningViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Services

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
   
          
        #endregion

        #region Declaration
        Visibility isCalendarVisible;
        private  DateTime planningDate;
        private List<ERMArticleStockPlanning> eRMArticleStockPlanningList;
        private Int32 idWarehouse;
        //[gulab lakade][20 02 2025]
        private DateTime currentweeklstDate;
        private DateTime fromDate;
        private bool isColumnChooserVisibleForGrid;//[GEOS2-7027][gulab lakade][24 02 2025
        TableView tableViewInstance;//[GEOS2-7027][gulab lakade][24 02 2025]
        private string myFilterString;//[GEOS2-6886][rani dhamankar][26 02 2025]
        public string ERM_MaterialStockPlanningGrid_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_MaterialStockPlanningGrid_Setting.Xml";
        private DateTime maxDateVisibleForDatepicker;  // [GEOS2-9402][gulab lakade][18 11 2025]
        private DateTime toDate;  // [GEOS2-9402][gulab lakade][18 11 2025]

        #endregion
        #region Properties
        public Visibility IsCalendarVisible
        {
            get
            {
                return isCalendarVisible;
            }

            set
            {
                isCalendarVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalendarVisible"));
            }
        }
        public DateTime PlanningDate
        {
            get
            {
                return planningDate;
            }

            set
            {
                planningDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlanningDate"));
               
            }
        }

        public List<ERMArticleStockPlanning> ERMArticleStockPlanningList
        {
            get
            {
                return eRMArticleStockPlanningList;
            }

            set
            {
                eRMArticleStockPlanningList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMArticleStockPlanningList"));
            }
        }

        public Int32 IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdWarehouse"));
            }
        }
        //[gulab lakade][20 02 2025]
        public DateTime CurrentweeklstDate
        {
            get
            {
                return currentweeklstDate;
            }

            set
            {
                currentweeklstDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentweeklstDate"));

            }
        }
        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDate"));

            }
        }
        //[GEOS2-6886][rani dhamankar][26 02 2025]
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        #region  [GEOS2-9402][gulab lakade][18 11 2025]

        public DateTime MaxDateVisibleForDatepicker
        {
            get
            {
                return maxDateVisibleForDatepicker;
            }

            set
            {
                maxDateVisibleForDatepicker = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxDateVisibleForDatepicker"));

            }
        }
        public DateTime ToDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));

            }
        }
        #endregion 
        #endregion
        #region [GEOS2-6886][Pallavi jadhav][14 02 2025]
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        #region [GEOS2-7027][gulab lakade][24 02 2025
        public bool IsColumnChooserVisibleForGrid
        {
            get
            {
                return isColumnChooserVisibleForGrid;
            }

            set
            {
                isColumnChooserVisibleForGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColumnChooserVisibleForGrid"));
            }
        }
        #endregion

        #endregion

        #region ICommands
        public ICommand RefreshMaterialStockPlanningCommand { get; set; }
        public ICommand PrintMaterialStockPlanningCommand { get; set; }
        public ICommand ExportMaterialStockPlanningCommand { get; set; }
        public ICommand ChangeWarehouseCommand { get; set; }
          public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ArticleOutgoingStockPopupWindowCommand { get; set; }//[rani dhamankar][17-02-2025][GEOS2-6887]
        public ICommand ArticleIncomingStockPopupWindowCommand { get; set; }//[rani dhamankar][24-02-2025][GEOS2-6888]
        #region [GEOS2-7027][gulab lakade][24 02 2025
        public ICommand TableViewLoadedCommand { get; set; }

        #endregion 
        #endregion

        #region Constructor
        public MaterialStockPlanningViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MaterialStockPlanningViewModel()...", category: Category.Info, priority: Priority.Low);
             ChangeWarehouseCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeWarehouseCommandAction);
                RefreshMaterialStockPlanningCommand = new RelayCommand(new Action<object>(RefreshMaterialStockPlanningCommandAction));
                PrintMaterialStockPlanningCommand = new RelayCommand(new Action<object>(PrintMaterialStockPlanningCommandAction));
                ExportMaterialStockPlanningCommand = new RelayCommand(new Action<object>(ExportMaterialStockPlanningCommandAction));
                ApplyCommand = new RelayCommand(new Action<object>(ApplyCommandAction));
                CancelCommand = new RelayCommand(new Action<object>(CancelCommandAction));
                ArticleOutgoingStockPopupWindowCommand = new DelegateCommand<object>(ArticleOutgoingStockPopupWindowCommandAction);//[rani dhamankar][17-02-2025][GEOS2-6887]
                ArticleIncomingStockPopupWindowCommand = new DelegateCommand<object>(ArticleIncomingStockPopupWindowCommandAction);//[rani dhamankar][24-02-2025][GEOS2-6888]
                #region [GEOS2-7027][gulab lakade][24 02 2025
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);

                #endregion
                GeosApplication.Instance.Logger.Log("Constructor MaterialStockPlanningViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor MaterialStockPlanningViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                PlanningDate = new DateTime();
                FromDate = new DateTime();
                FromDate= DateTime.Today;//[gulab lakade][20 02 2025]
                //PlanningDate = DateTime.Today;//[gulab lakade][20 02 2025]
                ToDate= new DateTime();//[GEOS2-9402][gulab lakade][18 11 2025]
                ToDate = DateTime.Today;//[GEOS2-9402][gulab lakade][18 11 2025]
                SetlastDayOfWeek();
                FillWarehouse(); //[GEOS2-6886][Pallavi jadhav][14 02 2025]
                //FillWarehouseList();//[GEOS2-6886][Pallavi jadhav][14 02 2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }

        private void SetlastDayOfWeek()
        {
            GeosApplication.Instance.Logger.Log("Method SetlastDayOfWeek ...", category: Category.Info, priority: Priority.Low);
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
            //DateTime today = PlanningDate;

            //int daysUntilSunday = DayOfWeek.Sunday - today.DayOfWeek;

            //if (daysUntilSunday < 0)
            //{
            //    daysUntilSunday += 7;
            //}
            DateTime today = DateTime.Today;
            #region [GEOS2-9402][gulab lakade][18 11 2025]
            // Get the last day of the current week (Sunday)
            //DateTime lastDayOfWeek = today.AddDays(DayOfWeek.Saturday - today.DayOfWeek);
            DateTime lastDayOfWeek = today.AddMonths(-6);

            PlanningDate = lastDayOfWeek;
            CurrentweeklstDate = lastDayOfWeek;
            MaxDateVisibleForDatepicker= DateTime.Today;   // [GEOS2-9402][gulab lakade][18 11 2025]
            #endregion 
            // ERMCommon.Instance.PlanningDate = PlanningDate;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method SetlastDayOfWeek() executed successfully", category: Category.Info, priority: Priority.Low);

        }
        #region Warehouse
        private void ChangeWarehouseCommandAction(object obj)
        {

            try {

                GeosApplication.Instance.Logger.Log("Method ChangeWarehouseCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillWarehouseList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeWarehouseCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeWarehouseCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Refresh
        private void RefreshMaterialStockPlanningCommandAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method RefreshMaterialStockPlanningCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                //MyFilterString = string.Empty;
                //FillWarehouseList();
                #region[GEOS2-6886][rani dhamankar][26-02-2025]
                FillWarehouseList(); //[GEOS2-6886][Pallavi jadhav][14 02 2025]
                //detailView.ShowTotalSummary = true;
                detailView.SearchString = string.Empty;
                //gridControl.FilterString = string.Empty;
                //gridControl.TotalSummary.Clear();
                //gridControl.UpdateLayout();
                //gridControl.RefreshData();
                //gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                // new GridSummaryItem() {
                //    SummaryType = SummaryItemType.Count,
                //    Alignment = GridSummaryItemAlignment.Left,
                //    DisplayFormat = "Total Count : {0}"
                //}});
                #endregion

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshMaterialStockPlanningCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshMaterialStockPlanningCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshMaterialStockPlanningCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshMaterialStockPlanningCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        #endregion

        #region Print and Export to excel

        private void PrintMaterialStockPlanningCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintPlanningDateReviewAction()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintProductionTimelineCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintProductionTimelineCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void ExportMaterialStockPlanningCommandAction(object obj)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Material Stock Planning List";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();

                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    activityTableView.ShowTotalSummary = true;
                    GeosApplication.Instance.Logger.Log("Method PrintProductionTimelineCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintProductionTimelineCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }



        #endregion

        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //MenuFlyout menu = (MenuFlyout)obj;

                //SetlastDayOfWeek();
                //menu.IsOpen = false;
                //ERMCommon.Instance.PlanningDate = PlanningDate;//[gulab lakade][20 02 2025]
                FillWarehouseList();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
               
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

       

        private void CancelCommandAction(object obj)
        {

            try
            {
                MenuFlyout menu = (MenuFlyout)obj;
                menu.IsOpen = false;
            }
            catch (Exception ex)
            {
                
            }
        }

        //[GEOS2-6886][Pallavi jadhav][14 02 2025]
        private void FillWarehouse()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouse ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                string SelectedPlantName = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                // Data.Common.Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == SelectedPlantName);
                Data.Common.Company usrDefault = ERMCommon.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == SelectedPlantName);// [GEOS2-8698][rani dhamankar][16-07-2025]
                ERMCommon.Instance.WarehouseList = new List<ERM_Warehouses>();
                ERMArticleStockPlanningList = new List<ERMArticleStockPlanning>();
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == SelectedPlantName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                ERMService = new ERMServiceController(serviceurl);//testing
                //ERMService = new ERMServiceController("localhost:6699");//testing
                #region  [GEOS2-9404][gulab lakade][18 11 2025]
                //ERMCommon.Instance.WarehouseList = ERMService.GetWarehousesByIDSite_V2610(usrDefault.IdCompany);
                ERMCommon.Instance.WarehouseList = ERMService.GetWarehousesByIDSite_V2690(usrDefault.IdCompany);
                // ERMCommon.Instance.Selectedwarehouse = ERMCommon.Instance.WarehouseList.FirstOrDefault();
                ERMCommon.Instance.Selectedwarehouse = ERMCommon.Instance.WarehouseList.Where(a=>a.SiteName== SelectedPlantName).FirstOrDefault();
              
                #endregion 
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillWarehouse() executed successfully", category: Category.Info, priority: Priority.Low);


            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeWarehouseCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-6886][Pallavi jadhav][14 02 2025]
        private void FillWarehouseList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWarehouseList ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                string PlantName = Convert.ToString(ERMCommon.Instance.Selectedwarehouse.Name);
                try
                {
                    ERMArticleStockPlanningList = new List<ERMArticleStockPlanning>();
                    IdWarehouse = Convert.ToInt32(ERMCommon.Instance.Selectedwarehouse.IdWarehouse);
                    //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == ERMCommon.Instance.Selectedwarehouse.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    #region [GEOS2-9404][gulab lakade][18 11 2025]
                    string serviceurl = string.Empty;
                    if (!string.IsNullOrEmpty(ERMCommon.Instance.Selectedwarehouse.SiteName))
                    {
                        serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == ERMCommon.Instance.Selectedwarehouse.SiteName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    }
                    else
                    {
                        serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    }
                    //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                    #endregion 
                    ERMService = new ERMServiceController(serviceurl);
                    //ERMService = new ERMServiceController("localhost:6699");
                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + PlantName;
                    //ERMArticleStockPlanningList.AddRange(ERMService.GetArticleStockPlanningList_V2610(IdWarehouse, PlanningDate, ERMCommon.Instance.PlanningDate));
                    //ERMArticleStockPlanningList = ERMService.GetArticleStockPlanningList_V2610(IdWarehouse, FromDate, PlanningDate);
                    ERMArticleStockPlanningList = ERMService.GetArticleStockPlanningList_V2690(IdWarehouse, PlanningDate, ToDate);  // [GEOS2-9123][gulab lakade][20 11 2025]
                    ERMArticleStockPlanningList = ERMArticleStockPlanningList.OrderBy(a => a.ProjectedStock).ToList();
                    ERMArticleStockPlanningList = new List<ERMArticleStockPlanning>(ERMArticleStockPlanningList.ToList()); 
                   // ERMArticleStockPlanningList.AddRange(ERMService.GetArticleStockPlanningList_V2610(IdWarehouse,FromDate, PlanningDate));
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method FillWarehouseList() executed successfully", category: Category.Info, priority: Priority.Low);

                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                        if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                        {
                            ERMCommon.Instance.FailedPlants.Add(PlantName);
                            if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                            {
                                ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                            }
                        }
                    System.Threading.Thread.Sleep(1000);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                        if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                        {
                            ERMCommon.Instance.FailedPlants.Add(PlantName);
                            if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                            {
                                ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                            }
                        }
                    System.Threading.Thread.Sleep(1000);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", PlantName, " Failed");
                    if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                        if (!ERMCommon.Instance.FailedPlants.Contains(PlantName))
                        {
                            ERMCommon.Instance.FailedPlants.Add(PlantName);
                            if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                            {
                                ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                            }
                        }
                    System.Threading.Thread.Sleep(1000);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", PlantName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                }


                GeosApplication.Instance.Logger.Log("Method FillWarehouseList() executed successfully", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillWarehouseList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

         #endregion
        #region [rani dhamankar][17-02-2025][GEOS2-6887]
        private void ArticleOutgoingStockPopupWindowCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ArticleOutgoingStockPopupWindowCommandAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                TableView detailView = (TableView)obj;
                ERMArticleStockPlanning focusedRowData = (ERMArticleStockPlanning)detailView.FocusedRow;
                string StageName = string.Empty;
                string cellExpectedValue = string.Empty;
                var gridView = detailView.Grid.View as TableView;
                int idArticle = 0;
                if (focusedRowData != null)
                {
                    idArticle = focusedRowData.IdArticle;
                }


                OutgoingMaterialStockPlanningView outgoingMaterialStockPlanningView = new OutgoingMaterialStockPlanningView();
                OutgoingMaterialStockPlanningViewModel outgoingMaterialStockPlanningViewModel = new OutgoingMaterialStockPlanningViewModel();
                EventHandler handle = delegate { outgoingMaterialStockPlanningView.Close(); };
                outgoingMaterialStockPlanningViewModel.RequestClose += handle;
                //outgoingMaterialStockPlanningViewModel.Init(idArticle, IdWarehouse, FromDate, PlanningDate);
                outgoingMaterialStockPlanningViewModel.Init(idArticle, IdWarehouse, PlanningDate, ToDate);  // [GEOS2-9402][gulab lakade][18 11 2025] 
                outgoingMaterialStockPlanningView.DataContext = outgoingMaterialStockPlanningViewModel;
                var ownerInfo = (obj as FrameworkElement);
                outgoingMaterialStockPlanningView.Owner = Window.GetWindow(ownerInfo);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
              
                outgoingMaterialStockPlanningView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method ArticleOutgoingStockPopupWindowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ArticleOutgoingStockPopupWindowCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region [GEOS2-7027][gulab lakade][24 02 2025
        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                int visibleFalseColumnsCount = 0;

                if (File.Exists(ERM_MaterialStockPlanningGrid_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ERM_MaterialStockPlanningGrid_SettingFilePath);
                    GridControl gridControlInstance = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)gridControlInstance.View;
                    this.tableViewInstance = tableView;

                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ERM_MaterialStockPlanningGrid_SettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;


                foreach (GridColumn column in gridControl.Columns)
                {

                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseColumnsCount++;
                    }
                }

                if (visibleFalseColumnsCount > 0)
                {
                    IsColumnChooserVisibleForGrid = true;
                }
                else
                {
                    IsColumnChooserVisibleForGrid = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
              
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                 new GridSummaryItem() {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}"
                }});



                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_MaterialStockPlanningGrid_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsColumnChooserVisibleForGrid = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ERM_MaterialStockPlanningGrid_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.DependencyProperty == TreeListControl.FilterStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 

        #region [rani dhamankar][24-02-2025][GEOS2-6889]
        private void ArticleIncomingStockPopupWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleIncomingStockPopupWindowCommandAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                TableView detailView = (TableView)obj;
                ERMArticleStockPlanning focusedRowData = (ERMArticleStockPlanning)detailView.FocusedRow;
                string StageName = string.Empty;
                string cellExpectedValue = string.Empty;
                var gridView = detailView.Grid.View as TableView;
                int idArticle = 0;
                if (focusedRowData != null)
                {
                    idArticle = focusedRowData.IdArticle;
                }
                IncomingMaterialStockPlanningView incomingMaterialStockPlanningView = new IncomingMaterialStockPlanningView();
                IncomingMaterialStockPlanningViewModel incomingMaterialStockPlanningViewModel = new IncomingMaterialStockPlanningViewModel();
                EventHandler handle = delegate { incomingMaterialStockPlanningView.Close(); };
                incomingMaterialStockPlanningViewModel.RequestClose += handle;
                incomingMaterialStockPlanningView.DataContext = incomingMaterialStockPlanningViewModel;
                //incomingMaterialStockPlanningViewModel.Init(idArticle, IdWarehouse, FromDate, PlanningDate);
                incomingMaterialStockPlanningViewModel.Init(idArticle, IdWarehouse, PlanningDate, ToDate); // [GEOS2-9402][gulab lakade][18 11 2025] 
                incomingMaterialStockPlanningView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method ArticleIncomingStockPopupWindowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ArticleIncomingStockPopupWindowCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }



}
