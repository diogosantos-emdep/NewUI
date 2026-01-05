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
    public class AddEditPCMArticleImageViewModel : ViewModelBase, INotifyPropertyChanged
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
        private string articleSavedImageName;
        private string imageName;
        private string description;
        private DateTime updatedDate;

        string FileTobeSavedByName = "";
        private bool isCheckedDefaultImage;

        private ObservableCollection<PCMArticleImage> imageList;
        private PCMArticleImage selectedImage;
        private ulong idImage;
        private ObservableCollection<PCMArticleImage> articleImageList;
        private PCMArticleImage oldDefaultImage;
        private bool isImageReadOnly = false;
        private bool isReadOnlyField;
        private bool isAcceptDisable;
        private ObservableCollection<PCMArticleImage> imagesList;
        private byte isImageShareWithCustomer;
        public int Use;


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

        public string ArticleSavedImageName
        {
            get
            {
                return articleSavedImageName;
            }
            set
            {
                articleSavedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSavedImageName"));
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

        public ObservableCollection<PCMArticleImage> ImageList
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

        public PCMArticleImage SelectedImage
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

        public ObservableCollection<PCMArticleImage> ArticleImageList
        {
            get
            {
                return articleImageList;
            }
            set
            {
                articleImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImageList"));
            }
        }

        public PCMArticleImage OldDefaultImage
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
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }
        public bool IsAcceptDisable
        {
            get { return isAcceptDisable; }
            set
            {
                isAcceptDisable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptDisable"));
            }
        }
        public ObservableCollection<PCMArticleImage> ImagesList
        {
            get { return imagesList; }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }

        public byte IsImageShareWithCustomer
        {
            get { return isImageShareWithCustomer; }
            set
            {
                isImageShareWithCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageShareWithCustomer"));
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

/// [001][smazhar][21-08-2019][GEOS2-2551]Must be possible "Set as main picture" to the warehouse Article picture 

        public AddEditPCMArticleImageViewModel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditPCMArticleImageViewModel ...", category: Category.Info, priority: Priority.Low);
                PCMArticleImage tempObject = new PCMArticleImage();
                tempObject = (PCMArticleImage)obj;
                IsAcceptDisable = true;
                if (tempObject!=null)
                {
                    HideFiled(tempObject);
                }
                
                AcceptImageActionCommand = new DelegateCommand<object>(ArticleImageAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                IsCheckedDefaultImage = false;
               

                GeosApplication.Instance.Logger.Log("Constructor AddEditPCMArticleImageViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditPCMArticleImageViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods


        public void ArticleImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ArticleImageAction()...", category: Category.Info, priority: Priority.Low);
                if (IsNew)
                {
                    SelectedImage = new PCMArticleImage();
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
                            int index = ArticleSavedImageName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? ArticleSavedImageName : ArticleSavedImageName.Substring(0, index);
                            ImageName = FileTobeSavedByName;
                        }
                        SelectedImage.OriginalFileName = ImageName;
                        SelectedImage.SavedFileName = ArticleSavedImageName;
                        SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedImage.IdArticle = 1;
                        SelectedImage.PCMArticleImageInBytes = FileInBytes;
                        SelectedImage.Description = Description;
                        SelectedImage.IsImageShareWithCustomer = IsImageShareWithCustomer;
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
                    SelectedImage.SavedFileName = ArticleSavedImageName;
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = ArticleSavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ArticleSavedImageName : ArticleSavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.IdArticle = 1;
                    SelectedImage.PCMArticleImageInBytes = FileInBytes;
                    SelectedImage.Description = Description;
                    SelectedImage.IsImageShareWithCustomer = IsImageShareWithCustomer;
                    SelectedImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                    IsSave = true;
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ArticleImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ArticleImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                    ArticleSavedImageName = file.Name;

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

        public void EditInit(PCMArticleImage productTypeImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdImage = productTypeImage.IdPCMArticleImage;
                ImageName = productTypeImage.OriginalFileName;
                ArticleSavedImageName = productTypeImage.SavedFileName;

                Description = productTypeImage.Description;
                FileInBytes = productTypeImage.PCMArticleImageInBytes;
                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                ArticleImageList = new ObservableCollection<PCMArticleImage>();
                SelectedImage = productTypeImage;
                ArticleImageList.Add(SelectedImage);

                if (productTypeImage.Position == 1)
                {
                    IsCheckedDefaultImage = true;
                    IsImageReadOnly = true;
                }
                //if(productTypeImage.IsWarehouseImage == 1)
                IsImageShareWithCustomer = productTypeImage.IsImageShareWithCustomer;
               

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

         //[011]
        private void HideFiled(PCMArticleImage obj)
        {
           
                if (obj.IsWarehouseImage == 1)

                {
                    IsReadOnlyField = true;
                    IsAcceptDisable = false;
                }
               
         
         

        }

        #endregion
    }
}
