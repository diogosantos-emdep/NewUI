using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Xpf.Gauges;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.WindowsUI;
using System.Collections.ObjectModel;
using DevExpress.Data;
using System.Globalization;
using System.Data;
using System.Windows.Markup;
using System.Xml;
using DevExpress.XtraPrinting;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PlantDeliveryAnalysisViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        #endregion

        #region Declaration

        private string userSettingsKey = "ERM_Plant_Delivery_Analysis ";
        public string PlantDeliveryAnalysisGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_PlantDeliveryAnalysis.Xml";
        public string PlantDeliveryAnalysisFilterSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ERM_PlantDeliveryAnalysisFilterSetting.Xml";

        int isButtonStatus;
        Visibility isCalendarVisible;
        private ObservableCollection<PlantDeliveryAnalysis> plantDeliveryAnalysisList;
        private List<PlantDeliveryAnalysis> plantDeliveryAnalysisTempList;
        DateTime fromDate;
        DateTime toDate;
        private string amount;
        private bool isBusy;
        private bool isPeriod;
        private Duration _currentDuration;
        DateTime startDate;
        DateTime endDate;

        XYDiagram2D diagram = new XYDiagram2D();
        private ChartControl chartControl;
        private DataTable dt = new DataTable();
        private DataTable graphDataTable;

        #region [GEOS2-5113][Rupali Sarode][06-12-2023]

        private List<string> TempXMLRegionsList;
        private List<string> TempXMLOTStatussList;
        private List<long> TempXMLCategory1List;
        private List<long> TempXMLCategory2List;
        private List<string> TempXMLTemplatesList;
        private List<string> TempXMLOTItemStatussList;
        private List<string> TempXMLCustomerGroupsList;
        private List<string> TempXMLCustomerPlantsList;

        #endregion [GEOS2-5113][Rupali Sarode][06-12-2023]

        XYDiagram2D diagramSample = new XYDiagram2D();
        private ChartControl chartControlSample;
        private double amountt;
        private string currencySymbolForTotal;

        //rajashri GEOS2=5916
        private ChartControl chartControlOnTime;
        private ChartControl chartControlAvg;
        private DataTable dtontime = new DataTable();
        private DataTable dtavg = new DataTable();
        private DataTable onTimeDeliveryDataTable;
        private DataTable graphAvgdelivery;
        private bool isViewSupervisorERM; //[Pallavi jadhav][GEOS2-5910][17-07-2024]
        #endregion

        #region  public Commands
        public ICommand PeriodCommand { get; set; }
        public ICommand ApplyCommand { get; set; }
        public ICommand PeriodCustomRangeCommand { get; set; }
        public ICommand RefreshPlantDeliveryAnalysisCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }

        public ICommand PieChartSalesTeamLoadCommand { get; set; }
        public ICommand PieChartDrawSeriesPointCommand { get; set; }

        public ICommand CircularGaugeControlLoadCommand { get; set; }
        public ICommand ChangeOTStatusCommand { get; set; }
        public ICommand ChangeTemplateCommand { get; set; }

        public ICommand ChangeOTItemStatusCommand { get; set; }
        public ICommand ChangeCategory1Command { get; set; }
        public ICommand ChangeCategory2Command { get; set; }
        public ICommand ChangeCustomerGroupCommand { get; set; }
        public ICommand ChangeCustomerPlantCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ActionLoadCommand { get; set; }
        public ICommand ChartDashboardSaleCustomDrawCrosshairCommand { get; set; }
        public ICommand PlantDeliveryAnalysisGridControlUnloadedCommand { get; set; }

        public ICommand ChartDaysPOtoShipmentLoadActionCommand { get; set; }

        public ICommand CustomSummaryCommand { get; set; } // [GEOS2-5130][Rupali Sarode][19-12-2023]
        public ICommand LayoutUpdatedCommand { get; set; } // [GEOS2-5130][Rupali Sarode][19-12-2023]
        public ICommand ShowPOListDialogWindowCommand { get; set; }  //[GEOS2-5223][Rupali Sarode][22-02-2023]
        public ICommand ShowOnTimeDeliveryDialogWindowCommand { get; set; }  //[GEOS2-5223][Rupali Sarode][22-02-2023]
        public ICommand ShowAvgDeliveryDaysDialogWindowCommand { get; set; }  //[GEOS2-5223][Rupali Sarode][22-02-2023]
        public ICommand ShowAverageDeliveryDaysXPlantDialogWindowCommand { get; set; }  //[GEOS2-5223][Rupali Sarode][22-02-2023]
        public ICommand ShowAverageDaysPOShipmentDialogWindowCommand { get; set; }  //[GEOS2-5223][Rupali Sarode][22-02-2023]
        public ICommand OnDeliveryLoadCommand1 { get; set; }//rajashri GEOS2=5916
        public ICommand ActionLoadCommandAvgDelivery { get; set; }//rajashri GEOS2=5916

        public ICommand ExportplantdeliveryCommand { get; set; }//Aishwarya Ingale[Geos2-6431]
        #endregion

        #region Property
        public int IsButtonStatus
        {
            get
            {
                return isButtonStatus;
            }

            set
            {
                isButtonStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsButtonStatus"));
            }
        }

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

        public ObservableCollection<PlantDeliveryAnalysis> PlantDeliveryAnalysisList
        {
            get
            {
                return plantDeliveryAnalysisList;
            }

            set
            {
                plantDeliveryAnalysisList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantDeliveryAnalysisList"));
            }
        }

        public List<PlantDeliveryAnalysis> PlantDeliveryAnalysisTempList
        {
            get
            {
                return plantDeliveryAnalysisTempList;
            }

            set
            {
                plantDeliveryAnalysisTempList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantDeliveryAnalysisTempList"));
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
        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }
        List<string> failedPlants;
        public List<string> FailedPlants
        {
            get { return failedPlants; }
            set { failedPlants = value; }
        }
        Boolean isShowFailedPlantWarning;
        string warningFailedPlants;
        public Boolean IsShowFailedPlantWarning
        {
            get { return isShowFailedPlantWarning; }
            set
            {
                isShowFailedPlantWarning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowFailedPlantWarning"));
            }
        }

        public string WarningFailedPlants
        {
            get { return warningFailedPlants; }
            set
            {
                warningFailedPlants = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarningFailedPlants"));
            }
        }

        private List<PlantDeliveryAnalysis> regionList;
        public List<PlantDeliveryAnalysis> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }

        private List<PlantDeliveryAnalysis> oTStatusList;
        public List<PlantDeliveryAnalysis> OTStatusList
        {
            get
            {
                return oTStatusList;
            }

            set
            {
                oTStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTStatusList"));
            }
        }

        private List<PlantDeliveryAnalysis> category1List;
        public List<PlantDeliveryAnalysis> Category1List
        {
            get
            {
                return category1List;
            }

            set
            {
                category1List = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Category1List"));
            }
        }

        private List<PlantDeliveryAnalysis> category2List;
        public List<PlantDeliveryAnalysis> Category2List
        {
            get
            {
                return category2List;
            }

            set
            {
                category2List = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Category2List"));
            }
        }

        private List<PlantDeliveryAnalysis> customerGroupList;
        public List<PlantDeliveryAnalysis> CustomerGroupList
        {
            get
            {
                return customerGroupList;
            }

            set
            {
                customerGroupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroupList"));
            }
        }

        private List<PlantDeliveryAnalysis> customerPlantList;
        public List<PlantDeliveryAnalysis> CustomerPlantList
        {
            get
            {
                return customerPlantList;
            }

            set
            {
                customerPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlantList"));
            }
        }

        private List<object> selectedOTStatus;
        public List<object> SelectedOTStatus
        {
            get { return selectedOTStatus; }
            set
            {
                selectedOTStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTStatusNotEmpty"));//Aishwarya Ingale[Geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<PlantDeliveryAnalysis> eRMPlantDeliveryAnalysisList_Cloned;
        public List<PlantDeliveryAnalysis> ERMPlantDeliveryAnalysisList_Cloned
        {
            get
            {
                return eRMPlantDeliveryAnalysisList_Cloned;
            }
            set
            {
                eRMPlantDeliveryAnalysisList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMPlantDeliveryAnalysisList_Cloned"));
            }
        }

        private List<PlantDeliveryAnalysis> category1List_Cloned;
        public List<PlantDeliveryAnalysis> Category1List_Cloned
        {
            get
            {
                return category1List_Cloned;
            }

            set
            {
                category1List_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Category1List_Cloned"));
            }
        }
        private List<PlantDeliveryAnalysis> category2List_Cloned;
        public List<PlantDeliveryAnalysis> Category2List_Cloned
        {
            get
            {
                return category2List_Cloned;
            }

            set
            {
                category2List_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Category2List_Cloned"));
            }
        }
        private List<object> selectedCategory1;
        public List<object> SelectedCategory1
        {
            get { return selectedCategory1; }
            set
            {
                selectedCategory1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCategory1"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCategory1NotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }
        private List<object> selectedCategory2;
        public List<object> SelectedCategory2
        {
            get { return selectedCategory2; }
            set
            {
                selectedCategory2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCategory2"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCategory2NotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<PlantDeliveryAnalysis> customerGroup_Cloned;
        public List<PlantDeliveryAnalysis> CustomerGroup_Cloned
        {
            get
            {
                return customerGroup_Cloned;
            }

            set
            {
                customerGroup_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerGroup_Cloned"));
            }
        }
        private List<object> selectedCustomerGroup;
        public List<object> SelectedCustomerGroup
        {
            get { return selectedCustomerGroup; }
            set
            {
                selectedCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerGroup"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerGroupNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }


        private List<PlantDeliveryAnalysis> customerPlant_Cloned;
        public List<PlantDeliveryAnalysis> CustomerPlant_Cloned
        {
            get
            {
                return customerPlant_Cloned;
            }

            set
            {
                customerPlant_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlant_Cloned"));
            }
        }

        private List<object> selectedCustomerPlant;
        public List<object> SelectedCustomerPlant
        {
            get { return selectedCustomerPlant; }
            set
            {
                selectedCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPlant"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedCustomerPlantsNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<PlantDeliveryAnalysis> oTStatus_Cloned;
        public List<PlantDeliveryAnalysis> OTStatus_Cloned
        {
            get
            {
                return oTStatus_Cloned;
            }

            set
            {
                oTStatus_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTStatus_Cloned"));
            }
        }

        private List<PlantDeliveryAnalysis> region_Cloned;
        public List<PlantDeliveryAnalysis> Region_Cloned
        {
            get
            {
                return region_Cloned;
            }

            set
            {
                region_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Region_Cloned"));
            }
        }

        private List<object> selectedRegion;
        public List<object> SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedRegionNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public bool IsPeriod
        {
            get { return isPeriod; }
            set { isPeriod = value; }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        private string aVGONTimeDataCurrentYear;

        public string AVGONTimeDataCurrentYear
        {
            get
            {
                return aVGONTimeDataCurrentYear;
            }

            set
            {
                aVGONTimeDataCurrentYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataCurrentYear"));
            }
        }
        private string aVGONTimeDataCurrentYearScaleValue;
        public string AVGONTimeDataCurrentYearScaleValue
        {
            get
            {
                return aVGONTimeDataCurrentYearScaleValue;
            }

            set
            {
                aVGONTimeDataCurrentYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataCurrentYearScaleValue"));
            }
        }

        private List<PlantDeliveryAnalysis> oTItemStatusList;
        public List<PlantDeliveryAnalysis> OTItemStatusList
        {
            get
            {
                return oTItemStatusList;
            }

            set
            {
                oTItemStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemStatusList"));
            }
        }

        private List<PlantDeliveryAnalysis> oTItemStatusList_Cloned;
        public List<PlantDeliveryAnalysis> OTItemStatusList_Cloned
        {
            get
            {
                return oTItemStatusList_Cloned;
            }

            set
            {
                oTItemStatusList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTItemStatusList_Cloned"));
            }
        }

        private List<object> selectedOTItemStatus;
        public List<object> SelectedOTItemStatus
        {
            get { return selectedOTItemStatus; }
            set
            {
                selectedOTItemStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOTItemStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedOTItemStatusNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<PlantDeliveryAnalysis> templateList;
        public List<PlantDeliveryAnalysis> TemplateList
        {
            get
            {
                return templateList;
            }

            set
            {
                templateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateList"));
            }
        }

        private List<PlantDeliveryAnalysis> templateList_Cloned;
        public List<PlantDeliveryAnalysis> TemplateList_Cloned
        {
            get
            {
                return templateList_Cloned;
            }

            set
            {
                templateList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateList_Cloned"));
            }
        }

        private List<object> selectedTemplate;
        public List<object> SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplate"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedTemplateFilterNotEmpty"));//Aishwarya Ingale[geos2-5749]
                UpdateColor();//Aishwarya Ingale[Geos2-5749]
            }
        }

        private List<PlantDeliveryAnalysis> oNTimeDeliveryANDAVGDeliveryList;
        public List<PlantDeliveryAnalysis> ONTimeDeliveryANDAVGDeliveryList
        {
            get
            {
                return oNTimeDeliveryANDAVGDeliveryList;
            }

            set
            {
                oNTimeDeliveryANDAVGDeliveryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ONTimeDeliveryANDAVGDeliveryList"));
            }
        }

        private List<PlantDeliveryAnalysis> oNTimeDeliveryANDAVGDeliveryList_Cloned;
        public List<PlantDeliveryAnalysis> ONTimeDeliveryANDAVGDeliveryList_Cloned
        {
            get
            {
                return oNTimeDeliveryANDAVGDeliveryList_Cloned;
            }

            set
            {
                oNTimeDeliveryANDAVGDeliveryList_Cloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ONTimeDeliveryANDAVGDeliveryList_Cloned"));
            }
        }

        private string lastYearStartDate;
        public string LastYearStartDate
        {
            get
            {
                return lastYearStartDate;
            }

            set
            {
                lastYearStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastYearStartDate"));
            }
        }

        private string currentYearStartDate;
        public string CurrentYearStartDate
        {
            get
            {
                return currentYearStartDate;
            }

            set
            {
                currentYearStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentYearStartDate"));
            }
        }


        private string aVGONTimeDataLastYear;

        public string AVGONTimeDataLastYear
        {
            get
            {
                return aVGONTimeDataLastYear;
            }

            set
            {
                aVGONTimeDataLastYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataLastYear"));
            }
        }
        private string aVGONTimeDataLastYearScaleValue;
        public string AVGONTimeDataLastYearScaleValue
        {
            get
            {
                return aVGONTimeDataLastYearScaleValue;
            }

            set
            {
                aVGONTimeDataLastYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGONTimeDataLastYearScaleValue"));
            }
        }

        private string aVGDeliveryDataLastYearScaleValue;
        public string AVGDeliveryDataLastYearScaleValue
        {
            get
            {
                return aVGDeliveryDataLastYearScaleValue;
            }

            set
            {
                aVGDeliveryDataLastYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataLastYearScaleValue"));
            }
        }

        private string aVGDeliveryDataCurrentYearScaleValue;
        public string AVGDeliveryDataCurrentYearScaleValue
        {
            get
            {
                return aVGDeliveryDataCurrentYearScaleValue;
            }

            set
            {
                aVGDeliveryDataCurrentYearScaleValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataCurrentYearScaleValue"));
            }
        }

        private string aVGDeliveryDataCurrentYear;

        public string AVGDeliveryDataCurrentYear
        {
            get
            {
                return aVGDeliveryDataCurrentYear;
            }

            set
            {
                aVGDeliveryDataCurrentYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataCurrentYear"));
            }
        }

        private string aVGDeliveryDataLastYear;

        public string AVGDeliveryDataLastYear
        {
            get
            {
                return aVGDeliveryDataLastYear;
            }

            set
            {
                aVGDeliveryDataLastYear = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AVGDeliveryDataLastYear"));
            }
        }

        IList<Currency> currencies;
        public IList<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }

        private string totalAVGONTimeTotal;
        public string TotalAVGONTimeTotal
        {
            get
            {
                return totalAVGONTimeTotal;
            }

            set
            {
                totalAVGONTimeTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalAVGONTimeTotal"));
            }
        }


        private string totalAVGDeliveryTotal;
        public string TotalAVGDeliveryTotal
        {
            get
            {
                return totalAVGDeliveryTotal;
            }

            set
            {
                totalAVGDeliveryTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalAVGDeliveryTotal"));
            }
        }

        private string onTimeDelivery;
        public string OnTimeDelivery
        {
            get
            {
                return onTimeDelivery;
            }

            set
            {
                onTimeDelivery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OnTimeDelivery"));
            }
        }

        private string avgDeliveryDays;
        public string AvgDeliveryDays
        {
            get
            {
                return avgDeliveryDays;
            }

            set
            {
                avgDeliveryDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AvgDeliveryDays"));
            }
        }

        private string amountWithCurrency;
        public string AmountWithCurrency
        {
            get
            {
                return amountWithCurrency;
            }

            set
            {
                amountWithCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountWithCurrency"));
            }
        }

        private string currencySymbolFromSetting;
        public string CurrencySymbolFromSetting
        {
            get
            {
                return currencySymbolFromSetting;
            }

            set
            {
                currencySymbolFromSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencySymbolFromSetting"));
                if (!string.IsNullOrEmpty(CurrencySymbolFromSetting))
                {
                    ERMCommon.Instance.CurrencySymbolFromSetting = CurrencySymbolFromSetting;
                }
            }
        }

        private string endValue;
        public string EndValue
        {
            get
            {
                return endValue;
            }

            set
            {
                endValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndValue"));
            }
        }

        private string endValueOnTime;
        public string EndValueOntime
        {
            get
            {
                return endValueOnTime;
            }

            set
            {
                endValueOnTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndValueOntime"));
            }
        }


        public ObservableCollection<UI.Helper.Summary> TotalSummary { get; private set; }


        public DataTable GraphDataTable
        {
            get { return graphDataTable; }
            set { graphDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphDataTable")); }
        }

        private List<PlantDeliveryAnalysis> plantNameList;
        public List<PlantDeliveryAnalysis> PlantNameList
        {
            get
            {
                return plantNameList;
            }

            set
            {
                plantNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantNameList"));
            }
        }



        private List<int> yearsList;
        public List<int> YearsList
        {
            get
            {
                return yearsList;
            }

            set
            {
                yearsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearsList"));
            }
        }

        private List<int> yearsAverageDayPOtoShipmentList;
        public List<int> YearsAverageDayPOtoShipmentList
        {
            get
            {
                return yearsAverageDayPOtoShipmentList;
            }

            set
            {
                yearsAverageDayPOtoShipmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearsAverageDayPOtoShipmentList"));
            }
        }


        private List<POGoAheadAndSampleDays> pOGoAheadAndSampleDaysList;
        public List<POGoAheadAndSampleDays> POGoAheadAndSampleDaysList
        {
            get
            {
                return pOGoAheadAndSampleDaysList;
            }

            set
            {
                pOGoAheadAndSampleDaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POGoAheadAndSampleDaysList"));
            }
        }

        private DataTable pOtoShipmentDataTable;
        public DataTable POtoShipmentDataTable
        {
            get { return pOtoShipmentDataTable; }
            set { pOtoShipmentDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("POtoShipmentDataTable")); }
        }

        public string CurrencySymbolForTotal
        {
            get
            {
                return currencySymbolForTotal;
            }

            set
            {
                currencySymbolForTotal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencySymbolForTotal"));
            }
        }
        #region Aishwarya Ingale[Geos2-5749]
        public bool IsSelectedRegionNotEmpty
        {
            get
            {
                return SelectedRegion != null && SelectedRegion.Count > 0 && SelectedRegion.Count != RegionList.Count;
            }

        }

        public bool IsSelectedOTStatusNotEmpty
        {
            get
            {
                return SelectedOTStatus != null && SelectedOTStatus.Count > 0 && SelectedOTStatus.Count != OTStatusList.Count;
            }

        }

        public bool IsSelectedCategory1NotEmpty
        {
            get
            {
                return SelectedCategory1 != null && SelectedCategory1.Count > 0 && SelectedCategory1.Count != Category1List.Count;
            }

        }

        public bool IsSelectedCategory2NotEmpty
        {
            get
            {
                return SelectedCategory2 != null && SelectedCategory2.Count > 0 && SelectedCategory2.Count != Category2List.Count;
            }

        }

        public bool IsSelectedTemplateFilterNotEmpty
        {
            get
            {
                return SelectedTemplate != null && SelectedTemplate.Count > 0 && SelectedTemplate.Count != TemplateList.Count;
            }

        }

        public bool IsSelectedOTItemStatusNotEmpty
        {
            get
            {
                return SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0 && SelectedOTItemStatus.Count != OTItemStatusList.Count;
            }

        }

        public bool IsSelectedCustomerGroupNotEmpty
        {
            get
            {
                return SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0 && SelectedCustomerGroup.Count != CustomerGroupList.Count;
            }

        }

        public bool IsSelectedCustomerPlantsNotEmpty
        {
            get
            {
                return SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0 && SelectedCustomerPlant.Count != CustomerPlantList.Count;
            }

        }

        //Aishwarya Ingale[Geos2-5749]
        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Color"));
            }
        }

        //Aishwarya Ingale[Geos2-5749]
        private void UpdateColor()
        {
            if (IsSelectedRegionNotEmpty || IsSelectedOTStatusNotEmpty || IsSelectedCategory1NotEmpty || IsSelectedCategory2NotEmpty || IsSelectedTemplateFilterNotEmpty || IsSelectedOTItemStatusNotEmpty || IsSelectedCustomerGroupNotEmpty || IsSelectedCustomerPlantsNotEmpty)
            {
                Color = "Black";
            }
            else
            {
                Color = "White"; // or any other default color
            }
        }

        #region rajashri GEOS2-5916
        private double totalPercentage;

        public double TotalPercentage
        {
            get
            {
                return totalPercentage;
            }

            set
            {
                totalPercentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentage"));
            }
        }
        private double totalPercentage1;

        public double TotalPercentage1
        {
            get
            {
                return totalPercentage1;
            }

            set
            {
                totalPercentage1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalPercentage1"));
            }
        }
        private int lastYearDate;
        public int LastYearDate
        {
            get
            {
                return lastYearDate;
            }

            set
            {
                lastYearDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastYearDate"));
            }
        }

        private bool isgreater;

        public bool Isgreater
        {
            get
            {
                return isgreater;
            }

            set
            {
                isgreater = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Isgreater"));
            }
        }
        private bool isgreaterAvgdelivery;

        public bool IsgreaterAvgdelivery
        {
            get
            {
                return isgreaterAvgdelivery;
            }

            set
            {
                isgreaterAvgdelivery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsgreaterAvgdelivery"));
            }
        }
        private int currentYearDate;
        public int CurrentYearDate
        {
            get
            {
                return currentYearDate;
            }

            set
            {
                currentYearDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentYearDate"));
            }
        }
        public DataTable OnTimeDeliveryDataTable
        {
            get { return onTimeDeliveryDataTable; }
            set { onTimeDeliveryDataTable = value; OnPropertyChanged(new PropertyChangedEventArgs("OnTimeDeliveryDataTable")); }
        }
        public DataTable GraphAvgdelivery
        {
            get { return graphAvgdelivery; }
            set { graphAvgdelivery = value; OnPropertyChanged(new PropertyChangedEventArgs("GraphAvgdelivery")); }
        }

        #region //[Pallavi jadhav][GEOS2-5910][17-07-2024]
        public bool IsViewSupervisorERM
        {
            get { return isViewSupervisorERM; }
            set
            {
                isViewSupervisorERM = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POtoShipmentDataTable"));
            }
        }
        #endregion
        #endregion
        #endregion

        public virtual bool DialogResult { get; protected set; }//Aishwarya Ingale[Geos2-6431]
        public virtual string ResultFileName { get; protected set; }//Aishwarya Ingale[Geos2-6431]
        #endregion

        #region Constructor
        public PlantDeliveryAnalysisViewModel()
        {
            try
            {
                PeriodCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCommandAction);
                PeriodCustomRangeCommand = new DevExpress.Mvvm.DelegateCommand<object>(PeriodCustomRangeCommandAction);
                ApplyCommand = new DevExpress.Mvvm.DelegateCommand<object>(ApplyCommandAction);
                CancelCommand = new DevExpress.Mvvm.DelegateCommand<object>(CancelCommandAction);
                RefreshPlantDeliveryAnalysisCommand = new DevExpress.Mvvm.DelegateCommand<object>(RefreshPlantDeliveryAnalysis);
                ChangePlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangePlantCommandAction);
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                // PieChartSalesTeamLoadCommand = new DelegateCommand<RoutedEventArgs>(PieChartSalesTeamLoadCommandAction);
                ChangeOTStatusCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeOTStatusCommandAction);
                ChangeTemplateCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeTemplateCommandAction);
                ChangeOTItemStatusCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeOTItemStatusCommandAction);
                ChangeCategory1Command = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCategory1CommandAction);
                ChangeCategory2Command = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCategory2CommandAction);
                ChangeCustomerGroupCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerGroupCommandAction);
                ChangeCustomerPlantCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeCustomerPlantCommandAction);
                ChangeRegionCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChangeRegionCommandAction);
                ActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartLoadAction);
                ChartDashboardSaleCustomDrawCrosshairCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDashboardSaleCustomDrawCrosshairCommandAction);
                //CircularGaugeControlLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(CircularGaugeControlLoadCommandAction);
                PlantDeliveryAnalysisGridControlUnloadedCommand = new DelegateCommand<object>(PlantDeliveryAnalysisGridControlUnloadedCommandAction);
                ChartDaysPOtoShipmentLoadActionCommand = new DevExpress.Mvvm.DelegateCommand<object>(ChartDaysPOtoShipmentLoadAction);
                ShowPOListDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowPOListDialogWindowCommandAction);
                ShowOnTimeDeliveryDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowOnTimeDeliveryDialogWindowCommandAction);
                ShowAvgDeliveryDaysDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowAvgDeliveryDaysDialogWindowCommandAction);
                ShowAverageDeliveryDaysXPlantDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowAverageDeliveryDaysXPlantDialogWindowCommandAction);
                ShowAverageDaysPOShipmentDialogWindowCommand = new DevExpress.Mvvm.DelegateCommand<object>(ShowAverageDaysPOShipmentDialogWindowCommandAction);
                OnDeliveryLoadCommand1 = new DevExpress.Mvvm.DelegateCommand<object>(OnTimeDeliveryChartLoadAction);
                ActionLoadCommandAvgDelivery = new DevExpress.Mvvm.DelegateCommand<object>(AvgDeliveryChartLoadAction1);
                ExportplantdeliveryCommand = new DevExpress.Mvvm.DelegateCommand<object>(ExportplantdeliveryCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor PlantDeliveryAnalysisViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DashboardViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Method

        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                #region //[Pallavi jadhav][GEOS2-5910][17-07-2024]
                if (GeosApplication.Instance.IsViewSupervisorERM)
                {
                    IsViewSupervisorERM = false;
                }
                else
                {
                    IsViewSupervisorERM = true;
                }

                #endregion

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                int CurrentYear = DateTime.Now.Year;
                int LastYear = DateTime.Now.Year - 1;
                //start[GEOS2-5442][gulab lakade][01 03 2024]
                //OnTimeDelivery = "ON TIME DELIVERY "+ LastYear+"V"+CurrentYear;

                //AvgDeliveryDays = "AVG DELIVERY DAYS " + LastYear + "V" + CurrentYear;
                OnTimeDelivery = "ON TIME DELIVERY " + LastYear + "VS" + CurrentYear;
                LastYearDate = LastYear;
                CurrentYearDate = CurrentYear;
                AvgDeliveryDays = "AVG DELIVERY DAYS " + LastYear + "VS" + CurrentYear;
                //end[GEOS2-5442][gulab lakade][01 03 2024]
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 12, 31);
                LastYearStartDate = TempLastYearStartDate.ToString("dd/MM/yyyy");
                CurrentYearStartDate = TempCurrentYearStartDate.ToString("dd/MM/yyyy");
                Currencies = CrmStartUp.GetCurrencies();
                setDefaultPeriod();
                StartDate = DateTime.Today.Date;
                EndDate = DateTime.Today.Date;

                int year = DateTime.Now.Year;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);
                FillPlantDeliveryAnalysis();

                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList.Where(item => (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date <= EndToDate && (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date >= StartFromDate).ToList());


                //double sumOfAmount = PlantDeliveryAnalysisList.Sum(item =>Convert.ToDouble(item.Amounts)); 
                //  AmountWithCurrency = CurrencySymbolFromSetting + sumOfAmount.ToString();
                // string formattedSum = CurrencySymbolFromSetting + sumOfAmount.ToString("C");
                // AmountWithCurrency = formattedSum;

                ERMPlantDeliveryAnalysisList_Cloned = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList);



                FillRegion();
                FillOTStatus();
                FillTemplate();
                FillOTItemStatus();
                FillCategory1();
                FillCategory2();
                FillCustomerGroup();
                FillCustomerPlant();

                ReadXmlFile();  //[GEOS2-5113][Rupali Sarode][06-12-2023]
                FillAllselectedFilter();
                // FillONTimeDeliveryANDAVGDelivery();
                FillDataONTime();
                FillAVGDelivery();

                #region DAYS AFTER THE AGREED DELIVERY DATE Chart
                FindYear();



                CreateTable();
                #endregion

                #region “Average Days PO to Shipment” chart 
                FindYearforAverageDayPOtoShipment();
                CreateDaysPOtoShipmentTable();
                #endregion

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }

        }
        private void RefreshPlantDeliveryAnalysis(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails ...", category: Category.Info, priority: Priority.Low);


                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                FillPlantDeliveryAnalysis();
                //  ERMPlantDeliveryAnalysisList_Cloned = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList);
                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned);
                // FillONTimeDeliveryANDAVGDelivery();
                FillRegion();
                FillOTStatus();
                FillTemplate();
                FillOTItemStatus();
                FillCategory1();
                FillCategory2();
                FillCustomerGroup();
                FillCustomerPlant();
                ReadXmlFile();  //[GEOS2-5113][Rupali Sarode][06-12-2023]
                FillAllselectedFilter();
                FillDataONTime();
                FillAVGDelivery();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method RefreshDashboardDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDashboardDetails() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void setDefaultPeriod()
        {

            int year = DateTime.Now.Year;
            DateTime StartFromDate = new DateTime(year, 1, 1);
            DateTime EndToDate = new DateTime(year, 12, 31);
            //FromDate = StartFromDate.ToString("dd/MM/yyyy");
            //ToDate = EndToDate.ToString("dd/MM/yyyy");
            FromDate = StartFromDate;
            ToDate = EndToDate;
        }
        //private void FillPlantDeliveryAnalysis()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis ...", category: Category.Info, priority: Priority.Low);
        //        if (PlantDeliveryAnalysisList == null)
        //        {
        //            PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
        //        }
        //        PlantDeliveryAnalysisList.AddRange(ERMService.GetPendingWorkorders_V2450());
        //        GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch(Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlantDeliveryAnalysis()", category: Category.Exception, priority: Priority.Low);
        //    }
        //}


        private void FillPlantDeliveryAnalysis()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                          // ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;
                            //var CurrencySymbolFromSetting = String.Empty;
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            CurrencySymbolFromSetting = Currencies.Where(i => i.Name == CurrencyNameFromSetting).FirstOrDefault().Symbol;

                            //ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2450(CurrencyNameFromSetting, CurrencySymbolFromSetting,IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null)));
                            // ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2460(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null))); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]

                            //  ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2540(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null))); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]

                            ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2560(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                         DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null))); // [Aishwarya Ingale][18-09-2024][GEOS2-6431]

                            //Aishwarya Ingale[Geos2-6431]
                            if (ONTimeDeliveryANDAVGDeliveryList != null)
                            {
                                ONTimeDeliveryANDAVGDeliveryList.ToList().ForEach(i =>
                                i.Amount = (i.Amounts == null || i.Amounts == 0.0)
                                 ? string.Empty
                                : Convert.ToString(CurrencySymbolFromSetting + " " + Math.Round(i.Amounts, 2).ToString("n", CultureInfo.CurrentCulture)));
                            }

                        }

                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillPlantDeliveryAnalysis() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList);
                    ONTimeDeliveryANDAVGDeliveryList_Cloned = new List<PlantDeliveryAnalysis>();
                    ONTimeDeliveryANDAVGDeliveryList_Cloned = ONTimeDeliveryANDAVGDeliveryList.ToList();


                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantDeliveryAnalysis()", category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ChangePlantCommandAction(object obj)
        {
            try
            {


                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>();

                if (obj == null)
                {


                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                        //return;
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                        var TempSelectedPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedPlant = new List<object>();

                        foreach (var tmpPlant in (dynamic)TempSelectedPlant)
                        {
                            TmpSelectedPlant.Add(tmpPlant);
                        }

                        ERMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();
                        ERMCommon.Instance.SelectedAuthorizedPlantsList = TmpSelectedPlant;

                        if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                        {
                            try
                            {
                                GetPlantDeliveryAnalysis(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                                FillRegion();
                                FillOTStatus();
                                FillTemplate();
                                FillOTItemStatus();
                                FillCategory1();
                                FillCategory2();
                                FillCustomerGroup();
                                FillCustomerPlant();
                                ReadXmlFile();  //[GEOS2-5113][Rupali Sarode][06-12-2023]
                                FillAllselectedFilter();
                                //     FillONTimeDeliveryANDAVGDelivery();
                                FillDataONTime();
                                FillAVGDelivery();
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                            }
                        }
                        else
                        {

                        }


                    }
                }


                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void GetPlantDeliveryAnalysis(List<object> SelectedPlant, DateTime FromDate, DateTime ToDate)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetDVManagementProduction ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    PlantDeliveryAnalysisTempList = new List<PlantDeliveryAnalysis>();
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>();
                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl); // [Rupali Sarode][08-06-2023]

                       //    ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;
                            // var CurrencySymbolFromSetting = String.Empty;
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            CurrencySymbolFromSetting = Currencies.Where(i => i.Name == CurrencyNameFromSetting).FirstOrDefault().Symbol;
                            //PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2450(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null)));
                            //PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2460(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null))); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]
                            //PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2460(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, FromDate,
                            //ToDate));

                            // PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2540(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null)));

                            ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2560(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null))); // [Aishwarya Ingale][18-09-2024][GEOS2-6431]

                            //Aishwarya Ingale[Geos2-6431]
                            if (ONTimeDeliveryANDAVGDeliveryList != null)
                                ONTimeDeliveryANDAVGDeliveryList.ToList().ForEach(i =>
                               i.Amount = (i.Amounts == 0.0)
                              ? string.Empty
                            : Convert.ToString(CurrencySymbolFromSetting + " " + Math.Round(i.Amounts, 2).ToString("n", CultureInfo.CurrentCulture)));
                        }

                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillPlantDeliveryAnalysis() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                    }

                    //ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisTempList);
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList);
                  //  PlantDeliveryAnalysisTempList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList);

                    //    DateTime todate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
                    //  DateTime fromdate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                    DateTime todate = ToDate;
                    DateTime fromdate = FromDate;
                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList.Where(item => (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date <= todate && (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date >= fromdate).ToList());
                    ERMPlantDeliveryAnalysisList_Cloned = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList);
                    ONTimeDeliveryANDAVGDeliveryList_Cloned = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList);

                    FillRegion();
                    FillOTStatus();
                    FillTemplate();
                    FillOTItemStatus();
                    FillCategory1();
                    FillCategory2();
                    FillCustomerGroup();
                    FillCustomerPlant();
                    ReadXmlFile();  //[GEOS2-5113][Rupali Sarode][06-12-2023]
                    FillAllselectedFilter();
                    FillDataONTime();
                    FillAVGDelivery();
                    FindYear();
                    CreateTable();
                    FindYearforAverageDayPOtoShipment();
                    CreateDaysPOtoShipmentTable();
                   // CreateTable();
                    //CreateDaysPOtoShipmentTable();
                    //    PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisTempList);



                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantDeliveryAnalysis()", category: Category.Exception, priority: Priority.Low);
            }

        }


        private void GetPeriodDeliveryAnalysis(List<object> SelectedPlant, DateTime FromDate, DateTime ToDate)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method GetDVManagementProduction ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    PlantDeliveryAnalysisTempList = new List<PlantDeliveryAnalysis>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl); // [Rupali Sarode][08-06-2023]

                         //   ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;
                            //  var CurrencySymbolFromSetting = String.Empty;
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }
                            CurrencySymbolFromSetting = Currencies.Where(i => i.Name == CurrencyNameFromSetting).FirstOrDefault().Symbol;
                            //PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2450(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //   DateTime.ParseExact(ToDate, "dd/MM/yyyy", null)));

                            //PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2460(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, DateTime.ParseExact(FromDate, "dd/MM/yyyy", null),
                            //  DateTime.ParseExact(ToDate, "dd/MM/yyyy", null))); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]


                         //   PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2460(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, FromDate, ToDate)); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]

                          //  PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2540(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, FromDate, ToDate)); // [Pallavi Jadhav][24-11-2023][GEOS2-5053]

                            PlantDeliveryAnalysisTempList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2560(CurrencyNameFromSetting, CurrencySymbolFromSetting, IdSite, FromDate, ToDate)); // [Aishwarya Ingale][18-09-2024][GEOS2-6431]

                            //Aishwarya Ingale[Geos2-6431]
                            if (PlantDeliveryAnalysisTempList != null)
                                PlantDeliveryAnalysisTempList.ToList().ForEach(i =>
                                 i.Amount = (i.Amounts == 0.0)
                                 ? string.Empty
                               : Convert.ToString(CurrencySymbolFromSetting + " " + Math.Round(i.Amounts, 2).ToString("n", CultureInfo.CurrentCulture)));

                        }

                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillPlantDeliveryAnalysis() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                    }

                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(PlantDeliveryAnalysisTempList);

                    //DateTime todate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
                    //DateTime fromdate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
                    //   PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisTempList.Where(item => (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date <= todate && (item.GoAheadDate != null ? item.GoAheadDate : item.PODate).Value.Date >= fromdate).ToList());
                    ERMPlantDeliveryAnalysisList_Cloned = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList);

                    FillRegion();
                    FillOTStatus();
                    FillTemplate();
                    FillOTItemStatus();
                    FillCategory1();
                    FillCategory2();
                    FillCustomerGroup();
                    FillCustomerPlant();
                    ReadXmlFile();  //[GEOS2-5113][Rupali Sarode][06-12-2023]
                    FillAllselectedFilter();
                    FillDataONTime();
                    FillAVGDelivery();
                    FindYear();
                    CreateTable();
                    FindYearforAverageDayPOtoShipment();
                    CreateDaysPOtoShipmentTable();
                    //    PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisTempList);



                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillPlantDeliveryAnalysis() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantDeliveryAnalysis()", category: Category.Exception, priority: Priority.Low);
            }

        }
        private void PieChartSalesTeamLoadCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PieChartSalesTeamLoadCommandAction ...", category: Category.Info, priority: Priority.Low);

                ChartControl chartControl = (ChartControl)(obj.OriginalSource);
                chartControl.BeginInit();

                PieSeries2D series = (PieSeries2D)chartControl.Diagram.Series[0];
                series.AnimationAutoStartMode = AnimationAutoStartMode.SetStartState;
                Pie2DFlyInAnimation pie2DFlyInAnimation = new Pie2DFlyInAnimation();
                pie2DFlyInAnimation.Duration = new TimeSpan(0, 0, 3);
                pie2DFlyInAnimation.PointOrder = PointAnimationOrder.Random;
                series.PointAnimation = pie2DFlyInAnimation;

                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method PieChartSalesTeamLoadCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PieChartSalesTeamLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillRegion()
        {
            try
            {



                RegionList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedRegion = new PlantDeliveryAnalysis();
                    selectedRegion.OTStatus = item.OTStatus;
                    selectedRegion.Region = item.Region;
                    //   selectedRegion.IdProductCategory = item.IdProductCategory;
                    RegionList.Add(selectedRegion);

                }


                var TempPlantDeliveryAnalysisList1 = (from a in RegionList
                                                      select new
                                                      {
                                                          a.Region


                                                      }).Distinct().ToList();
                RegionList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.Region = item1.Region;
                    selectedvalue.OfferStatusType = PlantDeliveryAnalysisList.Where(a => a.Region == item1.Region).FirstOrDefault().OfferStatusType;
                    //   var temp = PlantDeliveryAnalysisList.Where(a => a.Region == item1.Region).ToList();
                    //foreach (var item in temp)
                    //{
                    //    selectedvalue.OfferStatusType = item.OfferStatusType;

                    //    selectedvalue = new PlantDeliveryAnalysis();
                    //}
                    //        selectedvalue.IdProductCategory = PlantDeliveryAnalysisList.Where(a => a.OTStatus == item1.OTStatus).FirstOrDefault().IdProductCategory;


                    RegionList.Add(selectedvalue);

                }
                RegionList = new List<PlantDeliveryAnalysis>(RegionList);
                Region_Cloned = new List<PlantDeliveryAnalysis>();
                Region_Cloned = RegionList.ToList();
                // List<PlantDeliveryAnalysis> TEmpRegion = new List<PlantDeliveryAnalysis>(RegionList.Select(a => a.Region).Distinct().ToList());


                //List<PlantDeliveryAnalysis> tempRegion = RegionList.Select(a => a.Region).Distinct();
                // RegionList = new List<PlantDeliveryAnalysis>(tempRegion);

            }
            catch (Exception ex)
            {

            }
        }

        private void FillOTStatus()
        {
            try
            {



                OTStatusList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedOTStatus = new PlantDeliveryAnalysis();
                    selectedOTStatus.OfferStatusType = item.OfferStatusType;
                    //     selectedOTStatus.IdProductCategory = item.IdProductCategory;
                    OTStatusList.Add(selectedOTStatus);

                }
                //List<PlantDeliveryAnalysis> TempPlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
                //TempPlantDeliveryAnalysisList = OTStatusList.Select(a=>a.OTStatus).Distinct().ToList();
                //OTStatusList = new List<PlantDeliveryAnalysis>(TempPlantDeliveryAnalysisList);

                var TempPlantDeliveryAnalysisList1 = (from a in OTStatusList
                                                      select new
                                                      {
                                                          a.OfferStatusType,


                                                      }).Distinct().ToList();
                OTStatusList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.OfferStatusType = item1.OfferStatusType;
                    selectedvalue.IdProductCategory = PlantDeliveryAnalysisList.Where(a => a.OfferStatusType == item1.OfferStatusType).FirstOrDefault().IdProductCategory;
                    selectedvalue.Region = PlantDeliveryAnalysisList.Where(b => b.OfferStatusType == item1.OfferStatusType).FirstOrDefault().Region;

                    OTStatusList.Add(selectedvalue);

                }
                OTStatusList = new List<PlantDeliveryAnalysis>(OTStatusList);
                OTStatus_Cloned = new List<PlantDeliveryAnalysis>();
                OTStatus_Cloned = OTStatusList.ToList();
            }
            catch (Exception ex)
            {

            }
        }


        private void FillTemplate()
        {
            try
            {



                TemplateList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedOTStatus = new PlantDeliveryAnalysis();
                    selectedOTStatus.TemplateName = item.TemplateName;
                    //     selectedOTStatus.IdProductCategory = item.IdProductCategory;
                    TemplateList.Add(selectedOTStatus);

                }
                //List<PlantDeliveryAnalysis> TempPlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
                //TempPlantDeliveryAnalysisList = OTStatusList.Select(a=>a.OTStatus).Distinct().ToList();
                //OTStatusList = new List<PlantDeliveryAnalysis>(TempPlantDeliveryAnalysisList);

                var TempPlantDeliveryAnalysisList1 = (from a in TemplateList
                                                      select new
                                                      {
                                                          a.TemplateName,


                                                      }).Distinct().ToList();
                TemplateList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.TemplateName = item1.TemplateName;
                    selectedvalue.IdProductCategory = PlantDeliveryAnalysisList.Where(a => a.TemplateName == item1.TemplateName).FirstOrDefault().IdProductCategory;
                    selectedvalue.OTStatus = PlantDeliveryAnalysisList.Where(b => b.TemplateName == item1.TemplateName).FirstOrDefault().OTStatus;

                    TemplateList.Add(selectedvalue);

                }
                TemplateList = new List<PlantDeliveryAnalysis>(TemplateList);
                TemplateList_Cloned = new List<PlantDeliveryAnalysis>();
                TemplateList_Cloned = TemplateList.ToList();
            }
            catch (Exception ex)
            {

            }
        }
        private void FillOTItemStatus()
        {
            try
            {



                OTItemStatusList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedOTStatus = new PlantDeliveryAnalysis();
                    selectedOTStatus.OTStatus = item.OTStatus;
                    selectedOTStatus.IdProductCategory = item.IdProductCategory;
                    OTItemStatusList.Add(selectedOTStatus);

                }
                //List<PlantDeliveryAnalysis> TempPlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>();
                //TempPlantDeliveryAnalysisList = OTStatusList.Select(a=>a.OTStatus).Distinct().ToList();
                //OTStatusList = new List<PlantDeliveryAnalysis>(TempPlantDeliveryAnalysisList);

                var TempPlantDeliveryAnalysisList1 = (from a in OTItemStatusList
                                                      select new
                                                      {
                                                          a.OTStatus,


                                                      }).Distinct().ToList();
                OTItemStatusList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.OTStatus = item1.OTStatus;
                    selectedvalue.TemplateName = PlantDeliveryAnalysisList.Where(a => a.OTStatus == item1.OTStatus).FirstOrDefault().TemplateName;
                    selectedvalue.IdCustomer = PlantDeliveryAnalysisList.Where(b => b.OTStatus == item1.OTStatus).FirstOrDefault().IdCustomer;

                    OTItemStatusList.Add(selectedvalue);

                }
                OTItemStatusList = new List<PlantDeliveryAnalysis>(OTItemStatusList);
                OTItemStatusList_Cloned = new List<PlantDeliveryAnalysis>();
                OTItemStatusList_Cloned = OTItemStatusList.ToList();
            }
            catch (Exception ex)
            {

            }
        }

        private void FillCategory2()
        {
            try
            {

                Category2List = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedCategory1 = new PlantDeliveryAnalysis();
                    selectedCategory1.ProductCategoryGrid = new ProductCategoryGrid();
                    selectedCategory1.ProductCategoryGrid.Category = new ProductCategoryGrid();
                    if (item.ProductCategoryGrid != null)
                    {
                        if (item.ProductCategoryGrid.Category != null)
                        {
                            selectedCategory1.ProductCategoryGrid.Category.Name = item.ProductCategoryGrid.Category.Name;
                            selectedCategory1.ProductCategoryGrid.Category.IdProductCategory = item.ProductCategoryGrid.Category.IdProductCategory;
                            selectedCategory1.ProductCategoryGrid.Category.IdParent = item.ProductCategoryGrid.Category.IdParent;
                        }
                    }
                    Category2List.Add(selectedCategory1);

                }


                var TempPlantDeliveryAnalysisList = (from a in Category2List
                                                     select new
                                                     {
                                                         a.ProductCategoryGrid.Category.Name,
                                                         //a.ProductCategoryGrid.Category.IdParent,
                                                         //a.ProductCategoryGrid.Category.IdProductCategory

                                                     }).Distinct().ToList();
                Category2List = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList.Where(i => i.Name != null))
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();
                    selectedvalue.ProductCategoryGrid.Category = new ProductCategoryGrid();
                    selectedvalue.ProductCategoryGrid.Category.Name = item1.Name;
                    //if (PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name).ProductCategoryGrid.Category!=null)
                    //var temp = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name);
                    //if(temp!=null)
                    //{
                    //    if (temp.ProductCategoryGrid != null)
                    //    {
                    //        if (temp.ProductCategoryGrid.Category != null)
                    //        {
                    //            selectedvalue.ProductCategoryGrid.Category.IdParent = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name).ProductCategoryGrid.Category.IdParent;
                    //            // PlantDeliveryAnalysisList.Where(a=>a.ProductCategoryGrid.Category)
                    //            //PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name).ProductCategoryGrid.Category.IdParent; ;
                    //            selectedvalue.ProductCategoryGrid.Category.IdProductCategory = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name).ProductCategoryGrid.Category.IdProductCategory;
                    //            //selectedvalue.ProductCategoryGrid.Category.IdParent = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Name == item1.Name).ProductCategoryGrid.Category.IdParent; ;
                    selectedvalue.ProductCategoryGrid.Category.IdProductCategory = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.Category.Name == item1.Name).ProductCategoryGrid.Category.IdProductCategory;
                    Category2List.Add(selectedvalue);
                    // }
                    // }
                    //   }
                }
                Category2List = new List<PlantDeliveryAnalysis>(Category2List);
                Category2List_Cloned = new List<PlantDeliveryAnalysis>();
                Category2List_Cloned = Category2List.ToList();
            }
            catch (Exception ex)
            {

            }
        }

        private void FillCategory1()
        {
            try
            {



                Category1List = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedCategory1 = new PlantDeliveryAnalysis();
                    selectedCategory1.ProductCategoryGrid = new ProductCategoryGrid();
                    // selectedCategory2.ProductCategoryGrid.Category = new ProductCategoryGrid();
                    if (item.ProductCategoryGrid != null)
                    {
                        selectedCategory1.ProductCategoryGrid.Name = item.ProductCategoryGrid.Name;
                        selectedCategory1.ProductCategoryGrid.IdProductCategory = item.ProductCategoryGrid.IdProductCategory;
                        selectedCategory1.ProductCategoryGrid.IdParent = item.ProductCategoryGrid.IdParent;
                        Category1List.Add(selectedCategory1);
                    }

                }
                var TempPlantDeliveryAnalysisList = (from a in Category1List
                                                     select new
                                                     {
                                                         // a.ProductCategoryGrid.Name,
                                                         a.ProductCategoryGrid.IdProductCategory,
                                                         // a.ProductCategoryGrid.IdParent
                                                     }).Distinct().ToList();
                Category1List = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();
                    string tempCategory1Name = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.Name;
                    var tempSelectedValue = Category1List.Where(i => i.ProductCategoryGrid.Name == tempCategory1Name).ToList();
                    if (tempSelectedValue == null || tempSelectedValue.Count == 0)
                    {
                        selectedvalue.ProductCategoryGrid.Name = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.Name;
                        selectedvalue.ProductCategoryGrid.IdProductCategory = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.IdProductCategory == item1.IdProductCategory).IdProductCategory;
                        selectedvalue.ProductCategoryGrid.IdParent = PlantDeliveryAnalysisList.FirstOrDefault(a => a.ProductCategoryGrid.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.IdParent;
                        selectedvalue.OTStatus = PlantDeliveryAnalysisList.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OTStatus;
                        selectedvalue.TemplateName = PlantDeliveryAnalysisList.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).TemplateName;
                        selectedvalue.OfferStatusType = PlantDeliveryAnalysisList.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OfferStatusType;
                        Category1List.Add(selectedvalue);
                    }
                }
                Category1List = new List<PlantDeliveryAnalysis>(Category1List);
                Category1List_Cloned = new List<PlantDeliveryAnalysis>();
                Category1List_Cloned = Category1List.ToList();

            }
            catch (Exception ex)
            {

            }
        }

        private void FillCustomerGroup()
        {
            try
            {

                CustomerGroupList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedCustomerGroup = new PlantDeliveryAnalysis();
                    selectedCustomerGroup.CustomerGroup = item.CustomerGroup;
                    selectedCustomerGroup.IdProductCategory = item.IdProductCategory;
                    selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    CustomerGroupList.Add(selectedCustomerGroup);

                }
                var TempPlantDeliveryAnalysisList1 = (from a in CustomerGroupList
                                                      select new
                                                      {
                                                          // a.CustomerGroup,
                                                          // a.IdProductCategory,
                                                          a.IdCustomer

                                                      }).Distinct().ToList();
                CustomerGroupList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.CustomerGroup = PlantDeliveryAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().CustomerGroup;
                    selectedvalue.IdProductCategory = PlantDeliveryAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().IdProductCategory;
                    selectedvalue.IdCustomer = item1.IdCustomer;
                    selectedvalue.CustomerPlant = PlantDeliveryAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().CustomerPlant;
                    selectedvalue.OTStatus = PlantDeliveryAnalysisList.Where(i => i.IdCustomer == item1.IdCustomer).FirstOrDefault().OTStatus;
                    CustomerGroupList.Add(selectedvalue);

                }
                CustomerGroupList = new List<PlantDeliveryAnalysis>(CustomerGroupList);
                CustomerGroup_Cloned = new List<PlantDeliveryAnalysis>();
                CustomerGroup_Cloned = CustomerGroupList.ToList();
            }
            catch (Exception ex)
            {

            }
        }

        private void FillCustomerPlant()
        {
            try
            {

                CustomerPlantList = new List<PlantDeliveryAnalysis>();

                foreach (var item in PlantDeliveryAnalysisList)
                {
                    PlantDeliveryAnalysis selectedCustomerGroup = new PlantDeliveryAnalysis();
                    selectedCustomerGroup.CustomerPlant = item.CustomerPlant;
                    selectedCustomerGroup.IdCustomer = item.IdCustomer;
                    CustomerPlantList.Add(selectedCustomerGroup);

                }
                var TempPlantDeliveryAnalysisList1 = (from a in CustomerPlantList
                                                      select new
                                                      {
                                                          a.CustomerPlant,
                                                          a.IdCustomer

                                                      }).Distinct().ToList();
                CustomerPlantList = new List<PlantDeliveryAnalysis>();

                foreach (var item1 in TempPlantDeliveryAnalysisList1)
                {
                    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                    selectedvalue.CustomerPlant = item1.CustomerPlant;
                    selectedvalue.IdCustomer = item1.IdCustomer;

                    CustomerPlantList.Add(selectedvalue);

                }
                CustomerPlantList = new List<PlantDeliveryAnalysis>(CustomerPlantList);
                CustomerPlant_Cloned = new List<PlantDeliveryAnalysis>();
                CustomerPlant_Cloned = CustomerPlantList.ToList();
            }
            catch (Exception ex)
            {


            }
        }

        private void ChangeOTItemStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeOTItemStatusCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedOTStatus = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedOTStatus = new List<object>();

                        if (TempSelectedOTStatus != null)
                        {
                            foreach (var tmpOTStatus in (dynamic)TempSelectedOTStatus)
                            {
                                TmpSelectedOTStatus.Add(tmpOTStatus);
                            }

                            SelectedOTItemStatus = new List<object>();
                            SelectedOTItemStatus = TmpSelectedOTStatus;
                        }

                        if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();

                        //List<string> TemplateIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();


                        //List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(TemplateList_Cloned.Where(i => TemplateIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                        //TemplateList = new List<PlantDeliveryAnalysis>(filteredData);


                        List<string> CustomerIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerIds.Contains(Convert.ToString(i.OTStatus))).Distinct().ToList());
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerGroupList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();
                        TempcustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());

                        CustomerGroupList = new List<PlantDeliveryAnalysis>(TempcustomerGroupList);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);

                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        //SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
                        //SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());

                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeOTItemStatusCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeOTItemStatusCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCategory1CommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCategory1CommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCategory1 = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCategory1 = new List<object>();

                        if (TempSelectedCategory1 != null)
                        {
                            foreach (var tmpCategory1 in (dynamic)TempSelectedCategory1)
                            {
                                TmpSelectedCategory1.Add(tmpCategory1);
                            }

                            SelectedCategory1 = new List<object>();
                            SelectedCategory1 = TmpSelectedCategory1;
                        }

                        if (SelectedCategory1 == null) SelectedCategory1 = new List<object>();

                        List<string> Category2Ids = SelectedCategory1.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdParent)).Distinct().ToList();

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category2Ids.Contains(Convert.ToString(i.ProductCategoryGrid.IdParent))).ToList());
                        Category2List = new List<PlantDeliveryAnalysis>(filteredData);
                        TemplateList = new List<PlantDeliveryAnalysis>(filteredData);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> Tempcategory2List = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TemptemplateList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerGroupList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();
                       // Tempcategory2List.AddRange(Category2List.Where(i => i.ProductCategoryGrid != null && i.ProductCategoryGrid.Category != null).GroupBy(i => i.ProductCategoryGrid.Category.IdProductCategory).Select(grp => grp.First()).Distinct().ToList());

                        Tempcategory2List.AddRange(Category2List.Where(i => i.ProductCategoryGrid != null && i.ProductCategoryGrid.Category != null).GroupBy(i => i.ProductCategoryGrid.Category.IdProductCategory).Select(grp => grp.First()).ToList().Distinct());
                        TemptemplateList.AddRange(TemplateList.GroupBy(i => i.TemplateName).Select(grp => grp.First()).ToList().Distinct());
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTStatus).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());

                        Category2List = new List<PlantDeliveryAnalysis>(Tempcategory2List);
                        TemplateList = new List<PlantDeliveryAnalysis>(TemptemplateList);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(TempOTItemStatusList);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(TempcustomerGroupList);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);

                        SelectedTemplate = new List<object>(TemplateList.ToList());
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCategory2 = new List<object>(Category2List.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());

                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCategory1CommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCategory1CommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCategory2CommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCategory2CommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCategory2 = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCategory2 = new List<object>();

                        if (TempSelectedCategory2 != null)
                        {
                            foreach (var tmpCategory2 in (dynamic)TempSelectedCategory2)
                            {
                                TmpSelectedCategory2.Add(tmpCategory2);
                            }

                            SelectedCategory2 = new List<object>();
                            SelectedCategory2 = TmpSelectedCategory2;
                        }

                        if (SelectedCategory2 == null) SelectedCategory2 = new List<object>();

                        //List<int> Category2Ids = SelectedCategory1.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdParent)).Distinct().ToList();


                        //List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(Category2List_Cloned.Where(i => Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());
                        //Category2List = new List<PlantDeliveryAnalysis>(filteredData);


                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCategory2CommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCategory2CommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCustomerGroupCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCustomerGroup = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCustomerGroup = new List<object>();

                        if (TempSelectedCustomerGroup != null)
                        {
                            foreach (var tmpCustomerGroup in (dynamic)TempSelectedCustomerGroup)
                            {
                                TmpSelectedCustomerGroup.Add(tmpCustomerGroup);
                            }

                            SelectedCustomerGroup = new List<object>();
                            SelectedCustomerGroup = TmpSelectedCustomerGroup;
                        }

                        if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();


                        List<string> CustomerPlantIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerGroupCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCustomerPlantCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedCustomerPlant = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedCustomerPlant = new List<object>();

                        if (TempSelectedCustomerPlant != null)
                        {
                            foreach (var tmpCustomerPlant in (dynamic)TempSelectedCustomerPlant)
                            {
                                TmpSelectedCustomerPlant.Add(tmpCustomerPlant);
                            }

                            SelectedCustomerPlant = new List<object>();
                            SelectedCustomerPlant = TmpSelectedCustomerPlant;
                        }

                        if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();



                        //List<int> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdCustomer)).Distinct().ToList();


                        //List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToInt32(i.IdCustomer))).ToList());
                        //CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        ////var  test = CustomerGroupList.GroupBy(a => a.CustomerGroup).ToList();
                        //List<PlantDeliveryAnalysis> TempCustomerGroupList = new List<PlantDeliveryAnalysis>();


                        //var TempPlantDeliveryAnalysisList1 = (from a in CustomerGroupList
                        //                                      select new
                        //                                      {
                        //                                          a.CustomerGroup,
                        //                                      // a.IdCustomer

                        //                                          //    a.IdProductCategory


                        //                                      }).Distinct().ToList();
                        ////CustomerGroupList = new List<PlantDeliveryAnalysis>();

                        //foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        //{
                        //    PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                        //    selectedvalue.CustomerGroup = item1.CustomerGroup;
                        //    selectedvalue.IdCustomer = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.CustomerGroup == item1.CustomerGroup).IdCustomer;
                        //    selectedvalue.CustomerPlant = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.CustomerGroup == item1.CustomerGroup).CustomerPlant;
                        //    selectedvalue.IdProductCategory = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(i => i.CustomerGroup == item1.CustomerGroup).IdProductCategory;
                        //    selectedvalue.OTStatus= ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(i => i.CustomerGroup == item1.CustomerGroup).OTStatus;
                        //    TempCustomerGroupList.Add(selectedvalue);

                        //}
                        //CustomerGroupList = new List<PlantDeliveryAnalysis>(TempCustomerGroupList);
                        //CustomerGroup_Cloned = new List<PlantDeliveryAnalysis>();
                        //CustomerGroup_Cloned = CustomerGroupList.ToList();
                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerGroupCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedRegion = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedRegion = new List<object>();

                        if (TempSelectedRegion != null)
                        {
                            foreach (var tmpRegion in (dynamic)TempSelectedRegion)
                            {
                                TmpSelectedRegion.Add(tmpRegion);
                            }

                            SelectedRegion = new List<object>();
                            SelectedRegion = TmpSelectedRegion;
                        }

                        if (SelectedRegion == null) SelectedRegion = new List<object>();

                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).Region)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());
                        //[Aishwarya Ingale][Geos2-5918]
                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(Convert.ToString(i.Region))).Distinct().ToList());
                        OTStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        Category1List = new List<PlantDeliveryAnalysis>(filteredData);
                        Category2List = new List<PlantDeliveryAnalysis>(filteredData);
                        TemplateList = new List<PlantDeliveryAnalysis>(filteredData);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempOtStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> Tempcategory1List = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> Tempcategory2List = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TemptemplateList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerGroupList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();

                        TempOtStatusList.AddRange(OTStatusList.GroupBy(i => i.OfferStatusType).Select(grp => grp.First()).ToList().Distinct());
                        Tempcategory1List.AddRange(Category1List.GroupBy(i => i.ProductCategoryGrid.Name).Select(grp => grp.First()).ToList().Distinct());
                        Tempcategory2List.AddRange(Category2List .Where(i => i.ProductCategoryGrid.Category != null).GroupBy(i => i.ProductCategoryGrid.Category.IdProductCategory).Select(grp => grp.First()).Distinct().ToList()); //[Aishwarya Ingale][Geos2-5918]

                        TemptemplateList.AddRange(TemplateList.GroupBy(i => i.TemplateName).Select(grp => grp.First()).ToList().Distinct());
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTStatus).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());

                        OTStatusList = new List<PlantDeliveryAnalysis>(TempOtStatusList);
                        Category1List = new List<PlantDeliveryAnalysis>(Tempcategory1List);
                        Category2List = new List<PlantDeliveryAnalysis>(Tempcategory2List);
                        TemplateList = new List<PlantDeliveryAnalysis>(TemptemplateList);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(TempOTItemStatusList);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(TempcustomerGroupList);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);

                        SelectedTemplate = new List<object>(TemplateList.ToList());
                        SelectedOTStatus = new List<object>(OTStatusList.ToList());
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCategory1 = new List<object>(Category1List.ToList());
                        SelectedCategory2 = new List<object>(Category2List.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                        //SelectedTemplate.AddRange(TemplateList.ToList());
                        //SelectedOTStatus.AddRange(OTStatusList.ToList());
                        //SelectedOTItemStatus.AddRange(OTItemStatusList.ToList());
                        //SelectedCategory1.AddRange(Category1List.ToList());
                        //SelectedCategory2.AddRange(Category2List.ToList());
                        //SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
                        //SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());

                        ApplyFilterConditions();
                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                        //PlantDeliveryAnalysisView view = new PlantDeliveryAnalysisView();
                        //view.CustomSummaryCommandAction( string.Empty, string.Empty ,OTStatusList);

                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeCustomerGroupCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCustomerGroupCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeOTStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeOTStatusCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedOTStatus = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedOTStatus = new List<object>();

                        if (TempSelectedOTStatus != null)
                        {
                            foreach (var tmpOTStatus in (dynamic)TempSelectedOTStatus)
                            {
                                TmpSelectedOTStatus.Add(tmpOTStatus);
                            }

                            SelectedOTStatus = new List<object>();
                            SelectedOTStatus = TmpSelectedOTStatus;
                        }

                        if (SelectedOTStatus == null) SelectedOTStatus = new List<object>();

                        List<string> Category1Ids = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category1Ids.Contains(Convert.ToString(i.OfferStatusType))).ToList());

                        //List<int> Category1Ids = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();
                        //List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(Category1List_Cloned.Where(i => Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                        Category1List = new List<PlantDeliveryAnalysis>(filteredData);
                        Category2List = new List<PlantDeliveryAnalysis>(filteredData);
                        TemplateList = new List<PlantDeliveryAnalysis>(filteredData);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> Tempcategory1List = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> Tempcategory2List = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TemptemplateList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerGroupList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();
                        Tempcategory1List.AddRange(Category1List.GroupBy(i => i.ProductCategoryGrid.Name).Select(grp => grp.First()).ToList().Distinct());
                        // Tempcategory2List.AddRange(Category2List.GroupBy(i => i.ProductCategoryGrid.Category.IdProductCategory).Select(grp => grp.First()).ToList().Distinct());
                        Tempcategory2List.AddRange(Category2List.Where(i => i.ProductCategoryGrid != null && i.ProductCategoryGrid.Category != null).GroupBy(i => i.ProductCategoryGrid.Category.IdProductCategory).Select(grp => grp.First()).Distinct().ToList());

                        TemptemplateList.AddRange(TemplateList.GroupBy(i => i.TemplateName).Select(grp => grp.First()).ToList().Distinct());
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTStatus).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());


                        Category1List = new List<PlantDeliveryAnalysis>(Tempcategory1List);
                        Category2List = new List<PlantDeliveryAnalysis>(Tempcategory2List);
                        TemplateList = new List<PlantDeliveryAnalysis>(TemptemplateList);
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(TempOTItemStatusList);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(TempcustomerGroupList);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);

                        SelectedTemplate = new List<object>(TemplateList.ToList());
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCategory1 = new List<object>(Category1List.ToList());
                        SelectedCategory2 = new List<object>(Category2List.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());


                        ApplyFilterConditions();

                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeOTStatusCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeOTStatusCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeTemplateCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeTemplateCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (obj == null)
                {
                }
                else
                {

                    DevExpress.Xpf.Editors.ClosePopupEventArgs CloseModetemp = (DevExpress.Xpf.Editors.ClosePopupEventArgs)obj;

                    if (CloseModetemp.CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
                    {
                    }
                    else
                    {

                        var TempSelectedTemplate = (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).EditValue);
                        List<Object> TmpSelectedTemplate = new List<object>();

                        if (TempSelectedTemplate != null)
                        {
                            foreach (var tmpOTStatus in (dynamic)TempSelectedTemplate)
                            {
                                TmpSelectedTemplate.Add(tmpOTStatus);
                            }

                            SelectedTemplate = new List<object>();
                            SelectedTemplate = TmpSelectedTemplate;
                        }

                        if (SelectedTemplate == null) SelectedTemplate = new List<object>();

                        //List<int> Category1Ids = SelectedTemplate.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                        //List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(Category1List_Cloned.Where(i => Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                        //Category1List = new List<PlantDeliveryAnalysis>(filteredData);


                        List<string> oTStatusIds = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => oTStatusIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerGroupList = new List<PlantDeliveryAnalysis>();
                        List<PlantDeliveryAnalysis> TempcustomerPlantList = new List<PlantDeliveryAnalysis>();
                        TempOTItemStatusList.AddRange(OTItemStatusList.GroupBy(i => i.OTStatus).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerGroupList.AddRange(CustomerGroupList.GroupBy(i => i.CustomerGroup).Select(grp => grp.First()).ToList().Distinct());
                        TempcustomerPlantList.AddRange(CustomerPlantList.GroupBy(i => i.CustomerPlant).Select(grp => grp.First()).ToList().Distinct());

                        OTItemStatusList = new List<PlantDeliveryAnalysis>(TempOTItemStatusList);
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(TempcustomerGroupList);
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempcustomerPlantList);

                        SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());


                        ApplyFilterConditions();

                        FillAVGDelivery();
                        FillDataONTime();
                        CreateTable();
                        CreateDaysPOtoShipmentTable();
                        CreateXmlFile();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangeTemplateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeTemplateCommandAction()-2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ApplyFilterConditions()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()...", category: Category.Info, priority: Priority.Low);

                if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();
                if (SelectedCategory1 == null) SelectedCategory1 = new List<object>();
                if (SelectedCategory2 == null) SelectedCategory2 = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();
                if (SelectedRegion == null) SelectedRegion = new List<object>();
                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                if (SelectedOTStatus == null) SelectedOTStatus = new List<object>();
                if (ERMPlantDeliveryAnalysisList_Cloned != null)
                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.ToList());
                if (ONTimeDeliveryANDAVGDeliveryList_Cloned != null)
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.ToList());

                if (SelectedRegion.Count == RegionList.Count && SelectedOTStatus.Count == OTStatusList.Count && SelectedTemplate.Count == TemplateList.Count && SelectedOTItemStatus.Count == OTItemStatusList.Count && SelectedCategory1.Count == Category1List.Count && SelectedCategory2.Count == Category2List.Count && SelectedCustomerGroup.Count == CustomerGroupList.Count && SelectedCustomerPlant.Count == CustomerPlantList.Count)
                {
                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.ToList());
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.ToList());
                } // If Selected ALL filters

                else if (SelectedRegion != null && SelectedRegion.Count > 0)
                {
                    List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).Region)).Distinct().ToList();

                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                    if (SelectedOTStatus != null && SelectedOTStatus.Count > 0)
                    {
                        List<string> OTStatusIds = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType)).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType)).ToList());
                        if (SelectedCategory1 != null && SelectedCategory1.Count > 0)
                        {
                            List<int> Category1Ids = SelectedCategory1.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdProductCategory)).Distinct().ToList();
                            // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                            PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                            ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                            if (SelectedCategory2 != null && SelectedCategory2.Count > 0)
                            {

                                List<int> Category2Ids = SelectedCategory2.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.Category.IdProductCategory)).Distinct().ToList();

                                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt16(i.ProductCategoryGrid.IdProductCategory)) && Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());
                                ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt16(i.ProductCategoryGrid.IdProductCategory)) && Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());

                            }
                            if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                            {

                                List<string> TemplateIds = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();

                                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                                ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(Convert.ToString(i.TemplateName))).ToList());

                                if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                                {
                                    List<string> OTItemStatusIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();

                                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(Convert.ToString(i.OTStatus))).ToList());
                                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(Convert.ToString(i.OTStatus))).ToList());
                                    if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                                    {
                                        List<string> customerIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();

                                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                                        if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                                        {

                                            List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                                            PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && customerIds.Contains(i.CustomerGroup) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                            ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && customerIds.Contains(i.CustomerGroup) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                        }
                                    }
                                }

                                else if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                                {
                                    List<string> customerIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();

                                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                                    if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                                    {

                                        List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && customerIds.Contains(i.CustomerGroup) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && customerIds.Contains(i.CustomerGroup) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                    }
                                }
                                else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                                {

                                    List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                                    PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region) && OTStatusIds.Contains(i.OfferStatusType) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && TemplateIds.Contains(i.TemplateName) && CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                }



                            }

                        }
                    }

                }
                //else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                //{
                //    List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                //    PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                //    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                //    if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                //    {
                //        List<string> customerIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();

                //        PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                //        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());
                //        if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                //        {
                //            List<string> OTItemStatusIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();

                //            PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(Convert.ToString(i.OTStatus))).ToList());
                //            ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(Convert.ToString(i.OTStatus))).ToList());
                //            if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                //            {

                //                List<string> TemplateIds = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();

                //                PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                //                ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                //                if (SelectedCategory1 != null && SelectedCategory1.Count > 0)
                //                {
                //                    List<int> Category1Ids = SelectedCategory1.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdProductCategory)).Distinct().ToList();
                //                    // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                //                    PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                //                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && TemplateIds.Contains(i.TemplateName) && OTItemStatusIds.Contains(i.OTStatus) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                //                    if (SelectedCategory2 != null && SelectedCategory2.Count > 0)
                //                    {

                //                        List<int> Category2Ids = SelectedCategory2.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.Category.IdProductCategory)).Distinct().ToList();

                //                        PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());
                //                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());

                //                    }

                //                    if (SelectedOTStatus != null && SelectedOTStatus.Count > 0)
                //                    {
                //                        List<string> OTStatusIds = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();

                //                        PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && OTStatusIds.Contains(i.OfferStatusType)).ToList());
                //                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && OTStatusIds.Contains(i.OfferStatusType)).ToList());
                //                        if (SelectedRegion != null && SelectedRegion.Count > 0)
                //                        {
                //                            List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).Region)).Distinct().ToList();

                //                            PlantDeliveryAnalysisList = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))&& OTStatusIds.Contains(i.OfferStatusType) && RegionIds.Contains(i.Region)).ToList());
                //                            ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(i.CustomerPlant) && customerIds.Contains(i.CustomerGroup) && OTItemStatusIds.Contains(i.OTStatus) && TemplateIds.Contains(i.TemplateName) && Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory)) && OTStatusIds.Contains(i.OfferStatusType) && RegionIds.Contains(i.Region)).ToList());
                //                        }
                //                    }

                //                }
                //            }
                //        }
                //    }
                //}
                else
                {
                    if (SelectedRegion != null && SelectedRegion.Count > 0)
                    {
                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).Region)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => RegionIds.Contains(i.Region)).ToList());
                    }
                    else if (SelectedOTStatus != null && SelectedOTStatus.Count > 0)
                    {

                        List<string> OTStatusIds = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => OTStatusIds.Contains(i.OfferStatusType)).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => OTStatusIds.Contains(i.OfferStatusType)).ToList());
                    }
                    else if (SelectedTemplate != null && SelectedTemplate.Count > 0)
                    {

                        List<string> TemplateIds = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => TemplateIds.Contains(i.TemplateName)).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => TemplateIds.Contains(i.TemplateName)).ToList());
                    }
                    else if (SelectedOTItemStatus != null && SelectedOTItemStatus.Count > 0)
                    {

                        List<string> OTStatusIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => OTStatusIds.Contains(i.OTStatus)).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => OTStatusIds.Contains(i.OTStatus)).ToList());
                    }
                    else if (SelectedCategory1 != null && SelectedCategory1.Count > 0)
                    {
                        List<int> Category1Ids = SelectedCategory1.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdProductCategory)).Distinct().ToList();
                        // List<int> OTStatusCategoryIds = SelectedOTStatus.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdProductCategory)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => Category1Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.IdProductCategory))).ToList());

                        if (SelectedCategory2 != null)
                        {

                            if (SelectedCategory2.Count > 0)
                            {
                                List<int> Category2Ids = SelectedCategory2.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdParent)).Distinct().ToList();

                                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());
                                ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());

                            }
                        }
                    }
                    else if (SelectedCategory2 != null && SelectedCategory2.Count > 0)
                    {
                        List<int> Category2Ids = SelectedCategory2.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdParent)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => Category2Ids.Contains(Convert.ToInt32(i.ProductCategoryGrid.Category.IdProductCategory))).ToList());

                    }
                    else if (SelectedCustomerGroup != null && SelectedCustomerGroup.Count > 0)
                    {
                        List<int> customerIds = SelectedCustomerGroup.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).IdCustomer)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => customerIds.Contains(Convert.ToInt32(i.IdCustomer))).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => customerIds.Contains(Convert.ToInt32(i.IdCustomer))).ToList());

                        if (SelectedCustomerPlant != null)
                        {
                            if (SelectedCustomerPlant.Count > 0)
                            {
                                List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                                PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                                ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                            }
                        }
                    }
                    else if (SelectedCustomerPlant != null && SelectedCustomerPlant.Count > 0)
                    {
                        List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();

                        PlantDeliveryAnalysisList = new ObservableCollection<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                        ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerPlant))).ToList());
                    }
                }


                GeosApplication.Instance.Logger.Log("Method ApplyFilterConditions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in ApplyFilterConditions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillAllselectedFilter()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()...", category: Category.Info, priority: Priority.Low);
                if (SelectedOTItemStatus == null) SelectedOTItemStatus = new List<object>();
                if (SelectedCategory1 == null) SelectedCategory1 = new List<object>();
                if (SelectedCategory2 == null) SelectedCategory2 = new List<object>();
                if (SelectedCustomerPlant == null) SelectedCustomerPlant = new List<object>();
                if (SelectedCustomerGroup == null) SelectedCustomerGroup = new List<object>();
                if (SelectedRegion == null) SelectedRegion = new List<object>();
                if (SelectedTemplate == null) SelectedTemplate = new List<object>();
                if (SelectedOTStatus == null) SelectedOTStatus = new List<object>();

                #region [GEOS2-5113][Rupali Sarode][06-12-2023]

                if (!File.Exists(PlantDeliveryAnalysisFilterSettingFilePath))
                {
                    //SelectedTemplate.AddRange(TemplateList.ToList());
                    //SelectedOTStatus.AddRange(OTStatusList.ToList());
                    //SelectedRegion.AddRange(RegionList.ToList());
                    //SelectedOTItemStatus.AddRange(OTItemStatusList.ToList());
                    //SelectedCategory1.AddRange(Category1List.ToList());
                    //SelectedCategory2.AddRange(Category2List.ToList());
                    //SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
                    //SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());
                    SelectedTemplate=new List<object>(TemplateList.ToList());
                    SelectedOTStatus = new List<object>(OTStatusList.ToList());
                    SelectedRegion = new List<object>(RegionList.ToList());
                    SelectedOTItemStatus = new List<object>(OTItemStatusList.ToList());
                    SelectedCategory1 = new List<object>(Category1List.ToList());
                    SelectedCategory2 = new List<object>(Category2List.ToList());
                    SelectedCustomerGroup = new List<object>(CustomerGroupList.ToList());
                    SelectedCustomerPlant = new List<object>(CustomerPlantList.ToList());
                }
                else
                {
                    if (TempXMLRegionsList.Count == 0)  // If nothing is selected previously 
                        SelectedRegion = new List<object>();
                    else if (RegionList.Where(i => TempXMLRegionsList.Contains(i.Region)).Distinct().Count() == 0)  // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedRegion.AddRange(RegionList.ToList());
                    else
                    {

                        SelectedRegion = new List<object>(RegionList.Where(i => TempXMLRegionsList.Contains(i.Region)).Distinct().ToList());
                        //SelectedRegion.AddRange(RegionList.Where(i => TempXMLRegionsList.Contains(i.Region)).Distinct().ToList());

                        List<string> RegionIds = SelectedRegion.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).Region)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => RegionIds.Contains(Convert.ToString(i.Region))).ToList());
                        OTStatusList = new List<PlantDeliveryAnalysis>(filteredData);
                        List<PlantDeliveryAnalysis> TempRegionList = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList1 = (from a in OTStatusList
                                                              select new
                                                              {
                                                                  a.OfferStatusType,
                                                                  // a.IdCustomer

                                                                  //    a.IdProductCategory


                                                              }).Distinct().ToList();

                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.OfferStatusType = item1.OfferStatusType;
                            //selectedvalue.IdCustomer = item1.IdCustomer;
                            selectedvalue.IdProductCategory = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(i => i.OfferStatusType == item1.OfferStatusType).IdProductCategory;
                            selectedvalue.Region = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(i => i.OfferStatusType == item1.OfferStatusType).Region;
                            TempRegionList.Add(selectedvalue);

                        }
                        OTStatusList = new List<PlantDeliveryAnalysis>(TempRegionList);

                    }

                    if (TempXMLOTStatussList.Count == 0)   // If nothing is selected previously 
                        SelectedOTStatus = new List<object>();
                    else if (OTStatusList.Where(i => TempXMLOTStatussList.Contains(i.OfferStatusType)).Distinct().Count() == 0) // (6)	 If the user selection values are not present in the DDL must be selected ALL per default
                        SelectedOTStatus.AddRange(OTStatusList.ToList());
                    else
                    {
                        SelectedOTStatus = new List<object>(OTStatusList.Where(i => TempXMLOTStatussList.Contains(i.OfferStatusType)).Distinct().ToList());
                        // SelectedOTStatus.AddRange(OTStatusList.Where(i => TempXMLOTStatussList.Contains(i.OfferStatusType)).Distinct().ToList());

                        List<string> Category1Ids = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();
                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category1Ids.Contains(Convert.ToString(i.OfferStatusType))).ToList());

                        Category1List = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempCategory1List = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList1 = (from a in Category1List
                                                              select new
                                                              {
                                                                  a.ProductCategoryGrid.IdProductCategory,

                                                              }).Distinct().ToList();

                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();


                            string tempCategory1Name = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.ProductCategoryGrid.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.Name;
                            var tempSelectedValue = TempCategory1List.Where(i => i.ProductCategoryGrid.Name == tempCategory1Name).ToList();
                            if (tempSelectedValue == null || tempSelectedValue.Count == 0)
                            {
                                selectedvalue.ProductCategoryGrid.Name = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.Name;
                                selectedvalue.ProductCategoryGrid.IdProductCategory = item1.IdProductCategory;
                                selectedvalue.ProductCategoryGrid.IdParent = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).ProductCategoryGrid.IdParent;
                                selectedvalue.OTStatus = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OTStatus;
                                selectedvalue.TemplateName = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).TemplateName;
                                selectedvalue.OfferStatusType = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OfferStatusType;
                                TempCategory1List.Add(selectedvalue);
                            }

                        }
                        Category1List = new List<PlantDeliveryAnalysis>(TempCategory1List);
                    }



                    if (TempXMLCategory1List.Count == 0)
                        SelectedCategory1 = new List<object>();
                    else if (Category1List.Where(i => TempXMLCategory1List.Contains(i.ProductCategoryGrid.IdProductCategory)).Distinct().Count() == 0)
                        SelectedCategory1.AddRange(Category1List.ToList());
                    else
                    {
                        SelectedCategory1 = new List<object>(Category1List.Where(i => TempXMLCategory1List.Contains(i.ProductCategoryGrid.IdProductCategory)).Distinct().ToList());
                        // SelectedCategory1.AddRange(Category1List.Where(i => TempXMLCategory1List.Contains(i.ProductCategoryGrid.IdProductCategory)).Distinct().ToList());

                        List<string> Category2Ids = SelectedCategory1.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdParent)).Distinct().ToList();

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => Category2Ids.Contains(Convert.ToString(i.ProductCategoryGrid.IdParent))).ToList());
                        Category2List = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempCategory2List = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList = (from a in Category2List
                                                             select new
                                                             {
                                                                 a.ProductCategoryGrid.Category.Name,
                                                                 a.ProductCategoryGrid.Category.IdProductCategory,
                                                             }).Distinct().ToList();

                        Category2List = new List<PlantDeliveryAnalysis>();
                        foreach (var item1 in TempPlantDeliveryAnalysisList)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();
                            selectedvalue.ProductCategoryGrid.Category = new ProductCategoryGrid();
                            selectedvalue.ProductCategoryGrid.Category.Name = item1.Name;
                            selectedvalue.ProductCategoryGrid.Category.IdProductCategory = item1.IdProductCategory;
                            TempCategory2List.Add(selectedvalue);

                        }
                        Category2List = new List<PlantDeliveryAnalysis>(TempCategory2List);

                        List<string> TemplateIds = SelectedCategory1.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdProductCategory)).Distinct().ToList();

                        List<PlantDeliveryAnalysis> filteredData1 = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => TemplateIds.Contains(Convert.ToString(i.ProductCategoryGrid.IdProductCategory))).ToList());
                        TemplateList = new List<PlantDeliveryAnalysis>(filteredData1);

                        List<PlantDeliveryAnalysis> TempTemplateList = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList1 = (from a in TemplateList
                                                              select new
                                                              {
                                                                  a.TemplateName,

                                                              }).Distinct().ToList();

                        TemplateList = new List<PlantDeliveryAnalysis>();
                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.TemplateName = item1.TemplateName;
                            selectedvalue.OTStatus = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(i => i.TemplateName == item1.TemplateName).OTStatus;
                            TempTemplateList.Add(selectedvalue);

                        }

                        TemplateList = new List<PlantDeliveryAnalysis>(TempTemplateList);
                    }

                    if (TempXMLCategory2List.Count == 0)
                        SelectedCategory2 = new List<object>();
                    else if (Category2List.Where(i => TempXMLCategory2List.Contains(i.ProductCategoryGrid.Category.IdProductCategory)).Distinct().Count() == 0)
                        SelectedCategory2.AddRange(Category2List.ToList());
                    else
                    {
                        SelectedCategory2 = new List<object>(Category2List.Where(i => TempXMLCategory2List.Contains(i.ProductCategoryGrid.Category.IdProductCategory)).Distinct().ToList());
                        // SelectedCategory2.AddRange(Category2List.Where(i => TempXMLCategory2List.Contains(i.ProductCategoryGrid.Category.IdProductCategory)).Distinct().ToList());

                    }


                    if (TempXMLTemplatesList.Count == 0)
                        SelectedTemplate = new List<object>();
                    else if (TemplateList.Where(i => TempXMLTemplatesList.Contains(i.TemplateName)).Distinct().Count() == 0)
                        SelectedTemplate.AddRange(TemplateList.ToList());
                    else
                    {
                        SelectedTemplate = new List<object>(TemplateList.Where(i => TempXMLTemplatesList.Contains(i.TemplateName)).Distinct().ToList());
                        //SelectedTemplate.AddRange(TemplateList.Where(i => TempXMLTemplatesList.Contains(i.TemplateName)).Distinct().ToList());

                        List<string> oTStatusIds = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => oTStatusIds.Contains(Convert.ToString(i.TemplateName))).ToList());
                        OTItemStatusList = new List<PlantDeliveryAnalysis>(filteredData);

                        List<PlantDeliveryAnalysis> TempOTItemStatusList = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList1 = (from a in OTItemStatusList
                                                              select new
                                                              {
                                                                  a.OTStatus

                                                                  //    a.IdProductCategory


                                                              }).Distinct().ToList();

                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();
                            selectedvalue.OTStatus = item1.OTStatus;

                            selectedvalue.IdCustomer = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.OTStatus == item1.OTStatus).IdCustomer;
                            //selectedvalue.TemplateName = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).TemplateName;
                            //selectedvalue.OfferStatusType = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OfferStatusType;
                            TempOTItemStatusList.Add(selectedvalue);

                        }

                        OTItemStatusList = new List<PlantDeliveryAnalysis>(TempOTItemStatusList);
                    }


                    if (TempXMLOTItemStatussList.Count == 0)
                        SelectedOTItemStatus = new List<object>();
                    else if (OTItemStatusList.Where(i => TempXMLOTItemStatussList.Contains(i.OTStatus)).Distinct().Count() == 0)
                        SelectedOTItemStatus.AddRange(OTItemStatusList.ToList());
                    else
                    {
                        SelectedOTItemStatus = new List<object>(OTItemStatusList.Where(i => TempXMLOTItemStatussList.Contains(i.OTStatus)).Distinct().ToList());

                        // SelectedOTItemStatus.AddRange(OTItemStatusList.Where(i => TempXMLOTItemStatussList.Contains(i.OTStatus)).Distinct().ToList());

                        List<string> CustomerIds = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerIds.Contains(Convert.ToString(i.OTStatus))).ToList());
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(filteredData);
                        var TempPlantDeliveryAnalysisList1 = (from a in CustomerGroupList
                                                              select new
                                                              {
                                                                  a.CustomerGroup,

                                                              }).Distinct().ToList();
                        CustomerGroupList = new List<PlantDeliveryAnalysis>();

                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.CustomerGroup = item1.CustomerGroup;
                            selectedvalue.CustomerPlant = ERMPlantDeliveryAnalysisList_Cloned.Where(a => a.CustomerGroup == item1.CustomerGroup).FirstOrDefault().CustomerPlant;
                            CustomerGroupList.Add(selectedvalue);

                        }
                        CustomerGroupList = new List<PlantDeliveryAnalysis>(CustomerGroupList);

                    }

                    if (TempXMLCustomerGroupsList.Count == 0)
                        SelectedCustomerGroup = new List<object>();
                    else if (CustomerGroupList.Where(i => TempXMLCustomerGroupsList.Contains(i.CustomerGroup)).Distinct().Count() == 0)
                        SelectedCustomerGroup.AddRange(CustomerGroupList.ToList());
                    else
                    {
                        SelectedCustomerGroup = new List<object>(CustomerGroupList.Where(i => TempXMLCustomerGroupsList.Contains(i.CustomerGroup)).Distinct().ToList());

                        //SelectedCustomerGroup.AddRange(CustomerGroupList.Where(i => TempXMLCustomerGroupsList.Contains(i.CustomerGroup)).Distinct().ToList());
                        List<string> CustomerPlantIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();
                        // List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(PlantDeliveryAnalysisList.Where(i => SelectedRegion.Contains(i.Region)).Distinct().ToList());

                        List<PlantDeliveryAnalysis> filteredData = new List<PlantDeliveryAnalysis>(ERMPlantDeliveryAnalysisList_Cloned.Where(i => CustomerPlantIds.Contains(Convert.ToString(i.CustomerGroup))).ToList());

                        CustomerPlantList = new List<PlantDeliveryAnalysis>(filteredData);
                        List<PlantDeliveryAnalysis> TempCustomerPlantList = new List<PlantDeliveryAnalysis>();
                        var TempPlantDeliveryAnalysisList1 = (from a in CustomerPlantList
                                                              select new
                                                              {
                                                                  a.CustomerPlant

                                                                  //    a.IdProductCategory


                                                              }).Distinct().ToList();

                        foreach (var item1 in TempPlantDeliveryAnalysisList1)
                        {
                            PlantDeliveryAnalysis selectedvalue = new PlantDeliveryAnalysis();
                            selectedvalue.ProductCategoryGrid = new ProductCategoryGrid();
                            selectedvalue.CustomerPlant = item1.CustomerPlant;

                            selectedvalue.IdCustomer = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.CustomerPlant == item1.CustomerPlant).IdCustomer;
                            //selectedvalue.TemplateName = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).TemplateName;
                            //selectedvalue.OfferStatusType = ERMPlantDeliveryAnalysisList_Cloned.FirstOrDefault(a => a.IdProductCategory == item1.IdProductCategory).OfferStatusType;
                            TempCustomerPlantList.Add(selectedvalue);

                        }
                        CustomerPlantList = new List<PlantDeliveryAnalysis>(TempCustomerPlantList);
                    }


                    if (TempXMLCustomerPlantsList.Count == 0)
                        SelectedCustomerPlant = new List<object>();
                    else if (CustomerPlantList.Where(i => TempXMLCustomerPlantsList.Contains(i.CustomerPlant)).Distinct().Count() == 0)
                        SelectedCustomerPlant.AddRange(CustomerPlantList.ToList());
                    else
                    {
                        SelectedCustomerPlant = new List<object>(CustomerPlantList.Where(i => TempXMLCustomerPlantsList.Contains(i.CustomerPlant)).Distinct().ToList());

                        //SelectedCustomerPlant.AddRange(CustomerPlantList.Where(i => TempXMLCustomerPlantsList.Contains(i.CustomerPlant)).Distinct().ToList());
                    }

                    ApplyFilterConditions();
                    //FillAVGDelivery();
                    //FillDataONTime();
                    //CreateTable();
                }

                #endregion [GEOS2-5113][Rupali Sarode][06-12-2023]

                GeosApplication.Instance.Logger.Log("Method FillAllselectedFilter()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in FillAllselectedFilter() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FlyoutControl_Closed(object sender, EventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor FlyoutControl_Closed ...", category: Category.Info, priority: Priority.Low);
                var flyout = (sender as FlyoutControl);
                flyout.AnimationDuration = _currentDuration;
                flyout.Closed -= FlyoutControl_Closed;
                Processing();

                DateTime baseDate = DateTime.Today;
                var today = baseDate;
                //this week
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                //Last week
                var lastWeekStart = thisWeekStart.AddDays(-7);
                var lastWeekEnd = thisWeekStart.AddSeconds(-1);
                //this month
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                //last month
                var lastMonthStart = thisMonthStart.AddMonths(-1);
                var lastMonthEnd = thisMonthStart.AddSeconds(-1);
                //last one month
                var lastOneMonthStart = baseDate.AddMonths(-1);
                var lastOneMonthEnd = baseDate;
                //Last one week
                var lastOneWeekStart = baseDate.AddDays(-7);
                var lastOneWeekEnd = baseDate;
                //Last Year
                int year = DateTime.Now.Year - 1;
                DateTime StartFromDate = new DateTime(year, 1, 1);
                DateTime EndToDate = new DateTime(year, 12, 31);

                if (IsButtonStatus == 0)
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 1)//this month
                {
                    //FromDate = thisMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = thisMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = thisMonthStart;
                    ToDate = thisMonthEnd;
                    PlanningSchedulerControl scheduler = new PlanningSchedulerControl();
                    DateTime start = Convert.ToDateTime(FromDate);
                    DateTime end = Convert.ToDateTime(FromDate).AddDays(1);
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(start, end);

                    DateTime startDate = new DateTime(Convert.ToInt32(Convert.ToDateTime(FromDate).Year), Convert.ToDateTime(FromDate).Month, 1);
                    scheduler.Month = startDate;
                    scheduler.SelectedInterval = new DevExpress.Mvvm.DateTimeRange(Convert.ToDateTime(FromDate), Convert.ToDateTime(FromDate).AddDays(1));
                }
                else if (IsButtonStatus == 2)//last one month
                {
                    //FromDate = lastOneMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastOneMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastOneMonthStart;
                    ToDate = lastOneMonthEnd;
                }
                else if (IsButtonStatus == 3) //last month
                {
                    //FromDate = lastMonthStart.ToString("dd/MM/yyyy");
                    //ToDate = lastMonthEnd.ToString("dd/MM/yyyy");
                    FromDate = lastMonthStart;
                    ToDate = lastMonthEnd;
                }
                else if (IsButtonStatus == 4) //this week
                {
                    //FromDate = thisWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = thisWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 5) //last one week
                {
                    //    FromDate = lastOneWeekStart.ToString("dd/MM/yyyy");
                    //    ToDate = lastOneWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = thisWeekStart;
                    ToDate = thisWeekEnd;
                }
                else if (IsButtonStatus == 6) //last week
                {
                    //FromDate = lastWeekStart.ToString("dd/MM/yyyy");
                    //ToDate = lastWeekEnd.ToString("dd/MM/yyyy");
                    FromDate = lastWeekStart;
                    ToDate = lastWeekEnd;
                }
                else if (IsButtonStatus == 7) //custome range
                {
                    //FromDate = StartDate.ToString("dd/MM/yyyy");
                    //ToDate = EndDate.ToString("dd/MM/yyyy");
                    FromDate = StartDate;
                    ToDate = EndDate;
                }
                else if (IsButtonStatus == 8)//this year
                {
                    setDefaultPeriod();
                }
                else if (IsButtonStatus == 9)//last year
                {
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    //FromDate = StartFromDate.ToString("dd/MM/yyyy");
                    //ToDate = EndToDate.ToString("dd/MM/yyyy");
                    FromDate = StartFromDate;
                    ToDate = EndToDate;
                }
                else if (IsButtonStatus == 10)//last 12 month
                {
                    DateTime Date_F = DateTime.Now.Date.AddMonths(-12);
                    DateTime Date_T = DateTime.Now.Date;
                    //FromDate = Date_F.ToString("dd/MM/yyyy");
                    //ToDate = Date_T.ToString("dd/MM/yyyy");
                    FromDate = Date_F;
                    ToDate = Date_T;

                }


                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    GetPeriodDeliveryAnalysis(ERMCommon.Instance.SelectedAuthorizedPlantsList, FromDate, ToDate);
                }
                //   FillDVManagementProduction();
                IsBusy = false;
                IsPeriod = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FlyoutControl_Closed....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FlyoutControl_Closed() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private Action Processing()
        {
            IsBusy = true;
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
            return null;
        }

        private void PeriodCommandAction(object obj)
        {
            if (obj == null)
                return;

            Button button = (Button)obj;
            if (button.Name == "ThisMonth")
            {
                IsButtonStatus = 1;
            }
            else if (button.Name == "LastOneMonth")
            {
                IsButtonStatus = 2;
            }
            else if (button.Name == "LastMonth")
            {
                IsButtonStatus = 3;
            }
            else if (button.Name == "ThisWeek")
            {
                IsButtonStatus = 4;
            }
            else if (button.Name == "LastOneWeek")
            {
                IsButtonStatus = 5;
            }
            else if (button.Name == "LastWeek")
            {
                IsButtonStatus = 6;
            }
            else if (button.Name == "CustomRange")
            {
                IsButtonStatus = 7;
            }
            else if (button.Name == "ThisYear")
            {
                IsButtonStatus = 8;
            }
            else if (button.Name == "LastYear")
            {
                IsButtonStatus = 9;
            }
            else if (button.Name == "Last12Months")
            {
                IsButtonStatus = 10;
            }
            // IsCalendarVisible = Visibility.Collapsed;
        }
        private void PeriodCustomRangeCommandAction(object obj)
        {
            IsButtonStatus = 7;
            IsCalendarVisible = Visibility.Visible;
        }
        private void ApplyCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ApplyCommandAction ...", category: Category.Info, priority: Priority.Low);

                MenuFlyout menu = (MenuFlyout)obj;
                _currentDuration = menu.FlyoutControl.AnimationDuration;
                menu.FlyoutControl.AnimationDuration = new System.Windows.Duration(TimeSpan.FromMilliseconds(1));
                menu.FlyoutControl.Closed += FlyoutControl_Closed;
                menu.IsOpen = false;
                GeosApplication.Instance.Logger.Log("Method ApplyCommandAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ApplyCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelCommandAction(object obj)
        {
            MenuFlyout menu = (MenuFlyout)obj;
            menu.IsOpen = false;
        }

        #region Column Chooser

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(PlantDeliveryAnalysisGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(PlantDeliveryAnalysisGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(PlantDeliveryAnalysisGridSettingFilePath);

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
                        visibleFalseCoulumn++;
                    }
                }





                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PlantDeliveryAnalysisGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(PlantDeliveryAnalysisGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
        private void FillONTimeDeliveryANDAVGDelivery()
        {
            try
            {


                GeosApplication.Instance.Logger.Log("Method FillONTimeDeliveryANDAVGDelivery ...", category: Category.Info, priority: Priority.Low);
                if (ERMCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.Name));
                    if (ERMCommon.Instance.FailedPlants == null)
                    {
                        ERMCommon.Instance.FailedPlants = new List<string>();
                    }

                    FailedPlants = new List<string>();

                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>();

                    foreach (var itemPlantOwnerUsers in plantOwners)
                    {
                        try
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == itemPlantOwnerUsers.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                            ERMService = new ERMServiceController(serviceurl);
                            //  ERMService = new ERMServiceController("localhost:6699");
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.Name;

                            string IdSite = Convert.ToString(itemPlantOwnerUsers.IdSite);

                            var CurrencyNameFromSetting = String.Empty;
                            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ERM_SelectedCurrency"))
                            {
                                CurrencyNameFromSetting = GeosApplication.Instance.UserSettings["ERM_SelectedCurrency"];
                            }

                            //ONTimeDeliveryANDAVGDeliveryList.AddRange(ERMService.GetPlantDeliveryAnalysis_V2450(CurrencyNameFromSetting, IdSite, DateTime.ParseExact(LastYearStartDate, "dd/MM/yyyy", null),
                            //     DateTime.ParseExact(CurrentYearStartDate, "dd/MM/yyyy", null)));



                        }

                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (GeosApplication.Instance.SplashScreenMessage.EndsWith("Failed"))
                                if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                                {
                                    ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                    if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                    {
                                        ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                        ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                        ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                    }
                                }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = string.Empty;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method FillPlantDeliveryAnalysis() in filling list - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed");
                            if (!ERMCommon.Instance.FailedPlants.Contains(itemPlantOwnerUsers.Name))
                            {
                                ERMCommon.Instance.FailedPlants.Add(itemPlantOwnerUsers.Name);
                                if (ERMCommon.Instance.FailedPlants != null && ERMCommon.Instance.FailedPlants.Count > 0)
                                {
                                    ERMCommon.Instance.IsShowFailedPlantWarning = true;
                                    ERMCommon.Instance.WarningFailedPlants = string.Join(",", ERMCommon.Instance.FailedPlants.Select(x => x.ToString()).ToArray());
                                    ERMCommon.Instance.WarningFailedPlants = string.Format(System.Windows.Application.Current.FindResource("DataLoadingFailMessage").ToString(), ERMCommon.Instance.WarningFailedPlants.ToString());
                                }
                            }
                            System.Threading.Thread.Sleep(1000);
                            GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.Name, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    ONTimeDeliveryANDAVGDeliveryList = new List<PlantDeliveryAnalysis>(ONTimeDeliveryANDAVGDeliveryList);
                    ONTimeDeliveryANDAVGDeliveryList_Cloned = new List<PlantDeliveryAnalysis>();
                    ONTimeDeliveryANDAVGDeliveryList_Cloned = ONTimeDeliveryANDAVGDeliveryList.ToList();
                    // FillDataONTime();

                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    if (ERMCommon.Instance.FailedPlants == null || ERMCommon.Instance.FailedPlants.Count == 0)
                    {
                        ERMCommon.Instance.IsShowFailedPlantWarning = false;
                        ERMCommon.Instance.WarningFailedPlants = string.Empty;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillONTimeDeliveryANDAVGDelivery() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillONTimeDeliveryANDAVGDelivery()", category: Category.Exception, priority: Priority.Low);

            }
        }
        private void FillDataONTime()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDataONTime ...", category: Category.Info, priority: Priority.Low);
                var CurrentYear = DateTime.Now.Year;
                var LastYear = CurrentYear - 1;
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
                DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);

                #region  //rajashri  GEOS2-5916
                DataTable dtontime = new DataTable();
                dtontime.Columns.Add("Year", typeof(string));
                dtontime.Columns.Add("AVGONTimeData", typeof(double));
                //dtontime = CreateDataTable();
                #endregion
                


                List<PlantDeliveryAnalysis> LastYearPlantDeliveryAnalysis = new List<PlantDeliveryAnalysis>();

                LastYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == LastYear && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate).ToList();
                // var tempOntimeDeliveryAVG = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && i.DeliveryDate != null).ToList();
                if (LastYearPlantDeliveryAnalysis != null && LastYearPlantDeliveryAnalysis.Count > 0)
                {
                    var CountofShippingDateLessThanEqualsToDeliveryDate = LastYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
                    double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / LastYearPlantDeliveryAnalysis.Count) * 100;
                    int intValue = (int)Math.Floor(AVGOnTime);
                    AVGONTimeDataLastYearScaleValue = Convert.ToString(intValue);
                    //  AVGONTimeDataLastYearScaleValue = AVGONTimeDataLastYearScaleValue + "%";
                    string formattedAVGOnTime = AVGOnTime.ToString("F2");
                    AVGONTimeDataLastYear = formattedAVGOnTime;
                    ERMCommon.Instance.LastYearONTimeDeliveryAVG = AVGONTimeDataLastYear + "%";

                    DataRow drLastYear = dtontime.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGONTimeData"] = AVGONTimeDataLastYear;
                    dtontime.Rows.Add(drLastYear);
                }
                else
                {
                    AVGONTimeDataLastYear = "0";
                    DataRow drLastYear = dtontime.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGONTimeData"] = AVGONTimeDataLastYear;
                    dtontime.Rows.Add(drLastYear);
                }


                List<PlantDeliveryAnalysis> CurrentYearPlantDeliveryAnalysis = new List<PlantDeliveryAnalysis>();
                CurrentYearPlantDeliveryAnalysis = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.DeliveryDate.HasValue && i.ShippingDate.HasValue && i.DeliveryDate.Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                if (CurrentYearPlantDeliveryAnalysis != null && CurrentYearPlantDeliveryAnalysis.Count > 0)
                {
                    var CountofShippingDateLessThanEqualsToDeliveryDate = CurrentYearPlantDeliveryAnalysis.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
                    double AVGOnTime = ((double)CountofShippingDateLessThanEqualsToDeliveryDate.Count / CurrentYearPlantDeliveryAnalysis.Count) * 100;

                    int intValue = (int)Math.Floor(AVGOnTime);
                    AVGONTimeDataCurrentYearScaleValue = Convert.ToString(intValue);
                    //AVGONTimeDataCurrentYearScaleValue = AVGONTimeDataCurrentYearScaleValue + "%";
                    string formattedAVGOnTime = AVGOnTime.ToString("F2");
                    AVGONTimeDataCurrentYear = formattedAVGOnTime;
                    ERMCommon.Instance.CurrentYearONTimeDeliveryAVG = AVGONTimeDataCurrentYear + "%";
                    DataRow drCurrentYear = dtontime.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGONTimeData"] = AVGONTimeDataCurrentYear;
                    dtontime.Rows.Add(drCurrentYear);
                }
                else
                {
                    AVGONTimeDataCurrentYear = "0";
                    DataRow drCurrentYear = dtontime.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGONTimeData"] = AVGONTimeDataCurrentYear;
                    dtontime.Rows.Add(drCurrentYear);
                }

                TotalAVGONTimeTotal = "ONTimeDelivery" + CurrentYear + " " + AVGONTimeDataCurrentYear + "%" + "\n" + "ONTimeDelivery" + LastYear + " " + AVGONTimeDataLastYear + "%";
                int AddcurrentyearANDLastYearOntime = Convert.ToInt32(AVGONTimeDataCurrentYearScaleValue) + Convert.ToInt32(AVGONTimeDataLastYearScaleValue);
                EndValueOntime = Convert.ToString(AddcurrentyearANDLastYearOntime + 15);


                OnTimeDeliveryDataTable = dtontime;
                double lastYearValue = Convert.ToDouble(AVGONTimeDataLastYear);
                double currentYearValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                double greaterValue = 0.0, smallerValue = 0.0;
                if (currentYearValue > lastYearValue)
                {
                    greaterValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                    smallerValue = Convert.ToDouble(AVGONTimeDataLastYear);
                }
                else
                {
                    greaterValue = Convert.ToDouble(AVGONTimeDataLastYear);
                    smallerValue = Convert.ToDouble(AVGONTimeDataCurrentYear);
                }

                TotalPercentage = Math.Round(greaterValue - smallerValue, 2); 


                Isgreater = lastYearValue < currentYearValue;
                if (chartControlOnTime != null)
                {
                    chartControlOnTime.UpdateData();
                    OntimeDeliveryCreatechart1();
                }
                GeosApplication.Instance.Logger.Log("Method FillDataONTime() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillDataONTime()", category: Category.Exception, priority: Priority.Low);
            }

        }
        private void OnTimeDeliveryChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method OnTimeDeliveryChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlOnTime = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;

                OntimeDeliveryCreatechart1();



                //chartControlOnTime.EndInit();
                //chartControlOnTime.AnimationMode = ChartAnimationMode.OnDataChanged;
                //chartControlOnTime.Animate();

                GeosApplication.Instance.Logger.Log("Method OnTimeDeliveryChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnTimeDeliveryChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void AvgDeliveryChartLoadAction1(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlAvg = (ChartControl)obj;

                CreatechartAvgDelivery();
                //chartControlAvg.EndInit();
                //chartControlAvg.AnimationMode = ChartAnimationMode.OnDataChanged;
                //chartControlAvg.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D { };
        private void OntimeDeliveryCreatechart1()
        {
            try
            {

                XYDiagram2D diagram1 = new XYDiagram2D();
                chartControlOnTime.Diagram = diagram1;
                chartControlOnTime.BeginInit();

                diagram1.ActualAxisX.Label = new AxisLabel();
                //diagram.ActualAxisX.Label.TextPattern ="45";
                diagram1.ActualAxisY.Label = new AxisLabel();
                diagram1.ActualAxisY.Label.TextPattern = "{V:F0}%";

                // Configure the X and Y axes

                diagram1.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions
                {
                    AutoGrid = false
                };

                //BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGONTimeDataCurrentYear"
                //};

                //BarSideBySideSeries2D seriesLastYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGONTimeDataLastYear"
                //};

                BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                {
                    ArgumentDataMember = "Year",
                    ValueDataMember = "AVGONTimeData",
                    BarWidth = 0.3,
                    CrosshairLabelPattern = "{V:F2}%"
                };
                System.Windows.Media.Color commonColor = System.Windows.Media.Color.FromRgb(130, 163, 255); // CornflowerBlue color
                seriesCurrentYear.Brush = new SolidColorBrush(commonColor);
                //seriesLastYear.Brush = new SolidColorBrush(commonColor);
                seriesCurrentYear.DataSource = OnTimeDeliveryDataTable;
                //seriesLastYear.DataSource = OnTimeDeliveryDataTable;


                //trendline
                LineSeries2D lineDashedontimeDelivery = new LineSeries2D();
                lineDashedontimeDelivery.LineStyle = new LineStyle();
                lineDashedontimeDelivery.LineStyle.DashStyle = new DashStyle();
                lineDashedontimeDelivery.LineStyle.Thickness = 2;
                lineDashedontimeDelivery.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedontimeDelivery.CrosshairLabelVisibility = false;
                lineDashedontimeDelivery.ArgumentDataMember = "Year";
                lineDashedontimeDelivery.ToolTipEnabled = false;
                lineDashedontimeDelivery.ValueDataMember = "AVGONTimeData";
                diagram1.Series.Add(lineDashedontimeDelivery);

                //chartControl.Diagram.Series.Add(seriesCurrentYear);
                //RefreshChartControl();

                diagram1.Series.Add(seriesCurrentYear);
                //diagram.Series.Add(seriesLastYear);
                chartControlOnTime.EndInit();
                chartControlOnTime.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlOnTime.Animate();
                GeosApplication.Instance.Logger.Log("Method Createchart1() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in Createchart1() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        private void CreatechartAvgDelivery()
        {
            try
            {

                XYDiagram2D diagram2 = new XYDiagram2D();
                chartControlAvg.Diagram = diagram2;
                //diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;
                chartControlAvg.BeginInit();

                // Configure the X and Y axes
                diagram2.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions
                {
                    AutoGrid = false
                };

                //BarSideBySideSeries2D seriesCurrentYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGDeliveryDataCurrentYear"
                //};

                //BarSideBySideSeries2D seriesLastYear = new BarSideBySideSeries2D
                //{
                //    ArgumentDataMember = "MonthYear",
                //    ValueDataMember = "AVGDeliveryDataLastYear"
                //};

                BarSideBySideSeries2D seriesLastCurrentYear = new BarSideBySideSeries2D
                {
                    ArgumentDataMember = "Year",
                    ValueDataMember = "AVGDaysData",
                    BarWidth = 0.3,
                     LegendTextPattern = "{S} : {V:F0}",
                    ValueScaleType= ScaleType.Numerical,
                    ArgumentScaleType = ScaleType.Auto
                };
                System.Windows.Media.Color commonColor = System.Windows.Media.Color.FromRgb(130, 163, 255);// CornflowerBlue color
                seriesLastCurrentYear.Brush = new SolidColorBrush(commonColor);
                seriesLastCurrentYear.DataSource = GraphAvgdelivery;

                //trendline
                LineSeries2D lineDashedontimeDelivery = new LineSeries2D();
                lineDashedontimeDelivery.LineStyle = new LineStyle();
                lineDashedontimeDelivery.LineStyle.DashStyle = new DashStyle();
                lineDashedontimeDelivery.LineStyle.Thickness = 2;
                lineDashedontimeDelivery.LineStyle.DashStyle.Dashes = new DoubleCollection(new double[] { 3, 3, 3, 3 });
                lineDashedontimeDelivery.ArgumentScaleType = ScaleType.Auto;
                lineDashedontimeDelivery.ValueScaleType = ScaleType.Numerical;
                lineDashedontimeDelivery.ArgumentDataMember = "Year";
                lineDashedontimeDelivery.ValueDataMember = "AVGDaysData";
                lineDashedontimeDelivery.CrosshairLabelVisibility = false;
                diagram2.Series.Add(lineDashedontimeDelivery);
                diagram2.Series.Add(seriesLastCurrentYear);
                chartControlAvg.EndInit();
                chartControlAvg.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlAvg.Animate();
                GeosApplication.Instance.Logger.Log("Method CreatechartAvgDelivery() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in CreatechartAvgDelivery() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        private void FillAVGDelivery()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery ...", category: Category.Info, priority: Priority.Low);
                var CurrentYear = DateTime.Now.Year;
                var LastYear = CurrentYear - 1;
                DateTime TempCurrentYearStartDate = new DateTime(CurrentYear, 1, 1);
                DateTime TempCurrentYearEndDate = new DateTime(CurrentYear, 12, 31);
                DateTime TempLastYearStartDate = new DateTime(LastYear, 1, 1);
                DateTime TempLastYearEndDate = new DateTime(LastYear, 12, 31);


                DataTable dtavg = new DataTable();
                dtavg.Columns.Add("Year", typeof(string));
                dtavg.Columns.Add("AVGDaysData", typeof(double));

               

                List<PlantDeliveryAnalysis> LastYearAVGDelivery = new List<PlantDeliveryAnalysis>();
                LastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == LastYear && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate).ToList();


                List<int> IntLastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && ((i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == LastYear) && i.ShippingDate <= TempLastYearEndDate && i.ShippingDate >= TempLastYearStartDate)
               .Select(i => (i.ShippingDate - (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value).Value.Days).ToList();

                //LastYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null).Where(item => item.ShippingDate <= TempLastYearEndDate && item.ShippingDate >= TempLastYearStartDate).ToList();

                if (LastYearAVGDelivery.Count > 0)
                {
                    int sumOfAVGLastYeardays = IntLastYearAVGDelivery.Sum(i => i);
                    int SumOfLastyearTotalDays = LastYearAVGDelivery.Sum(i => i.TotalDays);
                    double AVGDeliveryLastyear = ((double)SumOfLastyearTotalDays / sumOfAVGLastYeardays) * 100;
                    int intValue = (int)Math.Round(AVGDeliveryLastyear);
                    AVGDeliveryDataLastYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryLastyear.ToString("F2");
                    AVGDeliveryDataLastYear = AVGDeliveryDataLastYearScaleValue;
                    ERMCommon.Instance.LastYearDeliveryAVG = AVGDeliveryDataLastYear;
                    DataRow drLastYear = dtavg.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGDaysData"] = AVGDeliveryDataLastYear;
                    dtavg.Rows.Add(drLastYear);
                }
                else
                {
                    AVGDeliveryDataLastYearScaleValue = "0";
                    ERMCommon.Instance.LastYearDeliveryAVG = "0";
                    DataRow drLastYear = dtavg.NewRow();
                    drLastYear["Year"] = LastYear.ToString();
                    drLastYear["AVGDaysData"] = AVGDeliveryDataLastYearScaleValue;
                    dtavg.Rows.Add(drLastYear);
                }


                List<PlantDeliveryAnalysis> CurrentYearAVGDelivery = new List<PlantDeliveryAnalysis>();
                CurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                // CurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null&& i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate).ToList();
                List<int> IntCurrentYearAVGDelivery = ONTimeDeliveryANDAVGDeliveryList.Where(i => i.ShippingDate != null && ((i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value.Year == CurrentYear) && i.ShippingDate <= TempCurrentYearEndDate && i.ShippingDate >= TempCurrentYearStartDate)
                .Select(i => (i.ShippingDate - (i.GoAheadDate != null ? i.GoAheadDate : i.PODate).Value).Value.Days).ToList();


                if (CurrentYearAVGDelivery.Count > 0)
                {
                    int sumOfAVGCurrentYeardays = IntCurrentYearAVGDelivery.Sum(i => i);
                    //var temp = CurrentYearAVGDelivery.Where(a=>a.ShippingDate!=null)
                    int SumOfCurrentYearTotalDays = CurrentYearAVGDelivery.Sum(i => i.TotalDays);
                    double AVGDeliveryCurrentyear = ((double)SumOfCurrentYearTotalDays / sumOfAVGCurrentYeardays) * 100;
                    int intValue = (int)Math.Round(AVGDeliveryCurrentyear);
                    AVGDeliveryDataCurrentYearScaleValue = Convert.ToString(intValue);
                    string formattedAVG = AVGDeliveryCurrentyear.ToString("F2");
                    AVGDeliveryDataCurrentYear = formattedAVG;
                    ERMCommon.Instance.CurrentYearDeliveryAVG = formattedAVG;
                    DataRow drCurrentYear = dtavg.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGDaysData"] = AVGDeliveryDataCurrentYearScaleValue;
                    dtavg.Rows.Add(drCurrentYear);
                }
                else
                {
                    AVGDeliveryDataCurrentYearScaleValue = "0";
                    ERMCommon.Instance.CurrentYearDeliveryAVG = "0";
                    DataRow drCurrentYear = dtavg.NewRow();
                    drCurrentYear["Year"] = CurrentYear.ToString();
                    drCurrentYear["AVGDaysData"] = AVGDeliveryDataCurrentYearScaleValue;
                    dtavg.Rows.Add(drCurrentYear);
                }

                int AddcurrentyearANDLastYear = Convert.ToInt32(AVGDeliveryDataCurrentYearScaleValue) + Convert.ToInt32(AVGDeliveryDataLastYearScaleValue);
                EndValue = Convert.ToString(AddcurrentyearANDLastYear + 15);

                TotalAVGDeliveryTotal = "AVGDelivery" + CurrentYear + " " + AVGDeliveryDataCurrentYearScaleValue + "\n" + "AVGDelivery" + LastYear + " " + AVGDeliveryDataLastYear;



                GraphAvgdelivery = dtavg;
                double lastYearValue = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                double currentYearValue = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);
                
                double greaterValue1 = 0.0, smallerValue1 = 0.0;
                if (currentYearValue > lastYearValue)
                {
                    greaterValue1 = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);
                    smallerValue1 = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                }
                else
                {
                    greaterValue1 = Convert.ToDouble(AVGDeliveryDataLastYearScaleValue);
                    smallerValue1 = Convert.ToDouble(AVGDeliveryDataCurrentYearScaleValue);
                }

                TotalPercentage1 = greaterValue1 - smallerValue1;
                
                IsgreaterAvgdelivery = lastYearValue < currentYearValue;
                if (chartControlAvg != null)
                {
                    chartControlAvg.UpdateData();
                    CreatechartAvgDelivery();
                }
                GeosApplication.Instance.Logger.Log("Method FillAVGDelivery() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillAVGDelivery()", category: Category.Exception, priority: Priority.Low);

            }
        }

        #region Days after the agreed delivery date chart
        private void CreateTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateTable ...", category: Category.Info, priority: Priority.Low);

                dt.Rows.Clear();

                PlantNameList = new List<PlantDeliveryAnalysis>();

                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                foreach (var Plantname in plantOwners)
                {
                    Int32 IdSite = Convert.ToInt32(Plantname.IdSite);
                    DataRow dr = dt.NewRow();
                    dr[0] = Plantname.Name.ToString().PadLeft(2, '0');
                    int Count = YearsList.Count();
                    foreach (int year in YearsList)
                    {


                        string Year = Convert.ToString(year);
                        int fyear = DateTime.Now.Year;
                        if (fyear != year)
                        {
                            var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                            if (tempPlantDeliveryAnalysisList != null)
                            {
                                List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                                templist = PlantDeliveryAnalysisList.Where(item =>
                                {
                                    DateTime? selectedDate = null;
                                    if (item.OTIdSite == IdSite)
                                    {
                                        if (item.GoAheadDate != null)
                                        {
                                            if (item.GoAheadDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                            else if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                        }
                                        else
                                        {

                                            if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                            else if (item.SamplesDate != null && item.SamplesDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                        }
                                    }

                                    return selectedDate != null;
                                }).ToList();

                                if (templist.Count > 0)
                                {


                                    var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate && !a.DaystoShippment.Contains("-") && a.DaystoShippment != "0").ToList();
                                    if (TotalRecordCount.Count > 0)
                                    {
                                        List<int> totalDays = new List<int>();
                                        foreach (var item in TotalRecordCount)
                                        {
                                            int days = (item.DeliveryDate.Value.Date - item.ShippingDate.Value.Date).Days;
                                            totalDays.Add(days);
                                        }
                                        int sumofDays = totalDays.Sum(i => i);
                                        int AVGDays = sumofDays / TotalRecordCount.Count;

                                        dr[Year] = AVGDays;
                                    }

                                    //  dr[Year] = 100;
                                }
                            }

                        }
                        else
                        {
                            var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                            if (tempPlantDeliveryAnalysisList != null)
                            {
                                List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                                templist = PlantDeliveryAnalysisList.Where(item =>
                                {
                                    DateTime? selectedDate = null;
                                    if (item.OTIdSite == IdSite)
                                    {
                                        if (item.GoAheadDate != null)
                                        {
                                            if (item.GoAheadDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                            else if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                            }
                                        }
                                        else
                                        {

                                            if (item.PODate != null && item.PODate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                            else if (item.SamplesDate != null && item.SamplesDate.Value.Year == year)
                                            {
                                                selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                                            }
                                        }
                                    }

                                    return selectedDate != null;
                                }).ToList();
                                if (templist.Count > 0)
                                {

                                    //result.Where(a=>a.v)
                                    var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate && !a.DaystoShippment.Contains("-") && a.DaystoShippment != "0").ToList();

                                    if (TotalRecordCount.Count > 0)
                                    {
                                        List<int> totalDays = new List<int>();
                                        foreach (var item in TotalRecordCount)
                                        {
                                            int days = (item.DeliveryDate.Value.Date - item.ShippingDate.Value.Date).Days;
                                            totalDays.Add(days);
                                        }
                                        int sumofDays = totalDays.Sum(i => i);
                                        int AVGDays = sumofDays / TotalRecordCount.Count;

                                        dr[Year] = AVGDays;
                                    }

                                    //  dr[Year] = 100;
                                }
                            }
                        }

                    }

                    dt.Rows.Add(dr);



                }





                GeosApplication.Instance.Logger.Log("Method CreateTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GraphDataTable = dt;

            if (chartControl != null)
            {
                chartControl.UpdateData();
                Createchart();
            }
        }

        private void ChartLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControl = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                chartControl.BeginInit();

                Createchart();



                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        private void ChartDashboardSaleCustomDrawCrosshairCommandAction(object obj)
        {
            try
            {
                CustomDrawCrosshairEventArgs e = (CustomDrawCrosshairEventArgs)obj;
                foreach (var group in e.CrosshairElementGroups)
                {
                    var reverseList = group.CrosshairElements.ToList();
                    group.CrosshairElements.Clear();
                    foreach (var item in reverseList)
                    {
                        if (item.Series.DisplayName == "OnHold-T-S")
                            group.CrosshairElements.Add(item);
                        else
                            group.CrosshairElements.Insert(0, item);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartDashboardSaleCustomDrawCrosshairCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        static List<int> GetYearsBetween(int fromYear, int toYear)
        {

            List<int> yearsBetween = new List<int>();

            for (int year = fromYear; year <= toYear; year++)
            {
                yearsBetween.Add(year);
            }

            return yearsBetween;
        }

        private void FindYear()
        {
            try
            {


                DateTime tempFromDate = Convert.ToDateTime(FromDate);
                int year1 = tempFromDate.Year;
                DateTime tempToDate = Convert.ToDateTime(ToDate);
                int Year2 = tempToDate.Year;

                YearsList = GetYearsBetween(year1, Year2);

                dt = new DataTable();
                dt.Columns.Add("MonthYear");
                foreach (int item in YearsList)
                {
                    dt.Columns.Add(Convert.ToString(item));
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void Createchart()
        {
            try
            {
                //  chartControl.BeginInit();
                XYDiagram2D diagram = new XYDiagram2D();
                chartControl.Diagram = diagram;
                //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
                //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
                diagram.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
                diagram.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
                // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
                diagram.ActualAxisY.NumericOptions = new NumericOptions();
                //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
                diagram.ActualAxisY.NumericOptions.Format = NumericFormat.General;
                AxisY2D axisY = new AxisY2D();
                AxisTitle axisYTitle = new AxisTitle();
                axisYTitle.Content = "Zero = Agreed ETD";
                axisY.Title = axisYTitle;
                diagram.AxisY = axisY;

                int count1 = 1;
                int columnIndex1 = 1;
                List<string> columns1 = new List<string>();
                //for (int i = 0; i < GraphDataTable.Columns.Count; i++)
                //{


                foreach (DataRow item in GraphDataTable.Rows)
                {
                    string argumentColumnName = "MonthYear";

                    System.Data.DataRow datarow = (System.Data.DataRow)item;
                    var columns = datarow.Table.Columns;

                    foreach (System.Data.DataColumn columnPrefix in columns)
                    {
                        string valueColumnName = columnPrefix.ColumnName;
                        //object columnValue = datarow[columnPrefix.ColumnName];


                        if (valueColumnName != argumentColumnName)
                        {
                            if (GraphDataTable.Columns.Contains(valueColumnName))
                            {
                                // Check if the series already exists for the column
                                BarSideBySideSeries2D existingSeries = diagram.Series
                                    .OfType<BarSideBySideSeries2D>()
                                    .FirstOrDefault(series => series.ValueDataMember == valueColumnName);

                                // If the series doesn't exist, create a new one
                                if (existingSeries == null)
                                {


                                    //int bottum = Convert.ToInt32(columnValue);
                                    //int paddingbottum = bottum * 7;
                                    BarSideBySideSeries2D series = new BarSideBySideSeries2D
                                    {
                                        Name = "series",
                                        ArgumentDataMember = argumentColumnName,
                                        ValueDataMember = valueColumnName,
                                        DisplayName = valueColumnName,
                                        LabelsVisibility = true,

                                        Label = new SeriesLabel
                                        {
                                            Visible = true,


                                            //Padding = new Thickness(0,0,0, paddingbottum)


                                        }

                                    };

                                    Legend Legend = new Legend
                                    {
                                        Name = "year" + valueColumnName,
                                        HorizontalPosition = HorizontalPosition.Center,
                                        VerticalPosition = VerticalPosition.TopOutside,
                                        Orientation = Orientation.Horizontal,
                                        // IndentFromDiagram = new Thickness(10)
                                        // MaxCrosshairContentHeight= 10
                                        // Padding = new Thickness(50, 0, 0, 0) 

                                    };


                                    series.DataSource = GraphDataTable;
                                    chartControl.Legends.Add(Legend);
                                    diagram.Series.Add(series);
                                }
                            }
                        }
                    }
                }





                chartControl.EndInit();
                chartControl.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControl.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region [GEOS2-5113][Rupali Sarode][06-12-2023]
        private void CreateXmlFile()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateXmlFile ...", category: Category.Info, priority: Priority.Low);

                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "body", string.Empty);
                doc.AppendChild(element1);

                if (SelectedRegion != null)
                {
                    XmlElement ParentNodeRegions = doc.CreateElement(string.Empty, "Regions", string.Empty);
                    element1.AppendChild(ParentNodeRegions);

                    foreach (PlantDeliveryAnalysis ObjRegion in SelectedRegion)
                    {
                        XmlElement childNode1 = doc.CreateElement("Region");
                        //XmlAttribute name = doc.CreateAttribute("Name");
                        //name.Value = ObjRegion.Region;
                        //childNode1.Attributes.Append(name);
                        XmlText RegionId = doc.CreateTextNode(ObjRegion.Region);
                        childNode1.AppendChild(RegionId);
                        ParentNodeRegions.AppendChild(childNode1);
                    }

                }

                if (SelectedOTStatus != null)
                {
                    List<string> OTStatus = SelectedOTStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OfferStatusType)).Distinct().ToList();
                    XmlElement ParentNodeOTStatus = doc.CreateElement("OTStatuss");
                    element1.AppendChild(ParentNodeOTStatus);

                    foreach (string ObjOTStatus in OTStatus)
                    {
                        XmlElement childNode1 = doc.CreateElement("OTStatus");
                        XmlText OtStatus = doc.CreateTextNode(ObjOTStatus);
                        childNode1.AppendChild(OtStatus);
                        ParentNodeOTStatus.AppendChild(childNode1);
                    }
                }

                if (SelectedCategory1 != null)
                {
                    List<string> Category1Ids = SelectedCategory1.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).ProductCategoryGrid.IdProductCategory)).Distinct().ToList();
                    XmlElement ParentNodeCategory1 = doc.CreateElement("Category1");
                    element1.AppendChild(ParentNodeCategory1);

                    foreach (string ObjCategory1 in Category1Ids)
                    {
                        XmlElement childNode1 = doc.CreateElement("Category1Id");
                        XmlText Category1 = doc.CreateTextNode(ObjCategory1);
                        childNode1.AppendChild(Category1);
                        ParentNodeCategory1.AppendChild(childNode1);
                    }
                }

                if (SelectedCategory2 != null)
                {
                    List<int> Category2Ids = SelectedCategory2.Select(i => Convert.ToInt32((i as PlantDeliveryAnalysis).ProductCategoryGrid.Category.IdProductCategory)).Distinct().ToList();
                    XmlElement ParentNodeCategory2 = doc.CreateElement("Category2");
                    element1.AppendChild(ParentNodeCategory2);

                    foreach (int ObjCategory2 in Category2Ids)
                    {
                        XmlElement childNode1 = doc.CreateElement("Category2Id");
                        XmlText Category2 = doc.CreateTextNode(Convert.ToString(ObjCategory2));
                        childNode1.AppendChild(Category2);
                        ParentNodeCategory2.AppendChild(childNode1);
                    }
                }

                if (SelectedTemplate != null)
                {
                    List<string> TemplateName = SelectedTemplate.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).TemplateName)).Distinct().ToList();
                    XmlElement ParentNodeTemplates = doc.CreateElement("Templates");
                    element1.AppendChild(ParentNodeTemplates);

                    foreach (string ObjTemplate in TemplateName)
                    {
                        XmlElement childNode1 = doc.CreateElement("Template");
                        XmlText Template = doc.CreateTextNode(ObjTemplate);
                        childNode1.AppendChild(Template);
                        ParentNodeTemplates.AppendChild(childNode1);
                    }
                }

                if (SelectedOTItemStatus != null)
                {
                    List<string> OTItemStatus = SelectedOTItemStatus.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).OTStatus)).Distinct().ToList();
                    XmlElement ParentNodeOTItemStatuss = doc.CreateElement("OTItemStatuss");
                    element1.AppendChild(ParentNodeOTItemStatuss);

                    foreach (string ObjOTItemStatus in OTItemStatus)
                    {
                        XmlElement childNode1 = doc.CreateElement("OTItemStatus");
                        XmlText OTItemStatusText = doc.CreateTextNode(ObjOTItemStatus);
                        childNode1.AppendChild(OTItemStatusText);
                        ParentNodeOTItemStatuss.AppendChild(childNode1);
                    }
                }


                if (SelectedCustomerGroup != null)
                {
                    List<string> CustomerGroupIds = SelectedCustomerGroup.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerGroup)).Distinct().ToList();
                    XmlElement ParentNodeCustomerGroups = doc.CreateElement("CustomerGroups");
                    element1.AppendChild(ParentNodeCustomerGroups);

                    foreach (string ObjCustomerGroup in CustomerGroupIds)
                    {
                        XmlElement childNode1 = doc.CreateElement("CustomerGroup");
                        XmlText CustomerGroup = doc.CreateTextNode(ObjCustomerGroup);
                        childNode1.AppendChild(CustomerGroup);
                        ParentNodeCustomerGroups.AppendChild(childNode1);
                    }
                }

                if (SelectedCustomerPlant != null)
                {
                    List<string> CustomerPlantIds = SelectedCustomerPlant.Select(i => Convert.ToString((i as PlantDeliveryAnalysis).CustomerPlant)).Distinct().ToList();
                    XmlElement ParentNodeCustomerPlants = doc.CreateElement("CustomerPlants");
                    element1.AppendChild(ParentNodeCustomerPlants);

                    foreach (string ObjCustomerPlant in CustomerPlantIds)
                    {
                        XmlElement childNode1 = doc.CreateElement("CustomerPlant");
                        XmlText CustomerGroup = doc.CreateTextNode(ObjCustomerPlant);
                        childNode1.AppendChild(CustomerGroup);
                        ParentNodeCustomerPlants.AppendChild(childNode1);
                    }
                }

                doc.Save(PlantDeliveryAnalysisFilterSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method CreateXmlFile() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateXmlFile() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        private void ReadXmlFile()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReadXmlFile ...", category: Category.Info, priority: Priority.Low);

                TempXMLRegionsList = new List<string>();
                TempXMLOTStatussList = new List<string>();
                TempXMLCategory1List = new List<long>();
                TempXMLCategory2List = new List<long>();
                TempXMLTemplatesList = new List<string>();
                TempXMLOTItemStatussList = new List<string>();
                TempXMLCustomerGroupsList = new List<string>();
                TempXMLCustomerPlantsList = new List<string>();

                if (File.Exists(PlantDeliveryAnalysisFilterSettingFilePath))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(PlantDeliveryAnalysisFilterSettingFilePath);

                    XmlNodeList NodeListRegions = doc.SelectNodes("/body/Regions");

                    foreach (XmlNode node in NodeListRegions)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLRegionsList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListOTStatuss = doc.SelectNodes("/body/OTStatuss");

                    foreach (XmlNode node in NodeListOTStatuss)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLOTStatussList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCategory1 = doc.SelectNodes("/body/Category1");

                    foreach (XmlNode node in NodeListCategory1)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCategory1List.Add(Convert.ToInt32(child.InnerText));
                        }
                    }

                    XmlNodeList NodeListCategory2 = doc.SelectNodes("/body/Category2");

                    foreach (XmlNode node in NodeListCategory2)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCategory2List.Add(Convert.ToInt32(child.InnerText));
                        }
                    }

                    XmlNodeList NodeListTemplates = doc.SelectNodes("/body/Templates");

                    foreach (XmlNode node in NodeListTemplates)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLTemplatesList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListOTItemStatuss = doc.SelectNodes("/body/OTItemStatuss");

                    foreach (XmlNode node in NodeListOTItemStatuss)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLOTItemStatussList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCustomerGroups = doc.SelectNodes("/body/CustomerGroups");

                    foreach (XmlNode node in NodeListCustomerGroups)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerGroupsList.Add(child.InnerText);
                        }
                    }

                    XmlNodeList NodeListCustomerPlants = doc.SelectNodes("/body/CustomerPlants");

                    foreach (XmlNode node in NodeListCustomerPlants)
                    {
                        foreach (XmlNode child in node.ChildNodes)
                        {
                            TempXMLCustomerPlantsList.Add(child.InnerText);
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ReadXmlFile() executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReadXmlFile() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void PlantDeliveryAnalysisGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantDeliveryAnalysisGridControlUnloadedCommandAction ...", category: Category.Info, priority: Priority.Low);

                CreateXmlFile();

                GeosApplication.Instance.Logger.Log("Method PlantDeliveryAnalysisGridControlUnloadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PlantDeliveryAnalysisGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion [GEOS2-5113][Rupali Sarode][06-12-2023]



        #region “Average Days PO to Shipment” chart 


        private void FindYearforAverageDayPOtoShipment()
        {
            try
            {

                DateTime tempFromDate = Convert.ToDateTime(FromDate);
                int year1 = tempFromDate.Year;
                DateTime tempToDate = Convert.ToDateTime(ToDate);
                int Year2 = tempToDate.Year;

                YearsAverageDayPOtoShipmentList = GetYearsBetween(year1, Year2);
                //dtSample = new DataTable();
                //dtSample.Columns.Add("MonthYear");
                //dtSample.Columns.Add("year");
                //foreach (int item in YearsList)
                //{
                //    // dt.Columns.Add(Convert.ToString(item));
                //    dtSample.Columns.Add("POGoAhead_" + item);
                //    dtSample.Columns.Add("Sample_" + item);
                //}

            }
            catch (Exception ex)
            {


            }
        }


        private void CreateDaysPOtoShipmentTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CreateDaysPOtoShipmentTable ...", category: Category.Info, priority: Priority.Low);

                //     dt.Rows.Clear();

                PlantNameList = new List<PlantDeliveryAnalysis>();
                POGoAheadAndSampleDaysList = new List<POGoAheadAndSampleDays>();

                List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                foreach (var Plantname in plantOwners)
                {
                    Int32 IdSite = Convert.ToInt32(Plantname.IdSite);


                    int Count = YearsAverageDayPOtoShipmentList.Count();
                    foreach (int year in YearsAverageDayPOtoShipmentList)
                    {
                        POGoAheadAndSampleDays POGoAheadAndSampleDays = new POGoAheadAndSampleDays();
                        POGoAheadAndSampleDays.Plant = Plantname.Name;
                        POGoAheadAndSampleDays.Year = Convert.ToString(year);

                        // DataRow dr = dtSample.NewRow();
                        //   dr[0] = Plantname.Name.ToString().PadLeft(2, '0');

                        // dr["year"] = year;
                        string Year = Convert.ToString(year);
                        int fyear = DateTime.Now.Year;
                        //if (fyear != year)
                        //{
                        var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                        if (tempPlantDeliveryAnalysisList != null)
                        {
                            List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                            templist = PlantDeliveryAnalysisList.Where(item =>
                            {
                                DateTime? selectedDate = null;
                                if (item.OTIdSite == IdSite)
                                {
                                    if (item.SamplesDate != null)
                                    {
                                        if (item.SamplesDate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.SamplesDate : (item.SamplesDate ?? item.PODate);
                                        }
                                        else if (item.PODate != null && item.PODate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                                        }
                                    }
                                    else
                                    {

                                        if (item.GoAheadDate != null && item.GoAheadDate.Value.Year == year)
                                        {
                                            selectedDate = item.GoAheadDate != null ? item.GoAheadDate : item.PODate;
                                        }
                                        else if (item.PODate != null && item.PODate.Value.Year == year)
                                        {
                                            selectedDate = item.PODate != null ? item.PODate : item.GoAheadDate;
                                        }
                                    }
                                }

                                return selectedDate != null;
                            }).ToList();

                            if (templist.Count > 0)
                            {



                                var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate && !a.DaystoShippment.Contains("-") && a.DaystoShippment != "0").ToList();//Aishwarya Ingale[Geos2-6431]
                                if (TotalRecordCount.Count > 0)
                                {
                                    List<int> totalSampleDays = new List<int>();
                                    List<int> totalPOorGoaheadDays = new List<int>();
                                    foreach (var item in TotalRecordCount)
                                    {
                                        if (item.Sample == "C")
                                        {
                                            if (item.SamplesDate != null)
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.SamplesDate.Value.Date).Days;
                                                totalSampleDays.Add(days);
                                            }

                                        }
                                        else
                                        {
                                            if (item.GoAheadDate != null)
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.GoAheadDate.Value.Date).Days;
                                                totalPOorGoaheadDays.Add(days);
                                            }
                                            else
                                            {
                                                int days = (item.ShippingDate.Value.Date - item.PODate.Value.Date).Days;
                                                totalPOorGoaheadDays.Add(days);
                                            }
                                        }

                                    }
                                    int sumofSampleDays = totalSampleDays.Sum(i => i);
                                    int AVGSampleDays = sumofSampleDays / TotalRecordCount.Count;
                                    int sumofPOGoAhead = totalPOorGoaheadDays.Sum(a => a);

                                    int AVGtotalPOorGoaheadDays = sumofPOGoAhead / TotalRecordCount.Count;
                                    // dr[Year] = AVGSampleDays + "," + AVGtotalPOorGoaheadDays;
                                    // dr["Sample_" + Year] = AVGSampleDays;
                                    //dr["POGoAhead_" + Year] = AVGtotalPOorGoaheadDays;

                                    POGoAheadAndSampleDays.PoGoheadDays = AVGtotalPOorGoaheadDays;
                                    POGoAheadAndSampleDays.SampleDays = AVGSampleDays;
                                }

                                //  dr[Year] = 100;
                            }
                        }
                        POGoAheadAndSampleDaysList.Add(POGoAheadAndSampleDays);
                        //}
                        //else
                        //{
                        //var tempPlantDeliveryAnalysisList = PlantDeliveryAnalysisList.Where(i => i.DeliveryDate.Value.Year == year).ToList();
                        //if (tempPlantDeliveryAnalysisList != null)
                        //{
                        //    List<PlantDeliveryAnalysis> templist = new List<PlantDeliveryAnalysis>();

                        //    templist = PlantDeliveryAnalysisList.Where(item =>
                        //    {
                        //        DateTime? selectedDate = null;
                        //        if (item.OTIdSite == IdSite)
                        //        {
                        //            if (item.GoAheadDate != null)
                        //            {
                        //                if (item.GoAheadDate.Value.Year == year)
                        //                {
                        //                    selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                        //                }
                        //                else if (item.PODate != null && item.PODate.Value.Year == year)
                        //                {
                        //                    selectedDate = item.PODate != null ? item.GoAheadDate : (item.GoAheadDate ?? item.PODate);
                        //                }
                        //            }
                        //            else
                        //            {

                        //                if (item.PODate != null && item.PODate.Value.Year == year)
                        //                {
                        //                    selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                        //                }
                        //                else if (item.SamplesDate != null && item.SamplesDate.Value.Year == year)
                        //                {
                        //                    selectedDate = item.PODate != null ? item.PODate : item.SamplesDate;
                        //                }
                        //            }
                        //        }

                        //        return selectedDate != null;
                        //    }).ToList();
                        //    if (templist.Count > 0)
                        //    {

                        //        //result.Where(a=>a.v)
                        //        var TotalRecordCount = templist.Where(a => a.ShippingDate <= a.DeliveryDate).ToList();
                        //        if (TotalRecordCount.Count > 0)
                        //        {
                        //            List<int> totalDays = new List<int>();
                        //            foreach (var item in TotalRecordCount)
                        //            {
                        //                int days = (item.DeliveryDate.Value.Date - item.ShippingDate.Value.Date).Days;
                        //                totalDays.Add(days);
                        //            }
                        //            int sumofDays = totalDays.Sum(i => i);
                        //            int AVGDays = sumofDays / TotalRecordCount.Count;

                        //            dr[Year] = AVGDays;
                        //        }

                        //        //  dr[Year] = 100;
                        //    }
                        //}
                        // }
                        // dt.Rows.Add(dr);
                    }





                }


                GeosApplication.Instance.Logger.Log("Method CreateDaysPOtoShipmentTable() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateDaysPOtoShipmentTable() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            POtoShipmentDataTable = dt;



            if (chartControl != null)
            {
                chartControl.UpdateData();
                Createloadchart();
            }
        }

        private void ChartDaysPOtoShipmentLoadAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method ChartLoadAction ...", category: Category.Info, priority: Priority.Low);

                chartControlSample = (ChartControl)obj;
                // chartcontrol.DataSource = GraphDataTable;
                //chartControl.BeginInit();

                Createloadchart();



                //diagram.ActualAxisX.ActualLabel.Angle = 270;

                chartControlSample.EndInit();
                chartControlSample.AnimationMode = ChartAnimationMode.OnDataChanged;
                chartControlSample.Animate();

                GeosApplication.Instance.Logger.Log("Method ChartLoadAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChartLoadAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void Createloadchart()
        {
            XYDiagram2D diagramSample = new XYDiagram2D();
            chartControlSample.Diagram = diagramSample;
            chartControlSample.BeginInit();
            //diagram.ActualAxisX.Title = new AxisTitle() { Content = "Months" };
            //diagram.ActualAxisY.Title = new AxisTitle() { Content = "Amount" };
            diagramSample.ActualAxisX.QualitativeScaleOptions = new QualitativeScaleOptions();
            diagramSample.ActualAxisX.QualitativeScaleOptions.AutoGrid = false;
            // GeosApplication.SetCurrencySymbol(GeosApplication.Instance.CurrentCurrencySymbol);
            diagramSample.ActualAxisY.NumericOptions = new NumericOptions();
            //     diagram.ActualAxisY.GridSpacing = 2000; // Set the interval of 50 values on the y-axis
            diagramSample.ActualAxisY.NumericOptions.Format = NumericFormat.General;
            AxisY2D axisY = new AxisY2D();
            AxisTitle axisYTitle = new AxisTitle();
            axisYTitle.Content = "Zero = Agreed ETD";
            axisY.Title = axisYTitle;
            diagramSample.AxisY = axisY;
            diagramSample.ActualAxisX.Label = new AxisLabel();
            diagramSample.ActualAxisX.Label.Angle = 45;
            #region RND
            if (POGoAheadAndSampleDaysList != null)
            {
                if (POGoAheadAndSampleDaysList.Count() > 0)
                {
                    var PlantName = POGoAheadAndSampleDaysList.GroupBy(a => a.Plant).ToList();
                    if (PlantName.Count() > 0)
                    {

                        foreach (var plantitem in PlantName)
                        {
                            //Legend legend = new Legend();
                            //legend.HorizontalPosition = HorizontalPosition.Center;
                            //legend.VerticalPosition = VerticalPosition.TopOutside;
                            //legend.Orientation = Orientation.Horizontal;
                            // string PlantName = Convert.ToString(plantitem.Key);
                            var YearGroup = POGoAheadAndSampleDaysList.Where(x => x.Plant == plantitem.Key).GroupBy(a => a.Year).ToList();
                            if (YearGroup.Count() > 0)
                            {
                                int YearCount = 1;
                                foreach (var yearitem in YearGroup)
                                {



                                    var YearRecord = POGoAheadAndSampleDaysList.Where(x => x.Plant == plantitem.Key && x.Year == yearitem.Key).FirstOrDefault();
                                    if (YearRecord != null)
                                    {

                                        BarSideBySideStackedSeries2D firstSeries = new BarSideBySideStackedSeries2D
                                        {
                                            DisplayName = yearitem.Key + "(Sample Days)",
                                            StackedGroup = YearCount,
                                            BarWidth = 0.5,
                                            ArgumentScaleType = ScaleType.Auto,
                                            ValueScaleType = ScaleType.Numerical,
                                            LabelsVisibility = true,
                                            ShowInLegend = false,
                                            //  Model = new Quasi3DBar2DModel()

                                            Label = new SeriesLabel
                                            {
                                                Visible = true,

                                                TextOrientation = TextOrientation.BottomToTop

                                                //Padding = new Thickness(0,0,0, paddingbottum)


                                            }
                                        };



                                        //firstSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.SampleDays));
                                        string graycolor = "#3F76BF";
                                        firstSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.SampleDays)
                                        {
                                            Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(graycolor))

                                        });

                                        // Create the second BarSideBySideStackedSeries2D
                                        BarSideBySideStackedSeries2D secondSeries = new BarSideBySideStackedSeries2D
                                        {
                                            DisplayName = yearitem.Key + "(GoAhead PO Days)",
                                            StackedGroup = YearCount,
                                            BarWidth = 0.5,
                                            ArgumentScaleType = ScaleType.Auto,
                                            ValueScaleType = ScaleType.Numerical,
                                            LabelsVisibility = true,
                                            ShowInLegend = false,

                                            Label = new SeriesLabel
                                            {
                                                Visible = true,

                                                TextOrientation = TextOrientation.BottomToTop

                                                //Padding = new Thickness(0,0,0, paddingbottum)


                                            }

                                            //Model = new Quasi3DBar2DModel()
                                        };




                                        // secondSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.PoGoheadDays));
                                        string bluecolor = "#9E9E9E";
                                        secondSeries.Points.Add(new SeriesPoint(plantitem.Key, YearRecord.PoGoheadDays)
                                        {
                                            Brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(bluecolor)),


                                        });


                                        diagramSample.Series.Add(firstSeries);
                                        diagramSample.Series.Add(secondSeries);
                                        Legend legend = new Legend
                                        {
                                            Name = "year" + yearitem.Key,
                                            HorizontalPosition = HorizontalPosition.Center,
                                            VerticalPosition = VerticalPosition.TopOutside,
                                            Orientation = Orientation.Horizontal
                                        };

                                        chartControlSample.Legends.Add(legend);

                                        firstSeries.Legend = legend;
                                        secondSeries.Legend = legend;
                                    }
                                    YearCount++;


                                }
                            }
                            //  chartControl.Legends.Add(legend);
                        }
                    }

                }
            }




            chartControlSample.Diagram = diagramSample;





            #endregion


            chartControlSample.EndInit();
            chartControlSample.AnimationMode = ChartAnimationMode.OnDataChanged;
            chartControlSample.Animate();
        }
        #endregion

        private void ShowPOListDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPOListDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                PDAShowPOListViewModel PDAShowPOListViewModel = new PDAShowPOListViewModel();
                PDAShowPOListView PDAShowPOListView = new PDAShowPOListView();

                EventHandler handle = delegate { PDAShowPOListView.Close(); };

                PDAShowPOListViewModel.PlantDeliveryAnalysisList = PlantDeliveryAnalysisList;
                // PDAShowPOListViewModel.ERMDeliveryVisualManagementStagesList = ERMDeliveryVisualManagementStagesList;

                PDAShowPOListViewModel.RequestClose += handle;
                PDAShowPOListView.DataContext = PDAShowPOListViewModel;
                //  PDAShowPOListViewModel.Init();
                PDAShowPOListView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowPOListDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPOListDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowOnTimeDeliveryDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowOnTimeDeliveryDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                PDAShowOnTimeDeliveryViewModel PDAShowOnTimeDeliveryViewModel = new PDAShowOnTimeDeliveryViewModel();
                PDAShowOnTimeDeliveryView PDAShowOnTimeDeliveryView = new PDAShowOnTimeDeliveryView();

                EventHandler handle = delegate { PDAShowOnTimeDeliveryView.Close(); };

                PDAShowOnTimeDeliveryViewModel.ONTimeDeliveryANDAVGDeliveryList = ONTimeDeliveryANDAVGDeliveryList;

                PDAShowOnTimeDeliveryViewModel.RequestClose += handle;
                PDAShowOnTimeDeliveryView.DataContext = PDAShowOnTimeDeliveryViewModel;
                PDAShowOnTimeDeliveryViewModel.Init();
                PDAShowOnTimeDeliveryView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowOnTimeDeliveryDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowOnTimeDeliveryDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowAvgDeliveryDaysDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAvgDeliveryDaysDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                PDAShowAvgDeliveryDaysViewModel PDAShowAvgDeliveryDaysViewModel = new PDAShowAvgDeliveryDaysViewModel();
                PDAShowAvgDeliveryDaysView PDAShowAvgDeliveryDaysView = new PDAShowAvgDeliveryDaysView();

                EventHandler handle = delegate { PDAShowAvgDeliveryDaysView.Close(); };

                PDAShowAvgDeliveryDaysViewModel.ONTimeDeliveryANDAVGDeliveryList = ONTimeDeliveryANDAVGDeliveryList;

                PDAShowAvgDeliveryDaysViewModel.RequestClose += handle;
                PDAShowAvgDeliveryDaysView.DataContext = PDAShowAvgDeliveryDaysViewModel;
                PDAShowAvgDeliveryDaysViewModel.Init();
                PDAShowAvgDeliveryDaysView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowAvgDeliveryDaysDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAvgDeliveryDaysDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowAverageDeliveryDaysXPlantDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAverageDeliveryDaysXPlantDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                PDAShowAverageDeliveryDaysXPlantViewModel PDAShowAverageDeliveryDaysXPlantViewModel = new PDAShowAverageDeliveryDaysXPlantViewModel();
                PDAShowAverageDeliveryDaysXPlantView PDAShowAverageDeliveryDaysXPlantView = new PDAShowAverageDeliveryDaysXPlantView();

                EventHandler handle = delegate { PDAShowAverageDeliveryDaysXPlantView.Close(); };

                PDAShowAverageDeliveryDaysXPlantViewModel.PlantDeliveryAnalysisList = PlantDeliveryAnalysisList;
                PDAShowAverageDeliveryDaysXPlantViewModel.FromDate = FromDate;
                PDAShowAverageDeliveryDaysXPlantViewModel.ToDate = ToDate;
                PDAShowAverageDeliveryDaysXPlantViewModel.RequestClose += handle;
                PDAShowAverageDeliveryDaysXPlantView.DataContext = PDAShowAverageDeliveryDaysXPlantViewModel;


                PDAShowAverageDeliveryDaysXPlantViewModel.Init();
                PDAShowAverageDeliveryDaysXPlantView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowAverageDeliveryDaysXPlantDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPOListDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ShowAverageDaysPOShipmentDialogWindowCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowAverageDaysPOShipmentDialogWindowCommandAction()...", category: Category.Info, priority: Priority.Low);

                PDAShowAverageDaysPOViewModel PDAShowAverageDaysPOViewModel = new PDAShowAverageDaysPOViewModel();
                PDAShowAverageDaysPOView PDAShowAverageDaysPOView = new PDAShowAverageDaysPOView();

                EventHandler handle = delegate { PDAShowAverageDaysPOView.Close(); };

                PDAShowAverageDaysPOViewModel.FromDate = FromDate;
                PDAShowAverageDaysPOViewModel.ToDate = ToDate;

                PDAShowAverageDaysPOViewModel.YearsAverageDayPOtoShipmentList = YearsAverageDayPOtoShipmentList;
                PDAShowAverageDaysPOViewModel.PlantDeliveryAnalysisList = PlantDeliveryAnalysisList;
                PDAShowAverageDaysPOViewModel.RequestClose += handle;
                PDAShowAverageDaysPOView.DataContext = PDAShowAverageDaysPOViewModel;
                PDAShowAverageDaysPOViewModel.Init();

                PDAShowAverageDaysPOView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method ShowAverageDaysPOShipmentDialogWindowCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowAverageDaysPOShipmentDialogWindowCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ExportplantdeliveryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportplantdeliveryCommandAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Plant Delivery Analysis";
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
                    GeosApplication.Instance.Logger.Log("Method ExportplantdeliveryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportplantdeliveryCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion


    }

}
