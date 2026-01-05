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
using DevExpress.Mvvm;
using System.ComponentModel;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddImageInProductTypeViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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

        private string windowHeader;
        private bool isNew;
        private bool isSave;

        private byte[] fileInBytes;
        private string productTypeSavedImageName;
        private string imageName;
        private string description;
        private DateTime updatedDate;

        string FileTobeSavedByName = "";
        private bool isCheckedDefaultImage;

        private ObservableCollection<ProductTypeImage> imageList;
        private ProductTypeImage selectedImage;
        private ulong idImage;
        private ObservableCollection<ProductTypeImage> productTypeImageList;
        private ProductTypeImage oldDefaultImage;
        private bool isImageReadOnly=false;
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

        public string ProductTypeSavedImageName
        {
            get
            {
                return productTypeSavedImageName;
            }
            set
            {
                productTypeSavedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeSavedImageName"));
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

        public ObservableCollection<ProductTypeImage> ImageList
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

        public ProductTypeImage SelectedImage
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

        public ulong IdImage
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

        public ObservableCollection<ProductTypeImage> ProductTypeImageList
        {
            get
            {
                return productTypeImageList;
            }
            set
            {
                productTypeImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypeImageList"));
            }
        }

        public ProductTypeImage OldDefaultImage
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
        public bool IsImageReadOnly
        {
            get
            {
                return isImageReadOnly;
            }

            set
            {
                isImageReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageReadOnly"));
            }
        }

        #endregion

        #region ICommand
        public ICommand AcceptImageActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }

        #endregion

        #region Constructor

        public AddImageInProductTypeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddImageInProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptImageActionCommand = new DelegateCommand<object>(ProductTypeImageAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                IsCheckedDefaultImage = false;

                GeosApplication.Instance.Logger.Log("Constructor AddImageInProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddImageInProductTypeViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        

        #endregion

        #region Methods


        private void ProductTypeImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeFileAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ImageName"));

                if (error != null)
                {
                    return;
                }

                if (IsNew)
                {
                    SelectedImage = new ProductTypeImage();
                    if (IsCheckedDefaultImage && FileInBytes != null)
                    {
                        if (ImageList.Count > 0)
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
                        SelectedImage.Position = (ulong)ImageList.Count + 1;
                    }

                    if (FileInBytes != null)
                    {
                        if (string.IsNullOrEmpty(ImageName))
                        {
                            int index = ProductTypeSavedImageName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? ProductTypeSavedImageName : ProductTypeSavedImageName.Substring(0, index);
                            ImageName = FileTobeSavedByName;
                        }
                        SelectedImage.OriginalFileName = ImageName;
                        SelectedImage.SavedFileName = ProductTypeSavedImageName;
                        SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedImage.IdCPType = 1;
                        SelectedImage.ProductTypeImageInBytes = FileInBytes;
                        SelectedImage.Description = Description;
                        SelectedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
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
                            if (ImageList.Count > 0)
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
                    SelectedImage.SavedFileName = ProductTypeSavedImageName;
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = ProductTypeSavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ProductTypeSavedImageName : ProductTypeSavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.IdCPType = 1;
                    SelectedImage.ProductTypeImageInBytes = FileInBytes;
                    SelectedImage.Description = Description;
                    SelectedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

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

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

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
                    ProductTypeSavedImageName = file.Name;

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
     
        public void EditInit(ProductTypeImage productTypeImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdImage = productTypeImage.IdCPTypeImage;
                ImageName = productTypeImage.OriginalFileName;
                ProductTypeSavedImageName = productTypeImage.SavedFileName;

                Description = productTypeImage.Description;
                FileInBytes = productTypeImage.ProductTypeImageInBytes;
                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                ProductTypeImageList = new ObservableCollection<ProductTypeImage>();
                SelectedImage = productTypeImage;
                ProductTypeImageList.Add(SelectedImage);
               
                if (productTypeImage.Position == 1)
                {
                    IsCheckedDefaultImage = true;
                    IsImageReadOnly = true;
                }
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    return AddEditModuleValidation.GetErrorMessage(imageName, ImageName);
                }

                return null;
            }
        }
        #endregion
    }
}
