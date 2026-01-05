using DevExpress.Mvvm;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Runtime.Serialization.Json;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Core;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeSetLocationMapViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
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
        private double dialogHeight;
        private double dialogWidth;
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
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
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
        public ICommand EmployeeSetLocationMapViewCloseButtonCommand { get; set; } //close Window

        public ICommand EmployeeSetLocationMapViewAcceptButtonCommand { get; set; } //close Window

        public ICommand ComboBoxEditValueChangeCommand { get; private set; }
        public ICommand CommandTextInput { get; set; }

        #endregion //Command

        #region Constructor
        public EmployeeSetLocationMapViewModel()
        {
            System.Windows.Forms.Screen screen = GeosApplication.Instance.GetWorkingScreenFrom();
            DialogWidth = screen.Bounds.Width - 100;
            DialogHeight = screen.Bounds.Height - 130;

            SearchResultsList = new ObservableCollection<LocationInformation>();
            GeosApplication.Instance.Logger.Log("Constructor EmployeeSetLocationMapViewModel ...", category: Category.Info, priority: Priority.Low);
            IsIntCount = 0;
            SearchClearCommand = new DelegateCommand<BingSearchCompletedEventArgs>(SearchClearAction);
            MouseDoubleClickCommand = new DelegateCommand<RoutedEventArgs>(GetMapLocation);// (GetMapLocation);
            EmployeeSetLocationMapViewCloseButtonCommand = new RelayCommand(new Action<object>(WindowClose));
            EmployeeSetLocationMapViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptCoordinate));
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
                GeosApplication.Instance.Logger.Log("Get an error in Method EmployeeSetLocationMapViewModel()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Constructor EmployeeSetLocationMapViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

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
            //LocalMapLatitudeAndLongitude = mapControl.Layers[0].ScreenToGeoPoint(((MouseButtonEventArgs)obj).GetPosition(mapControl));
            LocalMapLatitudeAndLongitude = null;
            InformationLayer info = new InformationLayer();
            info = (InformationLayer)mapControl.Layers[2];
            info.ClearResults();

            GetLocationAddress(MapLatitudeAndLongitude);

            //WebClient client = new WebClient();
            //string strLatitude = MapLatitudeAndLongitude.Latitude.ToString(culture_En);  // culture_En is using for keep Latitude in english US culture.
            //string strLongitude = MapLatitudeAndLongitude.Longitude.ToString(culture_En); // culture_En is using for keep Longitude in english US culture.
            //client.DownloadStringCompleted += client_DownloadStringCompleted;
            //string Url = string.Format(MapUrl, strLatitude, strLongitude);
            //client.DownloadStringAsync(new Uri(Url, UriKind.RelativeOrAbsolute));

            GeosApplication.Instance.Logger.Log("Method GetMapLocation() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for take full address string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted ...", category: Category.Info, priority: Priority.Low);
        //    try
        //    {
        //        if (!isFromGoogle)
        //        {
        //            var getResult = e.Result;
        //            dynamic root = JsonConvert.DeserializeObject(getResult);
        //            foreach (var rs in root.resourceSets)
        //            {
        //                foreach (var r in rs.resources)
        //                {
        //                    LocationAddress = (r.address.formattedAddress.Value);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var getResult = e.Result;
        //            JObject parseJson = JObject.Parse(getResult);

        //            if (parseJson["status"].ToString() != "ZERO_RESULTS")
        //            {
        //                var getJsonres = parseJson["results"][0];
        //                var getJson = getJsonres["formatted_address"];
        //                //var getAddress = getJson["long_name"];
        //                LocationAddress = getJson.ToString();
        //                LocalMapLatitudeAndLongitude = MapLatitudeAndLongitude;
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method client_DownloadStringCompleted() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in client_DownloadStringCompleted() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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
                    LocalMapLatitudeAndLongitude = null;
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
                            LocationAddress = (r.address.formattedAddress.Value);
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
            //if (LocalMapLatitudeAndLongitude != null)
            //{
            //    MapLatitudeAndLongitude = LocalMapLatitudeAndLongitude;
            //    if(LocationAddress == null)
            //    {
            //        string strLatitude = MapLatitudeAndLongitude.Latitude.ToString(culture_En);
            //        string strLongitude = MapLatitudeAndLongitude.Longitude.ToString(culture_En);
            //        RootObject rootObject = getAddress(Convert.ToDouble(strLatitude), Convert.ToDouble(strLongitude));
            //        LocationAddress = rootObject.display_name;
            //    }
            //}

            //if (MapLatitudeAndLongitude != null && string.IsNullOrWhiteSpace(LocationAddress))
            //Shubham[skadam] GEOS2-4013 Automatic location from Map is not supporting latin characters in some occasions 20 12 2022
            if (MapLatitudeAndLongitude != null)
                GetLocationAddress(MapLatitudeAndLongitude);


            if (MapLatitudeAndLongitude != null && !string.IsNullOrWhiteSpace(LocationAddress))
            {
                RequestClose(null, null);
            }

            if (string.IsNullOrWhiteSpace(LocationAddress))
            {
                CustomMessageBox.Show("Unable to determine location, Please choose location again.", Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
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
        /// <summary>
        /// Method to get Location adress by Latitude and Longitude
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static RootObject GetAddress(double lat, double lon)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com/en-us");
            //Shubham[skadam] GEOS2-4013 Automatic location from Map is not supporting latin characters in some occasions 20 12 2022
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + lat + "&lon=" + lon + "&accept-language=en-US");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            return rootObject;
        }

        private void GetLocationAddress(GeoPoint mapLatitudeAndLongitude)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetLocationAddress ...", category: Category.Info, priority: Priority.Low);
                string strLatitude = MapLatitudeAndLongitude.Latitude.ToString(culture_En);
                string strLongitude = MapLatitudeAndLongitude.Longitude.ToString(culture_En);
                RootObject rootObject = GetAddress(Convert.ToDouble(strLatitude), Convert.ToDouble(strLongitude));
                LocationAddress = rootObject.display_name;
                GeosApplication.Instance.Logger.Log("Method GetLocationAddress() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetLocationAddress() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
              
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

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
