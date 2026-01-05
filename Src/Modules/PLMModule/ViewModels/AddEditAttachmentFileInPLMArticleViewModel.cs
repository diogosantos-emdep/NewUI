
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
using System.Windows;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    class AddEditAttachmentFileInPLMArticleViewModel:ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //IPLMService PLMService = new PLMServiceController("localhost:6699");
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
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
       
        private CPLDocument selectedCustomerPriceFile;
        private BPLDocument selectedBasicPriceFile;
        private byte[] fileInBytes;
        private List<BPLDocument> bPLAttachmentList;
        private List<CPLDocument> cPLAttachmentList;
        private List<Object> attachmentObjectList;

        private List<Object> cPLAttachmentObjectList;
        private string articleSavedFileName;
        private string fileName;
        private Int32 idBasePriceList;
        private string description;
        private LookupValue cplselectedType;
        private LookupValue bplselectedType;
        private DateTime updatedDate;
        private uint idCPTypeAttachedDoc;
        private List<LookupValue> bPLTypeList;
        private List<LookupValue> cPLTypeList;
        private byte isShareWithCustomer;
        private bool isCPLAttachment;
        string FileTobeSavedByName = "";
        private string error = string.Empty;
        Visibility isCPLTypeAttachment = Visibility.Collapsed;
        string[] fileUploadDargDropFromFileExplorer;
        Visibility isBPLTypeAttachment = Visibility.Collapsed;
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

       
        public BPLDocument SelectedBasicPriceFile
        {
            get
            {
                return selectedBasicPriceFile;
            }

            set
            {
                selectedBasicPriceFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedBasicPriceFile"));
            }
        }
        public CPLDocument SelectedCustomerPriceFile
        {
            get
            {
                return selectedCustomerPriceFile;
            }

            set
            {
                selectedCustomerPriceFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerPriceFile"));
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

        public List<BPLDocument> BPLAttachmentList
        {
            get
            {
                return bPLAttachmentList;
            }

            set
            {
                bPLAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BPLAttachmentList"));

            }
        }

        public List<CPLDocument> CPLAttachmentList
        {
            get
            {
                return cPLAttachmentList;
            }

            set
            {
                cPLAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLAttachmentList"));

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

        public List<object> CPLAttachmentObjectList
        {
            get { return cPLAttachmentObjectList; }
            set
            {
                cPLAttachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLAttachmentObjectList"));
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

        public LookupValue BPLSelectedType
        {
            get
            {
                return bplselectedType;
            }

            set
            {
                bplselectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BPLSelectedType"));
            }
        }

        public LookupValue CPLSelectedType
        {
            get
            {
                return cplselectedType;
            }

            set
            {
                cplselectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLSelectedType"));
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

        public List<LookupValue> BPLTypeList
        {
            get
            {
                return bPLTypeList;
            }
            set
            {
                bPLTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BPLTypeList"));
            }
        }
        public List<LookupValue> CPLTypeList
        {
            get
            {
                return cPLTypeList;
            }
            set
            {
                cPLTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLTypeList"));
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
        public Int32 IdBasePriceList
        {
            get
            {
                return idBasePriceList;
            }
            set
            {
                idBasePriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdBasePriceList"));
            }
        }
        public bool IsCPLAttachment
        {
            get
            {
                return isCPLAttachment;
            }

            set
            {
                isCPLAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLAttachment"));
            }
        }
        public Visibility IsBPLTypeAttachment
        {
            get { return isBPLTypeAttachment; }
            set
            {
                isBPLTypeAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBPLTypeAttachment"));
            }
        }
        public Visibility IsCPLTypeAttachment
        {
            get { return isCPLTypeAttachment; }
            set
            {
                isCPLTypeAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCPLTypeAttachment"));
            }
        }

        public string[] FileUploadDargDropFromFileExplorer
        {
            get { return fileUploadDargDropFromFileExplorer; }
            set
            {
                fileUploadDargDropFromFileExplorer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileUploadDargDropFromFileExplorer"));
                BrowseFileActionFileUploaded(FileUploadDargDropFromFileExplorer);
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
        public AddEditAttachmentFileInPLMArticleViewModel()
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
            //IsCPLTypeAttachment = Visibility.Hidden;
            IsBPLTypeAttachment = Visibility.Visible;
            
            BPLTypeList = new List<LookupValue>(PCMService.GetLookupValues(106));
            BPLTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
            BPLSelectedType = BPLTypeList.FirstOrDefault();
          
        }
        public void CPLInit()
        {
            IsCPLTypeAttachment = Visibility.Visible;
            CPLTypeList = new List<LookupValue>(PCMService.GetLookupValues(106));
            CPLTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
            CPLSelectedType = CPLTypeList.FirstOrDefault();
        }
        private void ArticleFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleFileAction()...", category: Category.Info, priority: Priority.Low);

               
               // CustomerPriceListGridViewModel customerPriceListGridViewModel = new CustomerPriceListGridViewModel();
                if (IsCPLTypeAttachment == Visibility.Visible)
                {
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomerPriceFile"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CPLSelectedType"));
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
                        SelectedCustomerPriceFile = new CPLDocument();

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
                                int index = ArticleSavedFileName.LastIndexOf('.');
                                FileTobeSavedByName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                                FileName = FileTobeSavedByName;
                            }

                        }


                       
                        SelectedCustomerPriceFile.SavedFileName = ArticleSavedFileName;
                        SelectedCustomerPriceFile.OriginalFileName = FileName;
                        SelectedCustomerPriceFile.PLMArticleFileInBytes = FileInBytes;
                        SelectedCustomerPriceFile.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedCustomerPriceFile.IdAttachmentType = CPLSelectedType.IdLookupValue;
                        //SelectedArticleFile.IdArticleDoc = IdCPTypeAttachedDoc;
                        SelectedCustomerPriceFile.Description = Description;
                        SelectedCustomerPriceFile.CreatedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedCustomerPriceFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedCustomerPriceFile.ArticleDocumentType = new ArticleDocumentType();
                        SelectedCustomerPriceFile.ArticleDocumentType.DocumentType = CPLSelectedType.Value;
                        SelectedCustomerPriceFile.IsShareWithCustomer = IsShareWithCustomer;
                        IsSave = true;
                    }
                    else
                    {
                        
                        if (CPLAttachmentObjectList == null)
                        {
                            RequestClose(null, null);
                            GeosApplication.Instance.Logger.Log("Method ArticleFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                            return;
                        }
                        SelectedCustomerPriceFile = new CPLDocument();

                        if (!string.IsNullOrEmpty(ArticleSavedFileName))
                        {
                            int index = ArticleSavedFileName.LastIndexOf('.');
                            FileName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                            SelectedCustomerPriceFile.SavedFileName = ArticleSavedFileName;
                        }


                        SelectedCustomerPriceFile.OriginalFileName = FileName;
                        SelectedCustomerPriceFile.IdCustomerPriceListDoc = IdCPTypeAttachedDoc;
                        SelectedCustomerPriceFile.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedCustomerPriceFile.IdAttachmentType = CPLSelectedType.IdLookupValue;
                        SelectedCustomerPriceFile.Description = Description;
                        SelectedCustomerPriceFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedCustomerPriceFile.PLMArticleFileInBytes = FileInBytes;
                        SelectedCustomerPriceFile.ArticleDocumentType = new ArticleDocumentType();
                        SelectedCustomerPriceFile.ArticleDocumentType.DocumentType = CPLSelectedType.Value;
                        SelectedCustomerPriceFile.IsShareWithCustomer = IsShareWithCustomer;

                        IsSave = true;
                    }

                }
                else
                {
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedBasicPriceFile"));
                    PropertyChanged(this, new PropertyChangedEventArgs("BPLSelectedType"));

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
                        SelectedBasicPriceFile = new BPLDocument();

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
                                int index = ArticleSavedFileName.LastIndexOf('.');
                                FileTobeSavedByName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                                FileName = FileTobeSavedByName;
                            }

                        }


                        SelectedBasicPriceFile.IdBasePriceList = IdBasePriceList;
                        SelectedBasicPriceFile.SavedFileName = ArticleSavedFileName;
                        SelectedBasicPriceFile.OriginalFileName = FileName;
                        SelectedBasicPriceFile.PLMArticleFileInBytes = FileInBytes;
                        SelectedBasicPriceFile.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedBasicPriceFile.IdAttachmentType = BPLSelectedType.IdLookupValue;
                        //SelectedArticleFile.IdArticleDoc = IdCPTypeAttachedDoc;
                        SelectedBasicPriceFile.Description = Description;
                        SelectedBasicPriceFile.CreatedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedBasicPriceFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedBasicPriceFile.ArticleDocumentType = new ArticleDocumentType();
                        SelectedBasicPriceFile.ArticleDocumentType.DocumentType = BPLSelectedType.Value;
                    
                        SelectedBasicPriceFile.IsShareWithCustomer = IsShareWithCustomer;
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
                        SelectedBasicPriceFile = new BPLDocument();

                        if (!string.IsNullOrEmpty(ArticleSavedFileName))
                        {
                            int index = ArticleSavedFileName.LastIndexOf('.');
                            FileName = index == -1 ? ArticleSavedFileName : ArticleSavedFileName.Substring(0, index);
                            SelectedBasicPriceFile.SavedFileName = ArticleSavedFileName;
                        }


                        SelectedBasicPriceFile.OriginalFileName = FileName;
                        SelectedBasicPriceFile.IdBasepricedoc = IdCPTypeAttachedDoc;
                        SelectedBasicPriceFile.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedBasicPriceFile.IdAttachmentType = BPLSelectedType.IdLookupValue;
                        SelectedBasicPriceFile.Description = Description;
                        SelectedBasicPriceFile.ModifiedIn = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedBasicPriceFile.PLMArticleFileInBytes = FileInBytes;
                        SelectedBasicPriceFile.ArticleDocumentType = new ArticleDocumentType();
                        SelectedBasicPriceFile.ArticleDocumentType.DocumentType = BPLSelectedType.Value;
                        SelectedBasicPriceFile.IsShareWithCustomer = IsShareWithCustomer;

                        IsSave = true;
                    }
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ArticleFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ArticleFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void BrowseFileActionFileUploaded(string[] fileUploaded)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            
           
            try
            {
                foreach (string item in fileUploaded)
                {
                    string extension = System.IO.Path.GetExtension(item);
                    if (IsCPLTypeAttachment == Visibility.Visible)
                    {
                      

                        if (extension == ".pdf")
                        {
                        
                            FileInBytes = System.IO.File.ReadAllBytes(item);
                            CPLAttachmentList = new List<CPLDocument>();

                            FileInfo file = new FileInfo(item);
                            ArticleSavedFileName = file.Name;
                            if (string.IsNullOrEmpty(FileName))
                            {
                                FileName = file.Name;
                                int index = FileName.LastIndexOf('.');
                                FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                                FileName = FileTobeSavedByName;
                            }
                            List<CPLDocument> newAttachmentList = new List<CPLDocument>();
                            CPLDocument attachments = new CPLDocument();
                            attachments.SavedFileName = file.Name;
                            attachments.OriginalFileName = file.Name;
                            attachments.PLMArticleFileInBytes = FileInBytes;

                            CPLAttachmentObjectList = new List<object>();
                            CPLAttachmentObjectList.Add(attachments);

                            newAttachmentList.Add(attachments);
                            CPLAttachmentList = newAttachmentList;

                            if (CPLAttachmentList.Count > 0)
                            {
                                SelectedCustomerPriceFile = attachments;
                            }
                        }
                    }
                    else
                    {

                       

                        if (extension == ".pdf")
                        {
                            FileInBytes = System.IO.File.ReadAllBytes(item);
                            BPLAttachmentList = new List<BPLDocument>();

                            FileInfo file = new FileInfo(item);
                            ArticleSavedFileName = file.Name;
                            if (string.IsNullOrEmpty(FileName))
                            {
                                FileName = file.Name;
                                int index = FileName.LastIndexOf('.');
                                FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                                FileName = FileTobeSavedByName;
                            }
                            List<BPLDocument> newAttachmentList = new List<BPLDocument>();
                            BPLDocument attachment = new BPLDocument();
                            attachment.SavedFileName = file.Name;
                            attachment.OriginalFileName = file.Name;
                            attachment.PLMArticleFileInBytes = FileInBytes;

                            AttachmentObjectList = new List<object>();
                            AttachmentObjectList.Add(attachment);

                            newAttachmentList.Add(attachment);
                            BPLAttachmentList = newAttachmentList;

                            if (BPLAttachmentList.Count > 0)
                            {
                                SelectedBasicPriceFile = attachment;
                            }
                        }
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


        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                if (IsCPLTypeAttachment == Visibility.Visible)
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.DefaultExt = "Pdf Files|*.pdf";
                    dlg.Filter = "Pdf Files|*.pdf";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                        CPLAttachmentList = new List<CPLDocument>();

                        FileInfo file = new FileInfo(dlg.FileName);
                        ArticleSavedFileName = file.Name;
                        if (string.IsNullOrEmpty(FileName))
                        {
                            FileName = file.Name;
                            int index = FileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        List<CPLDocument> newAttachmentList = new List<CPLDocument>();
                        CPLDocument attachments = new CPLDocument();
                        attachments.SavedFileName = file.Name;
                        attachments.OriginalFileName = file.Name;
                        attachments.PLMArticleFileInBytes = FileInBytes;

                        CPLAttachmentObjectList = new List<object>();
                        CPLAttachmentObjectList.Add(attachments);

                        newAttachmentList.Add(attachments);
                        CPLAttachmentList = newAttachmentList;

                        if (CPLAttachmentList.Count > 0)
                        {
                            SelectedCustomerPriceFile = attachments;
                        }
                    }
                }
                else
                {

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.DefaultExt = "Pdf Files|*.pdf";
                    dlg.Filter = "Pdf Files|*.pdf";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                        BPLAttachmentList = new List<BPLDocument>();

                        FileInfo file = new FileInfo(dlg.FileName);
                        ArticleSavedFileName = file.Name;
                        if (string.IsNullOrEmpty(FileName))
                        {
                            FileName = file.Name;
                            int index = FileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        List<BPLDocument> newAttachmentList = new List<BPLDocument>();
                        BPLDocument attachment = new BPLDocument();
                        attachment.SavedFileName = file.Name;
                        attachment.OriginalFileName = file.Name;
                        attachment.PLMArticleFileInBytes = FileInBytes;

                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);

                        newAttachmentList.Add(attachment);
                        BPLAttachmentList = newAttachmentList;

                        if (BPLAttachmentList.Count > 0)
                        {
                            SelectedBasicPriceFile = attachment;
                        }
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

        public void PreviewDragOverEventAction(DragEventHandler e)
        {
            //e.Effects = DragDropEffects.All;
            //e.Handled = true;
        }

        public void PreviewDropEventAction(DragEventHandler e)
        {
            GeosApplication.Instance.Logger.Log("Method DragDropAction() ...", category: Category.Info, priority: Priority.Low);
          //  string[] fileloadup = (string[])e.Data.GetData(DataFormats.FileDrop);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                if (IsCPLTypeAttachment == Visibility.Visible)
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.DefaultExt = "Pdf Files|*.pdf";
                    dlg.Filter = "Pdf Files|*.pdf";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                        CPLAttachmentList = new List<CPLDocument>();

                        FileInfo file = new FileInfo(dlg.FileName);
                        ArticleSavedFileName = file.Name;
                        if (string.IsNullOrEmpty(FileName))
                        {
                            FileName = file.Name;
                            int index = FileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        List<CPLDocument> newAttachmentList = new List<CPLDocument>();
                        CPLDocument attachments = new CPLDocument();
                        attachments.SavedFileName = file.Name;
                        attachments.OriginalFileName = file.Name;
                        attachments.PLMArticleFileInBytes = FileInBytes;

                        CPLAttachmentObjectList = new List<object>();
                        CPLAttachmentObjectList.Add(attachments);

                        newAttachmentList.Add(attachments);
                        CPLAttachmentList = newAttachmentList;

                        if (CPLAttachmentList.Count > 0)
                        {
                            SelectedCustomerPriceFile = attachments;
                        }
                    }
                }
                else
                {

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.DefaultExt = "Pdf Files|*.pdf";
                    dlg.Filter = "Pdf Files|*.pdf";

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                        BPLAttachmentList = new List<BPLDocument>();

                        FileInfo file = new FileInfo(dlg.FileName);
                        ArticleSavedFileName = file.Name;
                        if (string.IsNullOrEmpty(FileName))
                        {
                            FileName = file.Name;
                            int index = FileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        List<BPLDocument> newAttachmentList = new List<BPLDocument>();
                        BPLDocument attachment = new BPLDocument();
                        attachment.SavedFileName = file.Name;
                        attachment.OriginalFileName = file.Name;
                        attachment.PLMArticleFileInBytes = FileInBytes;

                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);

                        newAttachmentList.Add(attachment);
                        BPLAttachmentList = newAttachmentList;

                        if (BPLAttachmentList.Count > 0)
                        {
                            SelectedBasicPriceFile = attachment;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DragDropAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DragDropAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

        public void EditInit(BPLDocument bplAttachedDoc)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                //IsCPLTypeAttachment = Visibility.Hidden;
                IsBPLTypeAttachment = Visibility.Visible;


                IdCPTypeAttachedDoc = (uint)bplAttachedDoc.IdBasepricedoc;
                FileName = bplAttachedDoc.OriginalFileName;

                Description = bplAttachedDoc.Description;
                ArticleSavedFileName = bplAttachedDoc.SavedFileName;
                FileInBytes = bplAttachedDoc.PLMArticleFileInBytes;
                FileName = bplAttachedDoc.OriginalFileName;
               
                BPLAttachmentList = new List<BPLDocument>();
                BPLAttachmentList.Add(bplAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)bplAttachedDoc);
                SelectedBasicPriceFile = bplAttachedDoc;

                BPLTypeList = new List<LookupValue>(PCMService.GetLookupValues(106));

                BPLSelectedType = BPLTypeList.Where(x => x.IdLookupValue == bplAttachedDoc.IdAttachmentType).FirstOrDefault();
                IsShareWithCustomer = bplAttachedDoc.IsShareWithCustomer;

                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        public void EditCPLInit(CPLDocument cplAttachedDoc)
        {
            try
            {
                IsCPLTypeAttachment = Visibility.Visible;
                //IsBPLTypeAttachment = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdCPTypeAttachedDoc = (uint)cplAttachedDoc.IdCustomerPriceListDoc;
                FileName = cplAttachedDoc.OriginalFileName;

                Description = cplAttachedDoc.Description;
                ArticleSavedFileName = cplAttachedDoc.SavedFileName;
                FileInBytes = cplAttachedDoc.PLMArticleFileInBytes;
                FileName = cplAttachedDoc.OriginalFileName;

                CPLAttachmentList = new List<CPLDocument>();
                CPLAttachmentList.Add(cplAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)cplAttachedDoc);
                SelectedCustomerPriceFile = cplAttachedDoc;

                CPLTypeList = new List<LookupValue>(PCMService.GetLookupValues(106));

                CPLSelectedType = CPLTypeList.Where(x => x.IdLookupValue == cplAttachedDoc.IdAttachmentType).FirstOrDefault();
                IsShareWithCustomer = cplAttachedDoc.IsShareWithCustomer;

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

                string error =  me[BindableBase.GetPropertyName(() => SelectedBasicPriceFile)]+
                                me[BindableBase.GetPropertyName(() => BPLSelectedType)];
                string errors =
                    me[BindableBase.GetPropertyName(() => SelectedCustomerPriceFile)]+
                    me[BindableBase.GetPropertyName(() => CPLSelectedType)];

                if (!string.IsNullOrEmpty(error) && IsBPLTypeAttachment==Visibility.Visible)
                    return "Please check inputted data.";
                if (!string.IsNullOrEmpty(errors) && IsCPLTypeAttachment==Visibility.Visible)
                    return "Please check inputted data.";
                

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string BPLFile = BindableBase.GetPropertyName(() => SelectedBasicPriceFile);
                string BPLDocumentType = BindableBase.GetPropertyName(() => BPLSelectedType);

                string CPLFile = BindableBase.GetPropertyName(() => SelectedCustomerPriceFile);
                string CPLDocumentType = BindableBase.GetPropertyName(() => CPLSelectedType);

                if (columnName == BPLFile && IsBPLTypeAttachment== Visibility.Visible)
                {
                    return AddEditBasePriceValidation.GetErrorMessage(BPLFile, SelectedBasicPriceFile);
                }
                if (columnName == BPLDocumentType && IsBPLTypeAttachment == Visibility.Visible)
                {
                    return AddEditBasePriceValidation.GetErrorMessage(BPLDocumentType, BPLSelectedType);
                }
                if (columnName == CPLFile && IsCPLTypeAttachment == Visibility.Visible)
                {
                    return AddEditCustomerPriceValidation.GetErrorMessage(CPLFile, SelectedCustomerPriceFile);
                }
                if (columnName == CPLDocumentType && IsCPLTypeAttachment == Visibility.Visible)
                {
                    return AddEditCustomerPriceValidation.GetErrorMessage(CPLDocumentType, CPLSelectedType);
                }
                return null;
            }
        }
        #endregion
    }
}
