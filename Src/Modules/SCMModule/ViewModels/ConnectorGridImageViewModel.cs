using System;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.SCM.Views;
using Prism.Logging;
using System.ComponentModel;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.Collections.Generic;
using System.Windows.Input;
using DevExpress.Mvvm.UI;
using System.Linq;
using System.Net;
using Emdep.Geos.Data.Common.SCM;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Emdep.Geos.Modules.SCM.Common_Classes;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class ConnectorGridImageViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Service
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declaration
        ObservableCollection<SCMConnectorImage> imagesList;
        static List<SCMConnectorImage> staticImagesList;
        private ImageSource referenceImage;
        private bool isReferenceImageExist;
        private SCMConnectorImage selectedImage;
        int selectedImageIndex;
        string imageCount;
       

        #endregion

        #region Properties


        public ObservableCollection<SCMConnectorImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }
        public static List<SCMConnectorImage> StaticImagesList
        {
            get { return staticImagesList; }
            set
            {
                staticImagesList = value;

            }
        }
        public ImageSource ReferenceImage
        {
            get { return referenceImage; }
            set
            {
                referenceImage = value;
                if (referenceImage != null)
                {
                    IsReferenceImageExist = true;
                }
                else
                {
                    ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.SCM;component/Assets/Images/ImageEditLogo.png"));
                    IsReferenceImageExist = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceImage"));
            }
        }
        public bool IsReferenceImageExist
        {
            get { return isReferenceImageExist; }
            set
            {
                isReferenceImageExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReferenceImageExist"));
            }
        }
        public SCMConnectorImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));

            }
        }
        public int SelectedImageIndex
        {
            get
            {
                return selectedImageIndex;
            }

            set
            {
                selectedImageIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImageIndex"));
            }
        }
        public string ImageCount
        {
            get { return imageCount; }
            set
            {
                imageCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageCount"));
            }
        }
        #endregion

        #region ICommands
        public ICommand ItemPositionChangedCommand { get; set; }

        public ICommand CancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }        //[shweta.thube][GEOS2-6630][04.04.2025]


        #endregion

        #region Constructor
        public ConnectorGridImageViewModel()
        {
            try
            {
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);        //[shweta.thube][GEOS2-6630][04.04.2025]
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor ArticleGridImageViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        public void Init(Connectors selectedConnectors)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
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
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                StaticImagesList = null;
                //SCMService = new SCMServiceController("localhost:6699");
                //Service GetAllConnectorImages updated with GetAllConnectorImages_V2460 [rdixit][GEOS2-4951][08.12.2023]
                //StaticImagesList = SCMService.GetAllConnectorImages_V2460(selectedConnectors);
                //[Rahul.Gadhave][GEOS2-5779][Date-09/08/2024]
                if (StaticImagesList == null)                   
                    StaticImagesList = SCMService.GetAllConnectorImagesForImageSection_V2550(selectedConnectors);

                ImagesList = new ObservableCollection<SCMConnectorImage>(StaticImagesList);
               
                ImageCount = "Images : " + ImagesList.Count;
                int count = 0;
                if (ImagesList != null)
                {
                    foreach (var Images in ImagesList)
                    {
                       //[GEOS2-9203][rdixit][06.10.2025]
                        try
                        {
                            count = count + 1;
                            if (!ImagesList.Any(a => a.Position == (uint)count))
                            {
                                if (ImagesList.Any(a => a.Position == 0))
                                {
                                    var temp = ImagesList.Where(s => s.Position == 0).FirstOrDefault();
                                    temp.Position = (uint)count;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                    ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList.ToList().OrderBy(o => (o.Position)).ToList());
                }

                List<SCMConnectorImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                List<SCMConnectorImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                {
                    ulong PositionCount = 1;
                    ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                }

                SCMConnectorImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                if (tempProductTypeImage != null)
                {
                    SelectedImage = tempProductTypeImage;
                }
                else
                {
                    SelectedImage = ImagesList.FirstOrDefault();
                }
                //[GEOS2-9199][rdixit][07.10.2025]
                if (SelectedImage.SavedFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                {
                    var drawingImg = GetwtgByteArrayToImage(SelectedImage.ConnectorsImageInBytes);
                    SelectedImage.AttachmentImage = ConvertDrawingImageToImageSource(drawingImg);
                }
                else
                    SelectedImage.AttachmentImage = GetByteArrayToImage(selectedConnectors.ConnectorsImageInBytes);

                SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;

                foreach (SCMConnectorImage img in ImagesList)
                {
                    //[GEOS2-9199][rdixit][07.10.2025]
                    if (img.SavedFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                    {
                        var drawingImg = GetwtgByteArrayToImage(img.ConnectorsImageInBytes);
                        img.AttachmentImage = ConvertDrawingImageToImageSource(drawingImg);
                    }
                    else
                    {
                        img.AttachmentImage = GetByteArrayToImage(img.ConnectorsImageInBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method Init()...", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                throw;
            }
        }
        private void ItemPositionChangedCommandAction(object obj)
        {
            ulong pos = 1;
            foreach (SCMConnectorImage img in ImagesList)
            {
                img.Position = pos;
                pos++;
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public Boolean Rotate(Bitmap bmp)
        {
            try
            {
                //[GEOS2-9203][rdixit][06.10.2025]
                System.Drawing.Imaging.PropertyItem pi = bmp.PropertyItems.Select(x => x).FirstOrDefault(x => x.Id == 0x0112);
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
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in Method Rotate: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                return false;
            }
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
            if (byteArrayIn != null && byteArrayIn?.Length > 0)
            {
                try
                {
                    //[GEOS2-9203][rdixit][06.10.2025]
                    using (var ms = new MemoryStream(byteArrayIn))
                    {
                        Bitmap bmp = new Bitmap(ms);

                        if (Rotate(bmp))
                            return Convert(bmp);
                        else
                            return ByteArrayToImage(byteArrayIn);
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log($"Error in Method GetByteArrayToImage: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                    return null;
                }
            }
            GeosApplication.Instance.Logger.Log("Invalid byte array input: null or empty", category: Category.Exception, priority: Priority.Low);
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

        public void InitView(Connectors selectedConnectors)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
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
                StaticImagesList = null;
                //SCMService = new SCMServiceController("localhost:6699");
                if (StaticImagesList == null)
                    StaticImagesList = SCMService.GetAllConnectorImages(selectedConnectors);

                ImagesList = new ObservableCollection<SCMConnectorImage>(StaticImagesList.Where(w => w.Ref.Equals(selectedConnectors.Ref) && w.SavedFileName.Equals(selectedConnectors.FileName)));
                ImageCount = "Images : " + ImagesList.Count;
                int count = 0;
                if (ImagesList != null)
                {
                    foreach (var Images in ImagesList)
                    {
                        count = count + 1;
                        if (!ImagesList.Any(a => a.Position == (uint)count))
                        {
                            if (ImagesList.Any(a => a.Position == 0))
                            {
                                var temp = ImagesList.Where(s => s.Position == 0).FirstOrDefault();
                                temp.Position = (uint)count;
                            }
                        }
                    }
                    ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList.ToList().OrderBy(o => (o.Position)).ToList());
                }

                List<SCMConnectorImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                List<SCMConnectorImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                {
                    ulong PositionCount = 1;
                    ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                }

                SCMConnectorImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                if (tempProductTypeImage != null)
                {
                    SelectedImage = tempProductTypeImage;

                }
                else
                {
                    SelectedImage = ImagesList.FirstOrDefault();
                }
                SelectedImage.AttachmentImage = GetByteArrayToImage(selectedConnectors.ConnectorsImageInBytes);


                SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;

                foreach (SCMConnectorImage img in ImagesList)
                {
                    img.AttachmentImage = GetByteArrayToImage(img.ConnectorsImageInBytes);

                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method InitView()...", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                throw;
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-9199][rdixit][07.10.2025]
        public Image GetwtgByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                if (byteArrayIn != null)
                {
                    GeosApplication.Instance.Logger.Log("Execution started in Method GetwtgByteArrayToImage.", category: Category.Info, priority: Priority.Low);
                    byte[] array;
                    byte a = 1;
                    Array objArray = Array.CreateInstance(a.GetType(), byteArrayIn.Length);
                    Array objArrayDest = Array.CreateInstance(a.GetType(), byteArrayIn.Length - 50); ;
                    byteArrayIn.CopyTo(objArray, 0);
                    Array.Copy(objArray, 50, objArrayDest, 0, byteArrayIn.Length - 50);
                    array = (byte[])objArrayDest;
                    MemoryStream myStream = new MemoryStream();
                    myStream.Write(array, 0, array.Length);
                    Image Img = Image.FromStream(myStream);
                    return Img;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method GetwtgByteArrayToImage." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }

        //[GEOS2-9199][rdixit][07.10.2025]
        public ImageSource ConvertDrawingImageToImageSource(Image drawingImage)
        {
            try
            {
                if (drawingImage != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        drawingImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStream.Position = 0;
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                    }
                    return bitmapImage;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ConvertDrawingImageToImageSource." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }
        #endregion

    }
}