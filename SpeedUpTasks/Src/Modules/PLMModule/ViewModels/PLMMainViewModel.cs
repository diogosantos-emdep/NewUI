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

        #endregion

        #region Constructor

       
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
                tileBarItemsHelperBasePriceList.Visibility = Visibility.Visible;

                TileCollection.Add(tileBarItemsHelperCustomerPriceList);

                //COMPETITOR PRICE LIST 
                TileBarItemsHelper tileBarItemsHelperCompetitorPriceList = new TileBarItemsHelper();
                tileBarItemsHelperCompetitorPriceList.Caption = "COMPETITOR PRICES";
                tileBarItemsHelperCompetitorPriceList.BackColor = "#D17800";
                tileBarItemsHelperCompetitorPriceList.GlyphUri = "wCompititor.png";
                tileBarItemsHelperCompetitorPriceList.Visibility = Visibility.Visible;

                TileCollection.Add(tileBarItemsHelperCompetitorPriceList);

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

        private void OpenBasePriceListView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()...", category: Category.Info, priority: Priority.Low);

                BasePriceListGridViewModel basePriceListGridViewModel = new BasePriceListGridViewModel();
                basePriceListGridViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.PLM.Views.BasePriceListGridView", basePriceListGridViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDetectionsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods



        #endregion

    }
}
