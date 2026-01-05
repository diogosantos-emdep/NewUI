using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using System.Windows.Media;
using Prism.Logging;
using System.Windows;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using iTextSharp.text.pdf;
using System.Windows.Controls;
using System.IO;
using Emdep.Geos.Modules.Hrm.Reports;
using DevExpress.XtraPrinting;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeExpensePhotosViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Task_Comments
        //[GEOS2-3957][rdixit][07.10.2022]
        #endregion

        #region Services
        private INavigationService Service { get { return GetService<INavigationService>(); } }
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services

        #region Public ICommand
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand ChangeImageCommand { get; set; }
        public ICommand Image_MouseDownCommand { get; set; }

        #endregion

        #region public Events       
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion // Events

        #region Declaration
        byte[] selectedPhotobyte;
        int angle;
        ObservableCollection<EmployeeExpensePhotoInfo> photos;
        ImageSource selectedPhoto;
        string employeeExpensePhotosHeader;
        string selectedPhotoType;
        #endregion

        #region Properties
        public byte[] SelectedPhotobyte
        {
            get { return selectedPhotobyte; }
            set
            {
                selectedPhotobyte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPhotobyte"));

            }
        }
        public ImageSource SelectedPhoto
        {
            get { return selectedPhoto; }
            set
            {
                selectedPhoto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPhoto"));

            }
        }
        public string SelectedPhotoType
        {
            get
            {
                return selectedPhotoType;
            }
            set
            {
                selectedPhotoType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPhotoType"));
            }
        }
        public ObservableCollection<EmployeeExpensePhotoInfo> Photos
        {
            get { return photos; }
            set
            {
                photos = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Photos"));
            }
        }        
        public string EmployeeExpensePhotosHeader
        {
            get
            {
                return employeeExpensePhotosHeader;
            }
            set
            {
                employeeExpensePhotosHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExpensePhotosHeader"));
            }
        }
        #endregion

        #region Constructor
        public EmployeeExpensePhotosViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor EmployeeExpensePhotosViewModel()...", category: Category.Info, priority: Priority.Low);
            angle = 0;
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
            ChangeImageCommand = new DelegateCommand<object>(ChangeImageCommandAction);
            Image_MouseDownCommand = new DelegateCommand<object>(Image_MouseDownCommandAction);
            GeosApplication.Instance.Logger.Log("Constructor EmployeeExpensePhotosViewModel()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        #endregion

        #region Methods

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void GetImages(int IdEmployeeExpense)
        {          
            try
            {
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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                GeosApplication.Instance.Logger.Log("Method GetImages()...", category: Category.Info, priority: Priority.Low);
                //GetEmployeeExpenseImageInBytes Service Changed to GetEmployeeExpenseImageInBytes_V2340  [GEOS2-3957][rdixit][15.12.2022] To get Images of Pdf Documents.
                //GetEmployeeExpenseImageInBytes Service Changed to GetEmployeeExpenseImageInBytes_V2350  [GEOS2-4010][rdixit][20.01.2023] To get Pdf Documents.
                Photos = new ObservableCollection<EmployeeExpensePhotoInfo>(HrmService.GetEmployeeExpenseImageInBytes_V2350(IdEmployeeExpense));
                if (Photos != null)
                {
                    foreach (var item in Photos)
                    {
                        item.ExpenseImage = ByteArrayToBitmapImage(item.ImageInByte);
                    }
                    var defaultImage = Photos.FirstOrDefault(i => i.ExpenseImage != null);
                    if (defaultImage != null)
                    {
                        SelectedPhotobyte = defaultImage.ImageInByte;
                        SelectedPhoto = defaultImage.ExpenseImage;
                        SelectedPhotoType = defaultImage.AttachmentType;
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GetImages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetImages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetImages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetImages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
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
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        public void ChangeImageCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeImageCommandAction....", category: Category.Info, priority: Priority.Low);

                var values = (System.Windows.RoutedEventArgs)obj;
                DevExpress.Xpf.Editors.ListBoxEdit SelectedListBoxEdit = (DevExpress.Xpf.Editors.ListBoxEdit)values.Source;
                EmployeeExpensePhotoInfo EmployeeExpensePhoto = (EmployeeExpensePhotoInfo)SelectedListBoxEdit.SelectedItem;
                SelectedPhoto = EmployeeExpensePhoto.ExpenseImage;
                SelectedPhotobyte = EmployeeExpensePhoto.ImageInByte;
                SelectedPhotoType = EmployeeExpensePhoto.AttachmentType;
                GeosApplication.Instance.Logger.Log("Method ChangeImageCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ChangeImageCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Added Changes To Rotate the Image [rdixit][20.10.2022][GEOS2-3957]
        private void Image_MouseDownCommandAction(object obj)
        {
            System.Windows.Input.MouseButtonEventArgs temp = obj as System.Windows.Input.MouseButtonEventArgs;
            //System.Windows.Controls.Image thumb = temp.Source as System.Windows.Controls.Image;
            //Point centerPoint = new Point(thumb.RenderTransformOrigin.X / 2, thumb.RenderTransformOrigin.Y / 2);
            //angle = angle + 90;
            //if (angle == 360) angle = 0;
            //RotateTransform rotateTransform = new RotateTransform(angle) { CenterX = centerPoint.X, CenterY = centerPoint.Y };
            //thumb.RenderTransform = rotateTransform;
            OpenPDFDocument();
        }
        //[GEOS2-4010][rdixit][20.01.2023] To get Pdf Documents.
        private void OpenPDFDocument()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument..."), category: Category.Info, priority: Priority.Low);
                EmployeeExpensePhotoInfo EmployeeExpensePhoto = Photos.FirstOrDefault(i => i.ExpenseImage == SelectedPhoto);
                if (EmployeeExpensePhoto != null)
                {
                    byte[] EmployeeExitAttachmentBytes = EmployeeExpensePhoto.PdfInByte;
                    EmployeeDocumentView employeeDocumentView = new EmployeeDocumentView();
                    EmployeeDocumentViewModel employeeDocumentViewModel = new EmployeeDocumentViewModel();
                    if (EmployeeExpensePhoto.FileType == ".pdf")
                    {
                        if (EmployeeExitAttachmentBytes != null)
                        {
                            employeeDocumentViewModel.OpenPdfFromBytes(EmployeeExitAttachmentBytes, EmployeeExpensePhoto.SavedFileName);
                            employeeDocumentView.DataContext = employeeDocumentViewModel;
                            employeeDocumentView.ShowDialog();
                        }
                        else
                        {
                            FileInfo fileinfo1 = new FileInfo(EmployeeExpensePhoto.SavedFileName);
                            string ext = fileinfo1.Extension;
                            EmployeeExpensePhoto.SavedFileName = EmployeeExpensePhoto.SavedFileName.Replace(ext.ToLower(), ".pdf");
                            CustomMessageBox.Show(string.Format("Could not find file {0}", EmployeeExpensePhoto.SavedFileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                    #region [GEOS2-5073][10.01.2024][rdixit]
                    else
                    { 
                        //[GEOS2-5073][rdixit][12.02.2024]                                
                        ExpenseAttachmentImageViewModel expenseAttachmentImageViewModel = new ExpenseAttachmentImageViewModel();
                        ExpenseAttachmentImageView expenseAttachmentImageView = new ExpenseAttachmentImageView();
                        if (SelectedPhoto != null)
                        {
                            expenseAttachmentImageViewModel.OpenPdfFromBytes(SelectedPhoto, EmployeeExpensePhoto.SavedFileName);
                            expenseAttachmentImageView.DataContext = expenseAttachmentImageViewModel;
                            expenseAttachmentImageView.ShowDialog();
                        }                   
                    }
                    #endregion
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }   
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - ServiceUnexceptedException", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
