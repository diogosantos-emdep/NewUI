using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Export.Xl;
using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Helper;
using Microsoft.Win32;
using System.Data;
using System.IO;
using DevExpress.Xpf.Charts;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Services;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    //[001][kshinde][20/06/2022][GEOS2-244]
    public class AddNewCarOEMViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }        

        #region Service
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());        
        #endregion

        #region Declarations
        // private string windowHeader;
        public bool IsSave { get; set; }
        public bool IsUpdated { get; set; }        
        private bool isNew;
        private ObservableCollection<CarOEM> carOEMList;
        private bool isLogoImageExist;
        private ImageSource logoImage;
        //ImageSource updatedLogoImage;
        bool isSavePermissionEnabled;
        bool isImageLoadPermissionEnabled;
        List<string> carName;
        string name;
        string carOEMName;
        string prevName;
        string informationError;
        #endregion

        #region Properties

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
        public string PrevName
        {
            get
            {
                return prevName;
            }
            set
            {
                prevName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrevName"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public bool IsSavePermissionEnabled
        {
            get { return isSavePermissionEnabled; }
            set
            {
                isSavePermissionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSavePermissionEnabled"));
            }
        }
        public bool IsImageLoadPermissionEnabled
        {
            get { return isImageLoadPermissionEnabled; }
            set
            {
                isImageLoadPermissionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImageLoadPermissionEnabled"));
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (name != null)
                {
                    IsImageLoadPermissionEnabled = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string CarOEMName
        {
            get
            {
                return carOEMName;
            }
            set
            {
                carOEMName = value;
                if (carOEMName != null)
                {
                    IsImageLoadPermissionEnabled = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("CarOEMName"));
            }
        }
        public ObservableCollection<CarOEM> CarOEMList
        {
            get { return carOEMList; }
            set
            {
                carOEMList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarOEMList"));
            }
        }
        public List<string> CarName
        {
            get { return carName; }
            set
            {
                carName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CarName"));
            }
        }
        public bool IsLogoImageExist
        {
            get { return isLogoImageExist; }
            set
            {
                isLogoImageExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLogoImageExist"));
            }
        }
        public ImageSource LogoImage
        {
            get { return logoImage; }
            set
            {
                logoImage = value;
                if (logoImage != null)
                {
                    IsLogoImageExist = true;
                    IsSavePermissionEnabled = true;
                }
                else
                {
                    //LogoImage = new BitmapImage(new Uri(@"pack://application:,,,/Emdep.Geos.Modules.Crm;component/Assets/Images/ImageEditLogo.png"));
                    IsLogoImageExist = false;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("LogoImage"));
            }
        }

        //public ImageSource UpdatedLogoImage
        //{
        //    get { return updatedLogoImage; }
        //    set
        //    {
        //        updatedLogoImage = value;                               
        //        OnPropertyChanged(new PropertyChangedEventArgs("UpdatedLogoImage"));
        //    }
        //}
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

        #region Icommands
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public string WindowHeader { get; internal set; }

        #endregion

        #region Constructor
        public AddNewCarOEMViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddNewCarOEMViewModel....", category: Category.Info, priority: Priority.Low);
            AcceptCommand = new DelegateCommand<object>(AcceptCommandAction);
            CancelCommand = new DelegateCommand<object>(CancelCommandAction);
           
            GeosApplication.Instance.Logger.Log("Constructor AddNewCarOEMViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Method
        //[001][rdixit][GEOS2-244][22/06/2022]
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewCarOEMViewModel.Init...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {

                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                //LogoImage = resizeImage(LogoImage, new System.Drawing.Size(128, 128));

                CarOEMList = new ObservableCollection<CarOEM>(CRMService.GetAllCarOEM());
                CarName = new List<string>();
                foreach (CarOEM car in CarOEMList)
                {
                    CarName.Add(car.Name);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-245][25/06/2022]
        public void EditInit(CarOEM Car)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditAction..."), category: Category.Info, priority: Priority.Low);
               Init();
                if (Car != null)
                {
                    CarOEMName= Name = Car.Name;
                    if (Car.CarOEMFileBytes != null)
                    {
                        LogoImage = Car.CarOEMImage;
                        
                    }
                    PrevName = Car.Name;

                 
                     InformationError = null;
                    allowValidation = true;
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    if (string.IsNullOrEmpty(error))
                        InformationError = null;
                    else
                        InformationError = "";

                    if (error != null)
                    {
                        return;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptCommandAction(object obj)
        {
            try
            {
                InformationError = null;
                allowValidation = true;
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                if (error != null)
                {
                    return;
                }

                if (LogoImage == null)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditCAROEMImageMandatoryValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...atleast one Image should selected.", category: Category.Info, priority: Priority.Low);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    return;
                }
                //[001][rdixit][GEOS2-244][22/06/2022]
                #region Add Car OEM
                if (IsNew)
                {
                    CarOEM CarOEM = new CarOEM();
                    if (IsLogoImageExist)
                    {
                        CarOEM.Name = Name;
                        byte[] ImageBytes = ImageSourceToBytes(LogoImage);
                        if (ImageBytes.Length <= 25000)
                        {
                            CarOEM.CarOEMFileBytes = ImageBytes;
                            CarOEM.CarOEMImage = ByteArrayToBitmapImage(ImageBytes);
                        }
                        else
                        {
                            var ProfileImage = ImageSourceToBytes(LogoImage);
                            byte[] profileImageInByte = (byte[])ProfileImage;
                            CarOEM.CarOEMFileBytes = profileImageInByte;
                            CarOEM.CarOEMImage = ByteArrayToBitmapImage(profileImageInByte);
                        }
                        CarOEMList.Add(CarOEM);
                        IsSave = CRMService.InsertCarOEM(ImageBytes, Name);
                        if (IsSave)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditCAROEMAddViewCreatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            RequestClose(null, null);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditCAROEMAddViewFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            RequestClose(null, null);
                        }
                    }
                }
                #endregion
                //[001][rdixit][GEOS2-245][25/06/2022]
                #region Edit Car OEM
                else
                {
                    CarOEM CarOEM = new CarOEM();
                    byte[] ImageBytes = ImageSourceToBytes(LogoImage);
                    int CarId = CarOEMList.Where(i => i.Name.ToUpper() == PrevName.ToUpper()).Select(u => u.IdCarOEM).FirstOrDefault(); 
                    IsUpdated = CRMService.UpdateCarOEM(ImageBytes, CarId, PrevName, Name);                    
                    if (IsUpdated)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdatedEditCAROEMAddViewCreatedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdatedEditCAROEMAddViewFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                }
                #endregion

                GeosApplication.Instance.Logger.Log("Method AddEditNewCarOEMViewModel.AcceptCommandAction() executed successfully...", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddNewCarOEMViewModel.AcceptCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void CancelCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelCommandAction()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
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

                string error =
                me[BindableBase.GetPropertyName(() => Name)];



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

                string name = BindableBase.GetPropertyName(() => Name);

                if (columnName == name)
                {
                    return AddEditCarOEMValidation.GetErrorMessage(name,PrevName,CarName, Name);
                }
                return null;
            }
        }
        #endregion

    }
}
