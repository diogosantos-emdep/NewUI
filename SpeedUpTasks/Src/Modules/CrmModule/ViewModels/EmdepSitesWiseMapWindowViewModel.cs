using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Emdep.Geos.Modules.Crm.ViewModels
{
    class EmdepSitesWiseMapWindowViewModel : INotifyPropertyChanged
    {
        #region Declaration
        // IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(Workbench.Properties.Settings.Default.Address, Workbench.Properties.Settings.Default.Port);
        private double latitude;// map  latitude
        private double longitude; // map  longitude
        private GeoPoint mapLatitudeAndLongitude;//  nmap point map Latitude And Longitude 
        private string mapPushPin;// map Push Pin
        private string siteAlias;// site Alias
        ObservableCollection<MapCustomElement> mapCustomElementList; //map Custom ElementList 
        public event EventHandler RequestClose;
        public bool IsInit { get; set; }
        #endregion

        #region  public Properties
        public ObservableCollection<MapCustomElement> MapCustomElementList
        {
            get { return mapCustomElementList; }
            set { mapCustomElementList = value; }
        }
        public string MapPushPin
        {
            get { return mapPushPin; }
            set { mapPushPin = value; }
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
        public GeoPoint MapLatitudeAndLongitude
        {
            get { return mapLatitudeAndLongitude; }
            set { mapLatitudeAndLongitude = value; OnPropertyChanged(new PropertyChangedEventArgs("MapLatitudeAndLongitude")); }
        }
        public string SiteAlias
        {
            get { return siteAlias; }
            set { siteAlias = value; OnPropertyChanged(new PropertyChangedEventArgs("SiteAlias")); }
        }
        #endregion

        #region  public event
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region  public ICommand
        public ICommand EmdepSitesMapClose { get; set; } //Emdep Sites Map Window close
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public EmdepSitesWiseMapWindowViewModel(string SiteAlias)
        {
            IsInit = true;
            GeosApplication.Instance.Logger.Log("Constructor EmdepSitesWiseMapWindowViewModel ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (!string.IsNullOrEmpty(SiteAlias))
                {
                    EmdepSitesMapClose = new RelayCommand(new Action<object>(EmdepSitesMapCloseWindow));
                    CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                    MapPushPin = "spain";// objSite.Name;
                    MapCustomElementList = new ObservableCollection<MapCustomElement>();
                    MapCustomElement MapCustomElement = new DevExpress.Xpf.Map.MapCustomElement();
                    Latitude = 41.292149;// objSite.Latitude;
                    Longitude = 1.299219;// objSite.Longitude;
                    MapLatitudeAndLongitude = new GeoPoint(Latitude, Longitude);
                    MapCustomElement.Location = MapLatitudeAndLongitude;
                    MapCustomElementList.Add(MapCustomElement);
                    //GeosApplication.Instance.ServerActiveMethod();
                }

                GeosApplication.Instance.Logger.Log("Constructor EmdepSitesWiseMapWindowViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesWiseMapWindowViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region  public Method

        /// <summary>
        /// methdo for close window
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesMapCloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
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
        #endregion

    }
}
