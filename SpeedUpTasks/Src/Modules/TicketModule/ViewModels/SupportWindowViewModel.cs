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

using Emdep.Geos.Modules.Ticket;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Commands;
using Prism.Logging;
using Emdep.Geos.UI.Adapters.Logging;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.Ticket.ViewModels
{
    public class SupportWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGlpiService GlpiService = new GlpiController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp workbenchControl = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration

        private IList<LookupValue> priorityList; //supportWindow Urrgency List
        private List<object> attachmentList; //supportWindow attachment document List
        private int prioritySelectedIndex; //supportWindow Urrgency SelectedIndex
        private string supportWindowTitle; //supportWindow Title
        private string supportWindowDescription; //supportWindow Dscription
        // private User User;//supportWindow user details
        private Uri resourcePath;
        private ResourceDictionary myresourcedictionary;
        private ResourceDictionary mystyles;
        private GlpiUser gLPIUser;
        private bool isBusy;
        public string GuidCode { get; set; }
        ResourceDictionary dict;
        public string myIP;
        private List<GeosModule> geosModuleList;
        public List<GeosModule> TempGeosModuleList;
        private int moduleSelectedIndex;
        #endregion // Declaration

        #region public Properties

        public IList<LookupValue> PriorityList
        {
            get { return priorityList; }
            set { priorityList = value; }
        }

        public List<object> AttachmentList
        {
            get { return attachmentList; }
            set
            {
                attachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentList"));
            }
        }

        public string SupportWindowTitle
        {
            get { return supportWindowTitle; }
            set
            {
                supportWindowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupportWindowTitle"));
            }
        }
        public string SupportWindowDescription
        {
            get { return supportWindowDescription; }
            set
            {
                supportWindowDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupportWindowDescription"));
            }
        }

        public int PrioritySelectedIndex
        {
            get { return prioritySelectedIndex; }
            set
            {
                prioritySelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrioritySelectedIndex"));
            }
        }

        public Uri ResourcePath
        {
            get { return resourcePath; }
            set { resourcePath = value; }
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
        public int ModuleSelectedIndex
        {
            get { return moduleSelectedIndex; }
            set { moduleSelectedIndex = value; OnPropertyChanged(new PropertyChangedEventArgs("ModuleSelectedIndex")); }
        }

        #endregion // Properties

        #region Command

        public ICommand SupportWindowAcceptButtonCommand { get; set; } //Accept on SupportWindow
        public ICommand SupportWindowCancelButtonCommand { get; set; } //Cancel on SupportWindow
        public ICommand SupportWindowBrowesButtonCommand { get; set; } //Cancel on SupportWindow

        #endregion // Command

        #region Events

        public event EventHandler RequestClose; //supportWindow for close window

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Constructor

        public SupportWindowViewModel()
        {
            string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;
            if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
            {
                if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
            }

            FileInfo logFile = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);
            GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, logFile);
            GeosApplication.Instance.Logger.Log("Support(Ticket) Application Start...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.Logger.Log("Start SupportWindowViewModel constructor", category: Category.Info, priority: Priority.Low);

            //SetLanguageDictionary();
            FillUrgenctList();
            FillModuleList();
            SupportWindowAcceptButtonCommand = new RelayCommand(new Action<object>(SupportWindowAccept));
            SupportWindowCancelButtonCommand = new RelayCommand(new Action<object>(SupportWindowCancel));
            SupportWindowBrowesButtonCommand = new RelayCommand(new Action<object>(SupportWindowBrowse));
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST        
            myIP = ApplicationOperation.GetEmdepGroupIP("10.0.");
            string file = System.AppDomain.CurrentDomain.FriendlyName;

            GeosApplication.Instance.Logger.Log("End SupportWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Method for fill urgency type
        /// </summary>
        /// <returns></returns>
        private void FillUrgenctList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUrgenctList ...", category: Category.Info, priority: Priority.Low);

                PriorityList = CrmStartUp.GetLookupValues(3);

                GeosApplication.Instance.Logger.Log("Method FillUrgenctList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillUrgenctList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillUrgenctList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillUrgenctList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


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
                {     if(!(GeosModuleList.Any(ij=> ij.IdGeosModule == TempGeosModuleList[i].IdGeosModule)))
                    GeosModuleList.Add(new GeosModule() { IdGeosModule = TempGeosModuleList[i].IdGeosModule, Name = TempGeosModuleList[i].Name, Acronym= TempGeosModuleList[i].Acronym });
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
                //IsInit = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ProductAndService() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("End FillModuleList Method", category: Category.Info, priority: Priority.Low);
        }
        /// <summary>
        /// Method for to save attach document and ticket detail on database.
        /// </summary>
        /// <param name="obj"></param>
        private void SupportWindowAccept(object obj)
        {
            GeosApplication.Instance.Logger.Log("Click on Accept button Support Window", category: Category.Info, priority: Priority.Low);
            GeosApplication.Instance.Logger.Log("Start SupportWindowAccept Method", category: Category.Info, priority: Priority.Low);
            IsBusy = true;

            try
            {
                if (!string.IsNullOrEmpty(SupportWindowDescription) && !string.IsNullOrEmpty(SupportWindowTitle) && ModuleSelectedIndex>0)
                {


                    #region
                    ////String guidcode = Guid.NewGuid().ToString();ApplicationSettings).Items[0].Value
                    //IGlpiService GLPIControl = new GlpiController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    //FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                    //List<FileInfo> FileDetail = new List<FileInfo>();
                    //List<GlpiDocumentType> GLPIDocumentType = GLPIControl.GetGLPIDocumentType();
                    //List<GlpiDocument> glpiDocument = new List<GlpiDocument>();
                    //GlpiDocument glpi = new GlpiDocument();
                    //GlpiLocation GLPILocation = new GlpiLocation();
                    //IGeosRepositoryService FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    //FileUploader userProfileFileUploader1 = new FileUploader();
                    //userProfileFileUploader1.FileUploadName = GUIDCode.GUIDCodeString();
                    //GuidCode = userProfileFileUploader1.FileUploadName;

                    //if (AttachmentList != null)
                    //{
                    //    foreach (FileInfo fs in AttachmentList)
                    //    {
                    //        FileDetail.Add(fs);
                    //    }

                    //    userProfileFileUploader1.FileByte = ConvertZipToByte(FileDetail, userProfileFileUploader1.FileUploadName);// read byte[]  zip file Byte 

                    //    GeosApplication.Instance.Logger.Log("Getting Upload GLPI Zip File ", category: Category.Info, priority: Priority.Low);
                    //    fileUploadReturnMessage = FileControl.UploaderGLPIZipFile(userProfileFileUploader1);
                    //    GeosApplication.Instance.Logger.Log("Getting Upload GLPI Zip File successfully", category: Category.Info, priority: Priority.Low);
                    //}

                    //if (fileUploadReturnMessage.IsFileUpload == true)
                    //{
                    //    for (int i = 0; i < FileDetail.Count; i++)
                    //    {
                    //        glpi = new GlpiDocument();
                    //        glpi.Comment = "XYZ";
                    //        glpi.DocumentCategoriesId = 1;
                    //        glpi.EntitiesId = 17;

                    //        glpi.FileName = FileDetail[i].Name;//((System.IO.FileInfo)((AttachmentList)._items[i])).Name.ToString();
                    //        glpi.FilePath = FileDetail[i].FullName;
                    //        glpi.IsBlacklisted = true;
                    //        glpi.IsDeleted = true;
                    //        glpi.IsRecursive = true;
                    //        glpi.Name = SupportWindowTitle;
                    //        if (!glpiDocument.Any(a => a.FileName == glpi.FileName))
                    //        {
                    //            glpiDocument.Add(glpi);
                    //        }
                    //    }
                    //}

                    //GlpiTicket glpiticket = new GlpiTicket();
                    //glpiticket.EntitiesId = 17;

                    //glpiticket.Name = SupportWindowTitle;
                    //glpiticket.CloseDate = null;
                    //glpiticket.SolveDate = null;
                    //glpiticket.DateMod = null;
                    //glpiticket.UsersIdLastupdater = 0;
                    //glpiticket.Status = 1;

                    //glpiticket.UsersIdRecipient = GLPIUser.Id;
                    //glpiticket.GlpiUserName = GLPIUser.Name + " " + (GLPIUser.Id);

                    //glpiticket.RequestTypesId = 1;
                    //glpiticket.Content = SupportWindowDescription + System.Environment.NewLine + System.Environment.NewLine + "IP" + "  " + myIP + System.Environment.NewLine + GLPIUser.Name + " " + GLPIUser.RealName;
                    //glpiticket.Urgency = 3;
                    //glpiticket.Impact = 3;
                    //glpiticket.Priority = PriorityList[PrioritySelectedIndex].IdLookupValue;
                    //glpiticket.ItilcategoriesId = 0;
                    //glpiticket.Type = 1;
                    //glpiticket.SolutiontypesId = 0;
                    //glpiticket.Solution = null;
                    //glpiticket.GlobalValidation = 1;
                    //glpiticket.SlasId = 0;
                    //glpiticket.SlalevelsId = 0;
                    //glpiticket.DueDate = null;
                    //glpiticket.BeginWaitingDate = null;

                    //GeosApplication.Instance.Logger.Log("Getting GLPI Location By Company Id ", category: Category.Info, priority: Priority.Low);
                    //GLPILocation = GLPIControl.GetGLPILocationByCompanyId(Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdCompany));
                    //GeosApplication.Instance.Logger.Log("Getting GLPI Location By Company Id Successfully", category: Category.Info, priority: Priority.Low);

                    //if (GLPILocation == null)
                    //{
                    //    glpiticket.LocationsId = 0;
                    //}
                    //else
                    //{
                    //    glpiticket.LocationsId = GLPILocation.Id;
                    //}

                    //if (AttachmentList != null)
                    //{
                    //    glpiticket.GlpiDocuments = glpiDocument;
                    //    glpiticket.GUIDString = userProfileFileUploader1.FileUploadName;
                    //}

                    //GeosApplication.Instance.Logger.Log("Getting Add GLPI Ticket ", category: Category.Info, priority: Priority.Low);
                    //GLPIControl.AddGLPITicket(glpiticket);
                    #endregion

                    GlpiTicketMail glpiTicketMail = new GlpiTicketMail();

                    glpiTicketMail.Title = "["+ PriorityList[PrioritySelectedIndex].Value+"]"+"["+ GeosModuleList[ModuleSelectedIndex].Acronym+"]"+ " "+SupportWindowTitle;
                    glpiTicketMail.Description = SupportWindowDescription;
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
                    GlpiService.SendGlpiTicketMail(glpiTicketMail);

                    GeosApplication.Instance.Logger.Log("Getting Add GLPI Ticket Successfully", category: Category.Info, priority: Priority.Low);

                    prioritySelectedIndex = 0;
                    SupportWindowTitle = string.Empty;
                    SupportWindowDescription = string.Empty;
                    //glpiDocument = null;
                    AttachmentList = null;
                    object resource = System.Windows.Application.Current.FindResource("SupportWindowCreateticket");
                    CustomMessageBox.SetValueToMessage(System.Windows.Application.Current.FindResource("SupportWindowCreateticket").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    //  DeleteTempFolder();
                    IsBusy = false;
                    RequestClose(null, null);
                }
                else
                {
                    GeosApplication.Instance.Logger.Log("Get an error On SupportWindowAccept Method ", category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("SupportWindowNotCreateticket").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SupportWindowAccept() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in SupportWindowAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //  DeleteTempFolder();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SupportWindowAccept() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            IsBusy = false;
        }

        private void SupportWindowCancel(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        private void SupportWindowBrowse(object obj)
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
            dict = new ResourceDictionary();
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

        public void Dispose()
        {
            Environment.Exit(0);
        }

        #endregion
        #endregion
    }
}


