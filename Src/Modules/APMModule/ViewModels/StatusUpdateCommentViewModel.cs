using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Data.Common.APM;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using DevExpress.Mvvm.Native;

namespace Emdep.Geos.Modules.APM.ViewModels
{
    public class StatusUpdateCommentViewModel : INotifyPropertyChanged, IDisposable, IDataErrorInfo
    {

        #region service

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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Declaration
        public bool IsSave { get; set; }
        private string informationError;
        private string comment;

        private ObservableCollection<AttachmentsByTask> attachmentObjectList;//[Sudhir.Jangra][GEOS2-7007]
        private string taskDescription;//[Sudhir.Jangra][GEOS2-7007]
        private string fileName;//[Sudhir.Jangra][GEOS2-7007]
        private string uniqueFileName;//[Sudhir.Jangra][GEOS2-7007]
        private string fileNameString;//[Sudhir.Jangra][GEOS2-7007]
        private bool isBusy;//[Sudhir.Jangra][GEOS2-7007]
        private AttachmentsByTask attachment;//[Sudhir.Jangra][GEOS2-7007]
        private bool isDescriptionEnabled;//[Sudhir.Jangra][GEOS2-7007]
        #endregion

        #region Properties 


        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public ObservableCollection<AttachmentsByTask> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
                if (AttachmentObjectList!=null&& AttachmentObjectList.Count>0)
                {
                    IsDescriptionEnabled = true;
                }
                else
                {
                    IsDescriptionEnabled = false;
                }
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public string TaskDescription
        {
            get { return taskDescription; }
            set
            {
                taskDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TaskDescription"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set
            {
                uniqueFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public string FileNameString
        {
            get { return fileNameString; }
            set
            {
                fileNameString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileNameString"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public AttachmentsByTask Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public bool IsDescriptionEnabled
        {
            get { return isDescriptionEnabled; }
            set
            {
                isDescriptionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDescriptionEnabled"));
            }
        }

        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; } //[Shweta.Thube][GEOS2-5976]

        public ICommand AcceptButtonCommand { get; set; } //[Shweta.Thube][GEOS2-5976]

        public ICommand ChooseFileActionCommand { get; set; }//[Sudhir.Jangra][GEOS2-7007]
        #endregion

        #region Constructor

        public StatusUpdateCommentViewModel()
        {
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction)); //[Shweta.Thube][GEOS2-5976]
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow)); //[Shweta.Thube][GEOS2-5976]
            ChooseFileActionCommand = new RelayCommand(new Action<object>(ChooseFileActionCommandAction));//[Sudhir.Jangra][GEOS2-7007]
        }
        #endregion

        #region Methods                           
        private void CloseWindow(object obj)
        {
            try
            {
                IsSave = false;
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Shweta.Thube][GEOS2-5976]
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                InformationError = null;
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("Comment"));

                if (error != null)
                {
                    //IsBusy = false;
                    return;
                }
                if (AttachmentObjectList==null)
                {
                    AttachmentObjectList = new ObservableCollection<AttachmentsByTask>();
                }
                if (AttachmentObjectList!=null&& AttachmentObjectList.Count>0)
                {
                    AttachmentObjectList.ForEach(x => x.AttachmentImage = null);
                    AttachmentObjectList.ForEach(x => x.Description = TaskDescription);
                }

                IsSave = true;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Sudhir.Jangra][GEOS2-7007]
        public void ChooseFileActionCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            // DXSplashScreen.Show<SplashScreenView>();
            // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    var newFileList = AttachmentObjectList != null ? new ObservableCollection<AttachmentsByTask>(AttachmentObjectList) : new ObservableCollection<AttachmentsByTask>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new AttachmentsByTask();
                    Attachment.FileByte = System.IO.File.ReadAllBytes(dlg.FileName);
                    Attachment.FileType = file.Extension;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = file.Name;
                    Attachment.CreatedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSize = file.Length;
                    Attachment.FileType = file.Extension;
                    Attachment.FileUploadName = file.Name;
                    Attachment.IsUploaded = true;

                    Attachment.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    Attachment.CreatedByName = GeosApplication.Instance.ActiveUser.FullName;
                    Attachment.CreatedIn = DateTime.Now;

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
                    Attachment.AttachmentImage = image;

                    // not allow to add same files
                    List<AttachmentsByTask> fooList = newFileList.OfType<AttachmentsByTask>().ToList();
                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    AttachmentObjectList = newFileList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
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
        #region //[Shweta.Thube][GEOS2-5979] 
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
                string error = me[BindableBase.GetPropertyName(() => Comment)];

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
                string CommentProp = BindableBase.GetPropertyName(() => Comment);

                if (columnName == CommentProp)
                {
                    return AddStatusUpdateCommentValidation.GetErrorMessage(CommentProp, Comment);
                }

                return null;
            }
        }
        #endregion
        #endregion
    }
}
