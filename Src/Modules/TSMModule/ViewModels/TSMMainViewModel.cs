using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.TSM.Views;
using Emdep.Geos.Modules.TSM.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
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
using Prism.Logging;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using UsersView = Emdep.Geos.Modules.TSM.Views.UsersView;
using Emdep.Geos.Modules.Crm;
using DevExpress.XtraReports.UI;
using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Modules.TSM.CommonClass;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.TSM.ViewModels
{
    public class TSMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ITSMService TSMService = new TSMServiceController("localhost:6699");
        #endregion


        // [pallavi.kale][07-01-2025] [GEOS2-5385]

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

        #endregion

        #region Constructor
        public TSMMainViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TSMMainViewModel()...", category: Category.Info, priority: Priority.Low);
                 FillPeopleList();// [pallavi.kale][24-09-2025] [GEOS2-8953]
                 FillUserAuthorizedPlants();//[GEOS2-8963][pallavi.kale][28.11.2025]

                // [pallavi.kale][08-01-2025] [GEOS2-5386]
                TileCollection = new ObservableCollection<TileBarItemsHelper>();
                // [pallavi.kale][04-09-2025] [GEOS2-8939]
                //Dashboard
                TileBarItemsHelper tileBarItemsHelperDashboard = new TileBarItemsHelper();
                tileBarItemsHelperDashboard.Caption = System.Windows.Application.Current.FindResource("TSMViewModelDashboard").ToString();
                tileBarItemsHelperDashboard.BackColor = "#00879C";
                tileBarItemsHelperDashboard.GlyphUri = "Dashboard.png";
                tileBarItemsHelperDashboard.Visibility = Visibility.Visible;
                tileBarItemsHelperDashboard.Children = new ObservableCollection<TileBarItemsHelper>();
                TileCollection.Add(tileBarItemsHelperDashboard);
                // [pallavi.kale][04-09-2025] [GEOS2-8941]
                //Clients
                TileBarItemsHelper tileBarItemsHelperClients = new TileBarItemsHelper();
                tileBarItemsHelperClients.Caption = System.Windows.Application.Current.FindResource("TSMViewModelClients").ToString();
                tileBarItemsHelperClients.BackColor = "#D17800";
                tileBarItemsHelperClients.GlyphUri = "Clients.png";
                tileBarItemsHelperClients.Visibility = Visibility.Visible;
                tileBarItemsHelperClients.Children = new ObservableCollection<TileBarItemsHelper>();
                // [pallavi.kale][04-09-2025] [GEOS2-8949]
                TileBarItemsHelper tileBarItemsHelperCustomer = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("TSMMainViewModelCustomer").ToString(),
                    BackColor = "#D17800",
                    GlyphUri = "Customer.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigateCustomersView())
                   
                };
                tileBarItemsHelperClients.Children.Add(tileBarItemsHelperCustomer);
                // [pallavi.kale][14-10-2025] [GEOS2-8955]
                TileBarItemsHelper tileBarItemsHelperContact = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("AccountViewContactCount").ToString(),
                    BackColor = "#D17800",
                    GlyphUri = "Contacts.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigateContactsView())

                };
                tileBarItemsHelperClients.Children.Add(tileBarItemsHelperContact);
                TileCollection.Add(tileBarItemsHelperClients);
                // [pallavi.kale][04-09-2025] [GEOS2-8943]
                //Order 
                TileBarItemsHelper tileBarItemsHelperOrder = new TileBarItemsHelper();
                tileBarItemsHelperOrder.Caption = System.Windows.Application.Current.FindResource("TSMViewModelOrder").ToString();
                tileBarItemsHelperOrder.BackColor = "#427940";
                tileBarItemsHelperOrder.GlyphUri = "Leads.png";
                tileBarItemsHelperOrder.Visibility = Visibility.Visible;
                tileBarItemsHelperOrder.NavigateCommand = new DelegateCommand(() => NavigatePendingOrderView());
                tileBarItemsHelperOrder.Children = new ObservableCollection<TileBarItemsHelper>();
                TileCollection.Add(tileBarItemsHelperOrder);
                // [pallavi.kale][04-09-2025] [GEOS2-8945]
                //Reports
                TileBarItemsHelper tileBarItemsHelperReports = new TileBarItemsHelper();
                tileBarItemsHelperReports.Caption = System.Windows.Application.Current.FindResource("TSMViewModelReports").ToString();
                tileBarItemsHelperReports.BackColor = "#A84FA9";
                tileBarItemsHelperReports.GlyphUri = "Reports.png";
                tileBarItemsHelperReports.Visibility = Visibility.Visible;
                tileBarItemsHelperReports.Children = new ObservableCollection<TileBarItemsHelper>();
                TileBarItemsHelper tileBarItemsHelperWorklogReport = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("WorklogReports").ToString(),
                    BackColor = "#a8389f",
                    GlyphUri = "bModulesReport.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigateWorklogReportView())

                };
                tileBarItemsHelperReports.Children.Add(tileBarItemsHelperWorklogReport);
                TileBarItemsHelper tileBarItemsHelperOTP= new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("OTP").ToString(),
                    BackColor = "#a8389f",
                    GlyphUri = "bModulesReport.png",
                    Visibility = Visibility.Visible,
                    //NavigateCommand = new DelegateCommand(() => NavigateWorklogReportView())

                };
                tileBarItemsHelperReports.Children.Add(tileBarItemsHelperOTP);
                TileCollection.Add(tileBarItemsHelperReports);

                //Configuration
                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("TSMMainViewModelConfigurations").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#c7bfe6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 140) )
                {

                    TileBarItemsHelper tileBarItemHelperUsers = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("TSMMainViewModelUsers").ToString(),
                        BackColor = "#c7bfe6",
                        GlyphUri = "users.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand(() => NavigateUsersView())

                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemHelperUsers);

                }

                TileBarItemsHelper tileBarItemHelperMyPreferences = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("TSMMainViewModelMyPreferences").ToString(),
                    BackColor = "#c7bfe6",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Visible,

                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemHelperMyPreferences);
                TileCollection.Add(tileBarItemsHelperConfiguration);

                GeosApplication.Instance.Logger.Log("Constructor Constructor TSMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor TSMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor TSMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor TSMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        } 
        private void NavigateUsersView()
        {
            // [GEOS2-5388][pallavi.kale] [13.01.2025]
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateUsersView ...", category: Category.Info, priority: Priority.Low);

                UsersView usersView = new UsersView();
                UsersViewModel usersViewModel = new UsersViewModel();

                usersView.DataContext = usersViewModel;
                Service.Navigate("Emdep.Geos.Modules.TSM.Views.UsersView", usersViewModel, null, this, true);

                usersViewModel.Init();

                GeosApplication.Instance.Logger.Log("Method NavigateUsersView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateUsersView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        // [pallavi.kale][04-09-2025] [GEOS2-8949]
        private void NavigateCustomersView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateCustomersView ...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.IsTSMCustomerViewVisible = false;
                GeosApplication.Instance.CmbPlantOwnerUsers= Visibility.Collapsed;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Collapsed;
                
                AccountView accountView = new AccountView();
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                AccountViewModel accountViewModel = new AccountViewModel();
                accountView.DataContext = accountViewModel;
                accountViewModel.ModuleName = "TSMModule";
                Service.Navigate("Emdep.Geos.Modules.Crm.Views.AccountView", accountViewModel, null, this, true);
                GeosApplication.Instance.Logger.Log("Method NavigateCustomersView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateCustomersView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
           
        }
        // [pallavi.kale][24-09-2025] [GEOS2-8953]
        public void FillPeopleList()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPeopleList ...", category: Category.Info, priority: Priority.Low);

                    if (GeosApplication.Instance.PeopleList == null)
                    {
                         GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                    }
                GeosApplication.Instance.Logger.Log("Method FillPeopleList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPeopleList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPeopleList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPeopleList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        // [pallavi.kale][14-10-2025] [GEOS2-8955]
        private void NavigateContactsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateContactsView ...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.IsTSMCustomerViewVisible = false;
                GeosApplication.Instance.CmbPlantOwnerUsers = Visibility.Collapsed;
                GeosApplication.Instance.CmbSalesOwnerUsers = Visibility.Collapsed;
               
                ContactView contactView = new ContactView();
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                ContactViewModel contactViewModel = new ContactViewModel();
                contactView.DataContext = contactViewModel;
                contactViewModel.ModuleName = "TSMModule";
                Service.Navigate("Emdep.Geos.Modules.Crm.Views.ContactView", contactViewModel, null, this, true);
               
                GeosApplication.Instance.Logger.Log("Method NavigateContactsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
               
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateContactsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        //[GEOS2-8963][pallavi.kale][28.11.2025]
        private void NavigatePendingOrderView()
        {
            // [GEOS2-8963][pallavi.kale] [03.11.2025]
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePendingOrderView ...", category: Category.Info, priority: Priority.Low);

                PendingOrdersView pendingOrdersView = new PendingOrdersView();
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                PendingOrdersViewModel pendingOrdersViewModel = new PendingOrdersViewModel();
                pendingOrdersView.DataContext = pendingOrdersViewModel;
                Service.Navigate("Emdep.Geos.Modules.TSM.Views.PendingOrdersView", pendingOrdersViewModel, null, this, true);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method NavigatePendingOrderView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePendingOrderView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-8963][pallavi.kale][28.11.2025]
        private void FillUserAuthorizedPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUserAuthorizedPlants() started executing...", category: Category.Info, priority: Priority.Low);
                //TSMService = new TSMServiceController("localhost:6699");
                TSMCommon.Instance.PlantOwnerList = TSMService.GetAllCompaniesDetails_V2690(GeosApplication.Instance.ActiveUser.IdUser).OrderBy(o => o.Country.IdCountry).ToList();
                TSMCommon.Instance.SelectedPlantOwnerList = new List<object>();

                Company companyDefault = TSMCommon.Instance.PlantOwnerList.FirstOrDefault(x => x.ShortName == GeosApplication.Instance.SiteName);
                if (companyDefault != null)
                {
                    TSMCommon.Instance.SelectedPlantOwnerList.Add(companyDefault);
                }
                else
                {
                    TSMCommon.Instance.SelectedPlantOwnerList.AddRange(TSMCommon.Instance.SelectedPlantOwnerList);
                }

                if (TSMCommon.Instance.PlantOwnerList != null)
                {
                    foreach (var item in TSMCommon.Instance.PlantOwnerList)
                    {
                        if (item.Country != null)
                        {
                            item.ServiceProviderUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        }
                    }
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
        
        //[GEOS2-8981][pallavi.kale][28.11.2025]
        private void NavigateWorklogReportView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateWorklogReportView ...", category: Category.Info, priority: Priority.Low);

                WorklogReportView worklogReportView = new WorklogReportView();
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                WorklogReportViewModel worklogReportViewModel = new WorklogReportViewModel();
                worklogReportView.DataContext = worklogReportViewModel;
                Service.Navigate("Emdep.Geos.Modules.TSM.Views.WorklogReportView", worklogReportViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateWorklogReportView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateWorklogReportView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion










    }
}
