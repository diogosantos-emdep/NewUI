using DevExpress.Xpf.Core;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SCM;
using System.IO;
using DevExpress.Mvvm;
using System.ComponentModel;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Grid;
using System.Collections.Generic;
using Emdep.Geos.Modules.SCM.Common_Classes;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class AddFamilyImageViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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
        private string windowHeader;
        private bool isSave;
        private byte[] fileInBytes;
        private string imageName;
        private string description;
        string FileTobeSavedByName = "";
        private uint idImage;
        private FamilyImage selectedImage;
        private Int32 idFamily;
        private string error = string.Empty;
        private string connectorFamilySavedImageName;
        private ObservableCollection<FamilyImage> familyImagesList;
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
        public ObservableCollection<FamilyImage> FamilyImagesList
        {
            get { return familyImagesList; }
            set
            {
                familyImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyImagesList"));
            }
        }
        public string ConnectorFamilySavedImageName
        {
            get
            {
                return connectorFamilySavedImageName;
            }
            set
            {
                connectorFamilySavedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorFamilySavedImageName"));
            }
        }
        public Int32 IdFamily
        {
            get
            {
                return idFamily;
            }

            set
            {
                idFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdFamily"));
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
        public FamilyImage SelectedImage
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


        #endregion

        #region Constructor

        public AddFamilyImageViewModel()
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

        public void Init(Int32 idFamily)
        {       
            IdFamily = idFamily;
            if (FamilyImagesList?.Count > 0)
                Position = FamilyImagesList.Select(i => i.Position).Max() + 1;
            else
                Position = 1;
            FamilyImagesList = new ObservableCollection<FamilyImage>();
        }

        private void AddImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageAction()...", category: Category.Info, priority: Priority.Low);

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
                SelectedImage = new FamilyImage();

                if (FamilyImagesList == null)
                    FamilyImagesList = new ObservableCollection<FamilyImage>();

                if (FileInBytes != null)
                {
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = ConnectorFamilySavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ConnectorFamilySavedImageName : ConnectorFamilySavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }

                    SelectedImage.Position = Position; 
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.SavedFileName = ConnectorFamilySavedImageName;
                    SelectedImage.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.IdFamily = Convert.ToUInt16(IdFamily);
                    SelectedImage.ConnectorFamilyImageInBytes = FileInBytes;
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

                    ConnectorFamilySavedImageName = file.Name;

                    //if (string.IsNullOrEmpty(ImageName))
                    //{
                        ImageName = file.Name;
                        int index = ImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ImageName : ImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    //}
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

        public void EditInit(FamilyImage selectedImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);          
                if (selectedImage != null)
                {
                    IdImage = selectedImage.IdSCMFamilyImage;
                    IdFamily = selectedImage.IdFamily;
                    ImageName = selectedImage.OriginalFileName;
                    Description = selectedImage.Description;
                    Position = selectedImage.Position;
                    ConnectorFamilySavedImageName = selectedImage.SavedFileName;
                    FileInBytes = selectedImage.ConnectorFamilyImageInBytes;
                }
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
