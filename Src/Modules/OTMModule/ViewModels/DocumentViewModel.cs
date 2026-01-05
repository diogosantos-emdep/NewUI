using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.UI.CustomControls;
using DevExpress.XtraRichEdit;
using Emdep.Geos.Data.Common.OTM;
using DevExpress.XtraPrinting;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.Charts.Native;
using Syncfusion.Presentation;
//using Emdep.Geos.Data.Common.Hrm;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
     class DocumentViewModel : INotifyPropertyChanged
     {

        #region Declaration

        private bool isInIt = false;
        private MemoryStream pdfDoc;
        private string fileName;
        private bool isPresent;
        private ImageSource connectorAttachedImage;
        private MemoryStream employeeEducationalPdfDoc;
        private bool isWord;//[pramod.misal][10.04.2025][GEOS2-7848]
        private bool isPng;//[pramod.misal][10.04.2025][GEOS2-7848]
        private bool isExcel; //[pramod.misal][10.04.2025][GEOS2-7848]
        private ImageSource pngDoc;//[pramod.misal][10.04.2025][GEOS2-7848]
        #endregion
        #region Properties    
        public MemoryStream EmployeeEducationalPdfDoc
        {
            get { return employeeEducationalPdfDoc; }
            set
            {
                employeeEducationalPdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEducationalPdfDoc"));
            }
        }
        private MemoryStream excelDoc;

        public MemoryStream ExcelDoc
        {
            get { return excelDoc; }
            set
            {
                excelDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcelDoc"));
            }
        }

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
        public ImageSource ConnectorAttachedImage
        {
            get
            {
                return connectorAttachedImage;
            }

            set
            {
                connectorAttachedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorAttachedImage"));
            }
        }
        private bool isPdf;
        public bool IsPdf
        {
            get { return isPdf; }
            set
            {
                isPdf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPdf"));
            }
        }


        private bool _isPptFile;
        public bool IsPptFile
        {
            get => _isPptFile;
            set
            {
                _isPptFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPptFile"));
                
            }
        }

        private List<BitmapImage> _pptFile;
        public List<BitmapImage> PptFile
        {
            get => _pptFile;
            set
            {
                _pptFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PptFile"));
            }
        }
        //
        public bool IsMp4 { get; set; }
        private MemoryStream _mp4Doc;
        public MemoryStream Mp4Doc
        {
            get => _mp4Doc;
            set
            {
                _mp4Doc = value;
               
                OnPropertyChanged(new PropertyChangedEventArgs("Mp4Doc"));
                SaveToTempFileAndSetPath(); // automatically trigger path
            }
        }

        private Uri _mp4Path;

        public Uri Mp4Path
        {
            get { return _mp4Path; }
            set
            {
                _mp4Path = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Mp4Path"));
            }
        }

        //[pramod.misal][10.04.2025][GEOS2-7848]
        public bool IsPng
        {
            get { return isPng; }
            set
            {
                isPng = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPng"));
            }
        }

        
        
        public bool IsExcel
        {
            get { return isExcel; }
            set
            {
                isExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExcel"));
            }
        }
        

        //[pramod.misal][10.04.2025][GEOS2-7848]
        public ImageSource PngDoc
        {
            get { return pngDoc; }
            set
            {

                pngDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PngDoc"));
            }
        }

        private MemoryStream wordDoc;

        public MemoryStream WordDoc
        {
            get { return wordDoc; }
            set
            {
                wordDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WordDoc"));
            }
        }

       
        //[pramod.misal][10.04.2025][GEOS2-7848]
        public bool IsWord
        {
            get { return isWord; }
            set
            {
                isWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWord"));
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
        #endregion

        #region Comstructor
        public DocumentViewModel()
        {

        }
        #endregion

        #region Methods

        public void OpenPdfByRegisterPoAttachment(RegisterPoAttachments SelectedConnectorAttachementsFile, object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfByRegisterPoAttachment()...", category: Category.Info, priority: Priority.Low);
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
                string fileExtension = "";
                if (SelectedConnectorAttachementsFile.SavedFileName != null && SelectedConnectorAttachementsFile.ConnectorAttachementsDocInBytes != null)
                {
                    if (SelectedConnectorAttachementsFile is RegisterPoAttachments)
                    {
                        //  RegisterPoAttachments connectorAttachementDoc = (RegisterPoAttachments)obj;
                        FileName = SelectedConnectorAttachementsFile.SavedFileName;
                        temp = SelectedConnectorAttachementsFile.ConnectorAttachementsDocInBytes;
                        fileExtension = Path.GetExtension(FileName).ToLower();
                    }
                }
                

                if (temp != null)
                {
                    if (fileExtension == ".pdf")
                    {
                        PdfDoc = new MemoryStream(temp);
                        IsPresent = true;
                        IsPdf = true;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                    }
                    else if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                    {
                        // Handle JPEG file
                        BitmapImage imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = new MemoryStream(temp);
                        imageSource.CacheOption = BitmapCacheOption.OnLoad;
                        imageSource.EndInit();

                        ConnectorAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
                        IsPdf = false;
                        IsPresent = true;
                        //--
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog.FileName = FileName; // Default file name
                        saveFileDialog.DefaultExt = ".jpg"; // Default file extension
                        saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        // Show save file dialog box
                        Nullable<bool> result = saveFileDialog.ShowDialog();

                        // Process save file dialog box results
                        if (result == true)
                        {
                            // Save document
                            string filename = saveFileDialog.FileName;
                            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                            {
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                                encoder.Save(fileStream);
                            }
                        }


                    }
                    else if (fileExtension == ".tif" || fileExtension == ".tiff")
                    {
                        // Handle tif file
                        // Code for handling tif file...
                        TiffBitmapDecoder decoder = new TiffBitmapDecoder(new MemoryStream(temp), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        BitmapSource bitmapSource = decoder.Frames[0]; // Assuming you want to display the first frame
                        ConnectorAttachedImage = bitmapSource;
                        IsPdf = false;
                        IsPresent = true;


                        //--
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog.FileName = FileName; // Default file name
                        saveFileDialog.DefaultExt = ".jpg"; // Default file extension
                        saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        // Show save file dialog box
                        Nullable<bool> result = saveFileDialog.ShowDialog();

                        // Process save file dialog box results
                        if (result == true)
                        {
                            // Save document
                            string filename = saveFileDialog.FileName;
                            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                            {
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                                encoder.Save(fileStream);
                            }
                        }
                    }
                    else if (fileExtension == ".docx")
                    {
                        
                        using (MemoryStream ms = new MemoryStream(temp))
                        {
                            RichEditControl richEditControl = new RichEditControl();
                            richEditControl.LoadDocument(ms, DocumentFormat.OpenXml);

                            // Add a save button
                            Button saveButton = new Button();
                            saveButton.Content = "Save";
                            saveButton.Margin = new Thickness(5);
                            saveButton.Width = 90; // Set width explicitly
                            saveButton.Click += (sender, e) =>
                            {
                                // Prompt the user to choose a location to save the file
                                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                                saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    // Save the document
                                    richEditControl.SaveDocument(saveFileDialog.FileName, DocumentFormat.OpenXml);
                                    MessageBox.Show("File saved successfully!");
                                }
                            };

                            // Create a Grid panel with two rows
                            Grid grid = new Grid();
                            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }); // Auto-sized row
                            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Centered row

                            // Add the RichEditControl to the first row
                            Grid.SetRow(richEditControl, 0);
                            grid.Children.Add(richEditControl);

                            // Create a DockPanel for buttons at the bottom
                            DockPanel buttonsPanel = new DockPanel();

                            // Add the Save button to the DockPanel
                            DockPanel.SetDock(saveButton, Dock.Right);
                            buttonsPanel.Children.Add(saveButton);

                            // Add the DockPanel to the second row
                            Grid.SetRow(buttonsPanel, 1);
                            grid.Children.Add(buttonsPanel);

                            // Hide the ribbon context menu
                            Window newWindow = new Window();
                            newWindow.Content = grid;
                            newWindow.WindowState = WindowState.Maximized;

                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            // Show the window
                            newWindow.ShowDialog();

                        }


                    }
                    else if (fileExtension == ".png")
                    {
                        // Handle PNG file
                        BitmapImage imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = new MemoryStream(temp);
                        imageSource.CacheOption = BitmapCacheOption.OnLoad;
                        imageSource.EndInit();

                        ConnectorAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
                        IsPdf = false;
                        IsPresent = true;
                        //--
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog.FileName = FileName; // Default file name
                        saveFileDialog.DefaultExt = ".png"; // Default file extension
                        saveFileDialog.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*"; // Filter files by extension
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        // Show save file dialog box
                        Nullable<bool> result = saveFileDialog.ShowDialog();

                        // Process save file dialog box results
                        if (result == true)
                        {
                            // Save document
                            string filename = saveFileDialog.FileName;
                            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                            {
                                PngBitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                                encoder.Save(fileStream);
                            }
                        }
                    }

                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    IsPresent = false;
                }

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfByRegisterPoAttachment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByRegisterPoAttachment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdfByRegisterPoAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByRegisterPoAttachment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rahul.gadhave][GEOS2-6829][03.01.2025]
        public void OpenPdfByTemplateCode(OtRequestTemplates Selectedfile, object empRecord)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfByTemplateCode()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window
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
                    }, x => new SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
                }
                byte[] temp = null;
               // byte[] fileBytes = null;
                string fileExtension = "";

                if (Selectedfile?.File != null && Selectedfile.FileDocInBytes != null)
                {
                    //fileBytes = Selectedfile.FileDocInBytes;
                    FileName = Selectedfile.File;
                    temp = Selectedfile.FileDocInBytes;
                    fileExtension = Path.GetExtension(Selectedfile.File).ToLower();
                }
                if (temp!=null)
                {
                    if (fileExtension == ".pdf")
                    {
                        PdfDoc = new MemoryStream(temp);
                        IsPresent = true;
                        IsPdf = true;
                    }
                    else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        ExcelDoc = new MemoryStream(temp);
                        IsPresent = true;
                        IsPdf = false;
                    }

                }
                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenPdfByTemplateCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByTemplateCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPdfByTemplateCode() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByTemplateCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [pooja.jadhav][17.02.2025][GEOS2-6724]
        /// </summary>//[pramod.misal][GEOS2-8777][4 - 07 - 2025]https://helpdesk.emdep.com/browse/GEOS2-8777
        /// <param name="Attachment"></param>
        public void OpenPORequestAttachment(Emailattachment Attachment)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPORequestAttachment()...", category: Category.Info, priority: Priority.Low);
                byte[] temp = null;              
                string fileExtension = "";
                FileName = Attachment.AttachmentName;

                if ( Attachment.FileDocInBytes != null)
                {
                    temp = Attachment.FileDocInBytes;
                    fileExtension = Path.GetExtension(Attachment.AttachmentPath).ToLower();
                }

                if (temp != null)
                {
                    //[pramod.misal][GEOS2-8777][4 - 07 - 2025]https://helpdesk.emdep.com/browse/GEOS2-8777
                    string[] downloadExtensions = new[]{".cpt", ".mp4",".ppt", ".rar", ".zip", ".ods", ".msg", ".7z", ".wmz", ".stp", ".emz", ".txt", ".stl", ".dwg",""};

                    if (fileExtension == ".pdf")
                    {
                        PdfDoc = new MemoryStream(temp);
                        IsPresent = true;
                        IsPdf = true;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsExcel = false;
                    }
                    //else if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsm")
                    //{
                    //    ExcelDoc = new MemoryStream(temp);
                    //    IsPresent = true;
                    //    //IsPdf = false;
                    //    IsExcel = true;
                    //}

                    else if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".xlsm" || fileExtension == ".xlsb" )
                    {
                        #region Old code
                        //using (MemoryStream ms = new MemoryStream(temp))
                        //{
                        //    SpreadsheetControl spreadsheetControl = new SpreadsheetControl();
                        //    spreadsheetControl.LoadDocument(ms, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                        //    // Add a save button
                        //    Button saveButton = new Button();
                        //    saveButton.Content = "Save";
                        //    saveButton.Margin = new Thickness(5);
                        //    saveButton.Width = 90;
                        //    saveButton.Click += (sender, e) =>
                        //    {
                        //        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        //        saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel Macro-Enabled Workbook (*.xlsm)|*.xlsm|Excel 97-2003 Workbook (*.xls)|*.xls";
                        //        if (saveFileDialog.ShowDialog() == true)
                        //        {
                        //            spreadsheetControl.SaveDocument(saveFileDialog.FileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FileSaveSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //        }
                        //    };

                        //    // Create a Grid panel with two rows
                        //    Grid grid = new Grid();
                        //    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        //    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                        //    // Add the SpreadsheetControl to the first row
                        //    Grid.SetRow(spreadsheetControl, 0);
                        //    grid.Children.Add(spreadsheetControl);

                        //    // Create a DockPanel for buttons at the bottom
                        //    DockPanel buttonsPanel = new DockPanel();
                        //    DockPanel.SetDock(saveButton, Dock.Right);
                        //    buttonsPanel.Children.Add(saveButton);

                        //    // Add the DockPanel to the second row
                        //    Grid.SetRow(buttonsPanel, 1);
                        //    grid.Children.Add(buttonsPanel);

                        //    // Hide the ribbon context menu
                        //    Window newWindow = new Window();
                        //    newWindow.Content = grid;
                        //    newWindow.WindowState = WindowState.Maximized;

                        //    // Show the window
                        //    newWindow.ShowDialog();
                        //}

                        #endregion

                        //[pramod.misal][10.04.2025][GEOS2-7848]
                        temp = Attachment.FileDocInBytes;
                        ExcelDoc = new MemoryStream(temp);
                        IsPresent = true;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsExcel = true;
                        IsPdf = false;
                    }

                    #region old code
                    //else if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                    //{
                    //    // Handle JPEG file
                    //    BitmapImage imageSource = new BitmapImage();
                    //    imageSource.BeginInit();
                    //    imageSource.StreamSource = new MemoryStream(temp);
                    //    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    //    imageSource.EndInit();

                    //    ConnectorAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
                    //    IsPdf = false;
                    //    IsPresent = true;
                    //    //--
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName; // Default file name
                    //    saveFileDialog.DefaultExt = ".jpg"; // Default file extension
                    //    saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension

                    //    // Show save file dialog box
                    //    Nullable<bool> result = saveFileDialog.ShowDialog();

                    //    // Process save file dialog box results
                    //    if (result == true)
                    //    {
                    //        // Save document
                    //        string filename = saveFileDialog.FileName;
                    //        using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                    //        {
                    //            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    //            encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    //            encoder.Save(fileStream);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        IsPresent = false;
                    //    }

                    //}

                    #endregion

                    #region Old code for tif
                    //else if (fileExtension == ".tifuu" || fileExtension == ".tiffuu")
                    //{

                    //    // Handle tif file
                    //    // Code for handling tif file...
                    //    TiffBitmapDecoder decoder = new TiffBitmapDecoder(new MemoryStream(temp), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    //    BitmapSource bitmapSource = decoder.Frames[0]; // Assuming you want to display the first frame
                    //    ConnectorAttachedImage = bitmapSource;
                    //    IsPdf = false;
                    //    IsPresent = true;


                    //    //--
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName; // Default file name
                    //    saveFileDialog.DefaultExt = ".jpg"; // Default file extension
                    //    saveFileDialog.Filter = "JPEG Files (*.jpg)|*.jpg|All files (*.*)|*.*"; // Filter files by extension

                    //    // Show save file dialog box
                    //    Nullable<bool> result = saveFileDialog.ShowDialog();

                    //    // Process save file dialog box results
                    //    if (result == true)
                    //    {
                    //        // Save document
                    //        string filename = saveFileDialog.FileName;
                    //        using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                    //        {
                    //            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    //            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    //            encoder.Save(fileStream);
                    //        }
                    //    }
                    //}

                    #endregion

                    else if (fileExtension == ".docx" || fileExtension == ".doc" || fileExtension == ".html" || fileExtension == ".htm")
                    {
                       //[pramod.misal][10.04.2025][GEOS2-7848]
                        temp = Attachment.FileDocInBytes;
                        WordDoc = new MemoryStream(temp);
                        IsPresent = true;
                        IsExcel = false;
                        IsPdf = false;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsWord = true;

                        #region Old code
                        //using (MemoryStream ms = new MemoryStream(temp))
                        //{
                        //    RichEditControl richEditControl = new RichEditControl();
                        //    richEditControl.LoadDocument(ms, DocumentFormat.OpenXml);

                        //    // Add a save button
                        //    Button saveButton = new Button();
                        //    saveButton.Content = "Save";
                        //    saveButton.Margin = new Thickness(5);
                        //    saveButton.Width = 90; // Set width explicitly
                        //    saveButton.Click += (sender, e) =>
                        //    {
                        //        // Prompt the user to choose a location to save the file
                        //        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        //        saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
                        //        if (saveFileDialog.ShowDialog() == true)
                        //        {
                        //            // Save the document
                        //            richEditControl.SaveDocument(saveFileDialog.FileName, DocumentFormat.OpenXml);
                        //            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FileSaveSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //            //MessageBox.Show("File saved successfully!");
                        //        }
                        //    };

                        //    // Create a Grid panel with two rows
                        //    Grid grid = new Grid();
                        //    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }); // Auto-sized row
                        //    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // Centered row

                        //    // Add the RichEditControl to the first row
                        //    Grid.SetRow(richEditControl, 0);
                        //    grid.Children.Add(richEditControl);

                        //    // Create a DockPanel for buttons at the bottom
                        //    DockPanel buttonsPanel = new DockPanel();

                        //    // Add the Save button to the DockPanel
                        //    DockPanel.SetDock(saveButton, Dock.Right);
                        //    buttonsPanel.Children.Add(saveButton);

                        //    // Add the DockPanel to the second row
                        //    Grid.SetRow(buttonsPanel, 1);
                        //    grid.Children.Add(buttonsPanel);

                        //    // Hide the ribbon context menu
                        //    Window newWindow = new Window();
                        //    newWindow.Content = grid;
                        //    newWindow.WindowState = WindowState.Maximized;

                        //    // Show the window
                        //    newWindow.ShowDialog();
                        //}

                        #endregion
                    }
                    else if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".tif" || fileExtension == ".tiff" || fileExtension == ".bmp")
                    {
                        //[pramod.misal][10.04.2025][GEOS2-7848]
                        temp = Attachment.FileDocInBytes;
                        PngDoc = ByteArrayToPhotoSource(temp);
                        IsPresent = true;
                        IsExcel = false;
                        IsPdf = false;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsPng = true;

                        #region Old code
                        // Handle PNG file
                        //BitmapImage imageSource = new BitmapImage();
                        //imageSource.BeginInit();
                        //imageSource.StreamSource = new MemoryStream(temp);
                        //imageSource.CacheOption = BitmapCacheOption.OnLoad;
                        //imageSource.EndInit();

                        //ConnectorAttachedImage = imageSource; // Set ImageSource property to bind to an Image control in your UI
                        //IsPdf = false;
                        //IsPresent = true;
                        ////--
                        //var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        //saveFileDialog.FileName = FileName; // Default file name
                        //saveFileDialog.DefaultExt = ".png"; // Default file extension
                        //saveFileDialog.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*"; // Filter files by extension

                        //// Show save file dialog box
                        //Nullable<bool> result = saveFileDialog.ShowDialog();

                        //// Process save file dialog box results
                        //if (result == true)
                        //{
                        //    // Save document
                        //    string filename = saveFileDialog.FileName;
                        //    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                        //    {
                        //        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        //        encoder.Frames.Add(BitmapFrame.Create(imageSource));
                        //        encoder.Save(fileStream);
                        //    }
                        //}

                        #endregion
                    }
                    else if (fileExtension == ".xml")
                    {

                        //System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                        //myProcess.StartInfo.FileName = Attachment.AttachmentPath;
                        //myProcess.StartInfo.UseShellExecute = true;
                        //myProcess.StartInfo.RedirectStandardOutput = false;
                        //myProcess.Start();

                        // Create a temporary XML file path
                        string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");

                        // Write the byte array to the temporary XML file
                        File.WriteAllBytes(tempFilePath, Attachment.FileDocInBytes);

                        // Convert file path to URL format for browser compatibility
                        string fileUrl = "file:///" + tempFilePath.Replace("\\", "/");
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        // Open the XML file in the default web browser
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = fileUrl,
                             
                            UseShellExecute = true // Ensures the file opens in the default web browser
                        });
                    }

                    //[pramod.misal][GEOS2-8777][01-07-2025]
                    #region sepate code for each file
                    //else if (fileExtension == ".zip")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName; // Default file name
                    //    saveFileDialog.DefaultExt = ".zip"; // Correct default extension
                    //    saveFileDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}


                    //else if (fileExtension == ".rar")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".rar";
                    //    saveFileDialog.Filter = "RAR Archive (*.rar)|*.rar|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}


                    //else if (fileExtension == ".txt")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".txt";
                    //    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}


                    //else if (fileExtension == ".stl" || string.IsNullOrEmpty(fileExtension))
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".stl";
                    //    saveFileDialog.Filter = "STL files (*.stl)|*.stl|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}

                    //else if (fileExtension == ".mp4")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".mp4";
                    //    saveFileDialog.Filter = "MP4 Video (*.mp4)|*.mp4|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}


                    //else if (fileExtension == ".cpt")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".cpt";
                    //    saveFileDialog.Filter = "Corel Photo-Paint (*.cpt)|*.cpt|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}

                    //else if (fileExtension == ".ods")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".ods";
                    //    saveFileDialog.Filter = "OpenDocument Spreadsheet (*.ods)|*.ods|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}

                    //else if (fileExtension == ".msg")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".msg";
                    //    saveFileDialog.Filter = "Outlook Message (*.msg)|*.msg|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}
                    //else if (fileExtension == ".xlsb")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".xlsb";
                    //    saveFileDialog.Filter = "Excel Binary Workbook (*.xlsb)|*.xlsb|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}

                    //else if (fileExtension == ".7z")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".7z";
                    //    saveFileDialog.Filter = "7-Zip Archive (*.7z)|*.7z|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}
                    //else if (fileExtension == ".emz")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".emz";
                    //    saveFileDialog.Filter = "Enhanced Metafile Zip (*.emz)|*.emz|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}

                    //else if (fileExtension == ".bmp")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".bmp";
                    //    saveFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}
                    //else if (fileExtension == ".dwg")
                    //{
                    //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                    //    saveFileDialog.FileName = FileName;
                    //    saveFileDialog.DefaultExt = ".dwg";
                    //    saveFileDialog.Filter = "AutoCAD Drawing (*.dwg)|*.dwg|All files (*.*)|*.*";

                    //    if (saveFileDialog.ShowDialog() == true)
                    //    {
                    //        SaveToFile(saveFileDialog.FileName, temp);
                    //    }
                    //}   

                    #endregion


                    #region //[pramod.misal][GEOS2-8777][4 - 07 - 2025]https://helpdesk.emdep.com/browse/GEOS2-8777

                    else if (fileExtension == ".pptx")
                    {
                        byte[] pptBytes = Attachment.FileDocInBytes;
                        var slideImages = ConvertSlidesToImages(pptBytes); 

                        PptFile = slideImages;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        IsPptFile = true;
                        IsPresent = true;

                    } 
                    //[pramod.misal][GEOS2-8777][03.07.2025]
                    if (downloadExtensions.Contains(fileExtension.ToLower()))
                    {
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog.FileName = FileName;
                        saveFileDialog.DefaultExt = fileExtension;
                        string extensionDescription = fileExtension.TrimStart('.').ToUpper();
                        saveFileDialog.Filter = $"{extensionDescription} files (*{fileExtension})|*{fileExtension}|All files (*.*)|*.*";

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            SaveToFile(saveFileDialog.FileName, temp);
                        }
                    }

                    #endregion


                }

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenPORequestAttachment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPORequestAttachment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenPORequestAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPORequestAttachment()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [pramod.misal][GEOS2-8777][4-07-2025]https://helpdesk.emdep.com/browse/GEOS2-8777
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        private void SaveToFile(string filePath, byte[] data)
        {
            try
            {
                File.WriteAllBytes(filePath, data);
                string fileNameOnly = System.IO.Path.GetFileName(filePath);
                CustomMessageBox.Show($"{fileNameOnly} saved successfully.", Application.Current.Resources["PopUpSuccessColor"].ToString(),CustomMessageBox.MessageImagePath.Ok,MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                string fileNameOnly = System.IO.Path.GetFileName(filePath);
                GeosApplication.Instance.Logger.Log($"Error saving {fileNameOnly}: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show($"Failed to save {fileNameOnly}.", Application.Current.Resources["PopUpWarningColor"].ToString(),CustomMessageBox.MessageImagePath.NotOk,MessageBoxButton.OK);
            }
        }



        private List<BitmapImage> ConvertSlidesToImages(byte[] pptBytes)
        {
            List<BitmapImage> slideImages = new List<BitmapImage>();

            using (MemoryStream ms = new MemoryStream(pptBytes))
            using (Syncfusion.Presentation.IPresentation presentation = Presentation.Open(ms))
            {
                foreach (ISlide slide in presentation.Slides)
                {
                    using (MemoryStream imgStream = new MemoryStream())
                    {
                        var image = slide.ConvertToImage(Syncfusion.Drawing.ImageType.Metafile);
                        image.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);

                        BitmapImage bmpImage = new BitmapImage();
                        bmpImage.BeginInit();
                        bmpImage.StreamSource = new MemoryStream(imgStream.ToArray());
                        bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                        bmpImage.EndInit();

                        slideImages.Add(bmpImage);
                    }
                }
            }

            return slideImages;
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

        // Save byte[] as file and create Uri for MediaElement
        private void SaveToTempFileAndSetPath()
        {
            if (Mp4Doc == null) return;

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mp4");
            File.WriteAllBytes(tempFile, Mp4Doc.ToArray());
            Mp4Path = new Uri(tempFile);
        }


        #endregion
    }
}
