using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Modules.APM.ViewModels;
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
using System.Windows.Media;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Modules.APM.CommonClasses;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class APMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declarations
        bool isBusy;
        private ObservableCollection<TileBarItemsHelper> tileCollection;
        ActionPlansViewModel objActionPlansViewModel;
       
        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection
        {
            get { return tileCollection; }
            set
            {
                tileCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TileCollection"));
            }
        }

        public ActionPlansViewModel ObjActionPlansViewModel
        {
            get { return objActionPlansViewModel; }
            set
            {
                objActionPlansViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjActionPlansViewModel"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        private VisibilityPerBUViewModel objVisibilityPerBUViewModel;
        public VisibilityPerBUViewModel ObjVisibilityPerBUViewModel
        {
            get { return objVisibilityPerBUViewModel; }
            set
            {
                objVisibilityPerBUViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjVisibilityPerBUViewModel"));
            }
        }


        #endregion

        #region Constructor
        public APMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("APMMainViewModel: Constructor start", category: Category.Info, priority: Priority.Low);

                #region GEOS2-5977 Sudhir.Jangra 03/09/2024
                // APMCommon.Instance.SelectedPeriod = DateTime.Now.Year;
                if (GeosApplication.Instance.FinancialYearLst == null)
                {
                    GeosApplication.Instance.FillFinancialYear();
                }
                if (APMCommon.Instance.SelectedPeriod == null)
                {
                    APMCommon.Instance.SelectedPeriod = new List<object>();
                }
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    if (GeosApplication.Instance.FinancialYearLst != null)
                    {
                        // Get the first matching year
                        var selectedYear = GeosApplication.Instance.FinancialYearLst
                                           .FirstOrDefault(x => x == DateTime.Now.Year);

                        // If a matching year is found, assign it to SelectedPeriod
                        if (selectedYear != 0) // If no match is found, FirstOrDefault returns 0 for long
                        {
                            APMCommon.Instance.SelectedPeriod.Clear(); // Clear existing items in the list
                            APMCommon.Instance.SelectedPeriod.Add(selectedYear); // Add the selected year as object
                        }
                    }
                }

               // APMCommon.Instance.ActiveEmployee = APMService.GetEmployeeCurrentDetail_V2560(GeosApplication.Instance.ActiveUser.IdUser, APMCommon.Instance.SelectedPeriod);



                //[Sudhir.Jangra][GEOS2-5976]
                string idPeriods = string.Empty;
                if (APMCommon.Instance.SelectedPeriod != null)
                {
                    List<long> selectedPeriod = APMCommon.Instance.SelectedPeriod.Cast<long>().ToList();
                    idPeriods = string.Join(",", selectedPeriod);
                }

                // Call the service asynchronously to avoid blocking the UI thread during ViewModel construction.
                Task.Run(() =>
                {
                    try
                    {
                        var emp = APMService.GetEmployeeCurrentDetail_V2570(GeosApplication.Instance.ActiveUser.IdUser, idPeriods);
                        if (emp != null)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                APMCommon.Instance.ActiveEmployee = emp;
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance?.Logger?.Log($"Async APM service call failed: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                    }
                });


                APMCommon.Instance.IdUserPermission = SelectIdUserPermission();
                #endregion


                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("APMViewModelDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                tileBarItemsHelperDashboard.Children = new ObservableCollection<TileBarItemsHelper>();

                //Dashboard 1 - Annual Sales.
                TileBarItemsHelper tileBarItemDashboard1 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("APMMainViewModelDashboard1").ToString(),
                    BackColor = "#00879C",
                    GlyphUri = "b_graph_dashboard1.png",
                    Visibility = Visibility.Visible,
                    //  NavigateCommand = new DelegateCommand(()b => { Service.Navigate(OpenAnnualSalesPerformanceView(), null, this); })
                };
                tileBarItemsHelperDashboard.Children.Add(tileBarItemDashboard1);

                //Dashboard 2 - Sales.
                TileBarItemsHelper tileBarItemDashboard2 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("APMMainViewModelDashboard2").ToString(),
                    BackColor = "#00879C",
                    GlyphUri = "bDashboard2bar-chart.png",
                    Visibility = Visibility.Visible,
                    //  NavigateCommand = new DelegateCommand(() => { Service.Navigate(OpenDashboardSaleView(), null, this); })
                };
                tileBarItemsHelperDashboard.Children.Add(tileBarItemDashboard2);

                TileCollection.Add(tileBarItemsHelperDashboard);

                TileBarItemsHelper tileBarItemsHelperPacking = new TileBarItemsHelper();
                tileBarItemsHelperPacking.Caption = System.Windows.Application.Current.FindResource("APMViewModelActionPlan").ToString();
                tileBarItemsHelperPacking.BackColor = "#880015";
                tileBarItemsHelperPacking.GlyphUri = "APM.png";
                tileBarItemsHelperPacking.Visibility = Visibility.Visible;
                tileBarItemsHelperPacking.Children = new ObservableCollection<TileBarItemsHelper>();
                
                // Add sub-tiles for old and new views
                TileBarItemsHelper tileBarItemModernView = new TileBarItemsHelper()
                {
                    Caption = "Action Plans (Modern UI)",
                    BackColor = "#880015",
                    GlyphUri = "APM.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigateActionPlansModernView())
                };
                tileBarItemsHelperPacking.Children.Add(tileBarItemModernView);

                TileBarItemsHelper tileBarItemClassicView = new TileBarItemsHelper()
                {
                    Caption = "Action Plans (Classic UI)",
                    BackColor = "#880015",
                    GlyphUri = "APM.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigateActionPlansView())
                };
                tileBarItemsHelperPacking.Children.Add(tileBarItemClassicView);
                
                TileCollection.Add(tileBarItemsHelperPacking);

                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("APMMainViewModelConfigurations").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#8B99E8";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                //[Shweta.Thube][GEOS2-6696]
                //Configuration 1 - Visibility per BU
                TileBarItemsHelper tileBarItemConfiguration1 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("APMMainViewModelConfiguration1").ToString(),
                    BackColor = "#8B99E8",
                    GlyphUri = "Configuration1.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateVisibilityPerBUView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration1);
                //[Shweta.Thube][GEOS2-8061]
                TileBarItemsHelper tileBarItemConfiguration2 = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("APMMainViewModelNotificationSettings").ToString(),
                    BackColor = "#8B99E8",
                    GlyphUri = "mailNotification.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(NavigateNotificationSettingsView)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration2);
                TileCollection.Add(tileBarItemsHelperConfiguration);

                GeosApplication.Instance.Logger.Log("APMMainViewModel: Constructor executed successfully", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log($"APMMainViewModel: TileCollection size {TileCollection?.Count}", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("APMMainViewModel: Constructor end", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor APMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor APMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor APMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Navigate to MODERN optimized Action Plans view
        /// Features: Async paging, virtualization, lazy-load master/detail
        /// </summary>
        private async void NavigateActionPlansModernView()
        {
            ActionPlansModernViewModel modernViewModel = null;
            try
            {
                GeosApplication.Instance.Logger.Log("APMMainViewModel: NavigateActionPlansModernView - START", category: Category.Info, priority: Priority.Low);

                // Validações críticas ANTES de criar o ViewModel
                if (GeosApplication.Instance == null)
                {
                    throw new InvalidOperationException("GeosApplication.Instance is null");
                }

                if (GeosApplication.Instance.ApplicationSettings == null)
                {
                    throw new InvalidOperationException("ApplicationSettings is null");
                }

                if (!GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                {
                    throw new InvalidOperationException("ServicePath not found in ApplicationSettings");
                }

                GeosApplication.Instance.Logger.Log("Creating ActionPlansModernView...", category: Category.Info, priority: Priority.Low);
                var modernView = new ActionPlansModernView();

                GeosApplication.Instance.Logger.Log("Creating ActionPlansModernViewModel...", category: Category.Info, priority: Priority.Low);
                modernViewModel = new ActionPlansModernViewModel();
                
                GeosApplication.Instance.Logger.Log("Setting DataContext...", category: Category.Info, priority: Priority.Low);
                modernView.DataContext = modernViewModel;
                
                GeosApplication.Instance.Logger.Log("Navigating to view...", category: Category.Info, priority: Priority.Low);
                Service.Navigate("Emdep.Geos.Modules.APM.Views.ActionPlansModernView", modernViewModel, null, this, true);
                
                GeosApplication.Instance.Logger.Log("Calling InitAsync()...", category: Category.Info, priority: Priority.Low);
                // Initialize async (non-blocking)
                await modernViewModel.InitAsync();

                GeosApplication.Instance.Logger.Log("APMMainViewModel: NavigateActionPlansModernView - SUCCESS", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error in NavigateActionPlansModernView: {ex.Message}\nStackTrace: {ex.StackTrace}";
                GeosApplication.Instance?.Logger?.Log(errorMsg, category: Category.Exception, priority: Priority.High);
                
                // Mostrar detalhes do erro para debug
                CustomMessageBox.Show($"Erro ao carregar Action Plans (Modern):\n\n{ex.Message}\n\nVer logs para detalhes.", 
                    "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Navigate to CLASSIC Action Plans view (legacy)
        /// </summary>
        private void NavigateActionPlansView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("APMMainViewModel: NavigateActionPlansView - START", category: Category.Info, priority: Priority.Low);

                ActionPlansView actionPlansView = new ActionPlansView();
                ActionPlansViewModel actionPlansViewModel = new ActionPlansViewModel();
                actionPlansViewModel.Init();
                actionPlansView.DataContext = actionPlansViewModel;
                Service.Navigate("Emdep.Geos.Modules.APM.Views.ActionPlansView", actionPlansViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("APMMainViewModel: NavigateActionPlansView - SUCCESS", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateActionView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.Thube] [GEOS2-6696]
        private void NavigateVisibilityPerBUView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateVisibilityPerBUView ...", category: Category.Info, priority: Priority.Low);
               
                VisibilityPerBUViewModel visibilityPerBUViewModel = new VisibilityPerBUViewModel();
                visibilityPerBUViewModel.Init();
                ObjVisibilityPerBUViewModel = visibilityPerBUViewModel;
                APMCommon.Instance.GetVisibilityPerBUViewModelDetails = ObjVisibilityPerBUViewModel;
                Service.Navigate("Emdep.Geos.Modules.APM.Views.VisibilityPerBUView", visibilityPerBUViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateVisibilityPerBUView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateVisibilityPerBUView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }       
        public int SelectIdUserPermission()
        {
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 39))
            {
                return 39;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 40))
            {
                return 40;
            }
            else if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 41))
            {
                return 41;
            }

            return 0;
        }
        //[Shweta.Thube][GEOS2-8061]
        private void NavigateNotificationSettingsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateNotificationSettingsView ...", category: Category.Info, priority: Priority.Low);

                NotificationSettingsViewModel notificationSettingsViewModel = new NotificationSettingsViewModel();
                NotificationSettingsView notificationSettingsView = new NotificationSettingsView();
                notificationSettingsViewModel.Init();
                EventHandler handle = delegate { notificationSettingsView.Close(); };
                notificationSettingsViewModel.RequestClose += handle;
                notificationSettingsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("APMMainViewModelNotificationSettings").ToString();
                notificationSettingsView.DataContext = notificationSettingsViewModel;
                notificationSettingsView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method NavigateNotificationSettingsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateNotificationSettingsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
