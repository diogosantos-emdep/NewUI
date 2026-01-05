using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

using System.Windows;
using System.Threading;
using Emdep.Geos.Modules.Ticket.Properties;
using Emdep.Geos.Modules;
using Emdep.Geos.Data.Common.Glpi;
using Emdep.Geos.UI;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;
using DevExpress.Compression;
using Emdep.Geos.UI.CustomControls;
using System.ServiceModel;
using System.Net;
using System.Resources;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Commands;
using Prism.Logging;
using Emdep.Geos.UI.Adapters.Logging;

namespace Emdep.Geos.Modules.Ticket.ViewModels
{
    public class FeedbackWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        private List<object> attachmentList;        //Feedback attachment document List
        private int moduleSelectedIndex;            //FeedbackWindow Module SelectedIndex
        private string feedbackWindowDescription;   //FeedbackWindow Dscription
        private List<GeosModule> geosModuleList;
        public List<GeosModule> TempGeosModuleList;
        private GlpiUser gLPIUser;
        public string Installedversion { get; set; }
        public string myIP;
        private bool isBusy;
        public bool catchError { get; set; }
        public bool IsInit { get; set; }
        public string GuidCode { get; set; }
        ResourceDictionary dict = new ResourceDictionary();
        IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGlpiService glpiControl = new GlpiController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region  public Properties

