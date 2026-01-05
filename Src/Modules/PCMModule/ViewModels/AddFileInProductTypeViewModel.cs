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
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Data.Common.Epc;
using System.Windows;
using System.Linq;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddFileInProductTypeViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {

        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ICrmService CRMService = new CrmServiceController("localhost:6699");
       // IPCMService PCMService = new PCMServiceController("localhost:6699");
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

        private ProductTypeAttachedDoc selectedProductTypeFile;
        private byte[] fileInBytes;
        private ObservableCollection<ProductTypeAttachedDoc> productTypeAttachmentList;
        private List<Object> attachmentObjectList;
        private string productTypeSavedFileName;
        private string fileName;
        private string description;
        private DateTime updatedDate;
        private uint idCPTypeAttachedDoc;

        string FileTobeSavedByName = "";
        private string error = string.Empty;
        private List<Object> moduleAttachmentObjectList;//[sudhir.jangra][GEOS2-4072][Add ModuleType in attachment][08/12/2022]
        private LookupValue moduleSelectedType;//[sudhir.jangra][GEOS2-4072][Add Module Type In Attachment][08/12/2022]
        private List<LookupValue> moduleTypeList;//[sudhir.jangra][GEOS2-4072][Add Module Type In Attachment][08/12/2022]
                                                 //  private GeosModuleDocumentation selectedModuleList;//[sudhir.jangra][GEOS2-4072][Add Module Type In Attachment][08/12/2022]
        Visibility isModuleTypeAttachment = Visibility.Collapsed;//[sudhir.jangra][GEOS2-4072][Add Module Type In Attachment][08/12/2022]
        string[] fileUploadDargDropFromFileExplorer;//[Sudhir.Jangra][GEOS2-4072][Added Drag And Drop]
        #endregion


        #region Properties
        //[Sudhir.Jangra][GEOS2-4072][14/12/2022]
        public string[] ProductFileUploadDargDropFromFileExplorer
        {
            get { return fileUploadDargDropFromFileExplorer; }
            set
            {
                fileUploadDargDropFromFileExplorer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileUploadDargDropFromFileExplorer"));
                BrowseFileActionFileUploaded(ProductFileUploadDargDropFromFileExplorer);
            }
        }
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

        public ProductTypeAttachedDoc SelectedProductTypeFile
        {
            get
            {
                return selectedProductTypeFile;
            }

            set
            {
                selectedProductTypeFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypeFile"));
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

        public ObservableCollection<ProductTypeAttachedDoc> ProductTypeAttachmentList
        {
            get
            {
                return productTypeAttachmentList;
            }

            set
            {
                productTypeAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeAttachmentList"));

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

        public string ProductTypeSavedFileName
        {
            get
            {
                return productTypeSavedFileName;
            }
            set
            {
                productTypeSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeSavedFileName"));
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
       
        public List<LookupValue> ModuleTypeList
        {
            get { return moduleTypeList; }
            set
            {
                moduleTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleTypeList"));
            }
        }
        public Visibility IsModuleTypeAttachment
        {
            get { return isModuleTypeAttachment; }
            set
            {
                isModuleTypeAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsModuleTypeAttachment"));
            }
        }
        public LookupValue ModuleSelectedType
        {
            get { return moduleSelectedType; }
            set { moduleSelectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleSelectedType"));
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
        public AddFileInProductTypeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddFileInProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptFileActionCommand = new DelegateCommand<object>(ProductTypeFileAction);
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

        //[Sudhir.Jangra][GEOS2-4072][Added Drag And Drop]
        public void BrowseFileActionFileUploaded(string[] fileUploaded)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...",category:Category.Info,priority:Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                foreach (string item in fileUploaded)
                {
                    string extension = System.IO.Path.GetExtension(item);
                    if (extension==".pdf")
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(item);
                        ProductTypeAttachmentList = new ObservableCollection<ProductTypeAttachedDoc>();
                        FileInfo file = new FileInfo(item);
                        ProductTypeSavedFileName = file.Name;
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName: FileName.Substring(0,index);
                        FileName = FileTobeSavedByName;
                        ObservableCollection<ProductTypeAttachedDoc> newAttachmentList = new ObservableCollection<ProductTypeAttachedDoc>();
                        ProductTypeAttachedDoc attachment = new ProductTypeAttachedDoc();
                        attachment.SavedFileName = file.Name;
                        attachment.ProductTypeAttachedDocInBytes = FileInBytes;
                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);
                        newAttachmentList.Add(attachment);
                        ProductTypeAttachmentList = newAttachmentList;
                        if (ProductTypeAttachmentList.Count>0)
                        {
                            SelectedProductTypeFile = ProductTypeAttachmentList[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BrowseFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ProductTypeFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()...", category: Category.Info, priority: Priority.Low);
               
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProductTypeFile"));
                PropertyChanged(this, new PropertyChangedEventArgs("ModuleSelectedType"));//[Sudhir.Jangra][GEOS-4072][13/12/2022]

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
                    SelectedProductTypeFile = new ProductTypeAttachedDoc();
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = ProductTypeSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ProductTypeSavedFileName : ProductTypeSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedProductTypeFile.OriginalFileName = FileName;
                    SelectedProductTypeFile.SavedFileName = ProductTypeSavedFileName;
                    SelectedProductTypeFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProductTypeFile.IdDocType = 1;
                    SelectedProductTypeFile.ProductTypeAttachedDocInBytes = FileInBytes;
                    SelectedProductTypeFile.Description = Description;
                    SelectedProductTypeFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedProductTypeFile.AttachmentType = new LookupValue();
                    SelectedProductTypeFile.AttachmentType = ModuleSelectedType;//[Sudhir.Jangra][Geos2-4072][12/12/2022]
                    IsSave = true;
                }
                else
                {
                    SelectedProductTypeFile = new ProductTypeAttachedDoc();
                    SelectedProductTypeFile.SavedFileName = ProductTypeSavedFileName;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = ProductTypeSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ProductTypeSavedFileName : ProductTypeSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }

                    SelectedProductTypeFile.OriginalFileName = FileName;
                    SelectedProductTypeFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProductTypeFile.IdDocType = 1;
                    SelectedProductTypeFile.ProductTypeAttachedDocInBytes = FileInBytes;
                    SelectedProductTypeFile.Description = Description;
                    SelectedProductTypeFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedProductTypeFile.AttachmentType = new LookupValue();
                    SelectedProductTypeFile.AttachmentType = ModuleSelectedType;//[Sudhir.Jangra][Geos2-4072][12/12/2022]
                    
                    IsSave = true;
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ProductTypeFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    ProductTypeAttachmentList = new ObservableCollection<ProductTypeAttachedDoc>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ProductTypeSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<ProductTypeAttachedDoc> newAttachmentList = new ObservableCollection<ProductTypeAttachedDoc>();
                    ProductTypeAttachedDoc attachment = new ProductTypeAttachedDoc();
                    attachment.SavedFileName = file.Name;
                    attachment.ProductTypeAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    ProductTypeAttachmentList = newAttachmentList;

                    if (ProductTypeAttachmentList.Count > 0)
                    {
                        SelectedProductTypeFile = ProductTypeAttachmentList[0];
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

        public void EditInit(ProductTypeAttachedDoc productTypeAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                IsModuleTypeAttachment = Visibility.Visible;
                IdCPTypeAttachedDoc = productTypeAttachedDoc.IdCPTypeAttachedDoc;
                FileName = productTypeAttachedDoc.OriginalFileName;

                Description = productTypeAttachedDoc.Description;
                ProductTypeSavedFileName = productTypeAttachedDoc.SavedFileName;
                FileInBytes = productTypeAttachedDoc.ProductTypeAttachedDocInBytes;

                ProductTypeAttachmentList = new ObservableCollection<ProductTypeAttachedDoc>();
                ProductTypeAttachmentList.Add(productTypeAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)productTypeAttachedDoc);
                SelectedProductTypeFile = productTypeAttachedDoc;
                //[Sudhir.Jangra][Geos2-4072][12/12/2022]
                ModuleTypeList = new List<LookupValue>(CRMService.GetLookupValues(106));
                ModuleSelectedType = ModuleTypeList.Where(x => x.IdLookupValue == productTypeAttachedDoc.AttachmentType.IdLookupValue).FirstOrDefault();
                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        public void Init()
        {
            IsModuleTypeAttachment = Visibility.Visible;
            ModuleTypeList = new List<LookupValue>(CRMService.GetLookupValues(106));
            ModuleTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
            ModuleSelectedType=ModuleTypeList.FirstOrDefault();
        }
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
                    me[BindableBase.GetPropertyName(() => SelectedProductTypeFile)] +
                    me[BindableBase.GetPropertyName(() => ModuleSelectedType)];
             

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

                string ProductFile = BindableBase.GetPropertyName(() => SelectedProductTypeFile);
                string SelectedAttachmentValue = BindableBase.GetPropertyName(() => ModuleSelectedType);

                if (columnName == ProductFile)
                {
                    return AddEditModuleValidation.GetErrorMessage(ProductFile, SelectedProductTypeFile);
                }
                if (columnName == SelectedAttachmentValue)
                {
                    return AddEditAttachmentTypeValidation.GetErrorMessage(SelectedAttachmentValue, ModuleSelectedType, null);
                }
                return null;
            }
        }
        #endregion
    }
}
