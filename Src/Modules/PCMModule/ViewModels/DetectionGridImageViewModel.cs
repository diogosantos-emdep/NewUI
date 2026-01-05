using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.WindowsUI;
using WindowsUIDemo;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.Data.Common.PLM;
using Newtonsoft.Json;
using Emdep.Geos.Data.Common.SynchronizationClass;
using DevExpress.Data.Filtering;
using Emdep.Geos.Modules.PLM.CommonClasses;
using System.Net;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class DetectionGridImageViewModel : ViewModelBase, INotifyPropertyChanged

    {
        #region Service
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
        #endregion


        #region Events
        public event EventHandler RequestClose;

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
        ObservableCollection<DetectionImage> detectionImageList;
        DetectionImage detectionImage;
        DetectionImage selectedImage;
        int selectedImageIndex;
        ImageSource attachmentImage;
        string imageCount;
        #endregion

        #region Properties    

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

        public DetectionImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        static List<DetectionImage> staticDetectionImageList;
        public static List<DetectionImage> StaticDetectionImageList
        {
            get
            {
                return staticDetectionImageList;
            }

            set
            {
                staticDetectionImageList = value;             
            }
        }
        public ObservableCollection<DetectionImage> DetectionImageList
        {
            get
            {
                return detectionImageList;
            }

            set
            {
                detectionImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionImageList"));
            }
        }
        public DetectionImage DetectionImage
        {
            get
            {
                return detectionImage;
            }

            set
            {
                detectionImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionImage"));
            }
        }
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }

            set
            {
                attachmentImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentImage"));
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
        //public ICommand DetectionItemPositionChangedCommandAction { get; set; }
        public ICommand ItemPositionChangedCommand { get; set; }

        public ICommand CancelButtonCommand { get; set; }
        #endregion

        #region Constructor

        public DetectionGridImageViewModel()
        {
            try
            {
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor DetectionGridImageViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion



        #region Methods

        public void Init(UInt32 IdDetection)
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

                if(StaticDetectionImageList == null)
                    StaticDetectionImageList = PCMService.GetDetectionImagesByIdDetection_V2380(IdDetection).ToList();//Service V2370 version updated with version V2380  [rdixit][GEOS2-2922][24.04.2023]

                else if (StaticDetectionImageList.Any(i => i.IdDetection != IdDetection))
                    StaticDetectionImageList = PCMService.GetDetectionImagesByIdDetection_V2380(IdDetection).ToList();

                DetectionImageList = new ObservableCollection<DetectionImage>(StaticDetectionImageList);
                ImageCount = "Images : " + DetectionImageList.Count;
                if (DetectionImageList.Count > 0)
                {
                    foreach (DetectionImage item in DetectionImageList)
                    {
                        string url = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Detections/" + item.IdDetectionImage + "/" + item.SavedFileName;
                        try
                        {
                            if (GeosApplication.IsImageURLException == false)
                            {
                                //using (WebClient webClient = new WebClient())
                                //{                                    
                                //    item.DetectionImageInBytes = webClient.DownloadData(url);
                                //}
                                item.DetectionImageInBytes = Utility.ImageUtil.GetImageByWebClient(url);
                            }
                            else
                            {                               
                                item.DetectionImageInBytes = GeosService.GetImagesByUrl(url);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                            if (GeosApplication.IsImageURLException == false)
                                GeosApplication.IsImageURLException = true;

                            item.DetectionImageInBytes = GeosService.GetImagesByUrl(url);
                        }
                    }

                    List<DetectionImage> productTypeImage_PositionZero = DetectionImageList.Where(a => a.Position == 0).ToList();
                    List<DetectionImage> productTypeImage_PositionOne = DetectionImageList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        DetectionImageList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    DetectionImage tempProductTypeImage = DetectionImageList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;

                    }
                    else
                    {
                        SelectedImage = DetectionImageList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.DetectionImageInBytes);


                    SelectedImageIndex = DetectionImageList.IndexOf(SelectedImage) + 1;

                    foreach (DetectionImage img in DetectionImageList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.DetectionImageInBytes);
                    }
                    // DetectionImageList = new ObservableCollection<DetectionImage>(DetectionImageList.OrderBy(a => a.Position));
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
            foreach (DetectionImage img in DetectionImageList)
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
        #endregion
    }
}
