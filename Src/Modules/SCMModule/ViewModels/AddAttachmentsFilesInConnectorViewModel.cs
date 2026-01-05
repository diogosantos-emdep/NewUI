using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    class AddAttachmentsFilesInConnectorViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        //[pramod.misal][GEOS2-4074][15.05.2024]
        string[] fileUploadDargDropFromFileExplorer; 
        ConnectorAttachements selectedAttachmentType;
        List<ConnectorAttachements> attachmentTypeList;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private byte[] fileInBytes;
        private ObservableCollection<ConnectorAttachements> connectorAttachmentList;//optionWayDetectionSparePartAttachmentList
        private string connectorAttachmentSavedFileName;
        private List<Object> attachmentObjectList;
        private ConnectorAttachements selectedConnectorFile;//SelectedOptionWayDetectionSparePartFile
        private string fileName;
        private string description;
        private Int32 idconnectordoc;

        string FileTobeSavedByName = "";
        private DateTime updatedDate;
        List<ConnectorAttachements> companyTypeList;
        private ObservableCollection<Company> listCompany;
        private Company selectedCompany;

        public List<ConnectorAttachements> CompanyTypeList
        {
            get { return companyTypeList; }
            set
            {
                companyTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyTypeList"));
            }
        }
       
        public ObservableCollection<Company> ListCompany
        {
            get { return listCompany; }
            set
            {
                listCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCompany"));
            }
        }

        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                selectedCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompany"));
            }
        }
        #endregion

        #region Properties
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
        public ConnectorAttachements SelectedAttachmentType
        {
            get { return selectedAttachmentType; }
            set
            {

                selectedAttachmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachmentType"));
            }
        }
        public List<ConnectorAttachements> AttachmentTypeList
        {
            get { return attachmentTypeList; }
            set
            {
                attachmentTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentTypeList"));
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


        public ObservableCollection<ConnectorAttachements> ConnectorAttachmentList//)OptionWayDetectionSparePartAttachmentList
        {
            get
            { //optionWayDetectionSparePartAttachmentList
                return connectorAttachmentList;
            }

            set
            {
                connectorAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorAttachmentList"));

            }
        }

        public string ConnectorAttachmentSavedFileName //OptionWayDetectionSparePartSavedFileName
        {
            get
            {  //optionWayDetectionSparePartSavedFileName
                return connectorAttachmentSavedFileName;
            }
            set
            {
                connectorAttachmentSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorAttachmentSavedFileName"));
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

        public ConnectorAttachements SelectedConnectorFile
        {
            get
            {
                return selectedConnectorFile;
            }

            set
            {
                selectedConnectorFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorFile"));

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

        public Int32 Idconnectordoc
        {
            get
            {
                return idconnectordoc;
            }

            set
            {
                idconnectordoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idconnectordoc"));
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

        //[pramod.misal][GEOS2-5387][08-04-2024]

        ObservableCollection<ConnectorAttachements> connectorAttachementFilesList;
        public ObservableCollection<ConnectorAttachements> ConnectorAttachementFilesList
        {
            get { return connectorAttachementFilesList; }
            set
            {
                connectorAttachementFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("connectorAttachementFilesList"));
            }
        }

        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]

        #endregion

        #region Constructor
        public AddAttachmentsFilesInConnectorViewModel()
        {
            try
            {
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                AcceptFileActionCommand = new DelegateCommand<object>(AddFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GetConnectorAttachmentTypes();
                GetAllCompany();

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddAttachmentsFilesInConnectorViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region  Methods

        //GetAttachmentTypes()
        private void GetConnectorAttachmentTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetConnectorAttachmentTypes()...", category: Category.Info, priority: Priority.Low);

                if (AttachmentTypeList == null)
                {
                    //SCMService = new SCMServiceController("localhost:6699");
                    AttachmentTypeList = new List<ConnectorAttachements>(SCMService.GetAllConnectorAttachmentTypes().ToList());
                    AttachmentTypeList.FirstOrDefault();
                }
               
                SelectedAttachmentType = AttachmentTypeList.FirstOrDefault();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetAllCompany()
        {  
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetConnectorAttachmentTypes()...", category: Category.Info, priority: Priority.Low);

                if (ListCompany == null)
                {
                    //SCMService = new SCMServiceController("localhost:6699");
                    ListCompany = new ObservableCollection<Company>(SCMService.GetAllCompany_V2490());
                    ListCompany.Insert(0, new Company() { Id = 0, Name = "---" });

                    SelectedCompany = ListCompany.FirstOrDefault();
                    GeosApplication.Instance.Logger.Log(string.Format("Method FillCompany()....executed successfully"), category: Category.Info, priority: Priority.Low);
                    
                }

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAttachmentTypes()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAttachmentType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedConnectorFile"));
                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                if (Description.Contains("\r\n"))
                {
                    Description = Description.TrimEnd(trimChars);
                    Description = Description.TrimStart(trimChars);
                }

                if (IsNew)
                {
                    SelectedConnectorFile = new ConnectorAttachements();
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = ConnectorAttachmentSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ConnectorAttachmentSavedFileName : ConnectorAttachmentSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedConnectorFile.OriginalFileName = FileName;
                    SelectedConnectorFile.SavedFileName = ConnectorAttachmentSavedFileName;
                    SelectedConnectorFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedConnectorFile.IdDocType = 1;
                    SelectedConnectorFile.ConnectorAttachementsDocInBytes = FileInBytes;
                    SelectedConnectorFile.Description = Description;
                    SelectedConnectorFile.AttachmentType = SelectedAttachmentType;
                    SelectedConnectorFile.ModifiedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedConnectorFile.CustomerName = SelectedCompany.Name;
                    SelectedConnectorFile.DocumentType = SelectedAttachmentType.DocumentType;
                    SelectedConnectorFile.IdCustomer =Convert.ToInt32(SelectedCompany.Id);
                    SelectedConnectorFile.IdDocType = SelectedAttachmentType.IdDocType;
                    IsSave = true;
                }
                else
                {
                    SelectedConnectorFile = new ConnectorAttachements();
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = ConnectorAttachmentSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ConnectorAttachmentSavedFileName : ConnectorAttachmentSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedConnectorFile.OriginalFileName = FileName;
                    SelectedConnectorFile.SavedFileName = ConnectorAttachmentSavedFileName;
                    SelectedConnectorFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedConnectorFile.IdDocType = 1;
                    SelectedConnectorFile.ConnectorAttachementsDocInBytes = FileInBytes;
                    SelectedConnectorFile.Description = Description;
                    SelectedConnectorFile.AttachmentType = SelectedAttachmentType;
                    //SelectedConnectorFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedConnectorFile.ModifiedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    SelectedConnectorFile.CustomerName = SelectedCompany.Name;
                    SelectedConnectorFile.DocumentType = SelectedAttachmentType.DocumentType;
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

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                //dlg.DefaultExt = "Pdf Files|*.pdf";
                //dlg.Filter = "Pdf Files|*.pdf";
                dlg.DefaultExt = "*.*"; // Default extension if none is selected
                //dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.xlsx|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg|Word Documents|*.docx|Excel Spreadsheets|*.xlsx";
                dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.png|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg;*.png|Word Documents|*.docx";


                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    connectorAttachmentList = new ObservableCollection<ConnectorAttachements>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ConnectorAttachmentSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<ConnectorAttachements> newAttachmentList = new ObservableCollection<ConnectorAttachements>();
                    ConnectorAttachements attachment = new ConnectorAttachements();
                    attachment.SavedFileName = file.Name;
                    attachment.ConnectorAttachementsDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    ConnectorAttachmentList = newAttachmentList;

                    if (ConnectorAttachmentList.Count > 0)
                    {
                        SelectedConnectorFile = connectorAttachmentList[0];
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
                    if (extension == ".pdf")
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(item);
                        ConnectorAttachmentList = new ObservableCollection<ConnectorAttachements>();
                        FileInfo file = new FileInfo(item);
                        ConnectorAttachmentSavedFileName = file.Name;
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                        ObservableCollection<ConnectorAttachements> newAttachmentList = new ObservableCollection<ConnectorAttachements>();
                        ConnectorAttachements attachment = new ConnectorAttachements();
                        attachment.SavedFileName = file.Name;
                        attachment.ConnectorAttachementsDocInBytes = FileInBytes;

                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);

                        newAttachmentList.Add(attachment);
                        ConnectorAttachmentList = newAttachmentList;

                        if (ConnectorAttachmentList.Count > 0)
                        {
                            SelectedConnectorFile = ConnectorAttachmentList[0];
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

        //[pramod.misal][GEOS2-5477][16.05.2024]
        public void EditInit(ConnectorAttachements connectorAttachements)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                Idconnectordoc = connectorAttachements.Idconnectordoc;
                FileName= connectorAttachements.OriginalFileName;
                Description = connectorAttachements.Description;
                connectorAttachmentSavedFileName = connectorAttachements.SavedFileName;
                FileInBytes = connectorAttachements.ConnectorAttachementsDocInBytes;
                if (connectorAttachements.CreatedDate.HasValue)                
                    UpdatedDate = (DateTime)connectorAttachements.CreatedDate;                
                else                
                    UpdatedDate = DateTime.MinValue;
                
                ConnectorAttachmentList = new ObservableCollection<ConnectorAttachements>();
                ConnectorAttachmentList.Add(connectorAttachements);
                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)connectorAttachements);
                SelectedConnectorFile = connectorAttachements;
                AttachmentTypeList = new List<ConnectorAttachements>(SCMService.GetAllConnectorAttachmentTypes().ToList());
                SelectedAttachmentType = AttachmentTypeList.Where(i => i.IdDocType == connectorAttachements.IdDocType).FirstOrDefault();
                SelectedCompany = ListCompany.Where(item =>item.Name== connectorAttachements.CustomerName).FirstOrDefault();
                if (SelectedCompany == null)
                {
                    ListCompany.Insert(0, new Company() { Id = 0, Name = "---" });
                    SelectedCompany = ListCompany.FirstOrDefault();
                }
                Idconnectordoc = connectorAttachements.Idconnectordoc;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(ObservableCollection<ConnectorAttachements> Attachemetlist)
        {
            ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>();
            if (Attachemetlist != null)
            {
                foreach (var attachment in Attachemetlist)
                {
                    ConnectorAttachementFilesList.Add(attachment);
                }
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation [pramod.misal][15.05.2024]
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
                    me[BindableBase.GetPropertyName(() => SelectedAttachmentType)] +
                    me[BindableBase.GetPropertyName(() => SelectedConnectorFile)];

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

                string SelectedAttachmentValue = BindableBase.GetPropertyName(() => SelectedAttachmentType);
                string SelectedConnectorFileValue = BindableBase.GetPropertyName(() => SelectedConnectorFile);

                if (columnName == SelectedAttachmentValue)
                {
                    return AddEditConnectorAttachmentTypeValidation.GetErrorMessage(SelectedAttachmentValue, SelectedAttachmentType, null);
                }
                if (columnName == SelectedConnectorFileValue)
                {
                    // Apply validation for already existing record in SelectedConnectorFile
                    if (ConnectorAttachementFilesList != null)
                    {
                        try
                        {
                            var existingFile = ConnectorAttachementFilesList.FirstOrDefault(x => x.SavedFileName != null && x.SavedFileName.ToString().Trim() == SelectedConnectorFile?.SavedFileName?.ToString().Trim());
                            if (existingFile != null)
                            {
                                return "The selected file already exists.";
                            }

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        
                        
                    }
                    else
                    {
                        return AddEditConnectorAttachmentTypeValidation.GetErrorMessage(SelectedConnectorFileValue, SelectedConnectorFile, null);
                    }



                }
                return null;
            }
        }
        #endregion
    }
}
