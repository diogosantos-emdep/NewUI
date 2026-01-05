using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit.Commands;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.APM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    class SubTaskAttachmentsViewModel: NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IAPMService APMService = new APMServiceController("localhost:6699");
        //IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");

        #endregion

        #region public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        #endregion // Events

        #region Declarations
        private double dialogHeight;
        private double dialogWidth;
        private string windowHeader;
        private bool isSave;
        private bool showCommentsFlyout;
        private List<Object> subTaskAttachmentList;
        private bool isBusy;
        private string fileName;
        private string uniqueFileName;
        private AttachmentsByTask attachment;
        private string fileNameString;
        private ObservableCollection<AttachmentsByTask> listAttachment;
        private bool isOpen;
        private string description;
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }

        private List<AttachmentsByTask> clonedAttachmentList;
        public string GuidCode { get; set; }

        private ObservableCollection<AttachmentsByTask> listUpdateTaskAttachment;

        private Int64 idTask;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
                if (ShowCommentsFlyout)
                {
                    IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }

        public List<object> SubTaskAttachmentList
        {
            get { return subTaskAttachmentList; }
            set
            {
                subTaskAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SubTaskAttachmentList"));
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set
            {
                uniqueFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName"));
            }
        }

        public AttachmentsByTask Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }

        public string FileNameString
        {
            get { return fileNameString; }
            set
            {
                fileNameString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileNameString"));
            }
        }

        public ObservableCollection<AttachmentsByTask> ListAttachment
        {
            get { return listAttachment; }
            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOpen"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public List<AttachmentsByTask> ClonedAttachmentList
        {
            get { return clonedAttachmentList; }
            set
            {
                clonedAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedAttachmentList"));
            }
        }

        public ObservableCollection<AttachmentsByTask> ListUpdateTaskAttachment
        {
            get { return listUpdateTaskAttachment; }
            set
            {
                listUpdateTaskAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListUpdateTaskAttachment"));
            }
        }

        public Int64 IdTask
        {
            get { return idTask; }
            set
            {
                idTask = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdTask"));
            }
        }

        #endregion

        #region ICommands
        public ICommand ChooseFileCommand { get; set; }

        public ICommand UploadFileCommand { get; set; }

        public ICommand CancelButtonCommand { get; set; }

        public ICommand AcceptButtonActionCommand { get; set; }

        public ICommand DeleteFileCommand { get; set; }

        public ICommand DownloadFileCommand { get; set; }
        #endregion

        #region Constructor
        public SubTaskAttachmentsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor SubTaskAttachmentsViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 300;
                AcceptButtonActionCommand = new RelayCommand(new Action<object>(AcceptButtonActionCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
                UploadFileCommand = new RelayCommand(new Action<object>(UploadFileCommandAction));
                DeleteFileCommand = new RelayCommand(new Action<object>(DeleteAttachmentRowCommandAction));
                DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);


                GeosApplication.Instance.Logger.Log("Constructor SubTaskAttachmentsViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SubTaskAttachmentsViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init(Int64 idActionPlanTask)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                SubTaskAttachmentList = new List<object>();
                IdTask = idActionPlanTask;
                FillSubTaskAttachmentList(IdTask);

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            
        }
        private void FillSubTaskAttachmentList(Int64 idTask)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSubTaskAttachmentList ...", category: Category.Info, priority: Priority.Low);
               // APMService = new APMServiceController("localhost:6699");
                List<AttachmentsByTask> temp = APMService.GetTaskAttachmentsByIdSubTask_V2650(idTask);
                ListAttachment = new ObservableCollection<AttachmentsByTask>();
                foreach (AttachmentsByTask item in temp)
                {
                    ImageSource objimage = FileExtensionToFileIcon.FindIconForFilename(item.SavedFileName, true);
                    item.AttachmentImage = objimage;
                    item.OriginalFileName = Path.GetFileNameWithoutExtension(item.SavedFileName);

                    if (item.CreatedBy == GeosApplication.Instance.ActiveUser.IdUser || GeosApplication.Instance.IsAPMActionPlanPermission)
                        item.IsDeleted = 1;
                    else
                        item.IsDeleted = 0;
                    ListAttachment.Add((AttachmentsByTask)item.Clone());
                }

                ClonedAttachmentList = new List<AttachmentsByTask>();
                if (ListAttachment != null && ListAttachment.Count > 0)
                {
                    foreach (AttachmentsByTask item in ListAttachment)
                    {
                        ClonedAttachmentList.Add((AttachmentsByTask)item.Clone());
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FillSubTaskAttachmentList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskAttachmentList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskAttachmentList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSubTaskAttachmentList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction ...", category: Category.Info, priority: Priority.Low);

                List<AttachmentsByTask> attachmentList = new List<AttachmentsByTask>();

                foreach (AttachmentsByTask item in ClonedAttachmentList)//[Deleted]
                {
                    if (!ListAttachment.Any(x => x.IdActionPlanTaskAttachment == item.IdActionPlanTaskAttachment))
                    {
                        AttachmentsByTask attachment = new AttachmentsByTask();
                        attachment = item;
                        attachment.IdActionPlanTaskAttachment = item.IdActionPlanTaskAttachment;
                        attachment.IdTask = IdTask;
                        attachment.IsDeleted = 1;
                        attachment.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        attachment.AttachmentImage = null;
                        attachmentList.Add(attachment);
                    }
                }

                ListUpdateTaskAttachment = new ObservableCollection<AttachmentsByTask>();

                string GUIDCode = string.Empty;
                //[Added]
                if (ListAttachment != null && ListAttachment.Count > 0)
                {
                    foreach (AttachmentsByTask item in ListAttachment)
                    {
                        if (!ClonedAttachmentList.Any(x => x.IdActionPlanTaskAttachment == item.IdActionPlanTaskAttachment))
                        {
                            item.IdTask = IdTask;
                            item.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            ListUpdateTaskAttachment.Add(item);
                            item.AttachmentImage = null;
                        }
                    }

                    if (ListUpdateTaskAttachment.Count > 0)
                    {
                        bool IsUpdateAttachment = UploadTaskAttachment();

                        if (IsUpdateAttachment == true)
                        {
                            foreach (var item in ListUpdateTaskAttachment)
                            {
                                attachmentList.Add(item);
                            }
                            GUIDCode = GuidCode;
                        }
                    }
                    IsBusy = false;
                }


                attachmentList = APMService.AddDeleteSubTaskAttachments_V2650(attachmentList, GUIDCode, IdTask);
                IsSave = true;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditTaskAttachmentUpdatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonActionCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void UploadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                // DXSplashScreen.Show<SplashScreenView>();
                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                List<FileInfo> FileDetail = new List<FileInfo>();
                if (SubTaskAttachmentList != null)
                {
                    foreach (AttachmentsByTask item in SubTaskAttachmentList)
                    {
                        item.Description = Description;
                        ListAttachment.Add(item);
                    }
                }
                SubTaskAttachmentList = new List<object>();
                Description = string.Empty;
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void BrowseFile(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            // DXSplashScreen.Show<SplashScreenView>();
            // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    var newFileList = SubTaskAttachmentList != null ? new List<object>(SubTaskAttachmentList) : new List<object>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new AttachmentsByTask();
                    Attachment.FileType = file.Extension;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = file.Name;
                    Attachment.CreatedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSize = file.Length;
                    Attachment.FileType = file.Extension;
                    Attachment.FileUploadName = file.Name;
                    Attachment.IsUploaded = true;

                    Attachment.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    Attachment.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;
                    Attachment.CreatedIn = DateTime.Now;

                    var theIcon = IconFromFilePath(FileName);
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    if (theIcon != null)
                    {
                        // Save it to disk, or do whatever you want with it.
                        if (!Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }

                        if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                        {
                            using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                theIcon.Save(stream);
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                        theIcon.Dispose();
                    }

                    // useful to get icon end process of temp. used imgage 
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    Attachment.AttachmentImage = image;

                    // not allow to add same files
                    List<AttachmentsByTask> fooList = newFileList.OfType<AttachmentsByTask>().ToList();
                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    SubTaskAttachmentList = newFileList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }

            return result;
        }

        public void DeleteAttachmentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() ...", category: Category.Info, priority: Priority.Low);
                // DXSplashScreen.Show<SplashScreenView>();
                // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDelete = false;
                AttachmentsByTask attachmentObject = (AttachmentsByTask)parameter;


                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActionPlanTaskFileDeleteMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ListAttachment != null && ListAttachment.Count > 0)
                    {
                        ListAttachment.Remove((AttachmentsByTask)attachmentObject);
                        isDelete = true;
                    }

                    if (isDelete)
                    {
                        IsBusy = false;
                        GeosApplication.Instance.Logger.Log("Method DeleteAttachmentRowCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionPlanTaskFileDeleteFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
                else
                {
                    IsBusy = false;
                }


            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }



        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["ActionPlanTaskFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    AttachmentsByTask attachmentObject = (AttachmentsByTask)obj;
                    FileInfo file = new FileInfo(attachmentObject.SavedFileName);

                    SaveFileDialogService.DefaultExt = file.Extension;
                    SaveFileDialogService.DefaultFileName = attachmentObject.OriginalFileName;
                    SaveFileDialogService.Filter = "All Files|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();

                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }

                        attachmentObject.FileUploadName = attachmentObject.SavedFileName;
                        attachmentObject.AttachmentImage = null;

                        attachmentObject = APMService.DownloadTaskAttachment_V2580(attachmentObject);

                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, attachmentObject.FileByte);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));

                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }



        public bool UploadTaskAttachment()
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadTaskAttachment() ...", category: Category.Info, priority: Priority.Low);

                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileUploader taskAttachmentFileUploader = new FileUploader();
                taskAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = taskAttachmentFileUploader.FileUploadName;

                if (ListAttachment != null && ListAttachment.Count > 0)
                {
                    foreach (AttachmentsByTask fs in ListUpdateTaskAttachment)
                    {
                        if (fs.IsUploaded == true)
                        {
                            FileInfo file = new FileInfo(fs.FilePath);
                            FileDetail.Add(file);
                        }
                    }
                    taskAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, taskAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload task Attachment UploadTaskAttachment Zip File ", category: Category.Info, priority: Priority.Low);
                    //GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderActionPlanTaskAttachmentZipFile_V2580(taskAttachmentFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload task Attachment UploadTaskAttachment Zip File successfully", category: Category.Info, priority: Priority.Low);
                }

                if (fileUploadReturnMessage.IsFileUpload == true)
                {
                    isupload = true;
                    //IsBusy = false;

                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                    if (!string.IsNullOrEmpty(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    GeosApplication.Instance.Logger.Log("Method UploadTaskAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    //IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadTaskAttachment() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadTaskAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadTaskAttachment() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }

        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = tempPath + "TempFolderForEdit\\";
            if (!Directory.Exists(tempfolderPath))
            {
                Directory.CreateDirectory(tempfolderPath);
            }

            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);

                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            string destinationPath = tempfolderPath + ListUpdateTaskAttachment[i].SavedFileName;

                            if (File.Exists(destinationPath))
                            {
                                File.Delete(destinationPath);
                            }

                            File.Copy(filesDetail[i].FullName, destinationPath);

                            archive.AddFile(destinationPath, "/");
                            ListUpdateTaskAttachment[i].FilePath = destinationPath;
                        }

                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }

                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error on ConvertZipToByte method", category: Category.Exception, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }

        //private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        //{
        //    byte[] filedetails = null;

        //    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
        //    string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolderForEdit\";
        //    if (!Directory.Exists(tempfolderPath))
        //    {
        //        System.IO.Directory.CreateDirectory(tempfolderPath);
        //    }
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);
        //        using (ZipArchive archive = new ZipArchive())
        //        {
        //            if (filesDetail.Count > 0)
        //            {
        //                for (int i = 0; i < filesDetail.Count; i++)
        //                {
        //                    ListUpdateTaskAttachment[i].FileUploadName = ListUpdateTaskAttachment[i].SavedFileName;
        //                    System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + ListUpdateTaskAttachment[i].SavedFileName);
        //                    string s = tempfolderPath + ListUpdateTaskAttachment[i].SavedFileName;
        //                    archive.AddFile(s, @"/");
        //                    ListUpdateTaskAttachment[i].FilePath = s;
        //                }

        //                archive.Save(tempPath + GuidCode + ".zip");
        //                filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
        //        return filedetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method", category: Category.Exception, priority: Priority.Low);
        //        DeleteTempFolder();
        //        return filedetails;
        //    }
        //}

        private void DeleteTempFolder()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteTempFolder()..."), category: Category.Info, priority: Priority.Low);

                string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteTempFolder()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteTempFolder...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }            
        }

        #endregion

        #region Validation
       
        #endregion
    }
}
