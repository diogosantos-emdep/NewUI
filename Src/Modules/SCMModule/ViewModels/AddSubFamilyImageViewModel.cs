using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
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
using System.Linq;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Modules.SCM.Common_Classes;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class AddSubFamilyImageViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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
        UInt64 position;
        private string error = string.Empty;
        private bool isNew;
        private bool isSave;
        private byte[] fileInBytes;
        private string imageName;
        private string description;
        private DateTime updatedDate;
        string FileTobeSavedByName = "";
        private ObservableCollection<SubFamilyImage> imagesList;
        private string savedImageName;
        private uint idImage;
        private SubFamilyImage selectedImage;
        #endregion

        #region Properties
        public UInt64 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Position"));
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
        public ObservableCollection<SubFamilyImage> ImagesList
        {
            get
            {
                return imagesList;
            }

            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));

            }
        }
        public string ImageName
        {
            get
            {
                return imageName;
            }

            set
            {
                imageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageName"));

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
        public string SavedImageName
        {
            get
            {
                return savedImageName;
            }
            set
            {
                savedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SavedImageName"));
            }
        }
        public uint IdImage
        {
            get
            {
                return idImage;
            }

            set
            {
                idImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdImage"));
            }
        }
        public SubFamilyImage SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                selectedImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }
        #endregion

        #region ICommand
        public ICommand AcceptImageActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        //public ICommand PreviewDragEnterCommand { get; set; }
        //public ICommand PreviewDragOverCommand { get; set; }
        //public ICommand PreviewDropCommand { get; set; }


        #endregion

        #region Constructor

        public AddSubFamilyImageViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddFamilyImageViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptImageActionCommand = new DelegateCommand<object>(AddImageAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                GeosApplication.Instance.Logger.Log("Constructor AddFamilyImageViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddFamilyImageViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Methods

        private void AddImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()...", category: Category.Info, priority: Priority.Low);
                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                if (Description.Contains("\r\n"))
                {
                    Description = Description.Trim(trimChars);
                }
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ImageName"));

                if (error != null)
                {
                    return;
                }
                if (FileInBytes != null)
                {
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = SavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? SavedImageName : SavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    SelectedImage.Position = Position;
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.SavedFileName = SavedImageName;
                    SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.SubFamilyImageInBytes = FileInBytes;
                    SelectedImage.Description = Description;
                    SelectedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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

                dlg.Title = "Select a picture";
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {

                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);

                    FileInfo file = new FileInfo(dlg.FileName);
                    SavedImageName = file.Name;

                    if (string.IsNullOrEmpty(ImageName))
                    {
                        ImageName = file.Name;
                        int index = ImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ImageName : ImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
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
        public void EditInit(SubFamilyImage selectedImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                if (selectedImage != null)
                {
                    Position = selectedImage.Position;
                    IdImage = selectedImage.IdSubFamilyImage;
                    ImageName = selectedImage.OriginalFileName;
                    Description = selectedImage.Description;
                    SavedImageName = selectedImage.SavedFileName;
                    FileInBytes = selectedImage.SubFamilyImageInBytes;
                    UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                    ImagesList = new ObservableCollection<SubFamilyImage>();
                    SelectedImage = selectedImage;
                    ImagesList.Add(SelectedImage);
                }
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(SubFamilyImage selectedImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                SelectedImage = selectedImage;
                if (ImagesList?.Count > 0)
                    Position = ImagesList.Select(i => i.Position).Max() + 1;
                else
                    Position = 1;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                                me[BindableBase.GetPropertyName(() => ImageName)];

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

                string imageName = BindableBase.GetPropertyName(() => ImageName);

                if (columnName == imageName)
                {
                    return AddEditConnectorFamilyValidation.GetErrorMessage(imageName, null, ImageName);
                }

                return null;
            }
        }
        #endregion
    }
}
