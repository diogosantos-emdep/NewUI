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
    class AddImageInOptionWayDetectionSparePartViewModel : ViewModelBase, INotifyPropertyChanged
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
        private string imageName;
        private string description;
        private DateTime updatedDate;

        string FileTobeSavedByName = "";
        private bool isCheckedDefaultImage;

        private ObservableCollection<DetectionImage> optionWayDetectionSparePartImagesList;
        private DetectionImage selectedOptionWayDetectionSparePartImage;
        private string optionWayDetectionSparePartSavedImageName;
        private uint idImage;
        private DetectionImage oldDefaultImage;
        private bool isImageReadOnly = false;
        private DetectionImage selectedImage;


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

        public ObservableCollection<DetectionImage> OptionWayDetectionSparePartImagesList
        {
            get
            {
                return optionWayDetectionSparePartImagesList;
            }

            set
            {
                optionWayDetectionSparePartImagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartImagesList"));

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

        public DetectionImage SelectedOptionWayDetectionSparePartImage
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

        public string OptionWayDetectionSparePartSavedImageName
        {
            get
            {
                return optionWayDetectionSparePartSavedImageName;
            }
            set
            {
                optionWayDetectionSparePartSavedImageName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionWayDetectionSparePartSavedImageName"));
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

        public DetectionImage OldDefaultImage
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

        public DetectionImage SelectedImage
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


        #endregion

        #region Constructor

        public AddImageInOptionWayDetectionSparePartViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddImageInOptionWayDetectionSparePartViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptImageActionCommand = new DelegateCommand<object>(AddImageAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);

                GeosApplication.Instance.Logger.Log("Constructor AddImageInOptionWayDetectionSparePartViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddImageInOptionWayDetectionSparePartViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    Description = Description.TrimEnd(trimChars);
                    Description = Description.TrimStart(trimChars);
                }

                if (IsNew)
                {
                    SelectedOptionWayDetectionSparePartImage = new DetectionImage();
                    if (IsCheckedDefaultImage && FileInBytes != null)
                    {
                        if (OptionWayDetectionSparePartImagesList.Count > 0)
                        {
                            OldDefaultImage = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).FirstOrDefault();
                            OldDefaultImage.Position = (ulong)OptionWayDetectionSparePartImagesList.Count + 1;
                            SelectedOptionWayDetectionSparePartImage.Position = 1;
                        }
                        else
                        {
                            SelectedOptionWayDetectionSparePartImage.Position = 1;
                        }
                    }
                    else
                    {
                        SelectedOptionWayDetectionSparePartImage.Position = (ulong)OptionWayDetectionSparePartImagesList.Count + 1;
                    }

                    if (FileInBytes != null)
                    {
                        if (string.IsNullOrEmpty(ImageName))
                        {
                            int index = OptionWayDetectionSparePartSavedImageName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? OptionWayDetectionSparePartSavedImageName : OptionWayDetectionSparePartSavedImageName.Substring(0, index);
                            ImageName = FileTobeSavedByName;
                        }
                        SelectedOptionWayDetectionSparePartImage.OriginalFileName = ImageName;
                        SelectedOptionWayDetectionSparePartImage.SavedFileName = OptionWayDetectionSparePartSavedImageName;
                        SelectedOptionWayDetectionSparePartImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedOptionWayDetectionSparePartImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        SelectedOptionWayDetectionSparePartImage.IdDetection = 1;
                        SelectedOptionWayDetectionSparePartImage.DetectionImageInBytes = FileInBytes;
                        SelectedOptionWayDetectionSparePartImage.Description = Description;
                        SelectedOptionWayDetectionSparePartImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
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
                            if (OptionWayDetectionSparePartImagesList.Count > 0)
                            {
                                OldDefaultImage = OptionWayDetectionSparePartImagesList.Where(a => a.Position == 1).FirstOrDefault();
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
                    SelectedImage.SavedFileName = OptionWayDetectionSparePartSavedImageName;
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = OptionWayDetectionSparePartSavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? OptionWayDetectionSparePartSavedImageName : OptionWayDetectionSparePartSavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    SelectedImage.OriginalFileName = ImageName;
                    SelectedImage.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedImage.IdDetection = 1;
                    SelectedImage.DetectionImageInBytes = FileInBytes;
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
                    OptionWayDetectionSparePartSavedImageName = file.Name;

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

        public void EditInit(DetectionImage OptionWayDetectionSparePartImage)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdImage = OptionWayDetectionSparePartImage.IdDetectionImage;
                ImageName = OptionWayDetectionSparePartImage.OriginalFileName;
                Description = OptionWayDetectionSparePartImage.Description;
                OptionWayDetectionSparePartSavedImageName = OptionWayDetectionSparePartImage.SavedFileName;
                FileInBytes = OptionWayDetectionSparePartImage.DetectionImageInBytes;
                UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                OptionWayDetectionSparePartImagesList = new ObservableCollection<DetectionImage>();
                SelectedImage = OptionWayDetectionSparePartImage;
                OptionWayDetectionSparePartImagesList.Add(SelectedImage);

                if (OptionWayDetectionSparePartImage.Position == 1)
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
    }
}