        public List<object> AttachmentList
        {
            get
            {
                return attachmentList;
            }

            set
            {
                attachmentList = value; OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public int ModuleSelectedIndex
        {
            get { return moduleSelectedIndex; }
            set { moduleSelectedIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("ModuleSelectedIndex")); }
        }
        public string FeedbackWindowDescription
        {
            get { return feedbackWindowDescription; }
            set { feedbackWindowDescription = value; OnPropertyChanged(new PropertyChangedEventArgs("FeedbackWindowDescription")); }
        }

        public List<GeosModule> GeosModuleList
        {
            get
            {
                return geosModuleList;
            }

            set
            {
                geosModuleList = value; ; OnPropertyChanged(new PropertyChangedEventArgs("geosModuleList"));
            }
        }
        public GlpiUser GLPIUser
        {
            get { return gLPIUser; }
            set { gLPIUser = value; }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }



        #endregion

        #region  Events

        public event EventHandler RequestClose; //FeedbackWindow for close window

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region  public ICommand

        public ICommand FeedbackWindowAcceptButtonCommand { get; set; } //Accept on FeedbackWindow
        public ICommand FeedbackWindowCancelButtonCommand { get; set; } //Cancel on FeedbackWindow
        public ICommand FeedbackWindowBrowesButtonCommand { get; set; } //Cancel on FeedbackWindow

        #endregion

        #region  Constructor

        public FeedbackWindowViewModel()
        {
            IsInit = true;
            string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;
            if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
            {
                if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
            }
            FileInfo logFile = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
            GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, logFile);
            GeosApplication.Instance.Logger.Log("Support(Feedback) Application Start...", category: Category.Info, priority: Priority.Low);


            GeosApplication.Instance.Logger.Log("Start FeedbackWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
            FillModuleList();
            //SetLanguageDictionary();
            FeedbackWindowAcceptButtonCommand = new RelayCommand(new Action<object>(FeedbackWindowAccept));
            FeedbackWindowCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            FeedbackWindowBrowesButtonCommand = new RelayCommand(new Action<object>(FeedbackWindowBrowse));
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST        
            myIP = ApplicationOperation.GetEmdepGroupIP("10.0.");
            string file = System.AppDomain.CurrentDomain.FriendlyName;

            GeosApplication.Instance.Logger.Log("End FeedbackWindowViewModel constructor", category: Category.Info, priority: Priority.Low);

        }



        #endregion

        #region public Methods

        /// <summary>
        /// Method for to fill modules list .
        /// </summary>
        private void FillModuleList()
        {
            try
            {
                GeosModuleList = new List<GeosModule>();

                GeosApplication.Instance.Logger.Log("Getting Geos Module list User Id", category: Category.Info, priority: Priority.Low);
                TempGeosModuleList = workbenchControl.GetUserModulesPermissions(GeosApplication.Instance.ActiveUser.IdUser);
                GeosApplication.Instance.Logger.Log("Getting Geos Module list User Id successfully", category: Category.Info, priority: Priority.Low);
                GeosModuleList.Add(new GeosModule() { IdGeosModule = 0, Name = "---" });
                for (int i = 0; i < TempGeosModuleList.Count; i++)
                {
                    if (!(GeosModuleList.Any(ij => ij.IdGeosModule == TempGeosModuleList[i].IdGeosModule)))
                        GeosModuleList.Add(new GeosModule() { IdGeosModule = TempGeosModuleList[i].IdGeosModule, Name = TempGeosModuleList[i].Name, Acronym = TempGeosModuleList[i].Acronym });
                }
                GeosApplication.Instance.ServerActiveMethod();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillModuleList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On FillModuleList Method", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                if (!GeosApplication.Instance.IsServiceActive)
                {
                    GeosApplication.Instance.ServerDeactiveMethod();
                }
                IsInit = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("End FillModuleList Method", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for to save attach document .
        /// </summary>
        /// <param name="obj"></param>
        private void FeedbackWindowAccept(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Accept button Feedback Window", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log("Start FeedbackWindowAccept Method", category: Category.Info, priority: Priority.Low);

            IsBusy = true;

            try
            {
                #region
                //FileUploader userProfileFileUploader1 = new FileUploader();
                //userProfileFileUploader1.FileUploadName = GUIDCode.GUIDCodeString();
                //GuidCode = userProfileFileUploader1.FileUploadName;
                //FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

                //IGeosRepositoryService fileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //GlpiDocument glpi = new GlpiDocument();
                //GlpiLocation gLPILocation = new GlpiLocation();
                //GlpiItilCategory gLPIItilCategory = new GlpiItilCategory();
                //IGlpiService gLPIControl = new GlpiController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //List<GlpiDocument> glpiDocument = new List<GlpiDocument>();

                //if (!string.IsNullOrEmpty(FeedbackWindowDescription))
                //{
                //    List<FileInfo> FileDetail = new List<FileInfo>();

                //    if (AttachmentList != null)
                //    {
                //        foreach (FileInfo fs in AttachmentList)
                //        {
                //            FileDetail.Add(fs);
                //        }

                //        userProfileFileUploader1.FileByte = ConvertZipToByte(FileDetail, userProfileFileUploader1.FileUploadName);// read byte[]  zip file Byte 

                //        GeosApplication.Instance.Logger.Log("Getting Upload GLPI Zip File ", category: Category.Info, priority: Priority.Low);
                //        fileUploadReturnMessage = fileControl.UploaderGLPIZipFile(userProfileFileUploader1);
                //        GeosApplication.Instance.Logger.Log("Getting Upload GLPI Zip File successfully", category: Category.Info, priority: Priority.Low);
                //    }

                //    if (fileUploadReturnMessage.IsFileUpload == true)
                //    {
                //        for (int i = 0; i < FileDetail.Count; i++)
                //        {
                //            glpi = new GlpiDocument();
                //            glpi.Comment = "XYZ";
                //            glpi.DocumentCategoriesId = 1;
                //            glpi.EntitiesId = 19;

                //            glpi.FileName = FileDetail[i].Name;//((System.IO.FileInfo)((AttachmentList)._items[i])).Name.ToString();
                //            glpi.FilePath = FileDetail[i].FullName;
                //            glpi.IsBlacklisted = true;
                //            glpi.IsDeleted = true;
                //            glpi.IsRecursive = true;
                //            // glpi.Name = SupportWindowTitle;
                //            if (!glpiDocument.Any(a => a.FileName == glpi.FileName))
                //            {
                //                glpiDocument.Add(glpi);
                //            }
                //        }
                //    }

                //    GlpiTicket glpiticket = new GlpiTicket();
                //    glpiticket.EntitiesId = 19;
                //    if (GeosModuleList[ModuleSelectedIndex].IdGeosModule == 0)
                //    {
                //        glpiticket.Name = "Improvement" + " " + Installedversion;
                //    }
                //    else
                //    {
                //        glpiticket.Name = GeosModuleList[ModuleSelectedIndex].Name + " " + "Improvement" + " " + Installedversion;
                //    }

                //    glpiticket.CloseDate = null;
                //    glpiticket.SolveDate = null;
                //    glpiticket.DateMod = null;
                //    glpiticket.UsersIdLastupdater = 0;
                //    glpiticket.Status = 1;

                //    glpiticket.UsersIdRecipient = GLPIUser.Id;
                //    glpiticket.GlpiUserName = GLPIUser.Name + " " + (GLPIUser.Id);
                //    glpiticket.RequestTypesId = 1;
                //    glpiticket.Content = feedbackWindowDescription + System.Environment.NewLine + System.Environment.NewLine + "IP" + "  " + myIP + System.Environment.NewLine + GLPIUser.Name + " " + GLPIUser.RealName;
                //    glpiticket.Urgency = 3;
                //    glpiticket.Impact = 3;
                //    glpiticket.Priority = 3;
                //    GeosApplication.Instance.Logger.Log("Getting GLPI Category By ModuleId ", category: Category.Info, priority: Priority.Low);
                //    gLPIItilCategory = gLPIControl.GetGLPIItilCategoryByModuleId(GeosModuleList[ModuleSelectedIndex].IdGeosModule);
                //    GeosApplication.Instance.Logger.Log("Getting GLPI Category By ModuleId Successfully", category: Category.Info, priority: Priority.Low);

                //    if (gLPIItilCategory == null)
                //    {
                //        glpiticket.ItilcategoriesId = 0;
                //    }
                //    else
                //    {
                //        glpiticket.ItilcategoriesId = gLPIItilCategory.Id;
                //    }

                //    glpiticket.Type = 2;
                //    glpiticket.SolutiontypesId = 0;
                //    glpiticket.Solution = null;
                //    glpiticket.GlobalValidation = 1;
                //    glpiticket.SlasId = 0;
                //    glpiticket.SlalevelsId = 0;
                //    glpiticket.DueDate = null;
                //    glpiticket.BeginWaitingDate = null;

                //    GeosApplication.Instance.Logger.Log("Getting GLPI Location By Company Id ", category: Category.Info, priority: Priority.Low);
                //    gLPILocation = gLPIControl.GetGLPILocationByCompanyId(Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdCompany));
                //    GeosApplication.Instance.Logger.Log("Getting GLPI Location By Company Id Successfully", category: Category.Info, priority: Priority.Low);

                //    if (gLPILocation == null)
                //    {
                //        glpiticket.LocationsId = 0;
                //    }
                //    else
                //    {
                //        glpiticket.LocationsId = gLPILocation.Id;
                //    }
                //    if (AttachmentList != null)
                //    {
                //        glpiticket.GlpiDocuments = glpiDocument;
                //        glpiticket.GUIDString = userProfileFileUploader1.FileUploadName;
                //    }
                #endregion
                if (!string.IsNullOrEmpty(FeedbackWindowDescription) && ModuleSelectedIndex>0)
                {   
                    GeosApplication.Instance.Logger.Log("Getting Add GLPI Ticket ", category: Category.Info, priority: Priority.Low);
                GlpiTicketMail glpiTicketMail = new GlpiTicketMail();

                glpiTicketMail.Title = "[" + GeosModuleList[ModuleSelectedIndex].Acronym + "]" + " New Improvement";
                    //GeosModuleList[ModuleSelectedIndex].Name ;
                    glpiTicketMail.Description = FeedbackWindowDescription;
                glpiTicketMail.ActiveUserMailId = GeosApplication.Instance.ActiveUser.CompanyEmail;
                glpiTicketMail.Attachments = new Dictionary<string, byte[]>();
                    if (AttachmentList != null)
                    {
                        foreach (FileInfo item in AttachmentList)
                        {
                            if (File.Exists(item.FullName))
                            {
                                if (!(glpiTicketMail.Attachments.Any(i => i.Key == item.Name)))
                                    glpiTicketMail.Attachments.Add(item.Name, File.ReadAllBytes(item.FullName));

                            }

                        }
                    }
                    glpiControl.SendGlpiTicketMail(glpiTicketMail);
               // gLPIControl.AddGLPITicket(glpiticket);
                    GeosApplication.Instance.Logger.Log("Getting Add GLPI Ticket Successfully", category: Category.Info, priority: Priority.Low);

                    ModuleSelectedIndex = 0;
                    feedbackWindowDescription = string.Empty;
                   // glpiDocument = null;
                    AttachmentList = null;
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("SupportWindowCreateticket").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                   // DeleteTempFolder();
                    IsBusy = false;
                    RequestClose(null, null);
                }
                else
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("SupportWindowNotCreateticket").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.ServerActiveMethod();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FeedbackWindowAccept() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in FeedbackWindowAccept() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);

                if (!GeosApplication.Instance.IsServiceActive)
                {
                    GeosApplication.Instance.ServerDeactiveMethod();
                }

                DeleteTempFolder();
            }

            IsBusy = false;
        }

        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        private void FeedbackWindowBrowse(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                FileInfo file = new FileInfo(dlg.FileName);
                var newFileList = AttachmentList != null ? new List<object>(AttachmentList) : new List<object>();
                newFileList.Add(file);
                AttachmentList = newFileList;
            }
        }

        /// <summary>
        /// Method for set Language as per culture. 
        /// </summary>
        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            string lan = GeosApplication.Instance.CurrentCulture;

            try
            {
                dict.Source = new Uri("/Emdep.Geos.Modules.Ticket;component/Resources/Language." + lan + ".xaml", UriKind.RelativeOrAbsolute);
            }
            catch (Exception)
            {
                dict.Source = new Uri("/Emdep.Geos.Modules.Ticket;component/Resources/Language.xaml", UriKind.RelativeOrAbsolute);
            }

            System.Windows.Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// This method for to convert zip to byte
        /// </summary>
        /// <param name="filesDetail"></param>
        /// <param name="GuidCode"></param>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";

            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);

                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            archive.AddFile(filesDetail[i].FullName, @"/");
                        }

                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }

                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method", category: Category.Info, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }

        /// <summary>
        /// Method for delete TempFolder folders.
        /// </summary>
        private void DeleteTempFolder()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        /// <summary>
        /// this mathod for close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
