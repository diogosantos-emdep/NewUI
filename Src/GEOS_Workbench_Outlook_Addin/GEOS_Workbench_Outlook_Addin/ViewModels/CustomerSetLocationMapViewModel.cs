using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Map;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class CustomerSetLocationMapViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        private double latitude;    // map  latitude
        private double longitude;   // map  longitude
        private GeoPoint mapLatitudeAndLongitude;   //  map point map Latitude And Longitude 
        private GeoPoint localMapLatitudeAndLongitude;   //  map point map Latitude And Longitude 
        private bool isCoordinatesNull;
        public string LocationAddress { get; set; }
        private CultureInfo culture_En = new CultureInfo("en-US");
        private bool isAcceptButtonEnabled;

        string MapUrl = string.Empty;
        string searchTxt;
        string MapSearchsrt = string.Empty;
        bool isFromGoogle;
        public int IsIntCount { get; set; }

        ObservableCollection<LocationInformation> searchResultsList;
        LocationInformation selectedLocationInformation;
        private string visible;
        #endregion // Declaration

        #region Public Properties

        public ObservableCollection<LocationInformation> SearchResultsList
        {
            get
            {
                return searchResultsList;
            }

            set
            {
                searchResultsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchResultsList"));
            }
        }

        public bool IsAcceptButtonEnabled
        {
            get
            {
                return isAcceptButtonEnabled;
            }

            set
            {
                isAcceptButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButtonEnabled"));
            }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; OnPropertyChanged(new PropertyChangedEventArgs("Latitude")); }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; OnPropertyChanged(new PropertyChangedEventArgs("Longitude")); }
        }

        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
        }

        public GeoPoint MapLatitudeAndLongitude
        {
            get { return mapLatitudeAndLongitude; }
            set
            {
                mapLatitudeAndLongitude = value;

                if (mapLatitudeAndLongitude != null)
                    IsCoordinatesNull = true;
                else
                    IsCoordinatesNull = false;

                OnPropertyChanged(new PropertyChangedEventArgs("MapLatitudeAndLongitude"));
            }
        }

        public GeoPoint LocalMapLatitudeAndLongitude
        {
            get { return localMapLatitudeAndLongitude; }
            set
            {
                localMapLatitudeAndLongitude = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocalMapLatitudeAndLongitude"));
            }
        }

        public string SearchTxt
        {
            get
            {
                return searchTxt;
            }

            set
            {
                searchTxt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchTxt"));

                // SearchCompleted();
            }
        }
        public LocationInformation SelectedLocationInformation
        {
            get
            {
                return selectedLocationInformation;
            }

            set
            {
                selectedLocationInformation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocationInformation"));

                if (selectedLocationInformation != null)
                    MapLatitudeAndLongitude = selectedLocationInformation.Location;

            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        #endregion // Public Properties

        #region public event

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Command

        public ICommand SearchClearCommand { get; set; }
        public ICommand MouseDoubleClickCommand { get; set; }
        public ICommand CustomerSetLocationMapViewCloseButtonCommand { get; set; } //close Window

        public ICommand CustomerSetLocationMapViewAcceptButtonCommand { get; set; } //close Window

        public ICommand ComboBoxEditValueChangeCommand { get; private set; }
        public ICommand CommandTextInput { get; set; }

        #endregion //Command

        #region Constructor

        public CustomerSetLocationMapViewModel()
        {
            SearchResultsList = new ObservableCollection<LocationInformation>();
            GeosApplication.Instance.Logger.Log("Constructor CustomerSetLocationMapViewModel ...", category: Category.Info, priority: Priority.Low);
            IsIntCount = 0;
            SearchClearCommand = new DelegateCommand<BingSearchCompletedEventArgs>(SearchClearAction);
            MouseDoubleClickCommand = new DelegateCommand<RoutedEventArgs>(GetMapLocation);// (GetMapLocation);
            CustomerSetLocationMapViewCloseButtonCommand = new RelayCommand(new Action<object>(WindowClose));
            CustomerSetLocationMapViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptCoordinate));
            ComboBoxEditValueChangeCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(ComboBoxEditValueChangeAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            IsAcceptButtonEnabled = true;

            MapSearchsrt = "https://dev.virtualearth.net/REST/v1/Locations?query={0}&key=s2gyr2NLYqXFe3lNMXLY~5tf9Ztp4qv52kNeVXarbwQ~ArREQRhM-E2eTViuLV2QdA_sdv_K--IlaZ_o-hXzdWemNOKlnPOiuEVQaCA0FDaV";

            //**this code is for change address information provider.
            //**if google is not working then change provider to bing.
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 2000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    isFromGoogle = true;
                    MapUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng= {0},{1}&sensor=false";
                }
                else
                {
                    isFromGoogle = false;
                    MapUrl = "https://dev.virtualearth.net/REST/v1/Locations/ {0},{1}?key=s2gyr2NLYqXFe3lNMXLY~5tf9Ztp4qv52kNeVXarbwQ~ArREQRhM-E2eTViuLV2QdA_sdv_K--IlaZ_o-hXzdWemNOKlnPOiuEVQaCA0FDaV";
                }
                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }
                //SearchResultsList.Add(new LocationInformation() { DisplayName = "'abcdefghijklmnopqrstuvwxyz" });
                //SearchResultsList.Add(new LocationInformation() { DisplayName = "Nagpur" });
                //SearchResultsList.Add(new LocationInformation() { DisplayName = "Sagar" });
                //SearchResultsList.Add(new LocationInformation() { DisplayName = "Anupam" });
                //SearchResultsList.Add(new LocationInformation() { DisplayName = "Sumit" });
            }
            catch (Exception ex)
            {
                isFromGoogle = false;
                MapUrl = "https://dev.virtualearth.net/REST/v1/Locations/ {0},{1}?key=s2gyr2NLYqXFe3lNMXLY~5tf9Ztp4qv52kNeVXarbwQ~ArREQRhM-E2eTViuLV2QdA_sdv_K--IlaZ_o-hXzdWemNOKlnPOiuEVQaCA0FDaV";
            }

            GeosApplication.Instance.Logger.Log("Constructor CustomerSetLocationMapViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion //Constructor

        #region Methods

        /// <summary>
        /// Method for search location.
        /// </summary>
        /// <param name="obj"></param>
        private void ComboBoxEditValueChangeAction(RoutedEventArgs obj)
        {
            //fill list
            GetMapLocationSearch();
        }
        /// <summary>
        /// Method for take location coordinate.
        /// </summary>
        /// <param name="obj"></param>
        private void GetMapLocation(RoutedEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method GetMapLocation ...", category: Category.Info, priority: Priority.Low);

            MapControl mapControl = new MapControl();
            mapControl = (MapControl)obj.Source;
            MapLatitudeAndLongitude = mapControl.Layers[0].ScreenToGeoPoint(((MouseButtonEventArgs)obj).GetPosition(mapControl));
            LocalMapLatitudeAndLongitude = mapControl.Layers[0].ScreenToGeoPoint(((MouseButtonEventArgs)obj).GetPosition(mapControl));
            InformationLayer info = new InformationLayer();
            info = (InformationLayer)mapControl.Layers[2];
            info.ClearResults();

            WebClient client = new WebClient();
            string strLatitude = MapLatitudeAndLongitude.Latitude.ToString(culture_En);  // culture_En is using for keep Latitude in english US culture.
            string strLongitude = MapLatitudeAndLongitude.Longitude.ToString(culture_En); // culture_En is using for keep Longitude in english US culture.
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            string Url = string.Format(MapUrl, strLatitude, strLongitude);
            client.DownloadStringAsync(new Uri(Url, UriKind.RelativeOrAbsolute));

            GeosApplication.Instance.Logger.Log("Method GetMapLocation() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for take full address string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!isFromGoogle)
                {
                    var getResult = e.Result;
                    dynamic root = JsonConvert.DeserializeObject(getResult);

                    foreach (var rs in root.resourceSets)
                    {
                        foreach (var r in rs.resources)
                        {
                            LocationAddress = (r.address.formattedAddress.Value);
                        }
                    }
                }
                else
                {
                    var getResult = e.Result;
                    JObject parseJson = JObject.Parse(getResult);

                    if (parseJson["status"].ToString() != "ZERO_RESULTS")
                    {
                        var getJsonres = parseJson["results"][0];
                        var getJson = getJsonres["formatted_address"];
                        //var getAddress = getJson["long_name"];
                        LocationAddress = getJson.ToString();
                        LocalMapLatitudeAndLongitude = MapLatitudeAndLongitude;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in client_DownloadStringCompleted() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchClearAction(object obj)
        {
            SearchTxt = string.Empty;
            MapLatitudeAndLongitude = null;
        }

        //private void SearchCompleted()
        //{
        //    GetMapLocationSearch();
        //    if (IsIntCount > 0)
        //    { }

        //    else
        //        IsIntCount++;
        //}

        /// <summary>
        /// Method for take location coordinate.
        /// </summary>
        /// <param name="obj"></param>
        private void GetMapLocationSearch()
        {
            GeosApplication.Instance.Logger.Log("Method GetMapLocation ...", category: Category.Info, priority: Priority.Low);

            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringSearchCompleted;
            string Url = string.Format(MapSearchsrt, SearchTxt.Trim());
            client.DownloadStringAsync(new Uri(Url, UriKind.RelativeOrAbsolute));

            GeosApplication.Instance.Logger.Log("Method GetMapLocation() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for take full address string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void client_DownloadStringSearchCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!string.IsNullOrWhiteSpace(SearchTxt))
                {
                    SearchResultsList.Clear();
                    var getResult = e.Result;
                    dynamic root = JsonConvert.DeserializeObject(getResult);

                    foreach (var rs in root.resourceSets)
                    {
                        foreach (var r in rs.resources)
                        {
                            LocationInformation locationInformation = new LocationInformation();

                            locationInformation.DisplayName = r.address.formattedAddress.Value;

                            locationInformation.Location = new GeoPoint();
                            locationInformation.Location.Latitude = r.point.coordinates[0];
                            locationInformation.Location.Longitude = r.point.coordinates[1];

                            SearchResultsList.Add(locationInformation);
                        }
                    }

                    if (SearchResultsList.Count > 0)
                    {
                        SelectedLocationInformation = SearchResultsList[0];
                        MapLatitudeAndLongitude = SearchResultsList[0].Location;
                    }
                }
                else
                {
                    SearchResultsList.Clear();
                    MapLatitudeAndLongitude = null;
                }

                GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in client_DownloadStringCompleted() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// method for Accept Coordinate.
        /// </summary>
        /// <param name="obj"></param>
        public void AcceptCoordinate(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AcceptCoordinate ...", category: Category.Info, priority: Priority.Low);

            //assign final selected coordinate.
            if (LocalMapLatitudeAndLongitude != null)
            {
                MapLatitudeAndLongitude = LocalMapLatitudeAndLongitude;
            }

            if (MapLatitudeAndLongitude != null && !string.IsNullOrWhiteSpace(LocationAddress))
            {
                RequestClose(null, null);
            }

            if (string.IsNullOrWhiteSpace(LocationAddress))
            {
                CustomMessageBox.Show("Unable to determine location, Please choose location again.", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            GeosApplication.Instance.Logger.Log("Method AcceptCoordinate() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// method for close window
        /// </summary>
        /// <param name="obj"></param>
        public void WindowClose(object obj)
        {
            MapLatitudeAndLongitude = null;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion //Methods
    }
}
