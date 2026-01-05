
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Interface;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SCM.Common_Classes
{//[Sudhir.Jangra][GEOS2-4565]
    public class SCMCommon : Prism.Mvvm.BindableBase, INotifyPropertyChanged
    {

        #region Fields
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //   ISCMService SCMService = new SCMServiceController("localhost:6699");
        #endregion

        #region Singleton object

        //Singleton object
        private static readonly SCMCommon instance = new SCMCommon();

        #endregion

        #region Declaration
        bool isPinned;
        private ObservableCollection<Data.Common.Company> plantList;//[Sudhir.Jangra][GEOS2-5203]
        private List<object> selectedPlant;//[Sudhir.Jangra][GEOS2-5203]
        private Data.Common.Company selectedSinglePlant;//[Sudhir.Jangra][GEOS2-5203]
        private bool isCancelbtn;
        private bool isSavebtn= false;
        private bool isSignout=false;
        #region//chitra.girigosavi GEOS2-7867 23/04/2025
        private List<ConnectorProperties> _lstDetailsToSave;
        List<GeosSettings> geosSettings;
        private ObservableCollection<Family> listFamily;
        private ObservableCollection<ConnectorProperties> listConnectorProperties;
        private ObservableCollection<Emdep.Geos.Data.Common.SCM.Color> listColor;
        private ObservableCollection<Gender> listGender;
        //[GEOS2-9552][rdixit][19.09.2025]
        ObservableCollection<ConnectorSubFamily> listSubfamily;
        int tabIndex;
        ObservableCollection<ITabViewModel> _Tabs;
        Visibility isSaveVisible;
        Visibility isSwitchViewVisible;
        #endregion
        #endregion

        #region Properties      
        public bool IsPinned
        {
            get { return isPinned; }
            set
            {
                isPinned = value;
                OnPropertyChanged("IsPinned");
            }
        }
        public ObservableCollection<Data.Common.Company> PlantList
        {
            get { return plantList; }
            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }

        public List<object> SelectedPlant
        {
            get { return selectedPlant; }
            set
            {
                selectedPlant = value;
                OnPropertyChanged("SelectedPlant");
            }
        }

        //[pramod.misal][GEOS2-5524][06.08.2024]
        public Data.Common.Company SelectedSinglePlant
        {
            get { return selectedSinglePlant; }
            set
            {
                selectedSinglePlant = value;
                OnPropertyChanged("SelectedPlant1");
            }
        }

        public static SCMCommon Instance
        {
            get { return instance; }
        }

        private ObservableCollection<Data.Common.SCM.ValueType> selectedList;

        public ObservableCollection<Data.Common.SCM.ValueType> SelectedList
        {
            get { return selectedList; }
            set
            {
                selectedList = value;
                OnPropertyChanged("SelectedList");
            }
        }
        private Dictionary<string, string> customProperties;
        Dictionary<string, string> customPropertiesMax;
        Dictionary<string, string> customPropertiesMin;
        Dictionary<string, List<string>> customPropertiesList;
        public Dictionary<string, List<string>> CustomPropertiesList
        {
            get { return customPropertiesList; }
            set
            {
                customPropertiesList = value;
                OnPropertyChanged("CustomPropertiesList");
            }
        }
        public Dictionary<string,string> CustomProperties
        {
            get { return customProperties; }
            set
            {
                customProperties = value;
                OnPropertyChanged("CustomProperties");
            }
        }
    
        public Dictionary<string, string> CustomPropertiesMin
        {
            get { return customPropertiesMin; }
            set
            {
                customPropertiesMin = value;
                OnPropertyChanged("CustomPropertiesMin");
            }
        }

        
        public Dictionary<string, string> CustomPropertiesMax
        {
            get { return customPropertiesMax; }
            set
            {
                customPropertiesMax = value;
                OnPropertyChanged("CustomPropertiesMax");
            }
        }
        private Grid grid;
        public Grid Grid
        {
            get { return grid; }
            set
            {
                grid = value;
                OnPropertyChanged("Grid");
            }
        }
        //[rdixit][25.01.2024][GEOS2-5148]
        private string header;
        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }
        List<LookUpValues> selectedTypeList;
        public List<LookUpValues> SelectedTypeList
        {
            get { return selectedTypeList; }
            set
            {
                selectedTypeList = value;
                OnPropertyChanged("SelectedTypeList");
            }
        }

        public bool IsPlantChangedEventCanOccur { get; set; }

        //[pramod.misal][04.09.2024][GEOS2-5392]
        public bool IsCancelbtn
        {
            get { return isCancelbtn; }
            set
            {
                isCancelbtn = value;
                OnPropertyChanged("IsCancelbtn");
            }
        }

        //[pramod.misal][04.09.2024][GEOS2-5392]
        public bool IsSavebtn
        {
            get { return isSavebtn; }
            set
            {
                isSavebtn = value;
                OnPropertyChanged("IsSavebtn");
            }
        }

        public bool IsSignout
        {
            get { return isSignout; }
            set
            {
                isSignout = value;
                OnPropertyChanged("IsSignout");
            }
        }

        #region//chitra.girigosavi GEOS2-7867 23/04/2025
        public List<ConnectorProperties> lstDetailsToSave
        {

            get
            {
                return _lstDetailsToSave;
            }
            set
            {
                _lstDetailsToSave = value;
                OnPropertyChanged("lstDetailsToSave");
            }
        }
        public List<GeosSettings> GeosSettings
        {
            get { return geosSettings; }
            set
            {
                geosSettings = value;
                OnPropertyChanged(("GeosSettings"));
            }
        }
        public ObservableCollection<Family> ListFamily
        {
            get
            {
                return listFamily;
            }
            set
            {
                listFamily = value;
                OnPropertyChanged(("ListFamily"));
            }
        }
        public ObservableCollection<ConnectorProperties> ListConnectorProperties
        {
            get
            {
                return listConnectorProperties;
            }
            set
            {
                listConnectorProperties = value;
                OnPropertyChanged(("ListConnectorProperties"));
            }
        }
        public ObservableCollection<Emdep.Geos.Data.Common.SCM.Color> ListColor
        {
            get
            {
                return listColor;
            }
            set
            {
                listColor = value;
                OnPropertyChanged(("ListColor"));
            }
        }
        public ObservableCollection<Gender> ListGender
        {
            get { return listGender; }
            set
            {
                listGender = value;
                OnPropertyChanged(("ListGender"));
            }
        }
        //[GEOS2-9552][rdixit][19.09.2025]
        public ObservableCollection<ConnectorSubFamily> ListSubfamily
        {
            get
            {
                return listSubfamily;
            }
            set
            {
                listSubfamily = value;
                OnPropertyChanged("ListSubfamily");
            }
        }

        public int TabIndex
        {
            get => tabIndex;
            set
            {
                if (tabIndex != value)
                {
                    tabIndex = value;
                    OnPropertyChanged("TabIndex");
                    if (Tabs?.Count > TabIndex && TabIndex>-1)
                    {
                        if(Tabs[tabIndex].TabName.Contains("Result") || 
                            Tabs[tabIndex].TabName.Contains(Application.Current.FindResource("ExactMatch").ToString())||
                            Tabs[tabIndex].TabName.Contains(Application.Current.FindResource("SCMAppearance").ToString())||
                            Tabs[tabIndex].TabName.Contains(Application.Current.FindResource("SCMSearchWaysMargin").ToString())||
                            Tabs[tabIndex].TabName.Contains(Application.Current.FindResource("Components").ToString()))
                        {
                            IsSaveVisible = Visibility.Collapsed;
                            IsSwitchViewVisible = Visibility.Visible;
                        }
                        else
                        {
                            IsSwitchViewVisible = Visibility.Collapsed;
                            IsSaveVisible = Visibility.Visible;
                        }
                    }
                }
            }
        }

        public ObservableCollection<ITabViewModel> Tabs {
            get
            {
                return _Tabs;
            }
            set
            {
                _Tabs = value;
                OnPropertyChanged("Tabs");
            }
        }

        public Visibility IsSaveVisible
        {
            get
            {
                return isSaveVisible;
            }
            set
            {
                isSaveVisible = value;
                OnPropertyChanged("IsSaveVisible");
            }
        }

        public Visibility IsSwitchViewVisible
        {
            get
            {
                return isSwitchViewVisible;
            }
            set
            {
                isSwitchViewVisible = value;
                OnPropertyChanged("IsSwitchViewVisible");
            }
        }

        #endregion

        #endregion

        #region
        public SCMCommon()
        {
            IsPlantChangedEventCanOccur = true;
        }
        #endregion

        #region Methods

        public Boolean Rotate(Bitmap bmp)
        {
            PropertyItem pi = bmp.PropertyItems.Select(x => x)
                                               .FirstOrDefault(x => x.Id == 0x0112);
            if (pi == null) return false;

            byte o = pi.Value[0];

            if (o == 2) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            if (o == 3) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            if (o == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if (o == 5) bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            if (o == 6) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (o == 7) bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
            if (o == 8) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);

            return true;
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        public ImageSource GetByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn != null)
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    Bitmap bmp = new Bitmap(ms);

                    if (SCMCommon.Instance.Rotate(bmp))
                        return Convert(bmp);
                    else
                        return ByteArrayToImage(byteArrayIn);
                }
            }
            return null;
        }
        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new System.Windows.Media.Imaging.BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
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
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ByteArrayToImage." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        #endregion

    }
}
