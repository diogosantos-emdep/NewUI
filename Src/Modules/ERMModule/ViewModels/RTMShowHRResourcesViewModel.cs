using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Menu;
using DevExpress.Xpf.Scheduler.UI;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.OptimizedClass;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility.Text;
using Prism.Commands;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Data;
using DevExpress.Utils;
using Emdep.Geos.UI.Commands;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class RTMShowHRResourcesViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Service

        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private DataTable dtDashboard;
        private ObservableCollection<BandItem> bandsDashboard = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutDashboard;
        private List<string> customers;
        private string loadsxWorkstation;
        private string futureLoad;
        private string hRResources;
        private string idstages;
        private string jobDescriptionID;
        private List<GeosAppSetting> workStages = new List<GeosAppSetting>();
        private List<ERMWorkStageWiseJobDescription> workStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        private List<PlantOperationWeek> plantWeekList = new List<PlantOperationWeek>();
        private PlantOperationWeek plantWeek = new PlantOperationWeek();
        DataTable dtPlantOperation;
        private List<ERMEmployeePlantOperation> employeeplantOperationalList = new List<ERMEmployeePlantOperation>();
        private ERMEmployeePlantOperation employeeplantOperational = new ERMEmployeePlantOperation();
        private List<ERMEmployeePlantOperation> employeeplantOperationalListForRealTime = new List<ERMEmployeePlantOperation>(); //[GEOS2-4553][Rupali Sarode][19-06-2023]
        private List<ERMPlantOperationalPlanning> plantOperationalPlanning = new List<ERMPlantOperationalPlanning>();
        private DateTime hRResourceStartDate;
        private DateTime hRResourceEndDate;
        private List<RTMHRResourcesExpectedTime> rTMHRResourcesExpectedTimeList;
        private TableView FutureGridControl1;
        private GridControl FutureGridControl;
        private GridControl futureGridControlFilter;

        private List<ERMDeliveryVisualManagement> eRMDeliveryVisualManagementList_Cloned;
        private List<RTMHRResourcesExpectedTime> rTMHRResourcesExpectedTimeList_Cloned;
        private List<RTMFutureLoad> rTMFutureLoadList_Cloned;

        private List<DeliveryVisualManagementStages> eRMRTMHRResourcesStageList;

        #endregion Declaration

        #region Public Commands
        public ICommand CloseButtonCommand { get; set; }

        #endregion Public Commands

        #region property

        public DataTable DtDashboard
        {
            get { return dtDashboard; }
            set
            {
                dtDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDashboard"));
            }
        }

        public ObservableCollection<BandItem> BandsDashboard
        {
            get { return bandsDashboard; }
            set
            {
                bandsDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboard"));
            }
        }

        public DataTable DataTableForGridLayoutDashboard
        {
            get
            {
                return dataTableForGridLayoutDashboard;
            }
            set
            {
                dataTableForGridLayoutDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutDashboard"));
            }
        }
        public string LoadsxWorkstation
        {
            get
            {
                return loadsxWorkstation;
            }
            set
            {
                loadsxWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadsxWorkstation"));
            }
        }

        public string FutureLoad
        {
            get
            {
                return futureLoad;
            }
            set
            {
                futureLoad = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureLoad"));
            }
        }
        public string HRResources
        {
            get
            {
                return hRResources;
            }
            set
            {
                hRResources = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResources"));
            }
        }

        public string Idstages
        {
            get { return idstages; }
            set
            {
                idstages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idstages"));
            }
        }
        public List<GeosAppSetting> WorkStages
        {
            get { return workStages; }
            set
            {
                workStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStages"));
            }
        }

        public List<ERMWorkStageWiseJobDescription> WorkStageWiseJobDescription
        {
            get { return workStageWiseJobDescription; }
            set
            {
                workStageWiseJobDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStageWiseJobDescription"));
            }
        }
        public List<PlantOperationWeek> PlantWeekList
        {
            get { return plantWeekList; }
            set
            {
                plantWeekList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantWeekList"));
            }
        }

        public DataTable DtPlantOperation
        {
            get { return dtPlantOperation; }
            set
            {
                dtPlantOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtPlantOperation"));
            }
        }

        public List<ERMEmployeePlantOperation> EmployeeplantOperationallist
        {
            get { return employeeplantOperationalList; }
            set
            {
                employeeplantOperationalList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperationallist"));
            }
        }

        public ERMEmployeePlantOperation EmployeeplantOperational
        {
            get { return employeeplantOperational; }
            set
            {
                employeeplantOperational = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperational"));
            }
        }

        public List<ERMEmployeePlantOperation> EmployeeplantOperationalListForRealTime
        {
            get { return employeeplantOperationalListForRealTime; }
            set
            {
                employeeplantOperationalListForRealTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeplantOperationalListForRealTime"));
            }
        }

        public List<ERMPlantOperationalPlanning> PlantOperationalPlanning
        {
            get { return plantOperationalPlanning; }
            set
            {
                plantOperationalPlanning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantOperationalPlanning"));
            }
        }

        public List<PlantOperationProductionStage> PlantOperationProductionStage { get; set; }

        public DateTime HRResourceStartDate
        {
            get { return hRResourceStartDate; }
            set
            {
                hRResourceStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResourceStartDate"));
            }
        }
        public DateTime HRResourceEndDate
        {
            get { return hRResourceEndDate; }
            set
            {
                hRResourceEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HRResourceEndDate"));
            }
        }

        public List<RTMHRResourcesExpectedTime> RTMHRResourcesExpectedTimeList
        {
            get { return rTMHRResourcesExpectedTimeList; }
            set
            {
                rTMHRResourcesExpectedTimeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RTMHRResourcesExpectedTimeList"));
            }
        }

        public List<DeliveryVisualManagementStages> ERMRTMHRResourcesStageList
        {
            get
            {
                return eRMRTMHRResourcesStageList;
            }
            set
            {
                eRMRTMHRResourcesStageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMRTMHRResourcesStageList"));
            }
        }

        #endregion property

        #region  public event

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Event

        #region Constructor
        public RTMShowHRResourcesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RTMShowHRResourcesViewModel() ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));



                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor RTMShowHRResourcesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor RTMShowHRResourcesViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion Constructor

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
                //GetIdStageAndJobDescriptionByAppSetting();
                //GetWeekList();
                AddColumnsToDataTableWithBandsForHrResources();
                HRResources = System.Windows.Application.Current.FindResource("HRResources").ToString();

                FillDashboardHRResources();

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddColumnsToDataTableWithBandsForHrResources()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsForHrResources ...", category: Category.Info, priority: Priority.Low);
                //DtDashboard = new DataTable();//[GEOS2-4708][gulab lakade][25 07 2023]
                BandsDashboard = new ObservableCollection<BandItem>(); BandsDashboard.Clear();
                BandItem band0 = new BandItem() { BandName = "FirstRow", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                band0.Columns = new ObservableCollection<ColumnItem>();

                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "Week", Width = 120, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.CalenderWeek, Visible = true });

                BandsDashboard.Add(band0);
                DataTableForGridLayoutDashboard = new DataTable();
                DataTableForGridLayoutDashboard.Columns.Add("CalenderWeek", typeof(string));
                DataTableForGridLayoutDashboard.Columns.Add("GroupColor", typeof(bool));
                List<string> tempCurrentstage = new List<string>();


                #region //[pallavi jadhav][GEOS2-4869][9 27 2023]

                ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList.Where(a => a.StageCode != "{null}").ToList();
                ERMRTMHRResourcesStageList = ERMRTMHRResourcesStageList.GroupBy(x => x.IdStage, (key, group) => group.First()).ToList();

                #endregion
                if (ERMRTMHRResourcesStageList.Count > 0) //[pallavi jadhav][GEOS2-4869][9 27 2023]
                {
                    foreach (var item in ERMRTMHRResourcesStageList) //[pallavi jadhav][GEOS2-4869][9 27 2023]
                    {
                        if (Convert.ToString(item) != null)
                        {

                            tempCurrentstage.Add(Convert.ToString(item));
                            BandItem band1 = new BandItem()
                            {
                                BandName = Convert.ToString(item.StageCode),
                                BandHeader = Convert.ToString(item.StageCode),
                                Visible = true
                            };

                            band1.Columns = new ObservableCollection<ColumnItem>();


                            DataTableForGridLayoutDashboard.Columns.Add("HRExpected_" + Convert.ToString(item.IdStage), typeof(string));

                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRExpected_" + Convert.ToString(item.IdStage), HeaderText = "HRE", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.HRExpected });
                            DataTableForGridLayoutDashboard.Columns.Add("HRPlan_" + Convert.ToString(item.IdStage), typeof(string));
                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "HRPlan_" + Convert.ToString(item.IdStage), HeaderText = "HRP", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.HRPlan });

                            DataTableForGridLayoutDashboard.Columns.Add("ProductionExpectedTime_" + Convert.ToString(item.IdStage), typeof(string));
                            DataTableForGridLayoutDashboard.Columns.Add("Tempcolorflag_" + Convert.ToString(item.IdStage), typeof(bool));
                            DataTableForGridLayoutDashboard.Columns.Add("Tempcolor_" + Convert.ToString(item.IdStage), typeof(string));

                            switch (item.IdStage)
                            {

                                case 1:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime1 });

                                    break;

                                case 2:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime2 });
                                    break;

                                case 3:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime3 });
                                    break;

                                case 4:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime4 });
                                    break;

                                case 5:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime5 });
                                    break;

                                case 6:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime6 });
                                    break;

                                case 7:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime7 });
                                    break;

                                case 8:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime8 });
                                    break;

                                case 9:

                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime9 });
                                    break;

                                case 10:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime10 });
                                    break;

                                case 11:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime11 });
                                    break;

                                case 12:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime12 });
                                    break;

                                case 21:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime21 });
                                    break;

                                case 26:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime26 });
                                    break;

                                case 27:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime27 });
                                    break;
                                case 28:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime28 });
                                    break;

                                case 29:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime29 });
                                    break;

                                case 32:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime32 });
                                    break;
                                case 33:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime33 });
                                    break;
                                case 35:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime35 });
                                    break;

                                case 37:
                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime37 });
                                    break;

                                default:


                                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "ProductionExpectedTime_" + Convert.ToString(item.IdStage), HeaderText = "PET", Width = 70, Visible = true, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.ProductionExpectedTime });

                                    break;

                            }

                            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "IdStage_", HeaderText = "IdStage", Width = 0, IsVertical = false, RealTimeMonitorHRResourcesSetting = RealTimeMonitorHRResourcesColumnTemplateSelector.RealTimeMonitorHRResourcesSettingType.Hidden, Visible = false });
                            if (!DataTableForGridLayoutDashboard.Columns.Contains("IdStage_"))
                            {
                                DataTableForGridLayoutDashboard.Columns.Add("IdStage_" + Convert.ToString(item.IdStage), typeof(string));
                            }


                            BandsDashboard.Add(band1);

                        }
                    }
                }

                BandsDashboard = new ObservableCollection<BandItem>(BandsDashboard);

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsForHrResources executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsForHrResources() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDashboardHRResources()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardHRResources ...", category: Category.Info, priority: Priority.Low);

                List<string> tempCurrentstage = new List<string>();
                var currentculter = CultureInfo.CurrentCulture;
                string DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                DataTableForGridLayoutDashboard.Clear();
                DtPlantOperation = null;

                EmployeeplantOperationallist = EmployeeplantOperationallist.OrderBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek1 = EmployeeplantOperationallist.GroupBy(a => a.CalenderWeek).ToList();
                var TempCalenderWeek = TempCalenderWeek1.OrderBy(a => a.Key).ToList();
                Int64 TotalHRExpected = 0;
                float TotalReal = 0;

                foreach (var calendar in TempCalenderWeek)
                {
                    List<ERMEmployeePlantOperation> TempEmployeeplantOperationallist = new List<ERMEmployeePlantOperation>();
                    TempEmployeeplantOperationallist = EmployeeplantOperationallist.Where(a => a.CalenderWeek == Convert.ToString(calendar.Key)).ToList();

                    int count = 0;
                    count++;
                    DataRow dr = DataTableForGridLayoutDashboard.NewRow();
                    DateTime currentDate = DateTime.Now;
                    int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                    int year = DateTime.Now.Year;
                    string Week = year + "CW" + currentWeek;
                    foreach (var Employeedata in TempEmployeeplantOperationallist.GroupBy(a => a.IdEmployee))
                    {
                        
                        dr["CalenderWeek"] = Convert.ToString(calendar.Key);
                        dr["GroupColor"] = false;
                        if (calendar.Key == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }

                        List<ERMEmployeePlantOperation> TempEmployeelist = new List<ERMEmployeePlantOperation>();
                        TempEmployeelist = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key).ToList();

                        if (TempEmployeelist.Count > 0)
                        {
                            List<TempIdStage> IdStageList = new List<TempIdStage>();

                            foreach (ERMEmployeePlantOperation item in TempEmployeelist)
                            {

                                var TempIdStageList = WorkStageWiseJobDescription.Where(x => x.IdJobDescription.Contains(Convert.ToString(item.IdJobDescription))).ToList();
                                if (TempIdStageList.Count > 0)
                                {
                                    foreach (var itemIdStage in TempIdStageList)
                                    {

                                        var IsExist_IdStage = ERMRTMHRResourcesStageList.Where(x => x.IdStage == itemIdStage.IdWorkStage).ToList();
                                        if (IsExist_IdStage.Count > 0)
                                        {
                                            TempIdStage tempIdStage = new TempIdStage();
                                            tempIdStage.IdStage = Convert.ToInt32(itemIdStage.IdWorkStage);
                                            tempIdStage.IdJobDescription = Convert.ToInt32(item.IdJobDescription);
                                            tempIdStage.JobDescriptionUse = Convert.ToDecimal(item.JobDescriptionUsage);
                                            IdStageList.Add(tempIdStage);
                                        }
                                    }

                                }

                            }
                            if (IdStageList.Count > 0)
                            {

                                var tempIdStage = IdStageList.GroupBy(a => a.IdStage).ToList();
                                if (tempIdStage.Count > 0)
                                {
                                    foreach (var item in tempIdStage)
                                    {
                                        List<TempIdStage> TempidStageList = new List<TempIdStage>();
                                        TempidStageList = IdStageList.Where(a => a.IdStage == item.Key).ToList();
                                        if (TempidStageList.Count > 0)
                                        {
                                            int TempHRExpectedValue = 0;
                                            int TempHRPlanValue = 0;
                                            string TempTimeType = string.Empty;
                                            float TempRealTime = 0;

                                            decimal TempJobDescriptionUsage = 0;
                                            foreach (var item1 in TempidStageList)
                                            {
                                                var tempvalue = TempEmployeeplantOperationallist.Where(a => a.IdEmployee == Employeedata.Key && a.CalenderWeek == calendar.Key && a.IdJobDescription == item1.IdJobDescription).FirstOrDefault();

                                                if (TempJobDescriptionUsage == 0)
                                                {
                                                    TempJobDescriptionUsage = Convert.ToDecimal(item1.JobDescriptionUse);
                                                    if (tempvalue != null)
                                                    {
                                                        TempHRExpectedValue = Convert.ToInt32(tempvalue.HRExpected);
                                                        TempHRPlanValue = Convert.ToInt32(tempvalue.HRPlan);
                                                        if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                                                        {
                                                            var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                                                            if (tempReal != null)
                                                            {
                                                                TempRealTime = (float)(tempReal.TimeDifferenceInMinutes);
                                                            }
                                                        }

                                                    }

                                                }
                                                else
                                                {
                                                    TempJobDescriptionUsage = TempJobDescriptionUsage + Convert.ToDecimal(item1.JobDescriptionUse);
                                                    if (tempvalue != null)
                                                    {
                                                        TempHRExpectedValue = TempHRExpectedValue + Convert.ToInt32(tempvalue.HRExpected);
                                                        TempHRPlanValue = TempHRPlanValue + Convert.ToInt32(tempvalue.HRPlan);
                                                        if (tempvalue.EmployeePlantOperationalRealTimeList.Count() > 0)
                                                        {
                                                            var tempReal = tempvalue.EmployeePlantOperationalRealTimeList.Where(x => x.CalenderWeek == calendar.Key && x.IdEmployee == Employeedata.Key && x.Idstage == item1.IdStage).FirstOrDefault();
                                                            if (tempReal != null)
                                                            {
                                                                TempRealTime = TempRealTime + (float)(tempReal.TimeDifferenceInMinutes);
                                                            }
                                                        }
                                                    }
                                                }

                                            }


                                            string IdStage = "IdStage_" + Convert.ToString(item.Key);
                                            dr[IdStage] = Convert.ToString(item.Key);
                                            string HRExpected = "HRExpected_" + Convert.ToString(item.Key);
                                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRExpected]))) dr[HRExpected] = 0;

                                            dr[HRExpected] = Convert.ToInt32(dr[HRExpected]) + Convert.ToInt32(TempHRExpectedValue.ToString("0"));

                                            string HRPlan = "HRPlan_" + Convert.ToString(item.Key);

                                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRPlan]))) dr[HRPlan] = 0;
                                            dr[HRPlan] = Convert.ToInt32(dr[HRPlan]) + Math.Round(Convert.ToDouble(TempHRPlanValue), 0);


                                        }

                                    }
                                }
                            }

                        }
                    }

                    //Add Expected Time data
                    if (RTMHRResourcesExpectedTimeList != null)
                    {
                        foreach (RTMHRResourcesExpectedTime ExpectedTime in RTMHRResourcesExpectedTimeList.Where(i => i.DeliveryWeek == Convert.ToString(calendar.Key)).ToList())
                        {
                            if (ExpectedTime.RTMCurrentStageList != null)
                            {
                                for (int iItem = 0; iItem < ExpectedTime.RTMCurrentStageList.Count; iItem++)
                                {
                                    try
                                    {

                                        string expected = "ProductionExpectedTime_" + Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].IdStage);
                                        TimeSpan Tempexpected = TimeSpan.Parse("0");
                                        if (!string.IsNullOrEmpty(Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].Expected)) && Convert.ToString(ExpectedTime.RTMCurrentStageList[iItem].Expected) != "0")
                                        {
                                            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(ExpectedTime.RTMCurrentStageList[iItem].Expected));
                                            if (DataTableForGridLayoutDashboard.Columns.Contains(expected))
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(dr[expected])))
                                                    dr[expected] = TimeSpan.Parse("0");
                                                dr[expected] = TimeSpan.Parse(Convert.ToString(dr[expected])) + Tempexpected;
                                            }
                                        }
                                        else
                                        {
                                            Tempexpected = TimeSpan.FromMinutes(Convert.ToDouble(0.0));
                                            if (DataTableForGridLayoutDashboard.Columns.Contains(expected))
                                            {
                                                if (string.IsNullOrEmpty(Convert.ToString(dr[expected])))
                                                    dr[expected] = TimeSpan.Parse("0");
                                                dr[expected] = TimeSpan.Parse(Convert.ToString(dr[expected])) + Tempexpected;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                    }


                    if (ERMRTMHRResourcesStageList.Count > 0)
                    {
                        foreach (var item in ERMRTMHRResourcesStageList)
                        {
                            string tempcolorFlag = "Tempcolorflag_" + Convert.ToString(item.IdStage);
                            string HRPlan = "HRPlan_" + Convert.ToString(item.IdStage);
                            string ProductionExpectedTime = "ProductionExpectedTime_" + Convert.ToString(item.IdStage);
                            string TempColor = "Tempcolor_" + Convert.ToString(item.IdStage);
                            string HRExpected = "HRExpected_" + Convert.ToString(item.IdStage);

                            Int64 ProductionExpectedTimeHours = 0;
                            Int64 HRPlanHours = 0;

                            if (string.IsNullOrEmpty(Convert.ToString(dr[ProductionExpectedTime])))
                                ProductionExpectedTimeHours = 0;
                            else
                                ProductionExpectedTimeHours = TimeSpan.Parse(Convert.ToString(dr[ProductionExpectedTime])).Hours;

                            if (ProductionExpectedTimeHours == 0)
                                dr[ProductionExpectedTime] = "";
                            else
                                dr[ProductionExpectedTime] = ProductionExpectedTimeHours;

                            if (string.IsNullOrEmpty(Convert.ToString(dr[HRPlan])))
                                HRPlanHours = 0;
                            else
                                HRPlanHours = Convert.ToInt64(dr[HRPlan]);

                            if (ProductionExpectedTimeHours > HRPlanHours)
                            {
                                if (DataTableForGridLayoutDashboard.Columns.Contains(tempcolorFlag))
                                {
                                    dr[tempcolorFlag] = true;
                                    dr[TempColor] = "Red";

                                }
                            }
                            else
                            {
                                if (DataTableForGridLayoutDashboard.Columns.Contains(tempcolorFlag))
                                {
                                    dr[tempcolorFlag] = false;
                                    dr[TempColor] = "";
                                }
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr[HRPlan])))
                            {
                                if (Convert.ToInt64(dr[HRPlan]) == 0)
                                {
                                    dr[HRPlan] = "";
                                }
                            }

                            if (!String.IsNullOrEmpty(Convert.ToString(dr[HRExpected])))
                            {
                                if (Convert.ToInt64(dr[HRExpected]) == 0)
                                {
                                    dr[HRExpected] = "";
                                }
                            }
                        }
                    }
                    DataTableForGridLayoutDashboard.Rows.Add(dr);

                }
                DtPlantOperation = new DataTable();
                DtPlantOperation = DataTableForGridLayoutDashboard;


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardHRResources()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardHRResources() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private class TempIdStage
        {
            public Int32 IdStage;
            public Int32 IdJobDescription;
            public decimal JobDescriptionUse;
        }


        //private void GetIdStageAndJobDescriptionByAppSetting()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting ...", category: Category.Info, priority: Priority.Low);

        //        Idstages = string.Empty;
        //        jobDescriptionID = string.Empty;
        //        WorkStages = new List<GeosAppSetting>();
        //        WorkStages = WorkbenchStartUp.GetSelectedGeosAppSettings("98");
        //        if (workStages.Count > 0)
        //        {
        //            List<string> tempWorkStageList = new List<string>();
        //            foreach (var item in workStages)
        //            {
        //                string tempstring = Convert.ToString(item.DefaultValue.Replace('(', ' '));
        //                tempWorkStageList = Convert.ToString(tempstring).Split(')').ToList();
        //            }
        //            if (tempWorkStageList.Count > 0)
        //            {
        //                if (WorkStageWiseJobDescription != null)
        //                {
        //                    WorkStageWiseJobDescription = new List<ERMWorkStageWiseJobDescription>();
        //                }
        //                List<string> TempJobDescriptionID = new List<string>();
        //                foreach (var item in tempWorkStageList)
        //                {
        //                    List<string> tempIDStageList = Convert.ToString(item.Trim()).Split(';').ToList();
        //                    if (tempIDStageList.Count > 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(tempIDStageList[0]) && !string.IsNullOrEmpty(tempIDStageList[1]))
        //                        {
        //                            ERMWorkStageWiseJobDescription IDStage = new ERMWorkStageWiseJobDescription();
        //                            string tempstring = Convert.ToString(tempIDStageList[0].Replace(',', ' '));
        //                            IDStage.IdWorkStage = Convert.ToInt32(tempstring.Trim());
        //                            IDStage.IdJobDescription = new List<string>();
        //                            TempJobDescriptionID.AddRange(Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList());
        //                            IDStage.IdJobDescription = Convert.ToString(tempIDStageList[1].Trim()).Split(',').ToList();
        //                            WorkStageWiseJobDescription.Add(IDStage);
        //                            if (string.IsNullOrEmpty(Idstages))
        //                            {
        //                                Idstages = Convert.ToString(tempstring.Trim());
        //                            }
        //                            else
        //                            {
        //                                Idstages = Idstages + "," + Convert.ToString(tempstring.Trim());
        //                            }
        //                        }
        //                    }

        //                }
        //                if (TempJobDescriptionID.Count > 0)
        //                {
        //                    var Temp = TempJobDescriptionID.Distinct().ToList();
        //                    if (Temp.Count > 0)
        //                    {

        //                        foreach (var Tempitem in Temp)
        //                        {
        //                            if (string.IsNullOrEmpty(jobDescriptionID))
        //                            {
        //                                jobDescriptionID = Convert.ToString(Tempitem);
        //                            }
        //                            else
        //                            {
        //                                jobDescriptionID = jobDescriptionID + "," + Convert.ToString(Tempitem);
        //                            }
        //                        }

        //                    }
        //                }

        //            }
        //        }
        //        GeosApplication.Instance.Logger.Log("Method GetIdStageAndJobDescriptionByAppSetting() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in GetIdStageAndJobDescriptionByAppSetting() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void GetWeekList()
        //{
        //    try
        //    {

        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        //        GeosApplication.Instance.Logger.Log("Method GetWeekList ...", category: Category.Info, priority: Priority.Low);

        //        #region

        //        //CultureInfo CultureEnglish = new CultureInfo("en-GB");

        //        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
        //        CultureInfo CultureEnglish = new CultureInfo(culture.Name);
        //        DateTime TodaysDate;
        //        TodaysDate = DateTime.Now.Date;
        //        // TodaysDate = Convert.ToDateTime("01/01/2023");
        //        HRResourceStartDate = TodaysDate;

        //        DateTime TempFromDate = DateTime.Parse(Convert.ToString(TodaysDate), CultureEnglish, DateTimeStyles.AdjustToUniversal);

        //        int MinweekNum = CultureEnglish.Calendar.GetWeekOfYear(Convert.ToDateTime(TempFromDate).Date, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);

        //        var diff = Convert.ToDateTime(TempFromDate).Date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

        //        if (diff < 0)
        //        {
        //            diff += 7;
        //        }

        //        DateTime FirstDateOfWeek = Convert.ToDateTime(TempFromDate).Date.AddDays(-diff).Date;

        //        DateTime LastDateOfWeek = FirstDateOfWeek.AddDays(6);
        //        PlantWeekList = new List<PlantOperationWeek>();
            
        //        while (PlantWeekList.Count < 12)
        //        {
        //            plantWeek = new PlantOperationWeek();
        //            int Year = Convert.ToInt32(FirstDateOfWeek.Year);
        //            int weekNum = CultureEnglish.Calendar.GetWeekOfYear(FirstDateOfWeek, CalendarWeekRule.FirstFourDayWeek, culture.DateTimeFormat.FirstDayOfWeek);
        //            string CalendarWeek = Year + "CW" + weekNum.ToString("00");
        //            DateTime LastDate = FirstDateOfWeek.AddDays(6);
        //            plantWeek.CalenderWeek = CalendarWeek;
        //            plantWeek.FirstDateofweek = FirstDateOfWeek;
        //            plantWeek.LastDateofWeek = LastDate;
        //            PlantWeekList.Add(plantWeek);
        //            FirstDateOfWeek = LastDate.AddDays(1);
        //        }
              

        //        HRResourceEndDate = PlantWeekList[PlantWeekList.Count - 1].LastDateofWeek;

        //        #endregion
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method GetWeekList() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion Methods
    }

   

}
