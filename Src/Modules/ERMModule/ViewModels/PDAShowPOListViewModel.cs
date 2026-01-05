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
    public class PDAShowPOListViewModel : ViewModelBase, INotifyPropertyChanged
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
        private ObservableCollection<PlantDeliveryAnalysis> plantDeliveryAnalysisList;
        private List<PlantDeliveryAnalysis> plantDeliveryAnalysisTempList;
        private bool isViewSupervisorERM; //[Pallavi jadhav][GEOS2-5910][17-07-2024]
        #endregion Declaration

        #region Property
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
        #endregion Property

        #region Constructor

        public PDAShowPOListViewModel()
        {
            CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
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

        }

        #endregion Constructor

        #region Methods

        //public void Init()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);


        //        GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        #endregion
    }
}
