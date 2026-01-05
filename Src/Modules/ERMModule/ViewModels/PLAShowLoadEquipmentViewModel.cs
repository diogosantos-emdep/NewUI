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
    public class PLAShowLoadEquipmentViewModel : ViewModelBase, INotifyPropertyChanged
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

        #endregion Command

        #region Declaration
        private ObservableCollection<BandItem> bandsDashboardLoadEquipment = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayoutLoadEquipment; 
        private GridControl gridControlLoadEquipment;
        private GridControl gridControlLoadEquipmentFilter;
        private List<PlantLoadAnalysis> plantLoadAnalysisList;
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisEquipment;
        private List<PlantLoadAnalysis> groupByTempPlantLoadAnalysisEquipmentCopy;
        #endregion Declaration

        #region Property
        public DataTable DataTableForGridLayoutLoadEquipment
        {
            get
            {
                return dataTableForGridLayoutLoadEquipment;
            }
            set
            {
                dataTableForGridLayoutLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutLoadEquipment"));
            }
        }

        public ObservableCollection<BandItem> BandsDashboardLoadEquipment
        {
            get { return bandsDashboardLoadEquipment; }
            set
            {
                bandsDashboardLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsDashboardLoadEquipment"));
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

        

        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisEquipment
        {
            get { return groupByTempPlantLoadAnalysisEquipment; }
            set
            {
                groupByTempPlantLoadAnalysisEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisEquipment"));
            }
        }
        public List<PlantLoadAnalysis> GroupByTempPlantLoadAnalysisEquipmentCopy
        {
            get { return groupByTempPlantLoadAnalysisEquipmentCopy; }
            set
            {
                groupByTempPlantLoadAnalysisEquipmentCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupByTempPlantLoadAnalysisEquipmentCopy"));
            }
        }

        public GridControl GridControlLoadEquipment
        {
            get
            {
                return gridControlLoadEquipment;
            }
            set
            {
                gridControlLoadEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadEquipment"));
            }
        }

        public GridControl GridControlLoadEquipmentFilter
        {
            get
            {
                return gridControlLoadEquipmentFilter;
            }
            set
            {
                gridControlLoadEquipmentFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GridControlLoadEquipmentFilter"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> TotalSummaryLoadEquipment { get; private set; }
        public ObservableCollection<Emdep.Geos.UI.Helper.Summary> GroupSummaryLoadEquipment { get; private set; }

        #endregion Property

        #region Constructor
        public PLAShowLoadEquipmentViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));

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
                AddColumnsToDataTableWithBandsinLoadEquipments();
                BandsDashboardLoadEquipment = new ObservableCollection<BandItem>(BandsDashboardLoadEquipment);
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddColumnsToDataTableWithBandsinLoadEquipments()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments ...", category: Category.Info, priority: Priority.Low);
                // Here need to created only summary as band details are directly send from main view model

                GroupSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();
                TotalSummaryLoadEquipment = new ObservableCollection<Emdep.Geos.UI.Helper.Summary>();

                if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    List<Site> plantOwners = ERMCommon.Instance.SelectedAuthorizedPlantsList.Cast<Site>().ToList();
                    foreach (var SelectedPlant in plantOwners)
                    {
                        string PlantName = string.Empty;

                        foreach (var item in GroupByTempPlantLoadAnalysisEquipment)
                        {
                            GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                            TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Plant_" + item.OriginalIdSite + "_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        }

                        GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });
                        TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "Total_" + SelectedPlant.IdSite, DisplayFormat = "{0}" });

                    }


                }

                GroupSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Sum, FieldName = "GrandTotal", DisplayFormat = "{0}" });
                TotalSummaryLoadEquipment.Add(new Emdep.Geos.UI.Helper.Summary() { Type = SummaryItemType.Count, FieldName = "otCodeEquipment", DisplayFormat = "Total" });

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTableWithBandsinLoadEquipments executed Successfully", category: Category.Info, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTableWithBandsinLoadEquipments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        #endregion Methods
    }
}
