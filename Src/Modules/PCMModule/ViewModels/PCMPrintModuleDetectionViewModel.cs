using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Pdf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraSpreadsheet.TileLayout.View;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Modules.PCM.Reports;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.PCM.ViewModels
{//[Sudhir.Jangra][GEOS2-4091][24/01/2023]
    public class PCMPrintModuleDetectionViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose; // for close window

        #endregion  // Events

        #region Declaration
        List<CPLCustomer> includedCustomersList;
        private List<Language> languages;
        string language;
        private int settingWindowLanguageSelectedIndex;
        private bool isPCMPrintModuleDetectionAttachments;
        private bool isPCMPrintModuleDetectionLinks;
        private ProductTypes productTypesDetails;
        private DetectionDetails detectionDetails;

        #endregion

        #region Properties
        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
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
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public int SettingWindowLanguageSelectedIndex
        {
            get { return settingWindowLanguageSelectedIndex; }
            set
            {
                settingWindowLanguageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SettingWindowLanguageSelectedIndex"));
            }
        }
        public bool IsPCMPrintModuleDetectionAttachments
        {
            get { return isPCMPrintModuleDetectionAttachments; }
            set
            {
                isPCMPrintModuleDetectionAttachments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPCMPrintModuleDetectionAttachments"));
            }
        }
        public bool IsPCMPrintModuleDetectionLinks
        {
            get { return isPCMPrintModuleDetectionLinks; }
            set
            {
                isPCMPrintModuleDetectionLinks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPCMPrintModuleDetectionLinks"));
            }
        }
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
        public DetectionDetails DetectionDetails
        {
            get { return detectionDetails; }
            set
            {
                detectionDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionDetails"));
            }
        }

        private ObservableCollection<DetectionOrderGroup> orderGroupList;
        public ObservableCollection<DetectionOrderGroup> OrderGroupList
        {
            get
            {
                return orderGroupList;
            }

            set
            {
                orderGroupList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("OrderGroupList"));
            }
        }
        private List<DetectiontypesInformations> detectionTypes;
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
        public ICommand PCMPrintModuleDetectionViewCancelButtonCommand { get; set; }
        public ICommand PCMPrintModuleDetectionViewAcceptButtonCommand { get; set; }
        #endregion
        #region Constructor
        public PCMPrintModuleDetectionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PCMPrintModuleDetectionViewModel()...", category: Category.Info, priority: Priority.Low);
                FillLanguage();
                IsPCMPrintModuleDetectionAttachments = true;
                IsPCMPrintModuleDetectionLinks = true;
                PCMPrintModuleDetectionViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PCMPrintModuleDetectionViewAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandWindow));
                // SetLanguageDictionary();
                //  DetectionTypes = new List<DetectiontypesInformations>();
                //PCMService = new PCMServiceController("localhost:6699");
                //  DetectionTypes = PCMService.GetDetectionTypes();
                DetectionTypes = new List<DetectiontypesInformations>();
                DetectionTypes = PCMService.GetDetectionTypes();
                GeosApplication.Instance.Logger.Log("Constructor Constructor PCMPrintModuleDetectionViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor PCMPrintModuleDetectionViewModel() Method - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in  Constructor PCMPrintModuleDetectionViewModel() Method - ServiceUnexceptedException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Constructor PCMPrintModuleDetectionViewModel()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
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
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
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
                string FileName = DetectionDetails.Name;
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                string DetectionDatesheet = DetectionDetails.DetectionTypes.Name + " " + "Datasheet";
                DetectiontypesInformations Detectiontypes = DetectionTypes.Find(f => f.IdDetectionType == DetectionDetails.DetectionTypes.IdDetectionType);
                Language Language = Languages[settingWindowLanguageSelectedIndex];
                PCMPrintModuleDetectionReport pcmPrintModuleDetectionReport = new PCMPrintModuleDetectionReport();

                //pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = DetectionDatesheet;
                pcmPrintModuleDetectionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrDetectionDatesheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 18, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrNameText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrReviewDateText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrDescriptionText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrFamilyText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.PCM.Properties.Resources.Emdep_logo_mini);
                pcmPrintModuleDetectionReport.imgLogo.Image = BitmapimgLogo;
                pcmPrintModuleDetectionReport.imgLogo.Height = BitmapimgLogo.Height;
                pcmPrintModuleDetectionReport.imgLogo.WidthF = BitmapimgLogo.Width + 10;

                pcmPrintModuleDetectionReport.xrLblEmdep.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrEmailText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrPhoneText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrlblWebSite.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                try
                {
                    #region switch
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail"].ToString();
                            #endregion
                            break;
                        case "es":
                            #region es
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_es + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_es"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_es"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_es"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_es"].ToString();
                            #endregion
                            break;

                        case "fr":
                            #region fr
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_fr + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_fr"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_fr"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_fr"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_fr"].ToString();
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_pt + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_pt"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_pt"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_pt"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_pt"].ToString();
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_ro + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_ro"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_ro"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_ro"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_ro"].ToString();
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_ru + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_ru"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_ru"].ToString();
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 16, System.Drawing.FontStyle.Bold);
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_ru"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_ru"].ToString();
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name_zh + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet_zh"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES_zh"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone_zh"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail_zh"].ToString();
                            #endregion
                            break;
                        default:
                            #region default
                            pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = Detectiontypes.Name + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet"].ToString();
                            pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES"].ToString();
                            pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone"].ToString();
                            pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail"].ToString();
                            #endregion
                            break;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    DetectionDatesheet = Detectiontypes.Name + " " + Application.Current.Resources["PCMPrintDetectionReportModuleDatasheet"].ToString();
                    pcmPrintModuleDetectionReport.xrLabel1.Text = Application.Current.Resources["PCMPrintDetectionReportSUPPORTEDMODULES"].ToString();
                    pcmPrintModuleDetectionReport.xrlblPhone.Text = Application.Current.Resources["PCMPrintDetectionReportPhone"].ToString();
                    pcmPrintModuleDetectionReport.xrlblEmail.Text = Application.Current.Resources["PCMPrintDetectionReportEmail"].ToString();
                }

                #region DetectionDetails
                try
                {
                    #region switch
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Group :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate"].ToString();
                            #endregion
                            break;
                        case "es":
                            #region es
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_es;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_es;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_es"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_es"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Grupo :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_es"].ToString();
                            #endregion
                            break;
                        case "fr":
                            #region fr
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_fr;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_fr;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_fr"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_fr"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Groupe :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_fr"].ToString();
                            #endregion
                            break;
                        case "pt":
                            #region pt
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_pt;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_pt;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_pt"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_pt"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Grupo :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_pt"].ToString();
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_ro;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_ro;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ro"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ro"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Grup :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ro"].ToString();
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_ru;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_ru;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_ru"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_ru"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "группа :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_ru"].ToString();
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_zh;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_zh;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName_zh"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription_zh"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "组 :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate_zh"].ToString();
                            #endregion
                            break;
                        default:
                            #region default
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description;
                            pcmPrintModuleDetectionReport.xrLblName.Text = Application.Current.Resources["PCMPrintModuleReportName"].ToString();
                            pcmPrintModuleDetectionReport.xrLblDescription.Text = Application.Current.Resources["PCMPrintModuleReportDescription"].ToString();
                            pcmPrintModuleDetectionReport.xrLblFamily.Text = "Group :";
                            pcmPrintModuleDetectionReport.xrLblReviewDate.Text = Application.Current.Resources["PCMPrintModuleReportReviewDate"].ToString();
                            #endregion
                            break;
                    }
                    #endregion
                    #region xrFamilyText
                    //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                    try
                    {
                        if (OrderGroupList == null)
                        {
                            OrderGroupList = new ObservableCollection<DetectionOrderGroup>();
                            // GroupList = new ObservableCollection<DetectionGroup>();
                        }
                        OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(DetectionDetails.IdDetectionType));
                        pcmPrintModuleDetectionReport.xrFamilyText.Text = OrderGroupList.FirstOrDefault(x => x.IdGroup == DetectionDetails.IdGroup).Name;
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                    if (DetectionDetails.LastUpdate == null)
                    {
                        pcmPrintModuleDetectionReport.xrReviewDateText.Text = DetectionDetails.CreatedIn.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        pcmPrintModuleDetectionReport.xrReviewDateText.Text = DetectionDetails.LastUpdate.ToString("dd/MM/yyyy");
                    }

                }
                catch (Exception ex)
                {
                }

                #endregion
                #region IncludedCustomerList [GEOS2-6531][rdixit][07.01.2025]

                if (IncludedCustomersList != null && IncludedCustomersList.Count > 0)
                {
                    IncludedCustomersReport includedCustomersReport = new IncludedCustomersReport();
                    includedCustomersReport.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    includedCustomersReport.table2.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 7, System.Drawing.FontStyle.Regular);
                    includedCustomersReport.objectDataSource1.DataSource = IncludedCustomersList;
                    pcmPrintModuleDetectionReport.xrIncludedCustomerList.ReportSource = includedCustomersReport;
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


                #endregion
                #region ProductTypeAttachedLinkList
                if (IsPCMPrintModuleDetectionLinks)
                {
                    if (DetectionDetails.DetectionAttachedLinkList != null && DetectionDetails.DetectionAttachedLinkList.Count > 0)
                    {
                        PCMModuleReferencesReport PCMModuleReferencesReport = new PCMModuleReferencesReport();
                        #region LINKS
                        //Shubham[skadam] GEOS2-5024 Improvements in PCM module 22 12 2023
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

                        PCMModuleReferencesReport.objectDataSource1.DataSource = DetectionDetails.DetectionAttachedLinkList;
                        pcmPrintModuleDetectionReport.xrLinkList.ReportSource = PCMModuleReferencesReport;

                    }
                }

                #endregion
                #region DetectionAttachedLinkList
                //if (IsPCMPrintModuleDetectionLinks)
                //{
                //    if (DetectionDetails.DetectionAttachedLinkList != null && DetectionDetails.DetectionAttachedLinkList.Count > 0)
                //    {
                //        PCMModuleDetectionListReport PCMModuleDetectionReport = new PCMModuleDetectionListReport();
                //        PCMModuleDetectionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                //        PCMModuleDetectionReport.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                //        PCMModuleDetectionReport.objectDataSource1.DataSource = DetectionDetails.DetectionAttachedLinkList;
                //        pcmPrintModuleDetectionReport.xrLinkList.ReportSource = PCMModuleDetectionReport;

                //    }
                //}

                #endregion

                #region ProductTypeAttachedDocList
                if (IsPCMPrintModuleDetectionAttachments)
                {
                    if (DetectionDetails.DetectionAttachedDocList != null && DetectionDetails.DetectionAttachedDocList.Count > 0)
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

                        PCMModuleDocumentationReport.objectDataSource1.DataSource = DetectionDetails.DetectionAttachedDocList;
                        pcmPrintModuleDetectionReport.xrDocList.ReportSource = PCMModuleDocumentationReport;
                    }
                }
                #endregion

                #region Related Modules
                if (DetectionDetails.ProductTypesList != null && DetectionDetails.ProductTypesList.Count > 0)
                {
                    try
                    {
                        PCMDetectionRelatedModulesListReport PCMDetectionRelatedModule = new PCMDetectionRelatedModulesListReport();
                        PCMDetectionRelatedModule.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMDetectionRelatedModule.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        PCMDetectionRelatedModule.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        //PCMDetectionRelatedModule.xrRelatedModuleText.Text = "●" + "   " + itemTemplate;
                        PCMDetectionRelatedModule.xrRichText1.Text = "";
                        List<Template> Template = DetectionDetails.ProductTypesList.Select(s => s.Template).Distinct().ToList();
                        List<string> TemplateName = Template.Select(s => s.Name).Distinct().ToList();
                        string xrRichTextString = string.Empty;
                        foreach (string itemTemplate in TemplateName)
                        {
                            xrRichTextString = xrRichTextString + "\t\t●" + "   " + "<b>" + itemTemplate + "</b>" + "\n";
                            //PCMDetectionRelatedModule.xrRichText1.Html= xrRichTextString+"\t\t●" + "   <b>" + itemTemplate + "</b> \n";

                            foreach (var relatedModule in DetectionDetails.ProductTypesList.Where(w => w.Template.Name == itemTemplate))
                            {
                                #region switch
                                switch (Language.TwoLetterISOLanguage.ToLower())
                                {
                                    case "en":
                                        #region en
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name + "\n";
                                        #endregion
                                        break;
                                    case "es":
                                        #region es
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_es + "\n";
                                        #endregion
                                        break;

                                    case "fr":
                                        #region fr
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_fr + "\n";
                                        #endregion
                                        break;

                                    case "pt":
                                        #region pt
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_pt + "\n";
                                        #endregion
                                        break;
                                    case "ro":
                                        #region ro
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_ro + "\n";
                                        #endregion
                                        break;
                                    case "ru":
                                        #region ru
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_ru + "\n";
                                        #endregion
                                        break;
                                    case "zh":
                                        #region zh
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_zh + "\n";
                                        #endregion
                                        break;
                                    default:
                                        #region default
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name + "\n";
                                        #endregion
                                        break;
                                }
                                #endregion
                            }


                        }
                        PCMDetectionRelatedModule.xrLabel1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        PCMDetectionRelatedModule.xrLabel1.AllowMarkupText = true;
                        PCMDetectionRelatedModule.xrLabel1.Text = xrRichTextString.Replace("\t", "<nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp>");
                        //PCMDetectionRelatedModule.xrRichText1.Text = xrRichTextString;
                        //PCMDetectionRelatedModule.xrRelatedModuleText.Text = xrRichTextString;
                        pcmPrintModuleDetectionReport.xrProductList.ReportSource = PCMDetectionRelatedModule;
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                #region GetAllCompanyinfo
                try
                {
                    dynamic d = (dynamic)GeosApplication.Instance.SelectedPlantOwnerUsersList;
                    Emdep.Geos.Data.Common.Company company = (Company)d[0];
                    Company GetAllCompanyinfo = CrmStartUp.GetCompanyDetailsById(company.IdCompany);
                    pcmPrintModuleDetectionReport.xrLblEmdep.Text = GetAllCompanyinfo.RegisteredName.ToString();
                    pcmPrintModuleDetectionReport.xrEmailText.Text = GetAllCompanyinfo.Email.ToString();
                    pcmPrintModuleDetectionReport.xrPhoneText.Text = GetAllCompanyinfo.Telephone.ToString();
                    pcmPrintModuleDetectionReport.xrlblWebSite.Text = GetAllCompanyinfo.Website.ToString();

                }
                catch (Exception ex)
                {
                }
                #endregion
                try
                {
                    if (DetectionDetails.DetectionImageList.Count == 0)
                    {
                        pcmPrintModuleDetectionReport.xrPictureBox2.Visible = false;
                        pcmPrintModuleDetectionReport.xrPictureBox3.Visible = false;
                        pcmPrintModuleDetectionReport.xrPictureBox4.Visible = false;
                    }
                    if (DetectionDetails.DetectionImageList != null && DetectionDetails.DetectionImageList.Count > 0)
                    {
                        PCMModuleProductTypeImageListReport PCMModuleProductTypeImageListReport = new PCMModuleProductTypeImageListReport();
                        foreach (DetectionImage item in DetectionDetails.DetectionImageList)
                        {
                            if (item.DetectionImageInBytes != null)
                            {
                                var ms = new MemoryStream(item.DetectionImageInBytes);
                                System.Drawing.Image image = Image.FromStream(ms);
                                Bitmap Bitmap = ResizeImage(new Bitmap(image));

                                switch (item.Position)
                                {
                                    #region 1TO3
                                    case 1:
                                        pcmPrintModuleDetectionReport.xrPictureBox2.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        break;
                                    case 2:

                                        pcmPrintModuleDetectionReport.xrPictureBox3.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        break;
                                    case 3:
                                        pcmPrintModuleDetectionReport.xrPictureBox4.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        break;
                                    #endregion
                                    #region 4TO6
                                    case 4:
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.WidthF = Bitmap.Width;
                                        break;
                                    case 5:
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        break;
                                    case 6:
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        break;
                                    #endregion
                                    #region 7to9
                                    case 7:
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        RectangleF rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox1.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        RectangleF rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 8:
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.WidthF = Bitmap.Width;
                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox2.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 9:
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.WidthF = Bitmap.Width;
                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox3.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                        #endregion
                                }
                            }
                        }
                        if (DetectionDetails.DetectionImageList.Count > 3)
                        {
                            pcmPrintModuleDetectionReport.xrImageList.ReportSource = PCMModuleProductTypeImageListReport;
                        }
                    }

                }
                catch (Exception ex) { }

                pcmPrintModuleDetectionReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                pcmPrintModuleDetectionReport.ExportOptions.Email.Subject = FileName;

                if (IsPCMPrintModuleDetectionAttachments)
                {
                    try
                    {
                        if (DetectionDetails.DetectionAttachedDocList != null && DetectionDetails.DetectionAttachedDocList.Count > 0)
                        {
                            XtraReport report = new XtraReport();
                            int count = 0;
                            List<PdfDocumentProcessor> pdfDocumentProcessorList = new List<PdfDocumentProcessor>();
                            foreach (DetectionAttachedDoc item in DetectionDetails.DetectionAttachedDocList)
                            {
                                try
                                {
                                    MemoryStream DetectionAttachedDocInBytesstream = new MemoryStream(item.DetectionAttachedDocInBytes);

                                    XtraReport xtraReport = new XtraReport();
                                    xtraReport = XtraReport.FromStream(DetectionAttachedDocInBytesstream, true);
                                    if (xtraReport != null)
                                    {
                                        xtraReport.CreateDocument();
                                        report.ModifyDocument(x => x.AddPages(xtraReport.Pages));
                                    }
                                    PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
                                    pdfDocumentProcessor.AppendDocument(DetectionAttachedDocInBytesstream);
                                    pdfDocumentProcessorList.Add(pdfDocumentProcessor);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            int largestEdgeLength = 1000;
                            DetectionDetails.MergePDFDocument = new List<Bitmap>();
                            foreach (PdfDocumentProcessor PdfDocumentProcessor in pdfDocumentProcessorList)
                            {
                                for (int i = 1; i <= PdfDocumentProcessor.Document.Pages.Count; i++)
                                {
                                    Bitmap BitmapImage = PdfDocumentProcessor.CreateBitmap(i, largestEdgeLength);
                                    var pb = new XRPictureBox
                                    {
                                        Image = BitmapImage,
                                    };
                                    DetectionDetails.MergePDFDocument.Add(BitmapImage);
                                }
                            }
                            for (int i = 0; i < DetectionDetails.MergePDFDocument.Count; i++)
                            {
                                pcmPrintModuleDetectionReport.SubBand1.Band.Controls.Add(new XRPictureBox
                                {
                                    Image = DetectionDetails.MergePDFDocument[i],
                                    Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze,
                                    Size = new System.Drawing.Size((DetectionDetails.MergePDFDocument[i].Width - 200), (DetectionDetails.MergePDFDocument[i].Height - 200)),
                                    LocationF = new PointF(0, i * 800)
                                });

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                #region Watermark
                if (!DetectionDetails.Status.Value.ToLower().Equals("Active".ToLower()))
                {
                    pcmPrintModuleDetectionReport.Watermark.ShowBehind = true;
                    pcmPrintModuleDetectionReport.Watermark.Text = DetectionDetails.Status.Value;
                    pcmPrintModuleDetectionReport.Watermark.Font = new Font(pcmPrintModuleDetectionReport.Watermark.Font.FontFamily, 150);
                    pcmPrintModuleDetectionReport.Watermark.ForeColor = System.Drawing.Color.Gray;
                    pcmPrintModuleDetectionReport.Watermark.TextTransparency = 150;
                }
                #endregion
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcmPrintModuleDetectionReport;
                pcmPrintModuleDetectionReport.CreateDocument();
                window.Show();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandWindow()." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
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
                string FileName = DetectionDetails.Name;
                //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                string DetectionDatesheet = DetectionDetails.DetectionTypes.Name + " " + "Datasheet";
                PCMPrintModuleDetectionReport pcmPrintModuleDetectionReport = new PCMPrintModuleDetectionReport();
                pcmPrintModuleDetectionReport.xrDetectionDatesheet.Text = DetectionDatesheet;
                pcmPrintModuleDetectionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrDetectionDatesheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrNameText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrReviewDateText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrDescriptionText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrFamilyText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);              
                Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.PCM.Properties.Resources.Emdep_logo_mini);
                pcmPrintModuleDetectionReport.imgLogo.Image = BitmapimgLogo;
                pcmPrintModuleDetectionReport.imgLogo.Height = BitmapimgLogo.Height;
                pcmPrintModuleDetectionReport.imgLogo.WidthF = BitmapimgLogo.Width;

                pcmPrintModuleDetectionReport.xrLblEmdep.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                pcmPrintModuleDetectionReport.xrEmailText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrPhoneText.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                pcmPrintModuleDetectionReport.xrlblWebSite.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                Language Language = Languages[settingWindowLanguageSelectedIndex];
                #region DetectionDetails
                try
                {
                    #region switch
                    switch (Language.TwoLetterISOLanguage.ToLower())
                    {
                        case "en":
                            #region en
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description;
                            #endregion
                            break;
                        case "es":
                            #region es
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_es;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_es;
                            #endregion
                            break;

                        case "fr":
                            #region es
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_fr;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_fr;
                            #endregion
                            break;

                        case "pt":
                            #region pt
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_pt;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_pt;
                            #endregion
                            break;
                        case "ro":
                            #region ro
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_ro;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_ro;
                            #endregion
                            break;
                        case "ru":
                            #region ru
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_ru;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_ru;
                            #endregion
                            break;
                        case "zh":
                            #region zh
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name_zh;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description_zh;
                            #endregion
                            break;
                        default:
                            #region default
                            pcmPrintModuleDetectionReport.xrNameText.Text = DetectionDetails.Name;
                            pcmPrintModuleDetectionReport.xrDescriptionText.Text = DetectionDetails.Description;
                            #endregion
                            break;
                    }
                    #endregion
                    #region xrFamilyText
                    //Shubham[skadam] GEOS2-4091 Add option in PCM to print a datasheet of a Module [2 of 3]  06 02 2023
                    try
                    {
                        if (OrderGroupList == null)
                        {
                            OrderGroupList = new ObservableCollection<DetectionOrderGroup>();
                            // GroupList = new ObservableCollection<DetectionGroup>();
                        }
                        OrderGroupList = new ObservableCollection<DetectionOrderGroup>(PCMService.GetDetectionOrderGroupsWithDetections(DetectionDetails.IdDetectionType));
                        pcmPrintModuleDetectionReport.xrFamilyText.Text = OrderGroupList.FirstOrDefault(x => x.IdGroup == DetectionDetails.IdGroup)?.Name;
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                    if (DetectionDetails.LastUpdate == null)
                    {
                        pcmPrintModuleDetectionReport.xrReviewDateText.Text = DetectionDetails.CreatedIn.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        pcmPrintModuleDetectionReport.xrReviewDateText.Text = DetectionDetails.LastUpdate.ToString("dd/MM/yyyy");
                    }

                }
                catch (Exception ex)
                {
                }

                #endregion
                #region IncludedCustomerList [GEOS2-6531][rdixit][07.01.2025]
                try
                {
                    var PCMCustomerList = new List<CPLCustomer>(PCMService.GetCustomersWithRegionsByIdDetection_V2280(DetectionDetails.IdDetections));
                    if (PCMCustomerList != null && PCMCustomerList.Count > 0)
                    {
                        IncludedCustomersList = PCMCustomerList.Where(i => i.IsIncluded == 1).ToList();
                        if (IncludedCustomersList != null && IncludedCustomersList.Count > 0)
                        {
                            IncludedCustomersReport includedCustomersReport = new IncludedCustomersReport();
                            includedCustomersReport.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                            includedCustomersReport.table2.Font = new Font(Application.Current.MainWindow.FontFamily.Source, 7, System.Drawing.FontStyle.Regular);
                            includedCustomersReport.objectDataSource1.DataSource = IncludedCustomersList;
                            pcmPrintModuleDetectionReport.xrIncludedCustomerList.ReportSource = includedCustomersReport;
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
                #endregion

                #region ProductTypeAttachedLinkList
                if (IsPCMPrintModuleDetectionLinks)
                {
                    if (DetectionDetails.DetectionAttachedLinkList != null && DetectionDetails.DetectionAttachedLinkList.Count > 0)
                    {
                        PCMModuleReferencesReport PCMModuleReferencesReport = new PCMModuleReferencesReport();
                        PCMModuleReferencesReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleReferencesReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleReferencesReport.objectDataSource1.DataSource = DetectionDetails.DetectionAttachedLinkList;
                        pcmPrintModuleDetectionReport.xrLinkList.ReportSource = PCMModuleReferencesReport;

                    }
                }
                #endregion


                #region ProductTypeAttachedDocList
                if (IsPCMPrintModuleDetectionAttachments)
                {
                    if (DetectionDetails.DetectionAttachedDocList != null && DetectionDetails.DetectionAttachedDocList.Count > 0)
                    {
                        PCMModuleDocumentationReport PCMModuleDocumentationReport = new PCMModuleDocumentationReport();
                        PCMModuleDocumentationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMModuleDocumentationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                        PCMModuleDocumentationReport.objectDataSource1.DataSource = DetectionDetails.DetectionAttachedDocList;
                        pcmPrintModuleDetectionReport.xrDocList.ReportSource = PCMModuleDocumentationReport;
                    }
                }
                #endregion

                #region Related Modules
                if (DetectionDetails.ProductTypesList != null && DetectionDetails.ProductTypesList.Count > 0)
                {
                    try
                    {
                        PCMDetectionRelatedModulesListReport PCMDetectionRelatedModule = new PCMDetectionRelatedModulesListReport();
                        PCMDetectionRelatedModule.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                        PCMDetectionRelatedModule.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        PCMDetectionRelatedModule.xrRichText1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        //PCMDetectionRelatedModule.xrRelatedModuleText.Text = "●" + "   " + itemTemplate;
                        PCMDetectionRelatedModule.xrRichText1.Text = "";
                        List<Template> Template = DetectionDetails.ProductTypesList.Select(s => s.Template).Distinct().ToList();
                        List<string> TemplateName = Template.Select(s => s.Name).Distinct().ToList();
                        string xrRichTextString = string.Empty;
                        foreach (string itemTemplate in TemplateName)
                        {
                            xrRichTextString = xrRichTextString + "\t\t●" + "   " + "<b>" + itemTemplate + "</b>" + "\n";
                            //PCMDetectionRelatedModule.xrRichText1.Html= xrRichTextString+"\t\t●" + "   <b>" + itemTemplate + "</b> \n";

                            foreach (var relatedModule in DetectionDetails.ProductTypesList.Where(w => w.Template.Name == itemTemplate))
                            {
                                #region switch
                                switch (Language.TwoLetterISOLanguage.ToLower())
                                {
                                    case "en":
                                        #region en
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name + "\n";
                                        #endregion
                                        break;
                                    case "es":
                                        #region es
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_es + "\n";
                                        #endregion
                                        break;

                                    case "fr":
                                        #region fr
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_fr + "\n";
                                        #endregion
                                        break;

                                    case "pt":
                                        #region pt
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_pt + "\n";
                                        #endregion
                                        break;
                                    case "ro":
                                        #region ro
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_ro + "\n";
                                        #endregion
                                        break;
                                    case "ru":
                                        #region ru
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_ru + "\n";
                                        #endregion
                                        break;
                                    case "zh":
                                        #region zh
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name_zh + "\n";
                                        #endregion
                                        break;
                                    default:
                                        #region default
                                        xrRichTextString = xrRichTextString + "\t\t\t" + "\u006F" + "   " + relatedModule.Name + "\n";
                                        #endregion
                                        break;
                                }
                                #endregion
                            }


                        }
                        PCMDetectionRelatedModule.xrLabel1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                        PCMDetectionRelatedModule.xrLabel1.AllowMarkupText = true;
                        PCMDetectionRelatedModule.xrLabel1.Text = xrRichTextString.Replace("\t", "<nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp><nbsp>");
                        //PCMDetectionRelatedModule.xrRichText1.Text = xrRichTextString;
                        //PCMDetectionRelatedModule.xrRelatedModuleText.Text = xrRichTextString;
                        pcmPrintModuleDetectionReport.xrProductList.ReportSource = PCMDetectionRelatedModule;
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                #region GetAllCompanyinfo
                try
                {
                    dynamic d = (dynamic)GeosApplication.Instance.SelectedPlantOwnerUsersList;
                    Emdep.Geos.Data.Common.Company company = (Company)d[0];
                    Company GetAllCompanyinfo = CrmStartUp.GetCompanyDetailsById(company.IdCompany);
                    pcmPrintModuleDetectionReport.xrLblEmdep.Text = GetAllCompanyinfo.RegisteredName.ToString();
                    pcmPrintModuleDetectionReport.xrEmailText.Text = GetAllCompanyinfo.Email.ToString();
                    pcmPrintModuleDetectionReport.xrPhoneText.Text = GetAllCompanyinfo.Telephone.ToString();
                    pcmPrintModuleDetectionReport.xrlblWebSite.Text = GetAllCompanyinfo.Website.ToString();

                }
                catch (Exception ex)
                {
                }
                #endregion
                try
                {
                    if (DetectionDetails.DetectionImageList.Count == 0)
                    {
                        pcmPrintModuleDetectionReport.xrPictureBox2.Visible = false;
                        pcmPrintModuleDetectionReport.xrPictureBox3.Visible = false;
                        pcmPrintModuleDetectionReport.xrPictureBox4.Visible = false;
                    }
                    if (DetectionDetails.DetectionImageList != null && DetectionDetails.DetectionImageList.Count > 0)
                    {
                        PCMModuleProductTypeImageListReport PCMModuleProductTypeImageListReport = new PCMModuleProductTypeImageListReport();
                        foreach (DetectionImage item in DetectionDetails.DetectionImageList)
                        {
                            if (item.DetectionImageInBytes != null)
                            {
                                var ms = new MemoryStream(item.DetectionImageInBytes);
                                System.Drawing.Image image = Image.FromStream(ms);
                                Bitmap Bitmap = ResizeImage(new Bitmap(image));

                                switch (item.Position)
                                {
                                    #region 1TO3
                                    case 1:
                                        pcmPrintModuleDetectionReport.xrPictureBox2.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        break;
                                    case 2:

                                        pcmPrintModuleDetectionReport.xrPictureBox3.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        break;
                                    case 3:
                                        pcmPrintModuleDetectionReport.xrPictureBox4.Image = Bitmap;
                                        pcmPrintModuleDetectionReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        pcmPrintModuleDetectionReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        break;
                                    #endregion
                                    #region 4TO6
                                    case 4:
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox1.WidthF = Bitmap.Width;
                                        break;
                                    case 5:
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox2.WidthF = Bitmap.Width;
                                        break;
                                    case 6:
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox3.WidthF = Bitmap.Width;
                                        break;
                                    #endregion
                                    #region 7to9
                                    case 7:
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.WidthF = Bitmap.Width;
                                        RectangleF rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox1.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        RectangleF rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox4.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 8:
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.WidthF = Bitmap.Width;
                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox2.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox5.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                    case 9:
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.Image = Bitmap;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.HeightF = Bitmap.Height;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.WidthF = Bitmap.Width;
                                        rectangleF = new RectangleF();
                                        rectangleF = PCMModuleProductTypeImageListReport.xrPictureBox3.BoundsF;
                                        rectangleF.Y = rectangleF.Y + 200;
                                        rectangleFxrPictureBox4 = new RectangleF();
                                        rectangleFxrPictureBox4 = rectangleF;
                                        rectangleFxrPictureBox4.Y = rectangleFxrPictureBox4.Y + item.Description.Length;
                                        PCMModuleProductTypeImageListReport.xrPictureBox6.BoundsF = rectangleFxrPictureBox4;
                                        break;
                                        #endregion
                                }
                            }
                        }
                        if (DetectionDetails.DetectionImageList.Count > 3)
                        {
                            pcmPrintModuleDetectionReport.xrImageList.ReportSource = PCMModuleProductTypeImageListReport;
                        }
                    }

                }
                catch (Exception ex) { }

                pcmPrintModuleDetectionReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                pcmPrintModuleDetectionReport.ExportOptions.Email.Subject = FileName;

                if (IsPCMPrintModuleDetectionAttachments)
                {
                    try
                    {
                        if (DetectionDetails.DetectionAttachedDocList != null && DetectionDetails.DetectionAttachedDocList.Count > 0)
                        {
                            XtraReport report = new XtraReport();
                            int count = 0;
                            List<PdfDocumentProcessor> pdfDocumentProcessorList = new List<PdfDocumentProcessor>();
                            foreach (DetectionAttachedDoc item in DetectionDetails.DetectionAttachedDocList)
                            {
                                try
                                {
                                    MemoryStream DetectionAttachedDocInBytesstream = new MemoryStream(item.DetectionAttachedDocInBytes);

                                    XtraReport xtraReport = new XtraReport();
                                    xtraReport = XtraReport.FromStream(DetectionAttachedDocInBytesstream, true);
                                    if (xtraReport != null)
                                    {
                                        xtraReport.CreateDocument();
                                        report.ModifyDocument(x => x.AddPages(xtraReport.Pages));
                                    }
                                    PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor();
                                    pdfDocumentProcessor.AppendDocument(DetectionAttachedDocInBytesstream);
                                    pdfDocumentProcessorList.Add(pdfDocumentProcessor);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            int largestEdgeLength = 1000;
                            DetectionDetails.MergePDFDocument = new List<Bitmap>();
                            foreach (PdfDocumentProcessor PdfDocumentProcessor in pdfDocumentProcessorList)
                            {
                                for (int i = 1; i <= PdfDocumentProcessor.Document.Pages.Count; i++)
                                {
                                    Bitmap BitmapImage = PdfDocumentProcessor.CreateBitmap(i, largestEdgeLength);
                                    var pb = new XRPictureBox
                                    {
                                        Image = BitmapImage,
                                    };
                                    DetectionDetails.MergePDFDocument.Add(BitmapImage);
                                }
                            }
                            for (int i = 0; i < DetectionDetails.MergePDFDocument.Count; i++)
                            {
                                pcmPrintModuleDetectionReport.SubBand1.Band.Controls.Add(new XRPictureBox
                                {
                                    Image = DetectionDetails.MergePDFDocument[i],
                                    Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze,
                                    Size = new System.Drawing.Size((DetectionDetails.MergePDFDocument[i].Width - 200), (DetectionDetails.MergePDFDocument[i].Height - 200)),
                                    LocationF = new PointF(0, i * 800)
                                });

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                #region Watermark
                if (!DetectionDetails.Status.Value.ToLower().Equals("Active".ToLower()))
                {
                    pcmPrintModuleDetectionReport.Watermark.ShowBehind = true;
                    pcmPrintModuleDetectionReport.Watermark.Text = DetectionDetails.Status.Value;
                    pcmPrintModuleDetectionReport.Watermark.Font = new Font(pcmPrintModuleDetectionReport.Watermark.Font.FontFamily, 150);
                    pcmPrintModuleDetectionReport.Watermark.ForeColor = System.Drawing.Color.Gray;
                    pcmPrintModuleDetectionReport.Watermark.TextTransparency = 150;
                }
                #endregion
                //DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                //window.PreviewControl.DocumentSource = pcmPrintModuleDetectionReport;
                //pcmPrintModuleDetectionReport.CreateDocument();
                //window.Show();
                DevExpress.XtraPrinting.PdfExportOptions options = new DevExpress.XtraPrinting.PdfExportOptions();
                using (FileStream pdfFileStream = new FileStream(AnnouncementFolderName + FileName + "_datasheet.pdf", FileMode.Create))
                {
                    pcmPrintModuleDetectionReport.ExportToPdf(pdfFileStream, options);
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
        //Shubham[skadam] GEOS2-5024 Improvements in PCM module 25 12 2023
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
