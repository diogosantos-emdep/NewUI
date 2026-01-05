using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        #region Service

        #endregion

        #region Declaration
        private bool isInIt = false;
        private MemoryStream pdfDoc;
        private string fileName;
        private bool isPresent;
        private bool isPdf;
        #endregion

        #region Properties
        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
            }
        }

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

        public bool IsPresent
        {
            get
            {
                return isPresent;
            }

            set
            {
                isPresent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPresent"));
            }
        }

       
        public bool IsPdf
        {
            get { return isPdf; }
            set
            {
                isPdf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPdf"));
            }
        }

        private ImageSource healthAndSaftyAttachedImage;
        public ImageSource HealthAndSaftyAttachedImage
        {
            get
            {
                return healthAndSaftyAttachedImage;
            }

            set
            {
                healthAndSaftyAttachedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSaftyAttachedImage"));
            }
        }
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
        #endregion // end public events region

        #region Constructor
        public DocumentViewModel()
        {

        }
        #endregion

        #region Methods
        public void OpenHealthAndSafetyPdf(HealthAndSafetyAttachedDoc SelectedHealthAndSafetyFile, object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                byte[] temp = null;

                if (SelectedHealthAndSafetyFile is HealthAndSafetyAttachedDoc)
                {
                    HealthAndSafetyAttachedDoc healthAndSafetyItemDocument = (HealthAndSafetyAttachedDoc)obj;
                    FileName = healthAndSafetyItemDocument.SavedFileName;
                    temp = healthAndSafetyItemDocument.HealthAndSafetyAttachedDocInBytes;
                }
                if (temp != null)
                {
                    PdfDoc = new MemoryStream(temp);
                    IsPresent = true;
                }
                else
                {
                    CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsPresent = false;
                }

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<DevExpress.Xpo.DB.ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OpenEquipmentAndToolsPdf(EquipmentAndToolsAttachedDoc SelectedEquipmentAndToolsFile, object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                byte[] temp = null;

                if (SelectedEquipmentAndToolsFile is EquipmentAndToolsAttachedDoc)
                {
                    EquipmentAndToolsAttachedDoc equipmentAndToolsItemDocument = (EquipmentAndToolsAttachedDoc)obj;
                    FileName = equipmentAndToolsItemDocument.SavedFileName;
                    temp = equipmentAndToolsItemDocument.EquipmentAndToolsAttachedDocInBytes;
                }
                if (temp != null)
                {
                    PdfDoc = new MemoryStream(temp);
                    IsPresent = true;
                }
                else
                {
                    CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsPresent = false;
                }

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<DevExpress.Xpo.DB.ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.jangra][GEOS2-5579]
        public void OpenEmployeeEquipmentAndToolsPdf(EmployeeEquipmentAndTools SelectedEquipmentAndToolsFile, object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                byte[] temp = null;

                if (SelectedEquipmentAndToolsFile is EmployeeEquipmentAndTools)
                {
                    EmployeeEquipmentAndTools equipmentAndToolsItemDocument = (EmployeeEquipmentAndTools)obj;
                    FileName = equipmentAndToolsItemDocument.SavedFileName;
                    temp = equipmentAndToolsItemDocument.EquipmentAndToolsAttachedDocInBytes;
                }
                if (temp != null)
                {
                    PdfDoc = new MemoryStream(temp);
                    IsPresent = true;
                }
                else
                {
                    CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsPresent = false;
                }

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<DevExpress.Xpo.DB.ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods Please add above refrences if you are using below method:- 1.DevExpress.XtraRichEdit  2.DevExpress.Xpf.RichEdit  3.System.Windows.Media.Imaging;

        //[pramod.misal][19.06.2024]

        //public void OpenHealthAndSafetyPdf(HealthAndSafetyAttachedDoc SelectedHealthAndSafetyFile, object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenPdfByOptionWayDetectionSparePart()...", category: Category.Info, priority: Priority.Low);
        //        isInIt = true;
        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        byte[] temp = null;
        //        string fileExtension = "";
        //        if (SelectedHealthAndSafetyFile is HealthAndSafetyAttachedDoc)
        //        {
        //            HealthAndSafetyAttachedDoc connectorAttachementDoc = (HealthAndSafetyAttachedDoc)obj;
        //            FileName = connectorAttachementDoc.SavedFileName;
        //            temp = connectorAttachementDoc.HealthAndSafetyAttachedDocInBytes;
        //            fileExtension = Path.GetExtension(FileName).ToLower();
        //        }

        //        if (temp != null)
        //        {
        //            if (fileExtension == ".pdf")
        //            {
        //                PdfDoc = new MemoryStream(temp);
        //                IsPresent = true;
        //                IsPdf = true;

        //            }
        //            else if (fileExtension == ".jpg" || fileExtension == ".jpeg")
        //            {
        //                // Handle JPEG file
        //                BitmapImage imageSource = new BitmapImage();
        //                imageSource.BeginInit();
        //                imageSource.StreamSource = new MemoryStream(temp);
        //                imageSource.CacheOption = BitmapCacheOption.OnLoad;
        //                imageSource.EndInit();

        //                HealthAndSaftyAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
        //                IsPdf = false;
        //                IsPresent = true;
        //                //--
        //                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
        //                saveFileDialog.FileName = FileName; // Default file name
        //                saveFileDialog.DefaultExt = ".jpg"; // Default file extension
        //                saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension

        //                // Show save file dialog box
        //                Nullable<bool> result = saveFileDialog.ShowDialog();

        //                // Process save file dialog box results
        //                if (result == true)
        //                {
        //                    // Save document
        //                    string filename = saveFileDialog.FileName;
        //                    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
        //                    {
        //                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //                        encoder.Frames.Add(BitmapFrame.Create(imageSource));
        //                        encoder.Save(fileStream);
        //                    }
        //                }


        //            }
        //            else if (fileExtension == ".tif" || fileExtension == ".tiff")
        //            {
        //                // Handle tif file
        //                // Code for handling tif file...
        //                TiffBitmapDecoder decoder = new TiffBitmapDecoder(new MemoryStream(temp), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        //                BitmapSource bitmapSource = decoder.Frames[0]; // Assuming you want to display the first frame
        //                HealthAndSaftyAttachedImage = bitmapSource;
        //                IsPdf = false;
        //                IsPresent = true;


        //                //--
        //                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
        //                saveFileDialog.FileName = FileName; // Default file name
        //                saveFileDialog.DefaultExt = ".jpg"; // Default file extension
        //                saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension

        //                // Show save file dialog box
        //                Nullable<bool> result = saveFileDialog.ShowDialog();

        //                // Process save file dialog box results
        //                if (result == true)
        //                {
        //                    // Save document
        //                    string filename = saveFileDialog.FileName;
        //                    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
        //                    {
        //                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        //                        encoder.Save(fileStream);
        //                    }
        //                }
        //            }
        //            else if (fileExtension == ".docx")
        //            {
        //                //using (MemoryStream ms = new MemoryStream(temp))
        //                //{
        //                //    RichEditControl richEditControl = new RichEditControl();
        //                //    richEditControl.LoadDocument(ms, DocumentFormat.OpenXml);
        //                //    // Hide the ribbon context menu
        //                //    Window newWindow = new Window();
        //                //    newWindow.Content = richEditControl;
        //                //    newWindow.WindowState = WindowState.Maximized;
        //                //    // Show the window
        //                //    newWindow.ShowDialog();

        //                //}

        //                using (MemoryStream ms = new MemoryStream(temp))
        //                {
        //                    RichEditControl richEditControl = new RichEditControl();
        //                    richEditControl.LoadDocument(ms, DocumentFormat.OpenXml);

        //                    // Add a save button
        //                    Button saveButton = new Button();
        //                    saveButton.Content = "Save";
        //                    saveButton.Margin = new Thickness(5);
        //                    saveButton.Width = 90; // Set width explicitly
        //                    saveButton.Click += (sender, e) =>
        //                    {
        //                        // Prompt the user to choose a location to save the file
        //                        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
        //                        saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
        //                        if (saveFileDialog.ShowDialog() == true)
        //                        {
        //                            // Save the document
        //                            richEditControl.SaveDocument(saveFileDialog.FileName, DocumentFormat.OpenXml);
        //                            MessageBox.Show("File saved successfully!");
        //                        }
        //                    };

        //                    // Create a Grid panel with two rows
        //                    Grid grid = new Grid();
        //                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }); // Auto-sized row
        //                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Centered row

        //                    // Add the RichEditControl to the first row
        //                    Grid.SetRow(richEditControl, 0);
        //                    grid.Children.Add(richEditControl);

        //                    // Create a DockPanel for buttons at the bottom
        //                    DockPanel buttonsPanel = new DockPanel();

        //                    // Add the Save button to the DockPanel
        //                    DockPanel.SetDock(saveButton, Dock.Right);
        //                    buttonsPanel.Children.Add(saveButton);

        //                    // Add the DockPanel to the second row
        //                    Grid.SetRow(buttonsPanel, 1);
        //                    grid.Children.Add(buttonsPanel);

        //                    // Hide the ribbon context menu
        //                    Window newWindow = new Window();
        //                    newWindow.Content = grid;
        //                    newWindow.WindowState = WindowState.Maximized;

        //                    // Show the window
        //                    newWindow.ShowDialog();
        //                }


        //            }
        //            else if (fileExtension == ".png")
        //            {
        //                // Handle PNG file
        //                BitmapImage imageSource = new BitmapImage();
        //                imageSource.BeginInit();
        //                imageSource.StreamSource = new MemoryStream(temp);
        //                imageSource.CacheOption = BitmapCacheOption.OnLoad;
        //                imageSource.EndInit();

        //                HealthAndSaftyAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
        //                IsPdf = false;
        //                IsPresent = true;
        //                //--
        //                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
        //                saveFileDialog.FileName = FileName; // Default file name
        //                saveFileDialog.DefaultExt = ".png"; // Default file extension
        //                saveFileDialog.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*"; // Filter files by extension

        //                // Show save file dialog box
        //                Nullable<bool> result = saveFileDialog.ShowDialog();

        //                // Process save file dialog box results
        //                if (result == true)
        //                {
        //                    // Save document
        //                    string filename = saveFileDialog.FileName;
        //                    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
        //                    {
        //                        PngBitmapEncoder encoder = new PngBitmapEncoder();
        //                        encoder.Frames.Add(BitmapFrame.Create(imageSource));
        //                        encoder.Save(fileStream);
        //                    }
        //                }
        //            }




        //        }
        //        else
        //        {
        //            CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //            IsPresent = false;
        //        }

        //        isInIt = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

        //        GeosApplication.Instance.Logger.Log("Method OpenPdfByOptionWayDetectionSparePart()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<Services.Contracts.ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByOptionWayDetectionSparePart()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenPdfByOptionWayDetectionSparePart() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByOptionWayDetectionSparePart()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //public void OpenEquipmentAndToolsPdf(EquipmentAndToolsAttachedDoc SelectedEquipmentAndToolsFile, object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenPdf()...", category: Category.Info, priority: Priority.Low);
        //        isInIt = true;
        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        byte[] temp = null;

        //        if (SelectedEquipmentAndToolsFile is EquipmentAndToolsAttachedDoc)
        //        {
        //            EquipmentAndToolsAttachedDoc equipmentAndToolsItemDocument = (EquipmentAndToolsAttachedDoc)obj;
        //            FileName = equipmentAndToolsItemDocument.SavedFileName;
        //            temp = equipmentAndToolsItemDocument.EquipmentAndToolsAttachedDocInBytes;
        //        }
        //        if (temp != null)
        //        {
        //            PdfDoc = new MemoryStream(temp);
        //            IsPresent = true;
        //        }
        //        else
        //        {
        //            CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //            IsPresent = false;
        //        }

        //        isInIt = false;
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

        //        GeosApplication.Instance.Logger.Log("Method OpenPdf()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<DevExpress.Xpo.DB.ServiceException> ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenPdf() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdf()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        #endregion
    }
}
