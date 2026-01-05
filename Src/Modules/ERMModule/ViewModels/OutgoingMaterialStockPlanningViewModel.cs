using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
   
    public class OutgoingMaterialStockPlanningViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //[rani dhamankar][17-02-2025][GEOS2-6887]
        #region Services
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region Declaration
       
        private List<ERMOutgoingArticleStockPlanning> eRMOutgoingArticleStockPlanningList;
        private Int32 idWarehouse;
        private Int32 idArticle;
        DateTime fromDate;
        DateTime toDate;
        private string windowHeader;
        private double dialogHeight;
        private double dialogWidth;
        #endregion
        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        #endregion

        #region event
        public event EventHandler RequestClose;

        #endregion


        #region Property

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
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
       
        #endregion

        #region Constructor
        public OutgoingMaterialStockPlanningViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OutgoingMaterialStockPlanningViewModel()...", category: Category.Info, priority: Priority.Low);
                WindowHeader = System.Windows.Application.Current.FindResource("TitleOFOutgoingStockDetails").ToString();
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                CloseButtonCommand = new RelayCommand(new Action<object>(CloseButtonCommandAction));
            
                GeosApplication.Instance.Logger.Log("Constructor OutgoingMaterialStockPlanningViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor OutgoingMaterialStockPlanningViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method

        private void CancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }
        private void CloseButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }
        public void Init(int idArticle, int IdWarehouse, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
          
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                FillOutgoingMaterialStock(idArticle,IdWarehouse, FromDate, ToDate);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        public List<ERMOutgoingArticleStockPlanning> ERMOutgoingArticleStockPlanningList
        {
            get
            {
                return eRMOutgoingArticleStockPlanningList;
            }

            set
            {
                eRMOutgoingArticleStockPlanningList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ERMOutgoingArticleStockPlanningList"));
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
        
        #region [rani dhamankar][17-02-2025][GEOS2-6887]
        private void FillOutgoingMaterialStock(int IdArticle ,int IdWarehouse, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOutgoingMaterialStock() executed successfully", category: Category.Info, priority: Priority.Low);
                string SelectedPlantName = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                //Data.Common.Company usrDefault = GeosApplication.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == SelectedPlantName);
                Data.Common.Company usrDefault = ERMCommon.Instance.PlantOwnerUsersList.FirstOrDefault(x => x.ShortName == SelectedPlantName);// [GEOS2-8698][rani dhamankar][16-07-2025]
                IdWarehouse = Convert.ToInt32(ERMCommon.Instance.Selectedwarehouse.IdWarehouse);
                //  string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == ERMCommon.Instance.Selectedwarehouse.Name).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                #region [GEOS2-9398][gulab lakade][20 11 2025]
                //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == SelectedPlantName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                string serviceurl = string.Empty;
                if (!string.IsNullOrEmpty(ERMCommon.Instance.Selectedwarehouse.SiteName))
                {
                    serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == ERMCommon.Instance.Selectedwarehouse.SiteName).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                }
                else
                {
                    serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                }
                ERMService = new ERMServiceController(serviceurl);
                //ERMService = new ERMServiceController("localhost:6699");
                //ERMOutgoingArticleStockPlanningList = new List<ERMOutgoingArticleStockPlanning>(ERMService.GetOutgoingArticleStockPlanningList_V2610(IdArticle, IdWarehouse, FromDate, ToDate));
                //ERMOutgoingArticleStockPlanningList = new List<ERMOutgoingArticleStockPlanning>(ERMService.GetOutgoingArticleStockPlanningList_V2620(IdArticle, IdWarehouse, FromDate, ToDate));
                ERMOutgoingArticleStockPlanningList = new List<ERMOutgoingArticleStockPlanning>(ERMService.GetOutgoingArticleStockPlanningList_V2690(IdArticle, IdWarehouse, FromDate, ToDate));
                #endregion

                GeosApplication.Instance.Logger.Log("Method FillOutgoingMaterialStock() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOutgoingMaterialStock()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        
        #endregion

    }
}
