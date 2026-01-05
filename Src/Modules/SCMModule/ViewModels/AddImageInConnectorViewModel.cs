using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[pramod.misal][GEOS2-5754][27.08.2024]
    public class AddImageInConnectorViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private bool isCheckedDefaultImage;
        private byte[] fileInBytes;
        private string connectorSavedImageName;
        private string imageName;
        string FileTobeSavedByName = "";
        private string error = string.Empty;
        private bool isNew;
        private SCMConnectorImage selectedImage;
        private ObservableCollection<SCMConnectorImage> imageList;
        private SCMConnectorImage oldDefaultImage;
        private string description;
        private bool isSave;
        private SCMConnectorImage selectedOptionWayDetectionSparePartImage;
        ObservableCollection<SCMConnectorImage> connectorImagesList;
        ObservableCollection<SCMConnectorImage> editConnectorImagesList;
        private string reference;
        private bool isAdd;
        private DateTime updatedDate;
        private Int64 idConnectorImage;
        ObservableCollection<LookupValue> imageTypeList;
        LookupValue selectedImageType;
        private int idPictureType;
        private Int64 idConnector;
        #endregion

        #region Properties
        public ObservableCollection<SCMConnectorImage> ConnectorImagesList
        {
            get { return connectorImagesList; }
            set
            {
                connectorImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorImagesList"));
            }
        }
        public ObservableCollection<SCMConnectorImage> EditConnectorImagesList
        {
            get { return editConnectorImagesList; }
            set
            {
                editConnectorImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditConnectorImagesList"));
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
        public bool IsCheckedDefaultImage
        {
            get
            {
                return isCheckedDefaultImage;
            }
            set
            {
                isCheckedDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedDefaultImage"));
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
        public string ConnectorSavedImageName
        {
            get
            {
                return connectorSavedImageName;
            }
            set
            {
                connectorSavedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorSavedImageName"));
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
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));

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
        public bool IsAdd
        {
            get
            {
                return isAdd;
            }
            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }
        public SCMConnectorImage SelectedImage
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
        public ObservableCollection<SCMConnectorImage> ImageList
        {
            get
            {
                return imageList;
            }
            set
            {
                imageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageList"));
            }
        }
        public SCMConnectorImage OldDefaultImage
        {
            get
            {
                return oldDefaultImage;
            }
            set
            {
                oldDefaultImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldDefaultImage"));
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
        public SCMConnectorImage SelectedOptionWayDetectionSparePartImage
        {
            get
            {
                return selectedOptionWayDetectionSparePartImage;
            }

            set
            {
                selectedOptionWayDetectionSparePartImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartImage"));
            }
        }
        public Int64 IdConnectorImage
        {
            get
            {
                return idConnectorImage;
            }

            set
            {
                idConnectorImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdConnectorImage"));
            }
        }
        public int IdPictureType
        {
            get
            {
                return idPictureType;
            }

            set
            {
                idPictureType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdPictureType"));
            }
        }
        public Int64 IdConnector
        {
            get
            {
                return idConnector;
            }

            set
            {
                idConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdConnector"));
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
        public ObservableCollection<LookupValue> ImageTypeList
        {
            get { return imageTypeList; }
            set
            {
                imageTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageTypeList"));
            }
        }
        
        public LookupValue SelectedImageType
        {
            get { return selectedImageType; }
            set
            {
                selectedImageType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImageType"));
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
        public AddImageInConnectorViewModel()
        {
            try
            {
                AcceptImageActionCommand = new DelegateCommand<object>(AddImageInConnectorAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                IsCheckedDefaultImage = false;
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                //[rdixit][GEOS2-9199][11.09.2025]
                FillImageTypes();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddImageInConnectorViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
           
        }
        #endregion

        #region Method

        public void Init(ObservableCollection<SCMConnectorImage> ConnImageList)
        {
            ConnectorImagesList = new ObservableCollection<SCMConnectorImage>();
            if (ConnImageList != null)
            {
                foreach (var Images in ConnImageList)
                {
                    ConnectorImagesList.Add(Images);
                }
            }
        }

        //[rdixit][GEOS2-9199][11.09.2025]
        private void FillImageTypes()
        {
            try
            {
                ICrmService CRMServiceThreadingLocal = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ImageTypeList = new ObservableCollection<LookupValue>(CRMServiceThreadingLocal.GetLookupValues(186));
                SelectedImageType = null;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillImageTypes()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillImageTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-5754][01.07.2024]  
        public void EditInit(SCMConnectorImage connectorImage, ObservableCollection<SCMConnectorImage> ConnImageList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdConnectorImage = connectorImage.IdConnectorImage;    
                IdConnector = connectorImage.IdConnector;
                IdPictureType = connectorImage.IdPictureType;
                Reference = connectorImage.Ref;             
                ImageName = connectorImage.SavedFileName;   
                Description = connectorImage.Description;
                ConnectorSavedImageName = connectorImage.SavedFileName;
                FileInBytes = connectorImage.ConnectorsImageInBytes;
                UpdatedDate = GeosApplication.Instance.ServerDateTime.Date.Date;

                ConnectorImagesList = new ObservableCollection<SCMConnectorImage>();
                SelectedImage = connectorImage;
                SelectedImage.isDelVisible = true;
                ConnectorImagesList.Add(SelectedImage);

                if (connectorImage.Position==1)
                {
                    IsCheckedDefaultImage = true;
                }

                if (ConnImageList != null)
                {
                    EditConnectorImagesList = new ObservableCollection<SCMConnectorImage>();
                    foreach (var Images in ConnImageList)
                    {
                        EditConnectorImagesList.Add(Images);
                    }

                }
                //[rdixit][GEOS2-9199][11.09.2025]
                if (IdPictureType == 0)
                    SelectedImageType = ImageTypeList.FirstOrDefault(x => x.IdLookupValue == 2285);
                else
                    SelectedImageType = ImageTypeList.FirstOrDefault(x => x.IdLookupValue == 2286);
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        //[pramod.misal][GEOS2-5754][01.07.2024]  
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                dlg.Title = "Select a picture";
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp;*.wtg|" +
                             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                             "Portable Network Graphic (*.png)|*.png|" +
                             "Bitmap (*.bmp)|*.bmp|" +
                             "WTG Image (*.wtg)|*.wtg";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);

                    FileInfo file = new FileInfo(dlg.FileName);
                    ConnectorSavedImageName = file.Name;

                    string fileExtension = file.Extension.ToLower();
                    IdPictureType = 0;
                    switch (fileExtension)
                    {
                        case ".bmp":
                        case ".wtg":
                            IdPictureType = 1; // Set 1 for .wtg and .bmp
                            break;
                        default:
                            IdPictureType = 0;
                            break;
                    }

                    //[rdixit][GEOS2-9199][11.09.2025]
                    if (IdPictureType == 0)
                        SelectedImageType = ImageTypeList.FirstOrDefault(x => x.IdLookupValue == 2285);
                    else
                        SelectedImageType = ImageTypeList.FirstOrDefault(x => x.IdLookupValue == 2286);
                    ImageName = file.Name;

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
                //IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5754][01.07.2024]  
        private void AddImageInConnectorAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddImageInConnectorAction()...", category: Category.Info, priority: Priority.Low);
                //allowValidation = true;
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ImageName"));

                if (error != null)
                {
                    return;
                }

                if (IsNew)
                {
                    SelectedImage = new SCMConnectorImage();
                    if (IsCheckedDefaultImage && FileInBytes != null)
                    {
                        if (ImageList?.Count > 0)
                        {
                            OldDefaultImage = ImageList.Where(a => a.Position == 1).FirstOrDefault();
                            OldDefaultImage.Position = (ulong)ImageList.Count + 1;
                            SelectedImage.Position = 1;
                        }
                        else
                        {
                            SelectedImage.Position = 1;
                        }
                    }
                    else
                    {
                        ulong pos = SelectedImage.Position;
                        if (ImageList != null)
                        {
                            #region [rdixit][24.09.2024][GEOS2-6468]
                            FileInfo file = new FileInfo(ImageName);

                            if (file.Extension == ".wtg" || file.Extension == ".bmp")
                            {
                                var maxPosition = ImageList.Where(i => i.IdPictureType == 1).Select(j => (long)j.Position).DefaultIfEmpty(0).Max();
                                SelectedImage.Position = (ulong)(maxPosition + 1);
                            }
                            else
                            {
                                var maxPosition = ImageList.Where(i => i.IdPictureType == 0).Select(j => (long)j.Position).DefaultIfEmpty(0).Max();
                                SelectedImage.Position = (ulong)(maxPosition + 1);
                            }
                            #endregion
                        }
                        else
                            SelectedImage.Position = 1;
                    }

                    if (FileInBytes != null)
                    {
                        if (string.IsNullOrEmpty(ImageName))
                        {
                            int index = ConnectorSavedImageName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? ConnectorSavedImageName : ConnectorSavedImageName.Substring(0, index);
                            ImageName = FileTobeSavedByName;
                        }
                        SelectedImage.OriginalFileName = ImageName;
                        SelectedImage.SavedFileName = ImageName;
                        SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        //[rdixit][GEOS2-9199][11.09.2025]
                        if (SelectedImageType?.IdLookupValue == 2285)
                            SelectedImage.IdPictureType = 0;
                        else
                            SelectedImage.IdPictureType = 1;
                        
                        SelectedImage.ConnectorsImageInBytes = FileInBytes;
                        SelectedImage.Description = Description;
                        SelectedImage.isDelVisible = true;
                        SelectedImage.UpdatedDate = GeosApplication.Instance.ServerDateTime.Date.Date;
                        IsSave = true;
                    }
                }
                else
                {
                    ulong pos = SelectedImage.Position;
                    if (IsCheckedDefaultImage)
                    {
                        if (pos != 1)
                        {
                            if (ImageList?.Count > 0)
                            {
                                OldDefaultImage = ImageList.Where(a => a.Position == 1).FirstOrDefault();
                                OldDefaultImage.Position = pos;
                                SelectedImage.Position = 1;
                            }
                            else
                            {
                                SelectedImage.Position = 1;
                            }
                        }
                        else
                        {
                            SelectedImage.Position = 1;
                        }
                    }
                    else
                    {
                        SelectedImage.Position = pos;
                    }

                    SelectedImage.SavedFileName = ConnectorSavedImageName;
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = ConnectorSavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ConnectorSavedImageName : ConnectorSavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    SelectedImage.SavedFileName = ImageName;
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    //[rdixit][GEOS2-9199][11.09.2025]
                    if (SelectedImageType?.IdLookupValue == 2285)
                        SelectedImage.IdPictureType = 0;
                    else
                        SelectedImage.IdPictureType = 1;
                    SelectedImage.isDelVisible = true;
                    SelectedImage.ConnectorsImageInBytes = FileInBytes;
                    SelectedImage.Description = Description;
                    SelectedImage.UpdatedDate = GeosApplication.Instance.ServerDateTime.Date.Date;
                    IsSave = true;

                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddImageInConnectorAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageInConnectorAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                                me[BindableBase.GetPropertyName(() => ImageName)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        //string IDataErrorInfo.this[string columnName]
        //{
        //    get
        //    {
        //        if (!allowValidation) return null;

        //        string imageName = BindableBase.GetPropertyName(() => ImageName);

        //        if (columnName == imageName)
        //        {
        //            return AddEditConImageValidation.GetErrorMessage(imageName, ImageName);
        //        }
        //        if (columnName == imageName)
        //        {
        //            // Apply validation for already existing record in SelectedConnectorFile
        //            if (ConnectorImagesList !=null)
        //            {
        //                try
        //                {
        //                    var existingImage = ConnectorImagesList.FirstOrDefault(x => x.OriginalFileName != null && x.OriginalFileName.ToString().Trim() == ImageName.ToString());
        //                    if (existingImage != null)
        //                    {
        //                        return "The selected file already exists.";
        //                    }

        //                }
        //                catch (Exception ex)
        //                {

        //                    throw;
        //                }
        //            }
        //            else
        //            {
        //                return AddEditConImageValidation.GetErrorMessage(imageName, ImageName);
        //            }
        //        }

        //        return null;
        //    }
        //}


        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string imageName = BindableBase.GetPropertyName(() => ImageName);

                if (columnName == imageName)
                {
                    // Apply general validation
                    string errorMessage = AddEditConImageValidation.GetErrorMessage(imageName, ImageName);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return errorMessage;
                    }

                    // Apply validation for already existing record in ConnectorImagesList
                    if (ConnectorImagesList != null)
                    {
                        //var IsNew = ConnectorImagesList.FirstOrDefault(x => x.AttachmentImage == null);
                        if (IsNew)
                        {

                            var existingImage = ConnectorImagesList.FirstOrDefault(x => x.SavedFileName != null && x.SavedFileName.ToString().Trim() == ImageName);
                            if (existingImage != null)
                            {
                                return "The selected Image already exists.";
                            }

                        }
                        else
                        {

                            var existingImage = EditConnectorImagesList.Where(x=>x.IdConnectorImage != SelectedImage.IdConnectorImage)?.FirstOrDefault(x => x.SavedFileName != null && x.SavedFileName.ToString().Trim() == ImageName);
                            if (existingImage != null)
                            {
                                return "The selected Image already exists.";
                            }
                        }


                    }

                }

                return null;
            }
        }

        #endregion


    }
}
