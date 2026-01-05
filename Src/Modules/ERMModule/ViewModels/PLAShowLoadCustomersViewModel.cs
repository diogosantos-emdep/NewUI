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
    public class PLAShowLoadCustomersViewModel : ViewModelBase, INotifyPropertyChanged
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
        public ICommand GridActionLoadCustomersCommand { get; set; }
        #endregion Command

        #region Declaration
        private ObservableCollection<BandItem> bandsDashboardLoadCustomers = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutLoadCustomers; 
        private GridControl gridControlLoadCustomers; 
        private GridControl gridControlLoadCustomersFilter;
        private List<string> categoryColumns;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsLoadCustomers;
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
       

        public DataTable DataTableForGridLayoutLoadCustomers
        {
            get
            {
                return dataTableForGridLayoutLoadCustomers;
            }
            set
            {
                dataTableForGridLayoutLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadCustomers"));
            }
        }

        public GridControl GridControlLoadCustomers
        {
            get
            {
                return gridControlLoadCustomers;
            }
            set
            {
                gridControlLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadCustomers"));
            }
        }

        public GridControl GridControlLoadCustomersFilter
        {
            get
            {
                return gridControlLoadCustomersFilter;
            }
            set
            {
                gridControlLoadCustomersFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("gridControlLoadCustomersFilter"));
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

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsLoadCustomers
        {
            get { return columnsLoadCustomers; }
            set
            {
                columnsLoadCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsLoadCustomers"));
            }
        }


        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadCustomers { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadCustomers { get; private set; }
        #endregion Property

        #region Constructor
        public PLAShowLoadCustomersViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            GridActionLoadCustomersCommand = new DevExpress.Mvvm.DelegateCommand<object>(GridActionLoadCustomersCommandAction);

        }
        #endregion Constructor

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void GridActionLoadCustomersCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridActionLoadCustomersCommandAction()...", category: Category.Info, priority: Priority.Low);

                GridControlLoadCustomers = (GridControl)obj;

                GridControlLoadCustomersFilter = GridControlLoadCustomers;

                GeosApplication.Instance.Logger.Log("Method GridActionLoadCustomersCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GridActionLoadCustomersCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                AddColumnsToDataTableWithBandsinLoadCustomers();
                FillLoadCustomerData();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLoadCustomerData()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadCustomerData ...", category: Category.Info, priority: Priority.Low);
                DataTableForGridLayoutLoadCustomers.Clear();
                DataTable DataTableForGridLayoutLoadCustomerCopy = DataTableForGridLayoutLoadCustomers.Copy();
                string CategoryName = string.Empty;

                if (PlantLoadAnalysisList.Count > 0)
                {

                    var groupedData = PlantLoadAnalysisList
                        .GroupBy(item => new { item.CustomerGroup, item.CustomerPlant })
                        .Select(group => new
                        {
                            CustomerGroup = group.Key.CustomerGroup,
                            CustomerPlant = group.Key.CustomerPlant,
                            TotalQTY = group.Sum(item => item.QTY)
                        });

                    foreach (var groupItem in groupedData)
                    {
                        try
                        {
                            DataRow dr = DataTableForGridLayoutLoadCustomerCopy.NewRow();

                            dr["CustomerName"] = $"{groupItem.CustomerGroup} - {groupItem.CustomerPlant}";
                            dr["QTY"] = groupItem.TotalQTY;

                            DataTableForGridLayoutLoadCustomerCopy.Rows.Add(dr);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                DataTableForGridLayoutLoadCustomers = DataTableForGridLayoutLoadCustomerCopy;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillLoadCustomerData()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLoadCustomerData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTableWithBandsinLoadCustomers()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadCustomers ...", category: Category.Info, priority: Priority.Low);

                CategoryColumns = new List<string>();

                ColumnsLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Column>()
                {
                    new Emdep.Geos.UI.Helper.Column() { FieldName="CustomerName",HeaderText="CUSTOMER NAME", Settings = SettingsType.CustomerName, AllowCellMerge=false, Width=450, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                    new Emdep.Geos.UI.Helper.Column() { FieldName="QTY",HeaderText="CP's QTY", Settings = SettingsType.QTY, AllowCellMerge =false, Width=92, AllowEditing = false, Visible= true, IsVertical = false, FixedWidth=true, IsReadOnly = true  },
                };

                GroupSummaryLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadCustomers = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                DataTableForGridLayoutLoadCustomers = new DataTable();
                DataTableForGridLayoutLoadCustomers.Columns.Add("CustomerName", typeof(string));
                DataTableForGridLayoutLoadCustomers.Columns.Add("QTY", typeof(Int32));

                TotalSummaryLoadCustomers.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "QTY", DisplayFormat = "{0}" });
                TotalSummaryLoadCustomers.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "CustomerName", DisplayFormat = "Total" });

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadCustomers executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadCustomers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion Methods
    }
}
