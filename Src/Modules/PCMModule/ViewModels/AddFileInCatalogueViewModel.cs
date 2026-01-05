using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.WindowsUI;
using WindowsUIDemo;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.IO;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddFileInCatalogueViewModel : ViewModelBase, INotifyPropertyChanged
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

        #region Declarations

        private string windowHeader;
        private bool isNew;
        private string header;
        private string fileName;
        private string attachment;
        private string description;
        private byte[] fileInBytes;
        private string originalFileName;
        private CatalogueItemAttachedDoc selectedCatalogueFile;
        private ObservableCollection<CatalogueItemAttachedDoc> catalogueItemAttachmentList;
        private string catalogueItemFileName;
        private List<Object> attachmentObjectList;
        private uint idCatalogueItem;
        private int fileId;
        private bool isSave;
        private ProductTypeAttachedDoc selectedProductTypeFile;
        private ObservableCollection<ProductTypeAttachedDoc> productTypeAttachmentList;
        private uint idCatalogueItemAttachedDoc;


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

        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
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

        public string Attachment
        {
            get
            {
                return attachment;

            }

            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
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

        public int FileId
        {
            get
            {
                return fileId;
            }

            set
            {
                fileId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileId"));

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

        public string OriginalFileName
        {
            get
            {
                return originalFileName;
            }

            set
            {
                originalFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalFileName"));

            }
        }

        public CatalogueItemAttachedDoc SelectedCatalogueFile
        {
            get
            {
                return selectedCatalogueFile;
            }

            set
            {
                selectedCatalogueFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCatalogueFile"));

            }
        }

        public ObservableCollection<CatalogueItemAttachedDoc> CatalogueItemAttachmentList
        {
            get { return catalogueItemAttachmentList; }
            set
            {
                catalogueItemAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueItemAttachmentList"));
            }
        }

        public string CatalogueItemFileName
        {
            get { return catalogueItemFileName; }
            set
            {
                catalogueItemFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CatalogueItemFileName"));
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

        public uint IdCatalogueItem
        {
            get { return idCatalogueItem; }
            set
            {
                idCatalogueItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCatalogueItem"));
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

        public uint IdCatalogueItemAttachedDoc
        {
            get
            {
                return idCatalogueItemAttachedDoc;
            }

            set
            {
                idCatalogueItemAttachedDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCatalogueItemAttachedDoc"));
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
        public AddFileInCatalogueViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddFileInCatalogueViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptFileActionCommand = new DelegateCommand<object>(CatalogueFileAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddFileInCatalogueViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex )
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddFileInCatalogueViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Methods

        private void CatalogueFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CatalogueFileAction()...", category: Category.Info, priority: Priority.Low);

                if (IsNew)
                {
                    SelectedCatalogueFile = new CatalogueItemAttachedDoc();
                    SelectedCatalogueFile.OriginalFileName = CatalogueItemFileName;
                    SelectedCatalogueFile.SavedFileName = FileName;
                    SelectedCatalogueFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedCatalogueFile.IdDocType = 1;
                    SelectedCatalogueFile.CatalogueItemAttachedDocInBytes = FileInBytes;
                    SelectedCatalogueFile.Description = Description;
                }
                else
                {
                    SelectedCatalogueFile = new CatalogueItemAttachedDoc();
                    SelectedCatalogueFile.OriginalFileName = CatalogueItemFileName;
                    SelectedCatalogueFile.SavedFileName = FileName;
                    SelectedCatalogueFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedCatalogueFile.IdDocType = 1;
                    SelectedCatalogueFile.CatalogueItemAttachedDocInBytes = FileInBytes;
                    SelectedCatalogueFile.Description = Description;
                    SelectedCatalogueFile.IdCatalogueItemAttachedDoc = IdCatalogueItemAttachedDoc;
                }
                IsSave = true;

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CatalogueFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CatalogueFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    CatalogueItemAttachmentList = new ObservableCollection<CatalogueItemAttachedDoc>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    CatalogueItemFileName = file.Name;
                    FileName = file.Name;

                    ObservableCollection<CatalogueItemAttachedDoc> newAttachmentList = new ObservableCollection<CatalogueItemAttachedDoc>();

                    CatalogueItemAttachedDoc attachment = new CatalogueItemAttachedDoc();
                    attachment.OriginalFileName = file.Name;
                    attachment.CatalogueItemAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    CatalogueItemAttachmentList = newAttachmentList;

                    if (catalogueItemAttachmentList.Count > 0)
                    {
                        SelectedCatalogueFile = CatalogueItemAttachmentList[0];

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

        public void EditInit(CatalogueItemAttachedDoc catalogueItemAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit() ...", category: Category.Info, priority: Priority.Low);

                IdCatalogueItemAttachedDoc = catalogueItemAttachedDoc.IdCatalogueItemAttachedDoc;
                FileName = catalogueItemAttachedDoc.SavedFileName;
                Description = catalogueItemAttachedDoc.Description;
                CatalogueItemFileName = catalogueItemAttachedDoc.OriginalFileName;
                FileInBytes = catalogueItemAttachedDoc.CatalogueItemAttachedDocInBytes;

                CatalogueItemAttachmentList = new ObservableCollection<CatalogueItemAttachedDoc>();
                CatalogueItemAttachmentList.Add(catalogueItemAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)catalogueItemAttachedDoc);
                SelectedCatalogueFile = catalogueItemAttachedDoc;

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
