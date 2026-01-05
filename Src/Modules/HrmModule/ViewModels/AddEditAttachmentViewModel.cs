using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common.Hrm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Logging;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm.Views;
using System.IO;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    class AddEditAttachmentViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        private ProfessionalTrainingAttachments selectedProfTrainingFile;
        private byte[] fileInBytes;
        private ObservableCollection<ProfessionalTrainingAttachments> profTrainingAttachmentList;
        private List<Object> attachmentObjectList;
        private string profTrainingSavedFileName;
        private string fileName;
        private string description;
        private DateTime updatedDate;
        UInt32 idProfessionalTrainingAttachment;
        UInt64 idProfessionalTraining;
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

        public ProfessionalTrainingAttachments SelectedProfTrainingFile
        {
            get
            {
                return selectedProfTrainingFile;
            }

            set
            {
                selectedProfTrainingFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProfTrainingFile"));
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

        public ObservableCollection<ProfessionalTrainingAttachments> ProfTrainingAttachmentList
        {
            get
            {
                return profTrainingAttachmentList;
            }

            set
            {
                profTrainingAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfTrainingAttachmentList"));

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

        public string ProfTrainingSavedFileName
        {
            get
            {
                return profTrainingSavedFileName;
            }
            set
            {
                profTrainingSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProfTrainingSavedFileName"));
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

        public UInt32 IdProfessionalTrainingAttachment
        {
            get
            {
                return idProfessionalTrainingAttachment;
            }

            set
            {
                idProfessionalTrainingAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTrainingAttachment"));

            }
        }
        public UInt64 IdProfessionalTraining
        {
            get
            {
                return idProfessionalTraining;
            }

            set
            {
                idProfessionalTraining = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdProfessionalTraining"));

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
        public AddEditAttachmentViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditAttachmentViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptFileActionCommand = new DelegateCommand<object>(ProfessionalTrainingFileAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddEditAttachmentViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditAttachmentViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        public void EditInit(ulong idProfessionalTraining,ProfessionalTrainingAttachments professionalTrainingAttachments)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdProfessionalTrainingAttachment = professionalTrainingAttachments.IdProfessionalTrainingAttachment;
                FileName = professionalTrainingAttachments.OriginalFileName;

                Description = professionalTrainingAttachments.Description;
                ProfTrainingSavedFileName = professionalTrainingAttachments.SavedFileName;
                FileInBytes = professionalTrainingAttachments.ProfTrainigAttachedDocInBytes;
                IdProfessionalTraining = idProfessionalTraining;
                ProfTrainingAttachmentList = new ObservableCollection<ProfessionalTrainingAttachments>();
                ProfTrainingAttachmentList.Add(professionalTrainingAttachments);

                AttachmentObjectList = new List<object>();
                AttachmentObjectList.Add((object)professionalTrainingAttachments);
                SelectedProfTrainingFile = professionalTrainingAttachments;

                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void ProfessionalTrainingFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProfessionalTrainingFileAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProfTrainingFile"));

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
                    SelectedProfTrainingFile = new ProfessionalTrainingAttachments();
                    if (string.IsNullOrEmpty((FileName != null) ? (FileName.Trim()) : FileName))
                    {
                        int index = ProfTrainingSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ProfTrainingSavedFileName : ProfTrainingSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    SelectedProfTrainingFile.OriginalFileName = FileName;
                    SelectedProfTrainingFile.SavedFileName = ProfTrainingSavedFileName;
                    SelectedProfTrainingFile.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProfTrainingFile.IdProfessionalTraining = IdProfessionalTraining;
                    SelectedProfTrainingFile.ProfTrainigAttachedDocInBytes = FileInBytes;
                    SelectedProfTrainingFile.Description = Description;
                    SelectedProfTrainingFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;
                }
                else
                {
                    SelectedProfTrainingFile = new ProfessionalTrainingAttachments();
                  
                    if (string.IsNullOrEmpty((FileName != null) ? (FileName.Trim()) : FileName))
                    {
                        int index = ProfTrainingSavedFileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ProfTrainingSavedFileName : ProfTrainingSavedFileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                   
                    SelectedProfTrainingFile.OriginalFileName = FileName;
                    SelectedProfTrainingFile.SavedFileName = ProfTrainingSavedFileName;
                    SelectedProfTrainingFile.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProfTrainingFile.IdProfessionalTraining = IdProfessionalTraining;
                    SelectedProfTrainingFile.ProfTrainigAttachedDocInBytes = FileInBytes;
                    SelectedProfTrainingFile.Description = Description;
                    SelectedProfTrainingFile.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ProfessionalTrainingFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ProfessionalTrainingFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    ProfTrainingAttachmentList = new ObservableCollection<ProfessionalTrainingAttachments>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    ProfTrainingSavedFileName = file.Name;
                   
                    if (string.IsNullOrEmpty((FileName!=null)? (FileName.Trim()): FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<ProfessionalTrainingAttachments> newAttachmentList = new ObservableCollection<ProfessionalTrainingAttachments>();
                    ProfessionalTrainingAttachments attachment = new ProfessionalTrainingAttachments();
                    attachment.SavedFileName = file.Name;
                    attachment.ProfTrainigAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    ProfTrainingAttachmentList = newAttachmentList;

                    if (ProfTrainingAttachmentList.Count > 0)
                    {
                        SelectedProfTrainingFile = ProfTrainingAttachmentList[0];
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
                    me[BindableBase.GetPropertyName(() => SelectedProfTrainingFile)];
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

                string trainingFile = BindableBase.GetPropertyName(() => SelectedProfTrainingFile);
            
                if (columnName == trainingFile)
                {
                    return AddEditTrainingValidation.GetErrorMessage(trainingFile, SelectedProfTrainingFile);
                }
               
                return null;
            }
        }
        #endregion
    }
}
