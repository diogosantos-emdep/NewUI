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
    public class RTMShowCRMExpectedLoadsViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Declaration
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private List<OfferOption> offerOptions;
        public DataRowView selectedObject;
        private List<RTMFutureLoad> rTMFutureLoadList;
        private string futureLoad;
        private DataTable dataTableForGridLayout;
        private List<PlantOperationWeek> plantWeekList = new List<PlantOperationWeek>();
        private GridControl FutureGridControl;

        #endregion Declaration

        #region Public Commands
        public ICommand CloseButtonCommand { get; set; }
        public ICommand GridActionLoadCommand { get; set; }

        #endregion Public Commands
        #region Property

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
        public List<OfferOption> OfferOptions
        {
            get { return offerOptions; }
            set { offerOptions = value; }
        }
        public DataRowView SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public List<RTMFutureLoad> RTMFutureLoadList
        {
            get { return rTMFutureLoadList; }
            set { rTMFutureLoadList = value; }
        }

        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummary { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummary { get; private set; }

        public List<PlantOperationWeek> PlantWeekList
        {
            get { return plantWeekList; }
            set
            {
                plantWeekList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantWeekList"));
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
        public RTMShowCRMExpectedLoadsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor RTMShowCRMExpectedLoadsViewModel() ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                GridActionLoadCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCommandAction);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor RTMShowCRMExpectedLoadsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor RTMShowCRMExpectedLoadsViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                AddColumnsToDataTableWithBands();
                FutureLoad = System.Windows.Application.Current.FindResource("FutureLoad").ToString();

                FillDashboard();

                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void AddColumnsToDataTableWithBands()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands ...", category: Category.Info, priority: Priority.Low);
                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="OfferDeliveryDateYear",HeaderText="Year", Settings = SettingsType.OfferDeliveryDateYear, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Site",HeaderText="Plant", Settings = SettingsType.SiteName, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="CalendarWeek",HeaderText="CalendarWeek", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Code",HeaderText="Code", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Group",HeaderText="Group", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerSite",HeaderText="Site", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="BusinessUnit",HeaderText="BusinessUnit", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Offer Close Year", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="SalesOwner",HeaderText="SalesOwner", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                    //new Emdep.Geos.UI.Helper.Column() { FieldName="OfferDeliveryDateYear",HeaderText="", Settings = SettingsType.OfferDeliveryDateYear, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Site",HeaderText="", Settings = SettingsType.SiteName, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="CalendarWeek",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Code",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Group",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerSite",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="BusinessUnit",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Offer Close Year", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    //new Emdep.Geos.UI.Helper.Column() { FieldName="SalesOwner",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },


                     new Emdep.Geos.UI.Helper.Column() { FieldName="OfferDeliveryDateYear",HeaderText="", Settings = SettingsType.OfferDeliveryDateYear, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Site",HeaderText="", Settings = SettingsType.SiteName, AllowCellMerge=false, Width=60, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CalendarWeek",HeaderText="", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Plant",HeaderText="Offer Close Year", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Code",HeaderText="Offer Number", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="Group",HeaderText="Customer", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerSite",HeaderText="Plant", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="BusinessUnit",HeaderText="Business Unit", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="SalesOwner",HeaderText="Sales Owner", Settings = SettingsType.CalendarWeek, AllowCellMerge=false, Width=120, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },

                };

                GroupSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummary = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayout = new DataTable();
                DataTableForGridLayout.Columns.Add("BusinessUnit", typeof(string));
                DataTableForGridLayout.Columns.Add("CustomerSite", typeof(string));
                DataTableForGridLayout.Columns.Add("Group", typeof(string));
                DataTableForGridLayout.Columns.Add("Code", typeof(string));
                DataTableForGridLayout.Columns.Add("CalendarWeek", typeof(string));
                DataTableForGridLayout.Columns.Add("Site", typeof(string));
                DataTableForGridLayout.Columns.Add("OfferDeliveryDateYear", typeof(string));
                DataTableForGridLayout.Columns.Add("SalesOwner", typeof(string));
                DataTableForGridLayout.Columns.Add("GroupColor", typeof(bool));
                //  OfferOptions = CrmStartUp.GetAllOfferOptions();
                for (int i = 0; i < OfferOptions.Count; i++)
                {
                    if (!DataTableForGridLayout.Columns.Contains(OfferOptions[i].Name))
                    {

                        Columns.Add(new Emdep.Geos.UI.Helper.Column() { FieldName = OfferOptions[i].Name.ToString(), HeaderText = OfferOptions[i].Name.ToString(), Settings = SettingsType.ArrayOfferOption, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = false, IsVertical = true, FixedWidth = true });
                       
                        DataTableForGridLayout.Columns.Add(OfferOptions[i].Name.ToString(), typeof(string));

                        GroupSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name, DisplayFormat = "{0}" });
                        TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = OfferOptions[i].Name, DisplayFormat = "{0}" });


                    }
                }

               
                TotalSummary.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CalendarWeek", DisplayFormat = "Total" });
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBands executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBands() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDashboard()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDashboard ...", category: Category.Info, priority: Priority.Low);
                List<RTMFutureLoad> RTMFutureLoadWeekwiseList = new List<RTMFutureLoad>();

                DataTableForGridLayout.Rows.Clear();

                DataTable DataTableForGridLayoutCopy = new DataTable();

                DataTableForGridLayoutCopy = DataTableForGridLayout.Copy();

                PlantWeekList = PlantWeekList.OrderBy(a => a.CalenderWeek).ToList();
                List<string> YearOfDate;
                string[] Seperator = { "CW" };
                DateTime currentDate = DateTime.Now;
                int currentWeek = (int)(currentDate.DayOfYear / 7) + 1;
                int year = DateTime.Now.Year;
                string Week = year + "CW" + currentWeek;

                foreach (PlantOperationWeek weekitem in PlantWeekList)
                {

                    RTMFutureLoadWeekwiseList = RTMFutureLoadList.Where(i => i.DeliveryWeek == weekitem.CalenderWeek).ToList();

                    foreach (RTMFutureLoad RTMFutureLoadWeekwiseItem in RTMFutureLoadWeekwiseList)
                    {
                        DataRow dr = DataTableForGridLayoutCopy.NewRow();
                        if (RTMFutureLoadWeekwiseList == null || RTMFutureLoadWeekwiseList.Count == 0)
                            dr["CalendarWeek"] = "";
                        else
                        {
                            dr["CalendarWeek"] = weekitem.CalenderWeek;
                            YearOfDate = weekitem.CalenderWeek.Split(Seperator, StringSplitOptions.None).ToList();
                            dr["OfferDeliveryDateYear"] = YearOfDate[0].ToString();
                            dr["GroupColor"] = false;
                            if (weekitem.CalenderWeek == Week)
                            {
                                // string curWeek = rowsInCurrentWeek.Select(a => a.ToString()).FirstOrDefault();
                                //GroupColor = Week;
                                dr["GroupColor"] = true;
                            }
                        }
                        dr["Site"] = RTMFutureLoadWeekwiseItem.ActiveSite.SiteAlias;


                        #region RND
                        dr["Code"] = RTMFutureLoadWeekwiseItem.Code;
                        dr["Group"] = RTMFutureLoadWeekwiseItem.Group;
                        // dr["Region"] = RTMFutureLoadWeekwiseItem.Region;
                        dr["CustomerSite"] = RTMFutureLoadWeekwiseItem.CustomerSite;
                        dr["BusinessUnit"] = RTMFutureLoadWeekwiseItem.BusinessUnit;
                        if (RTMFutureLoadWeekwiseItem.SalesOwnerList != null)
                        {

                            string item = RTMFutureLoadWeekwiseItem.SalesOwnerList.FirstOrDefault(i => i.IdSite == RTMFutureLoadWeekwiseItem.IdSite).SalesOwner;
                            dr["SalesOwner"] = item;
                        }

                        #endregion
                        foreach (OptionsByOfferGrid item in RTMFutureLoadWeekwiseItem.OptionsByOffers)
                        {
                            if (item.OfferOption != null)
                            {
                                if (item.IdOption.ToString() == "6" ||
                                     item.IdOption.ToString() == "19" ||
                                     item.IdOption.ToString() == "21" ||
                                     item.IdOption.ToString() == "23" ||
                                     item.IdOption.ToString() == "25" ||
                                     item.IdOption.ToString() == "27")
                                {
                                    var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                    int indexc = Columns.IndexOf(column);
                                    Columns[indexc].Visible = false;
                                }
                                else if (DataTableForGridLayoutCopy.Columns.Contains(item.OfferOption.ToString()))
                                {
                                    if (DataTableForGridLayoutCopy.Columns[item.OfferOption.ToString()].ToString() == item.OfferOption.ToString())
                                    {
                                        var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == item.OfferOption.ToString().Trim().ToUpper());
                                        int indexc = Columns.IndexOf(column);
                                        Columns[indexc].Visible = true;

                                        if (string.IsNullOrEmpty(Convert.ToString(dr[item.OfferOption]))) dr[item.OfferOption] = 0;

                                        dr[item.OfferOption] = Convert.ToInt32(dr[item.OfferOption]) + item.Quantity;
                                    }
                                }
                            }
                        }
                        DataTableForGridLayoutCopy.Rows.Add(dr);
                        //if (!string.IsNullOrEmpty(Convert.ToString(dr["CalendarWeek"])))
                        //{

                        //    if (SelectedDeliveryCW == null) SelectedDeliveryCW = new List<object>();

                        //    if (SelectedDeliveryCW.Count > 0)
                        //    {
                        //        if (selectedDeliveryCW.Contains(Convert.ToString(dr["CalendarWeek"])))
                        //        {
                        //            DataTableForGridLayoutCopy.Rows.Add(dr);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        DataTableForGridLayoutCopy.Rows.Add(dr);
                        //    }

                        //}
                    }


                }

                DataTableForGridLayout = DataTableForGridLayoutCopy;
                SelectedObject = DataTableForGridLayout.DefaultView[(DataTableForGridLayout.Rows.Count - 1)];
                GeosApplication.Instance.Logger.Log("Method FillDashboard executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillDashboard() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void GridActionLoadCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()...", category: Category.Info, priority: Priority.Low);

                FutureGridControl = (GridControl)obj;

                FutureGridControl.GroupBy("OfferDeliveryDateYear");
                FutureGridControl.GroupBy("Site");
                FutureGridControl.GroupBy("CalendarWeek");
                //FutureGridControl.GroupBy("Code");
                //FutureGridControl.GroupBy("Group");
                //FutureGridControl.GroupBy("CustomerSite");
                //FutureGridControl.GroupBy("BusinessUnit");
                //FutureGridControl.GroupBy("SalesOwner");

                GeosApplication.Instance.Logger.Log("Method GridActionLoadCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion Methods
    }
}
