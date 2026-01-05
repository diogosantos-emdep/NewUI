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
using System.Text.RegularExpressions;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddFileInOptionWayDetectionSparePartViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        //[rdixit][GEOS2-4074][12.12.2022]
        string[] fileUploadDargDropFromFileExplorer;
        LookupValue selectedAttachmentType;
        List<LookupValue> attachmentTypeList;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private byte[] fileInBytes;
        private ObservableCollection<DetectionAttachedDoc> optionWayDetectionSparePartAttachmentList;
        private string optionWayDetectionSparePartSavedFileName;
        private List<Object> attachmentObjectList;
        private DetectionAttachedDoc selectedOptionWayDetectionSparePartFile;
        private string fileName;
        private string description;
        private uint idDetectionAttachedDoc;

        string FileTobeSavedByName = "";
        private DateTime updatedDate;
        #endregion

        #region Properties
        //[rdixit][GEOS2-4074][12.12.2022]
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
        public LookupValue SelectedAttachmentType
        {
            get { return selectedAttachmentType; }
            set
            {

                selectedAttachmentType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachmentType"));
            }
        }
        public List<LookupValue> AttachmentTypeList
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

        public ObservableCollection<DetectionAttachedDoc> OptionWayDetectionSparePartAttachmentList
        {
            get
            {
                return optionWayDetectionSparePartAttachmentList;
            }

            set
            {
                optionWayDetectionSparePartAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartAttachmentList"));

            }
        }

        public string OptionWayDetectionSparePartSavedFileName
        {
            get
            {
                return optionWayDetectionSparePartSavedFileName;
            }
            set
            {
                optionWayDetectionSparePartSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartSavedFileName"));
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

        public DetectionAttachedDoc SelectedOptionWayDetectionSparePartFile
        {
            get
            {
                return selectedOptionWayDetectionSparePartFile;
            }

            set
            {
                selectedOptionWayDetectionSparePartFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartFile"));

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

        public uint IdDetectionAttachedDoc
        {
            get
            {
                return idDetectionAttachedDoc;
            }

            set
            {
                idDetectionAttachedDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionAttachedDoc"));
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

        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand AcceptFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        //public ICommand PreviewDragEnterCommand { get; set; }
        //public ICommand PreviewDragOverCommand { get; set; }
        //public ICommand PreviewDropCommand { get; set; }


        #endregion

        #region Constructor
        public AddFileInOptionWayDetectionSparePartViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddFileInOptionWayDetectionSparePartViewModel ...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                AcceptFileActionCommand = new DelegateCommand<object>(AddFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                GetAttachmentTypes();
                //PreviewDragEnterCommand = new DelegateCommand<DragEventArgs>(PreviewDragEnterCommandAction);
                //PreviewDragOverCommand = new DelegateCommand<DragEventArgs>(PreviewDragOverCommandAction);
                //PreviewDropCommand = new DelegateCommand<DragEventArgs>(PreviewDropCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor AddFileInOptionWayDetectionSparePartViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddFileInOptionWayDetectionSparePartViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods

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

        private void AddFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()...", category: Category.Info, priority: Priority.Low);
				//[rdixit][GEOS2-4074][12.12.2022]
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAttachmentType"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartFile"));
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
                    SelectedOptionWayDetectionSparePartFile = new DetectionAttachedDoc();
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = OptionWayDetectionSparePartSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? OptionWayDetectionSparePartSavedFileName : OptionWayDetectionSparePartSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedOptionWayDetectionSparePartFile.OriginalFileName = FileName;
                    SelectedOptionWayDetectionSparePartFile.SavedFileName = OptionWayDetectionSparePartSavedFileName;
                    SelectedOptionWayDetectionSparePartFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedOptionWayDetectionSparePartFile.IdDocType = 1;
                    SelectedOptionWayDetectionSparePartFile.DetectionAttachedDocInBytes = FileInBytes;
                    SelectedOptionWayDetectionSparePartFile.Description = Description;
                    SelectedOptionWayDetectionSparePartFile.AttachmentType = SelectedAttachmentType;
                    SelectedOptionWayDetectionSparePartFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;
                }
                else
                {
                    SelectedOptionWayDetectionSparePartFile = new DetectionAttachedDoc();
                    if (string.IsNullOrEmpty(FileName))
                    {
                        int index = OptionWayDetectionSparePartSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? OptionWayDetectionSparePartSavedFileName : OptionWayDetectionSparePartSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedOptionWayDetectionSparePartFile.OriginalFileName = FileName;
                    SelectedOptionWayDetectionSparePartFile.SavedFileName = OptionWayDetectionSparePartSavedFileName;
                    SelectedOptionWayDetectionSparePartFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedOptionWayDetectionSparePartFile.IdDocType = 1;
                    SelectedOptionWayDetectionSparePartFile.DetectionAttachedDocInBytes = FileInBytes;
                    SelectedOptionWayDetectionSparePartFile.Description = Description;
                    SelectedOptionWayDetectionSparePartFile.AttachmentType = SelectedAttachmentType;
                    SelectedOptionWayDetectionSparePartFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
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
		//[rdixit][GEOS2-4074][12.12.2022]
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
                        OptionWayDetectionSparePartAttachmentList = new ObservableCollection<DetectionAttachedDoc>();

                        FileInfo file = new FileInfo(item);
                        OptionWayDetectionSparePartSavedFileName = file.Name;
                        //if (string.IsNullOrEmpty(FileName))
                        //{
                            FileName = file.Name;
                            int index = FileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        //}
                        ObservableCollection<DetectionAttachedDoc> newAttachmentList = new ObservableCollection<DetectionAttachedDoc>();
                        DetectionAttachedDoc attachment = new DetectionAttachedDoc();
                        attachment.SavedFileName = file.Name;
                        attachment.DetectionAttachedDocInBytes = FileInBytes;

                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);

                        newAttachmentList.Add(attachment);
                        OptionWayDetectionSparePartAttachmentList = newAttachmentList;

                        if (OptionWayDetectionSparePartAttachmentList.Count > 0)
                        {
                            SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartAttachmentList[0];
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
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    OptionWayDetectionSparePartAttachmentList = new ObservableCollection<DetectionAttachedDoc>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    OptionWayDetectionSparePartSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<DetectionAttachedDoc> newAttachmentList = new ObservableCollection<DetectionAttachedDoc>();
                    DetectionAttachedDoc attachment = new DetectionAttachedDoc();
                    attachment.SavedFileName = file.Name;
                    attachment.DetectionAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    OptionWayDetectionSparePartAttachmentList = newAttachmentList;

                    if (OptionWayDetectionSparePartAttachmentList.Count > 0)
                    {
                        SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartAttachmentList[0];
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
      

        public void EditInit(DetectionAttachedDoc detectionAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);             
                IdDetectionAttachedDoc = detectionAttachedDoc.IdDetectionAttachedDoc;
                FileName = detectionAttachedDoc.OriginalFileName;
                Description = detectionAttachedDoc.Description;
                OptionWayDetectionSparePartSavedFileName = detectionAttachedDoc.SavedFileName;
                FileInBytes = detectionAttachedDoc.DetectionAttachedDocInBytes;
                UpdatedDate = (DateTime)detectionAttachedDoc.UpdatedDate;
                OptionWayDetectionSparePartAttachmentList = new ObservableCollection<DetectionAttachedDoc>();
                OptionWayDetectionSparePartAttachmentList.Add(detectionAttachedDoc);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)detectionAttachedDoc);
                SelectedOptionWayDetectionSparePartFile = detectionAttachedDoc;

                AttachmentTypeList = new List<LookupValue>(CRMService.GetLookupValues(106));
                SelectedAttachmentType = AttachmentTypeList.Where(i => i.IdLookupValue == detectionAttachedDoc.AttachmentType.IdLookupValue).FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
		//[rdixit][GEOS2-4074][12.12.2022]
        private void GetAttachmentTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAttachmentTypes()...", category: Category.Info, priority: Priority.Low);

                if (AttachmentTypeList == null)
                {
                    AttachmentTypeList = new List<LookupValue>(CRMService.GetLookupValues(106));
                }
                AttachmentTypeList.Insert(0, new LookupValue() { Value = "---", IdLookupValue = 0, IdLookupKey = 0 });
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

        //private void PreviewDragEnterCommandAction(DragEventArgs e)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PreviewDragEnterCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        //FileInfo file = new FileInfo();
        //        var dropPossible = e.Data != null && ((DataObject)e.Data).ContainsFileDropList();
        //        if (dropPossible)
        //        {
        //            e.Effects = DragDropEffects.Copy;
        //        }

        //        GeosApplication.Instance.Logger.Log("Method PreviewDragEnterCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method PreviewDragEnterCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void PreviewDragOverCommandAction(DragEventArgs e)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PreviewDragOverCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        //FileInfo file = new FileInfo();
        //        e.Handled = true;
        //        GeosApplication.Instance.Logger.Log("Method PreviewDragOverCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method PreviewDragOverCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //private void PreviewDropCommandAction(DragEventArgs e)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PreviewDropCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        //FileInfo file = new FileInfo();
        //        if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
        //        {
        //            //FileTextEdit.EditValue = null;
        //            //ImageName = "";
        //            foreach (string filePath in ((DataObject)e.Data).GetFileDropList())
        //            {

        //                ObservableCollection<DetectionAttachedDoc> newAttachmentList = new ObservableCollection<DetectionAttachedDoc>();
        //                FileName += filePath.Split('\\').Last() + "; ";
        //                FileInfo file = new FileInfo(filePath);
        //                if(file.Extension == ".pdf")
        //                {
        //                    OptionWayDetectionSparePartSavedFileName = file.Name;
        //                    FileInBytes = System.IO.File.ReadAllBytes(filePath);

        //                    DetectionAttachedDoc attachment = new DetectionAttachedDoc();
        //                    attachment.SavedFileName = file.Name;
        //                    attachment.DetectionAttachedDocInBytes = FileInBytes;

        //                    AttachmentObjectList = new List<object>();
        //                    AttachmentObjectList.Add(attachment);

        //                    newAttachmentList.Add(attachment);
        //                    OptionWayDetectionSparePartAttachmentList = newAttachmentList;

        //                    if (OptionWayDetectionSparePartAttachmentList.Count > 0)
        //                    {
        //                        SelectedOptionWayDetectionSparePartFile = OptionWayDetectionSparePartAttachmentList[0];
        //                    }
        //                }

        //                else
        //                {
        //                    FileName = "";
        //                    IsSave = false;
        //                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DropPDFFileWarningMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //                    return;
        //                }
        //            }
        //        }
        //        GeosApplication.Instance.Logger.Log("Method PreviewDropCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method PreviewDropCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion

        #region Validation [rdixit][GEOS2-4074][12.12.2022]
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
                    me[BindableBase.GetPropertyName(() => SelectedOptionWayDetectionSparePartFile)];

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
                string SelectedOptionWayDetectionSparePartFileValue = BindableBase.GetPropertyName(() => SelectedOptionWayDetectionSparePartFile);

                if (columnName == SelectedAttachmentValue)
                {
                    return AddEditAttachmentTypeValidation.GetErrorMessage(SelectedAttachmentValue, SelectedAttachmentType, null);
                }
                if (columnName == SelectedOptionWayDetectionSparePartFileValue)
                {
                    return AddEditAttachmentTypeValidation.GetErrorMessage(SelectedOptionWayDetectionSparePartFileValue, SelectedOptionWayDetectionSparePartFile, null);
                }
                return null;
            }
        }
        #endregion
    }
}
