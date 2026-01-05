using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
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
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddEngineeringAnalysisViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private bool isBusy;
        private string fileName;
        private string uniqueFileName;
        private string fileNameString;
        private List<Attachment> engineeringAnalysisAttachmentList;
        private List<object> tempAttachmentList;
        private Attachment attachment;
        private string comments;
        private DateTime dueDate;
        private int selectedIndexAttachment = -1;
        FileUploader engAnalysisAttachmentFileUploaderIndicator;
        private EngineeringAnalysis engAnalysis;
        private EngineeringAnalysis engAnalysisDuplicate;

        private bool isEngAnalysisChooseFileEnable = true;
        private bool isAcceptEnable = true;

        private bool isCompleted = true;
        private bool isCompletedReadOnly = true;

        private bool isCommentsReadOnly = false;
        private bool isAttachmentReadOnly = false;
        private bool isDateReadOnly = false;

        List<EngineeringAnalysisType> engineeringAnalysisTypes;
        List<object> selectedEngineeringAnalysisTypes;
        private bool isEngineeringAnalysisTypesReadOnly = false;
        private string visible;
        private string offerCode;
        #endregion

        #region Properties

        public bool IsSave { get; set; }
        public bool IsEdit { get; set; }

        public string GuidCode { get; set; }

        public FileUploader EngAnalysisAttachmentFileUploaderIndicator
        {
            get { return engAnalysisAttachmentFileUploaderIndicator; }
            set
            {
                engAnalysisAttachmentFileUploaderIndicator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisAttachmentFileUploaderIndicator"));
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

        public string Comments
        {
            get { return comments; }
            set
            {
                if (value != null)
                {
                    comments = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
                }
                else
                {
                    comments = value;
                    string error = EnableValidationAndGetError();
                    OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
                }
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }

        public int SelectedIndexAttachment
        {
            get { return selectedIndexAttachment; }
            set
            {
                selectedIndexAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAttachment"));
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

        public string FileNameString
        {
            get { return fileNameString; }
            set
            {
                fileNameString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileNameString"));
            }
        }

        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }

        public List<Attachment> EngineeringAnalysisAddedAttachmentList
        {
            get { return engineeringAnalysisAttachmentList; }
            set
            {
                engineeringAnalysisAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngineeringAnalysisAddedAttachmentList"));
            }
        }

        public List<object> TempAttachmentListUI
        {
            get { return tempAttachmentList; }
            set
            {
                tempAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempAttachmentListUI"));
            }
        }

        public EngineeringAnalysis EngAnalysis
        {
            get { return engAnalysis; }
            set
            {
                engAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysis"));
            }
        }

        public EngineeringAnalysis EngAnalysisDuplicate
        {
            get { return engAnalysisDuplicate; }
            set
            {
                engAnalysisDuplicate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisDuplicate"));
            }
        }

        public bool IsEngAnalysisChooseFileEnable
        {
            get { return isEngAnalysisChooseFileEnable; }
            set
            {
                isEngAnalysisChooseFileEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisChooseFileEnable"));
            }
        }

        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set
            {
                isAcceptEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable"));
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompleted"));
            }
        }

        public bool IsCompletedReadOnly
        {
            get { return isCompletedReadOnly; }
            set
            {
                isCompletedReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompletedReadOnly"));
            }
        }

        public bool IsCommentsReadOnly
        {
            get { return isCommentsReadOnly; }
            set
            {
                isCommentsReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentsReadOnly"));
            }
        }

        public bool IsAttachmentReadOnly
        {
            get { return isAttachmentReadOnly; }
            set
            {
                isAttachmentReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAttachmentReadOnly"));
            }
        }

        public bool IsDateReadOnly
        {
            get { return isDateReadOnly; }
            set
            {
                isDateReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDateReadOnly"));
            }
        }

        public List<EngineeringAnalysisType> EngineeringAnalysisTypes
        {
            get { return engineeringAnalysisTypes; }
            set
            {
                engineeringAnalysisTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngineeringAnalysisTypes"));
            }
        }

        public List<object> SelectedEngineeringAnalysisTypes
        {
            get { return selectedEngineeringAnalysisTypes; }
            set
            {
                selectedEngineeringAnalysisTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEngineeringAnalysisTypes"));
            }
        }

        public bool IsEngineeringAnalysisTypesReadOnly
        {
            get { return isEngineeringAnalysisTypesReadOnly; }
            set
            {
                isEngineeringAnalysisTypesReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngineeringAnalysisTypesReadOnly"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
               offerCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
            }
        }
        #endregion

        #region Commands

        public ICommand AddEngineeringAnalysisCancelButtonCommand { get; set; }
        public ICommand AddEngineeringAnalysisAcceptButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Events

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

        #region Constructor
        
        public AddEngineeringAnalysisViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEngineeringAnalysisViewModel ...", category: Category.Info, priority: Priority.Low);

                IsCompleted = false;
                IsCompletedReadOnly = !GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27);
                DueDate = DateTime.Now;

                GetEngAnalysisArticles();

                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    IsEngineeringAnalysisTypesReadOnly = true;
                    IsCommentsReadOnly = true;
                    IsAttachmentReadOnly = true;
                    IsDateReadOnly = true;
                }
                else
                {
                    IsEngineeringAnalysisTypesReadOnly = false;
                    IsCommentsReadOnly = false;
                    IsAttachmentReadOnly = false;
                    IsDateReadOnly = false;
                }

               
                AddEngineeringAnalysisCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEngineeringAnalysisAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }
                GeosApplication.Instance.Logger.Log("Constructor AddEngineeringAnalysisViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEngineeringAnalysisViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Method for Initialization.
        /// [001][cpatil][08-04-2020] GEOS2-2224 Partnumber not generated after EDIT Engineering Analysis
        /// [002][cpatil][15-05-2020] GEOS2-2279 Error when trying to modify offers with eng.analysis
        /// </summary>
        public void InIt(GeosStatus selectedGeosStatus)
        {
           
            FillEngineeringAnalysisDetails();

            //  [CRM-M040-02] (#49016) Validate Eng. Analysis
            if (selectedGeosStatus.IdOfferStatusType != 1 && selectedGeosStatus.IdOfferStatusType != 2)
            {
                IsEngineeringAnalysisTypesReadOnly = true;
                IsCommentsReadOnly = true;
                IsAttachmentReadOnly = true;
                IsEngAnalysisChooseFileEnable = false;
                IsDateReadOnly = true;
                IsAcceptEnable = false;
            }

            if (GeosApplication.Instance.IsPermissionReadOnly && GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
            {
                //IsAcceptControlEnableorder = false;
                IsEngAnalysisChooseFileEnable = false;
                IsAcceptEnable = true;
            }
            else if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                //IsAcceptControlEnableorder = false;
                IsEngAnalysisChooseFileEnable = false;
                IsAcceptEnable = false;
            }
            ////[001] Added User not able to unselected Article in status shipped
            //if(!IsEngineeringAnalysisTypesReadOnly)
            //{
            //    if (IsCompleted)
            //        IsEngineeringAnalysisTypesReadOnly = true;
            //    else
            //        IsEngineeringAnalysisTypesReadOnly = false;
            //}
        }

        /// <summary>
        /// [001][2018-07-09][skhade][CRM-M042-18] Eng. analysis option in Orders.
        /// This init method is called always from order.
        /// </summary>
        /// <param name="selectedGeosStatus">Selected geos status of order.</param>
        public void InitOrder(GeosStatus selectedGeosStatus)
        {
            FillEngineeringAnalysisDetails();

            IsEngineeringAnalysisTypesReadOnly = true;
            IsCommentsReadOnly = true;
            IsAttachmentReadOnly = true;
            IsEngAnalysisChooseFileEnable = false;
            IsDateReadOnly = true;
            IsCompletedReadOnly = true;
            IsAcceptEnable = false;
           
        }

        /// <summary>
        /// Method For Fill Analysis Details back
        /// </summary>
        public void FillEngineeringAnalysisDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysisDetails() ...", category: Category.Info, priority: Priority.Low);

                if (EngAnalysisDuplicate != null)
                {
                    Comments = EngAnalysisDuplicate.Comments;
                    DueDate = EngAnalysisDuplicate.DueDate;
                    TempAttachmentListUI = new List<object>();

                    if (EngAnalysisDuplicate.Attachments != null)
                    {
                        foreach (Attachment item in EngAnalysisDuplicate.Attachments)
                        {
                            TempAttachmentListUI.Add((Attachment)item.Clone());
                        }

                        SelectedIndexAttachment = 0;
                    }

                    if (EngAnalysisDuplicate.EngineeringAnalysisTypes != null && EngineeringAnalysisTypes != null)
                    {
                        if (SelectedEngineeringAnalysisTypes == null)
                            SelectedEngineeringAnalysisTypes = new List<object>();

                        foreach (EngineeringAnalysisType item in EngAnalysisDuplicate.EngineeringAnalysisTypes)
                        {
                            EngineeringAnalysisType eat = EngineeringAnalysisTypes.FirstOrDefault(x => x.IdArticle == item.IdArticle);
                            eat.IsArticleEnabled = item.IsArticleEnabled;
                            if (Convert.ToDouble(item.Quantity) > 0)
                            {
                                eat.IsSelected = true;
                                eat.Quantity = item.Quantity;
                                SelectedEngineeringAnalysisTypes.Add(EngineeringAnalysisTypes.FirstOrDefault(x => x.IdArticle == item.IdArticle));
                            }
                        }
                    }

                    IsCompleted = EngAnalysisDuplicate.IsCompleted;
                    if(IsCompleted)
                     IsCompletedReadOnly = true;
                    IsEdit = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillEngineeringAnalysisDetails() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysisDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

       

        /// [001][GEOS2-2217][cpatil][03-04-2020]Eng. Analysis type field not working as expected
        private void GetEngAnalysisArticles()
        {
            try
            {
                //[001] Changed service method GetEngAnalysisArticles to GetEngAnalysisArticles_V2041
                EngineeringAnalysisTypes = CrmStartUp.GetEngAnalysisArticles_V2041();
                EngineeringAnalysisTypes.ToList().ForEach(eat => { eat.IsArticleEnabled = true; });


            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);

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

                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                    Attachment = new Attachment();
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = file.Name;
                    Attachment.IsDeleted = false;
                    Attachment.IsNew = true;

                    var newFileList = (TempAttachmentListUI != null) ? new List<object>(TempAttachmentListUI) : new List<object>();

                    // not allow to add same files
                    List<Attachment> tmpList = newFileList.OfType<Attachment>().ToList();
                    if (!tmpList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }

                    TempAttachmentListUI = newFileList;
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in BrowseFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Upload File
        /// </summary>
        /// <param name="obj"></param>
        public bool UploadFileEngineeringAnalysis(List<object> tmpAttachmentUIList)
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                //List<Attachment> AttachmentDeletedList = new List<Attachment>();

                if (TempAttachmentListUI != null)
                {
                    EngineeringAnalysisAddedAttachmentList = new List<Attachment>();

                    foreach (Attachment item in TempAttachmentListUI)
                    {
                        if (item.IsNew)
                            EngineeringAnalysisAddedAttachmentList.Add(item);

                        //if (item.IsDeleted)
                        //    AttachmentDeletedList.Add(item);
                    }

                    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                    {
                        FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                        List<FileInfo> FileDetail = new List<FileInfo>();
                        FileUploader EngAnalysisAttachmentFileUploader = new FileUploader();
                        EngAnalysisAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                        GuidCode = EngAnalysisAttachmentFileUploader.FileUploadName;

                        foreach (Attachment fs in EngineeringAnalysisAddedAttachmentList)
                        {
                            FileInfo file = new FileInfo(fs.FilePath);
                            FileDetail.Add(file);
                        }

                        EngAnalysisAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, EngAnalysisAttachmentFileUploader.FileUploadName);
                        EngAnalysisAttachmentFileUploaderIndicator = EngAnalysisAttachmentFileUploader;
                        GeosApplication.Instance.Logger.Log("Getting Upload Engineering Analysis Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    }
                }

                if (IsEdit)
                {
                    //if (AttachmentDeletedList != null && AttachmentDeletedList.Count > 0)
                    //{
                    //    foreach (Attachment item in AttachmentDeletedList)
                    //    {
                    //        EngineeringAnalysisAddedAttachmentList.Add(item);
                    //    }

                    //    isupload = true;
                    //}
                    //else 

                    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                    {
                        isupload = true;
                    }
                }

                isupload = true;
            }
            catch (FaultException<ServiceException> ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }

        /// <summary>
        /// Method For Accept Button
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptButtonCommandAction(object obj)
        {
            List<Attachment> listFinal = new List<Attachment>();
            //bool isDeleted = false;

            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Comments"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedEngineeringAnalysisTypes"));

                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexAttachment"));

                if (error != null)
                {
                    IsBusy = false;
                    return;
                }
                else
                {
                    //Cloned object
                    if (EngAnalysisDuplicate == null)
                        EngAnalysisDuplicate = new EngineeringAnalysis();

                    EngAnalysisDuplicate.Comments = Comments;
                    EngAnalysisDuplicate.DueDate = DueDate;
                    EngAnalysisDuplicate.IsCompleted = IsCompleted;
                   

                    if (SelectedEngineeringAnalysisTypes != null)
                    {
                        EngAnalysisDuplicate.EngineeringAnalysisTypes = new List<EngineeringAnalysisType>(SelectedEngineeringAnalysisTypes.Cast<EngineeringAnalysisType>());
                        EngAnalysisDuplicate.EngineeringAnalysisTypes.ForEach(eat => { eat.IsSelected = true; eat.Quantity = "1"; });
                    }
                      

                    if (EngAnalysis == null)
                        EngAnalysis = new EngineeringAnalysis();

                    EngAnalysis.Comments = Comments;
                    EngAnalysis.DueDate = DueDate;
                    EngAnalysis.IsCompleted = IsCompleted;

                    if (SelectedEngineeringAnalysisTypes != null)
                    {
                        EngAnalysis.EngineeringAnalysisTypes = new List<EngineeringAnalysisType>(SelectedEngineeringAnalysisTypes.Cast<EngineeringAnalysisType>());
                        EngAnalysisDuplicate.EngineeringAnalysisTypes.ForEach(eat => { eat.IsSelected = true; eat.Quantity = "1"; });
                    }
                     

                    if (TempAttachmentListUI == null || TempAttachmentListUI.Count == 0)
                    {
                        if (EngAnalysis.Attachments != null && EngAnalysis.Attachments.Count > 0)
                            EngAnalysis.Attachments.ForEach(i => i.IsDeleted = true);

                        EngAnalysisDuplicate.Attachments = new List<Attachment>();
                    }
                    else if (TempAttachmentListUI != null && TempAttachmentListUI.Count > 0)
                    {
                        List<Attachment> listAsUIAttachment = TempAttachmentListUI.Cast<Attachment>().ToList();

                        if (EngAnalysis.Attachments != null)
                            foreach (Attachment item in EngAnalysis.Attachments)
                            {
                                if (!listAsUIAttachment.Any(x => x.OriginalFileName == item.OriginalFileName))
                                {
                                    item.IsDeleted = true;
                                }
                            }

                        bool isupload = UploadFileEngineeringAnalysis(TempAttachmentListUI);

                        if (!isupload)
                        {
                            IsBusy = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }

                        if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                        {
                            if (EngAnalysis.Attachments == null)
                                EngAnalysis.Attachments = new List<Attachment>();

                            EngAnalysis.Attachments.AddRange(EngineeringAnalysisAddedAttachmentList);
                        }

                        EngAnalysisDuplicate.Attachments = TempAttachmentListUI.Cast<Attachment>().ToList();
                    }

                    EngAnalysis.GUIDString = GuidCode;
                    IsSave = true;

                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                // could supply a default Icon here as well
            }

            return result;
        }

        /// <summary>
        /// Method for convert zip to byte.
        /// </summary>
        /// <param name="filesDetail"></param>
        /// <param name="GuidCode"></param>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";

            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
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
                            if (!File.Exists(tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName);
                            }

                            string s = tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName;
                            archive.AddFile(s, @"/");
                            EngineeringAnalysisAddedAttachmentList[i].FilePath = s;
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
                GeosApplication.Instance.Logger.Log(string.Format("Error On ConvertZipToByte Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => Comments)] +
                    me[BindableBase.GetPropertyName(() => DueDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedEngineeringAnalysisTypes)];

                //me[BindableBase.GetPropertyName(() => SelectedIndexAttachment)] +

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string comments = BindableBase.GetPropertyName(() => Comments);
                string dueDate = BindableBase.GetPropertyName(() => DueDate);
                string selectedEngineeringAnalysisTypes = BindableBase.GetPropertyName(() => SelectedEngineeringAnalysisTypes);

                //string selectedIndexAttachment = BindableBase.GetPropertyName(() => SelectedIndexAttachment);

                if (columnName == comments)                                 //Comments
                {
                    return EngineeringAnalysisValidation.GetErrorMessage(comments, Comments);
                }
                else if (columnName == dueDate)                             //Due date.
                {
                    return EngineeringAnalysisValidation.GetErrorMessage(dueDate, DueDate);
                }
                else if (columnName == selectedEngineeringAnalysisTypes)    // Selected EngineeringAnalysis Types.
                {
                    return EngineeringAnalysisValidation.GetErrorMessage(selectedEngineeringAnalysisTypes, SelectedEngineeringAnalysisTypes);
                }

                //else if (columnName == selectedIndexAttachment)           //AttachmentType
                //{
                //    return EngineeringAnalysisValidation.GetErrorMessage(selectedIndexAttachment, SelectedIndexAttachment);
                //}

                return null;
            }
        }

        #endregion
    }
}
