using System;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Helper;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using Prism.Logging;
using Emdep.Geos.Modules.SCM.Views;
using System.Linq;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using DevExpress.Diagram.Core.Shapes.Native;
using System.Windows.Media;
using DevExpress.Spreadsheet;
using System.Collections.Generic;
using Emdep.Geos.UI.Commands;
using System.Windows.Input;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //Shubham[skadam] GEOS2-4398 Add new module SCM and search options (2/5) 22 05 2023
    public class SCMMainViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       //ISCMService SCMService = new SCMServiceController("localhost:6699");
        #endregion // Services

        #region Declaration
        bool isBusy;
       
        #endregion

        #region Properties
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public static FamiliesManagerView ObjFamiliesManagerView { get; set; }
        ObservableCollection<LookUpValues> connectorTypeList;
        //[rdixit][GEOS2-5148,5149,5150][29.01.2024]
        public ObservableCollection<LookUpValues> ConnectorTypeList
        {
            get
            {
                return connectorTypeList;
            }
            set
            {
                connectorTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorTypeList"));
            }
        }

        public ICommand LoadedViewInstanceCommand { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        //[rdixit][GEOS2-5752][08.08.2024][001]
        public SCMMainViewModel()
        {

            try
            {
                //[GEOS2-7994][21.05.2025][rdixit]
                SetMyPreferenceDefaultValue();
                //[rdixit][GEOS2-5475][30.04.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    #region All_Plants
                    //[rdixit][GEOS2-5476][30.04.2024]
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMReportsAuditor)
                        SCMCommon.Instance.PlantList = new ObservableCollection<Data.Common.Company>(SCMService.GetAuthorizedPlants_V2550(0));//[001]
                    else
                        SCMCommon.Instance.PlantList = new ObservableCollection<Data.Common.Company>(SCMService.GetAuthorizedPlants_V2550(GeosApplication.Instance.ActiveUser.IdUser));//[001]

                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault(x => x.ShortName == serviceurl);
                    SCMCommon.Instance.SelectedPlant = new System.Collections.Generic.List<object>();
                    if (selectedPlant != null)
                    {
                        SCMCommon.Instance.SelectedPlant.Add(selectedPlant);
                    }
                    foreach (var item in SCMCommon.Instance.PlantList)
                    {
                        if (item != null)
                        {
                            item.ServiceProviderUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.Name == item.Alias).Select(xy => xy.ServiceProviderUrl).FirstOrDefault();
                        }
                    }
                    #endregion

                    TileCollection = new ObservableCollection<TileBarItemsHelper>();

                    #region IsSCMPermissionAdmin & IsSCMPermissionReadOnly
                    //connectors
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly)
                    {
                        TileBarItemsHelper tileBarItemsHelperConnectors = new TileBarItemsHelper();
                        tileBarItemsHelperConnectors.Caption = System.Windows.Application.Current.FindResource("SCMSampleSearch").ToString();
                        tileBarItemsHelperConnectors.BackColor = "#CC6D00";
                        tileBarItemsHelperConnectors.GlyphUri = "Connectors.png";
                        tileBarItemsHelperConnectors.Visibility = Visibility.Visible;
                        tileBarItemsHelperConnectors.Children = new ObservableCollection<TileBarItemsHelper>();

                        TileBarItemsHelper tilebarConnectors = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMConnectorsTitle").ToString(),
                            BackColor = "#CC6D00",
                            GlyphUri = "conector.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => NavigateConnectorsDashboardView(1900))
                        };
                        tileBarItemsHelperConnectors.Children.Add(tilebarConnectors);

                        TileBarItemsHelper tilebarstructure = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMstructure").ToString(),
                            BackColor = "#CC6D00",
                            GlyphUri = "Structures.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => NavigateConnectorsDashboardView(1901))
                        };
                        tileBarItemsHelperConnectors.Children.Add(tilebarstructure);

                        TileBarItemsHelper tilebarComponents = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMcomponents").ToString(),
                            BackColor = "#CC6D00",
                            GlyphUri = "Components.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => NavigateConnectorsDashboardView(1902))
                        };
                        tileBarItemsHelperConnectors.Children.Add(tilebarComponents);
                        TileCollection.Add(tileBarItemsHelperConnectors);

                        //Sample Registraion
                        //[rushikesh.gaikwad][GEOS2-5801][22.07.2024]
                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMSampleRegistrationPermission)
                        {
                            TileBarItemsHelper tileBarForSampleRegistration = new TileBarItemsHelper();
                            tileBarForSampleRegistration.Caption = System.Windows.Application.Current.FindResource("SCMsampleregistraion").ToString();
                            tileBarForSampleRegistration.BackColor = "#00879C";
                            tileBarForSampleRegistration.GlyphUri = "SampleRegistration.png";
                            tileBarForSampleRegistration.Visibility = Visibility.Visible;
                            tileBarForSampleRegistration.NavigateCommand = new DelegateCommand(() => NavigateSampleRegistrationView());//[rdixit][GEOS2-5802][05.09.2024]
                            tileBarForSampleRegistration.Children = new ObservableCollection<TileBarItemsHelper>();
                            TileCollection.Add(tileBarForSampleRegistration);
                        }

                        //Location
                        TileBarItemsHelper tileBarForLocation = new TileBarItemsHelper();
                        tileBarForLocation.Caption = System.Windows.Application.Current.FindResource("SCMlocations").ToString();
                        tileBarForLocation.BackColor = "#FF427940";
                        tileBarForLocation.GlyphUri = "Wlocation.png";
                        tileBarForLocation.Visibility = Visibility.Visible;
                        tileBarForLocation.Children = new ObservableCollection<TileBarItemsHelper>();
                        TileCollection.Add(tileBarForLocation);

                        //Report 
                        TileBarItemsHelper tileBarReports = new TileBarItemsHelper();
                        tileBarReports.Caption = System.Windows.Application.Current.FindResource("SCMReports").ToString();
                        tileBarReports.BackColor = "#a8389f";
                        tileBarReports.GlyphUri = "Reports.png";
                        tileBarReports.Visibility = Visibility.Visible;
                        tileBarReports.Children = new ObservableCollection<TileBarItemsHelper>();
                        TileCollection.Add(tileBarReports);

                        TileBarItemsHelper tilebarNewSampleReport = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMNewSamplesTitle").ToString(),
                            BackColor = "#a8389f",
                            GlyphUri = "NewSample.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => NavigateNewSamplesReportView())
                        };
                        tileBarReports.Children.Add(tilebarNewSampleReport);

                        TileBarItemsHelper tilebarModifiedSamplesreport = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMModifiedSamplesTitle").ToString(),
                            BackColor = "#a8389f",
                            GlyphUri = "ModifiedSamples.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => NavigateModifiedSamplesView())
                        };
                        tileBarReports.Children.Add(tilebarModifiedSamplesreport);

                        TileBarItemsHelper tilebar3dConnectoritemsreport = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCM3DConnectorItemsTitle").ToString(),
                            BackColor = "#a8389f",
                            GlyphUri = "3dConnector.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => Navigate3DConnectorItemsView())
                        };
                        tileBarReports.Children.Add(tilebar3dConnectoritemsreport);

                        if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 104))
                        {
                            TileBarItemsHelper tilebarAudititemsreport = new TileBarItemsHelper()
                            {
                                Caption = System.Windows.Application.Current.FindResource("SCMAuditItemsTitle").ToString(),
                                BackColor = "#a8389f",
                                GlyphUri = "bModulesReport.png",
                                Visibility = Visibility.Visible,
                                // NavigateCommand = new DelegateCommand(() => NavigateConnectorsDashboardView(1908))
                            };
                            tileBarReports.Children.Add(tilebarAudititemsreport);
                            //[rdixit][GEOS2-6654][03.01.2025]
                            TileBarItemsHelper tilereferenceByconnectorreport = new TileBarItemsHelper()
                            {
                                Caption = System.Windows.Application.Current.FindResource("SCMcustomerreferenceTitle").ToString(),
                                BackColor = "#a8389f",
                                GlyphUri = "conector.png",
                                Visibility = Visibility.Visible,
                                NavigateCommand = new DelegateCommand(() => NavigateReferenceByCustomerView())
                            };
                            tileBarReports.Children.Add(tilereferenceByconnectorreport);
                        }
                    }
                    #endregion

                    #region IsSCMPermissionAdmin & IsSCMViewConfigurationPermission
                    //[pramod.misal][GEOS0-5477][07-05-2024]
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        //Configuration 
                        TileBarItemsHelper tileBarConfiguration = new TileBarItemsHelper();
                        tileBarConfiguration.Caption = System.Windows.Application.Current.FindResource("SCMConfiguration").ToString();
                        tileBarConfiguration.BackColor = "#C7BFE6";
                        tileBarConfiguration.GlyphUri = "Configuration.png";
                        tileBarConfiguration.Visibility = Visibility.Visible;
                        tileBarConfiguration.Children = new ObservableCollection<TileBarItemsHelper>();


                        //Properties manager
                        TileBarItemsHelper tileBarItemPropertiesManagerSystemSettings = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMPropertiesManager").ToString(),
                            BackColor = "#C7BFE6",
                            GlyphUri = "PropertiesManager.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigatePropertiesManagerDashboardView)
                        };
                        tileBarConfiguration.Children.Add(tileBarItemPropertiesManagerSystemSettings);

                        //[Aishwarya.Ingale][GEOS2 4561][26/07/2023]
                        //Families manager
                        TileBarItemsHelper tileBarItemFamiliesManagerSystemSettings = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMFamiliesManager").ToString(),
                            BackColor = "#C7BFE6",
                            GlyphUri = "FamilyManager.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(() => { Service.Navigate(FamiliesManagerViewScreen(), null, this); })
                        };
                        tileBarConfiguration.Children.Add(tileBarItemFamiliesManagerSystemSettings);

                        //Search Filter Manager
                        TileBarItemsHelper tileBarItemSearchFilterManager = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMSearchFilterManager").ToString(),
                            BackColor = "#C7BFE6",
                            GlyphUri = "SearchFiltersManager.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand(NavigateSearchFiltersManagerView)
                        };
                        tileBarConfiguration.Children.Add(tileBarItemSearchFilterManager);

                        //[pramod.misal][GEOS2-5525][13-08-2024]
                        //Locations Manager
                        if ( GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission ||
                            GeosApplication.Instance.IsSCMPermissionReadOnly || 
                            GeosApplication.Instance.IsSCMEditLocationsManager) 
                        {
                            TileBarItemsHelper tileBarItemLocationsManager = new TileBarItemsHelper()
                            {
                                Caption = System.Windows.Application.Current.FindResource("SCMLocationsManagerTitle").ToString(),
                                BackColor = "#C7BFE6",
                                GlyphUri = "Location.png",
                                Visibility = Visibility.Visible,
                                NavigateCommand = new DelegateCommand(NavigateLocationsManagerView)
                            };
                            tileBarConfiguration.Children.Add(tileBarItemLocationsManager);
                        }
                       //[Shweta.Thube][GEOS2-6629][27.03.2025]
                       //My Preferences
                        TileBarItemsHelper tileBarItemMyPreferences = new TileBarItemsHelper()
                        {
                            Caption = System.Windows.Application.Current.FindResource("SCMMyPreferences").ToString(),
                            BackColor = "#C7BFE6",
                            GlyphUri = "MyPreference_Black.png",
                            Visibility = Visibility.Visible,
                            NavigateCommand = new DelegateCommand<object>(NavigateMyPreferencesView)
                        };
                        tileBarConfiguration.Children.Add(tileBarItemMyPreferences);


                        TileCollection.Add(tileBarConfiguration);

                    }
                    #endregion

                    GetConnectorType();
                    LoadedViewInstanceCommand = new RelayCommand(new Action<object>(LoadedViewInstanceAction));  //[shweta.thube][GEOS2-6630][04.04.2025]
               

                }
                GeosApplication.Instance.Logger.Log("Constructor Constructor SCMMainViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<Emdep.Geos.Services.Contracts.ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor SCMMainViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in  Constructor SCMMainViewModel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Constructor SCMMainViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

           // GeosApplication.Instance.IsLoadOneTime = false;

            FillSectionsDeatils();  //[shweta.thube][GEOS2-6630][04.04.2025]
            //if (GeosApplication.Instance.UserSettings["AutoRefresh"] == "No")
            //{
            //    int value;
            //    if (int.TryParse(GeosApplication.Instance.UserSettings["LoadDataOn"].ToString(), out value))
            //        if (Convert.ToInt32(GeosApplication.Instance.UserSettings["LoadDataOn"].ToString()) == 1) 
            //            //FillAllObjectsOneTime();

            //}
        }

        //[GEOS2-7994][21.05.2025][rdixit]
        void SetMyPreferenceDefaultValue()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetMyPreferenceDefaultValue ...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> lstPages = CrmService.GetLookupValues(183);
                if (!GeosApplication.Instance.UserSettings.ContainsKey("DefaultView"))
                {
                    GeosApplication.Instance.UserSettings.Add("DefaultView", "Grid");
                }
                if (!GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                {
                    if (lstPages?.Count > 0)
                        GeosApplication.Instance.UserSettings.Add("ResultPages", lstPages.OrderBy(i=>i.Value).FirstOrDefault().Value);
                    else
                        GeosApplication.Instance.UserSettings.Add("ResultPages", "10");
                }
                if (!GeosApplication.Instance.UserSettings.ContainsKey("ImageSize"))
                {
                    GeosApplication.Instance.UserSettings.Add("ImageSize", "80");
                }
                if (!GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                {
                    GeosApplication.Instance.UserSettings.Add("AllowPaging", "false");
                }
                GeosApplication.Instance.Logger.Log("Method SetMyPreferenceDefaultValue() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetMyPreferenceDefaultValue() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void FillSectionsDeatils()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils ...", category: Category.Info, priority: Priority.Low);

                List<SCMSections> scmList = new List<SCMSections>();

                scmList.Add(new SCMSections() { IdSection = 1, SectionName = System.Windows.Application.Current.FindResource("SCMsampleregistraion").ToString() });
                scmList.Add(new SCMSections() { IdSection = 2, SectionName = System.Windows.Application.Current.FindResource("SCMConnectorsTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 3, SectionName = System.Windows.Application.Current.FindResource("SCMLocationsManagerTitle").ToString() });               
                scmList.Add(new SCMSections() { IdSection = 5, SectionName = System.Windows.Application.Current.FindResource("SCMPropertiesManager").ToString() });
                scmList.Add(new SCMSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("SCMFamiliesManager").ToString() });             
                scmList.Add(new SCMSections() { IdSection = 7, SectionName = System.Windows.Application.Current.FindResource("SCMSearchFilterManager").ToString() });
                scmList.Add(new SCMSections() { IdSection = 8, SectionName = System.Windows.Application.Current.FindResource("SCMNewSamplesTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 9, SectionName = System.Windows.Application.Current.FindResource("SCMModifiedSamplesTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 10, SectionName = System.Windows.Application.Current.FindResource("SCM3DConnectorItemsTitle").ToString() });               

                GeosApplication.Instance.ScmSectionsList = scmList;

                string[] ScmSelectedStr = GeosApplication.Instance.UserSettings["SelectedSCMSectionLoadData"].Split(',');
                GeosApplication.Instance.SelectedScmSectionsList = new List<SCMSections>();
                if (ScmSelectedStr != null && ScmSelectedStr[0] != string.Empty)
                {
                    foreach (var item in ScmSelectedStr)
                    {

                        GeosApplication.Instance.SelectedScmSectionsList.Add(GeosApplication.Instance.ScmSectionsList.FirstOrDefault(crm => crm.IdSection == Convert.ToInt16(item.ToString())));
                    }

                }
                else
                {
                    GeosApplication.Instance.SelectedScmSectionsList = GeosApplication.Instance.ScmSectionsList.ToList();
                }

                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSectionsDeatils() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        public void Dispose()
        {
        }
        //[rdixit][GEOS2-5802][05.09.2024]
        private void NavigateSampleRegistrationView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateSampleRegistrationView()...", category: Category.Info, priority: Priority.Low);
                SampleRegistrationView sampleRegistrationView = new SampleRegistrationView();
                SampleRegistrationViewModel sampleRegistrationViewModel = new SampleRegistrationViewModel();
                sampleRegistrationView.DataContext = sampleRegistrationViewModel;
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.SampleRegistrationView", sampleRegistrationViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateSampleRegistrationView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateSampleRegistrationView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigateConnectorsDashboardView(int view)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigateConnectorsDashboardView()...", category: Category.Info, priority: Priority.Low);
                ConnectorView connectorView = new ConnectorView();
                #region [rdixit][GEOS2-5148,5149,5150][29.01.2024]
                if (ConnectorTypeList != null)
                {
                    if (view == 1900)
                    {
                        SCMCommon.Instance.Header = System.Windows.Application.Current.FindResource("SCMConnectorsTitle").ToString();
                        SCMCommon.Instance.SelectedTypeList = ConnectorTypeList.Where(i => i.IdLookupValue == view).ToList();
                    }
                    if (view == 1901)
                    {
                        SCMCommon.Instance.Header = System.Windows.Application.Current.FindResource("SCMstructure").ToString();
                        SCMCommon.Instance.SelectedTypeList = ConnectorTypeList.Where(i => i.IdLookupValue == view).ToList();
                    }
                    if (view == 1902)
                    {
                        SCMCommon.Instance.Header = System.Windows.Application.Current.FindResource("SCMcomponents").ToString();
                        SCMCommon.Instance.SelectedTypeList = ConnectorTypeList.Where(i => i.IdLookupValue == 1902 || i.IdLookupValue == 1903 || i.IdLookupValue == 1904).ToList();
                    }
                }
                #endregion
                //[rdixit][GEOS2-6984][27.03.2025]
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ConnectorViewModel connectorViewModel = new ConnectorViewModel();
                connectorView.DataContext = connectorViewModel;
                connectorViewModel.Init();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.ConnectorView", connectorViewModel, null, this, true);

                GeosApplication.Instance.Logger.Log("Method NavigateDetectionsView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateDetectionsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NavigatePropertiesManagerDashboardView()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NavigatePropertiesManagerDashboardView()...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                PropertiesManagerViewModel propertiesManagerViewModel = new PropertiesManagerViewModel();
                PropertiesManagerView propertiesManagerView = new PropertiesManagerView();
                EventHandler handle = delegate { propertiesManagerView.Close(); };
                propertiesManagerViewModel.RequestClose += handle;
                propertiesManagerViewModel.WindowHeader = System.Windows.Application.Current.FindResource("PropertyManagerHeader").ToString();
                propertiesManagerView.DataContext = propertiesManagerViewModel;
                IsBusy = false;
                propertiesManagerView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method NavigatePropertiesManagerDashboardView()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigatePropertiesManagerDashboardView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pallavi jadhav][GEOS2-4562][26 07 2023]
        private FamiliesManagerView FamiliesManagerViewScreen()
        {
            GC.Collect();

            FamiliesManagerView familiesManagerView = new FamiliesManagerView();
            FamiliesManagerViewModel familiesManagerViewModel = new FamiliesManagerViewModel();
            familiesManagerView.DataContext = familiesManagerViewModel;
            familiesManagerViewModel.Init();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GC.Collect();
            ObjFamiliesManagerView = familiesManagerView;
            return familiesManagerView;

        }

        //[Sudhir.Jangra][GEOS2-4971]
        private void NavigateSearchFiltersManagerView()
        {
            try
            {
                SearchFiltersManagerView searchFiltersManagerView = new SearchFiltersManagerView();
                SearchFiltersManagerViewModel searchFiltersManagerViewModel = new SearchFiltersManagerViewModel();
                EventHandler handle = delegate { searchFiltersManagerView.Close(); };
                searchFiltersManagerViewModel.RequestClose += handle;
                searchFiltersManagerView.DataContext = searchFiltersManagerViewModel;
                searchFiltersManagerViewModel.Init();
                searchFiltersManagerView.ShowDialogWindow();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateSearchFiltersManagerView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-5203]
        private void NavigateNewSamplesReportView()
        {
            try
            {
                NewSamplesView newSamplesView = new NewSamplesView();
                NewSamplesViewModel newSamplesViewModel = new NewSamplesViewModel();

                newSamplesView.DataContext = newSamplesViewModel;
                newSamplesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.NewSamplesView", newSamplesViewModel, null, this, true);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateNewSamplesReportView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5204]
        private void NavigateModifiedSamplesView()
        {
            try
            {
                ModifiedSamplesView modifiedSamplesView = new ModifiedSamplesView();
                ModifiedSamplesViewModel modifiedSamplesViewModel = new ModifiedSamplesViewModel();
                modifiedSamplesView.DataContext = modifiedSamplesViewModel;
                modifiedSamplesViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.ModifiedSamplesView", modifiedSamplesViewModel, null, this, true);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateModifiedSamplesView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-5205]
        private void Navigate3DConnectorItemsView()
        {
            try
            {
                ThreeDConnectorView threeDConnectorView = new ThreeDConnectorView();
                ThreeDConnectorViewModel threeDConnectorViewModel = new ThreeDConnectorViewModel();
                threeDConnectorView.DataContext = threeDConnectorViewModel;
                threeDConnectorViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.ThreeDConnectorView", threeDConnectorViewModel, null, this, true);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method Navigate3DConnectorItemsView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][GEOS2-6654][03.01.2025]
        private void NavigateReferenceByCustomerView()
        {
            try
            {
                CustomerReferenceView customerReferenceView = new CustomerReferenceView();
                CustomerReferenceViewModel customerReferenceViewModel = new CustomerReferenceViewModel();
                customerReferenceView.DataContext = customerReferenceViewModel;
                customerReferenceViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.CustomerReferenceView", customerReferenceViewModel, null, this, true);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateReferenceByCustomerView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        void GetConnectorType()//[rdixit][GEOS2-5148,5149,5150][29.01.2024]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Started GetConnectorType()....", Category.Info, priority: Priority.Low);
                ConnectorTypeList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(145));
                GeosApplication.Instance.Logger.Log("Ended GetConnectorType()....", Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in GetConnectorType() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetConnectorType()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5524][06.08.2024]
        private void NavigateLocationsManagerView()
        {
            try
            {
                LocationsManagerView locationsManagerView = new LocationsManagerView();
                LocationsManagerViewModel locationsManagerViewModel = new LocationsManagerViewModel();
                locationsManagerView.DataContext = locationsManagerViewModel;
                //locationsManagerViewModel.Init();
                Service.Navigate("Emdep.Geos.Modules.SCM.Views.LocationsManagerView", locationsManagerViewModel, null, this, true);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method NavigateLocationsManagerView()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.Thube][GEOS2-6629][27.03.2025]      
        private void NavigateMyPreferencesView(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenMyPreference ...", category: Category.Info, priority: Priority.Low);           
                IsBusy = true;
                MyPreferencesView myPreferencesView = new MyPreferencesView();
                MyPreferencesViewModel myPreferencesViewModel = new MyPreferencesViewModel();
                EventHandler handle = delegate { myPreferencesView.Close(); };
                myPreferencesViewModel.RequestClose += handle;
                myPreferencesView.DataContext = myPreferencesViewModel;
                IsBusy = false;
                myPreferencesView.ShowDialogWindow();

                GeosApplication.Instance.Logger.Log("Method NavigateMyPreferencesView() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in NavigateMyPreferencesView() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void LoadedViewInstanceAction(object obj)
        {
            SCMShortcuts.Instance.MainWindowINavigationService = this.Service;
        }
       
        #endregion
    }
}