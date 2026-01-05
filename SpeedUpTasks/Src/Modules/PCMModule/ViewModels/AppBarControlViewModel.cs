using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Common;
using DevExpress.Mvvm;
using System.IO;
using Prism.Logging;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public partial class AppBarControlViewModel
    {

        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        #endregion

        public ICommand CancelButtonCommand { get; set; }
        public ICommand ZoomInCommand { get; set; }
        public ICommand ZoomOutCommand { get; set; }
        public ICommand FlipCommand { get; set; }
        public ICommand ResetScaleCommand { get; set; }
        public ICommand RotateClockwiseCommand { get; set; }
        public ICommand RotateCounterclockwiseCommand { get; set; }
        public ICommand Rotate180Command { get; set; }
        public ICommand RotateResetCommand { get; set; }

        public event EventHandler RequestClose;

        public AppBarControlViewModel()
        {
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ZoomInCommand = new RelayCommand(new Action<object>(ZoomIn));
            ZoomOutCommand = new RelayCommand(new Action<object>(ZoomOut));
            FlipCommand = new RelayCommand(new Action<object>(Flip));
            ResetScaleCommand = new RelayCommand(new Action<object>(ResetScale));
            RotateClockwiseCommand = new RelayCommand(new Action<object>(RotateClockwise));
            RotateCounterclockwiseCommand = new RelayCommand(new Action<object>(RotateCounterclockwise));
            Rotate180Command = new RelayCommand(new Action<object>(Rotate180));
            RotateResetCommand = new RelayCommand(new Action<object>(RotateReset));
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        
        private List<Photo> photos;
        private Photo selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Photo> Photos
        {
            get
            {
                return photos;
            }

            set
            {
                photos = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Photos"));
            }
        }

        public Photo SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        void AddPhoto(string caption, string uri)
        {
            var image = new BitmapImage();
           
            image.BeginInit();
            image.UriSource = new Uri(uri, UriKind.Relative);
            image.EndInit();
            Photos.Add(new Photo { Caption = caption, SizeInfo = "800x800", Source = image, ViewSize = new Size(150, 100) });
        }

        [Command]
        public void RotateClockwise(object obj)
        {
            if (SelectedItem != null)
            {
                switch (SelectedItem.Rotation)
                {
                    case Rotation.Rotate0:
                        SelectedItem.Rotation = Rotation.Rotate90;
                        break;
                    case Rotation.Rotate90:
                        SelectedItem.Rotation = Rotation.Rotate180;
                        break;
                    case Rotation.Rotate180:
                        SelectedItem.Rotation = Rotation.Rotate270;
                        break;
                    case Rotation.Rotate270:
                        SelectedItem.Rotation = Rotation.Rotate0;
                        break;
                }
            }
        }

        [Command]
        public void RotateCounterclockwise(object obj)
        {
            if (SelectedItem != null)
            {
                switch (SelectedItem.Rotation)
                {
                    case Rotation.Rotate0:
                        SelectedItem.Rotation = Rotation.Rotate270;
                        break;
                    case Rotation.Rotate90:
                        SelectedItem.Rotation = Rotation.Rotate0;
                        break;
                    case Rotation.Rotate180:
                        SelectedItem.Rotation = Rotation.Rotate90;
                        break;
                    case Rotation.Rotate270:
                        SelectedItem.Rotation = Rotation.Rotate180;
                        break;
                }
            }
        }

        [Command]
        public void Rotate180(object obj)
        {
            if (SelectedItem != null)
            {
                switch (SelectedItem.Rotation)
                {
                    case Rotation.Rotate0:
                        SelectedItem.Rotation = Rotation.Rotate180;
                        break;
                    case Rotation.Rotate90:
                        SelectedItem.Rotation = Rotation.Rotate270;
                        break;
                    case Rotation.Rotate180:
                        SelectedItem.Rotation = Rotation.Rotate0;
                        break;
                    case Rotation.Rotate270:
                        SelectedItem.Rotation = Rotation.Rotate90;
                        break;
                }
            }
        }

        [Command]
        public void RotateReset(object obj)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Rotation = Rotation.Rotate0;
            }
        }

        //[Command]
        public void ZoomIn(object obj)
        {
            if (SelectedItem != null) SelectedItem.Scale += 0.1;
        }

        public void ZoomOut(object obj)
        {
            if (SelectedItem != null) SelectedItem.Scale = Math.Max(0.1, SelectedItem.Scale - 0.1);
        }

        public void ResetScale(object obj)
        {
            if (SelectedItem != null) SelectedItem.Scale = 1;
        }

        //[Command]
        public void Flip(object obj)
        {
            if (SelectedItem != null) SelectedItem.Flip = !SelectedItem.Flip;
        }

        public void AddPhotoCollection()
        {
            Photos = new List<Photo>();
            SelectedItem.Source = ByteArrayToImage(SelectedItem.ImageBytes);
            Photos.Add(SelectedItem);
        }
        private void AddPhotoCollection(ObservableCollection<ProductTypeImage> imagesList)
        {
            Photos = new List<Photo>();

        }
        private ImageSource ByteArrayToImage(byte[] imageBytes)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (imageBytes != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(imageBytes);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;

                    ImageSource imgSrc = biImg as ImageSource;

                    GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....executed successfully", category: Category.Info, priority: Priority.Low);
                    return imgSrc;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ByteArrayToImage...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return null;
        }

        public void Init(int index)
        {
            SelectedItem = Photos[index];
        }
        
        public void Init(DetectionImage selectedImage)
        {
            SelectedItem = new Photo();
            SelectedItem.source = selectedImage.AttachmentImage;
            SelectedItem.ImageBytes = selectedImage.DetectionImageInBytes;
            SelectedItem.Caption = selectedImage.SavedFileName;
            SelectedItem.SizeInfo = "800x800";
            SelectedItem.ViewSize = new Size(800, 800);

            AddPhotoCollection();
        }

        public void Init(ObservableCollection<ProductTypeImage> imagesList, ProductTypeImage selectedImage)
        {
            Photos = new List<Photo>();
            foreach (ProductTypeImage tempProductTypeImage in imagesList)
            {
                Photo tempPhoto = new Photo();
                tempPhoto.source = tempProductTypeImage.AttachmentImage;
                tempPhoto.ImageBytes = tempProductTypeImage.ProductTypeImageInBytes;
                tempPhoto.Caption = tempProductTypeImage.SavedFileName;
                tempPhoto.SizeInfo = "800x800";
                tempPhoto.ViewSize = new Size(800, 800);
                Photos.Add(tempPhoto);
            }
            if(Photos.Count > 0)
            {
                int imageIndex = imagesList.IndexOf(selectedImage);
                SelectedItem = Photos[imageIndex];
            }
        }

        public void Init(ProductTypeImage selectedImage)
        {
            //Photos = new List<Photo>();
            //Photo tempPhoto = new Photo();
            //tempPhoto.source = temp.AttachmentImage;
            //tempPhoto.ImageBytes = temp.ProductTypeImageInBytes;
            //tempPhoto.Caption = temp.SavedFileName;
            //tempPhoto.SizeInfo = "800x800";
            //tempPhoto.ViewSize = new Size(800, 800);
            //Photos.Add(tempPhoto);
            //SelectedItem = Photos.FirstOrDefault();

            SelectedItem = new Photo();
            SelectedItem.source = selectedImage.AttachmentImage;
            SelectedItem.ImageBytes = selectedImage.ProductTypeImageInBytes;
            SelectedItem.Caption = selectedImage.SavedFileName;
            SelectedItem.SizeInfo = "800x800";
            SelectedItem.ViewSize = new Size(800, 800);

            AddPhotoCollection();

        }

        public void Init(Articles SelectedWarehouseArticle)
        {
            SelectedItem = new Photo();
            //SelectedItem.source = selectedImage.AttachmentImage;
            SelectedItem.ImageBytes = SelectedWarehouseArticle.ArticleImageInBytes;
            SelectedItem.Caption = SelectedWarehouseArticle.Reference;
            SelectedItem.SizeInfo = "800x800";
            SelectedItem.ViewSize = new Size(800, 800);

            AddPhotoCollection();
        }
    }
    
    public class Photo : INotifyPropertyChanged
    {
        public ImageSource source;
        private string caption;
        private string sizeInfo; //{ get; set; }
        private Size viewSize; //{ get; set; }

        public Rotation rotation; //{ get; set; }
        private double scale; // { get; set; }

        private bool flip;
        private byte[] imageBytes;

        public ImageSource Source
        {
            get { return source; }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }


        public bool Flip
        {
            get { return flip; }
            set
            {
                flip = value;
                OnPropertyChanged("Flip");
            }
        }

        public double Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                OnPropertyChanged("Scale");
            }
        }

        public virtual Rotation Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                OnPropertyChanged("Rotation");
            }
        }

        public string Caption
        {
            get
            {  return caption; }
            set
            {
                caption = value;
                OnPropertyChanged("Caption");
            }
        }

        public string SizeInfo
        {
            get { return sizeInfo; }
            set
            {
                sizeInfo = value;
                OnPropertyChanged("SizeInfo");
            }
        }

        public Size ViewSize
        {
            get{ return viewSize; }

            set
            {
                viewSize = value;
                OnPropertyChanged("ViewSize");
            }
        }

        public byte[] ImageBytes
        {
            get
            {
                return imageBytes;
            }

            set
            {
                imageBytes = value;
                OnPropertyChanged("ImageBytes");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public Photo()
        {
            Scale = 1.1;
            ViewSize = new Size(double.NaN, double.NaN);
        }

        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(byteArrayIn);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    biImg.DecodePixelHeight = 10;
                    biImg.DecodePixelWidth = 10;

                    ImageSource imgSrc = biImg as ImageSource;

                    GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....executed successfully", category: Category.Info, priority: Priority.Low);
                    return imgSrc;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ByteArrayToImage...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
    }
}
