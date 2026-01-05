using DevExpress.Mvvm;
using DevExpress.Xpf;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    /// <summary>
    /// //[pramod.misal][GEOS2-7989][17-09-2025]https://helpdesk.emdep.com/browse/GEOS2-7989
    /// </summary>
    public class AddEditTransferViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private List<LookupValue> allLocations;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private bool isDuplicat;

        private Visibility isDuplicateBtnVisible=Visibility.Hidden;
        private List<LookupValue> destinationList;
        private List<LookupValue> allOptions;

        
        private List<LookupValue> originList;     
        private List<LookupValue> transportList;
        
        private LookupValue selectedTransport;

        private LookupValue selecteddestination;
        private LookupValue selectedOrigin;

        private List<LookupValue> providerList;
        private LookupValue selectedProvider;

        private string contactperson;
        private string contactNumber;
        private DateTime? transferDate;
        private string estimatedDuration;
        private string remarks;
        private string error = string.Empty;
        public Employee_trips_transfers selectedTransfer;
        public Employee_trips_transfers duplicatedTransfer;
        private ObservableCollection<Employee_trips_transfers> transfersList;

        private UInt32 idEmployeeTripNew;
        private int idEmployeeTripsTransfers;
        private DateTime fromdatec;
        private DateTime todatec;

        private TimeSpan transferTime;

        #endregion

        #region Properties

        public int IdEmployeeTripsTransfers
        {
            get { return idEmployeeTripsTransfers; }
            set
            {
                idEmployeeTripsTransfers = value;
                
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployeeTripsTransfers"));
            }
        }

        public UInt32 IdEmployeeTripNew
        {
            get
            {
                return idEmployeeTripNew;
            }
            set
            {
                idEmployeeTripNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdEmployeeTripNew"));
            }
        }
        public TimeSpan TransferTime
        {
            get
            {
                return transferTime;
            }

            set
            {
                transferTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransferTime"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.ArrivalDateHours = TransferTime;
            }
        }

        public Employee_trips_transfers SelectedTransfer
        {
            get
            {
                return selectedTransfer;
            }

            set
            {
                selectedTransfer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTransfer"));
            }
        }

        public ObservableCollection<Employee_trips_transfers> TransfersList
        {
            get { return transfersList; }

            set
            {
                transfersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransfersList"));
            }
        }

        public Employee_trips_transfers DuplicatedTransfer
        {
            get
            {
                return duplicatedTransfer;
            }

            set
            {
                duplicatedTransfer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DuplicatedTransfer"));

            }
        }

        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.Remarks = Remarks;


            }
        }

        public string EstimatedDuration
        {
            get { return estimatedDuration; }
            set
            {
                estimatedDuration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EstimatedDuration"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.EstimatedDuration = Convert.ToInt32(EstimatedDuration);


            }
        }

        public string ContactNumber
        {
            get { return contactNumber; }
            set
            {
                contactNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactNumber"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.ContactPersonNumber = ContactNumber;


            }
        }

        public DateTime? TransferDate
        {
            get { return transferDate; }
            set
            {
                //transferDate = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("TransferDate"));
                //if (DuplicatedTransfer == null)
                //{
                //    DuplicatedTransfer = new Employee_trips_transfers();
                //}
                //DuplicatedTransfer.FromDate = transferDate;
                transferDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransferDate"));

                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }

                if (transferDate.HasValue)
                {
                    DuplicatedTransfer.FromDate = transferDate.Value.Date;
                }
                else
                {
                    DuplicatedTransfer.FromDate = DateTime.MinValue; // or another default
                }

            }
        }
        public DateTime FromDateC
        {
            get { return fromdatec; }
            set
            {
                fromdatec = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromDateC"));
            }
        }

        public DateTime TodateC
        {
            get { return todatec; }
            set
            {
                todatec = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TodateC"));
            }
        }



        public string Contactperson
        {
            get { return contactperson; }
            set
            {
                contactperson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Contactperson"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.ContactPerson = Contactperson;

            }
        }

        public LookupValue SelectedTransport
        {
            get { return selectedTransport; }
            set
            {
                selectedTransport = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTransport"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.Transport_Method = SelectedTransport.Value;

            }
        }

        public LookupValue Selecteddestination
        {
            get => selecteddestination;
            set
            {
                if (selecteddestination != value)
                {
                    selecteddestination = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Selecteddestination"));

                    if (DuplicatedTransfer == null)
                        DuplicatedTransfer = new Employee_trips_transfers();

                    DuplicatedTransfer.Destination = Selecteddestination?.Value;
                    UpdateFilteredLists();
                }
            }
        }

        public LookupValue SelectedOrigin
        {
            get => selectedOrigin;
            set
            {
                if (selectedOrigin != value)
                {
                    selectedOrigin = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrigin"));

                    if (DuplicatedTransfer == null)
                        DuplicatedTransfer = new Employee_trips_transfers();

                    DuplicatedTransfer.Origin = SelectedOrigin?.Value;
                    UpdateFilteredLists();
                }
            }
        }

        public LookupValue SelectedProvider
        {
            get { return selectedProvider; }
            set
            {
                selectedProvider = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProvider"));
                if (DuplicatedTransfer == null)
                {
                    DuplicatedTransfer = new Employee_trips_transfers();
                }
                DuplicatedTransfer.Provider = SelectedProvider.Value;

            }
        }

        public List<LookupValue> DestinationList
        {
            get { return destinationList; }
            set
            {
                destinationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DestinationList"));
            }
        }

        public List<LookupValue> AllOptions
        {
            get { return allOptions; }
            set
            {
                allOptions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllOptions "));
            }
        }


        public List<LookupValue> OriginList
        {
            get { return originList; }
            set
            {
                originList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginList"));
            }
        }

        public List<LookupValue> TransportList
        {
            get { return transportList; }
            set
            {
                transportList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TransportList"));
            }
        }

        


        public List<LookupValue> ProviderList
        {
            get { return providerList; }
            set
            {
                providerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProviderList"));
            }
        }
        

        public Visibility IsDuplicateBtnVisible
        {
            get
            {
                return isDuplicateBtnVisible;
            }

            set
            {
                isDuplicateBtnVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicateBtnVisible"));
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

        public bool IsDuplicat
        {
            get
            {
                return isDuplicat;
            }

            set
            {
                isDuplicat = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDuplicat"));
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
        private EmployeeTrips editEmployeeTrip;
        public EmployeeTrips EditEmployeeTrips
        {
            get { return editEmployeeTrip; }
            set
            {
                editEmployeeTrip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditEmployeeTrips"));
            }
        }
        #endregion

        #region ICommand
        public ICommand AddEditTransferViewAcceptButton { get; set; }
        public ICommand CancelButtonCommand { get; set; }      
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AddEditTransferViewDuplicateCommand { get; set; }




        



        #endregion

        #region Constructor
        public AddEditTransferViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditTransferViewModel ...", category: Category.Info, priority: Priority.Low);
                AddEditTransferViewAcceptButton = new DelegateCommand<object>(AddEditTransferViewAcceptButtonAction);
                AddEditTransferViewDuplicateCommand = new DelegateCommand<object>(AddEditTransferViewDuplicateCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                TransferDate = DateTime.Today;
                GeosApplication.Instance.Logger.Log("Constructor AddEditTransferViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditTransferViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method

        //private void RefreshDestinationList()
        //{
        //    if (AllOptions == null) return;

        //    DestinationList = AllOptions
        //        .Where(x => SelectedOrigin == null || x.Value != SelectedOrigin.Value)
        //        .ToList();

        //    OnPropertyChanged(new PropertyChangedEventArgs("DestinationList"));

        //    // Reset destination if same as origin
        //    if (Selecteddestination != null && Selecteddestination.Value == SelectedOrigin?.Value)
        //        Selecteddestination = null;
        //}

        //private void RefreshOriginList()
        //{
        //    if (AllOptions == null) return;

        //    OriginList = AllOptions
        //        .Where(x => Selecteddestination == null || x.Value != Selecteddestination.Value)
        //        .ToList();

        //    OnPropertyChanged(new PropertyChangedEventArgs("OriginList"));

        //    // Reset origin if same as destination
        //    if (SelectedOrigin != null && SelectedOrigin.Value == Selecteddestination?.Value)
        //        SelectedOrigin = null;
        //}



        private void AddEditTransferViewAcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditTransferViewAcceptButtonAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrigin"));
                PropertyChanged(this, new PropertyChangedEventArgs("Selecteddestination"));
                PropertyChanged(this, new PropertyChangedEventArgs("TransferDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("TransferTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedTransport"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProvider"));
                PropertyChanged(this, new PropertyChangedEventArgs("Contactperson"));
                PropertyChanged(this, new PropertyChangedEventArgs("ContactNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("EstimatedDuration"));
                //PropertyChanged(this, new PropertyChangedEventArgs("Remarks"));
                if (error != null)
                {
                    return;
                }
                if (IsNew)
                {
                    SelectedTransfer = new Employee_trips_transfers();
                    //Origin
                    SelectedTransfer.Origin = SelectedOrigin.Value;
                    SelectedTransfer.IdEmployeeTrip =Convert.ToInt32(IdEmployeeTripNew);

                    
                    SelectedTransfer.IdTransferOriginType = SelectedOrigin.IdLookupValue;
                    //Destination
                    SelectedTransfer.Destination = Selecteddestination.Value;
                    SelectedTransfer.IdTransferDestinationType = Selecteddestination.IdLookupValue;
                    //Date
                    //SelectedTransfer.FromDate = TransferDate.Date;
                    if (TransferDate.HasValue)
                    {
                        SelectedTransfer.FromDate = TransferDate.Value.Date + TransferTime;
                    }
                    else
                    {
                        // Handle null case appropriately. For example:
                        SelectedTransfer.FromDate = DateTime.MinValue;
                    }

                    SelectedTransfer.ArrivalDateHours = TransferTime;
                    //Transport 
                    SelectedTransfer.TransportMethodId = SelectedTransport.IdLookupValue;
                    SelectedTransfer.Transport_Method = SelectedTransport.Value;
                    //Provider
                    SelectedTransfer.Provider = SelectedProvider.Value;
                    SelectedTransfer.ProviderId = SelectedProvider.IdLookupValue;
                    //contact person
                    SelectedTransfer.ContactPerson = Contactperson;
                    //Contact Number
                    SelectedTransfer.ContactPersonNumber = ContactNumber;
                    //Estimated Duration
                    SelectedTransfer.EstimatedDuration = Convert.ToInt32(EstimatedDuration);
                    //Remark
                    SelectedTransfer.Remarks = Remarks;
                    SelectedTransfer.TransactionOperation = ModelBase.TransactionOperations.Modify;

                    SelectedTransfer.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    
                    IsSave = true;
                }
                else
                {
                    SelectedTransfer = new Employee_trips_transfers();

                    SelectedTransfer = new Employee_trips_transfers();
                    SelectedTransfer.IdEmployeeTrip = Convert.ToInt32(IdEmployeeTripNew);
                    SelectedTransfer.IdEmployeeTripsTransfers = IdEmployeeTripsTransfers;

                    //Origin
                    SelectedTransfer.Origin = SelectedOrigin.Value;
                    SelectedTransfer.IdTransferOriginType = SelectedOrigin.IdLookupValue;
                    //Destination
                    SelectedTransfer.Destination = Selecteddestination.Value;
                    SelectedTransfer.IdTransferDestinationType = Selecteddestination.IdLookupValue;
                    //Date
                    //SelectedTransfer.FromDate = TransferDate.Date;
                    if (TransferDate.HasValue)
                    {
                        SelectedTransfer.FromDate = TransferDate.Value.Date + TransferTime;
                    }
                    else
                    {
                        // Handle null case appropriately. For example:
                        SelectedTransfer.FromDate = DateTime.MinValue;
                    }

                    SelectedTransfer.ArrivalDateHours = TransferTime;
                    //Transport 
                    SelectedTransfer.TransportMethodId = SelectedTransport.IdLookupValue;
                    SelectedTransfer.Transport_Method = SelectedTransport.Value;
                    //Provider
                    SelectedTransfer.Provider = SelectedProvider.Value;
                    SelectedTransfer.ProviderId = SelectedProvider.IdLookupValue;
                    //contact person
                    SelectedTransfer.ContactPerson = Contactperson;
                    //Contact Number
                    SelectedTransfer.ContactPersonNumber = ContactNumber;
                    //Estimated Duration
                    SelectedTransfer.EstimatedDuration = Convert.ToInt32(EstimatedDuration);
                    //Remark
                    SelectedTransfer.Remarks = Remarks;
                    SelectedTransfer.TransactionOperation = ModelBase.TransactionOperations.Update;
                    SelectedTransfer.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    IsSave = true;
                }
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AddEditTransferViewAcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditTransferViewAcceptButtonAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddEditTransferViewDuplicateCommandAction(object obj)
        {
            try
            {
                
                GeosApplication.Instance.Logger.Log("Method AddEditTransferViewDuplicateCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DuplicateTransferTrip"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsDuplicat = true;
                    RequestClose(null, null);
                    AddEditTransferView addEditTransferView = new AddEditTransferView();
                    AddEditTransferViewModel addEditTransferViewModel = new AddEditTransferViewModel();
                    EventHandler handle = delegate { addEditTransferView.Close(); };
                    addEditTransferViewModel.RequestClose += handle;
                    addEditTransferViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddTransfer").ToString();
                    addEditTransferViewModel.IsNew = true;
                    addEditTransferViewModel.IsDuplicat = true;
                    addEditTransferViewModel.AddInit(DuplicatedTransfer,IdEmployeeTripNew, EditEmployeeTrips);
                    addEditTransferView.DataContext = addEditTransferViewModel;
                    addEditTransferView.ShowDialog();
                    if (addEditTransferViewModel.IsDuplicat)
                    {
                        if (addEditTransferViewModel.SelectedTransfer != null)
                        {
                            if (TransfersList == null)
                            {
                                TransfersList = new ObservableCollection<Employee_trips_transfers>();
                            }
                            if (HrmCommon.Instance.TransfersList==null || HrmCommon.Instance.TransfersList.Count==0)
                            {
                                HrmCommon.Instance.TransfersList = new ObservableCollection<Employee_trips_transfers>();
                            }
                            HrmCommon.Instance.TransfersList.Add(addEditTransferViewModel.SelectedTransfer);
                        }

                    }

                    //if (addEditTransferViewModel.IsSave)
                    //{
                    //    if (addEditTransferViewModel.SelectedTransfer != null)
                    //    {
                    //        if (TransfersList == null)
                    //        {
                    //            TransfersList = new ObservableCollection<Employee_trips_transfers>();
                    //        }
                    //        TransfersList.Add(addEditTransferViewModel.SelectedTransfer);
                    //    }
                    //}


                }

                
                GeosApplication.Instance.Logger.Log("Method AddEditTransferViewDuplicateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditTransferViewDuplicateCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

     
        public void AddInit(Employee_trips_transfers DuplicatedTransfer, UInt32 IdEmployeeTrip, EmployeeTrips EditEmployeeTrips)
        {
            try
            {
                IdEmployeeTripNew = IdEmployeeTrip;
                DuplicatedTransfer = DuplicatedTransfer;
                GeosApplication.Instance.Logger.Log("Method AddInit()...", category: Category.Info, priority: Priority.Low);
                IsDuplicateBtnVisible = Visibility.Hidden;
                if (IsNew)
                {
                    FillDestinationList(DuplicatedTransfer);
                    FillOriginList(DuplicatedTransfer);                    
                    FillProviderList(DuplicatedTransfer);
                    FillTransportList(DuplicatedTransfer);
                    
                }
                if (IsDuplicat)
                {
                    if (DuplicatedTransfer != null)
                    {
                        TransferDate = DuplicatedTransfer.FromDate;
                        TransferTime = DuplicatedTransfer.ArrivalDateHours;
                        Contactperson = DuplicatedTransfer.ContactPerson;
                        ContactNumber = DuplicatedTransfer.ContactPersonNumber;
                        EstimatedDuration = Convert.ToString(DuplicatedTransfer.EstimatedDuration);
                        Remarks = DuplicatedTransfer.Remarks;
                    }

                }
                FromDateC = EditEmployeeTrips.FromDate;
                TodateC = EditEmployeeTrips.ToDate;

                GeosApplication.Instance.Logger.Log("Method AddInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(Employee_trips_transfers SelectedTrasfer, EmployeeTrips EditEmployeeTrips12)
        {
            try
            {
                EditEmployeeTrips = EditEmployeeTrips12;
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                IsDuplicateBtnVisible = Visibility.Visible;
                DuplicatedTransfer = new Employee_trips_transfers();
                if (SelectedTrasfer!= null)
                {
                    TransferDate = SelectedTrasfer.FromDate;
                    TransferTime= SelectedTrasfer.ArrivalDateHours;
                    Contactperson = SelectedTrasfer.ContactPerson;
                    ContactNumber = SelectedTrasfer.ContactPersonNumber;
                    EstimatedDuration= Convert.ToString(SelectedTrasfer.EstimatedDuration);
                    Remarks = SelectedTrasfer.Remarks;
                    IdEmployeeTripNew =Convert.ToUInt32(SelectedTrasfer.IdEmployeeTrip);
                    IdEmployeeTripsTransfers = SelectedTrasfer.IdEmployeeTripsTransfers;
                }
                FromDateC = EditEmployeeTrips.FromDate;
                TodateC = EditEmployeeTrips.ToDate;
                FillDestinationList(SelectedTrasfer);
                FillOriginList(SelectedTrasfer);
                FillProviderList(SelectedTrasfer);
                FillTransportList(SelectedTrasfer);

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillOriginList(Employee_trips_transfers SelectedTrasfer)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                //IList<LookupValue> tempTypeList = HrmService.GetLookupValues(188);
                //OriginList = new List<LookupValue>(tempTypeList);
                //if (IsNew == true && IsDuplicat == false)
                //{
                   
                //    SelectedOrigin = OriginList.FirstOrDefault();
                //}
                //else
                //{
                    
                //    SelectedOrigin = OriginList.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);

                //}

                //if (IsDuplicat)
                //{
                    
                //    SelectedOrigin = OriginList.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);
                //}

                //GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDestinationList(Employee_trips_transfers SelectedTrasfer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                //IList<LookupValue> tempTypeList = HrmService.GetLookupValues(188);
                //DestinationList = new List<LookupValue>(tempTypeList);
                //if (IsNew==true && IsDuplicat==false)
                //{
                //    Selecteddestination = DestinationList.FirstOrDefault();
                //    //SelectedOrigin = DestinationList.FirstOrDefault();
                //}
                //else
                //{
                //    Selecteddestination = DestinationList.FirstOrDefault(x => x.Value == SelectedTrasfer.Destination);
                //    //SelectedOrigin = DestinationList.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);

                //}

                //if (IsDuplicat)
                //{
                //    Selecteddestination = DestinationList.FirstOrDefault(x => x.Value == SelectedTrasfer.Destination);
                //    //SelectedOrigin = DestinationList.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);
                //}
                // Fetch the complete list of location values
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(188);
                allLocations = tempTypeList.ToList(); // master list

                // Set initial selection based on state
                if (IsNew && !IsDuplicat)
                {
                    SelectedOrigin = allLocations.FirstOrDefault();
                    Selecteddestination = allLocations.FirstOrDefault(x => x.Value != SelectedOrigin?.Value);
                }
                else
                {
                    SelectedOrigin = allLocations.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);
                    Selecteddestination = allLocations.FirstOrDefault(x => x.Value == SelectedTrasfer.Destination);
                }

                if (IsDuplicat)
                {
                    SelectedOrigin = allLocations.FirstOrDefault(x => x.Value == SelectedTrasfer.Origin);
                    Selecteddestination = allLocations.FirstOrDefault(x => x.Value == SelectedTrasfer.Destination);
                }

                // Filter origin and destination lists to ensure uniqueness
                UpdateFilteredLists();

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UpdateFilteredLists()
        {
            if (allLocations == null) return;

            OriginList = allLocations
                .Where(x => x.Value != Selecteddestination?.Value)
                .ToList();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(OriginList)));

            DestinationList = allLocations
                .Where(x => x.Value != SelectedOrigin?.Value)
                .ToList();
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(DestinationList)));
        }

        private void FillTransportList(Employee_trips_transfers SelectedTrasfer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(163);
                AllOptions = new List<LookupValue>(tempTypeList);
                TransportList = new List<LookupValue>(tempTypeList);
                if (IsNew == true && IsDuplicat == false)
                {
                    SelectedTransport = TransportList.FirstOrDefault();
                }
                else
                {
                    SelectedTransport = TransportList.FirstOrDefault(x => x.Value == SelectedTrasfer.Transport_Method);                   

                }
                if (IsDuplicat)
                {
                    SelectedTransport = TransportList.FirstOrDefault(x => x.Value == SelectedTrasfer.Transport_Method);
                }

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillProviderList(Employee_trips_transfers SelectedTrasfer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(189);
                ProviderList = new List<LookupValue>(tempTypeList);
                if (IsNew == true && IsDuplicat == false)
                {
                    SelectedProvider = ProviderList.FirstOrDefault();
                }
                else
                {
                    SelectedProvider = ProviderList.FirstOrDefault(x => x.Value == SelectedTrasfer.Provider);
                    
                }
                if (IsDuplicat)
                {
                    SelectedProvider = ProviderList.FirstOrDefault(x => x.Value == SelectedTrasfer.Provider);
                }

                GeosApplication.Instance.Logger.Log("Method FillMainTransport()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMainTransport() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillMainTransport() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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
                   me[BindableBase.GetPropertyName(() => SelectedOrigin)] +
                   me[BindableBase.GetPropertyName(() => Selecteddestination)] +
                   me[BindableBase.GetPropertyName(() => TransferDate)] +
                   me[BindableBase.GetPropertyName(() => TransferTime)] +
                   me[BindableBase.GetPropertyName(() => SelectedTransport)] +
                   me[BindableBase.GetPropertyName(() => SelectedProvider)] +
                   me[BindableBase.GetPropertyName(() => Contactperson)] +
                   me[BindableBase.GetPropertyName(() => ContactNumber)] +
                   me[BindableBase.GetPropertyName(() => EstimatedDuration)];             
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

                string Origin = BindableBase.GetPropertyName(() => SelectedOrigin);
                string destination = BindableBase.GetPropertyName(() => Selecteddestination);
                string Date = BindableBase.GetPropertyName(() => TransferDate);               
                string Time = BindableBase.GetPropertyName(() => TransferTime);
                string Transport = BindableBase.GetPropertyName(() => SelectedTransport);
                string Provider = BindableBase.GetPropertyName(() => SelectedProvider);
                string Contact = BindableBase.GetPropertyName(() => Contactperson);
                string ContactNum = BindableBase.GetPropertyName(() => ContactNumber);
                string EstimatedDur = BindableBase.GetPropertyName(() => EstimatedDuration);



                if (columnName == Origin)
                {
                    return TripTransferValidation.GetErrorMessage(Origin, SelectedOrigin);
                }
                else if (columnName == destination)
                    return TripTransferValidation.GetErrorMessage(destination, Selecteddestination);

                else if (columnName == Date)
                {
                    // First, validate that TransferDate is not null or invalid
                    var error = TripTransferValidation.GetErrorMessage(Date, TransferDate);
                    if (!string.IsNullOrWhiteSpace(error))
                        return error;

                    // ✅ Now do the range check
                    if (TransferDate.HasValue)
                    {
                        DateTime inputDate = TransferDate.Value.Date;
                        if (inputDate < FromDateC.Date || inputDate > TodateC.Date)
                        {
                            return "Transfer Date must be within the trip date range.";
                        }
                    }
                }



                else if (columnName == Time)
                {
                    // Validate that TransferTime is not null or invalid
                    var error = TripTransferValidation.GetErrorMessage(Time, TransferTime);
                    if (!string.IsNullOrWhiteSpace(error))
                        return error;

                    // ✅ Additional validation: Check if the time is negative
                    if (TransferTime.Ticks < 0)
                    {
                        return "TransferTime cannot be negative.";
                    }
                    if (TransferTime.Ticks == 0)
                    {
                        return "TransferTime Should be greater than 0.";
                    }
                }


                else if (columnName == Transport)
                    return TripTransferValidation.GetErrorMessage(Transport, SelectedTransport);

                else if (columnName == Provider)
                    return TripTransferValidation.GetErrorMessage(Provider, SelectedProvider);

                else if (columnName == Contact)
                    return TripTransferValidation.GetErrorMessage(Contact, Contactperson);

                else if (columnName == ContactNum)
                    return TripTransferValidation.GetErrorMessage(ContactNum, ContactNumber);

                else if (columnName == EstimatedDur)
                    return TripTransferValidation.GetErrorMessage(EstimatedDur, EstimatedDuration);

                return null;


           
            }
        }
        #endregion


    }
}
