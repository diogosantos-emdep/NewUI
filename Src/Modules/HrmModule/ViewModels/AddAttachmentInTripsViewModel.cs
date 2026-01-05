using DevExpress.Mvvm;
using DevExpress.Utils.Design.DataAccess;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
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
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    // [nsatpute][16-09-2024][GEOS2-5931]
    public class AddAttachmentInTripsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private string code;
        private string description;
        private bool isNew;
        private bool isSave;
        private string error = string.Empty;
        private bool inUse;
        private string windowHeader;
        private ObservableCollection<LookupValue> attachmentTypeList;
        private LookupValue selectedAttachmentType;
        private bool isBusy;
        private String fileName;
        private List<object> tripAttachmentList;
        private string uniqueFileName;
        private TripAttachment tripAttachmentFile;
        private string fileNameString;
        public ObservableCollection<TripAttachment> listAttachment;
        private ObservableCollection<TripAttachment> attachmentObjectList;
        private int attachmentIndex = 0;
        private double? amount;// [pallavi.kale][28-05-2025][GEOS2-7941]
        private Visibility isAmountVisible=Visibility.Hidden;// [pallavi.kale][28-05-2025][GEOS2-7941]

        #endregion

        #region Properties
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
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

        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
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


        public ObservableCollection<LookupValue> AttachmentTypeList
        {
            get {
                return attachmentTypeList; 
                }
            set {
                  attachmentTypeList = value;
                  OnPropertyChanged(new PropertyChangedEventArgs("AttachmentTypeList")); 
            }
        }


        public LookupValue SelectedAttachmentType
        {
            get { return selectedAttachmentType; }
            set { if(value != null) 
                    selectedAttachmentType = value;

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachmentType"));
               
              // [pallavi.kale][28-05-2025][GEOS2-7941]
              if (SelectedAttachmentType != null)
                { 
                    if (SelectedAttachmentType.IdLookupValue == 2122 ||
                        SelectedAttachmentType.IdLookupValue == 2123 ||
                        SelectedAttachmentType.IdLookupValue == 2128 ||
                        SelectedAttachmentType.IdLookupValue == 2133)
                    {
                        IsAmountVisible = Visibility.Visible;
                    }
                   else
                   {
                        IsAmountVisible = Visibility.Hidden;

                   }
                }
            }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(new PropertyChangedEventArgs("FileName")); }
        }
        public List<object> TripAttachmentList
        {
            get { return tripAttachmentList; }
            set
            {
                tripAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TripAttachmentList"));
            }
        }

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }
        public TripAttachment TripAttachmentFile
        {
            get { return tripAttachmentFile; }
            set { tripAttachmentFile = value; OnPropertyChanged(new PropertyChangedEventArgs("TripAttachmentFile")); }
        }
        public string FileNameString
        {
            get { return fileNameString; }
            set { fileNameString = value; OnPropertyChanged(new PropertyChangedEventArgs("FileNameString")); }
        }

        public ObservableCollection<TripAttachment> ListAttachment
        {
            get { return listAttachment; }
            set { listAttachment = value; OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment")); }
        }

        public ObservableCollection<TripAttachment> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }
        private List<TripAttachment> updatedFileList;
        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public double? Amount
        {
            get { return amount; }
            set
            {
                  amount = value; 
                  OnPropertyChanged(new PropertyChangedEventArgs("Amount")); 
            }
        }
        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public Visibility IsAmountVisible
        {
            get { return isAmountVisible; }
            set
            {
                isAmountVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAmountVisible"));
            }
        }

        #endregion

        #region Public ICommands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        #endregion

        #region Constructor
        public AddAttachmentInTripsViewModel(ObservableCollection<TripAttachment> ListAttachment, ObservableCollection<LookupValue> attachmentTypeList, List<TripAttachment> updatedFileList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditObjectivesViewModel ...", category: Category.Info, priority: Priority.Low);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                EscapeButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                ChooseFileActionCommand = new RelayCommand(new Action<object>(ChooseFileActionCommandAction));
                AttachmentObjectList = new ObservableCollection<TripAttachment>();
                this.ListAttachment = ListAttachment;
                this.updatedFileList = updatedFileList;
                Description = string.Empty;
                this.AttachmentTypeList = attachmentTypeList;
                SelectedAttachmentType = null;
                InUse = true;
				// [pallavi.kale][28-05-2025][GEOS2-7941]
                if (SelectedAttachmentType != null)
                {
                    if (SelectedAttachmentType.IdLookupValue == 2122 ||
                        SelectedAttachmentType.IdLookupValue == 2123 ||
                        SelectedAttachmentType.IdLookupValue == 2128 ||
                        SelectedAttachmentType.IdLookupValue == 2133
                        )
                    {
                        IsAmountVisible = Visibility.Visible;
                    }
                }
                GeosApplication.Instance.Logger.Log("Constructor AddEditObjectivesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditObjectivesViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void EditInit(TripAttachment attachment, int attachmentIndex)
        {
            TripAttachmentFile = attachment;
            this.attachmentIndex = attachmentIndex;
            SelectedAttachmentType = TripAttachmentFile.TripAttachmentType;
            if (TripAttachmentFile.FileName.Contains("."))
            {
                string[] a = TripAttachmentFile.FileName.Split('.');
                FileNameString = a[0];
            }
            else
            {
                FileNameString = TripAttachmentFile.FileName;
            }
            Description = TripAttachmentFile.Description;
            AttachmentObjectList.Add(TripAttachmentFile);
            Amount = TripAttachmentFile.Amount;
			// [pallavi.kale][28-05-2025][GEOS2-7941]
            if (SelectedAttachmentType != null)
            {
                if (SelectedAttachmentType.IdLookupValue == 2122 ||
                    SelectedAttachmentType.IdLookupValue == 2123 ||
                    SelectedAttachmentType.IdLookupValue == 2128 ||
                    SelectedAttachmentType.IdLookupValue == 2133
                    )
                {
                    IsAmountVisible = Visibility.Visible;
                }
            }

        }
        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public void AddInit()
        {
            Amount = null;

        }

        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                // allowValidation = true;
                bool fileExists = false;

                if (IsNew)
                    fileExists =  ListAttachment.Any(x => x.FileName == TripAttachmentFile?.FileName);
                else
                    fileExists =  ListAttachment.Where((x, index) => index != attachmentIndex).Any(x => x.FileName == TripAttachmentFile?.FileName);

                if (SelectedAttachmentType == null || TripAttachmentFile  == null ||  fileExists)
                {
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedAttachmentType"));
                    PropertyChanged(this, new PropertyChangedEventArgs("TripAttachmentFile"));
                    //if (error != null)
                    //{
                    //    return;
                    //}
                    if (!string.IsNullOrEmpty(error))
                    {
                        return;  // Prevent submission if there's any validation error
                    }
                }
                TripAttachmentFile.TripAttachmentType = SelectedAttachmentType;
                TripAttachmentFile.Description = Description;
                TripAttachmentFile.Amount = Amount;// [pallavi.kale][28-05-2025][GEOS2-7941]

                if (IsNew)
                    ListAttachment.Add(TripAttachmentFile);
                else
                {
                    TripAttachment oldAttachment = ListAttachment[attachmentIndex];
                    bool fileEdited = false;
                    if (oldAttachment.FileName != TripAttachmentFile.FileName)
                        fileEdited = true;
                    if (oldAttachment.TripAttachmentType.IdLookupValue != TripAttachmentFile.TripAttachmentType.IdLookupValue)
                        fileEdited = true;
                    if (oldAttachment.Description != TripAttachmentFile.Description)
                        fileEdited = true;
                    if (oldAttachment.Amount != TripAttachmentFile.Amount)
                        fileEdited = true;	// [pallavi.kale][28-05-2025][GEOS2-7941]

                    if (fileEdited)
                    {
                        if (TripAttachmentFile.IdAttachment > 0)
                            TripAttachmentFile.TransactionOperation = ModelBase.TransactionOperations.Update;
                        int index = ListAttachment.IndexOf(oldAttachment);
                        ListAttachment.Remove(oldAttachment);
                        ListAttachment.Insert(attachmentIndex, TripAttachmentFile);
                        if(updatedFileList.Any(x=> x.IdAttachment == TripAttachmentFile.IdAttachment))
                        {
                            updatedFileList.Remove(updatedFileList.FirstOrDefault(x => x.IdAttachment == TripAttachmentFile.IdAttachment));
                        }
                        updatedFileList.Add(oldAttachment);
                    }
                }
                IsSave = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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
                    if (IsNew)
                        TripAttachmentFile = new TripAttachment();
                    TripAttachmentFile.FileType = file.Extension;
                    TripAttachmentFile.FilePath = file.FullName;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    TripAttachmentFile.FileName = file.Name;

                    TripAttachmentFile.OriginalFileName = FileNameString;
                    TripAttachmentFile.SavedFileName = UniqueFileName + file.Extension;
                    TripAttachmentFile.TripAttachmentType = SelectedAttachmentType;
                    TripAttachmentFile.Description = Description;
                    TripAttachmentFile.UploadedIn = GeosApplication.Instance.ServerDateTime;
                    TripAttachmentFile.FileSizeInInt = file.Length;
                    if (TripAttachmentFile.FileSizeInInt >= 1024 * 1024)
                    {
                        // If the file size is 1 MB or larger, display in MB
                        TripAttachmentFile.FileSize = Math.Ceiling((double)TripAttachmentFile.FileSizeInInt / (1024 * 1024)).ToString("0") + " MB";
                    }
                    else
                    {
                        // If the file size is less than 1 MB, display in KB
                        TripAttachmentFile.FileSize = Math.Ceiling((double)TripAttachmentFile.FileSizeInInt / 1024).ToString("0") + " KB";
                    }

                    TripAttachmentFile.FileExtension = file.Extension;
                    TripAttachmentFile.FileUploadName = file.Name;
                    TripAttachmentFile.IsUploaded = true;
                    TripAttachmentFile.TransactionOperation = ModelBase.TransactionOperations.Add;
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
                    TripAttachmentFile.AttachmentImage = image;
                    AttachmentObjectList.Add(TripAttachmentFile);
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

        #endregion

        #region Validation
        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
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

                string error = string.Join(Environment.NewLine,
                                           ((IDataErrorInfo)this)[nameof(SelectedAttachmentType)],
                                           ((IDataErrorInfo)this)[nameof(TripAttachmentFile)]);
                //((IDataErrorInfo)this)[nameof(Description)]).Trim();

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

                // Validate each field using ObjectiveValidation.GetErrorMessage
                if (columnName == nameof(SelectedAttachmentType))
                {
                    return AddTripAttachmentValidations.GetErrorMessage("SelectedAttachmentType", SelectedAttachmentType);
                }
                if (columnName == nameof(TripAttachmentFile))
                {
                    bool fileExists = false;
                    if (IsNew)
                        fileExists = ListAttachment.Any(x => x.FileName == TripAttachmentFile?.FileName);
                    else
                        fileExists = ListAttachment.Where((x, index) => index != attachmentIndex).Any(x => x.FileName == TripAttachmentFile?.FileName);

                    return AddTripAttachmentValidations.GetErrorMessage("TripAttachmentFile", TripAttachmentFile, fileExists);
                }
                //if (columnName == nameof(Description))
                //{
                //    return AddTripAttachmentValidations.GetErrorMessage("Description", Description);
                //}
                return null;
            }
        }

        #endregion
    }
}
