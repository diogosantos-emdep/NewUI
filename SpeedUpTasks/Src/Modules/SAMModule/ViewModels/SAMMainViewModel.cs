using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.SAM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class SAMMainViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// [001][skhade][2019-09-25][GEOS2-1757] Create new module SAM (STRUCTURE ASSEMBLY MANAGER).
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ICrmService SAMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //ISAMService HrmService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration
        PendingWorkOrdersViewModel objPendingWorkOrdersViewModel;

        #endregion // Declaration

        #region Properties

        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

        public PendingWorkOrdersViewModel ObjPendingWorkOrdersViewModel
        {
            get { return objPendingWorkOrdersViewModel; }
            set
            {
                objPendingWorkOrdersViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjPendingWorkOrdersViewModel"));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// [001][skhade][2019-09-25][GEOS2-1757] Create new module SAM (STRUCTURE ASSEMBLY MANAGER).
        /// </summary>
        public SAMMainViewModel()
        {
            try
            {
                if (!GeosApplication.Instance.ObjectPool.ContainsKey("SAMMainViewModel") || GeosApplication.Instance.ObjectPool.Count == 0)
                    GeosApplication.Instance.ObjectPool.Add("SAMMainViewModel", this);
                GeosApplication.Instance.Logger.Log("Constructor SAMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                FillUserAuthorizedPlants();

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("WarehouseViewModelDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                tileBarItemsHelperDashboard.Children = new ObservableCollection<TileBarItemsHelper>();

                TileCollection.Add(tileBarItemsHelperDashboard);

                TileBarItemsHelper tileBarItemsHelperOrders = new TileBarItemsHelper();
                tileBarItemsHelperOrders.Caption = System.Windows.Application.Current.FindResource("SAMOrders").ToString();
                tileBarItemsHelperOrders.BackColor = "#FF427940";
                tileBarItemsHelperOrders.GlyphUri = "Orders.png";
                tileBarItemsHelperOrders.Visibility = Visibility.Visible;
                tileBarItemsHelperOrders.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tbiPendingWorkOrders = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMPendingWorkOrders").ToString(),
                    BackColor = "#FF427940",
                    GlyphUri = "WorkOrders.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(PendingWorkOrdersViewAction(), null, this); })
                };

                tileBarItemsHelperOrders.Children.Add(tbiPendingWorkOrders);

                TileCollection.Add(tileBarItemsHelperOrders);


                TileBarItemsHelper tileBarItemsHelperReports = new TileBarItemsHelper();
                tileBarItemsHelperReports.Caption = System.Windows.Application.Current.FindResource("SAMREPORTS").ToString();
                tileBarItemsHelperReports.BackColor = "#a8389f";
                tileBarItemsHelperReports.GlyphUri = "Reports.png";
                tileBarItemsHelperReports.Visibility = Visibility.Visible;
                tileBarItemsHelperReports.Children = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tbiWorklogReport = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("SAMWorklogReport").ToString(),
                    BackColor = "#a8389f",
                    GlyphUri = "bModulesReport.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate(WorklogReportViewAction(), null, this); })
                };

                tileBarItemsHelperReports.Children.Add(tbiWorklogReport);
                TileCollection.Add(tileBarItemsHelperReports);

                GeosApplication.Instance.Logger.Log("Constructor Constructor SAMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor SAMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor SAMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor SAMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

       
        private void FillUserAuthorizedPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUserAuthorizedPlants() started executing...", category: Category.Info, priority: Priority.Low);

                SAMCommon.Instance.PlantOwnerList = SAMService.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser).OrderBy(o => o.Country.IdCountry).ToList();
                SAMCommon.Instance.SelectedPlantOwnerList = new List<object>();

                // EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(GeosApplication.Instance.ActiveUser...Site.ConnectPlantId));
                //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();

                Company companyDefault = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(x => x.ShortName == GeosApplication.Instance.SiteName);
                if (companyDefault != null)
                {
                    SAMCommon.Instance.SelectedPlantOwnerList.Add(companyDefault);
                }
                else
                {
                    SAMCommon.Instance.SelectedPlantOwnerList.AddRange(SAMCommon.Instance.SelectedPlantOwnerList);
                }

                GeosApplication.Instance.Logger.Log("Method FillUserAuthorizedPlants() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method FillUserAuthorizedPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Method FillUserAuthorizedPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillUserAuthorizedPlants SAMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Methods

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>
        private void NavigateDashboardView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()...", category: Category.Info, priority: Priority.Low);
                //if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
                //{
                //    SavechangesOrder();
                //}
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.DashboardView", new DashboardViewModel(), null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private PendingWorkOrdersView PendingWorkOrdersViewAction()
        {
            if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
            {
                SavechangesOrder();
            }
            PendingWorkOrdersView workOrderView = new PendingWorkOrdersView();
            PendingWorkOrdersViewModel pendingWorkOrdersViewModel = new PendingWorkOrdersViewModel();
            workOrderView.DataContext = pendingWorkOrdersViewModel;
            return workOrderView;
        }

        //private WorklogReportView WorklogReportViewAction()
        //{
        //    if (MultipleCellEditHelperSAMWorkOrder.IsValueChanged)
        //    {
        //        SavechangesOrder();
        //    }
        //    WorklogReportView worklogReportView = new WorklogReportView();
        //    WorklogReportViewModel worklogReportViewModel = new WorklogReportViewModel();
        //    worklogReportView.DataContext = worklogReportViewModel;
        //    return worklogReportView;
        //}


        ///----------[Sprint-83] [GEOS2-2372]  [19-06-2020] [sjadhav]---------
        public void SavechangesOrder()
        {
            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["OteditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ObjPendingWorkOrdersViewModel == null)
                {
                    ObjPendingWorkOrdersViewModel = new PendingWorkOrdersViewModel();
                    ObjPendingWorkOrdersViewModel.MainOtsList = GeosApplication.Instance.MainOtsList;
                }
                if (MultipleCellEditHelperSAMWorkOrder.Checkview == "PendingWorkOrderTableView")
                {
                    ObjPendingWorkOrdersViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkOrder.Viewtableview);
                }                
            }
            MultipleCellEditHelperSAMWorkOrder.IsValueChanged = false;
        }

        private WorklogReportView WorklogReportViewAction()
        {
            WorklogReportView worklogReportView = new WorklogReportView();
            WorklogReportViewModel worklogReportViewModel = new WorklogReportViewModel();
            worklogReportView.DataContext = worklogReportViewModel;
            return worklogReportView;
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
