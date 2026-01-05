using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
//using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.SCM.Views;
using System.Text.RegularExpressions;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class AddLocationsManagerViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        //[pramod.misal][GEOS2-5524][03.07.2024]

        #region service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        bool isSave = false;
        bool isAcceptEnable = false;
        private bool isNew;
        ObservableCollection<Data.Common.SCM.Company> companyList;
        Data.Common.SCM.Company selectedcompany;
        private Data.Common.Company selectedItem;
        private ObservableCollection<SCMLocationsManager> scmLocationList;
        private SCMLocationsManager newLocation;
        private sbyte? isLeaf;
        private string parentValidationMessage;
        private SCMLocationsManager selectedParent;
        string fullName = string.Empty;
        private string locationName;
        private int maxPosition = 1;
        private int position = 1;
        private string regexp;
        private string htmlColor;
        string error;
        private SCMLocationsManager tempLocation;
        private string title;

        private bool isSCMEditLocationManagerBtn;//[pramod.misal][GEOS2-5482][24.05.2024]


        #endregion

        #region Properties 

        public bool IsSCMEditLocationManagerBtn
        {
            get { return isSCMEditLocationManagerBtn; }
            set
            {

                isSCMEditLocationManagerBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditLocationManagerBtn"));

            }
        }
        public bool InUse
        {
            get { return inUse; }
            set { inUse = value; OnPropertyChanged(new PropertyChangedEventArgs("InUse")); }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader")); }
        }

        public Data.Common.Company SelectedPlant
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));

            }

        }

        public ObservableCollection<SCMLocationsManager> SCMLocationList
        {
            get { return scmLocationList; }
            set
            {
                scmLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SCMLocationList"));
            }
        }
        public SCMLocationsManager NewLocation
        {
            get { return newLocation; }
            set { newLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("NewLocation")); }
        }

        public sbyte? IsLeaf
        {
            get { return isLeaf; }
            set { isLeaf = value; OnPropertyChanged(new PropertyChangedEventArgs("IsLeaf")); }
        }
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("isNew")); }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SCMLocationsManager SelectedParent
        {
            get { return selectedParent; }
            set { selectedParent = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedParent")); }

        }

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }

        public int MaxPosition
        {
            get { return maxPosition; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                maxPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxPosition"));
            }
        }

        public int Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(new PropertyChangedEventArgs("Position")); }

        }
        public string RegExp
        {
            get
            {
                return regexp;
            }

            set
            {
                regexp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegExp"));
            }
        }
        public SCMLocationsManager TempLocation
        {
            get { return tempLocation; }
            set { tempLocation = value; }
        }

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSave")); }
        }

        public string HTMLColor
        {
            get { return htmlColor; }
            set { htmlColor = value; OnPropertyChanged(new PropertyChangedEventArgs("HTMLColor")); }
        }

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(new PropertyChangedEventArgs("Title")); }
        }


        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        public ICommand ParentSelectionChangedCommand { get; set; }
        #endregion

        #region Constructor
        public AddLocationsManagerViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddLocationsManagerViewModel()...", category: Category.Info, priority: Priority.Low);

                SelectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault();
                AcceptButtonCommand = new RelayCommand(new Action<object>(AddLocationInformation));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                ParentSelectionChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(ParentAndNameValidation);
                FillParents();
                //string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                //Data.Common.Company selectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault(x => x.Alias == serviceurl);
                //if (selectedPlant != null)
                //{
                //    SelectedPlant = selectedPlant;
                //    SCMCommon.Instance.SelectedSinglePlant = selectedPlant;
                //}
                //else
                //{
                //    SelectedPlant = SCMCommon.Instance.PlantList.FirstOrDefault();
                //    SCMCommon.Instance.SelectedSinglePlant = SelectedPlant;
                //}
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditLocationsManager)
                {
                    IsSCMEditLocationManagerBtn = true;
                }
                else
                {
                    IsSCMEditLocationManagerBtn = false;
                }


               

                GeosApplication.Instance.Logger.Log("Constructor AddLocationsManagerViewModel  executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddLocationsManagerViewModel()....." + ex.Message, category: Category.Exception, priority: Priority.Low);
                throw;
            }
           
        }
        #endregion
     
        #region Methods
     
        public void EditInit(SCMLocationsManager SelectedSCMLocation)
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod EditInit(SelectedSCMLocation) ...", category: Category.Info, priority: Priority.Low);
            try
            {
                TempLocation = SelectedSCMLocation;
                if (SelectedSCMLocation.Parent==0)
                {
                    SelectedParent = SCMLocationList.FirstOrDefault(x => x.IdSampleLocation == -1);
                }
                else
                {
                    SelectedParent = SCMLocationList.FirstOrDefault(x => x.IdSampleLocation == SelectedSCMLocation.Parent);
                }
                Title = SelectedSCMLocation.Title;
                LocationName = SelectedSCMLocation.Name;
                fullName = GetFullName();
                ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                //int mPosition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                int mPosition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);
                MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                HTMLColor = SelectedSCMLocation.HtmlColor;
                Position = (int)SelectedSCMLocation.Position;
                InUse = SelectedSCMLocation.InUse;
                IsLeaf = SelectedSCMLocation.IsLeaf != null ? SelectedSCMLocation.IsLeaf : 0;
                if (!string.IsNullOrEmpty(HTMLColor))
                {
                    HTMLColor = HTMLColor.Insert(1, "F");
                    HTMLColor = HTMLColor.Insert(1, "F");
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method EditInit(SCMLocation)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod EditInit(SelectedSCMLocation) executed successfully", category: Category.Info, priority: Priority.Low);
        }

        //[pramod.misal][GEOS2-5524][28.08.2024]
        public void AddLocationInformation(object obj)
        {
            if (CheckRegExpForLocationName())
            {
                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method AddLocationInformation ()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive)
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
                if (!string.IsNullOrEmpty(LocationName))
                    LocationName = LocationName.Trim();

                ParentAndNameValidation(new EditValueChangedEventArgs("", ""));
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("LocationName"));
                // PropertyChanged(this, new PropertyChangedEventArgs("HTMLColor"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedParent"));
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));

                if (!string.IsNullOrEmpty(error))
                    return;

                TempLocation.Position = Position;
                TempLocation.Title = Title;
                if (selectedParent.Parent != -1)
                    TempLocation.Parent = SCMLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).IdSampleLocation;
                else
                    TempLocation.Parent = 0;

                TempLocation.Name = LocationName;
                TempLocation.IdSite = SelectedParent.IdSite;

                if (HTMLColor != null)
                {
                    if (HTMLColor.StartsWith("#"))
                        HTMLColor = HTMLColor.Remove(1, 2);
                }
                TempLocation.HtmlColor = HTMLColor;

                if (!IsNew)
                {
                    TempLocation.FullName = SelectedParent.FullName + "-" + LocationName;
                }

                // This condtion is used to check the InUse service
                /// [001] Added "IsInUSENoSCMLocation" service method
                /// 

                TempLocation.InUse = SCMService.IsInUSESCMLocation(TempLocation.IdSampleLocation, TempLocation.IdSite);
                if (InUse)
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                    }
                    else
                    {
                        TempLocation.InUse = true;
                    }
                }
                else
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("InUsedFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        TempLocation.InUse = false;
                    }
                }

                TempLocation.InUse = InUse;
                TempLocation.IsLeaf = IsLeaf != null ? IsLeaf : 0;
                TempLocation.IdSite = SCMCommon.Instance.SelectedSinglePlant.IdCompany;
                //TempLocation.IdSite = SelectedParent.IdSite;

                if (TempLocation.Parent == 0)
                {
                    TempLocation.FullName = TempLocation.Name;
                }

                try
                {
                    if (IsNew)
                    {
                        NewLocation = SCMService.AddSCMLocation(TempLocation);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    else
                    {
                        NewLocation = SCMService.UpdateSCMLocation(TempLocation);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    IsSave = true;
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation()  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method AddLocationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod AddLocationInformation() executed successfully", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            }
            else
            {
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("LocationName"));
                PropertyChanged(this, new PropertyChangedEventArgs("HTMLColor"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedParent"));
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));

                if (!string.IsNullOrEmpty(error))
                    return;
            }
        }


        //[pramod.misal][GEOS2-5524][05.08.2024]
        private void FillParents()
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod FillParents ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //SCMService = new SCMServiceController("localhost:6699");
                //SCMLocationList = new ObservableCollection<SCMLocationsManager>(SCMService.GetIsLeafSCMLocationsManager_V2550(SCMCommon.Instance.SelectedPlant.Cast<Data.Common.Company>().ToList()) as List<SCMLocationsManager>);
                SCMLocationList = new ObservableCollection<SCMLocationsManager>(SCMService.GetIsLeafSCMLocationsManager_V2550(SCMCommon.Instance.SelectedSinglePlant));
                SCMLocationList.Insert(0, new SCMLocationsManager { Parent = -1, IdSampleLocation = -1, FullName = "---" });
                SelectedParent = SCMLocationList.FirstOrDefault(x => x.FullName == "---");
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method FillParents()  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method FillParents() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method FillParents()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod FillParents() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        public void ParentAndNameValidationold(EditValueChangedEventArgs obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                parentValidationMessage = string.Empty;
                int mPosition = 0;
                
                if (IsNew)
                {
                    if (SelectedParent.Parent == -1)
                    {
                      
                        if (SCMService.IsExistSCMLocationsManagerName(LocationName, 0, SelectedPlant.IdCompany))
                        {
                            parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                        }
                    }

                    else if (SCMService.IsExistSCMLocationsManagerName(LocationName, SelectedParent.IdSampleLocation, SelectedPlant.IdCompany))
                    {
                        parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                    }

                    fullName = GetFullName();
                    if (SelectedParent.Parent == -1 || SelectedParent.Parent == 0)
                    {
                        ///[001] Added service method GetMaxPosition
                        mPosition = (int)SCMService.GetMaxPosition(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(LocationName))
                        {
                            ///[001] Added service method GetMaxPosition
                            mPosition = (int)SCMService.GetMaxPosition(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany);
                        }
                        else
                        {
                            ///[001] Added service method GetMaxPosition_V2080
                            //mPosition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);
                            mPosition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);
                        }
                    }
                    //[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                    
                    if (mPosition == 0)
                    {
                        mPosition++;
                        MaxPosition = maxPosition;
                    }
                    MaxPosition = mPosition;
                    if (!(obj.NewValue == string.Empty))
                        Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationName();
                }
                else
                {
                   
                    mPosition = Position;
                    
                    MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                    //Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationNameEdit();
                }



                EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ParentAndNameValidation(EditValueChangedEventArgs obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                parentValidationMessage = string.Empty;
                int mPosition = 0;

                if (IsNew)
                {
                    if (SelectedParent.Parent == -1)
                    {
                       
                        if (SCMService.IsExistSCMLocationsManagerName(LocationName, 0, SCMCommon.Instance.SelectedSinglePlant.IdCompany))
                        {
                            parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                        }
                    }

                    else if (SCMService.IsExistSCMLocationsManagerName(LocationName, SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany))
                    {
                        parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                    }

                    fullName = GetFullName();
                    if (SelectedParent.Parent == -1 || SelectedParent.Parent == 0)
                    {
                        ///[001] Added service method GetMaxPosition
                        mPosition = (int)SCMService.GetMaxPosition(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(LocationName))
                        {
                            ///[001] Added service method GetMaxPosition
                            mPosition = (int)SCMService.GetMaxPosition(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany);
                        }
                        else
                        {
                            ///[001] Added service method GetMaxPosition_V2080
                            //mPosition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);
                            mPosition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany, fullName);
                        }
                    }
                    //[001] Changed service method GetMaxPosition to GetMaxPosition_V2080

                    if (mPosition == 0)
                    {
                        mPosition++;
                        MaxPosition = maxPosition;
                    }
                    MaxPosition = mPosition;
                    if (!(obj.NewValue == string.Empty))
                        Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationName();
                }
                else
                {

                    mPosition = Position;

                    MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                    //Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationNameEdit();
                }



                EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public string GetFullName()
        {
            string fullName = string.Empty;
            if (string.IsNullOrEmpty(LocationName))
            {
                if (SelectedParent.FullName.Equals("---"))
                {
                    fullName = string.Empty;
                }
                else
                {
                    fullName = SelectedParent.FullName;
                }
            }
            else
            {
                if (SelectedParent.FullName.Equals("---"))
                {
                    fullName = LocationName;
                }
                else
                {
                    fullName = SelectedParent.FullName + "-" + LocationName;
                }
            }
            return fullName;
        }
       
        private void RegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                //RegExp = "([A-Z]{1}[0-9]{1})";          //Default value


                if (SelectedParent.IdSampleLocation == -1)
                {
                    IsLeaf = 0;
                    RegExp = "([A-Z]{1}[0-9]{1})";
                }
                else
                {
                    if (SelectedParent.Parent == 0)
                    {
                        IsLeaf = 0;
                        RegExp = "([0-9]{2})";                        //"";
                    }
                    else
                    {
                        IsLeaf = 1;
                        RegExp = "([A-Z]{1}[1-9]{0,1}[0-9]{1,2})";
                    }
                }
                GeosApplication.Instance.Logger.Log("RegExpForLocationName Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  RegExpForLocationName Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RegExpForLocationNameEdit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RegExpForLocationNameEdit Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                //RegExp = "([A-Z]{1}[0-9]{1})";          //Default value


                if (SelectedParent.IdSampleLocation == -1)
                {
                    //IsLeaf = 0;
                    RegExp = "([A-Z]{1}[0-9]{1})";
                }
                else
                {
                    if (SelectedParent.Parent == 0)
                    {
                        //IsLeaf = 0;
                        RegExp = "([0-9]{2})";                        //"";
                    }
                    else
                    {
                        //IsLeaf = 1;
                        RegExp = "([A-Z]{1}[1-9]{0,1}[0-9]{1,2})";
                    }
                }
                GeosApplication.Instance.Logger.Log("RegExpForLocationNameEdit Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  RegExpForLocationNameEdit Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] [vsana][24-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// </summary>
        /// <returns></returns>
        /// SCMCommon.Instance.SelectedSinglePlant.IdCompany
        private bool CheckRegExpForLocationNameold()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckRegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                bool retVal = true;
                int mposition = 0;
                var reg = new Regex("");

                if (IsNew)
                {
                    if (SelectedParent.IdSampleLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                        if (reg.IsMatch(LocationName.ToString()))
                        {
                            fullName = GetFullName();
                            ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                            //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                            mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);

                            MaxPosition = mposition;
                            MaxPosition = MaxPosition == 0 ? ++MaxPosition : MaxPosition;
                            Position = MaxPosition;
                            retVal = true;
                        }
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                            if (reg.IsMatch(LocationName.ToString()))
                            {
                                fullName = GetFullName();
                                ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                                mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);

                                MaxPosition = mposition;
                                Position = MaxPosition;
                                retVal = true;
                            }
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                            {
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                                if (reg.IsMatch(LocationName))
                                {
                                    fullName = GetFullName();
                                    ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                    //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                                    mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SelectedPlant.IdCompany, fullName);

                                    MaxPosition = mposition;
                                    Position = MaxPosition;
                                    retVal = true;
                                }
                            }
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }
                else
                {
                    if (SelectedParent.IdSampleLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }

                GeosApplication.Instance.Logger.Log("CheckRegExpForLocationName Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
                return retVal;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckRegExpForLocationName Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return false;
            }
        }

        private bool CheckRegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckRegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                bool retVal = true;
                int mposition = 0;
                var reg = new Regex("");

                if (IsNew)
                {
                    if (SelectedParent.IdSampleLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                        if (reg.IsMatch(LocationName.ToString()))
                        {
                            fullName = GetFullName();
                            ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                            //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                            mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany, fullName);

                            MaxPosition = mposition;
                            MaxPosition = MaxPosition == 0 ? ++MaxPosition : MaxPosition;
                            Position = MaxPosition;
                            retVal = true;
                        }
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                            if (reg.IsMatch(LocationName.ToString()))
                            {
                                fullName = GetFullName();
                                ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                                mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany, fullName);

                                MaxPosition = mposition;
                                Position = MaxPosition;
                                retVal = true;
                            }
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                            {
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                                if (reg.IsMatch(LocationName))
                                {
                                    fullName = GetFullName();
                                    ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                    //mposition = (int)SCMService.GetMaxPosition_V2540(SelectedParent.IdLocationByConnector == -1 ? 0 : SelectedParent.IdLocationByConnector, SelectedPlant.IdCompany, fullName);
                                    mposition = (int)SCMService.GetMaxPosition_V2550(SelectedParent.IdSampleLocation == -1 ? 0 : SelectedParent.IdSampleLocation, SCMCommon.Instance.SelectedSinglePlant.IdCompany, fullName);

                                    MaxPosition = mposition;
                                    Position = MaxPosition;
                                    retVal = true;
                                }
                            }
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }
                else
                {
                    if (SelectedParent.IdSampleLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }

                GeosApplication.Instance.Logger.Log("CheckRegExpForLocationName Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
                return retVal;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckRegExpForLocationName Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return false;
            }
        }


        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Init()
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init(SelectedSCMLocation) ...", category: Category.Info, priority: Priority.Low);
            try
            {
                InUse = true;
                tempLocation = new SCMLocationsManager();
                //FillParents();
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModelMethod Init Method Init(SCMLocation)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Validation
        bool allowValidation = false;
        private bool inUse;

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
                IDataErrorInfo DataError = (IDataErrorInfo)this;
                string error = DataError[BindableBase.GetPropertyName(() => LocationName)]
                    //+ DataError[BindableBase.GetPropertyName(() => HTMLColor)]
                    + DataError[BindableBase.GetPropertyName(() => SelectedParent)]
                    + DataError[BindableBase.GetPropertyName(() => LocationName)]
                    + DataError[BindableBase.GetPropertyName(() => Position)];
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
                string position = BindableBase.GetPropertyName(() => Position);
                if (columnName == position)
                {

                    if (Position > MaxPosition || Position == 0)
                    {
                        return LocationValidation.GetErrorMessage(position, this);
                    }
                }
                string locationName = BindableBase.GetPropertyName(() => LocationName);
                if (columnName == locationName)
                {
                    //return LocationValidation.GetErrorMessage(locationName, LocationName);
                    if (string.IsNullOrEmpty(LocationName))
                        return LocationValidation.GetErrorMessage(locationName, LocationName);
                    else
                    {
                        bool val = CheckRegExpForLocationName();
                        if (!val)
                            return "Not Valid Location name.";
                    }
                }


                string parent = BindableBase.GetPropertyName(() => SelectedParent);
                if (columnName == parent)
                {
                    return parentValidationMessage;
                }
                string locatiname = BindableBase.GetPropertyName(() => LocationName);
                if (columnName == htmlColor)
                {
                    //return LocationValidation.GetErrorMessage(htmlColor, HTMLColor);
                }

                return null;
            }
        }




        #endregion
    }
}

   








