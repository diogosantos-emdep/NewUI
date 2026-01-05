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
    public class PLAShowLoadWorkStationViewModel : ViewModelBase, INotifyPropertyChanged
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
        public ICommand GridActionLoadWorkstationCommand { get; set; }

        #endregion Command

        #region Declaration
        private ObservableCollection<BandItem> bandsDashboardLoadWorkstation = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutLoadWorkstation;
        private GridControl gridControlLoadWorkstation;
        private GridControl gridControlLoadWorkstationFilter;
        private List<DeliveryVisualManagementStages> loadWorkstationStageList;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsLoadWorkstation;
        private List<PlantLoadAnalysis> plantLoadAnalysisList;
        #endregion Declaration

        #region Property
        public DataTable DataTableForGridLayoutLoadWorkstation
        {
            get
            {
                return dataTableForGridLayoutLoadWorkstation;
            }
            set
            {
                dataTableForGridLayoutLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadWorkstation"));
            }
        }

        public GridControl GridControlLoadWorkstation
        {
            get
            {
                return gridControlLoadWorkstation;
            }
            set
            {
                gridControlLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadWorkstation"));
            }
        }

        public GridControl GridControlLoadWorkstationFilter
        {
            get
            {
                return gridControlLoadWorkstationFilter;
            }
            set
            {
                gridControlLoadWorkstationFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadWorkstationFilter"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadWorkstation { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadWorkstation { get; private set; }

        public List<DeliveryVisualManagementStages> LoadWorkstationStageList
        {
            get
            {
                return loadWorkstationStageList;
            }
            set
            {
                loadWorkstationStageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadWorkstationStageList"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsLoadWorkstation
        {
            get { return columnsLoadWorkstation; }
            set
            {
                columnsLoadWorkstation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsLoadWorkstation"));
            }
        }

        public List<PlantLoadAnalysis> PlantLoadAnalysisList
        {
            get { return plantLoadAnalysisList; }
            set
            {
                plantLoadAnalysisList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantLoadAnalysisList"));
            }
        }
        //private string groupColor;
        //public string GroupColor
        //{
        //    get { return groupColor; }
        //    set
        //    {
        //        groupColor = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("GroupColor"));
        //    }
        //}
        #endregion Property

        #region Constructor
        public PLAShowLoadWorkStationViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            GridActionLoadWorkstationCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadWorkstationCommandAction);  //[GEOS2-5039][Rupali Sarode][27-12-2023]

        }
        #endregion Constructor

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void AddColumnsToDataTableWithBandsinLoadWorkstation()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadWorkstation ...", category: Category.Info, priority: Priority.Low);

                ColumnsLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CalenderWeek", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerWithPlant", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Project", HeaderText="", Settings = SettingsType.CalendarWeekPDAWorkstation, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="OTCode", HeaderText="", Settings = SettingsType.PDAOTCode, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="ItemNumber", HeaderText="YearWeek", Settings = SettingsType.PDAOTCode, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };


                if (GridControlLoadWorkstationFilter != null)
                {
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "CalenderWeek", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "CustomerWithPlant", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "Project", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "OTCode", Visible = false });
                    GridControlLoadWorkstationFilter.Columns.Add(new GridColumn { FieldName = "ItemNumber", Visible = false });

                }


                DataTableForGridLayoutLoadWorkstation = new DataTable();
                DataTableForGridLayoutLoadWorkstation.Columns.Add("CalenderWeek", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("CustomerWithPlant", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("Project", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("OTCode", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("ItemNumber", typeof(string));
                DataTableForGridLayoutLoadWorkstation.Columns.Add("GroupColor", typeof(bool));

                GroupSummaryLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadWorkstation = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                ColumnsLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "TempColumn", HeaderText = "TempColumn", Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = false, IsVertical = false, FixedWidth = true, IsReadOnly = true });

                string StageName = string.Empty;
                //int j = 0;
                foreach (DeliveryVisualManagementStages item in LoadWorkstationStageList)
                {

                    StageName = "Plant_" + item.IdStage;

                    //       outerBand.Columns.Add(new ColumnItem() { ColumnFieldName = "Plant_" + item.IdStage + j, HeaderText = item.StageName.ToString(), Width = 70, Visible = true, IsVertical = false, PlantLoadAnalysisetting = PlantLoadAnalysisTemplateSelector.PlantLoadAnalysisSettingType.DefaultTemplate });
                    //       DataTableForGridLayoutLoadWorkstation.Columns.Add("Plant_" + item.IdStage + j, typeof(string));

                    ColumnsLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "Plant_" + item.IdStage, HeaderText = item.StageCode, Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 90, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true, IsReadOnly = true });

                    DataTableForGridLayoutLoadWorkstation.Columns.Add("Plant_" + item.IdStage, typeof(string));

                    GroupSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = StageName, DisplayFormat = "{0}" });
                    TotalSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = StageName, DisplayFormat = "{0}" });

                }
                TotalSummaryLoadWorkstation.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "ItemNumber", DisplayFormat = "Total" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadWorkstation executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadWorkstation() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillLoadWorkstationData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadWorkstationData ...", category: Category.Info, priority: Priority.Low);

                DataTableForGridLayoutLoadWorkstation.Clear();
                DataTable DataTableForGridLayoutLoadWorkstationCopy = DataTableForGridLayoutLoadWorkstation.Copy();
               string ColumnName = string.Empty;

                var groupedDataWorkStation = PlantLoadAnalysisList
                        .GroupBy(item => new { item.DeliveryWeek, item.CustomerWithPlant, item.Project, item.OTCode, item.ItemNumber, item.IdStage })
                        .Select(group => new
                        {
                            DeliveryWeek = group.Key.DeliveryWeek,
                            CustomerWithPlant = group.Key.CustomerWithPlant,
                          //CustomerPlant = group.Key.CustomerPlant,
                          Project = group.Key.Project,
                            OTCode = group.Key.OTCode,
                            IdStage = group.Key.IdStage,
                            ItemNumber = group.Key.ItemNumber,
                            TotalQTY = group.Sum(item => item.QTY)
                        }).OrderBy(a=>a.ItemNumber);
                //var groupedDataWorkStation = PlantLoadAnalysisList
                //   .GroupBy(item => new { item.DeliveryWeek, item.ItemNumber, item.IdStage })
                //   .Select(grp => grp.FirstOrDefault()).OrderBy(a=>a.ItemNumber).ToList();
                foreach (var groupItem in groupedDataWorkStation)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(groupItem.IdStage.ToString()) || groupItem.IdStage == 0)
                        {
                            continue; // Skip items with invalid IdStage
                        }

                         ColumnName = "Plant_" + groupItem.IdStage;
                        if (!DataTableForGridLayoutLoadWorkstationCopy.Columns.Contains(ColumnName))
                        {
                            DataTableForGridLayoutLoadWorkstationCopy.Columns.Add(ColumnName);
                        }

                        // Process each item separately
                        DataRow dr;
                        string Condition = "CalenderWeek = '" + groupItem.DeliveryWeek +
                                           "' and CustomerWithPlant = '" + groupItem.CustomerWithPlant +
                                           "' and Project = '" + groupItem.Project +
                                           "' and OTCode = '" + groupItem.OTCode +
                                           "' and ItemNumber = '" + groupItem.ItemNumber + "'";
                        //object cellValue = DataTableForGridLayoutLoadWorkstation.Rows[0]["CalenderWeek"];

                        DateTime currentDate = DateTime.Now;
                        int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                        int year = DateTime.Now.Year;
                        string Week = year + "CW" + currentWeek;
                     //   var rowsInCurrentWeek = DataTableForGridLayoutLoadWorkstation.AsEnumerable().Where(row => Convert.ToString(row["CalenderWeek"]) == Week).Select(row => row["CalenderWeek"]);
                        //GroupColor = string.Empty;
                        
                        DataRow[] ExistedRow = DataTableForGridLayoutLoadWorkstationCopy.Select(Condition);

                        if (ExistedRow != null && ExistedRow.Length > 0)
                        {
                            dr = ExistedRow.FirstOrDefault(); // Use existing row
                        }
                        else
                        {
                            dr = DataTableForGridLayoutLoadWorkstationCopy.NewRow(); // Create new row if no existing row is found
                        }

                        // Set common fields
                        dr["CalenderWeek"] = groupItem.DeliveryWeek;
                        dr["CustomerWithPlant"] = groupItem.CustomerWithPlant;
                        dr["Project"] = groupItem.Project;
                        dr["OTCode"] = groupItem.OTCode;
                        dr["ItemNumber"] = groupItem.ItemNumber;
                        dr["GroupColor"] = false;
                        if (groupItem.DeliveryWeek == Week)
                        {
                            // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                            //GroupColor = Week;
                            dr["GroupColor"] = true;
                        }
                        // Handle Quantity
                        if (DataTableForGridLayoutLoadWorkstationCopy.Columns.Contains(ColumnName))
                        {
                            dr[ColumnName] = groupItem.TotalQTY; // Set Quantity
                        }

                        // Add or update the row in the DataTable
                        if (ExistedRow == null || ExistedRow.Length == 0)
                        {
                            DataTableForGridLayoutLoadWorkstationCopy.Rows.Add(dr); // Add new row if it doesn't exist
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
                //foreach (var groupItem in groupedDataWorkStation)
                //{
                //    try
                //    {
                //        if (string.IsNullOrEmpty(Convert.ToString(groupItem.IdStage)) || groupItem.IdStage == 0)
                //        {

                //        }
                //        else
                //        {

                //            DataRow dr;
                //            string Condition = "CalenderWeek = '" + groupItem.DeliveryWeek +
                //                "' and CustomerWithPlant = '" + groupItem.CustomerWithPlant +
                //                "' and Project = '" + groupItem.Project +
                //                "' and OTCode = '" + groupItem.OTCode + "'";

                //            DataRow[] ExistedRow = DataTableForGridLayoutLoadWorkstationCopy.Select(Condition);
                //            if (ExistedRow != null)
                //            {
                //                if (ExistedRow.Length > 0)
                //                {
                //                    dr = ExistedRow.FirstOrDefault();
                //                }
                //                else
                //                    dr = DataTableForGridLayoutLoadWorkstationCopy.NewRow();
                //            }
                //            else
                //                dr = DataTableForGridLayoutLoadWorkstationCopy.NewRow();

                //            dr["CalenderWeek"] = groupItem.DeliveryWeek;
                //            dr["CustomerWithPlant"] = groupItem.CustomerWithPlant;
                //            dr["Project"] = groupItem.Project;
                //            dr["OTCode"] = groupItem.OTCode;

                //            ColumnName = "Plant_" + groupItem.IdStage;
                //            if (DataTableForGridLayoutLoadWorkstationCopy.Columns.Contains(ColumnName))
                //            {
                //                dr[ColumnName] = groupItem.TotalQTY;
                //            }

                //            if (ExistedRow.Length == 0)
                //            {
                //                DataTableForGridLayoutLoadWorkstationCopy.Rows.Add(dr);
                //            }
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //        GeosApplication.Instance.Logger.Log("Error in FillLoadWorkstationData() - 1 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                //    }
                //}

                
                
                DataTableForGridLayoutLoadWorkstation = DataTableForGridLayoutLoadWorkstationCopy;

                //
                //foreach (DataRow row in DataTableForGridLayoutLoadWorkstation.Rows)
                //{
                //    if(Convert.ToString(row["CalenderWeek"]) == Week)
                //    {
                //        GroupColor = Week;
                //    }
                //    else
                //    {
                //        GroupColor = "";
                //    }
                //}
                //GroupColor = DataTableForGridLayoutLoadWorkstation.Columns.Contains(Week);
                //if(Week == cellValue.ToString())
                //{
                //    GroupColor = "#0000FF";
                //}




                GeosApplication.Instance.Logger.Log("Method FillLoadWorkstationData executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }


            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillLoadWorkstationData() - 2 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void GridActionLoadWorkstationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadWorkstationCommandAction()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadWorkstation = (GridControl)obj;

                GridControlLoadWorkstationFilter = GridControlLoadWorkstation;

                GridControlLoadWorkstation.GroupBy("CalenderWeek");
                GridControlLoadWorkstation.GroupBy("CustomerWithPlant");
                GridControlLoadWorkstation.GroupBy("Project");
                GridControlLoadWorkstation.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadWorkstationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadEquipmentCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                AddColumnsToDataTableWithBandsinLoadWorkstation();
                FillLoadWorkstationData();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion Methods
    }
}
