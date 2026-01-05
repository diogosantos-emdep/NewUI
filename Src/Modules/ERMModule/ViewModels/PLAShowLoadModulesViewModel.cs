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
using Emdep.Geos.UI.Commands;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class PLAShowLoadModulesViewModel : ViewModelBase, INotifyPropertyChanged
    {
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
        #endregion Events

        #region Command
        public ICommand CloseButtonCommand { get; set; }
        public ICommand GridActionLoadCommand { get; set; }


        #endregion Command
        #region Declaration
        private GridControl FutureGridControl;
        private GridControl futureGridControlFilter;
        private ObservableCollection<BandItem> bandsDashboard = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutProductionInTime;
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisCopy;
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysis;
        private List<PlantLoadAnalysis> plantLoadAnalysisList;
        #endregion Declaration

        #region Property

        public List<PlantLoadAnalysis> PlantLoadAnalysisList
        {
            get { return plantLoadAnalysisList; }
            set
            {
                plantLoadAnalysisList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantLoadAnalysisList"));
            }
        }
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysis
        {
            get { return groupByTempPlantLoadAnalysis; }
            set
            {
                groupByTempPlantLoadAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysis"));
            }
        }
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisCopy
        {
            get { return groupByTempPlantLoadAnalysisCopy; }
            set
            {
                groupByTempPlantLoadAnalysisCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisCopy"));
            }
        }
        public DataTable DataTableForGridLayoutProductionInTime
        {
            get
            {
                return dataTableForGridLayoutProductionInTime;
            }
            set
            {
                dataTableForGridLayoutProductionInTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutProductionInTime"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummary { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummary { get; private set; }

        public ObservableCollection<BandItem> BandsDashboard
        {
            get { return bandsDashboard; }
            set
            {
                bandsDashboard = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboard"));
            }
        }
        public GridControl FutureGridControlFilter
        {
            get
            {
                return futureGridControlFilter;
            }
            set
            {
                futureGridControlFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FutureGridControlFilter"));
            }
        }
        #endregion Property

        #region Constructor
        public PLAShowLoadModulesViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            GridActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCommandAction);

        }
        #endregion Constructor

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                AddColumnsToDataTableWithBandsinLoadModules();
             //   FillLoadModulesData();
                BandsDashboard = new ObservableCollection<BandItem>(BandsDashboard);
               // DataTableForGridLayoutProductionInTime = new DataTable(DataTableForGridLayoutProductionInTime);
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GridActionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                FutureGridControl = (GridControl)obj;

                FutureGridControlFilter = FutureGridControl;
                FutureGridControl.GroupBy("CalenderWeek");
                FutureGridControl.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTableWithBandsinLoadModules()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadModules ...", category: Category.Info, priority: Priority.Low);

                //if (FutureGridControlFilter == null)
                //{
                //    FutureGridControlFilter = new GridControl();
                //}

                //GridControlBand band = new GridControlBand
                //{
                //    Header = "Your Band Header",
                //    Visible = false
                //};
                //GridColumn column = new GridColumn();
                //BandsDashboard = new ObservableCollection<BandItem>(); BandsDashboard.Clear();
                //BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = "Plant", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };
                //band0.Columns = new ObservableCollection<ColumnItem>();
                //band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CalenderWeek", Header = "", Width = 120, Visible = false });
                //band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "OTCode", Header = "", Width = 120, Visible = false });
                ////band.Columns.Add(new DevExpress.Xpf.Grid.GridColumn() { FieldName = "CPType", Header = "", Width = 120, Visible = false });
                //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CalenderWeek", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CalenderWeek, Visible = false });
                //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.OTCode, Visible = true });
                //band0.Columns.Add(new ColumnItem() { ColumnFieldName = "CPType", HeaderText = "YearWeek", Width = 120, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.CPType, Visible = false });
                //// band0.Columns.Add(new ColumnItem() { ColumnFieldName = "OTCode", HeaderText = "", Width = 120, IsVertical = false, Settings = SettingsType.OTCode, Visible = true });

                //BandsDashboard.Add(band0);
                GridSummaryItem GridSummaryItem = new GridSummaryItem();

                GroupSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                //DataTableForGridLayoutProductionInTime = new DataTable();
                //DataTableForGridLayoutProductionInTime.Columns.Add("CalenderWeek", typeof(string));
                //DataTableForGridLayoutProductionInTime.Columns.Add("OTCode", typeof(string));
                //DataTableForGridLayoutProductionInTime.Columns.Add("CPType", typeof(string));

                
                //GroupByTempPlantLoadAnalysisCopy = new List<PlantLoadAnalysis>();
                
                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {


                    //FutureGridControlFilter.GroupSummary.Clear();
                    //FutureGridControlFilter.TotalSummary.Clear();

                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    foreach (var SelectedPlant in plantOwners)
                    {

                        GridSummaryItem = new GridSummaryItem();
                        //GroupByTempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
                        // Outer BandItem
                        //BandItem outerBand = new BandItem()
                        //{
                        //    BandName = Convert.ToString(SelectedPlant.Name),
                        //    BandHeader = Convert.ToString(SelectedPlant.Name),
                        //    Visible = true
                        //};

                        //outerBand.Columns = new ObservableCollection<ColumnItem>();
                        //BandsDashboard.Add(outerBand);

                        //List<PlantLoadAnalysis> TempPlantLoadAnalysis = new List<PlantLoadAnalysis>();
                        //TempPlantLoadAnalysis = PlantLoadAnalysisList.Where(i => i.ProductionIdSite == SelectedPlant.IdSite).ToList();
                        //GroupByTempPlantLoadAnalysis.AddRange(TempPlantLoadAnalysis.GroupBy(i => i.OriginalIdSite)
                        //                                                         .Select(grp => grp.First())
                        //                                                         .ToList().Distinct());

                        //GroupByTempPlantLoadAnalysisCopy.AddRange(GroupByTempPlantLoadAnalysis);



                        foreach (var item in GroupByTempPlantLoadAnalysis)
                        {


                            //outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, HeaderText = item.OriginalSiteName.ToString(), Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Plant });

                            //DataTableForGridLayoutProductionInTime.Columns.Add("Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, typeof(Int32));

                            GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                            TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite, DisplayFormat = "{0}" });

                            //FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                            //{
                            //    FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite,
                            //    SummaryType = SummaryItemType.Sum,
                            //    DisplayFormat = "{0}"
                            //});

                            //FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                            //{
                            //    FieldName = "Plant_" + item.OriginalIdSite + SelectedPlant.IdSite,
                            //    SummaryType = SummaryItemType.Sum,
                            //    DisplayFormat = "{0}"
                            //});
                        }
                        //outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Total" + SelectedPlant.IdSite, HeaderText = "Total", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
                        //DataTableForGridLayoutProductionInTime.Columns.Add("Total" + SelectedPlant.IdSite, typeof(Int32));
                        GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total" + SelectedPlant.IdSite, DisplayFormat = "{0}" });


                        //FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                        //{
                        //    FieldName = "Total" + SelectedPlant.IdSite,
                        //    SummaryType = SummaryItemType.Sum,
                        //    DisplayFormat = "{0}"
                        //});

                        //FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                        //{
                        //    FieldName = "Total" + SelectedPlant.IdSite,
                        //    SummaryType = SummaryItemType.Sum,
                        //    DisplayFormat = "{0}"
                        //});
                    }


                }
                //BandItem TotaBand = new BandItem()
                //{
                //    BandName = "GrandTotal",
                //    BandHeader = "Total",
                //    Visible = true,
                //    AllowBandMove = false,
                //    FixedStyle = FixedStyle.Right,

                //};
                //TotaBand.Columns = new ObservableCollection<ColumnItem>();
                //BandsDashboard.Add(TotaBand);
                //TotaBand.Columns.Add(new ColumnItem() { ColumnFieldName = "GrandTotal", HeaderText = "", Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.Total });
                //DataTableForGridLayoutProductionInTime.Columns.Add("GrandTotal", typeof(Int32));
                GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CPType", DisplayFormat = "Total" });

                //FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                //{
                //    FieldName = "CPType",
                //    SummaryType = SummaryItemType.Count,
                //    DisplayFormat = "Total"
                //});

                //FutureGridControlFilter.TotalSummary.Add(new GridSummaryItem
                //{
                //    FieldName = "GrandTotal",
                //    SummaryType = SummaryItemType.Sum,
                //    DisplayFormat = "{0}"
                //});

                //FutureGridControlFilter.GroupSummary.Add(new GridSummaryItem
                //{
                //    FieldName = "GrandTotal",
                //    SummaryType = SummaryItemType.Sum,
                //    DisplayFormat = "{0}"
                //});
                //BandsDashboard = new ObservableCollection<BandItem>(BandsDashboard);

                //FutureGridControlFilter.Bands.Add(band);


                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadModules executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadModules() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLoadModulesData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadModulesData ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutProductionInTime.Clear();
                DataTable DataTableForGridLayoutPlantLoadAnalysisCopy = DataTableForGridLayoutProductionInTime.Copy();
               
                for (int i = 0; i < PlantLoadAnalysisList.Count; i++)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                   
                    int GrandTotal = 0;
                    int totalQTY = 0;
                    
                    try
                    {
                        DataTable dataTable = DataTableForGridLayoutPlantLoadAnalysisCopy;

                        DataRow[] foundRows = dataTable.Select($"OTCode = '{PlantLoadAnalysisList[i]?.OTCode}' AND CalenderWeek = '{PlantLoadAnalysisList[i]?.DeliveryWeek}' AND CPType = '{PlantLoadAnalysisList[i]?.ConnectorFamily}'");

                        var groupBY = PlantLoadAnalysisList.GroupBy(a => a.IdOTItem).First();

                        int groupedData = PlantLoadAnalysisList.Where(a => a.DeliveryWeek == PlantLoadAnalysisList[i].DeliveryWeek && a.OriginalIdSite == PlantLoadAnalysisList[i].OriginalIdSite && a.ConnectorFamily == PlantLoadAnalysisList[i].ConnectorFamily && a.OTCode == PlantLoadAnalysisList[i].OTCode).GroupBy(a => new { a.IdOTItem }).Sum(group => group.Sum(a => a.QTY));

                        if (foundRows.Length > 0)
                        {

                            foundRows[0]["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
                            totalQTY = totalQTY + groupedData;

                            foundRows[0]["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
                            GrandTotal = GrandTotal + totalQTY;
                            foundRows[0]["GrandTotal"] = GrandTotal;
                        }
                        else
                        {
                            DataRow dr = DataTableForGridLayoutPlantLoadAnalysisCopy.NewRow();
                            dr["CalenderWeek"] = PlantLoadAnalysisList[i].DeliveryWeek;
                            dr["OTCode"] = PlantLoadAnalysisList[i].OTCode;
                            dr["CPType"] = PlantLoadAnalysisList[i].ConnectorFamily;
                            dr["Plant_" + PlantLoadAnalysisList[i].OriginalIdSite + PlantLoadAnalysisList[i].ProductionIdSite] = groupedData;
                            totalQTY = totalQTY + groupedData;

                            dr["Total" + PlantLoadAnalysisList[i].ProductionIdSite] = totalQTY;
                            GrandTotal = GrandTotal + totalQTY;
                            dr["GrandTotal"] = GrandTotal;
                            DataTableForGridLayoutPlantLoadAnalysisCopy.Rows.Add(dr);
                        }
                    }

                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                var columnsToRemove = DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.Cast<DataColumn>()
         .Where(col => DataTableForGridLayoutPlantLoadAnalysisCopy.AsEnumerable().All(row => row.IsNull(col)))
         .ToList();
               // IsNotNull = true;
                
                // Remove the identified columns
                foreach (var col in columnsToRemove)
                {
                    if (!col.ColumnName.Contains("Total"))
                    {
                        DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.Remove(col);
                        //     DataTableForGridLayoutPlantLoadAnalysisCopy.Columns.RemoveAt(0);
                    }
                }
                if (columnsToRemove.Count > 0)
                {
                    AddColumnsToDataTableWithBandsinLoadModules();
                }
                DataTableForGridLayoutProductionInTime = DataTableForGridLayoutPlantLoadAnalysisCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                throw;
            }
        }

        #endregion Methods

    }
}
