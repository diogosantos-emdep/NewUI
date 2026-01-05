using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Modules.PCM.Views;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.IO;
using DevExpress.Mvvm;
using System.ComponentModel;
using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddEditAttachmentFileInPCMArticleViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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


        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;

        private ArticleDocument selectedArticleFile;
        private byte[] fileInBytes;
        private List<ArticleDocument> articleAttachmentList;
        private List<Object> attachmentObjectList;
        private string articleSavedFileName;
        private string fileName;
        private string description;
        private DocumentType selectedType;
        private DateTime updatedDate;
        private uint idCPTypeAttachedDoc;
        private ObservableCollection<DocumentType> articleTypeList;
        private byte isShareWithCustomer;

        string FileTobeSavedByName = "";
        private string error = string.Empty;

        #endregion


        #region Properties

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
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

        public ArticleDocument SelectedArticleFile
        {
            get
            {
                return selectedArticleFile;
            }

            set
            {
                selectedArticleFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleFile"));
            }
        }

        public byte[] FileInBytes
        {
            get
            {
                return fileInBytes;
            }

            set
            {
                fileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileInBytes"));
            }
        }

        public List<ArticleDocument> ArticleAttachmentList
        {
            get
            {
                return articleAttachmentList;
            }

            set
            {
                articleAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleAttachmentList"));

            }
        }

        public List<object> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }

        public string ArticleSavedFileName
        {
            get
            {
                return articleSavedFileName;
            }
            set
            {
                articleSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSavedFileName"));
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

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public DocumentType SelectedType
        {
            get
            {
                return selectedType;
            }

            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }

        public DateTime UpdatedDate
        {
            get
            {
                return updatedDate;
            }

            set
            {
                updatedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedDate"));

            }
        }

        public uint IdCPTypeAttachedDoc
        {
            get
            {
                return idCPTypeAttachedDoc;
            }

            set
            {
                idCPTypeAttachedDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCPTypeAttachedDoc"));

            }
        }

        public ObservableCollection<DocumentType> ArticleTypeList
        {
            get
            {
                return articleTypeList;
            }
            set
            {
                articleTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleTypeList"));
            }
        }

        public byte IsShareWithCustomer
        {
            get { return isShareWithCustomer; }
            set
            {
                isShareWithCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShareWithCustomer"));
            }
        }

        #endregion

        #region ICommand
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand SelectedTypeChangedCommand { get; set; }


        #endregion

        #region Constructor
        public AddEditAttachmentFileInPCMArticleViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddFileInProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptFileActionCommand = new DelegateCommand<object>(ArticleFileAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddFileInProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddFileInProductTypeViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            ArticleTypeList = new ObservableCollection<DocumentType>(PCMService.GetDocumentTypes());
            SelectedType = ArticleTypeList.FirstOrDefault();
        }

        private void ArticleFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleFileAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleFile"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                if (Description != null)
                {
                    if (Description.Contains("\r\n"))
                    {
                        Description = Description.TrimEnd(trimChars);
                        Description = Description.TrimStart(trimChars);
                    }
                }

                if (IsNew)
                {
                    SelectedArticleFile = new ArticleDocument();

                    if (string.IsNullOrEmpty(FileName))
                    {
                        if (string.IsNullOrEmpty(ArticleSavedFileName))
                        {
                            RequestClose(null, null);
                            GeosApplication.Instance.Logger.Log("Method ArticleFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                            return;
                        }
                        if (!string.IsNullOrEmpty(ArticleSavedFileName))
                        {
                            FileName = ArticleSavedFileName;
                            #region [GEOS2-6712][rdixit][06.12.2024]
                            //int index = ArticleSavedFileName.LastIndexOf('.');
                            //FileTobeSavedByName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                            //FileName = FileTobeSavedByName;
                            #endregion
                        }
                    }
                    FileInfo file = new FileInfo(ArticleSavedFileName);
                    SelectedArticleFile.SavedFileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + file.Extension;
                    SelectedArticleFile.OriginalFileName = FileName;
                    SelectedArticleFile.PCMArticleFileInBytes = FileInBytes;
                    SelectedArticleFile.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedArticleFile.IdDocType = SelectedType.IdDocumentType;
                    //SelectedArticleFile.IdArticleDoc = IdCPTypeAttachedDoc;
                    SelectedArticleFile.Description = Description;
                    SelectedArticleFile.CreatedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedArticleFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedArticleFile.ArticleDocumentType = new ArticleDocumentType();
                    SelectedArticleFile.ArticleDocumentType.DocumentType = SelectedType.Name;
                    SelectedArticleFile.IsShareWithCustomer = IsShareWithCustomer; 
                    IsSave = true;
                }
                else
                {
                    if (AttachmentObjectList == null)
                    {
                        RequestClose(null, null);
                        GeosApplication.Instance.Logger.Log("Method ArticleFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                        return;
                    }
                    SelectedArticleFile = new ArticleDocument();

                    if (!string.IsNullOrEmpty(ArticleSavedFileName))
                    {
                        FileName = ArticleSavedFileName;
                        #region [GEOS2-6712][rdixit][06.12.2024]
                        //int index = ArticleSavedFileName.LastIndexOf('.');
                        //FileName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                        //SelectedArticleFile.SavedFileName = ArticleSavedFileName;
                        #endregion
                    }
                    FileInfo file = new FileInfo(ArticleSavedFileName);
                    ArticleSavedFileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + file.Extension;
                    SelectedArticleFile.OriginalFileName = FileName;
                    SelectedArticleFile.IdArticleDoc = IdCPTypeAttachedDoc;
                    SelectedArticleFile.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedArticleFile.IdDocType = SelectedType.IdDocumentType;
                    SelectedArticleFile.Description = Description;
                    SelectedArticleFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedArticleFile.PCMArticleFileInBytes = FileInBytes;
                    SelectedArticleFile.ArticleDocumentType = new ArticleDocumentType();
                    SelectedArticleFile.ArticleDocumentType.DocumentType = SelectedType.Name;
                    SelectedArticleFile.IsShareWithCustomer = IsShareWithCustomer;

                    IsSave = true;
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ArticleFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ArticleFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    ArticleAttachmentList = new List<ArticleDocument>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ArticleSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        #region [GEOS2-6712][rdixit][06.12.2024]
                        //int index = FileName.LastIndexOf('.');
                        //FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        //FileName = FileTobeSavedByName;
                        #endregion
                    }
                    List<ArticleDocument> newAttachmentList = new List<ArticleDocument>();
                    ArticleDocument attachment = new ArticleDocument();
                    attachment.SavedFileName = file.Name;
                    attachment.OriginalFileName = file.Name;
                    attachment.PCMArticleFileInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    ArticleAttachmentList = newAttachmentList;

                    if (ArticleAttachmentList.Count > 0)
                    {
                        SelectedArticleFile = attachment;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BrowseFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        public void EditInit(ArticleDocument articleAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdCPTypeAttachedDoc = (uint)articleAttachedDoc.IdArticleDoc;
                FileName = articleAttachedDoc.OriginalFileName;

                Description = articleAttachedDoc.Description;
                ArticleSavedFileName = articleAttachedDoc.SavedFileName;
                FileInBytes = articleAttachedDoc.PCMArticleFileInBytes;
                FileName = articleAttachedDoc.OriginalFileName;

                ArticleAttachmentList = new List<ArticleDocument>();
                ArticleAttachmentList.Add(articleAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)articleAttachedDoc);
                SelectedArticleFile = articleAttachedDoc;

                ArticleTypeList = new ObservableCollection<DocumentType>(PCMService.GetDocumentTypes());

                SelectedType = ArticleTypeList.Where(x => x.Name == articleAttachedDoc.ArticleDocumentType.DocumentType).FirstOrDefault();
                IsShareWithCustomer = articleAttachedDoc.IsShareWithCustomer;

                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        public void ClearAllProperties()
        {
            FileName = string.Empty;
            ArticleSavedFileName = string.Empty;
            FileInBytes = null;
        }

        //private void SelectedTypeChangedAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method SelectedTestChangedAction()...", category: Category.Info, priority: Priority.Low);

        //    try
        //    {
        //        SelectedType.Name = SelectedArticleFile.ArticleDocumentType.DocumentType.ToString();


        //        GeosApplication.Instance.Logger.Log("Method SelectedTestChangedAction()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an Error in Method SelectedTestChangedAction()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => SelectedArticleFile)];

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

                string ArticleFile = BindableBase.GetPropertyName(() => SelectedArticleFile);

                if (columnName == ArticleFile)
                {
                    return AddEditModuleValidation.GetErrorMessage(ArticleFile, SelectedArticleFile);
                }

                return null;
            }
        }
        #endregion
    }
}
