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
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddEditFileInPCMArticleViewModel : ViewModelBase, INotifyPropertyChanged
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
        private ObservableCollection<ArticleDocument> articleAttachmentList;
        private List<Object> attachmentObjectList;
        private string articleSavedFileName;
        private string fileName;
        private string description;
        private DateTime updatedDate;
        private uint idCPTypeAttachedDoc;

        string FileTobeSavedByName = "";

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

        public ObservableCollection<ArticleDocument> ArticleAttachmentList
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

        #endregion

        #region ICommand
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }


        #endregion

        #region Constructor
        public AddEditFileInPCMArticleViewModel()
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


        private void ArticleFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleFileAction()...", category: Category.Info, priority: Priority.Low);

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
                        int index = ArticleSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedArticleFile.OriginalFileName = FileName;
                    SelectedArticleFile.SavedFileName = ArticleSavedFileName;
                    //SelectedArticleFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedArticleFile.IdDocType = 1;
                    //SelectedArticleFile.ProductTypeAttachedDocInBytes = FileInBytes;
                    SelectedArticleFile.Description = Description;
                    //SelectedArticleFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;
                }
                else
                {
                    SelectedArticleFile = new ArticleDocument();
                    SelectedArticleFile.SavedFileName = ArticleSavedFileName;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = ArticleSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedArticleFile.OriginalFileName = FileName;
                    //SelectedArticleFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedArticleFile.IdDocType = 1;
                    //SelectedArticleFile.ProductTypeAttachedDocInBytes = FileInBytes;
                    SelectedArticleFile.Description = Description;
                    //SelectedArticleFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
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
                    ArticleAttachmentList = new ObservableCollection<ArticleDocument>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ArticleSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<ArticleDocument> newAttachmentList = new ObservableCollection<ArticleDocument>();
                    ArticleDocument attachment = new ArticleDocument();
                    attachment.SavedFileName = file.Name;
                    //attachment.ProductTypeAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    ArticleAttachmentList = newAttachmentList;

                    if (ArticleAttachmentList.Count > 0)
                    {
                        SelectedArticleFile = ArticleAttachmentList[0];
                    }
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() executed successfully", category: Category.Info, priority: Priority.Low);
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

        public void EditInit(ArticleDocument productTypeAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdCPTypeAttachedDoc = (uint)productTypeAttachedDoc.IdArticleDoc;
                FileName = productTypeAttachedDoc.OriginalFileName;

                Description = productTypeAttachedDoc.Description;
                ArticleSavedFileName = productTypeAttachedDoc.SavedFileName;
                //FileInBytes = productTypeAttachedDoc.ProductTypeAttachedDocInBytes;

                ArticleAttachmentList = new ObservableCollection<ArticleDocument>();
                ArticleAttachmentList.Add(productTypeAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)productTypeAttachedDoc);
                SelectedArticleFile = productTypeAttachedDoc;

                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        #endregion
    }
}
