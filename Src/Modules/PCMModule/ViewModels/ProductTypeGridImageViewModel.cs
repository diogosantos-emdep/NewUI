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
using System.Net;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class ProductTypeGridImageViewModel
    {
        #region Services
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        ObservableCollection<ProductTypeImage> productImageList;
        ProductTypeImage productImage;
        ProductTypeImage selectedImage;
        int selectedImageIndex;
        string imageCount;
        #endregion

        #region Properties
        public ObservableCollection<ProductTypeImage> ProductImageList
        {
            get { return productImageList; }
            set
            {
                productImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductImageList"));
            }
        }
        static List<ProductTypeImage> staticProductImageList;
        public static List<ProductTypeImage> StaticProductImageList
        {
            get { return staticProductImageList; }
            set
            {
                staticProductImageList = value;             
            }
        }
        public ProductTypeImage ProductImage
        {
            get { return productImage; }
            set
            {
                productImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductImage"));
            }
        }
        public ProductTypeImage SelectedImage
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
            get { return selectedImageIndex; }
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
        #endregion


        #region Constructor
        public ProductTypeGridImageViewModel()
        {
            try
            {
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor ArticleGridImageViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        public void Init(UInt32 IdCPType)
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
                //ObservableCollection<ProductTypeImage> temp = new ObservableCollection<ProductTypeImage>(PCMService.GetProductTypeImagesByIdProductType(IdCPType));
                if (StaticProductImageList == null)                {                    StaticProductImageList = PCMService.GetProductTypeImagesByIdProductTypeForGrid_V2380(IdCPType);//Service GetProductTypeImagesByIdProductType updated with version V2380  [rdixit][GEOS2-2922][24.04.2023]
                    ProductImageList = new ObservableCollection<ProductTypeImage>(StaticProductImageList.Where(pi => pi.IdCPType == IdCPType).ToList());                }                else if (StaticProductImageList.Any(pi => pi.IdCPType == IdCPType))                    ProductImageList = new ObservableCollection<ProductTypeImage>(StaticProductImageList.Where(pi => pi.IdCPType == IdCPType).ToList());                else                {                    StaticProductImageList = PCMService.GetProductTypeImagesByIdProductTypeForGrid_V2380(IdCPType);//Service GetProductTypeImagesByIdProductType updated with version V2380  [rdixit][GEOS2-2922][24.04.2023]
                    ProductImageList = new ObservableCollection<ProductTypeImage>(StaticProductImageList.Where(pi => pi.IdCPType == IdCPType).ToList());                }

                ProductImageList = new ObservableCollection<ProductTypeImage>(StaticProductImageList);
                ImageCount = "Images : " + ProductImageList.Count;
                if (ProductImageList.Count > 0)
                {
                    foreach (ProductTypeImage item in ProductImageList)
                    {
                        string url = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Counterparts/" + item.IdCPTypeImage + "/" + item.SavedFileName;
                        try
                        {
                            if (GeosApplication.IsImageURLException == false)
                            {
                                //using (WebClient webClient = new WebClient())
                                //{
                                //    item.ProductTypeImageInBytes = webClient.DownloadData(url);
                                //}
                                item.ProductTypeImageInBytes = Utility.ImageUtil.GetImageByWebClient(url);
                            }
                            else
                            {
                                item.ProductTypeImageInBytes = GeosService.GetImagesByUrl(url);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                            if (GeosApplication.IsImageURLException == false)
                                GeosApplication.IsImageURLException = true;

                            item.ProductTypeImageInBytes = GeosService.GetImagesByUrl(url);
                        }
                    }

                    List<ProductTypeImage> productTypeImage_PositionZero = ProductImageList.Where(a => a.Position == 0).ToList();
                    List<ProductTypeImage> productTypeImage_PositionOne = ProductImageList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        ProductImageList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    ProductTypeImage tempProductTypeImage = ProductImageList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;

                    }
                    else
                    {
                        SelectedImage = ProductImageList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.ProductTypeImageInBytes);


                    SelectedImageIndex = ProductImageList.IndexOf(SelectedImage) + 1;

                    foreach (ProductTypeImage img in ProductImageList)
                    {
                        img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.ProductTypeImageInBytes);
                    }
                    ProductImageList = new ObservableCollection<ProductTypeImage>(ProductImageList.OrderBy(a => a.Position));
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
            foreach (ProductTypeImage img in ProductImageList)
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
