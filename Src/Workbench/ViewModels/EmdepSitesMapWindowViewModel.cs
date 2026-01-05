using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.Commands;
using Workbench.Views;
using Emdep.Geos.Modules;
using Emdep.Geos.UI;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using Prism.Logging;
using System.ServiceModel;

namespace Workbench.ViewModels
{
    class EmdepSitesMapWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        Bitmap bitmapImage;
        public List<Company> CompanyList;   //list of site 
        private int activeItemIndexValue;   // Active index in site image 
        private ObservableCollection<Company> companyObservableCollection;  // Collection of  Site

        private string companyAlias;    // Emdep Site Alias

        #endregion

        #region  public Properties

        public ObservableCollection<Company> CompanyObservableCollection
        {
            get { return companyObservableCollection; }
            set { companyObservableCollection = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyObservableCollection")); }
        }

        public Bitmap BitmapImage
        {
            get { return bitmapImage; }
            set { bitmapImage = value; }
        }

        public int ActiveItemIndexValue
        {
            get { return activeItemIndexValue; }
            set
            {
                if (this.activeItemIndexValue != value)
                {
                    this.activeItemIndexValue = value;
                    ; OnPropertyChanged(new PropertyChangedEventArgs("ActiveItemIndexValue"));
                }
            }
        }
        public string CompanyAlias
        {
            get { return companyAlias; }
            set { companyAlias = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyAlias")); }
        }

        #endregion

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

        #endregion

        #region  public ICommand

        public ICommand EmdepSitesMapWindowCommand { get; set; }        //Emdep Sites Map Window
        public ICommand EmdepSitesImageActionCommand { get; set; }      //Emdep Sites  Image  
        public ICommand WorkbenchWindowCloseButtonCommand { get; set; } //Emdep Sites  Image close 

        #endregion

        #region Constructor

        public EmdepSitesMapWindowViewModel()
        {
            EmdepSitesMapWindowCommand = new RelayCommand(new Action<object>(EmdepSitesMapWindow));
            EmdepSitesImageActionCommand = new RelayCommand(new Action<object>(EmdepSitesImageSelection));
            WorkbenchWindowCloseButtonCommand = new RelayCommand(new Action<object>(EmdepSitesWindowClose));
        }

        #endregion

        #region public Methods

        /// <summary>
        /// Method for open Emdep site on map
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesMapWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click On emdep site show on map ", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log("Start EmdepSitesMapWindow Method", category: Category.Info, priority: Priority.Low);

            CompanyAlias = CompanyObservableCollection[ActiveItemIndexValue].Alias;

            GeosApplication.Instance.Logger.Log("Initialising EmdepSitesMapWindow", category: Category.Info, priority: Priority.Low);
            EmdepSitesWiseMapWindowViewModel emdepSitesWiseMapWindowViewModel = new EmdepSitesWiseMapWindowViewModel(CompanyAlias);

            if (emdepSitesWiseMapWindowViewModel.IsInit == true)
            {
                EmdepSitesMapWindow emdepSitesMapWindow = new EmdepSitesMapWindow();
                EventHandler handle = delegate { emdepSitesMapWindow.Close(); };
                emdepSitesWiseMapWindowViewModel.RequestClose += handle;
                emdepSitesMapWindow.DataContext = emdepSitesWiseMapWindowViewModel;
                GeosApplication.Instance.Logger.Log("Initialising EmdepSitesMapWindow Successfully", category: Category.Info, priority: Priority.Low);
                emdepSitesMapWindow.ShowDialogWindow();
            }
        }

        /// <summary>
        /// method for set emdep site selection
        /// [001][GEOS2-2074][cpatil][18-02-2020]CRM - OPPORTUNITIES - Timeline
        /// [001][GEOS2-1977][cpatil][18-02-2020]The code added in the offer code must be taken from the application selecte
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesImageSelection(object obj)
        {
            if (CompanyObservableCollection[ActiveItemIndexValue].IsPermission == true)
            {
                string tempSiteName = GeosApplication.Instance.SiteName;

                try
                {
                    GeosProvider geosProvider = WorkbenchStartUp.GetServiceProviderDetailByCompanyId(CompanyObservableCollection[ActiveItemIndexValue].IdCompany);
                    GeosApplication.Instance.Logger.Log("Set emdep site as per user", category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.SiteName = CompanyObservableCollection[ActiveItemIndexValue].Alias;
                    /// [001] Added
                    GeosApplication.Instance.ActiveIdSite = CompanyObservableCollection[ActiveItemIndexValue].IdCompany;
                    CompanyAlias = GeosApplication.Instance.SiteName;
                    //GeosApplication.Instance.ActiveUser.IdCompany
                    List<GeosServiceProvider> GeosServiceProviderList = new List<GeosServiceProvider>();

                    GeosServiceProviderList = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider;
                    GeosServiceProviderList = GeosServiceProviderList.Select(serviceProvider => { if (serviceProvider.Name.Contains(tempSiteName)) serviceProvider.IsSelected = false; if (serviceProvider.Name.Contains(CompanyAlias)) serviceProvider.IsSelected = true; return serviceProvider; }).ToList();
                    GeosApplication.Instance.WriteApplicationSettingFile(GeosServiceProviderList);


                    //GeosApplication.Instance.IsPrivateNetworkIp(); // get ip is private or public 

                    ////bool IsSelected = GeosServiceProviderList.Any(serviceProvider => serviceProvider.IsSelected == true);// check is provider is selected 
                    //if (GeosApplication.Instance.IsPrivateNetworkIP == true) //private ip 
                    //{
                    //    GeosApplication.Instance.Ip = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetworkIP => serviceProviderPrivateNetworkIP.PrivateNetwork.IP).FirstOrDefault();
                    //    GeosApplication.Instance.Port = Convert.ToDecimal(GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetworkIP => serviceProviderPrivateNetworkIP.PrivateNetwork.Port).FirstOrDefault());
                    //}
                    //else
                    //{
                    //    GeosApplication.Instance.Ip = GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPublicNetworkIP => serviceProviderPublicNetworkIP.PublicNetwork.IP).FirstOrDefault();
                    //    GeosApplication.Instance.Port = Convert.ToDecimal(GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPublicNetworkIP => serviceProviderPublicNetworkIP.PublicNetwork.Port).FirstOrDefault());
                    //}

                    GeosApplication.Instance.ServicePath =GeosServiceProviderList.Where(serviceProvider => serviceProvider.IsSelected == true).Select(serviceProviderPrivateNetworkIP => serviceProviderPrivateNetworkIP.ServiceProviderUrl).FirstOrDefault();

                    //if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                    //{
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"] = GeosApplication.Instance.Ip;
                    //}
                    //else
                    //{
                    //    GeosApplication.Instance.ApplicationSettings.Add("ServicePath", GeosApplication.Instance.Ip);
                    //}

                    //if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServiceProviderPort"))
                    //{
                    //    GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"] = GeosApplication.Instance.Port.ToString();
                    //}
                    //else
                    //{
                    //    GeosApplication.Instance.ApplicationSettings.Add("ServiceProviderPort", GeosApplication.Instance.Port.ToString());
                    //}


                    if (GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
                    {
                        GeosApplication.Instance.ApplicationSettings["ServicePath"] = GeosApplication.Instance.ServicePath;
                    }
                    else
                    {
                        GeosApplication.Instance.ApplicationSettings.Add("ServicePath", GeosApplication.Instance.ServicePath);
                    }
                    GeosApplication.Instance.Logger.Log("Set emdep site as per user successfully", category: Category.Info, priority: Priority.Low);
                    RequestClose(null, null);
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesImageSelection() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesImageSelection() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesImageSelection() Method - Exception " + ex.Message, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("UnknownException").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            else
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EmdepSitesMapWindowNotPermission").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// method for close window
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesWindowClose(object obj)
        {
            RequestClose(null, null);
            CompanyAlias = GeosApplication.Instance.SiteName;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        #endregion

    }
}
