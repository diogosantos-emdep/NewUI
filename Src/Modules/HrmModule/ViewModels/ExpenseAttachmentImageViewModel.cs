using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    //[GEOS2-5073][rdixit][12.02.2024]
    public class ExpenseAttachmentImageViewModel : INotifyPropertyChanged
    {
        #region TaskLog

        /// <summary>
        /// [M049-36][20182210][Pdf viewer always maximized][adadibathina]
        /// </summary>
        /// 
        #endregion

        #region Services 
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration   
        private string fileName;
        private bool isPresent;
        private ImageSource photoSource;
        private byte[] photoBytes = null;
        #endregion

        #region Properties    
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public ImageSource PhotoSource
        {
            get { return photoSource; }
            set
            {

                photoSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhotoSource"));
            }
        }

        #endregion
        public ICommand PrintButtonCommand { get; set; }

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
        #endregion // end public events region
        public ExpenseAttachmentImageViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor ExpenseAttachmentImageViewModel()...", category: Category.Info, priority: Priority.Low);

            PrintButtonCommand = new RelayCommand(new Action<object>(PrintAction));
        }
        private void PrintAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEmployeeList()...", category: Category.Info, priority: Priority.Low);
                var imageSource = ((ImageEdit)obj).Source as BitmapSource;
                if (imageSource == null)
                {
                    MessageBox.Show("No image found to print.");
                    return;
                }

                #region [rdixit][GEOS2-5816][01.10.2024]
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    double printableWidth = printDialog.PrintableAreaWidth;
                    double printableHeight = printDialog.PrintableAreaHeight;

                    double scaleX = printableWidth / imageSource.PixelWidth;
                    double scaleY = printableHeight / imageSource.PixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    double scaledWidth = imageSource.PixelWidth * scale;
                    double scaledHeight = imageSource.PixelHeight * scale;
                    DrawingVisual visual = new DrawingVisual();
                    using (DrawingContext drawingContext = visual.RenderOpen())
                    {
                        drawingContext.DrawImage(imageSource, new Rect((printableWidth - scaledWidth) / 2, (printableHeight - scaledHeight) / 2, scaledWidth, scaledHeight));
                    }
                    printDialog.PrintVisual(visual, "Printed Image");
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method PrintEmployeeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEmployeeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OpenPdfFromBytes(ImageSource PhotoBytes, string fileName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfFromBytes()...", category: Category.Info, priority: Priority.Low);
                PhotoSource = PhotoBytes;
                FileName = fileName;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfFromBytes()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfFromBytes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdfFromBytes() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfFromBytes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public ImageSource ByteArrayToPhotoSource(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert ByteArrayToPhotoSource ...", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToPhotoSource() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
    }

}


