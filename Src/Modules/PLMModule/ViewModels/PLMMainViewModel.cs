using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.PLM.Views;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Core;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.ServiceProcess;


namespace Emdep.Geos.Modules.PLM.ViewModels
{
    class PLMMainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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
        #endregion

        #region Constructor

        /// [001][GEOS2-3336][cpatil][2021-10-06][SrN 7 - Synchronization between PLM and ECOS [PLM#41]]
        public PLMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PLMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                TileBarItemsHelper tileBarItemsHelperBasePriceList = new TileBarItemsHelper();
                tileBarItemsHelperBasePriceList.Caption = "BASE PRICES";
                tileBarItemsHelperBasePriceList.BackColor = "#00879C";
                tileBarItemsHelperBasePriceList.GlyphUri = "BasePrice.png";
                tileBarItemsHelperBasePriceList.Visibility = Visibility.Visible;
                tileBarItemsHelperBasePriceList.NavigateCommand = new DelegateCommand(OpenBasePriceListView);

                TileCollection.Add(tileBarItemsHelperBasePriceList);


               


                //CUSTOMER PRICE LIST
                TileBarItemsHelper tileBarItemsHelperCustomerPriceList = new TileBarItemsHelper();
                tileBarItemsHelperCustomerPriceList.Caption = "CUSTOMER PRICES";
                tileBarItemsHelperCustomerPriceList.BackColor = "#5C5C5C";
                tileBarItemsHelperCustomerPriceList.GlyphUri = "Customer.png";
                tileBarItemsHelperCustomerPriceList.Visibility = Visibility.Visible;
                tileBarItemsHelperCustomerPriceList.NavigateCommand = new DelegateCommand(OpenCustomerPriceListView);

                TileCollection.Add(tileBarItemsHelperCustomerPriceList);

                //COMPETITOR PRICE LIST 
                TileBarItemsHelper tileBarItemsHelperCompetitorPriceList = new TileBarItemsHelper();
                tileBarItemsHelperCompetitorPriceList.Caption = "COMPETITOR PRICES";
                tileBarItemsHelperCompetitorPriceList.BackColor = "#D17800";
                tileBarItemsHelperCompetitorPriceList.GlyphUri = "wCompititor.png";
                tileBarItemsHelperCompetitorPriceList.Visibility = Visibility.Visible;

                TileCollection.Add(tileBarItemsHelperCompetitorPriceList);

                //[001]

                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("PCMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();


                TileBarItemsHelper tileBarItemConfigurationSystemSettings = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("PCMSystemSetting").ToString(),
                    BackColor = "#00BFFF",
                    GlyphUri = "bSystemSettings.png",
                    Visibility = Visibility.Visible,
                  NavigateCommand = new DelegateCommand<object>(SystemSettingsViewAction)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationSystemSettings);


                TileCollection.Add(tileBarItemsHelperConfiguration);

                //[001]
                //System Settings Visibility
                if (!GeosApplication.Instance.IsPLMPermissionNameECOS_Synchronization)
                {
                    tileBarItemsHelperConfiguration.Visibility = Visibility.Collapsed;
                }
                GeosApplication.Instance.Logger.Log("Constructor Constructor PLMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor PLMMainViewModel() Method - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in  Constructor PLMMainViewModel() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PLMMainViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SystemSettingsViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;

                PLMSystemSettingsViewModel systemSettingsViewModel = new PLMSystemSettingsViewModel();
                PLMSystemSettingsView systemSettingsView = new PLMSystemSettingsView();
                EventHandler handle = delegate { systemSettingsView.Close(); };
                systemSettingsViewModel.RequestClose += handle;
                systemSettingsView.DataContext = systemSettingsViewModel;
                IsBusy = false;
                systemSettingsView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenBasePriceListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenBasePriceListView()...", category: Category.Info, priority: Priority.Low);

                BasePriceListGridViewModel basePriceListGridViewModel = new BasePriceListGridViewModel();
                basePriceListGridViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PLM.Views.BasePriceListGridView", basePriceListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method OpenBasePriceListView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenBasePriceListView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void OpenCustomerPriceListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenCustomerPriceListView()...", category: Category.Info, priority: Priority.Low);

               CustomerPriceListGridViewModel customerPriceListGridViewModel = new CustomerPriceListGridViewModel();
                customerPriceListGridViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PLM.Views.CustomerPriceListGridView", customerPriceListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method OpenCustomerPriceListView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenCustomerPriceListView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods



        #endregion

    }
}
