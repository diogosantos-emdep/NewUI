using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
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
using System.Windows.Media;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class FamilyAndSubFamilyImagesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISCMService SCMService = new SCMServiceController("localhost:6699");
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());   
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
        ObservableCollection<FamilyImage> familyImageList;
        FamilyImage familyImage;
        FamilyImage selectedImage;
        int selectedImageIndex;
        ImageSource attachmentImage;
        string imageCount;
        static List<FamilyImage> staticFamilyImageList;
        private string imagedescription;


        #endregion

        #region Properties

        public string ImageDescription
        {
            get { return imagedescription; }
            set { imagedescription = value; OnPropertyChanged(new PropertyChangedEventArgs("ImageDescription")); }
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

        public FamilyImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        
        public static List<FamilyImage> StaticFamilyImageList
        {
            get
            {
                return staticFamilyImageList;
            }

            set
            {
                staticFamilyImageList = value;
            }
        }
        public ObservableCollection<FamilyImage> FamilyImageList
        {
            get
            {
                return familyImageList;
            }

            set
            {
                familyImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyImageList"));
            }
        }
        public FamilyImage FamilyImage
        {
            get
            {
                return familyImage;
            }

            set
            {
                familyImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyImage"));
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
        public ICommand ItemPositionChangedCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public FamilyAndSubFamilyImagesViewModel()
        {
            try
            {
                ItemPositionChangedCommand = new DelegateCommand<object>(ItemPositionChangedCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor FamilyAndSubFamilyImagesViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void FamilyInit(object obj)
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
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,//rajashri GEOS2-5106
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;//rajashri GEOS2-5106
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                StaticFamilyImageList = new List<FamilyImage>();
                if (obj is ConnectorSubFamily)
                {
                    ConnectorSubFamily sub = (ConnectorSubFamily)obj;
                    //Service Updated from GetSubFamilyDetails to GetSubFamilyDetails_V2450 [rdixit][19.10.2023][GEOS2-4958]
                    ConnectorSubFamily test = SCMService.GetSubFamilyDetails_V2620(Convert.ToInt32(sub.Id), sub.FamilyName); //[rdixit][GEOS2-8386][08.10.2025]
                    foreach (var item in test.ImageList)
                    {
                        FamilyImage temp = new FamilyImage();
                        temp.ConnectorFamilyImageInBytes = item.SubFamilyImageInBytes;
                        temp.Description = item.Description;
                        temp.Position = item.Position;
                        if (StaticFamilyImageList == null)
                            StaticFamilyImageList = new List<FamilyImage>();
                        StaticFamilyImageList.Add(temp);
                    }
                    ImageDescription = test.Description;//rajashri GEOS2-4956
                }
                else
                {
                    Family fam = (Family)obj;
                    //Service GetFamilyImageImagesByIdFamily_V2430 updated with GetFamilyImageImagesByIdFamily_V2450 by [rdixit][19.10.2023][GEOS2-4958]
                    StaticFamilyImageList = new List<FamilyImage>(SCMService.GetFamilyImagesByIdFamily_V2620(Convert.ToInt32(fam.Id))); //[rdixit][GEOS2-8386][08.10.2025]
                    #region //rajashri GEOS2-4956
                    ConnectorFamily test = SCMService.GetFamilyDetails_V2470(Convert.ToInt32(fam.Id));
                    ImageDescription = test.Description;
                    #endregion

                }

                FamilyImageList = new ObservableCollection<FamilyImage>(StaticFamilyImageList);
                ImageCount = "Images : " + FamilyImageList.Count;
                if (FamilyImageList.Count > 0)
                {               
                    List<FamilyImage> productTypeImage_PositionZero = FamilyImageList.Where(a => a.Position == 0).ToList();
                    List<FamilyImage> productTypeImage_PositionOne = FamilyImageList.Where(a => a.Position == 1).ToList();
                    if (productTypeImage_PositionZero.Count > 0 || productTypeImage_PositionOne.Count > 1)
                    {
                        ulong PositionCount = 1;
                        FamilyImageList.ToList().ForEach(a => { a.Position = PositionCount++; });
                    }

                    FamilyImage tempProductTypeImage = FamilyImageList.FirstOrDefault(x => x.Position == 1);
                    if (tempProductTypeImage != null)
                    {
                        SelectedImage = tempProductTypeImage;
                    }
                    else
                    {
                        SelectedImage = FamilyImageList.FirstOrDefault();
                    }
                    SelectedImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(SelectedImage.ConnectorFamilyImageInBytes);
                    SelectedImageIndex = FamilyImageList.IndexOf(SelectedImage) + 1;
                    foreach (FamilyImage img in FamilyImageList)
                    {
                        img.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(img.ConnectorFamilyImageInBytes);
                    }                   
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);                
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
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
            foreach (FamilyImage img in FamilyImageList)
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

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

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
        #endregion
    }
}
