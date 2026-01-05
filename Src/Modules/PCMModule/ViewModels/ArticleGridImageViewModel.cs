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
    public class ArticleGridImageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());       
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
       // IPCMService PCMService = new PCMServiceController("localhost:6699");
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
        ObservableCollection<PCMArticleImage> imagesList;
        static List<PCMArticleImage> staticImagesList;
        Articles articles;
        private ImageSource referenceImage;
        private bool isReferenceImageExist;
        private PCMArticleImage selectedImage;   
        int selectedImageIndex;
        string imageCount;
        #endregion

        #region Properties
        public ObservableCollection<PCMArticleImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }
        public static List<PCMArticleImage> StaticImagesList
        {
            get { return staticImagesList; }
            set
            {
                staticImagesList = value;
            
            }
        }
        public Articles Articles
        {
            get { return articles; }
            set
            {
                articles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Articles"));
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
                    ReferenceImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/ImageEditLogo.png"));
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
        public PCMArticleImage SelectedImage
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
        #endregion

        #region Constructor
        public ArticleGridImageViewModel()
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
        public void Init(Articles selectedArticle)
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
                if(StaticImagesList == null)
                    StaticImagesList = PCMService.GetArticleImage_V2380(selectedArticle);//service V2370 updated with V2380 [rdixit][GEOS2-2922][24.04.2023]

                if (StaticImagesList.Any(i => i.IdArticle != selectedArticle.IdArticle))
                    StaticImagesList = PCMService.GetArticleImage_V2380(selectedArticle);//service V2370 updated with V2380 [rdixit][GEOS2-2922][24.04.2023]
                                                                                                                              

                ImagesList = new ObservableCollection<PCMArticleImage>(StaticImagesList);
                ImageCount = "Images : " + ImagesList.Count;             
                #region
                /*   var templist = temp1.PCMArticleImageList;
                   foreach (Articles item in temp)
                   {                        
                       if (ImagesList.Count > 0 && ImagesList != null && item.ArticleImageInBytes != null)
                       {
                           foreach (PCMArticleImage position in templist)
                           {
                               string Position = position.Position.ToString();
                               if (Position == string.Empty)
                               {
                                   item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.ArticleImageInBytes);
                               }
                               else
                               {
                                  item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(position.PCMArticleImageInBytes);
                               }
                           }
                       }
                       else if (item.ArticleImageInBytes != null && ImagesList.Count == 0)
                       {
                           item.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(item.ArticleImageInBytes);
                       }
                       else if (item.ArticleImageInBytes == null && ImagesList.Count > 0 && ImagesList != null)
                       {
                           if (ImagesList.Count > 0 && ImagesList != null)
                           {
                               for (ulong pos = 1; pos <= ImagesList.Max(a => a.Position) + 1; pos++)
                               {
                                   if (!(ImagesList.Any(a => a.Position == pos)))
                                   {
                                       PCMArticleImage Image = new PCMArticleImage();
                                       Image.PCMArticleImageInBytes = item.ArticleImageInBytes;
                                       Image.IsWarehouseImage = 1;
                                       Image.SavedFileName = item.Reference;
                                       Image.OriginalFileName = item.Reference;
                                       Image.Position = pos;
                                       Image.IsImageShareWithCustomer = item.IsImageShareWithCustomer;
                                       ImagesList.Add(Image);
                                       OldWarehouseImage = ImagesList.FirstOrDefault(a => a.IsWarehouseImage == 1);
                                       oldWarehouseposition = OldWarehouseImage.Position;

                                       break;
                                   }
                               }
                           }
                           else
                           {
                               PCMArticleImage Image = new PCMArticleImage();
                               Image.PCMArticleImageInBytes = item.ArticleImageInBytes;
                               Image.IsWarehouseImage = 1;
                               Image.SavedFileName = item.Reference;
                               Image.OriginalFileName = item.Reference;
                               Image.Position = 1;
                               Image.IsImageShareWithCustomer = item.IsImageShareWithCustomer;
                               ImagesList.Add(Image);
                           }
                           ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.OrderBy(x => x.Position).ToList());
                           if (item.ArticleImageInBytes != null && (!ImagesList.Any(a => a.Position == 1)))
                           {
                               ReferenceImage = ByteArrayToBitmapImage(item.ArticleImageInBytes);
                           }
                           else
                           {
                               if (item.ArticleImageInBytes == null && (ImagesList.Count == 0))
                               {
                                   ReferenceImage = ByteArrayToBitmapImage(selectedArticle.ArticleImageInBytes);
                                   OldReferenceImage = ReferenceImage;
                               }
                               else
                                   ReferenceImage = ByteArrayToBitmapImage(ImagesList.FirstOrDefault(a => a.Position == 1).PCMArticleImageInBytes);
                           }
                           OldReferenceImage = ReferenceImage;
                           */
                #endregion
                if (ImagesList.Count > 0)
                {
                    foreach (PCMArticleImage item in ImagesList)
                    {
                        if (item.IdPCMArticleImage != 0)
                        {
                            //string url = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Articles/" + selectedArticle.Reference + "/" + item.SavedFileName;

                            string url = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Articles/" + selectedArticle.Reference + "/" + item.IdPCMArticleImage + "_" + item.SavedFileName;   //[rushikesh.gaikwad][10.06.2024][GEOS2-5763]
                            try
                            {
                                if (GeosApplication.IsImageURLException == false)
                                {
                                    //using (WebClient webClient = new WebClient())
                                    //{
                                    //    item.PCMArticleImageInBytes = webClient.DownloadData(url);
                                    //}
                                    item.PCMArticleImageInBytes = Utility.ImageUtil.GetImageByWebClient(url);
                                }
                                else
                                {
                                    item.PCMArticleImageInBytes = GeosService.GetImagesByUrl(url);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                                if (GeosApplication.IsImageURLException == false)
                                    GeosApplication.IsImageURLException = true;

                                item.PCMArticleImageInBytes = GeosService.GetImagesByUrl(url);
                            }
                        }
                        //[rushikesh.gaikwad][10.06.2024][GEOS2-5763]
                        else
                        {
                            //string url = "https://api.emdep.com/GEOS/Images?FilePath=/Images/Articles/" + selectedArticle.Reference + "/"+ item.SavedFileName;
                            string url = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/ARTICLE%20VISUAL%20AIDS/" + selectedArticle.Reference + "/" + item.SavedFileName;
                            try
                            {
                                if (GeosApplication.IsImageURLException == false)
                                {
                                    //using (WebClient webClient = new WebClient())
                                    //{
                                    //    item.PCMArticleImageInBytes = webClient.DownloadData(url);
                                    //}
                                    item.PCMArticleImageInBytes = Utility.ImageUtil.GetImageByWebClient(url);
                                }
                                else
                                {
                                    item.PCMArticleImageInBytes = GeosService.GetImagesByUrl(url);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Added code to get https data for the user which is out side of the VPN  [rdixit][24.05.2023]
                                if (GeosApplication.IsImageURLException == false)
                                    GeosApplication.IsImageURLException = true;

                                item.PCMArticleImageInBytes = GeosService.GetImagesByUrl(url);
                            }
                        }
                    }
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
                        ImagesList = new ObservableCollection<PCMArticleImage>(ImagesList.ToList().OrderBy(o => (o.Position)).ToList());
                    }

                    List<PCMArticleImage> productTypeImage_PositionZero = ImagesList.Where(a => a.Position == 0).ToList();
                    List<PCMArticleImage> productTypeImage_PositionOne = ImagesList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        ImagesList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    PCMArticleImage tempProductTypeImage = ImagesList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                      
                    }
                    else
                    {
                        SelectedImage = ImagesList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(SelectedImage.PCMArticleImageInBytes);
                   

                    SelectedImageIndex = ImagesList.IndexOf(SelectedImage) + 1;
                }

                foreach (PCMArticleImage img in ImagesList)
                {
                    img.AttachmentImage = PCMCommon.Instance.GetByteArrayToImage(img.PCMArticleImageInBytes);
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
            foreach (PCMArticleImage img in ImagesList)
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
