using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Company = Emdep.Geos.Data.Common.SCM.Company;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class AddConnectorViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        //[GEOS2-8088][rdixit][29.05.2025]
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
        public Connectors Connector;
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
        private Int64 idConnector;
        private int idPictureType;
        private Int64 idConnectorImage;
        ObservableCollection<Company> companyList;
        Company company;
        #endregion

        #region Properties
        public ObservableCollection<Company> CompanyList
        {
            get { return companyList; }
            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }
        public Company Company
        {
            get { return company; }
            set { company = value; OnPropertyChanged(new PropertyChangedEventArgs("Company")); }
        }
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
        ConnectorSearch newConnector = new ConnectorSearch();
        public ConnectorSearch NewConnector
        {
            get
            {
                return newConnector;
            }

            set
            {
                newConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewConnector"));
            }
        }
      
        #endregion

        #region ICommand
        public ICommand AcceptActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  

        #endregion

        #region Constructor
        public AddConnectorViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Start Constructor AddConnectorViewModel().. ", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                AcceptActionCommand = new DelegateCommand<object>(AddConnectorAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                NewConnector = new ConnectorSearch();
                GeosApplication.Instance.Logger.Log("Executed Constructor AddConnectorViewModel().. ", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddConnectorViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Method

        public void Init()
        {                   
            FillCompany();
        }

        public void FillCompany()
        {
            try
            {
                CompanyList = new ObservableCollection<Company>(SCMService.GetAllCompanyList_V2520());
            }
            catch (Exception ex) { }
        }
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
                        case ".png":
                            IdPictureType = 0;
                            break;
                        case ".jpg":
                        case ".jpeg":
                            IdPictureType = 0;
                            break;
                        case ".bmp":
                        case ".wtg":
                            IdPictureType = 1; // Set 1 for .wtg and .bmp
                            break;
                        default:
                            IdPictureType = 0;
                            break;
                    }                  
                    ImageName = file.Name;                  
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in BrowseFileAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void AddConnectorAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddConnectorAction()...", category: Category.Info, priority: Priority.Low);
              
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ImageName"));
                PropertyChanged(this, new PropertyChangedEventArgs("Reference"));
                PropertyChanged(this, new PropertyChangedEventArgs("Company"));

                if (error != null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(Reference))
                {
                    CustomMessageBox.Show(string.Format(Application.Current.FindResource("ReferenceConnectorEmpty").ToString()),
                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                var Already_Exist_Ref = SCMService.CheckOtherRefIsValid(Reference);
                if (!string.IsNullOrEmpty(Already_Exist_Ref))
                {
                    CustomMessageBox.Show(string.Format(Application.Current.FindResource("ReferenceConnectorAlreadyExist").ToString(), Already_Exist_Ref),
                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

               
                newConnector.ReferencesList = new ObservableCollection<ConnectorReference>();
                newConnector.ConnectorsImageList = new ObservableCollection<SCMConnectorImage>();

                ConnectorReference otherRef = new ConnectorReference();
                otherRef.Reference = Reference;
                otherRef.Company = Company;
                otherRef.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                newConnector.ReferencesList.Add(otherRef);

                SCMConnectorImage image = SetImageValues();
                newConnector.ConnectorsImageList.Add(image);

                string NewRef = SCMService.AddConnector_V2680(newConnector);//[rdixit][14.10.2025][GEOS2-8895]
                IsSave = true;
                Connector = new Connectors() { Ref = NewRef };
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(string.Format(Application.Current.FindResource("ConnectorAddedSuccess").ToString(), NewRef), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddConnectorAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                IsSave = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddConnectorAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsSave = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddConnectorAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsSave = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddConnectorAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        SCMConnectorImage SetImageValues()
        {
            SCMConnectorImage image = new SCMConnectorImage();
            GeosApplication.Instance.Logger.Log("Method SetImageValues()...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (FileInBytes != null)
                {
                    if (string.IsNullOrEmpty(ImageName))
                    {
                        int index = ConnectorSavedImageName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? ConnectorSavedImageName : ConnectorSavedImageName.Substring(0, index);
                        ImageName = FileTobeSavedByName;
                    }
                    image.Position = 1;
                    image.OriginalFileName = ImageName;
                    image.SavedFileName = ImageName;
                    image.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    image.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    image.IdPictureType = IdPictureType;
                    image.ConnectorsImageInBytes = FileInBytes;
                    image.Description = Description;
                    image.isDelVisible = true;
                    image.UpdatedDate = GeosApplication.Instance.ServerDateTime.Date.Date;
                    //IsSave = true;
                }
                GeosApplication.Instance.Logger.Log("Method SetImageValues()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetImageValues() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return image;
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
                         me[BindableBase.GetPropertyName(() => ImageName)] +                    
                         me[BindableBase.GetPropertyName(() => Reference)] +                    
                         me[BindableBase.GetPropertyName(() => Company)];

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
                string company = BindableBase.GetPropertyName(() => Company); 
                string reference = BindableBase.GetPropertyName(() => Reference);

                if (columnName == imageName)
                {                  
                    return AddEditConImageValidation.GetErrorMessage(imageName, ImageName);                  
                }
                if (columnName == reference)
                {
                    return AddEditConImageValidation.GetErrorMessage(reference, Reference);
                }
                if (columnName == company)
                {
                    return AddEditConImageValidation.GetErrorMessage(company, Company);
                }
                return null;
            }
        }

        #endregion
    }
}
