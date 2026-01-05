using System;
using DevExpress.Mvvm;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Prism.Logging;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.OTM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DevExpress.Mvvm.POCO;
using DevExpress.DataProcessing.InMemoryDataProcessor;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    //[pramod.misal][GEOS2-6459][27-09-2024]-PO Registration (PO requests list)
    public class OTMMainViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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
        private ObservableCollection<TileBarItemsHelper> tileCollection;
        string toDatePOreg;
        string fromDatePOreg;
        #endregion

        #region Properties

        ParentTabView poRequestsView;
        ParentTabViewModel parentTabViewModel;
        public ParentTabViewModel ParentTabViewModel
        {
            get { return parentTabViewModel; }
            set
            {
                parentTabViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentTabViewModel"));
            }
        }
        public ParentTabView ParentTabView
        {
            get { return poRequestsView; }
            set
            {
                poRequestsView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentTabView"));
            }
        }
        public ObservableCollection<TileBarItemsHelper> TileCollection
        {
            get { return tileCollection; }
            set
            {
                tileCollection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TileCollection"));
            }
        }
        static ObservableCollection<PO> posList;
        public static ObservableCollection<PO> PosList
        {
            get { return posList; }
            set
            {
                posList = value;
            }
        }
     
        #endregion

        #region Constructor
        /// <summary>
        /// [001][ashish.malkhede][07.11.2024][GEOS2-6460]
        /// </summary>
        public OTMMainViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor OTMMainViewModel()...", category: Category.Info, priority: Priority.Low);

                TileCollection = new ObservableCollection<TileBarItemsHelper>();

                FillPlantList();
                #region PurchaseOrder
                //[pramod.misal][GEOS2-6459][27-09-2024] https://helpdesk.emdep.com/browse/GEOS2-6459
                //PurchaseOrder
                TileBarItemsHelper tileBarItemsHelperPurchaseOrder = new TileBarItemsHelper();
                tileBarItemsHelperPurchaseOrder.Caption = System.Windows.Application.Current.FindResource("OTMViewModelPurchaseOrders").ToString();
                tileBarItemsHelperPurchaseOrder.BackColor = "#FFF78A09";
                tileBarItemsHelperPurchaseOrder.GlyphUri = "PurchaseOrder.png";
                tileBarItemsHelperPurchaseOrder.Visibility = Visibility.Visible;
                tileBarItemsHelperPurchaseOrder.Children = new ObservableCollection<TileBarItemsHelper>();

                // Child-PoRequests
                TileBarItemsHelper tileBarItemsHelperPoRequests = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("OTMViewModelPoRequests").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "PoRequests.png",
                    Visibility = Visibility.Visible,
                    //NavigateCommand = new DelegateCommand(() => { Service.Navigate(PoRequestsViewAction(), null, this); })
                    NavigateCommand = new DelegateCommand(() => NavigatePORequestsView())
                };

                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemsHelperPoRequests);


                // Child-Registered PO
                TileBarItemsHelper tileBarItemsHelperRegisteredPO = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("OTMViewModelRegisteredPO").ToString(),
                    BackColor = "#FFF78A09",
                    GlyphUri = "RegisteredPO.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand(() => NavigatePORegisteredView()) //[001]
                };



                tileBarItemsHelperPurchaseOrder.Children.Add(tileBarItemsHelperRegisteredPO);

                TileCollection.Add(tileBarItemsHelperPurchaseOrder);
              #endregion PurchaseOrder

                #region Configuration
                //[pramod.misal][GEOS2-6461][27-09-2024] https://helpdesk.emdep.com/browse/GEOS2-6461
                //Configuration
                TileBarItemsHelper tileBarItemsHelperConfiguration = new TileBarItemsHelper();
                tileBarItemsHelperConfiguration.Caption = System.Windows.Application.Current.FindResource("OTMConfiguration").ToString();
                tileBarItemsHelperConfiguration.BackColor = "#C7BFE6";
                tileBarItemsHelperConfiguration.GlyphUri = "Configuration.png";
                tileBarItemsHelperConfiguration.Visibility = Visibility.Visible;
                tileBarItemsHelperConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();

                //Child-MyPreferences
                TileBarItemsHelper tileBarItemConfiguration = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("OTMMyPreferences").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "MyPreference_Black.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(NavigateMyPreferences)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfiguration);

                //[pooja.jadhav][06-01-2025][GEOS2-6732]
                //if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(p => p.IdPermission == 138))
                //{
                    TileBarItemsHelper tileBarItemConfigurationOTRequestsTemplates = new TileBarItemsHelper()
                    {
                        Caption = System.Windows.Application.Current.FindResource("OTMOTRequestTemplates").ToString(),
                        BackColor = "#C7BFE6",
                        GlyphUri = "OTRequestTemplates_Black.png",
                        Visibility = Visibility.Visible,
                        NavigateCommand = new DelegateCommand<object>(NavigateOTRequestTemplates)
                    };
                    tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationOTRequestsTemplates);
                //}
               
                //[Rahul.Gadhave][[GEOS2-9281][Date:05 - 09 - 2025]
                TileBarItemsHelper tileBarItemConfigurationsystem = new TileBarItemsHelper()
                {
                    Caption = System.Windows.Application.Current.FindResource("OTMSystemSettings").ToString(),
                    BackColor = "#C7BFE6",
                    GlyphUri = "bSystemSettings.png",
                    Visibility = Visibility.Visible,
                    NavigateCommand = new DelegateCommand<object>(NavigateSystemSettings)
                };
                tileBarItemsHelperConfiguration.Children.Add(tileBarItemConfigurationsystem);
                TileCollection.Add(tileBarItemsHelperConfiguration);
                #endregion Configuration
              
                GeosApplication.Instance.Logger.Log("Constructor Constructor OTMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor OTMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor OTMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor OTMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
        /// </summary>
        //private void NavigatePORequestsView()
        //{
        //    try
        //    {   //[GEOS2-6861][rdixit][16.01.2025]
        //        GeosApplication.Instance.Logger.Log("Method NavigatePORequestsView ...", category: Category.Info, priority: Priority.Low);
        //        if (ParentTabView == null)
        //            ParentTabView = new ParentTabView();
        //        if (ParentTabViewModel == null)
        //            ParentTabViewModel = new ParentTabViewModel();
        //        if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == Application.Current.FindResource("OTMViewModelPoRequests").ToString()))
        //        {
        //            ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new PORequestsViewModel { TabName = Application.Current.FindResource("OTMViewModelPoRequests").ToString(), ParentViewModel = this, Position = 1 }));
        //            ParentTabViewModel.Sort();
        //            Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
        //        }
        //        GeosApplication.Instance.Logger.Log("Method NavigatePORequestsView()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePORequestsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void NavigatePORequestsView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePORequestsView ...", category: Category.Info, priority: Priority.Low);
                if (ParentTabView == null)
                    ParentTabView = new ParentTabView();
                if (ParentTabViewModel == null)
                    ParentTabViewModel = new ParentTabViewModel();

                string tabName = Application.Current.FindResource("OTMViewModelPoRequests").ToString();
                if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == tabName))
                {
                    int newPosition = ParentTabViewModel.Tabs.Count > 0
                        ? ParentTabViewModel.Tabs.Max(t => t.Position) + 1
                        : 0;

                    ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new PORequestsViewModel
                    {
                        TabName = tabName,
                        ParentViewModel = this,
                        Position = newPosition
                    }));
                    ParentTabViewModel.Sort();
                    Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
                }
                GeosApplication.Instance.Logger.Log("Method NavigatePORequestsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePORequestsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        ///[001][ashish.malkhede][07.11.2024][GEOS2-6460]
        /// </summary>
        //private void NavigatePORegisteredView()
        //{
        //    try
        //    {
        //        //[GEOS2-6861][rdixit][16.01.2025]
        //        GeosApplication.Instance.Logger.Log("Method NavigatePORegisteredView ...", category: Category.Info, priority: Priority.Low);
        //        if (ParentTabView == null)
        //            ParentTabView = new ParentTabView();
        //        if (ParentTabViewModel == null)
        //            ParentTabViewModel = new ParentTabViewModel();
        //        if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == Application.Current.FindResource("OTMViewModelRegisteredPO").ToString()))
        //        {
        //            ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new PORegisterViewModel { TabName = Application.Current.FindResource("OTMViewModelRegisteredPO").ToString(), ParentViewModel = this, Position = 2 }));
        //            ParentTabViewModel.Sort();
        //            Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
        //        }
        //        GeosApplication.Instance.Logger.Log("Method NavigatePORegisteredView()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePORegisteredView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void NavigatePORegisteredView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePORegisteredView ...", category: Category.Info, priority: Priority.Low);
                if (ParentTabView == null)
                    ParentTabView = new ParentTabView();
                if (ParentTabViewModel == null)
                    ParentTabViewModel = new ParentTabViewModel();

                string tabName = Application.Current.FindResource("OTMViewModelRegisteredPO").ToString();
                if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == tabName))
                {
                    int newPosition = ParentTabViewModel.Tabs.Count > 0
                        ? ParentTabViewModel.Tabs.Max(t => t.Position) + 1
                        : 0;

                    ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new PORegisterViewModel
                    {
                        TabName = tabName,
                        ParentViewModel = this,
                        Position = newPosition
                    }));
                    ParentTabViewModel.Sort();
                    Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
                }
                GeosApplication.Instance.Logger.Log("Method NavigatePORegisteredView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePORegisteredView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        ////[GEOS2-6861][rdixit][16.01.2025]
        void DateTimeSetting(string title)
        {
            try
            {
                if (OTMCommon.Instance.FromDatePOreg == null && OTMCommon.Instance.ToDatePOreg == null)
                {
                    const string shortDateFormat = "dd/MM/yyyy";
                    DateTime today = DateTime.Now;
                    DateTime StartFromDate = new DateTime(today.Year, today.Month, 1);
                    DateTime EndToDate = StartFromDate.AddMonths(1).AddDays(-1);
                    OTMCommon.Instance.FromDatePOreg = StartFromDate.ToString(shortDateFormat);
                    OTMCommon.Instance.ToDatePOreg = EndToDate.ToString(shortDateFormat);
                    if (OTMCommon.Instance.FromDate == null && OTMCommon.Instance.ToDate == null)
                    {
                        OTMCommon.Instance.FromDate = StartFromDate.ToString(shortDateFormat);
                        OTMCommon.Instance.ToDate = EndToDate.ToString(shortDateFormat);
                    }
                }
                //[GEOS2-6861][rdixit][16.01.2025]
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                
                if (OTMCommon.Instance.SelectedSinglePlant == null)                
                    OTMCommon.Instance.SelectedSinglePlant = selectedPlant;
                
                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)                
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = selectedPlant;
                if (PosList == null)
                    PosList = new ObservableCollection<PO>();

                OTMCommon.Instance.PORequestsTitle = string.Format(Application.Current.FindResource("PORequestsTitle").ToString());
                OTMCommon.Instance.RegisteredPOTitle = string.Format(Application.Current.FindResource("RegisteredPOTitle").ToString());
                if (title == "PORequests")
                {
                    if (!PosList.Any(i => i.Header == OTMCommon.Instance.PORequestsTitle))
                    {
                        PosList.Insert(0,new PO()
                        {
                            StartDate = DateTime.Today.Date,
                            EndDate = DateTime.Today.Date,
                            ISVisiblePORegistered = Visibility.Collapsed,
                            ISVisiblePOReq = Visibility.Visible,
                            Header = OTMCommon.Instance.PORequestsTitle,
                            PORequestDetailsList = new ObservableCollection<PORequestDetails>()
                        });
                    }
                }
                else
                {
                    if (!PosList.Any(i => i.Header == OTMCommon.Instance.RegisteredPOTitle))
                    {
                        PosList.Add(new PO()
                        {
                            StartDate = DateTime.Today.Date,
                            EndDate = DateTime.Today.Date,
                            ISVisiblePORegistered = Visibility.Visible,
                            ISVisiblePOReq = Visibility.Collapsed,
                            Header = OTMCommon.Instance.RegisteredPOTitle,
                            PORegisteredDetailsList = new ObservableCollection<PORegisteredDetails>()
                        });
                    }
                }                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePORegisteredView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-6459][27-09-2024]
        private void FillPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlantList...", category: Category.Info, priority: Priority.Low);

                //OTMService = new OTMServiceController("localhost:6699");
                //OTMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Company>(OTMService.GetAllSitesWithImagesByIdUser(GeosApplication.Instance.ActiveUser.IdUser));\
                //[pramod.misal][GEOS2-6460][28-11-2024]
                OTMCommon.Instance.UserAuthorizedPlantsList = new ObservableCollection<Company>(OTMService.GetAllSitesWithImagesByIdUser_V2590(GeosApplication.Instance.ActiveUser.IdUser));
                //if (OTMCommon.Instance.UserAuthorizedPlantsList != null)
                //{
                //    foreach (var bpItem in OTMCommon.Instance.UserAuthorizedPlantsList.GroupBy(tpa => tpa.Iso))
                //    {
                //        ImageSource CompanyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().ImageInBytes);
                //        bpItem.ToList().Where(oti => oti.Iso == bpItem.Key).ToList().ForEach(oti => oti.SiteImage = CompanyFlagImage);
                //    }
                //} 

                OTMCommon.Instance.SelectedAuthorizedPlantsList = new List<object>();

                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);

                if (selectedPlant != null)
                {
                    OTMCommon.Instance.SelectedAuthorizedPlantsList.Add(selectedPlant);
                }
                else
                {
                    OTMCommon.Instance.SelectedAuthorizedPlantsList.AddRange(OTMCommon.Instance.UserAuthorizedPlantsList);
                }
                GeosApplication.Instance.Logger.Log("Method FillPlantList() executed successfully", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlantList() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        //[pramod.misal][GEOS2-6461][24-0-2024] https://helpdesk.emdep.com/browse/GEOS2-6461
        private void NavigateMyPreferences(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);

                MyPreferencesView myPreferencesView = new MyPreferencesView();
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log(string.Format("Method NavigateMyPreferences..."), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateMyPreferences()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            
        }

        /// <summary>
        ///  [pooja.jadhav][06-01-2025][GEOS2-6732]
        /// </summary>
        /// <param name="obj"></param>
        //private void NavigateOTRequestTemplates(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Method NavigateOTRequestTemplates..."), category: Category.Info, priority: Priority.Low);
        //        if (ParentTabView == null)
        //            ParentTabView = new ParentTabView();
        //        if (ParentTabViewModel == null)
        //            ParentTabViewModel = new ParentTabViewModel();
        //        if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == Application.Current.FindResource("OTImportTemplate").ToString()))
        //        {
        //            ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new OTMImportTemplateViewModel { TabName = Application.Current.FindResource("OTImportTemplate").ToString(), ParentViewModel = this, Position = 3 }));
        //            ParentTabViewModel.Sort();
        //            Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
        //        }
        //        GeosApplication.Instance.Logger.Log(string.Format("Method NavigateOTRequestTemplates..."), category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateOTRequestTemplates()..", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }

        //}
        private void NavigateOTRequestTemplates(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateOTRequestTemplates...", category: Category.Info, priority: Priority.Low);
                if (ParentTabView == null)
                    ParentTabView = new ParentTabView();
                if (ParentTabViewModel == null)
                    ParentTabViewModel = new ParentTabViewModel();

                string tabName = Application.Current.FindResource("OTImportTemplate").ToString();
                if (!ParentTabViewModel.Tabs.Any(tab => tab.TabName == tabName))
                {
                    int newPosition = ParentTabViewModel.Tabs.Count > 0
                        ? ParentTabViewModel.Tabs.Max(t => t.Position) + 1
                        : 0;

                    ParentTabViewModel.Tabs.Add(ViewModelSource.Create(() => new OTMImportTemplateViewModel
                    {
                        TabName = tabName,
                        ParentViewModel = this,
                        Position = newPosition
                    }));
                    ParentTabViewModel.Sort();
                    Service.Navigate("Emdep.Geos.Modules.OTM.Views.ParentTabView", ParentTabViewModel, null, this, true);
                }
                GeosApplication.Instance.Logger.Log("Method NavigateOTRequestTemplates...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateOTRequestTemplates()..", ex.Message), category: Category.Exception, priority: Priority.Low);               
            }
        }
        /// <summary>
        /// [Rahul.Gadhave][[GEOS2-9281][Date:05-09-2025]
        /// </summary>
        /// <param name="obj"></param>
        private void NavigateSystemSettings(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction ...", category: Category.Info, priority: Priority.Low);
                SystemSettingsViewModel systemSettingsViewModel = new SystemSettingsViewModel();
                SystemSettingsView systemSettingsView = new SystemSettingsView();
                EventHandler handle = delegate { systemSettingsView.Close(); };
                systemSettingsViewModel.RequestClose += handle;
                systemSettingsView.DataContext = systemSettingsViewModel;
                systemSettingsView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method SystemSettingsViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SystemSettingsViewAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        public void Dispose()
        {
            
        }
        #endregion
    }
}