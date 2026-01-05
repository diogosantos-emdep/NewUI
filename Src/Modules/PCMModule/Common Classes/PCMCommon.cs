using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
using System.Windows;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using Prism.Logging;
using DevExpress.Xpf.LayoutControl;
using System.ComponentModel;
using Emdep.Geos.Modules.PCM.ViewModels;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PLM.ViewModels;
using System.Windows.Input;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Office.NumberConverters;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.PCM.Common_Classes
{

    public sealed class PCMCommon : Prism.Mvvm.BindableBase
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region Task Logs

        /// <summary>
        /// [000][skhade][09-08-2019]Added Singleton class to define common properties and methods for PCM.
        /// </summary>

        #endregion //Task Logs

        #region Declarations

        private string pCM_Appearance;
        MaximizedElementPosition maximizedElementPosition;
        private ProductTypeArticleViewModel getProductTypeArticleViewModelDetails;
        private DetectionsViewModel getProductTypeDetectionViewModelDetails;
        private StructuresDetectionsViewModel getProductTypeStructureDetectionViewModelDetails;//[001][plahange][GEOS2-3956][20/10/2022]//
        private BasePriceListGridViewModel getBasePriceListViewModel;
        private CustomerPriceListGridViewModel getCustomerPriceListViewModel;
        private PriceListPermissionsViewModel getPriceListPermissionsViewModel;
        private DiscountsListGridViewModel getDiscountsListViewModel;//[001][kshinde][GEOS2-3102][25/07/2022]//Discounts
        private string pCM_SelectedCurrencySymbol;
        private Currency selectedCurrency;
        private string announcement;//[Sudhir.Jangra][GEOS2-2597][30/01/2023]
        List<CurrencyConversion> currencyConversionList;


        //[Rahul.Gadhave][GEOS2-6690][Date:16-04-2025]
        private ObservableCollection<PCMArticleCategory> categoryList;
        public IList<LookupValue> tempStatusList { get; set; }
        public IList<LookupValue> tempECOSVisibilityList { get; set; }
        private ObservableCollection<ProductTypesTemplate> moduleMenuList;
        private ObservableCollection<PCMArticleCategory> articleMenuList;
        public IList<LookupValue> tempRelationShipList { get; set; }
        private ObservableCollection<PCMArticleLogEntry> articleChangeLogList;
        private ObservableCollection<Language> languages;
        private List<LookupValue> logicList;
        #endregion //Declarations

        #region Properties
        public string Announcement
        {
            get { return announcement; }
            set
            {
                announcement = value;
                OnPropertyChanged("Announcement");
            }
        }
        public string PCM_Appearance
        {
            get { return pCM_Appearance; }
            set
            {
                pCM_Appearance = value;
                OnPropertyChanged("PCM_Appearance");
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged("MaximizedElementPosition");
            }
        }

        public string PCM_SelectedCurrencySymbol
        {
            get { return pCM_SelectedCurrencySymbol; }
            set
            {
                pCM_SelectedCurrencySymbol = value;
                OnPropertyChanged("PCM_SelectedCurrencySymbol");
            }
        }

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged("SelectedCurrency");
            }
        }
        public List<CurrencyConversion> CurrencyConversionList
        {
            get
            {
                return currencyConversionList;
            }

            set
            {
                currencyConversionList = value;
                OnPropertyChanged("CurrencyConversionList");
            }
        }
        #endregion //Properties
        //[Rahul.Gadhave][GEOS2-6690][Date:16-04-2025]
        public ObservableCollection<PCMArticleCategory> CategoryList
        {
            get
            {
                return categoryList;
            }

            set
            {
                categoryList = value;
                OnPropertyChanged("CategoryList");
            }
        }

        public ObservableCollection<ProductTypesTemplate> ModuleMenulist
        {
            get
            {
                return moduleMenuList;
            }

            set
            {
                moduleMenuList = value;
                OnPropertyChanged("ModuleMenulist");
            }
        }
        public ObservableCollection<PCMArticleCategory> ArticleMenuList
        {
            get
            {
                return articleMenuList;
            }

            set
            {
                articleMenuList = value;
                OnPropertyChanged("ArticleMenuList");
            }
        }
        public ObservableCollection<PCMArticleLogEntry> ArticleChangeLogList
        {
            get
            {
                return articleChangeLogList;
            }

            set
            {
                articleChangeLogList = value;
                OnPropertyChanged("ArticleChangeLogList");
            }
        }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged("Languages");
            }
        }
        public List<LookupValue> LogicList
        {
            get
            {
                return logicList;
            }

            set
            {
                logicList = value;
                OnPropertyChanged("LogicList");
            }
        }






        #region Singleton object

        //Singleton object
        private static readonly PCMCommon instance = new PCMCommon();

        #endregion //Singleton object

        #region Constructor

        private PCMCommon()
        {
        }

        #endregion //Constructor

        #region Public Properties

        public static PCMCommon Instance
        {
            get { return instance; }
        }

        public ProductTypeArticleViewModel GetProductTypeArticleViewModelDetails
        {
            get
            {
                return getProductTypeArticleViewModelDetails;
            }

            set
            {
                getProductTypeArticleViewModelDetails = value;
            }
        }
        public DetectionsViewModel GetProductTypeDetectionViewModelDetails
        {
            get
            {
                return getProductTypeDetectionViewModelDetails;
            }

            set
            {
                getProductTypeDetectionViewModelDetails = value;
            }
        }
        public StructuresDetectionsViewModel GetProductTypeStructureDetectionViewModelDetails //[001][plahange][GEOS2-3956][20/10/2022]//
        {
            get
            {
                return getProductTypeStructureDetectionViewModelDetails;
            }

            set
            {
                getProductTypeStructureDetectionViewModelDetails = value;
            }
        }
        public BasePriceListGridViewModel GetBasePriceListViewModel
        {
            get
            {
                return getBasePriceListViewModel;
            }

            set
            {
                getBasePriceListViewModel = value;
            }
        }
        public CustomerPriceListGridViewModel GetCustomerPriceListViewModel
        {
            get
            {
                return getCustomerPriceListViewModel;
            }

            set
            {
                getCustomerPriceListViewModel = value;
            }
        }
        public PriceListPermissionsViewModel GetPriceListPermissionsViewModel
        {
            get
            {
                return getPriceListPermissionsViewModel;
            }

            set
            {
                getPriceListPermissionsViewModel = value;
            }
        }
        public DiscountsListGridViewModel GetDiscountsListViewModel   //[001][kshinde][GEOS2-3102][25/07/2022]//Discounts
        {
            get
            {
                return getDiscountsListViewModel;
            }

            set
            {
                getDiscountsListViewModel = value;
            }
        }
        #endregion //Properties

        #region Common Methods
        #region Sudhir.Jangra GEOS2-2597 30/01/2023
        public void GetShortcuts()
        {
            if (GeosApplication.Instance.UserSettings != null)
            {
                // shortcuts
                // Get shortcut for Send
                if (GeosApplication.Instance.UserSettings.ContainsKey("Announcement"))
                {
                    Announcement = GeosApplication.Instance.UserSettings["Announcement"].ToString();
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
                    // shortcuts
                    // Get shortcut for Announcement
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Announcement"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Announcement"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            AnnouncementView pCMAnnouncementView = new AnnouncementView();
                            AnnouncementViewModel pCMAnnouncementViewModel = new AnnouncementViewModel();
                            EventHandler handle = delegate { pCMAnnouncementView.Close(); };
                            pCMAnnouncementViewModel.RequestClose += handle;
                            pCMAnnouncementView.DataContext = pCMAnnouncementViewModel;
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            pCMAnnouncementView.ShowDialogWindow();
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
                    return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
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
        #endregion

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

                    if (PCMCommon.Instance.Rotate(bmp))
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
                var image = new BitmapImage();

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

        public MaximizedElementPosition SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                        return MaximizedElementPosition;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString(), true);
                        return MaximizedElementPosition;
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                    return MaximizedElementPosition;
                }
            }
            return MaximizedElementPosition;
        }


        #endregion //Common Methods

    }
}
