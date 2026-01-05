using System;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Prism.Logging;
using DevExpress.Mvvm;
using DevExpress.Xpf.Map;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class MapLocatorViewModel
    {
        #region TaskLog

        /// <summary>
        /// [001][skhade][M051-02][Add map locator field in warehouse locations]
        /// </summary>
        /// 

        #endregion

        #region Services

        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private int screenHeight = 750;

        public double? width;
        public double? height;
        public double? latitude;
        public double? longitude;
        public bool IsAcceptClick;

        private Uri svgUri;

        #endregion

        #region Properties

        public Uri SvgUri
        {
            get { return svgUri; }
            set { svgUri = value; }
        }
        public int ScreenHeight
        {
            get { return screenHeight; }
            set { screenHeight = value; }
        }

        #endregion

        #region public Events

        public event EventHandler RequestClose;

        #endregion

        #region Public Commands

        public ICommand MapLocatorViewAcceptCommand { get; set; }
        public ICommand MapLocatorViewCancelCommand { get; set; }
        public ICommand VectorLayerDataLoadedCommand { get; set; }
        public ICommand CustomizeMapItemCommand { get; set; }
        public ICommand MapItemCreatingCommand { get; set; }
        public ICommand MapItemEditedCommand { get; set; }

        #endregion

        //public DelegateCommand<object> VectorLayerDataLoadedCommand { get; private set; }
        //public DelegateCommand<object> CustomizeMapItemCommand { get; private set; }
        //public DelegateCommand<object> MapItemCreatingCommand { get; private set; }
        //public DelegateCommand<object> MapItemEditedCommand { get; private set; }

        #region Constructor

        public MapLocatorViewModel()
        {
            MapLocatorViewAcceptCommand = new RelayCommand(new Action<object>(AcceptCommandAction));
            MapLocatorViewCancelCommand = new RelayCommand(new Action<object>(CloseWindow));
            VectorLayerDataLoadedCommand = new DelegateCommand<object>(VectorLayerDataLoaded, true);
            //CustomizeMapItemCommand = new DelegateCommand<object>(ListSourceDataAdapterCustomizeMapItem, true);
            MapItemCreatingCommand = new DelegateCommand<object>(MapItemCreating, true);
            MapItemEditedCommand = new DelegateCommand<object>(MapItemEdited, true);
        }

        #endregion

        #region Methods

        public void Init(FileDetail mapFileDetail, WarehouseLocation TempLocation)
        {
            try
            {
                //System.Windows.Forms.Screen.FromControl(this).Bounds
                //System.Windows.SystemParameters.WorkArea.Width
                //System.Windows.SystemParameters.WorkArea.Height
                var interopHelper = new System.Windows.Interop.WindowInteropHelper(System.Windows.Application.Current.MainWindow);
                var activeScreen = System.Windows.Forms.Screen.FromHandle(interopHelper.Handle);

                ScreenHeight = activeScreen.Bounds.Height - 100;

                if (TempLocation.Height != 0 && TempLocation.Width != 0)
                {
                    height = TempLocation.Height;
                    width = TempLocation.Width;
                    latitude = TempLocation.Latitude;
                    longitude = TempLocation.Longitude;
                }

                if (mapFileDetail != null && mapFileDetail.FileByte != null)
                {
                    string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    string svgFilePath = string.Format(@"{0}\Data\{1}", Path.GetTempPath(), mapFileDetail.FileName);
                    File.WriteAllBytes(svgFilePath, mapFileDetail.FileByte);
                    SvgUri = new Uri(svgFilePath);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Loading warehouse layout svg file - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Loading warehouse layout svg file - {0}. ErrorMessage- {1}", mapFileDetail.FileName, ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        bool loaded = false;
        void VectorLayerDataLoaded(object obj)
        {
            if (obj == null || loaded) return;
            loaded = true;

            MapControl mapControl = obj as MapControl;
            mapControl.ZoomToFitLayerItems();

            VectorLayer vectorLayer = new VectorLayer();

            //Create a storage for map items and generate them.
            MapItemStorage storage = new MapItemStorage();

            if (height != null && height != 0
                && width != null && width != 0
                && latitude != null
                && longitude != null)
            {
                storage.Items.Clear();
                MapRectangle mapRectangle = new MapRectangle() { Height = (double)height, Width = (double)width, Location = new GeoPoint((double)latitude, (double)longitude) };

                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                {
                    mapRectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2AB7FF");
                }
                else if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    mapRectangle.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF083493");
                }

                storage.Items.Add(mapRectangle);
            }

            vectorLayer.Data = storage;
            mapControl.Layers.Add(vectorLayer);
            mapControl.MapEditor.ActiveLayer = vectorLayer;
        }

        void MapItemCreating(object obj)
        {
            if (obj == null) return;

            MapShape shape = ((DevExpress.Xpf.Map.MapItemCreatingEventArgs)obj).Item as MapShape;
            if (shape != null)
            {
                if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                {
                    shape.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF2AB7FF");
                }
                else if (ApplicationThemeHelper.ApplicationThemeName == "WhiteAndBlue")
                {
                    shape.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FF083493");
                }
            }
        }

        void MapItemEdited(object obj)
        {
            //MapControl mapControl = obj as MapControl;
            //mapControl.ZoomToFitLayerItems();

            if (obj == null) return;

            var mapItems = ((DevExpress.Xpf.Map.MapItemEditedEventArgs)obj).Items;

            if (mapItems.ToList().Count > 0)
            {
                try
                {
                    MapRectangle rectangle = mapItems.ToList()[0] as MapRectangle;

                    width = rectangle.Width;
                    height = rectangle.Height;
                    latitude = ((DevExpress.Xpf.Map.GeoPoint)rectangle.Location).Latitude;
                    longitude = ((DevExpress.Xpf.Map.GeoPoint)rectangle.Location).Longitude;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in MapItemEdited - ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }

        }

        //void ListSourceDataAdapterCustomizeMapItem(object e)
        //{
        //    MapRectangle rectangle = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).MapItem as MapRectangle;
        //    WarehouseLocation wl = ((DevExpress.Xpf.Map.CustomizeMapItemEventArgs)e).SourceObject as WarehouseLocation;

        //    if (wl.HtmlColor != null)
        //        rectangle.Fill = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString(wl.HtmlColor);

        //    rectangle.Height = wl.Height;
        //    rectangle.Width = wl.Width;

        //    //Fill = brush;
        //    //dot.Fill = wl.HtmlColor;
        //    //QueryResultLatLong sourceDot = e.SourceObject as QueryResultLatLong;
        //    //dot.Fill = sourceDot.Color;
        //    //dot.Size = sourceDot.Value;
        //}

        private void AcceptCommandAction(object obj)
        {
            IsAcceptClick = true;
            RequestClose(null, null);
            DeleteSvgFiles();
        }

        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
            DeleteSvgFiles();
        }

        private void DeleteSvgFiles()
        {
            try
            {
                string basePath = string.Format(@"{0}\Data\", Path.GetTempPath());

                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, true);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in deleting warehouse layout svg files. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

    }
}
