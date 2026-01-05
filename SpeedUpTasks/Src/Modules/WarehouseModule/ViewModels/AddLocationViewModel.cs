using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.Warehouse.Views;
using System.Text.RegularExpressions;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddLocationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region TaskLog
        /// <summary>
        /// [WMS-M050-13] Add and edit warehouse locations in locations configuration[29/10/2018][adadibathina]
        /// WMS M054-14    New way to select easily the parent location u took this one [adadibathina]
        /// WMS	M056-11	Set automatic position to the warehouse locations [adadibathina]
        /// </summary>
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private WarehouseLocation newLocation;
        private WarehouseLocation tempLocation;
        private ObservableCollection<WarehouseLocation> warehouseLocationList;
        private WarehouseLocation selectedParent;
        private int position = 1;
        private string locationName;
        private string regexp;
        string error;
        private string htmlColor;
        private bool inUse;
        private sbyte? isLeaf;
        ////private bool isUpdated;
        private string parentValidationMessage;
        private int selectedDirection;
        private List<FileDetail> mapFiles;
        private FileDetail selectedMapFile;
        private bool isCoordinatesNull;

        private double? width;
        private double? height;
        private double? latitude;
        private double? longitude;
        private int maxPosition = 1;


        #endregion

        #region Properties
        public List<LookupValue> DirectionList { get; set; }

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

        public string WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader")); }
        }
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("isNew")); }
        }
        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSave")); }
        }

        public WarehouseLocation NewLocation
        {
            get { return newLocation; }
            set { newLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("NewLocation")); }
        }


        public WarehouseLocation TempLocation
        {
            get { return tempLocation; }
            set { tempLocation = value; }
        }

        public ObservableCollection<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set { warehouseLocationList = value; OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList")); }
        }

        public WarehouseLocation SelectedParent
        {
            get { return selectedParent; }
            set { selectedParent = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedParent")); }

        }

        public int Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(new PropertyChangedEventArgs("Position")); }

        }

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }
        public string HTMLColor
        {
            get { return htmlColor; }
            set { htmlColor = value; OnPropertyChanged(new PropertyChangedEventArgs("HTMLColor")); }
        }

        public bool InUse
        {
            get { return inUse; }
            set { inUse = value; OnPropertyChanged(new PropertyChangedEventArgs("InUse")); }
        }
        public sbyte? IsLeaf
        {
            get { return isLeaf; }
            set { isLeaf = value; OnPropertyChanged(new PropertyChangedEventArgs("IsLeaf")); }
        }

        public int SelectedDirection
        {
            get
            {
                return selectedDirection;
            }

            set
            {
                selectedDirection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDirection"));
            }
        }

        public List<FileDetail> MapFiles
        {
            get { return mapFiles; }
            set { mapFiles = value; }
        }

        public FileDetail SelectedMapFile
        {
            get { return selectedMapFile; }
            set
            {
                selectedMapFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMapFile"));
            }
        }

        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
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

        #endregion

        #region public Events
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

        #region  ICommand
        public ICommand ColorValueChangingCommand { get; set; }
        public ICommand LocationViewCancelButtonCommand { get; set; }
        public ICommand AddLocationAcceptCommand { get; set; }
        public ICommand ParentSelectionChangedCommand { get; set; }
        //public ICommand IsLeafChangedCommand { get; set; }
        public ICommand AddMapLocatorCommand { get; set; }

        #endregion

        #region Constructor

        public AddLocationViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddLocationViewModel()...", category: Category.Info, priority: Priority.Low);

            LocationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            AddLocationAcceptCommand = new RelayCommand(new Action<object>(AddLocationInformation));
            ParentSelectionChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(ParentAndNameValidation);
            //IsLeafChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(ParentAndNameValidation);
            AddMapLocatorCommand = new DelegateCommand<object>(new Action<object>((obj) => { AddMapLocatorCommandAction(obj); }));

            FillParents();
            FillDirectionList();
            FillMapFiles();

            GeosApplication.Instance.Logger.Log("Constructor AddLocationViewModelMethod  executed successfully", category: Category.Info, priority: Priority.Low);
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
                    // return LocationValidation.GetErrorMessage(htmlColor, HTMLColor);
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to AddLocation Information
        /// </summary>

        public void AddLocationInformation(object obj)
        {
            //if (CheckRegExpForLocationName())
            //{
                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method AddLocationInformation ()...", category: Category.Info, priority: Priority.Low);
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
                if (selectedParent.Parent != -1)// for sub parents
                    TempLocation.Parent = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).IdWarehouseLocation;
                else//for parent location
                    TempLocation.Parent = 0;

                TempLocation.Name = LocationName;
                if (HTMLColor != null)
                {
                    if (HTMLColor.StartsWith("#"))
                        HTMLColor = HTMLColor.Remove(1, 2);
                }
                TempLocation.HtmlColor = HTMLColor;
                TempLocation.FullName = LocationName;
                TempLocation.InUse = InUse;
                TempLocation.IsLead = IsLeaf != null ? IsLeaf : 0;
                TempLocation.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                if (DirectionList[SelectedDirection].IdLookupValue == 0)
                {
                    TempLocation.IdDirection = null;
                    TempLocation.Direction = null;
                }
                else
                {
                    TempLocation.IdDirection = DirectionList[SelectedDirection].IdLookupValue;
                    TempLocation.Direction = DirectionList[SelectedDirection];
                }

                if (TempLocation.Parent == 0)
                {
                    TempLocation.FullName = TempLocation.Name;
                }

                if (SelectedMapFile != null)
                    TempLocation.FileName = SelectedMapFile.FileName;

                try
                {
                    if (IsNew)
                    {
                        NewLocation = WarehouseService.AddWarehouseLocation(TempLocation);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    else
                    {
                        NewLocation = WarehouseService.UpdateWarehouseLocation(TempLocation);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    IsSave = true;
                }
                catch (FaultException<ServiceException> ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation()  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method AddLocationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }

                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod AddLocationInformation() executed successfully", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            //}
            //else
            //{
            //    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLocationViewModelMethodCheckLocationName").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //    //AddLocationView.
            //    //ParentAndNameValidation(new EditValueChangedEventArgs("", ""));
            //    error = EnableValidationAndGetError();
            //}

        }

        /// <summary>
        /// Method to initialize AddLocationViewModel for edit
        /// </summary>
        public void Init(WarehouseLocation SelectedWarehouseLocation)
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init(SelectedWarehouseLocation) ...", category: Category.Info, priority: Priority.Low);
            try
            {
                TempLocation = SelectedWarehouseLocation;
                
                if (SelectedWarehouseLocation.Parent == 0)
                    SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == -1);
                else
                    SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == SelectedWarehouseLocation.Parent);

                int mPosition = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                LocationName = SelectedWarehouseLocation.Name;
                HTMLColor = SelectedWarehouseLocation.HtmlColor;
                Position = (int)SelectedWarehouseLocation.Position;
                InUse = SelectedWarehouseLocation.InUse;
                IsLeaf = SelectedWarehouseLocation.IsLead != null ? SelectedWarehouseLocation.IsLead : 0;
                if (!string.IsNullOrEmpty(HTMLColor))
                {
                    HTMLColor = HTMLColor.Insert(1, "F");
                    HTMLColor = HTMLColor.Insert(1, "F");
                }

                if (SelectedWarehouseLocation.IdDirection != null)
                {
                    SelectedDirection = DirectionList.FindIndex(x => x.IdLookupValue == SelectedWarehouseLocation.IdDirection);
                }

                if (SelectedWarehouseLocation.FileName != null)
                {
                    SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == SelectedWarehouseLocation.FileName);
                }
                else
                {
                    SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == "---");
                }

                if (SelectedWarehouseLocation.Height != 0
                    && SelectedWarehouseLocation.Width != 0
                    && SelectedWarehouseLocation.Latitude != 0
                    && SelectedWarehouseLocation.Longitude != 0)
                {
                    IsCoordinatesNull = true;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method Init(WarehouseLocation)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init(SelectedWarehouseLocation) executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to initialize AddLocationViewModel for add
        /// </summary>
        /// <param name="WarehouseLocationList"></param>
        public void Init()
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                InUse = true;
                tempLocation = new WarehouseLocation();
                SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.FullName == "---");
                SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == "---");
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// To fill Warehouse locations 
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillParents()
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod FillParents ...", category: Category.Info, priority: Priority.Low);
            try
            {

                // [001] Changed Service method
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetIsLeafWarehouseLocations_V2034(WarehouseCommon.Instance.Selectedwarehouse) as List<WarehouseLocation>);
                WarehouseLocationList.Insert(0, new WarehouseLocation { Parent = -1, IdWarehouseLocation = -1, FullName = "---" });

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

        /// <summary>
        /// Parent and name validation
        /// </summary>
        /// <param name="obj"></param>
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
                        if (WarehouseService.IsExistWarehouseLocationName(LocationName, 0, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                        {
                            parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                        }
                    }

                    else if (WarehouseService.IsExistWarehouseLocationName(LocationName, SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                    {
                        parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                    }
                    mPosition = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                    //For add location MaxPostion incerease by one
                    MaxPosition = ++mPosition;
                    if (!(obj.NewValue == string.Empty))
                        Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationName();
                }
                else
                {
                    //if SelectedParent.IdWarehouseLocation==-1 send 0 means location full name is --- send 0 else send IdWarehouseLocation
                    mPosition = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                    MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                    //  Position = MaxPosition;

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


        //Method for Regular Expression for Location Name
        private void RegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                //RegExp = "([A-Z]{1}[0-9]{1})";          //Default value


                if (SelectedParent.IdWarehouseLocation == -1)
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
            catch(Exception ex)
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


                if (SelectedParent.IdWarehouseLocation == -1)
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

        private bool CheckRegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckRegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                bool retVal = true;
                var reg = new Regex("");
                if (SelectedParent.IdWarehouseLocation == -1)
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
                        reg = new Regex("([A-Z]{1}[1-9]{1}[0-9]{1,2})");
                        if (!reg.IsMatch(LocationName.ToString()))
                            reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                    }
                }
                if (!reg.IsMatch(LocationName.ToString()))
                    retVal = false;
                
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


        /// <summary>
        /// WMS	M051-13	Add new column Direction in locations configuration
        /// Method to fill direction list
        /// </summary>
        private void FillDirectionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(41);
                DirectionList = new List<LookupValue>();
                DirectionList = new List<LookupValue>(temptypeList);
                DirectionList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillHolidaysTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillMapFiles()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMapFiles()...", category: Category.Info, priority: Priority.Low);

                MapFiles = GeosRepositoryService.GetAllFileDetailsFromCompanyLayout();

                MapFiles.Insert(0, new FileDetail() { FileName = "---", FileByte = null });

                GeosApplication.Instance.Logger.Log("Method FillMapFiles()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddMapLocatorCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddMapLocatorCommandAction ...", category: Category.Info, priority: Priority.Low);

            MapLocatorView mapLocatorView = new MapLocatorView();
            MapLocatorViewModel mapLocatorViewModel = new MapLocatorViewModel();

            EventHandler handle = delegate { mapLocatorView.Close(); };
            mapLocatorViewModel.RequestClose += handle;
            mapLocatorView.DataContext = mapLocatorViewModel;
            mapLocatorViewModel.Init(SelectedMapFile, TempLocation);

            mapLocatorView.ShowDialog();

            if (mapLocatorViewModel.IsAcceptClick)
            {
                if (mapLocatorViewModel.height != null && mapLocatorViewModel.height != 0
                    && mapLocatorViewModel.width != null && mapLocatorViewModel.width != 0
                    && mapLocatorViewModel.latitude != null
                    && mapLocatorViewModel.longitude != null)
                {
                    IsCoordinatesNull = true;
                    TempLocation.Height = (double)mapLocatorViewModel.height;
                    TempLocation.Width = (double)mapLocatorViewModel.width;
                    TempLocation.Latitude = (double)mapLocatorViewModel.latitude;
                    TempLocation.Longitude = (double)mapLocatorViewModel.longitude;

                }
                else
                    IsCoordinatesNull = false;
            }

            GeosApplication.Instance.Logger.Log("Method AddMapLocatorCommandAction ...", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
