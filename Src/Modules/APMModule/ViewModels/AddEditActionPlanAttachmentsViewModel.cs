using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.APM.ViewModels
{//[Sudhir.Jangra][GEOS2-6019]
    public class AddEditActionPlanAttachmentsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IAPMService APMService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public Events
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
        private string fileName;
        private string uniqueFileName;
        private AttachmentsByActionPlan actionPlanAttachmentFile;
        private string fileNameString;
        private string description;
        private ObservableCollection<AttachmentsByActionPlan> attachmentObjectList;
        private bool isSave;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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

        public AttachmentsByActionPlan ActionPlanAttachmentFile
        {
            get { return actionPlanAttachmentFile; }
            set
            {
                actionPlanAttachmentFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanAttachmentFile"));
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

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public ObservableCollection<AttachmentsByActionPlan> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        #endregion

        #region ICommands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditActionPlanAttachmentsViewModel()
        {
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
            ChooseFileActionCommand = new RelayCommand(new Action<object>(ChooseFileActionCommandAction));
        }
        #endregion

        #region Methods
        public void Init()
        {
            AttachmentObjectList = new ObservableCollection<AttachmentsByActionPlan>();
        }
        public void EditInit(AttachmentsByActionPlan attachment)
        {
            ActionPlanAttachmentFile = attachment;
            if (AttachmentObjectList == null)
            {
                AttachmentObjectList = new ObservableCollection<AttachmentsByActionPlan>();
            }
            AttachmentObjectList.Add(attachment);
            FileNameString = attachment.OriginalFileName;
            Description = attachment.Description;
        }
        public void ChooseFileActionCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ChooseFileActionCommandAction() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");

                    ActionPlanAttachmentFile = new AttachmentsByActionPlan();
                    ActionPlanAttachmentFile.FileByte= System.IO.File.ReadAllBytes(dlg.FileName);
                    ActionPlanAttachmentFile.FileType = file.Extension;
                    ActionPlanAttachmentFile.FilePath = file.FullName;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    // ActionPlanAttachmentFile.FileName = file.Name;

                    ActionPlanAttachmentFile.OriginalFileName = FileNameString;
                    ActionPlanAttachmentFile.SavedFileName = file.Name;
                    //ActionPlanAttachmentFile.TripAttachmentType = SelectedAttachmentType;
                    ActionPlanAttachmentFile.Description = Description;
                    ActionPlanAttachmentFile.CreatedIn = GeosApplication.Instance.ServerDateTime;
                    ActionPlanAttachmentFile.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    ActionPlanAttachmentFile.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;
                    ActionPlanAttachmentFile.FileSizeInInt = file.Length;
                    if (ActionPlanAttachmentFile.FileSizeInInt >= 1024 * 1024)
                    {
                        // If the file size is 1 MB or larger, display in MB
                        ActionPlanAttachmentFile.FileSize = Math.Ceiling((double)ActionPlanAttachmentFile.FileSizeInInt / (1024 * 1024)).ToString("0") + " MB";
                    }
                    else
                    {
                        // If the file size is less than 1 MB, display in KB
                        ActionPlanAttachmentFile.FileSize = Math.Ceiling((double)ActionPlanAttachmentFile.FileSizeInInt / 1024).ToString("0") + " KB";
                    }

                    ActionPlanAttachmentFile.FileExtension = file.Extension;
                    ActionPlanAttachmentFile.FileUploadName = file.Name;
                    ActionPlanAttachmentFile.IsUploaded = true;
                    ActionPlanAttachmentFile.TransactionOperation = ModelBase.TransactionOperations.Add;
                    var theIcon = IconFromFilePath(FileName);
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    if (theIcon != null)
                    {
                        // Save it to disk, or do whatever you want with it.
                        if (!Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }

                        if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                        {
                            using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                theIcon.Save(stream);
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                        theIcon.Dispose();
                    }

                    // useful to get icon end process of temp. used imgage 
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    ActionPlanAttachmentFile.AttachmentImage = image;
                    AttachmentObjectList.Add(ActionPlanAttachmentFile);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method ChooseFileActionCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
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
                // swallow and return nothing. You could supply a default Icon here as well
            }

            return result;
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ActionPlanAttachmentFile"));
                if (error != null)
                {
                    return;
                }

                if (IsNew)
                {
                    AttachmentObjectList.ToList().ForEach(x => x.Description = Description);
                    IsSave = true;
                }
                else
                {
                    AttachmentObjectList.ToList().ForEach(x => x.Description = Description);
                    IsSave = true;
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                string error = me[BindableBase.GetPropertyName(() => ActionPlanAttachmentFile)] ;


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
               
                string actionPlanAttachmentFile = BindableBase.GetPropertyName(() => ActionPlanAttachmentFile);

               
                if (columnName == actionPlanAttachmentFile)
                {
                    return AddActionPlanAttachmentValidations.GetErrorMessage(actionPlanAttachmentFile, ActionPlanAttachmentFile);
                }
               
                return null;
            }
        }

        #endregion

    }
}
