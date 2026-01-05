using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using System.Drawing;
using System.Drawing.Imaging;
using Emdep.Geos.UI.CustomControls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using Emdep.Geos.Data.Common.Hrm;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.Hrm.Views;
using DevExpress.Spreadsheet;
using System.IO;
using System.Configuration;
using System.Globalization;
using DevExpress.Spreadsheet;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddGenerateCertificateViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
       IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
     //IHrmService HrmService = new HrmServiceController("localhost:6699");
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region Public Events
        public event EventHandler RequestClose;
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #region Declaration
        private bool isSave;
        public ProfessionalTraining NewProfessionalTraining { get; set; }
        public ProfessionalTraining AllTrainingData { get; set; }
        public string folderPathTextBox;
        private List<Language> languages;
        private int settingWindowLanguageSelectedIndex;
        public ObservableCollection<ProfessionalSkill> skillList;
        #endregion
        #region Properties
        public ObservableCollection<ProfessionalSkill> SkillList
        {
            get
            {
                return skillList;
            }

            set
            {
                skillList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SkillList"));
            }
        }

        public string FolderPathTextBox
        {
            get { return folderPathTextBox; }
            set
            {
                folderPathTextBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FolderPathTextBox"));
            }
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
        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }
        string language;
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));

            }
        }
        #endregion
        #region Commands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand BrowseButtonClickCommand { get; set; }
        public ICommand AcceptButtonClickCommand { get; set; }
        #endregion
        #region Constructor
        public AddGenerateCertificateViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddGenerateCertificateViewModel ...", category: Category.Info, priority: Priority.Low);
                FillLanguage();
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                BrowseButtonClickCommand = new DelegateCommand<object>(BrowseButtonClickCommandAction);
                AcceptButtonClickCommand = new DelegateCommand<object>(AcceptButtonClickCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor AddGenerateCertificateViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddGenerateCertificateViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region Methods
        private void BrowseButtonClickCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);

                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        FolderPathTextBox = folderBrowserDialog.SelectedPath;
                    }

                }

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonClickCommandAction(object obj)
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
                Workbook workbook = new Workbook();
                FileStream stream = null;
                Language Lang = Languages[settingWindowLanguageSelectedIndex];
                CultureInfo cultureInfo;
                List<string> filesName = new List<string>();
                string fileName = null;
                List<string> Alllangs = new List<string>();
                
                List<EmployeeTraineeDetails> TraineeData = new List<EmployeeTraineeDetails>();
                TraineeData = HrmService.GetTraineedataforExcel(NewProfessionalTraining.IdProfessionalTraining);
                string UpdatedpathTostore = string.Empty;
                //if (TraineeData.Count > 0)
                //{
                foreach (var item in TraineeData)
                    {
                        cultureInfo = new CultureInfo(workbook.Options.Culture.Name);
                        string Trainingcode = item.TrainingCode;
                        string sitename = GeosApplication.Instance.SiteName;

                        // byte[] excelTemplateInByte = GetTrainingTemplate(item.EmployeeCode, item.TrainingCode, ExcelPath);
                        byte[] excelTemplateInByte = HrmService.GetTrainingTemplate(sitename);
                        if (excelTemplateInByte == null)
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("TrainingReportFileNotFound").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                        workbook.LoadDocument(excelTemplateInByte, DocumentFormat.Xlsx);

                        Worksheet wsDl = workbook.Worksheets[1];
                        string filenameExcel = null;
                        string filenamePDF = null;
                   
                    if (FolderPathTextBox != null)
                    {
                         UpdatedpathTostore = FolderPathTextBox;
                    }else
                    {
                        UpdatedpathTostore = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }
                        if (!System.IO.Directory.Exists(UpdatedpathTostore))
                        {
                            System.IO.Directory.CreateDirectory(UpdatedpathTostore);
                        }
                        string search = "TR_" + item.TrainingCode + "_" + item.EmployeeCode + ".*";
                        FileInfo[] files = new DirectoryInfo(UpdatedpathTostore + "\\").GetFiles(search).OrderBy(f => f.CreationTime).ToArray();
                        var allfiles = files.ToList();
                        filenameExcel = "TR_" + item.TrainingCode + "_" + item.EmployeeCode + ".xlsx";
                        filenamePDF = "TR_" + item.TrainingCode + "_" + item.EmployeeCode + ".pdf";

                        #region index
                        Int32 Index_TraineeName = 0;
                        Int32 Index_TraineeCode = 0;
                        Int32 Index_TraineeJDCode = 0;
                        Int32 Index_TraineeDepartment = 0;
                        Int32 Index_TrainingMode = 0;
                        Int32 Index_TrainerName = 0;
                        Int32 Index_TrainerCode = 0;
                        Int32 Index_TrainerJDCode = 0;
                        Int32 Index_TrainerDepartment = 0;
                        Int32 Index_TrainerCompany = 0;
                        Int32 Index_SkillsStartRow = 0;
                        Int32 Index_TraineeSignatureDate = 0;
                        Int32 Index_TrainerSignatureDate = 0;
                        Int32 Index_CompanyName = 0;
                        Int32 Language = 0;
                        string Index_SkillsDescriptionColumn = string.Empty;
                        string Index_SkillsCourseColumn = string.Empty;
                        string Index_SkillsDurationColumn = string.Empty;
                        string Index_SkillsStartDateColumn = string.Empty;
                        string Index_SkillsEndDateColumn = string.Empty;
                        string Index_SkillsEvaluationColumn = string.Empty;
                        #endregion

                        #region indexnumber
                        for (int s = 0; s < 300; s++)
                        {
                            string cellValue = wsDl.Cells[s, 0].Value.ToString();
                            switch (cellValue)
                            {
                                case "Index_TraineeName":
                                    Index_TraineeName = s;
                                    break;
                                case "Index_TraineeCode":
                                    Index_TraineeCode = s;
                                    break;
                                case "Index_TraineeJDCode":
                                    Index_TraineeJDCode = s;
                                    break;
                                case "Index_TraineeDepartment":
                                    Index_TraineeDepartment = s;
                                    break;
                                case "Index_TrainingMode":
                                    Index_TrainingMode = s;
                                    break;
                                case "Index_TrainerName":
                                    Index_TrainerName = s;
                                    break;
                                case "Index_TrainerCode":
                                    Index_TrainerCode = s;
                                    break;
                                case "Index_TrainerJDCode":
                                    Index_TrainerJDCode = s;
                                    break;
                                case "Index_TrainerDepartment":
                                    Index_TrainerDepartment = s;
                                    break;
                                case "Index_TrainerCompany":
                                    Index_TrainerCompany = s;
                                    break;
                                case "Index_SkillsStartRow":
                                    Index_SkillsStartRow = s;
                                    break;
                                case "Index_SkillsDescriptionColumn":
                                    Index_SkillsDescriptionColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_SkillsCourseColumn":
                                    Index_SkillsCourseColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_SkillsDurationColumn":
                                    Index_SkillsDurationColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_SkillsStartDateColumn":
                                    Index_SkillsStartDateColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_SkillsEndDateColumn":
                                    Index_SkillsEndDateColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_SkillsEvaluationColumn":
                                    Index_SkillsEvaluationColumn = Convert.ToString(wsDl.Cells[s, 1].Value.ToString());
                                    break;
                                case "Index_TraineeSignatureDate":
                                    Index_TraineeSignatureDate = s;
                                    break;
                                case "Index_TrainerSignatureDate":
                                    Index_TrainerSignatureDate = s;
                                    break;
                                case "Index_CompanyName":
                                    Index_CompanyName = s;
                                    break;
                                case "Language":
                                    Language = s;
                                    break;
                            }
                        }
                        #endregion

                        #region adding value to index number
                        try
                        {
                            using (stream = new FileStream(Path.Combine(UpdatedpathTostore, filenameExcel), FileMode.Create, FileAccess.ReadWrite))
                            {
                                DateTime currentTime = DateTime.Now;

                                cultureInfo = new CultureInfo(Lang.CultureName);

                                wsDl.Cells[Index_TraineeCode, 1].Value = item.EmployeeCode;
                                wsDl.Cells[Language, 1].Value = Lang.TwoLetterISOLanguage;
                                wsDl.Cells[Index_TraineeName, 1].Value = item.TraineeName;
                                wsDl.Cells[Index_TraineeJDCode, 1].Value = item.JdcodeTrainee;
                                wsDl.Cells[Index_TrainingMode, 1].Value = item.Site;
                                wsDl.Cells[Index_TraineeDepartment, 1].Value = item.EmployeeDepartments;
                                wsDl.Cells[Index_TrainerDepartment, 1].Value = item.TrainerDepartment;
                                wsDl.Cells[Index_CompanyName, 1].Value = item.CompanyAlias;
                                wsDl.Cells[Index_TrainingMode, 1].Value = item.Site;

                                if (item.Trainercode != null)
                                {
                                    wsDl.Cells[Index_TrainerName, 1].Value = item.TrainerName;
                                    wsDl.Cells[Index_TrainerCode, 1].Value = item.Trainercode;
                                    wsDl.Cells[Index_TrainerJDCode, 1].Value = item.JdCodeTrainer;
                                    wsDl.Cells[Index_TrainerCompany, 1].Value = NewProfessionalTraining.ExternalEntity;
                                }
                                else
                                {
                                    wsDl.Cells[Index_TrainerName, 1].Value = NewProfessionalTraining.ExternalTrainer;
                                    wsDl.Cells[Index_TrainerCompany, 1].Value = NewProfessionalTraining.ExternalEntity;
                                }
                                wsDl.Cells[Index_TraineeSignatureDate, 1].Value = currentTime.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);
                                wsDl.Cells[Index_TrainerSignatureDate, 1].Value = currentTime.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);
                                Worksheet wsDlExpensesItems = workbook.Worksheets[0];

                                foreach (ProfessionalSkill itemProfessionalSkill in SkillList)
                                {
                                    wsDlExpensesItems.Cells[Index_SkillsDescriptionColumn + Index_SkillsStartRow].Value = itemProfessionalSkill.Name;
                                // wsDlExpensesItems.Cells[Index_SkillsCourseColumn + Index_SkillsStartRow].Value = itemProfessionalSkill.Name;
                                wsDlExpensesItems.Cells[Index_SkillsDurationColumn + Index_SkillsStartRow].Value = itemProfessionalSkill.Duration;
                                    wsDlExpensesItems.Cells[Index_SkillsEvaluationColumn + Index_SkillsStartRow].Value = item.CourseEvaluation;
                                    wsDlExpensesItems.Cells[Index_SkillsStartDateColumn + Index_SkillsStartRow].Value = item.Startdate.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);
                                    wsDlExpensesItems.Cells[Index_SkillsEndDateColumn + Index_SkillsStartRow].Value = item.End_date.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);
                                    Index_SkillsStartRow = Index_SkillsStartRow + 1;
                                }
                                workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                            }

                        using (FileStream pdfFileStream = new FileStream(Path.Combine(UpdatedpathTostore, filenamePDF), FileMode.Create))
                        {
                            workbook.ExportToPdf(pdfFileStream);
                        }

                    }
                        catch
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FileOpened").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }



                    }
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FilesSaved").ToString(), UpdatedpathTostore), System.Windows.Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsSave = true;
                    RequestClose(null, null);
                    workbook.Dispose();
                    if (stream != null)
                    {
                        stream.Dispose();
                    }

               // }
                
                #endregion

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonClickCommandActionCertificate() Method " + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
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
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
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
      
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
   
    }
}
