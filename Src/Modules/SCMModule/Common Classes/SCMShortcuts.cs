using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;
using Emdep.Geos.Modules.SCM.ViewModels;
using DevExpress.Mvvm;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
//[shweta.thube][GEOS2-6630][04.04.2025]
namespace Emdep.Geos.Modules.SCM.Common_Classes
{
    public sealed class SCMShortcuts : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        #endregion

        #region Singleton object

        //Singleton object
        private static readonly SCMShortcuts instance = new SCMShortcuts();

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



        #endregion
        #region Declaration
        private string create;
        private string search;
        private string locations;
        private string properties;
        private string families;
        private string searchManager;
        private string newSamples;
        private string modifiedSamples;
        private string connectors3D;
        private INavigationService mainWindowINavigationService;
        public event EventHandler RequestClose;
        private bool isActive=false;
        #endregion

        #region Properties      

        public static SCMShortcuts Instance
        {
            get { return instance; }
        }

        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }
        public string Create
        {
            get
            {
                return create;
            }

            set
            {
                create = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Create"));
            }
        }
        public string Search
        {
            get
            {
                return search;
            }

            set
            {
                search = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Search"));
            }
        }
        public string Locations
        {
            get
            {
                return locations;
            }

            set
            {
                locations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Locations"));
            }
        }
        public string Properties
        {
            get
            {
                return properties;
            }

            set
            {
                create = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Properties"));
            }
        }
        public string Families
        {
            get
            {
                return families;
            }

            set
            {
                families = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Families"));
            }
        }
        public string SearchManager
        {
            get
            {
                return searchManager;
            }

            set
            {
                searchManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchManager"));
            }

        }

        public string NewSamples
        {
            get
            {
                return newSamples;
            }

            set
            {
                newSamples = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewSamples"));
            }

        }

        public string ModifiedSamples
        {
            get
            {
                return modifiedSamples;
            }

            set
            {
                modifiedSamples = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedSamples"));
            }

        }

        public string Connectors3D
        {
            get
            {
                return connectors3D;
            }

            set
            {
                connectors3D = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Connectors3D"));
            }

        }
        public INavigationService MainWindowINavigationService
        {
            get
            {
                return mainWindowINavigationService;
            }
            set
            {
                mainWindowINavigationService = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MainWindowINavigationService"));
            }
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsActive"));
            }

        }
        #endregion


        #region Constructor
        public SCMShortcuts()
        {

        }
        #endregion
        #region Methods

        public void GetShortcuts()
        {
            if (GeosApplication.Instance.UserSettings != null)
            {
                // shortcuts
                if (GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                {
                    Create = GeosApplication.Instance.UserSettings["Create"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                {
                    Search = GeosApplication.Instance.UserSettings["Search"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                {
                    Locations = GeosApplication.Instance.UserSettings["Locations"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                {
                    Properties = GeosApplication.Instance.UserSettings["Properties"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                {
                    Families = GeosApplication.Instance.UserSettings["Families"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                {
                    SearchManager = GeosApplication.Instance.UserSettings["SearchManager"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                {
                    NewSamples = GeosApplication.Instance.UserSettings["NewSamples"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                {
                    ModifiedSamples = GeosApplication.Instance.UserSettings["ModifiedSamples"].ToString();
                }

                if (GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                {
                    Connectors3D = GeosApplication.Instance.UserSettings["Connectors3D"].ToString();
                }
            }
        }

        public void OpenWindowClickOnShortcutKey(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = "";
                if (obj.KeyboardDevice.Modifiers == ModifierKeys.None)
                {
                    ShortcutKey = obj.Key.ToString();
                }
                else
                {
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ShortcutKey = "ctrl";
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + shift";
                        }
                        else
                        {
                            ShortcutKey = "shift";
                        }
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + alt";
                        }
                        else
                        {
                            ShortcutKey = "alt";
                        }
                    }

                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + windows";
                        }
                        else
                        {
                            ShortcutKey = "windows";
                        }
                    }
                    if (obj.Key == Key.System)
                    {
                        if (obj.SystemKey.ToString().Contains("Left") || obj.SystemKey.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.SystemKey.ToString();
                        }
                    }
                    else
                    {
                        if (obj.Key.ToString().Contains("Left") || obj.Key.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.Key.ToString();
                        }
                    }
                }

                string[] Keys = ShortcutKey.Split('+');

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["NewSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    NewSamplesView newSamplesView = new NewSamplesView();
                                    NewSamplesViewModel newSamplesViewModel = new NewSamplesViewModel();
                                    newSamplesView.DataContext = newSamplesViewModel;
                                    newSamplesViewModel.Init();
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.NewSamplesView", newSamplesViewModel, null, this, true);
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["ModifiedSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    ModifiedSamplesView modifiedSamplesView = new ModifiedSamplesView();
                                    ModifiedSamplesViewModel modifiedSamplesViewModel = new ModifiedSamplesViewModel();
                                    modifiedSamplesView.DataContext = modifiedSamplesViewModel;
                                    modifiedSamplesViewModel.Init();
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.ModifiedSamplesView", modifiedSamplesViewModel, null, this, true);
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Connectors3D"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    ThreeDConnectorView threeDConnectorView = new ThreeDConnectorView();
                                    ThreeDConnectorViewModel threeDConnectorViewModel = new ThreeDConnectorViewModel();
                                    threeDConnectorView.DataContext = threeDConnectorViewModel;
                                    threeDConnectorViewModel.Init();
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.ThreeDConnectorView", threeDConnectorViewModel, null, this, true);

                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Search"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    ConnectorView connectorView = new ConnectorView();
                                    SCMCommon.Instance.Header = System.Windows.Application.Current.FindResource("SCMConnectorsTitle").ToString();
                                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                                    ConnectorViewModel connectorViewModel = new ConnectorViewModel();
                                    connectorView.DataContext = connectorViewModel;
                                    connectorViewModel.Init();
                                    IsActive = true;
                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.ConnectorView", connectorViewModel, null, this, true);
                                    
                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMSampleRegistrationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Create"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    SampleRegistrationView sampleRegistrationView = new SampleRegistrationView();
                                    SampleRegistrationViewModel sampleRegistrationViewModel = new SampleRegistrationViewModel();
                                    sampleRegistrationView.DataContext = sampleRegistrationViewModel;
                                    //RequestClose(null, null);
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.SampleRegistrationView", sampleRegistrationViewModel, null, this, true);

                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["SearchManager"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    SearchFiltersManagerView searchFiltersManagerView = new SearchFiltersManagerView();
                                    SearchFiltersManagerViewModel searchFiltersManagerViewModel = new SearchFiltersManagerViewModel();
                                    EventHandler handle = delegate { searchFiltersManagerView.Close(); };
                                    searchFiltersManagerViewModel.RequestClose += handle;
                                    searchFiltersManagerView.DataContext = searchFiltersManagerViewModel;
                                    searchFiltersManagerViewModel.Init();
                                    IsActive = true;
                                    searchFiltersManagerView.ShowDialogWindow();
                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Properties"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    PropertiesManagerViewModel propertiesManagerViewModel = new PropertiesManagerViewModel();
                                    PropertiesManagerView propertiesManagerView = new PropertiesManagerView();
                                    EventHandler handle = delegate { propertiesManagerView.Close(); };
                                    propertiesManagerViewModel.RequestClose += handle;
                                    propertiesManagerView.DataContext = propertiesManagerViewModel;
                                    IsActive = true;
                                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                                    propertiesManagerView.ShowDialog();

                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Families"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    FamiliesManagerView familiesManagerView = new FamiliesManagerView();
                                    FamiliesManagerViewModel familiesManagerViewModel = new FamiliesManagerViewModel();
                                    familiesManagerView.DataContext = familiesManagerViewModel;
                                    familiesManagerViewModel.Init();
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.FamiliesManagerView", familiesManagerViewModel, null, this, true);
                                }
                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission ||
                               GeosApplication.Instance.IsSCMPermissionReadOnly ||
                               GeosApplication.Instance.IsSCMEditLocationsManager)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Locations"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    Processing();
                                    LocationsManagerView locationsManagerView = new LocationsManagerView();
                                    LocationsManagerViewModel locationsManagerViewModel = new LocationsManagerViewModel();
                                    locationsManagerView.DataContext = locationsManagerViewModel;
                                    //locationsManagerViewModel.Init();
                                    IsActive = true;
                                    MainWindowINavigationService.Navigate("Emdep.Geos.Modules.SCM.Views.LocationsManagerView", locationsManagerViewModel, null, this, true);

                                }
                            }


                        }

                    }                    
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenWindowClickOnShortcutKey...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private int getComparedShortcutKeyCount(string[] Keys, string[] StoredKeys)
        {
            int count = 0;
            if (Keys.Count() == StoredKeys.Count())
            {
                foreach (string key in Keys)
                {
                    foreach (string storedKey in StoredKeys)
                    {
                        if (key.ToUpper().TrimStart().TrimEnd() == storedKey.ToUpper().TrimStart().TrimEnd())
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private void Processing()
        {
            if (!DXSplashScreen.IsActive)
            {
                //DXSplashScreen.Show<SplashScreenView>(); 
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
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
        }
        #endregion
    }
}
