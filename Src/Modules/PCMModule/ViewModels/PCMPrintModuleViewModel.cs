using System;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.Services.Contracts;
using Prism.Logging;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Data.Common;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Grid;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.Spreadsheet;
using System.Drawing.Printing;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.PCM.Reports;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.PCM.Views;
using System.Drawing.Imaging;
using System.IO;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Pdf;
using System.Net;
using System.Linq;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class PCMPrintModuleViewModel : ViewModelBase  , INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion  // Services
        #region Events

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose; // for close window

        #endregion  // Events

        #region Declaration
        string language;
        private ProductTypes productTypesDetails;
        private int settingWindowLanguageSelectedIndex;
        private List<Language> languages;
        List<CPLCustomer> includedCustomersList;
        private List<DetectiontypesInformations> detectionTypes;
        #endregion

        #region Properties
        public int SettingWindowLanguageSelectedIndex
        {
            get { return settingWindowLanguageSelectedIndex; }
            set
            {
                settingWindowLanguageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SettingWindowLanguageSelectedIndex"));
            }
        }
        public List<CPLCustomer> IncludedCustomersList
        {
            get
            {
                return includedCustomersList;
            }

            set
            {
                includedCustomersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncludedCustomersList"));
            }
        }
        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public bool IsLanguagteChange { get; set; }
        public bool IsPCMPrintModuleAttachments { get; set; }
        public bool IsPCMPrintModuleLinks { get; set; }
        public ProductTypes ProductTypesDetails
        {
            get
            {
                return productTypesDetails;
            }

            set
            {
                productTypesDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesDetails"));
            }
        }
        public List<DetectiontypesInformations> DetectionTypes
        {
            get
            {
                return detectionTypes;
            }

            set
            {
                detectionTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionTypes"));
            }
        }
        #endregion

        #region ICommand
        public ICommand PCMPrintModuleViewCancelButtonCommand { get; set; }
        public ICommand PCMPrintModuleViewAcceptButtonCommand { get; set; }

        #endregion

        #region Constructor
        public PCMPrintModuleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PCMPrintModuleViewModel()...", category: Category.Info, priority: Priority.Low);
                FillLanguage();
                IsPCMPrintModuleAttachments = true;
                IsPCMPrintModuleLinks = true;
                PCMPrintModuleViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PCMPrintModuleViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandWindow));
                // SetLanguageDictionary();
                DetectionTypes = new List<DetectiontypesInformations>();
                //PCMService = new PCMServiceController("localhost:6699");
                DetectionTypes = PCMService.GetDetectionTypes();
                GeosApplication.Instance.Logger.Log("Constructor Constructor PCMPrintModuleViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor PCMPrintModuleViewModel() Method - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in  Constructor PCMPrintModuleViewModel() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PCMPrintModuleViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        // Shubham[skadam] GEOS2-2596 Add option in PCM to print a datasheet of a Module [1 of 3] 06 01 2023
        private void AcceptButtonCommandWindow(object obj)
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                //string FileName = "PCMModuleReport";
                string FileName = ProductTypesDetails.Reference+"_"+ ProductTypesDetails.Name;
                //string PCMModuleReportPageFooter="/Emdep.Geos.Modules.PCM;component/Assets/Images/PCMModuleReportPageFooter.png";
                // byte[] imgData = System.IO.File.ReadAllBytes(PCMModuleReportPageFooter);
                //PCMPrintModuleReport pcmPrintModuleReport = new PCMPrintModuleReport();
                Language Language = Languages[settingWindowLanguageSelectedIndex];
                PCMPrintModuleReportNew pcmPrintModuleReport = new PCMPrintModuleReportNew();
                pcmPrintModuleReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleReport.xrModuleDatasheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                #region ModuleDatasheet
                //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                switch (Language.TwoLetterISOLanguage.ToLower())
                {
                    case "en":
                        #region en
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES"].ToString();
                        #endregion
                        break;
                    case "es":
                        #region es
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_es"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_es"].ToString();
                        #endregion
                        break;

                    case "fr":
                        #region es
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_fr"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_fr"].ToString();
                        #endregion
                        break;

                    case "pt":
                        #region pt
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_pt"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_pt"].ToString();
                        #endregion
                        break;
                    case "ro":
                        #region ro
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_ro"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_ro"].ToString();
                        #endregion
                        break;
                    case "ru":
                        #region ru
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_ru"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_ru"].ToString();
                        pcmPrintModuleReport.xrModuleDatasheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                        #endregion
                        break;
                    case "zh":
                        #region zh
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet_zh"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES_zh"].ToString();
                        #endregion
                        break;
                    default:
                        #region default
                        pcmPrintModuleReport.xrModuleDatasheet.Text = Application.Current.Resources["PCMPrintModuleReportModuleDatasheet"].ToString();
                        pcmPrintModuleReport.xrLabel13.Text = Application.Current.Resources["PCMPrintModuleReportSUPPORTEDFEATURES"].ToString();
                        #endregion
                        break;
                }
                #endregion

                //pcmPrintModuleReport.xrtxtName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //pcmPrintModuleReport.xrtxtDescription.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrLabel1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrLabel2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //pcmPrintModuleReport.xrLabel3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrLabel4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
               // pcmPrintModuleReport.xrLabel5.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
               //  pcmPrintModuleReport.xrLabel6.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
               // pcmPrintModuleReport.xrLabel7.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrLabel4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Bitmap BitmapimgLogo =new Bitmap(Emdep.Geos.Modules.PCM.Properties.Resources.Emdep_logo_mini);
                pcmPrintModuleReport.imgLogo.Image = BitmapimgLogo;
                pcmPrintModuleReport.imgLogo.Height = BitmapimgLogo.Height;
                pcmPrintModuleReport.imgLogo.WidthF = BitmapimgLogo.Width;

                pcmPrintModuleReport.xrLblEmdep.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleReport.xrLblEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrlblPhone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrlblWebSite.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                #region ProductTypesDetails
                try
                {
                    // [settingWindowLanguageSelectedIndex];
                    //pcmPrintModuleReport.xrtxtName.Text = ProductTypesDetails.Name;
                    //pcmPrintModuleReport.xrDescription.Text = ProductTypesDetails.Description;
                    #region MyRegion
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_es"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_pt"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_zh"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ro"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_fr"].ToString();
                    //pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ru"].ToString();

                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_es"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_pt"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_zh"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ro"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_fr"].ToString();
                    //pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ru"].ToString();

                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_es"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_pt"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_zh"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_ro"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_fr"].ToString();
                    //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_ru"].ToString();

                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_es"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_pt"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_zh"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ro"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_fr"].ToString();
                    //pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ru"].ToString();
                    #endregion
                    #region switch
                    //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail"].ToString();
                            #region OldCode
                            //pcmPrintModuleReport.xrLabel3.Text = ProductTypesDetails.Template.Name;                    
                            //if (ProductTypesDetails.ModifiedIn == null)
                            //{
                            //    pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.CreatedIn.Value.ToString("dd/MM/yyyy");
                            //}
                            //else
                            //{
                            //    pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.ModifiedIn.Value.ToString("dd/MM/yyyy");
                            //}
                            #endregion
                            #endregion
                            break;
                        case "es":
                            #region es
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_es;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_es;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_es"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_es"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_es"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_es"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_es"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_es"].ToString();
                            #endregion
                            break;

                        case "fr":
                            #region es
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_fr;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_fr;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_fr"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_fr"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_fr"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_fr"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_fr"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_fr"].ToString();
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_pt;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_pt;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_pt"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_pt"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_pt"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_pt"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_pt"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_pt"].ToString();
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_ro;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_ro;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ro"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ro"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_ro"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ro"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_ro"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_ro"].ToString();
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_ru;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_ru;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ru"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ru"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_ru"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ru"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_ru"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_ru"].ToString();
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_zh;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_zh;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_zh"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_zh"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily_zh"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_zh"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_zh"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_zh"].ToString();
                            #endregion
                            break;
                        default:
                            #region default
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description;
                            pcmPrintModuleReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName"].ToString();
                            pcmPrintModuleReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription"].ToString();
                            //pcmPrintModuleReport.xrLblTemplate.Text = Application.Current.Resources["PCMPrintModuleReportFamily"].ToString();
                            pcmPrintModuleReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate"].ToString();
                            pcmPrintModuleReport.xrLabel11.Text = Application.Current.Resources["PCMPrintDetectionReportPhone"].ToString();
                            pcmPrintModuleReport.xrLabel12.Text = Application.Current.Resources["PCMPrintDetectionReportEmail"].ToString();
                            #endregion
                            break;
                    }
                    #endregion
                    //pcmPrintModuleReport.xrLabel3.Text = ProductTypesDetails.Template.Name;
                    if (ProductTypesDetails.LastUpdate == null)
                    {
                        pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.CreatedIn.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.LastUpdate.Value.ToString("dd/MM/yyyy");
                    }

                }
                catch (Exception ex)
                {
                }

                #endregion

                #region ProductTypeAttachedLinkList
                if (IsPCMPrintModuleLinks)
                {
                    if (ProductTypesDetails.ProductTypeAttachedLinkList != null && ProductTypesDetails.ProductTypeAttachedLinkList.Count > 0)
                    {
                        PCMModuleReferencesReport PCMModuleReferencesReport = new PCMModuleReferencesReport();
                        #region LINKS
                        //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_es"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_pt"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_zh"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_ro"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_fr"].ToString();
                        //PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_ru"].ToString();

                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_es"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_pt"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_zh"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_ro"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_fr"].ToString();
                        //PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_ru"].ToString();
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL"].ToString();
                                #endregion
                                break;
                            case "es":
                                #region es
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_es"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_es"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_es"].ToString();
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_fr"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_fr"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_fr"].ToString();
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_pt"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_pt"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_pt"].ToString();
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_ro"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_ro"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_ro"].ToString();
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_ru"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_ru"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_ru"].ToString();
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS_zh"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame_zh"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL_zh"].ToString();
                                #endregion
                                break;
                            default:
                                #region default
                                PCMModuleReferencesReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportLINKS"].ToString();
                                PCMModuleReferencesReport.xrLabel1.Text = Application.Current.Resources["PCMPrintModuleReportURLame"].ToString();
                                PCMModuleReferencesReport.xrLabel2.Text = Application.Current.Resources["PCMPrintModuleReportURL"].ToString();
                                #endregion
                                break;
                        }
                        #endregion
                        PCMModuleReferencesReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleReferencesReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleReferencesReport.objectDataSource1.DataSource = ProductTypesDetails.ProductTypeAttachedLinkList;
                        pcmPrintModuleReport.xrLinkList.ReportSource = PCMModuleReferencesReport;

                    }
                }

                #endregion
                #region ProductTypeAttachedDocList
                if (IsPCMPrintModuleAttachments)
                {
                    if (ProductTypesDetails.ProductTypeAttachedDocList != null && ProductTypesDetails.ProductTypeAttachedDocList.Count > 0)
                    {
                        PCMModuleDocumentationReport PCMModuleDocumentationReport = new PCMModuleDocumentationReport();
                        #region LINKS
                        //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_es"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_pt"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_zh"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_ro"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_fr"].ToString();
                        //PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_ru"].ToString();
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS"].ToString();
                                #endregion
                                break;
                            case "es":
                                #region es
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_es"].ToString();
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_fr"].ToString();
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_pt"].ToString();
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_ro"].ToString();
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_ru"].ToString();
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS_zh"].ToString();
                                #endregion
                                break;
                            default:
                                #region default
                                PCMModuleDocumentationReport.xrLabel9.Text = Application.Current.Resources["PCMPrintModuleReportATTACHMENTS"].ToString();
                                #endregion
                                break;
                        }
                        #endregion

                        PCMModuleDocumentationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleDocumentationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleDocumentationReport.objectDataSource2.DataSource = ProductTypesDetails.ProductTypeAttachedDocList;
                        pcmPrintModuleReport.xrDocList.ReportSource = PCMModuleDocumentationReport;
                    }
                }
                #endregion
                #region WayList
                if (ProductTypesDetails.WayList!=null && ProductTypesDetails.WayList.Count>0)
                {
                    PCMModuleWayListReport PCMModuleWayListReport = new PCMModuleWayListReport();
                    PCMModuleWayListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleWayListReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    PCMModuleWayListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleWayListReport.xrLabel8.Text = "●" +"   "+ PCMModuleWayListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f=>f.IdDetectionType==1);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleWayListReport.xrRichText1.Text = "";
                    foreach (var Wayitem in ProductTypesDetails.WayList)
                    {
                        //PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t" +"\u006F" + "   " + Wayitem.Name + "\n";
                        //PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                        #region switch
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                                #endregion
                                break;
                            case "es":
                                #region es
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_es + "\n";
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_fr + "\n";
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_pt + "\n";
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_ro + "\n";
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_ru + "\n";
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_zh + "\n";
                                #endregion
                                break;
                            default:
                                #region default
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    //PCMModuleWayListReport.objectDataSource1.DataSource = ProductTypesDetails.WayList;
                    pcmPrintModuleReport.xrWayList.ReportSource = PCMModuleWayListReport;
                }
                #endregion
                #region FamilyList
                if (ProductTypesDetails.FamilyList != null && ProductTypesDetails.FamilyList.Count > 0)
                {
                    PCMModuleFamilyListReport PCMModuleFamilyListReport = new PCMModuleFamilyListReport();
                    PCMModuleFamilyListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleFamilyListReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleFamilyListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleFamilyListReport.xrLabel1.Text = "●" + "   " + PCMModuleFamilyListReport.xrLabel1.Text;
                    //DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 1);
                    PCMModuleFamilyListReport.xrRichText1.Text = "";
                    foreach (var Familyitem in ProductTypesDetails.FamilyList)
                    {
                        //PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                        #region switch
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                                #endregion
                                break;
                            case "es":
                                #region es
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_es + "\n";
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_fr + "\n";
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_pt + "\n";
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_ro + "\n";
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_ru + "\n";
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_zh + "\n";
                                #endregion
                                break;
                            default:
                                #region default
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    //PCMModuleFamilyListReport.objectDataSource1.DataSource = ProductTypesDetails.FamilyList;
                    pcmPrintModuleReport.xrFamilyList.ReportSource = PCMModuleFamilyListReport;
                }
                #endregion
                #region DetectionList_Group
                if (ProductTypesDetails.DetectionList_Group != null && ProductTypesDetails.DetectionList_Group.Count > 0)
                {
                    //"\u2022" 
                    // \u26AB ★
                    //"●" ■
                    //\u006F  o
                    PCMModuleDetectionListReport PCMModuleDetectionListReport = new PCMModuleDetectionListReport();
                    PCMModuleDetectionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleDetectionListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleDetectionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleDetectionListReport.xrLabel8.Text = "●" +"   " + PCMModuleDetectionListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 2);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleDetectionListReport.xrRichText1.Text ="";
                    List<Detections> GetAllDetections= ProductTypesDetails.DetectionList_Group.FindAll(f=>f.Parent==null);
                    foreach (Detections Detectionitem in GetAllDetections)
                    {
                        PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t"+ "\u006F" + "   " + Detectionitem.Name +"\n";
                        List<Detections> GetAllDetectionschild = ProductTypesDetails.DetectionList_Group.FindAll(f => f.Parent == Detectionitem.Key);
                        foreach (var childDetectionItem in GetAllDetectionschild)
                        {
                            //PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t"+ "\u2022" + "   "+ childDetectionItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrDetectionList.ReportSource = PCMModuleDetectionListReport;
                }
                #endregion
                #region OptionList_Group
                if (ProductTypesDetails.OptionList_Group != null && ProductTypesDetails.OptionList_Group.Count > 0)
                {
                    PCMModuleOptionListReport PCMModuleOptionListReport = new PCMModuleOptionListReport();
                    PCMModuleOptionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleOptionListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleOptionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + PCMModuleOptionListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 3);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleOptionListReport.xrRichText1.Text = "";
                    List<Options> GetAllOption = ProductTypesDetails.OptionList_Group.FindAll(f => f.Parent == null);
                    foreach (Options Option in GetAllOption)
                    {
                        PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Option.Name + "\n";
                        List<Options> GetAllOptionschild = ProductTypesDetails.OptionList_Group.FindAll(f => f.Parent == Option.Key);
                        foreach (var childOptionItem in GetAllOptionschild)
                        {
                            //PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrOptionList.ReportSource = PCMModuleOptionListReport;
                }
                #endregion
                #region SparePartsList_Group
                if (ProductTypesDetails.SparePartsList_Group != null && ProductTypesDetails.SparePartsList_Group.Count > 0)
                {
                    PCMModuleSparePartsListReport PCMModuleSparePartsListReport = new PCMModuleSparePartsListReport();
                    PCMModuleSparePartsListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleSparePartsListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleSparePartsListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + PCMModuleSparePartsListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 4);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleSparePartsListReport.xrRichText1.Text = "";
                    List<SpareParts> GetAllSpareParts = ProductTypesDetails.SparePartsList_Group.FindAll(f => f.Parent == null);
                    foreach (SpareParts SpareParts in GetAllSpareParts)
                    {
                        PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + SpareParts.Name + "\n";
                        List<SpareParts> GetAllSparePartschild = ProductTypesDetails.SparePartsList_Group.FindAll(f => f.Parent == SpareParts.Key);
                        foreach (SpareParts childSparePartsItem in GetAllSparePartschild)
                        {
                            //PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   "+Detectiontypes.Name;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrSparePartsList.ReportSource = PCMModuleSparePartsListReport;
                }
                #endregion
                #region GeosApplication.Instance
                //var WorkbenchLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //var geosApplication= GeosApplication.Instance;
                //var SelectedPlantOwnerUsersList= GeosApplication.Instance.SelectedPlantOwnerUsersList;
                //List<Company> emdepSiteList = GeosApplication.Instance.EmdepSiteList;
                //Company Company= emdepSiteList.Find(f=>f.IdCompany==559);
                //Company Company = GeosApplication.Instance.EmdepSiteList.Find(f => f.IdCompany == 18);
                #endregion
                #region GetAllCompanyinfo
                try
                {
                    dynamic d = (dynamic)GeosApplication.Instance.SelectedPlantOwnerUsersList;
                    Emdep.Geos.Data.Common.Company company = (Company)d[0];
                    Company GetAllCompanyinfo = CrmStartUp.GetCompanyDetailsById(company.IdCompany);
                    pcmPrintModuleReport.xrLblEmdep.Text = GetAllCompanyinfo.RegisteredName.ToString();
                    pcmPrintModuleReport.xrLblEmail.Text = GetAllCompanyinfo.Email.ToString();
                    pcmPrintModuleReport.xrlblPhone.Text = GetAllCompanyinfo.Telephone.ToString();
                    pcmPrintModuleReport.xrlblWebSite.Text = GetAllCompanyinfo.Website.ToString();

                }
                catch (Exception ex)
                {
                }
                #endregion
                //pcmPrintModuleReport.xrLblEmdep=
                //ImageSource bmp = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/PCMModuleReportPageFooter.png"));
                //System.Drawing.Image imgWin = System.Drawing.Image.FromFile();
                // pcmPrintModuleReport.xrPictureBox4.Image = bitmap;
                //image = doc.Shapes.InsertPicture(doc.CreatePosition(0), DocumentImageSource.FromFile((new Uri(@"pack://application:,,,/Emdep.Geos.Modules.PCM;component/Assets/Images/PCMModuleReportPageFooter.png")).LocalPath));
                //image.TextWrapping = TextWrappingType.InLineWithText;
                // pcmPrintModuleReport.xrPictureBox4.Image = GetBitmap((BitmapSource)bmp);
                try
                {
                    if (ProductTypesDetails.ProductTypeImageList.Count==0)
                    {
                        pcmPrintModuleReport.xrPictureBox1.Visible = false;
                        pcmPrintModuleReport.xrPictureBox2.Visible = false;
                        pcmPrintModuleReport.xrPictureBox3.Visible = false;
                    }
                    if (ProductTypesDetails.ProductTypeImageList!=null && ProductTypesDetails.ProductTypeImageList.Count > 0)
                    {
                        PCMModuleProductTypeImageListReport PCMModuleProductTypeImageListReport = new PCMModuleProductTypeImageListReport();
                        foreach (ProductTypeImage ProductTypeitem in ProductTypesDetails.ProductTypeImageList)
                        {
                            if (ProductTypeitem.ProductTypeImageInBytes!=null)
                            {
                                var ms = new MemoryStream(ProductTypeitem.ProductTypeImageInBytes);
                                System.Drawing.Image image = Image.FromStream(ms);
                                Bitmap Bitmap = ResizeImage(new Bitmap(image));
                                //Bitmap Bitmap = (new Bitmap(image));
                                switch (ProductTypeitem.Position)
                                {
                                    #region 1TO3
                                    case 1:
                                        //Bitmap Bitmap = ResizeImage(new Bitmap(image));
                                        pcmPrintModuleReport.xrPictureBox1.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox1.WidthF = Bitmap.Width;

                                        //pcmPrintModuleReport.xrLabel5.Text = ProductTypeitem.Description;
                                        //pcmPrintModuleReport.xrPictureBox1.Image = image;
                                        break;
                                    case 2:

                                        pcmPrintModuleReport.xrPictureBox2.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        //pcmPrintModuleReport.xrLabel6.Text = ProductTypeitem.Description;
                                        break;
                                    case 3:
                                        pcmPrintModuleReport.xrPictureBox3.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        // pcmPrintModuleReport.xrLabel7.Text = ProductTypeitem.Description;
                                        break;
                                    #endregion
                                    #region 4TO6
                                    case 4:
                                        //PCMModuleProductTypeImageListReport.xrPictureBox1.Image = ResizeImage(new Bitmap(image));
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel5.Text = ProductTypeitem.Description;
                                        break;
                                    case 5:
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel6.Text = ProductTypeitem.Description;
                                        break;
                                    case 6:
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel7.Text = ProductTypeitem.Description;
                                        break;
                                    #endregion
                                    #region 7to9
                                    case 7:
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel1.Text = ProductTypeitem.Description;
                                        RectangleF rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox1.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel1.BoundsF = rectangleF;
                                        RectangleF rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 8:
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel2.Text = ProductTypeitem.Description;

                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox2.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel2.BoundsF = rectangleF;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 9:
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel3.Text = ProductTypeitem.Description;

                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox3.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel3.BoundsF = rectangleF;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                        #endregion
                                }
                            }
                        }
                        if (ProductTypesDetails.ProductTypeImageList.Count >3)
                        {
                            pcmPrintModuleReport.xrImageList.ReportSource = PCMModuleProductTypeImageListReport;
                        }
                    }
                    
                } catch (Exception ex){ }
                //pcmPrintModuleReport.DataSource = ProductTypesDetails;
                //PCMModuleDocumentationReport.objectDataSource1.DataSource = ProductTypesDetails;
                pcmPrintModuleReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                pcmPrintModuleReport.ExportOptions.Email.Subject = FileName;
                if (IsPCMPrintModuleAttachments)
                {
                    try
                    {
                        if (ProductTypesDetails.ProductTypeAttachedDocList != null && ProductTypesDetails.ProductTypeAttachedDocList.Count > 0)
                        {
                            XtraReport report = new XtraReport();
                            int count = 0;
                            List<PdfDocumentProcessor> pdfDocumentProcessorList = new List<PdfDocumentProcessor>();
                            foreach (ProductTypeAttachedDoc item in ProductTypesDetails.ProductTypeAttachedDocList)
                            {
                                try
                                {
                                    //using (MemoryStream stream = new MemoryStream())
                                    //{
                                    //    report.SaveLayout(stream);
                                    //}
                                    MemoryStream ProductTypeAttachedDocInBytesstream = new MemoryStream(item.ProductTypeAttachedDocInBytes);
                                    //File.WriteAllBytes("D:\\"+item.OriginalFileName + ".pdf", item.ProductTypeAttachedDocInBytes);
                                    XtraReport xtraReport = new XtraReport();
                                    // XtraReport.LoadLayout(ProductTypeAttachedDocInBytesstream);
                                    xtraReport = XtraReport.FromStream(ProductTypeAttachedDocInBytesstream, true);
                                    //xtraReport.LoadLayout("D:\\" + item.OriginalFileName + ".pdf");
                                    //pcmPrintModuleReport.xrRichText1.LoadFile(ProductTypeAttachedDocInBytesstream, XRRichTextStreamType.RtfText);
                                    if (xtraReport!=null)
                                    {
                                        xtraReport.CreateDocument();
                                        report.ModifyDocument(x => x.AddPages(xtraReport.Pages));
                                    }
                                    PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
                                    //pdfDocumentProcessor.CreateEmptyDocument(ProductTypeAttachedDocInBytesstream);
                                    pdfDocumentProcessor.AppendDocument(ProductTypeAttachedDocInBytesstream);
                                    pdfDocumentProcessorList.Add(pdfDocumentProcessor);
                                    #region OldCode
                                    //if (count == 0)
                                    //{
                                    //    pdfDocumentProcessor.CreateEmptyDocument(ProductTypeAttachedDocInBytesstream);
                                    //    count = count + 1;
                                    //}
                                    //else
                                    //{
                                    //    pdfDocumentProcessor.AppendDocument(ProductTypeAttachedDocInBytesstream);
                                    //}
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            int largestEdgeLength = 1000;
                            //if (ProductTypesDetails.MergePDFDocument == null)
                                ProductTypesDetails.MergePDFDocument = new List<Bitmap>();
                            #region WorkingCode
                            //int pageCount = pdfDocumentProcessorList[0].Document.Pages.Count;
                            ////pdfDocumentProcessorList[0].SaveDocument("D:\\pdfDocumentProcessorList01.pdf");
                            //Bitmap pdfPageBitmap = pdfDocumentProcessorList[0].CreateBitmap(1, largestEdgeLength);
                            ////pdfPageBitmap.Save("D:\\pdfPageBitmap.png", ImageFormat.Png);
                            //pcmPrintModuleReport.xrPictureBox4.Image = pdfPageBitmap;
                            //pcmPrintModuleReport.xrPictureBox4.HeightF = pdfPageBitmap.Height;
                            //pcmPrintModuleReport.xrPictureBox4.WidthF = pdfPageBitmap.Width;
                            #endregion
                            foreach (PdfDocumentProcessor PdfDocumentProcessor in pdfDocumentProcessorList)
                            {
                                for (int i = 1; i <= PdfDocumentProcessor.Document.Pages.Count; i++)
                                {
                                    Bitmap BitmapImage = PdfDocumentProcessor.CreateBitmap(i, largestEdgeLength);
                                    var pb = new XRPictureBox
                                    {
                                        Image = BitmapImage,
                                    };
                                    ProductTypesDetails.MergePDFDocument.Add(BitmapImage);
                                    //ProductTypesDetails.MergePDF = BitmapImage;
                                }
                            }
                            for (int i = 0; i < ProductTypesDetails.MergePDFDocument.Count; i++)
                            {
                                pcmPrintModuleReport.SubBand1.Band.Controls.Add(new XRPictureBox
                                {
                                    //Image = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Penguins.jpg")),
                                    Image = ProductTypesDetails.MergePDFDocument[i],
                                    Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze,
                                    Size = new System.Drawing.Size((ProductTypesDetails.MergePDFDocument[i].Width - 200), (ProductTypesDetails.MergePDFDocument[i].Height-200)),
                                    LocationF = new PointF(0, i * 800)
                                });
                                #region Bands
                                //pcmPrintModuleReport.Bands[BandKind.Detail].Controls.Add(new XRPictureBox
                                //{
                                //    //Image = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Penguins.jpg")),
                                //    Image = ProductTypesDetails.MergePDFDocument[i],
                                //    Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze,
                                //    Size = new System.Drawing.Size(ProductTypesDetails.MergePDFDocument[i].Width, ProductTypesDetails.MergePDFDocument[i].Height),
                                //    LocationF = new PointF(0, i * 1000)
                                //});
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //[GEOS2-6531][rdixit][07.01.2025]
                if (IncludedCustomersList != null && IncludedCustomersList.Count > 0)
                {
                    IncludedCustomersReport includedCustomersReport = new IncludedCustomersReport();
                    includedCustomersReport.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    includedCustomersReport.table2.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 7, System.Drawing.FontStyle.Regular);
                    includedCustomersReport.objectDataSource1.DataSource = IncludedCustomersList;
                    pcmPrintModuleReport.xrIncludedCustomerList.ReportSource = includedCustomersReport;
                    #region Customer
                    try
                    {
                        #region switch
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                includedCustomersReport.xrLblCustomer.Text = "Customers :";
                                includedCustomersReport.tableCell1.Text = "Group";
                                includedCustomersReport.tableCell2.Text = "Region";
                                includedCustomersReport.tableCell3.Text = "Country";
                                includedCustomersReport.tableCell4.Text = "Plant";
                                break;
                            case "es":
                                includedCustomersReport.xrLblCustomer.Text = "Clientes :";
                                includedCustomersReport.tableCell1.Text = "Grupo";
                                includedCustomersReport.tableCell2.Text = "Región";
                                includedCustomersReport.tableCell3.Text = "País";
                                includedCustomersReport.tableCell4.Text = "Planta";
                                break;
                            case "fr":
                                includedCustomersReport.xrLblCustomer.Text = "Clients :";
                                includedCustomersReport.tableCell1.Text = "Groupe";
                                includedCustomersReport.tableCell2.Text = "Région";
                                includedCustomersReport.tableCell3.Text = "Pays";
                                includedCustomersReport.tableCell4.Text = "Usine";
                                break;
                            case "pt":
                                includedCustomersReport.xrLblCustomer.Text = "Clientes :";
                                includedCustomersReport.tableCell1.Text = "Grupo";
                                includedCustomersReport.tableCell2.Text = "Região";
                                includedCustomersReport.tableCell3.Text = "País";
                                includedCustomersReport.tableCell4.Text = "Planta";
                                break;
                            case "ro":
                                includedCustomersReport.xrLblCustomer.Text = "Clienți :";
                                includedCustomersReport.tableCell1.Text = "Grup";
                                includedCustomersReport.tableCell2.Text = "Regiune";
                                includedCustomersReport.tableCell3.Text = "Țară";
                                includedCustomersReport.tableCell4.Text = "Fabrică";
                                break;
                            case "ru":
                                includedCustomersReport.xrLblCustomer.Text = "Клиенты :";
                                includedCustomersReport.tableCell1.Text = "Группа";
                                includedCustomersReport.tableCell2.Text = "Регион";
                                includedCustomersReport.tableCell3.Text = "Страна";
                                includedCustomersReport.tableCell4.Text = "Завод";
                                break;
                            case "zh":
                                includedCustomersReport.xrLblCustomer.Text = "客户 :";
                                includedCustomersReport.tableCell1.Text = "组";
                                includedCustomersReport.tableCell2.Text = "地区";
                                includedCustomersReport.tableCell3.Text = "国家";
                                includedCustomersReport.tableCell4.Text = "工厂";
                                break;
                            default:
                                includedCustomersReport.xrLblCustomer.Text = "Customers :";
                                includedCustomersReport.tableCell1.Text = "Group";
                                includedCustomersReport.tableCell2.Text = "Region";
                                includedCustomersReport.tableCell3.Text = "Country";
                                includedCustomersReport.tableCell4.Text = "Plant";
                                break;
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion
                }
                #region Watermark
                if (!ProductTypesDetails.Status.Value.ToLower().Equals("Active".ToLower()))
                {
                    pcmPrintModuleReport.Watermark.ShowBehind = true;
                    pcmPrintModuleReport.Watermark.Text = ProductTypesDetails.Status.Value;
                    pcmPrintModuleReport.Watermark.Font = new Font(pcmPrintModuleReport.Watermark.Font.FontFamily, 150);
                    pcmPrintModuleReport.Watermark.ForeColor = System.Drawing.Color.Gray;
                    pcmPrintModuleReport.Watermark.TextTransparency = 150;
                }
                #endregion
               
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcmPrintModuleReport;
                pcmPrintModuleReport.CreateDocument();
                window.Show();
                #region MyRegion
                //string ResultFileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\Report1.pdf";
                //XtraReport XtraReport = CreateReport();
                //XtraReport.ExportToPdf(ResultFileName);
                //System.Diagnostics.Process.Start(ResultFileName);
                // PrintableControlLink pcl = new PrintableControlLink(XtraReport);
                //PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                //pcl.Margins.Bottom = 5;
                //pcl.Margins.Top = 5;
                //pcl.Margins.Left = 5;
                //pcl.Margins.Right = 5;
                //pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                //pcl.Landscape = true;
                //pcl.CreateDocument(false);

                //DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                //window.PreviewControl.DocumentSource = pcl;
                //// IsBusy = false;
                //window.Show();

                //DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                //printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];
                #endregion
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandWindow()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// method for resize image.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bitmap)
        {
            GeosApplication.Instance.Logger.Log("Method ResizeImage ...", category: Category.Info, priority: Priority.Low);

            Bitmap resized = new Bitmap(200, 200);
            try
            {
                Graphics g = Graphics.FromImage(resized);

                g.DrawImage(bitmap, new Rectangle(0, 0, resized.Width, resized.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                g.Dispose();

                //Save picture in users temp folder.
                string myTempFile = Path.Combine(Path.GetTempPath(), "EmdepLogo.jpg");

                //delete if already image exist there.
                if (File.Exists(myTempFile))
                {
                    File.Delete(myTempFile);
                }

                resized.Save(myTempFile, ImageFormat.Jpeg);

                return resized;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ResizeImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return resized;
        }
        Bitmap GetBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        /// <summary>
        /// This method is for to Fill language image in list as per Culture
        /// </summary>
        private void FillLanguage()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Getting All Language on list - FillLanguage()", category: Category.Info, priority: Priority.Low);
                Languages = WorkbenchStartUp.GetAllLanguage();

                for (int i = 0; i < languages.Count; i++)
                {
                    Languages[i].LanguageImage = GetImage("/Assets/Images/" + Languages[i].Name + ".gif");

                    if (GeosApplication.Instance.CurrentCulture.Substring(0, 2).ToUpper() == Languages[i].TwoLetterISOLanguage.ToString().ToUpper())
                    {
                        SettingWindowLanguageSelectedIndex = i;
                    }
                }
                try
                {
                    //Dictionary<string, byte[]> CountryIcon = PCMService.GetCountryIconFileInBytes();
                    foreach (var item in Languages)
                    {
                        ImageSource countryFlagImage = ByteArrayToBitmapImage(GetLanguagesImage(item.TwoLetterISOLanguage.ToLower().ToString()));
                        item.LanguageImage = countryFlagImage;
                        //var temp=CountryIcon.ContainsKey(item.TwoLetterISOLanguage.ToUpper());
                        //if (temp)
                        //{
                        //    ImageSource countryFlagImage = ByteArrayToBitmapImage(CountryIcon[item.TwoLetterISOLanguage.ToUpper()]);
                        //    item.LanguageImage = countryFlagImage;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
                GeosApplication.Instance.Logger.Log("Getting All Language on list successfully - FillLanguage()", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new System.Windows.Media.Imaging.BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
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
        /// <summary>
        /// This method is for to set Languge Dictionary
        /// </summary>
        private void SetLanguageDictionary()
        {
            string TempLanguage = GeosApplication.Instance.CurrentCulture;
            ResourceDictionary dict = new ResourceDictionary();

            Language = Languages[SettingWindowLanguageSelectedIndex].CultureName;
            GeosApplication.Instance.CurrentCulture = Language;

            if (TempLanguage != GeosApplication.Instance.CurrentCulture)
            {
                try
                {
                    GeosApplication.Instance.SetLanguageDictionary();
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                    dict.Source = new Uri("/GeosWorkbench;component/Resources/Language." + Language + ".xaml", UriKind.Relative);

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Language"))
                    {
                        GeosApplication.Instance.UserSettings["Language"] = GeosApplication.Instance.CurrentCulture;
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("Language", GeosApplication.Instance.CurrentCulture);
                    }
                }
                catch (Exception)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                    dict.Source = new Uri("/GeosWorkbench;component/Resources/Language.xaml", UriKind.Relative);

                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Language"))
                    {
                        GeosApplication.Instance.UserSettings["Language"] = GeosApplication.Instance.CurrentCulture;
                    }
                    else
                    {
                        GeosApplication.Instance.UserSettings.Add("Language", GeosApplication.Instance.CurrentCulture);
                    }
                }

                //App.Current.Resources.MergedDictionaries.Add(dict);
                IsLanguagteChange = true;
            }
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        public string AnnouncementFolderName { get; set; }
        public string AnnouncementFileName { get; set; }
        public void AcceptButtonCommandWindow()
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
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                //string FileName = "PCMModuleReport";
                string FileName = ProductTypesDetails.Reference + "_" + ProductTypesDetails.Name;
                //string PCMModuleReportPageFooter="/Emdep.Geos.Modules.PCM;component/Assets/Images/PCMModuleReportPageFooter.png";

                // byte[] imgData = System.IO.File.ReadAllBytes(PCMModuleReportPageFooter);
                //PCMPrintModuleReport pcmPrintModuleReport = new PCMPrintModuleReport();
                PCMPrintModuleReportNew pcmPrintModuleReport = new PCMPrintModuleReportNew();
                pcmPrintModuleReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleReport.xrModuleDatasheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);

                pcmPrintModuleReport.xrLabel1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrLabel2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                //pcmPrintModuleReport.xrLabel3.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);//[GEOS2-6531][rdixit][07.01.2025]
                pcmPrintModuleReport.xrLabel4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                pcmPrintModuleReport.xrLabel4.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.PCM.Properties.Resources.Emdep_logo_mini);
                pcmPrintModuleReport.imgLogo.Image = BitmapimgLogo;
                pcmPrintModuleReport.imgLogo.Height = BitmapimgLogo.Height;
                pcmPrintModuleReport.imgLogo.WidthF = BitmapimgLogo.Width;

                pcmPrintModuleReport.xrLblEmdep.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleReport.xrLblEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrlblPhone.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleReport.xrlblWebSite.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Language Language = Languages[settingWindowLanguageSelectedIndex];
                #region ProductTypesDetails
                try
                {
                    // [settingWindowLanguageSelectedIndex];
                    //pcmPrintModuleReport.xrtxtName.Text = ProductTypesDetails.Name;
                    //pcmPrintModuleReport.xrDescription.Text = ProductTypesDetails.Description;
                    #region switch
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description;

                            #endregion
                            break;
                        case "es":
                            #region es
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_es;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_es;
                            #endregion
                            break;

                        case "fr":
                            #region es
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_fr;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_pt;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_ro;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_ru;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name_zh;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            pcmPrintModuleReport.xrLabel1.Text = ProductTypesDetails.Name;
                            pcmPrintModuleReport.xrLabel2.Text = ProductTypesDetails.Description;
                            #endregion
                            break;
                    }
                    #endregion
                    //pcmPrintModuleReport.xrLabel3.Text = ProductTypesDetails.Template.Name; //[GEOS2-6531][rdixit][07.01.2025]
                    if (ProductTypesDetails.LastUpdate == null)
                    {
                        pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.CreatedIn.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        pcmPrintModuleReport.xrLabel4.Text = ProductTypesDetails.LastUpdate.Value.ToString("dd/MM/yyyy");
                    }

                }
                catch (Exception ex)
                {
                }

                #endregion

                #region ProductTypeAttachedLinkList
                if (IsPCMPrintModuleLinks)
                {
                    if (ProductTypesDetails.ProductTypeAttachedLinkList != null && ProductTypesDetails.ProductTypeAttachedLinkList.Count > 0)
                    {
                        PCMModuleReferencesReport PCMModuleReferencesReport = new PCMModuleReferencesReport();
                        PCMModuleReferencesReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleReferencesReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleReferencesReport.objectDataSource1.DataSource = ProductTypesDetails.ProductTypeAttachedLinkList;
                        pcmPrintModuleReport.xrLinkList.ReportSource = PCMModuleReferencesReport;

                    }
                }

                #endregion

                #region ProductTypeAttachedDocList
                if (IsPCMPrintModuleAttachments)
                {
                    if (ProductTypesDetails.ProductTypeAttachedDocList != null && ProductTypesDetails.ProductTypeAttachedDocList.Count > 0)
                    {
                        PCMModuleDocumentationReport PCMModuleDocumentationReport = new PCMModuleDocumentationReport();
                        PCMModuleDocumentationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleDocumentationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleDocumentationReport.objectDataSource2.DataSource = ProductTypesDetails.ProductTypeAttachedDocList;
                        pcmPrintModuleReport.xrDocList.ReportSource = PCMModuleDocumentationReport;
                    }
                }
                #endregion
                #region WayList
                if (ProductTypesDetails.WayList != null && ProductTypesDetails.WayList.Count > 0)
                {
                    PCMModuleWayListReport PCMModuleWayListReport = new PCMModuleWayListReport();
                    PCMModuleWayListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleWayListReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    PCMModuleWayListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleWayListReport.xrLabel8.Text = "●" +"   "+ PCMModuleWayListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 1);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleWayListReport.xrRichText1.Text = "";
                    foreach (var Wayitem in ProductTypesDetails.WayList)
                    {
                        //PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t" +"\u006F" + "   " + Wayitem.Name + "\n";
                        //PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                        #region switch
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                                #endregion
                                break;
                            case "es":
                                #region es
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_es + "\n";
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_fr + "\n";
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_pt + "\n";
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_ro + "\n";
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_ru + "\n";
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name_zh + "\n";
                                #endregion
                                break;
                            default:
                                #region default
                                //PCMModuleWayListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                PCMModuleWayListReport.xrRichText1.Text = PCMModuleWayListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Wayitem.Name + "\n";
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    //PCMModuleWayListReport.objectDataSource1.DataSource = ProductTypesDetails.WayList;
                    pcmPrintModuleReport.xrWayList.ReportSource = PCMModuleWayListReport;
                }
                #endregion
                #region FamilyList
                if (ProductTypesDetails.FamilyList != null && ProductTypesDetails.FamilyList.Count > 0)
                {
                    PCMModuleFamilyListReport PCMModuleFamilyListReport = new PCMModuleFamilyListReport();
                    PCMModuleFamilyListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleFamilyListReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleFamilyListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleFamilyListReport.xrLabel1.Text = "●" + "   " + PCMModuleFamilyListReport.xrLabel1.Text;
                    //DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 1);
                    PCMModuleFamilyListReport.xrRichText1.Text = "";
                    foreach (var Familyitem in ProductTypesDetails.FamilyList)
                    {
                        //PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                        #region switch
                        switch (Language.TwoLetterISOLanguage.ToLower())
                        {
                            case "en":
                                #region en
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                                #endregion
                                break;
                            case "es":
                                #region es
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_es + "\n";
                                #endregion
                                break;

                            case "fr":
                                #region fr
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_fr + "\n";
                                #endregion
                                break;

                            case "pt":
                                #region pt
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_pt + "\n";
                                #endregion
                                break;
                            case "ro":
                                #region ro
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_ro + "\n";
                                #endregion
                                break;
                            case "ru":
                                #region ru
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_ru + "\n";
                                #endregion
                                break;
                            case "zh":
                                #region zh
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name_zh + "\n";
                                #endregion
                                break;
                            default:
                                #region default
                                PCMModuleFamilyListReport.xrRichText1.Text = PCMModuleFamilyListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Familyitem.Name + "\n";
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    //PCMModuleFamilyListReport.objectDataSource1.DataSource = ProductTypesDetails.FamilyList;
                    pcmPrintModuleReport.xrFamilyList.ReportSource = PCMModuleFamilyListReport;
                }
                #endregion
                #region DetectionList_Group
                if (ProductTypesDetails.DetectionList_Group != null && ProductTypesDetails.DetectionList_Group.Count > 0)
                {
                    //"\u2022" 
                    // \u26AB ★
                    //"●" ■
                    //\u006F  o
                    PCMModuleDetectionListReport PCMModuleDetectionListReport = new PCMModuleDetectionListReport();
                    PCMModuleDetectionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleDetectionListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleDetectionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleDetectionListReport.xrLabel8.Text = "●" +"   " + PCMModuleDetectionListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 2);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleDetectionListReport.xrRichText1.Text = "";
                    List<Detections> GetAllDetections = ProductTypesDetails.DetectionList_Group.FindAll(f => f.Parent == null);
                    foreach (Detections Detectionitem in GetAllDetections)
                    {
                        PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Detectionitem.Name + "\n";
                        List<Detections> GetAllDetectionschild = ProductTypesDetails.DetectionList_Group.FindAll(f => f.Parent == Detectionitem.Key);
                        foreach (var childDetectionItem in GetAllDetectionschild)
                        {
                            //PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t"+ "\u2022" + "   "+ childDetectionItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleDetectionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleDetectionListReport.xrRichText1.Text = PCMModuleDetectionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childDetectionItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrDetectionList.ReportSource = PCMModuleDetectionListReport;
                }
                #endregion
                #region OptionList_Group
                if (ProductTypesDetails.OptionList_Group != null && ProductTypesDetails.OptionList_Group.Count > 0)
                {
                    PCMModuleOptionListReport PCMModuleOptionListReport = new PCMModuleOptionListReport();
                    PCMModuleOptionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleOptionListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleOptionListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + PCMModuleOptionListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 3);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleOptionListReport.xrRichText1.Text = "";
                    List<Options> GetAllOption = ProductTypesDetails.OptionList_Group.FindAll(f => f.Parent == null);
                    foreach (Options Option in GetAllOption)
                    {
                        PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + Option.Name + "\n";
                        List<Options> GetAllOptionschild = ProductTypesDetails.OptionList_Group.FindAll(f => f.Parent == Option.Key);
                        foreach (var childOptionItem in GetAllOptionschild)
                        {
                            //PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleOptionListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleOptionListReport.xrRichText1.Text = PCMModuleOptionListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childOptionItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrOptionList.ReportSource = PCMModuleOptionListReport;
                }
                #endregion
                #region SparePartsList_Group
                if (ProductTypesDetails.SparePartsList_Group != null && ProductTypesDetails.SparePartsList_Group.Count > 0)
                {
                    PCMModuleSparePartsListReport PCMModuleSparePartsListReport = new PCMModuleSparePartsListReport();
                    PCMModuleSparePartsListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    PCMModuleSparePartsListReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    PCMModuleSparePartsListReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + PCMModuleSparePartsListReport.xrLabel8.Text;
                    DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == 4);
                    #region Detectiontypes.Name
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                        case "es":
                            #region es
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                            #endregion
                            break;
                    }
                    #endregion
                    PCMModuleSparePartsListReport.xrRichText1.Text = "";
                    List<SpareParts> GetAllSpareParts = ProductTypesDetails.SparePartsList_Group.FindAll(f => f.Parent == null);
                    foreach (SpareParts SpareParts in GetAllSpareParts)
                    {
                        PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t" + "\u006F" + "   " + SpareParts.Name + "\n";
                        List<SpareParts> GetAllSparePartschild = ProductTypesDetails.SparePartsList_Group.FindAll(f => f.Parent == SpareParts.Key);
                        foreach (SpareParts childSparePartsItem in GetAllSparePartschild)
                        {
                            //PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                            #region switch
                            switch (Language.TwoLetterISOLanguage.ToLower())
                            {
                                case "en":
                                    #region en
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                                    #endregion
                                    break;
                                case "es":
                                    #region es
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_es;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_es + "\n";
                                    #endregion
                                    break;

                                case "fr":
                                    #region fr
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_fr;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_fr + "\n";
                                    #endregion
                                    break;

                                case "pt":
                                    #region pt
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_pt;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_pt + "\n";
                                    #endregion
                                    break;
                                case "ro":
                                    #region ro
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ro;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_ro + "\n";
                                    #endregion
                                    break;
                                case "ru":
                                    #region ru
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_ru;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_ru + "\n";
                                    #endregion
                                    break;
                                case "zh":
                                    #region zh
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name_zh;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name_zh + "\n";
                                    #endregion
                                    break;
                                default:
                                    #region default
                                    PCMModuleSparePartsListReport.xrLabel8.Text = "●" + "   " + Detectiontypes.Name;
                                    PCMModuleSparePartsListReport.xrRichText1.Text = PCMModuleSparePartsListReport.xrRichText1.Text + "\t\t\t" + "\u2022" + "   " + childSparePartsItem.Name + "\n";
                                    #endregion
                                    break;
                            }
                            #endregion
                        }
                    }
                    pcmPrintModuleReport.xrSparePartsList.ReportSource = PCMModuleSparePartsListReport;
                }
                #endregion               
                #region GetAllCompanyinfo
                try
                {
                    dynamic d = (dynamic)GeosApplication.Instance.SelectedPlantOwnerUsersList;
                    Emdep.Geos.Data.Common.Company company = (Company)d[0];
                    Company GetAllCompanyinfo = CrmStartUp.GetCompanyDetailsById(company.IdCompany);
                    pcmPrintModuleReport.xrLblEmdep.Text = GetAllCompanyinfo.RegisteredName.ToString();
                    pcmPrintModuleReport.xrLblEmail.Text = GetAllCompanyinfo.Email.ToString();
                    pcmPrintModuleReport.xrlblPhone.Text = GetAllCompanyinfo.Telephone.ToString();
                    pcmPrintModuleReport.xrlblWebSite.Text = GetAllCompanyinfo.Website.ToString();

                }
                catch (Exception ex)
                {
                }
                #endregion
              
                try
                {
                    if (ProductTypesDetails.ProductTypeImageList.Count == 0)
                    {
                        pcmPrintModuleReport.xrPictureBox1.Visible = false;
                        pcmPrintModuleReport.xrPictureBox2.Visible = false;
                        pcmPrintModuleReport.xrPictureBox3.Visible = false;

                      
                    }
                    if (ProductTypesDetails.ProductTypeImageList != null && ProductTypesDetails.ProductTypeImageList.Count > 0)
                    {
                        PCMModuleProductTypeImageListReport PCMModuleProductTypeImageListReport = new PCMModuleProductTypeImageListReport();
                        foreach (ProductTypeImage ProductTypeitem in ProductTypesDetails.ProductTypeImageList)
                        {
                            if (ProductTypeitem.ProductTypeImageInBytes != null)
                            {
                                var ms = new MemoryStream(ProductTypeitem.ProductTypeImageInBytes);
                                System.Drawing.Image image = Image.FromStream(ms);
                                Bitmap Bitmap = ResizeImage(new Bitmap(image));
                                //Bitmap Bitmap = (new Bitmap(image));
                                switch (ProductTypeitem.Position)
                                {
                                    #region 1TO3
                                    case 1:
                                        //Bitmap Bitmap = ResizeImage(new Bitmap(image));
                                        pcmPrintModuleReport.xrPictureBox1.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox1.WidthF = Bitmap.Width;

                                        //pcmPrintModuleReport.xrLabel5.Text = ProductTypeitem.Description;
                                        //pcmPrintModuleReport.xrPictureBox1.Image = image;
                                        break;
                                    case 2:

                                        pcmPrintModuleReport.xrPictureBox2.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        //pcmPrintModuleReport.xrLabel6.Text = ProductTypeitem.Description;
                                        break;
                                    case 3:
                                        pcmPrintModuleReport.xrPictureBox3.Image = Bitmap;
                                        pcmPrintModuleReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        pcmPrintModuleReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        // pcmPrintModuleReport.xrLabel7.Text = ProductTypeitem.Description;
                                        break;
                                    #endregion
                                    #region 4TO6
                                    case 4:
                                        //PCMModuleProductTypeImageListReport.xrPictureBox1.Image = ResizeImage(new Bitmap(image));
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel5.Text = ProductTypeitem.Description;
                                        break;
                                    case 5:
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel6.Text = ProductTypeitem.Description;
                                        break;
                                    case 6:
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel7.Text = ProductTypeitem.Description;
                                        break;
                                    #endregion
                                    #region 7to9
                                    case 7:
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel1.Text = ProductTypeitem.Description;
                                        RectangleF rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox1.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel1.BoundsF = rectangleF;
                                        RectangleF rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 8:
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel2.Text = ProductTypeitem.Description;

                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox2.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel2.BoundsF = rectangleF;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 9:
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.WidthF = Bitmap.Width;
                                        //PCMModuleProductTypeImageListReport.xrLabel3.Text = ProductTypeitem.Description;

                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox3.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        //PCMModuleProductTypeImageListReport.xrLabel3.BoundsF = rectangleF;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + ProductTypeitem.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                        #endregion
                                }
                            }
                        }
                        if (ProductTypesDetails.ProductTypeImageList.Count > 3)
                        {
                            pcmPrintModuleReport.xrImageList.ReportSource = PCMModuleProductTypeImageListReport;
                        }
                    }

                }
                catch (Exception ex) { }
                //pcmPrintModuleReport.DataSource = ProductTypesDetails;
                //PCMModuleDocumentationReport.objectDataSource1.DataSource = ProductTypesDetails;
                pcmPrintModuleReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                pcmPrintModuleReport.ExportOptions.Email.Subject = FileName;
                if (IsPCMPrintModuleAttachments)
                {
                    try
                    {
                        if (ProductTypesDetails.ProductTypeAttachedDocList != null && ProductTypesDetails.ProductTypeAttachedDocList.Count > 0)
                        {
                            XtraReport report = new XtraReport();
                            int count = 0;
                            List<PdfDocumentProcessor> pdfDocumentProcessorList = new List<PdfDocumentProcessor>();
                            foreach (ProductTypeAttachedDoc item in ProductTypesDetails.ProductTypeAttachedDocList)
                            {
                                try
                                {
                                    //using (MemoryStream stream = new MemoryStream())
                                    //{
                                    //    report.SaveLayout(stream);
                                    //}
                                    MemoryStream ProductTypeAttachedDocInBytesstream = new MemoryStream(item.ProductTypeAttachedDocInBytes);
                                    //File.WriteAllBytes("D:\\"+item.OriginalFileName + ".pdf", item.ProductTypeAttachedDocInBytes);
                                    XtraReport xtraReport = new XtraReport();
                                    // XtraReport.LoadLayout(ProductTypeAttachedDocInBytesstream);
                                    xtraReport = XtraReport.FromStream(ProductTypeAttachedDocInBytesstream, true);
                                    //xtraReport.LoadLayout("D:\\" + item.OriginalFileName + ".pdf");
                                    //pcmPrintModuleReport.xrRichText1.LoadFile(ProductTypeAttachedDocInBytesstream, XRRichTextStreamType.RtfText);
                                    if (xtraReport != null)
                                    {
                                        xtraReport.CreateDocument();
                                        report.ModifyDocument(x => x.AddPages(xtraReport.Pages));
                                    }
                                    PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
                                    //pdfDocumentProcessor.CreateEmptyDocument(ProductTypeAttachedDocInBytesstream);
                                    pdfDocumentProcessor.AppendDocument(ProductTypeAttachedDocInBytesstream);
                                    pdfDocumentProcessorList.Add(pdfDocumentProcessor);

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            int largestEdgeLength = 1000;
                            //if (ProductTypesDetails.MergePDFDocument == null)
                            ProductTypesDetails.MergePDFDocument = new List<Bitmap>();
 
                            foreach (PdfDocumentProcessor PdfDocumentProcessor in pdfDocumentProcessorList)
                            {
                                for (int i = 1; i <= PdfDocumentProcessor.Document.Pages.Count; i++)
                                {
                                    Bitmap BitmapImage = PdfDocumentProcessor.CreateBitmap(i, largestEdgeLength);
                                    var pb = new XRPictureBox
                                    {
                                        Image = BitmapImage,
                                    };
                                    ProductTypesDetails.MergePDFDocument.Add(BitmapImage);
                                    //ProductTypesDetails.MergePDF = BitmapImage;
                                }
                            }
                            for (int i = 0; i < ProductTypesDetails.MergePDFDocument.Count; i++)
                            {
                                pcmPrintModuleReport.SubBand1.Band.Controls.Add(new XRPictureBox
                                {
                                    //Image = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "Penguins.jpg")),
                                    Image = ProductTypesDetails.MergePDFDocument[i],
                                    Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze,
                                    Size = new System.Drawing.Size((ProductTypesDetails.MergePDFDocument[i].Width - 200), (ProductTypesDetails.MergePDFDocument[i].Height - 200)),
                                    LocationF = new PointF(0, i * 800)
                                });

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                //[GEOS2-6531][rdixit][07.01.2025]
                try
                {                   
                    var PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegions_V2530(ProductTypesDetails.IdCPType));
                    if (PCMCustomerList != null && PCMCustomerList.Count > 0)
                    {
                        IncludedCustomersList = PCMCustomerList.Where(i => i.IsIncluded == 1).ToList();
                        if (IncludedCustomersList != null && IncludedCustomersList.Count > 0)
                        {
                            IncludedCustomersReport includedCustomersReport = new IncludedCustomersReport();
                            includedCustomersReport.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                            includedCustomersReport.table2.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 7, System.Drawing.FontStyle.Regular);
                            includedCustomersReport.objectDataSource1.DataSource = IncludedCustomersList;
                            pcmPrintModuleReport.xrIncludedCustomerList.ReportSource = includedCustomersReport;
                            #region Customer
                            try
                            {
                                #region switch
                                switch (Language.TwoLetterISOLanguage.ToLower())
                                {
                                    case "en":
                                        includedCustomersReport.xrLblCustomer.Text = "Customers :";
                                        includedCustomersReport.tableCell1.Text = "Group";
                                        includedCustomersReport.tableCell2.Text = "Region";
                                        includedCustomersReport.tableCell3.Text = "Country";
                                        includedCustomersReport.tableCell4.Text = "Plant";
                                        break;
                                    case "es":
                                        includedCustomersReport.xrLblCustomer.Text = "Clientes :";
                                        includedCustomersReport.tableCell1.Text = "Grupo";
                                        includedCustomersReport.tableCell2.Text = "Región";
                                        includedCustomersReport.tableCell3.Text = "País";
                                        includedCustomersReport.tableCell4.Text = "Planta";
                                        break;
                                    case "fr":
                                        includedCustomersReport.xrLblCustomer.Text = "Clients :";
                                        includedCustomersReport.tableCell1.Text = "Groupe";
                                        includedCustomersReport.tableCell2.Text = "Région";
                                        includedCustomersReport.tableCell3.Text = "Pays";
                                        includedCustomersReport.tableCell4.Text = "Usine";
                                        break;
                                    case "pt":
                                        includedCustomersReport.xrLblCustomer.Text = "Clientes :";
                                        includedCustomersReport.tableCell1.Text = "Grupo";
                                        includedCustomersReport.tableCell2.Text = "Região";
                                        includedCustomersReport.tableCell3.Text = "País";
                                        includedCustomersReport.tableCell4.Text = "Planta";
                                        break;
                                    case "ro":
                                        includedCustomersReport.xrLblCustomer.Text = "Clienți :";
                                        includedCustomersReport.tableCell1.Text = "Grup";
                                        includedCustomersReport.tableCell2.Text = "Regiune";
                                        includedCustomersReport.tableCell3.Text = "Țară";
                                        includedCustomersReport.tableCell4.Text = "Fabrică";
                                        break;
                                    case "ru":
                                        includedCustomersReport.xrLblCustomer.Text = "Клиенты :";
                                        includedCustomersReport.tableCell1.Text = "Группа";
                                        includedCustomersReport.tableCell2.Text = "Регион";
                                        includedCustomersReport.tableCell3.Text = "Страна";
                                        includedCustomersReport.tableCell4.Text = "Завод";
                                        break;
                                    case "zh":
                                        includedCustomersReport.xrLblCustomer.Text = "客户 :";
                                        includedCustomersReport.tableCell1.Text = "组";
                                        includedCustomersReport.tableCell2.Text = "地区";
                                        includedCustomersReport.tableCell3.Text = "国家";
                                        includedCustomersReport.tableCell4.Text = "工厂";
                                        break;
                                    default:
                                        includedCustomersReport.xrLblCustomer.Text = "Customers :";
                                        includedCustomersReport.tableCell1.Text = "Group";
                                        includedCustomersReport.tableCell2.Text = "Region";
                                        includedCustomersReport.tableCell3.Text = "Country";
                                        includedCustomersReport.tableCell4.Text = "Plant";
                                        break;
                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                            }

                            #endregion
                        }
                    }
                }
                catch { }
                #region Watermark
                if (!ProductTypesDetails.Status.Value.ToLower().Equals("Active".ToLower()))
                {
                    pcmPrintModuleReport.Watermark.ShowBehind = true;
                    pcmPrintModuleReport.Watermark.Text = ProductTypesDetails.Status.Value;
                    pcmPrintModuleReport.Watermark.Font = new Font(pcmPrintModuleReport.Watermark.Font.FontFamily, 150);
                    pcmPrintModuleReport.Watermark.ForeColor = System.Drawing.Color.Gray;
                    pcmPrintModuleReport.Watermark.TextTransparency = 150;
                }
                #endregion
                //DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                //window.PreviewControl.DocumentSource = pcmPrintModuleReport;
                //pcmPrintModuleReport.CreateDocument();
                //window.Show();
                DevExpress.XtraPrinting.PdfExportOptions options = new DevExpress.XtraPrinting.PdfExportOptions();
                using (FileStream pdfFileStream = new FileStream(AnnouncementFolderName +FileName + "_datasheet.pdf", FileMode.Create))
                {
                    pcmPrintModuleReport.ExportToPdf(pdfFileStream, options);
                }
                AnnouncementFileName = FileName + "_datasheet.pdf";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandWindow()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
        private byte[] GetLanguagesImage(string Code)
        {
            try
            {
                string ProfileImagePath = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/Images/Languages/" + Code + ".png";
                byte[] ImageBytes = null;
                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage()...", category: Category.Info, priority: Priority.Low);
                try
                {

                    if (GeosApplication.ImageUrlBytePair == null)
                        GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                    if (GeosApplication.ImageUrlBytePair.Any(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()))
                    {
                        ImageBytes = GeosApplication.ImageUrlBytePair.FirstOrDefault(i => i.Key.ToString().ToLower() == ProfileImagePath.ToLower()).Value;
                    }
                    else
                    {
                        if (GeosApplication.IsImageURLException == false)
                        {
                            //using (WebClient webClient = new WebClient())
                            //{
                            //    ImageBytes = webClient.DownloadData(ProfileImagePath);
                            //}
                            ImageBytes = Utility.ImageUtil.GetImageByWebClient(ProfileImagePath);
                        }
                        else
                        {
                            ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                        }
                        if (ImageBytes.Length > 0)
                        {
                            GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                            return ImageBytes;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (GeosApplication.IsImageURLException == false)
                        GeosApplication.IsImageURLException = true;

                    if (!string.IsNullOrEmpty(ProfileImagePath))
                    {
                        ImageBytes = GeosRepositoryServiceController.GetImagesByUrl(ProfileImagePath);
                        GeosApplication.ImageUrlBytePair.Add(ProfileImagePath, ImageBytes);
                    }

                }

                GeosApplication.Instance.Logger.Log("Method GetEmployeesImage()....executed successfully", category: Category.Info, priority: Priority.Low);
                return ImageBytes;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetEmployeesImage()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }
        #endregion
    }
}