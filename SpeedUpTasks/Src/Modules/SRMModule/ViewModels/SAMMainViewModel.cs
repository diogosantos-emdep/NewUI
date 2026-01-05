using DevExpress.Mvvm;
using Emdep.Geos.Modules.SRM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
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

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class SRMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
        /// </summary>

        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //ISRMService HrmService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Properties

        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// [001][skhade][2020-02-24][GEOS2-1799] New module SRM - 1.
        /// </summary>
        public SRMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SRMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                //TileCollection = new ObservableCollection<TileBarItemsHelper>();

                //TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                //tileBarItemsHelperDashboard.Caption = "Menu 1"; //System.Windows.Application.Current.FindResource("HRMDashboard").ToString();
                //tileBarItemsHelperDashboard.BackColor = "#00879C";
                //tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                //tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                //tileBarItemsHelperDashboard.NavigateCommand = new DelegateCommand(NavigateDashboardView);
                //TileCollection.Add(tileBarItemsHelperDashboard);

                //TileBarItemsHelper tileBarItemsHelperEmployees = new TileBarItemsHelper();
                //tileBarItemsHelperEmployees.Caption = "Menu 2"; //System.Windows.Application.Current.FindResource("HRMEmployees").ToString();
                //tileBarItemsHelperEmployees.BackColor = "#CC6D00";
                //tileBarItemsHelperEmployees.GlyphUri = "hrm_employee.png";
                //tileBarItemsHelperEmployees.Visibility = Visibility.Visible;
                //tileBarItemsHelperEmployees.NavigateCommand = new DelegateCommand(NavigateEmployeesView);

                //TileCollection.Add(tileBarItemsHelperEmployees);

                GeosApplication.Instance.Logger.Log("Constructor Constructor HrmMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor SRMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor SRMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor SRMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method for Navigate Dashboard
        /// </summary>
        private void NavigateDashboardView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()...", category: Category.Info, priority: Priority.Low);

                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.DashboardView", new DashboardViewModel(), null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateDashboardView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Navigate Employees View
        /// </summary>
        private void NavigateEmployeesView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()...", category: Category.Info, priority: Priority.Low);

                //EmployeeProfileViewModel employeeProfileViewModel = new EmployeeProfileViewModel();
                //employeeProfileViewModel.Init();
                //Service.Navigate("Emdep.Geos.Modules.Hrm.Views.EmployeeProfileView", employeeProfileViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateEmployeesView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NavigateEmployeesView()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
