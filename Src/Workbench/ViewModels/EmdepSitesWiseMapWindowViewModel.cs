using DevExpress.Xpf.Map;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Workbench.ViewModels
{
    class EmdepSitesWiseMapWindowViewModel : INotifyPropertyChanged,IDisposable
    {
        #region Declaration

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private double latitude;    // map  latitude
        private double longitude;   // map  longitude
        private GeoPoint mapLatitudeAndLongitude;   //  nmap point map Latitude And Longitude 
        private string mapPushPin;  // map Push Pin
        private string companyAlias;// site Alias

        ObservableCollection<MapCustomElement> mapCustomElementList; //map Custom ElementList 

        #endregion  // Declaration

        #region  public Properties

        public bool IsInit { get; set; }

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

        public string CompanyAlias
        {
            get { return companyAlias; }
            set { companyAlias = value; OnPropertyChanged(new PropertyChangedEventArgs("CompanyAlias")); }
        }

        #endregion  // Properties

        #region  public event

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion  // Events

        #region  public ICommand

        public ICommand EmdepSitesMapClose { get; set; } //Emdep Sites Map Window close

        #endregion  // ICommand

        #region Constructor

        public EmdepSitesWiseMapWindowViewModel(string CompanyAlias)
        {
            IsInit = true;
            GeosApplication.Instance.Logger.Log("Start EmdepSitesWiseMapWindowViewModel constructor", category: Category.Info, priority: Priority.Low);

            try
            {
                if (!string.IsNullOrEmpty(CompanyAlias))
                {
                    EmdepSitesMapClose = new RelayCommand(new Action<object>(EmdepSitesMapCloseWindow));
                    Company objSite = new Company();

                    GeosApplication.Instance.Logger.Log("Get Site By Alias ", category: Category.Info, priority: Priority.Low);
                    objSite = WorkbenchStartUp.GetCompanyByAlias(CompanyAlias);
                    GeosApplication.Instance.Logger.Log("Get Site By Alias successfully ", category: Category.Info, priority: Priority.Low);

                    MapPushPin = objSite.Name;
                    MapCustomElementList = new ObservableCollection<MapCustomElement>();
                    MapCustomElement MapCustomElement = new DevExpress.Xpf.Map.MapCustomElement();
                    Latitude = objSite.Latitude.Value;
                    Longitude = objSite.Longitude.Value;
                    MapLatitudeAndLongitude = new GeoPoint(Latitude, Longitude);
                    MapCustomElement.Location = MapLatitudeAndLongitude;
                    MapCustomElementList.Add(MapCustomElement);
                    //GeosApplication.Instance.ServerActiveMethod();
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesWiseMapWindowViewModel() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On EmdepSitesWiseMapWindowViewModel", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings.SingleOrDefault(x => x.Key == "Address").Value.ToString(), null);
                if (!GeosApplication.Instance.IsServiceActive)
                {
                    GeosApplication.Instance.ServerDeactiveMethod();
                }
                IsInit = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EmdepSitesWiseMapWindowViewModel() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("End EmdepSitesWiseMapWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
        }

        #endregion  // Constructor

        #region  public Method

        /// <summary>
        /// methdo for close window
        /// </summary>
        /// <param name="obj"></param>
        public void EmdepSitesMapCloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        #endregion

    }
}
