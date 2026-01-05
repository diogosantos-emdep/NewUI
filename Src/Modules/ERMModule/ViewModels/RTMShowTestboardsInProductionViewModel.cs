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
    public class RTMShowTestboardsInProductionViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion


        #region Declaration

        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsProductionsInTime;
        private DataTable dataTableForGridLayoutProductionInTime;
        private ObservableCollection<BandItem> bandsDashboard1 = new ObservableCollection<BandItem>();
        private List<ERMProductionTime> productionTimeList;
        private GridControl ProductionGridControl;
        private GridControl productionGridControlFilter;
        private string testBoardInProduction;
        private List<GeosAppSetting> geosAppSettingList;
        private List<string> categoryColumns;
        private string groupColor;
        #endregion Declaration

        #region  public Commands
        public ICommand GridActionProductionLoadCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        #endregion


        #region Property
        public GridControl ProductionGridControlFilter
        {
            get
            {
                return productionGridControlFilter;
            }
            set
            {
                productionGridControlFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionGridControlFilter"));
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

        public List<ERMProductionTime> ProductionTimeList
        {
            get
            {
                return productionTimeList;
            }
            set
            {
                productionTimeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductionTimeList"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsProductionsInTime
        {
            get { return columnsProductionsInTime; }
            set
            {
                columnsProductionsInTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsProductionsInTime"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryTestboardsInProduction { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryTestboardsInProduction { get; private set; }

        public string TestBoardInProduction
        {
            get
            {
                return testBoardInProduction;
            }
            set
            {
                testBoardInProduction = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TestBoardInProduction"));
            }
        }

        public List<string> CategoryColumns
        {
            get { return categoryColumns; }
            set
            {
                categoryColumns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryColumns"));
            }
        }

        public List<GeosAppSetting> GeosAppSettingList
        {
            get { return geosAppSettingList; }
            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public string GroupColor
        {
            get
            {
                return groupColor;
            }
            set
            {
                groupColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupColor"));
            }
        }
        #endregion Property

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
        public RTMShowTestboardsInProductionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RTMShowTestboardsInProductionViewModel() ...", category: Category.Info, priority: Priority.Low);
                GridActionProductionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionProductionLoadCommandAction);
                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));

                GeosApplication.Instance.Logger.Log("Constructor RTMShowTestboardsInProductionViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor RTMShowTestboardsInProductionViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion Constructor

        #region Methods
        private void GridActionProductionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionProductionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                ProductionGridControl = (GridControl)obj;

                ProductionGridControlFilter = ProductionGridControl;

                //ProductionGridControl.GroupBy("DeliveryWeek");
                //ProductionGridControl.GroupBy("OTCode");

                GeosApplication.Instance.Logger.Log("Method GridActionProductionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionProductionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }


        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);
                
                AddColumnsToDataTableWithBandsinTestboardProduction();
                TestBoardInProduction = System.Windows.Application.Current.FindResource("TestboardsinProduction").ToString();

                FillDashboardProductionIntime();
                FillListOfColor();
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTableWithBandsinTestboardProduction()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinTestboardProduction ...", category: Category.Info, priority: Priority.Low);

                CategoryColumns = new List<string>();

                ColumnsProductionsInTime = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="RowLabels",HeaderText="Row Labels", Settings = SettingsType.RowLabels, AllowCellMerge=false, Width=150, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="DeliveryWeek",HeaderText="", Settings = SettingsType.DeliveryWeek, AllowCellMerge =false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Modules",HeaderText="#Modules", Settings = SettingsType.Modules, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="ProducedModules",HeaderText="Produced Modules", Settings = SettingsType.ProducedModules, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="InProduction",HeaderText="In Production", Settings = SettingsType.InProduction, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="OTCode",HeaderText="OTCode", Settings = SettingsType.OTCode, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= false, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="DeliveryDate",HeaderText="DeliveryDate", Settings = SettingsType.DeliveryDate, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= false, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };


                GroupSummaryTestboardsInProduction = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryTestboardsInProduction = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutProductionInTime = new DataTable();
                DataTableForGridLayoutProductionInTime.Columns.Add("RowLabels", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryWeek", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("OTCode", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("Modules", typeof(Int32));
                DataTableForGridLayoutProductionInTime.Columns.Add("ProducedModules", typeof(Int32));
                DataTableForGridLayoutProductionInTime.Columns.Add("InProduction", typeof(Int32));
                //DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryDate", typeof(DateTime));
                DataTableForGridLayoutProductionInTime.Columns.Add("DeliveryDateHtmlColor", typeof(string));
                DataTableForGridLayoutProductionInTime.Columns.Add("GroupColor", typeof(bool));
                for (int i = 0; i < ProductionTimeList.Count; i++)
                {
                    try
                    {
                        if (ProductionTimeList[i].ProductCategoryGrid != null)
                        {
                            if (ProductionTimeList[i].ProductCategoryGrid.Category != null)
                            {
                                if (!DataTableForGridLayoutProductionInTime.Columns.Contains(ProductionTimeList[i].ProductCategoryGrid.Category.Name))
                                {

                                    ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), HeaderText = ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), Settings = SettingsType.ArrayOfferOptionProduction, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true });
                                    DataTableForGridLayoutProductionInTime.Columns.Add(ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString(), typeof(string));
                                    CategoryColumns.Add(ProductionTimeList[i].ProductCategoryGrid.Category.Name.ToString());

                                    GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name, DisplayFormat = "{0}" });
                                    TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Category.Name, DisplayFormat = "{0}" });
                                }
                            }
                            else
                            {
                                if (!DataTableForGridLayoutProductionInTime.Columns.Contains(ProductionTimeList[i].ProductCategoryGrid.Name))
                                {

                                    ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), HeaderText = ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), Settings = SettingsType.ArrayOfferOptionProduction, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true });
                                    DataTableForGridLayoutProductionInTime.Columns.Add(ProductionTimeList[i].ProductCategoryGrid.Name.ToString(), typeof(string));
                                    CategoryColumns.Add(ProductionTimeList[i].ProductCategoryGrid.Name.ToString());

                                    GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Name, DisplayFormat = "{0}" });
                                    TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = ProductionTimeList[i].ProductCategoryGrid.Name, DisplayFormat = "{0}" });
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinTestboardProduction()- 1 Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    // }
                }



                ColumnsProductionsInTime.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = "GrandTotal", HeaderText = "GrandTotal", Settings = SettingsType.GrandTotal, AllowCellMerge = false, Width = 120, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = true, IsReadOnly = true });
                DataTableForGridLayoutProductionInTime.Columns.Add("GrandTotal", typeof(Int32));

                GroupSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "DeliveryWeek", DisplayFormat = "GrandTotal" });
                TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                //TotalSummaryTestboardsInProduction.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "RowLabels", DisplayFormat = "Grand Total" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinTestboardProduction executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinTestboardProduction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDashboardProductionIntime()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardProductionIntime ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutProductionInTime.Clear();

                DataTable DataTableForGridLayoutProductionInTimeCopy = DataTableForGridLayoutProductionInTime.Copy();
                string CategoryName = string.Empty;
                var distinctByIdoffer = ProductionTimeList
                                .GroupBy(pt => pt.IdOffer)
                                .Select(g => g.First())
                                .ToList();                         // [pallavi jadhav] [GEOS2-5831][03-07-2024]
        DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;
                if (ProductionTimeList.Count > 0)
                {
                    Int32 GrandTotalProductionInTime = 0;
                    Int64 InProductionCount = 0;
                    Int64 ModulesCount = 0;
                    Int64 ProducedModules = 0;
                    
                    foreach (ERMProductionTime item in ProductionTimeList)
                    {
                        try
                        {
                            DataRow dr = DataTableForGridLayoutProductionInTimeCopy.NewRow();
                            // dr["RowLabels"] = item.MergeCode;
                            dr["RowLabels"] = item.Code; // [pallavi jadhav] [GEOS2-5831][03-07-2024]
                            dr["Deliveryweek"] = item.DeliveryWeek;
                            dr["OTCode"] = item.OtCode;
                            dr["Modules"] = item.Modules;
                            dr["ProducedModules"] = item.ProducedModules;
                            dr["GroupColor"] = false;
                            if (item.DeliveryWeek == Week)
                            {
                                // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                                //GroupColor = Week;
                                dr["GroupColor"] = true;
                            }


                            if (String.IsNullOrEmpty(Convert.ToString(item.Modules))) ModulesCount = 0;
                            else
                                ModulesCount = item.Modules;

                            if (String.IsNullOrEmpty(Convert.ToString(item.ProducedModules))) ProducedModules = 0;
                            else
                                ProducedModules = item.ProducedModules;

                            InProductionCount = ModulesCount - ProducedModules;

                            dr["InProduction"] = InProductionCount;

                            //dr["DeliveryDate"] = Convert.ToDateTime(item.DeliveryDate);
                            dr["DeliveryDateHtmlColor"] = item.DeliveryDateHtmlColor;

                            //   item.ProductionList.FirstOrDefault().IdProductCategory
                            int CategoryQuantity = 0;
                            int CategoryCount = 0;
                            List<ERMProductionTime> objMergeCode = new List<ERMProductionTime>();

                            CategoryCount = ProductionTimeList.Where(i => i.Code == item.Code).ToList().Count(); // [pallavi jadhav] [GEOS2-5831][03-07-2024]

                            if (item.ProductCategoryGrid != null)
                            {
                                if (item.ProductCategoryGrid.Category != null)
                                {
                                    CategoryName = item.ProductCategoryGrid.Category.Name;

                                }
                                else
                                {
                                    CategoryName = item.ProductCategoryGrid.Name;
                                }

                                //if (item.ProductCategoryGrid.Category != null)
                                //{
                                //CategoryName = item.ProductCategoryGrid.Category.Name;
                                if (!string.IsNullOrEmpty(CategoryName))
                                {
                                    if (dr[CategoryName] != null)
                                    {
                                        CategoryQuantity = String.IsNullOrEmpty(Convert.ToString(dr[CategoryName])) ? 0 : Convert.ToInt32(dr[CategoryName]);
                                    }

                                    dr[CategoryName] = CategoryQuantity + CategoryCount;
                                }
                                //}
                            }

                            GrandTotalProductionInTime = 0;
                            foreach (string str in CategoryColumns)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(dr[str])))
                                {
                                    GrandTotalProductionInTime = GrandTotalProductionInTime + Convert.ToInt32(dr[str]);
                                }
                            }

                            dr["GrandTotal"] = GrandTotalProductionInTime;
                            DataTableForGridLayoutProductionInTimeCopy.Rows.Add(dr);

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                DataTableForGridLayoutProductionInTime = DataTableForGridLayoutProductionInTimeCopy;

                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillDashboardProductionIntime()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDashboardProductionIntime() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillListOfColor()
        {
            try
            {
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("14,15,16,17");
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillListOfColor() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillListOfColor() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillListOfColor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

    }
}
