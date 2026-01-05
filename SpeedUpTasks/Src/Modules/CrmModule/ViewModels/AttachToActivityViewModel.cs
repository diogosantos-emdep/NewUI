using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
//using Microsoft.Office.Interop.Outlook;
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
    public class AttachToActivityViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private string emailItemPath;
        private string emailItemSubject;
        private object selectedActivity;
        private bool isAccept;
        private List<ActivityGrid> activityList;
        private bool isCompleted;

        #endregion

        #region Properties

        public string GuidCode { get; set; }

        public List<ActivityGrid> ActivityList
        {
            get { return activityList; }
            set
            {
                activityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivityList"));
            }
        }

        public object SelectedActivity
        {
            get { return selectedActivity; }
            set
            {
                selectedActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivity"));
            }
        }

        public bool IsAccept
        {
            get { return isAccept; }
            set
            {
                isAccept = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccept"));
            }
        }

        #endregion

        #region ICommands

        public ICommand AttachToActivityViewAcceptButtonCommand { get; set; }
        public ICommand AttachToActivityViewCancelButtonCommand { get; set; }


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

        public AttachToActivityViewModel(string Subject, string mailPath)
        {
            try
            {
                emailItemPath = mailPath;
                emailItemSubject = Subject;
                CRMCommon.Instance.Init();
                GeosApplication.Instance.Logger.Log("Constructor AttachToActivityViewModel ...", category: Category.Info, priority: Priority.Low);

                AttachToActivityViewAcceptButtonCommand = new DelegateCommand<object>(AttachActivityAccept);
                AttachToActivityViewCancelButtonCommand = new DelegateCommand<object>(CloseWindow);

                FillActivityList();

                GeosApplication.Instance.Logger.Log("Constructor AttachToActivityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttachToActivityViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods

        public void AddAttachement(Activity activity)
        {
            try
            {
                FileInfo file = new FileInfo(emailItemPath);
                string FileName = file.FullName;
                string UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                ActivityAttachment Attachment = new ActivityAttachment();
                Attachment.FileType = file.Extension;
                Attachment.FilePath = file.FullName;
                Attachment.OriginalFileName = file.Name;
                Attachment.SavedFileName = UniqueFileName + file.Extension;
                Attachment.UploadedIn = DateTime.Now;
                Attachment.FileSize = file.Length;
                Attachment.FileType = file.Extension;
                Attachment.FileUploadName = UniqueFileName + file.Extension;
                Attachment.IsDeleted = 0;

                activity.ActivityAttachment = new List<ActivityAttachment>();
                activity.ActivityAttachment.Add(Attachment);
            }
            catch (Exception ex)
            {
                ////IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
        }

        /// <summary>
        /// Method for add new tag.
        /// </summary>
        /// <param name="obj"></param>
        //public void AttachActivityAccept(MailItem emailItem)
        public void AttachActivityAccept(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AttachActivityAccept() Method ...", category: Category.Info, priority: Priority.Low);

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedActivity"));
                ActivityGrid activityGrid = SelectedActivity as ActivityGrid;

                Activity activity = new Activity();
                activity.IdActivity = activityGrid.IdActivity;

                AddAttachement(activity);
                UploadActivityAttachment(activity.ActivityAttachment);
                activity.GUIDString = GuidCode;

                activity.LogEntriesByActivity = new List<LogEntriesByActivity>();

                // Comments
                LogEntriesByActivity activityComment = new LogEntriesByActivity();
                activityComment.IdActivity = activity.IdActivity;
                activityComment.IdLogEntryType = 1;
                activityComment.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                activityComment.Comments = emailItemSubject;
                activityComment.Datetime = DateTime.Now;
                activityComment.IsRtfText = false;
                activity.LogEntriesByActivity.Add(activityComment);

                // LogEntry
                LogEntriesByActivity logEntriesByActivity = new LogEntriesByActivity();
                logEntriesByActivity.IdLogEntryType = 2;
                logEntriesByActivity.IdActivity = activity.IdActivity;
                logEntriesByActivity.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                logEntriesByActivity.Comments = "The activity attachment from outlook has been added.";
                logEntriesByActivity.Datetime = DateTime.Now;
                logEntriesByActivity.IsRtfText = false;
                activity.LogEntriesByActivity.Add(logEntriesByActivity);

                IsAccept = CrmStartUp.UpdateAttachActivity(activity);

                if (IsAccept)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditActivityAttachmentAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AttachActivityAccept() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachActivityAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachActivityAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in AttachActivityAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Upload File
        /// </summary>
        public bool UploadActivityAttachment(List<ActivityAttachment> ActivityAttachments)
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFile() ...", category: Category.Info, priority: Priority.Low);

                //IsBusy = true;
                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileUploader activityAttachmentFileUploader = new FileUploader();
                activityAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = activityAttachmentFileUploader.FileUploadName;

                if (ActivityAttachments != null && ActivityAttachments.Count > 0)
                {
                    foreach (ActivityAttachment fs in ActivityAttachments)
                    {
                        FileInfo file = new FileInfo(fs.FilePath);
                        fs.AttachmentImage = null;
                        FileDetail.Add(file);
                    }

                    activityAttachmentFileUploader.FileByte = ConvertZipToByte(ActivityAttachments, FileDetail, activityAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderActivityAttachmentZipFile(activityAttachmentFileUploader);
                    GeosApplication.Instance.Logger.Log("Getting Upload activity Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
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
                    GeosApplication.Instance.Logger.Log("Method UploadFile() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    //IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                //IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<ActivityAttachment> ActivityAttachments, List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolderForEdit\";

            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
            }

            try
            {
                GeosApplication.Instance.Logger.Log("Add files into zip", category: Category.Info, priority: Priority.Low);

                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            ActivityAttachments[i].FileUploadName = ActivityAttachments[i].SavedFileName;
                            System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + ActivityAttachments[i].SavedFileName);
                            string s = tempfolderPath + ActivityAttachments[i].SavedFileName;
                            archive.AddFile(s, @"/");
                            ActivityAttachments[i].FilePath = s;
                        }

                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }

                GeosApplication.Instance.Logger.Log("Zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error On ConvertZipToByte Method", category: Category.Exception, priority: Priority.Low);
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
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        //double StringSimilarityScore(string name, string searchString)
        //{
        //    if (name.Contains(searchString))
        //    {
        //        return (double)searchString.Length / (double)name.Length;
        //    }

        //    return 0;
        //}

        private void FillActivityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillActivityList ...", category: Category.Info, priority: Priority.Low);

                ActivityParams objTimelineParams = new ActivityParams();
                objTimelineParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                    {
                        List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                        objTimelineParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                        objTimelineParams.idOwner = salesOwnersIds;
                        objTimelineParams.idPermission = 21;
                        objTimelineParams.idPlant = "0";
                        //objTimelineParams.activeSite = new ActiveSite { IdSite = 18, SiteAlias = "EAES", SiteServiceProvider = "10.0.9.202:84" };
                        objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                        objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;

                        //objTimelineParams.accountingYearFrom = Convert.ToDateTime("01-Jan-18 12:00:00 AM");
                        //objTimelineParams.accountingYearTo = Convert.ToDateTime("01-Jan-20 12:00:00 AM");
                    }
                    //else
                    //{
                    //    ActivityList = new ObservableCollection<Activ>();
                    //}
                    //objTimelineParams.idPermission = GeosApplication.Instance.IdUserPermission;
                    //objTimelineParams.idPlant = GeosApplication.Instance.ActiveIdSite.ToString();// "18";
                    //objTimelineParams.idOwner = GeosApplication.Instance. .ToString();// "28";
                }
                else if (GeosApplication.Instance.IdUserPermission == 22)
                {
                    if (GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                    {
                        List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                        var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                        objTimelineParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                        objTimelineParams.idOwner = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                        objTimelineParams.idPermission = 22;
                        objTimelineParams.idPlant = plantOwnersIds;
                        objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                        objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;

                    }
                    //else
                    //{
                    //    ActivityGridList = new ObservableCollection<ActivityGrid>();
                    //}
                }
                else
                {
                    objTimelineParams.idActiveUser = GeosApplication.Instance.ActiveUser.IdUser;
                    objTimelineParams.idOwner = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                    objTimelineParams.idPermission = 20;
                    objTimelineParams.idPlant = "0";
                    objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                    objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                }

                ActivityList = CrmStartUp.GetActivitiesDetail(objTimelineParams).ToList();

                GeosApplication.Instance.Logger.Log("Method FillActivityList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                //{
                //    DXSplashScreen.Close();
                //}
                //GeosApplication.Instance.Logger.Log("Get an error in FillActivityList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                //CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                //{
                //    DXSplashScreen.Close();
                //}
                //GeosApplication.Instance.Logger.Log("Get an error in FillActivityList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillActivityList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void ShowPopupAsPerTagName(string ProjectName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName ...", category: Category.Info, priority: Priority.Low);

            //TagNameList = CrmStartUp.GetAllTags().ToList();

            //if (TagNameList != null && !string.IsNullOrEmpty(TagName))
            //{
            //    if (TagName.Length > 1)
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 2).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();
            //    }
            //    else
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 1).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();

            //    }
            //}

            //else
            //{
            //    TagNameList = new List<Tag>();
            //    TagNameStrList = new List<string>();
            //}

            ////For alert Icon visibility
            //if (TagNameStrList.Count > 0)
            //{
            //    AlertVisibility = Visibility.Visible;
            //}
            //else
            //    AlertVisibility = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
