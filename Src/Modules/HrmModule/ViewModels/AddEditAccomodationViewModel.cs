using DevExpress.Map.Kml.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

// [shweta.thube][GEOS2-7989][18-09-2025] https://helpdesk.emdep.com/browse/GEOS2-7989
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEditAccommodationViewModel : INotifyPropertyChanged , IDataErrorInfo
    {
        #region Service

        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddEditAccommodationAcceptButtonCommand { get; set; }
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

        #region Declaration
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private List<LookupValue> accommodationTypeList;
        private LookupValue selectedAccommodationType;
        private Visibility isVisibleName;
        private Visibility isVisibleApartment;
        private ObservableCollection<TripAssets> accommodationAssetList;
        private DateTime? accommodationCheckInDate;
        private TimeSpan accommodationCheckInTime;
        private string error = string.Empty;
        private DateTime? accommodationCheckOutDate;
        private DateTime? fromdate;
        private DateTime? todate;
        private TimeSpan accommodationCheckOutTime;
        private string accommodationAddress;
        private string accommodationCoordinates;
        private TripAssets selectedAccommodationApartment;
        private string accommodationName;
        private EmployeeTripsAccommodations selectedAccommodations;
        private string remarks;
        private UInt32 idEmployeeTripNew;
        private int durationDays;
        private UInt32 idAccommodation;
        private EmployeeTrips employeeDetails;
        private bool ischeckoutdateenabled;
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

        public List<LookupValue> AccommodationTypeList
        {
            get { return accommodationTypeList; }
            set
            {
                accommodationTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationTypeList"));
            }
        }
        public LookupValue SelectedAccommodationType
        {
            get { return selectedAccommodationType; }
            set
            {
                selectedAccommodationType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccommodationType"));
                if (selectedAccommodationType?.Value == "Apartment")
                {
                    IsVisibleApartment = Visibility.Visible;
                }
                else
                {
                    IsVisibleApartment = Visibility.Collapsed;
                }
                if (selectedAccommodationType?.Value == "Hotel" || selectedAccommodationType?.Value == "Other")
                {
                    IsVisibleName = Visibility.Visible;
                }
                else
                {
                    IsVisibleName = Visibility.Collapsed;
                }

            }
        }
        public Visibility IsVisibleName
        {
            get { return isVisibleName; }
            set
            {
                isVisibleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleName"));
            }
        }
        public Visibility IsVisibleApartment
        {
            get { return isVisibleApartment; }
            set
            {
                isVisibleApartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleApartment"));
            }
        }
        public ObservableCollection<TripAssets> AccommodationAssetList
        {
            get
            {
                return accommodationAssetList;
            }
            set
            {
                accommodationAssetList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationAssetList"));
            }
        }
        public string AccommodationAddress
        {
            get { return accommodationAddress; }
            set { accommodationAddress = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationAddress")); }
        }
        public string AccommodationCoordinates
        {
            get { return accommodationCoordinates; }
            set { accommodationCoordinates = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCoordinates")); }
        }
        public TripAssets SelectedAccommodationApartment
        {
            get { return selectedAccommodationApartment; }
            set
            {
                selectedAccommodationApartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccommodationApartment"));                
            }
        }
        public string AccommodationName
        {
            get { return accommodationName; }
            set { accommodationName = value; OnPropertyChanged(new PropertyChangedEventArgs("AccommodationName")); }
        }
        public EmployeeTripsAccommodations SelectedAccommodations
        {
            get
            {
                return selectedAccommodations;
            }

            set
            {
                selectedAccommodations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAccommodations"));
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set {
                remarks = value;
            OnPropertyChanged(new PropertyChangedEventArgs("Remarks")); }
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
        //public DateTime? AccommodationCheckInDate
        //{
        //    get { return accommodationCheckInDate; }
        //    set
        //    {
        //        accommodationCheckInDate = value;
        //        if (AccommodationCheckOutDate > AccommodationCheckInDate)
        //        {
        //            DurationDays = Convert.ToInt32((AccommodationCheckOutDate - AccommodationCheckInDate).Days) + 1;
        //        }
        //        OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckInTime"));
        //    }
        //}
        public DateTime? AccommodationCheckInDate
        {
            get { return accommodationCheckInDate; }
            set
            {
                accommodationCheckInDate = value;

                if (AccommodationCheckOutDate.HasValue && accommodationCheckInDate.HasValue)
                {
                    TimeSpan? span = AccommodationCheckOutDate - accommodationCheckInDate;
                    if (span.HasValue)
                    {
                        DurationDays = Convert.ToInt32(span.Value.Days) + 1;
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckInTime"));
                if (AccommodationCheckInDate!=null)
                {
                    IsCheckOutDateEnabled = true;
                }
            }
        }

        public TimeSpan AccommodationCheckInTime
        {
            get { return accommodationCheckInTime; }
            set
            {
                accommodationCheckInTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckInTime"));
            }
        }
        //public DateTime? AccommodationCheckOutDate
        //{
        //    get { return accommodationCheckOutDate; }
        //    set
        //    {
        //        accommodationCheckOutDate = value;
        //        if (AccommodationCheckOutDate > AccommodationCheckInDate)
        //        {
        //            DurationDays = Convert.ToInt32((AccommodationCheckOutDate - AccommodationCheckInDate).Days) + 1;
        //        }
        //        OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckOutDate"));
        //    }
        //}        
        public DateTime? AccommodationCheckOutDate
        {
            get { return accommodationCheckOutDate; }
            set
            {
                accommodationCheckOutDate = value;

                if (AccommodationCheckOutDate.HasValue && AccommodationCheckInDate.HasValue)
                {
                    TimeSpan? span = AccommodationCheckOutDate - AccommodationCheckInDate;
                    if (span.HasValue)
                    {
                        DurationDays = span.Value.Days + 1;
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckOutDate"));
            }
        }


        public DateTime? Fromdate
        {
            get { return fromdate; }
            set
            {
                fromdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Fromdate"));
            }
        }

        public DateTime? ToDate
        {
            get { return todate; }
            set
            {
                todate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToDate"));
            }
        }


        public TimeSpan AccommodationCheckOutTime
        {
            get { return accommodationCheckOutTime; }
            set
            {
                accommodationCheckOutTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AccommodationCheckOutTime"));
            }
        }
        public int DurationDays
        {
            get { return durationDays; }
            set
            {
                durationDays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DurationDays"));
            }
        }
        public UInt32 IdAccommodation
        {
            get
            {
                return idAccommodation;
            }
            set
            {
                idAccommodation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdAccommodation"));
            }
        }

        public EmployeeTrips EmployeeDetails
        {
            get
            {
                return employeeDetails;
            }
            set
            {
                employeeDetails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDetails"));
            }
        }

        public bool IsCheckOutDateEnabled
        {
            get
            {
                return ischeckoutdateenabled;
            }

            set
            {
                ischeckoutdateenabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckOutDateEnabled"));
            }
        }
        #endregion 

        #region Constructor
        public AddEditAccommodationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditAccomodationViewModel()...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEditAccommodationAcceptButtonCommand = new RelayCommand(new Action<object>(AddEditAccommodationAcceptButtonCommandAction));           

                GeosApplication.Instance.Logger.Log("Constructor AddEditAccomodationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddEditAccomodationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method for Initialization .
        /// [001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        public void Init(UInt32 IdEmployeeTrip,EmployeeTrips EditEmployeeTrips)
        {
            try
            {
                EmployeeDetails = EditEmployeeTrips;
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                IdEmployeeTripNew = IdEmployeeTrip;
                FillAccommodationType();
                FillTripAssets();
                IsVisibleName = Visibility.Visible;
                Fromdate = EmployeeDetails.FromDate;
                ToDate = EmployeeDetails.ToDate;        
                AccommodationCheckInDate = null;
                AccommodationCheckOutDate = null;
                AccommodationCheckInTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                AccommodationCheckOutTime = new TimeSpan(0, 0, 0);
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditInit(EmployeeTripsAccommodations SelectedAccomaodation, EmployeeTrips EditEmployeeTrips)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                EmployeeDetails = EditEmployeeTrips;
                if (SelectedAccomaodation != null)
                {
                    FillAccommodationType();
                    FillTripAssets();
                    SelectedAccommodationType = AccommodationTypeList.FirstOrDefault(x => x.IdLookupValue == SelectedAccomaodation.IdType);
                    if (SelectedAccomaodation.ApartmentId!=0)
                    {
                        IsVisibleName = Visibility.Collapsed;
                        IsVisibleApartment = Visibility.Visible;
                        SelectedAccommodationApartment = AccommodationAssetList.FirstOrDefault(x => x.IdAsset == SelectedAccomaodation.ApartmentId);
                       
                    }
                    else
                    {
                        IsVisibleApartment = Visibility.Collapsed;
                        IsVisibleName = Visibility.Visible;
                        SelectedAccommodationApartment = AccommodationAssetList.FirstOrDefault(x => x.IdAsset == SelectedAccomaodation.ApartmentId);
                        AccommodationName = SelectedAccomaodation.Name;

                    }
                    AccommodationCheckInDate = SelectedAccomaodation.CheckInDate;
                    AccommodationCheckInTime = SelectedAccomaodation.CheckInTime;
                    AccommodationCheckOutTime = SelectedAccomaodation.CheckOutTime;
                    AccommodationCheckOutDate= SelectedAccomaodation.CheckOutDate;
                    AccommodationAddress = SelectedAccomaodation.Address;
                    AccommodationCoordinates = SelectedAccomaodation.Coordinates;
                    Remarks = SelectedAccomaodation.Remarks;
                    IdEmployeeTripNew = Convert.ToUInt32(SelectedAccomaodation.IdEmployeeTrip);
                    IdAccommodation = SelectedAccomaodation.IdAccommodations;
                  
                }

                

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillAccommodationType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMainTransport()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> tempTypeList = HrmService.GetLookupValues(138);
                AccommodationTypeList = tempTypeList
    .Where(x => x.IdLookupValue == 1874
             || x.IdLookupValue == 1873
             || x.IdLookupValue == 2121)
    .ToList();
                
                if (IsNew)
                {
                    SelectedAccommodationType = AccommodationTypeList.FirstOrDefault(x => x.Value == "Hotel");
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
        private void FillTripAssets()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTripAssets()...", category: Category.Info, priority: Priority.Low);
                IList<TripAssets> allTripassets = HrmService.HRM_GetEmployeeTripAssets();
                //CarAssetsList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2135));
                //SimCardAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2137));
                //MobilePhoneAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2138));
                AccommodationAssetList = new ObservableCollection<TripAssets>(allTripassets.Where(x => x.IdType == 2139));

                //if (IsNew)
                //{
                //    SelectedCarAsset = null;
                //    SelectedSimCardAsset = null;
                //    SelectedMobileAsset = null;
                //    SelectedAccommodationAsset = null;
                //}

                GeosApplication.Instance.Logger.Log("Method FillTripAssets()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTripAssets() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTripAssets() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillTripAssets() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void AddEditAccommodationAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditAccommodationAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccommodationType"));
                if (IsVisibleApartment == Visibility.Visible)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedAccommodationApartment"));
                }            
                if (IsVisibleName == Visibility.Visible) 
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AccommodationName"));
                }
                
                PropertyChanged(this, new PropertyChangedEventArgs("AccommodationCheckInDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("AccommodationCheckInTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("AccommodationCheckOutDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("AccommodationCheckOutTime"));
                PropertyChanged(this, new PropertyChangedEventArgs("AccommodationAddress"));

                if (error != null)
                {
                    return;
                }
                var a = AccommodationCheckInDate.Value.Date + AccommodationCheckInTime;
                var b= AccommodationCheckOutDate.Value.Date + AccommodationCheckOutTime;   

                if (a.Date >= EmployeeDetails.FromDate.Date && a<= b&& b.Date <= EmployeeDetails.ToDate.Date)
                {
                    if (IsNew)
                    {
                        SelectedAccommodations = new EmployeeTripsAccommodations();
                        SelectedAccommodations.IdEmployeeTrip = Convert.ToInt32(IdEmployeeTripNew);
                        SelectedAccommodations.IdType = (UInt32)SelectedAccommodationType.IdLookupValue;
                        SelectedAccommodations.AccommodationsType = SelectedAccommodationType.Value;
                        if (SelectedAccommodationApartment != null)
                        {
                            SelectedAccommodations.ApartmentId = SelectedAccommodationApartment.IdAsset;
                            SelectedAccommodations.ApartmentName = SelectedAccommodationApartment.PublicIdentifier;
                            SelectedAccommodations.Name = AccommodationName = SelectedAccommodationApartment.PublicIdentifier;
                        }
                        else
                        {
                            SelectedAccommodations.ApartmentId = 0;
                            SelectedAccommodations.ApartmentName = null;
                            SelectedAccommodations.Name = AccommodationName;
                            SelectedAccommodations.AccommodationName = AccommodationName;
                        }
                        //var checkOutDateOnly = AccommodationCheckOutDate.Date;
                        //SelectedAccommodations.CheckOutDate = checkOutDateOnly.Add(AccommodationCheckOutTime);
                        if (AccommodationCheckOutDate.HasValue)
                        {
                            var checkOutDateOnly = AccommodationCheckOutDate.Value.Date;
                            SelectedAccommodations.CheckOutDate = checkOutDateOnly.Add(AccommodationCheckOutTime);
                        }
                        SelectedAccommodations.CheckOutTime = AccommodationCheckOutTime;
                        //var checkInDateOnly = AccommodationCheckInDate.Date;
                        //SelectedAccommodations.CheckInDate = checkInDateOnly.Add(AccommodationCheckInTime);
                        if (AccommodationCheckInDate.HasValue)
                        {
                            var checkInDateOnly = AccommodationCheckInDate.Value.Date;
                            SelectedAccommodations.CheckInDate = checkInDateOnly.Add(AccommodationCheckInTime);
                        }

                        SelectedAccommodations.CheckInTime = AccommodationCheckInTime;
                        SelectedAccommodations.Address = AccommodationAddress;
                        SelectedAccommodations.Coordinates = AccommodationCoordinates;
                        SelectedAccommodations.Remarks = Remarks;
                        SelectedAccommodations.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        SelectedAccommodations.CreatedAt = DateTime.Now;
                        SelectedAccommodations.TransactionOperation = ModelBase.TransactionOperations.Modify;
                        IsSave = true;
                    }
                    else
                    {
                        SelectedAccommodations = new EmployeeTripsAccommodations();
                        SelectedAccommodations.IdAccommodations = IdAccommodation;
                        SelectedAccommodations.IdEmployeeTrip = Convert.ToInt32(IdEmployeeTripNew);
                        SelectedAccommodations.IdType = (UInt32)SelectedAccommodationType.IdLookupValue;
                        SelectedAccommodations.AccommodationsType = SelectedAccommodationType.Value;
                        if (SelectedAccommodationApartment != null)
                        {
                            SelectedAccommodations.ApartmentId = selectedAccommodationApartment.IdAsset;
                            SelectedAccommodations.ApartmentName = selectedAccommodationApartment.PublicIdentifier;
                            SelectedAccommodations.Name = selectedAccommodationApartment.PublicIdentifier;
                        }
                        else
                        {
                            SelectedAccommodations.ApartmentId = 0;
                            SelectedAccommodations.ApartmentName = null;
                            SelectedAccommodations.Name = AccommodationName;
                            SelectedAccommodations.AccommodationName = AccommodationName;
                        }
                        if (AccommodationCheckInDate.HasValue)
                        {
                            var checkInDateOnly = AccommodationCheckOutDate.Value.Date;
                            SelectedAccommodations.CheckOutDate = checkInDateOnly.Add(AccommodationCheckOutTime);
                        }
                        //SelectedAccommodations.CheckOutDate = AccommodationCheckOutDate.Value;
                        SelectedAccommodations.CheckOutTime = AccommodationCheckOutTime;
                        if (AccommodationCheckInDate.HasValue)
                        {
                            var checkInDateOnly = AccommodationCheckInDate.Value.Date;
                            SelectedAccommodations.CheckInDate = checkInDateOnly.Add(AccommodationCheckInTime);
                        }
                        //SelectedAccommodations.CheckInDate = AccommodationCheckInDate.Value;
                        SelectedAccommodations.CheckInTime = AccommodationCheckInTime;
                        SelectedAccommodations.Address = AccommodationAddress;
                        SelectedAccommodations.Coordinates = AccommodationCoordinates;
                        SelectedAccommodations.Remarks = Remarks;
                        SelectedAccommodations.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                        SelectedAccommodations.TransactionOperation = ModelBase.TransactionOperations.Update;
                        SelectedAccommodations.UpdatedAt = DateTime.Now;
                        IsSave = true;
                    }
                    RequestClose(null, null);
                }
                else
                {
                    if (AccommodationCheckInDate < EmployeeDetails.FromDate && AccommodationCheckOutDate > EmployeeDetails.ToDate)
                    {
                        // Both conditions fail
                        CustomMessageBox.Show(
                            System.Windows.Application.Current.FindResource("BothDateRangeMessage").ToString(),
                            Application.Current.Resources["PopUpWarningColor"].ToString(),
                            CustomMessageBox.MessageImagePath.NotOk,
                            MessageBoxButton.OK);
                    }
                    else if (AccommodationCheckInDate < EmployeeDetails.FromDate || AccommodationCheckInDate > EmployeeDetails.ToDate)
                    {
                        // Only check-in fails
                        CustomMessageBox.Show(
                            System.Windows.Application.Current.FindResource("CheckInDateRangeMessage").ToString(),
                            Application.Current.Resources["PopUpWarningColor"].ToString(),
                            CustomMessageBox.MessageImagePath.NotOk,
                            MessageBoxButton.OK);
                    }
                    else if (AccommodationCheckOutDate > EmployeeDetails.ToDate || AccommodationCheckOutDate < EmployeeDetails.FromDate)
                    {
                        // Only check-out fails
                        CustomMessageBox.Show(
                            System.Windows.Application.Current.FindResource("CheckOutDateRangeMessage").ToString(),
                            Application.Current.Resources["PopUpWarningColor"].ToString(),
                            CustomMessageBox.MessageImagePath.NotOk,
                            MessageBoxButton.OK);
                    }
                    else if ((AccommodationCheckOutDate < EmployeeDetails.FromDate) && (AccommodationCheckOutTime < EmployeeDetails.FromDate.TimeOfDay))
                    {
                        CustomMessageBox.Show(
                           System.Windows.Application.Current.FindResource("CheckOutDateTimeRangeMessage").ToString(),
                           Application.Current.Resources["PopUpWarningColor"].ToString(),
                           CustomMessageBox.MessageImagePath.NotOk,
                           MessageBoxButton.OK);
                    }
                    else if (AccommodationCheckOutDate < AccommodationCheckInDate)
                    {
                        // Only check-in fails
                        CustomMessageBox.Show(
                            System.Windows.Application.Current.FindResource("CheckOutDateTimeRangeMessageabc").ToString(),
                            Application.Current.Resources["PopUpWarningColor"].ToString(),
                            CustomMessageBox.MessageImagePath.NotOk,
                            MessageBoxButton.OK);
                    }
                    else if(a.Date <= b.Date == false)
                    {
                        CustomMessageBox.Show(
                          System.Windows.Application.Current.FindResource("CheckOutDateTimeRangeMessage").ToString(),
                          Application.Current.Resources["PopUpWarningColor"].ToString(),
                          CustomMessageBox.MessageImagePath.NotOk,
                          MessageBoxButton.OK);
                    }                

                }
                GeosApplication.Instance.Logger.Log("Method AddEditAccommodationAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditAccommodationAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                   me[BindableBase.GetPropertyName(() => SelectedAccommodationType)] +
                   me[BindableBase.GetPropertyName(() => selectedAccommodationApartment)] +
                   me[BindableBase.GetPropertyName(() => AccommodationName)] +
                   me[BindableBase.GetPropertyName(() => AccommodationCheckInDate)] +
                   me[BindableBase.GetPropertyName(() => AccommodationCheckInTime)] +
                   me[BindableBase.GetPropertyName(() => AccommodationCheckOutDate)] +
                   me[BindableBase.GetPropertyName(() => AccommodationCheckOutTime)] +
                   me[BindableBase.GetPropertyName(() => AccommodationAddress)];
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

                string Type = BindableBase.GetPropertyName(() => SelectedAccommodationType);
               
                if (IsVisibleApartment == Visibility.Visible)
                {
                    string Department = BindableBase.GetPropertyName(() => SelectedAccommodationApartment);
                    if (columnName == Department)
                        return TripAccommodationValidation.GetErrorMessage(Department, SelectedAccommodationApartment);
                }
                if (IsVisibleName == Visibility.Visible)
                {
                    string Name = BindableBase.GetPropertyName(() => AccommodationName);
                     if (columnName == Name)
                        return TripAccommodationValidation.GetErrorMessage(Name, AccommodationName);
                }
                
                string CheckInDate = BindableBase.GetPropertyName(() => AccommodationCheckInDate);
                string CheckInTime = BindableBase.GetPropertyName(() => AccommodationCheckInTime);
                string CheckOutDate = BindableBase.GetPropertyName(() => AccommodationCheckOutDate);
                string CheckOutTime = BindableBase.GetPropertyName(() => AccommodationCheckOutTime);
                string Address = BindableBase.GetPropertyName(() => AccommodationAddress);

                if (columnName == Type)
                {
                    return TripAccommodationValidation.GetErrorMessage(Type, SelectedAccommodationType);
                }
                     
                else if (columnName == CheckInDate)
                    return TripAccommodationValidation.GetErrorMessage(CheckInDate, AccommodationCheckInDate);

                else if (columnName == CheckInTime)
                {
                    // First, validate that CheckInTime is not null or invalid
                    var error = TripAccommodationValidation.GetErrorMessage(CheckInTime, AccommodationCheckInTime);
                    if (!string.IsNullOrWhiteSpace(error))
                        return error;

                    // ✅ Additional validation: Check if the time is negative
                    if (AccommodationCheckInTime.Ticks < 0)
                    {
                        return "Check-in Time cannot be negative.";
                    }
                    if (AccommodationCheckInTime.Ticks == 0)
                    {
                        return "Check-in Time Should be greater than 0.";
                    }
                    if (AccommodationCheckInDate != null && AccommodationCheckOutDate != null)
                    {
                        if (AccommodationCheckInTime.Ticks > AccommodationCheckOutTime.Ticks && AccommodationCheckInDate.Value.Date == AccommodationCheckOutDate.Value.Date)
                        {
                            return "Check-in  time must be the less than check-out time.";
                        }
                    }
                    
                }


                else if (columnName == CheckOutDate)
                    return TripAccommodationValidation.GetErrorMessage(CheckInDate, AccommodationCheckOutDate);

                else if (columnName == CheckOutTime)
                {
                    // Validate that CheckOutTime is not null or invalid
                    var error = TripAccommodationValidation.GetErrorMessage(CheckInTime, AccommodationCheckOutTime);
                    if (!string.IsNullOrWhiteSpace(error))
                        return error;

                    // ✅ Additional validation: Check if the time is negative
                    if (AccommodationCheckOutTime.Ticks < 0)
                    {
                        return "Check-out Time cannot be negative.";
                    }
                    if (AccommodationCheckOutTime.Ticks == 0)
                    {
                        return "Check-out Time Should be greater than 0.";
                    }
                    if (AccommodationCheckInTime.Ticks > AccommodationCheckOutTime.Ticks && AccommodationCheckInDate.Value.Date == AccommodationCheckOutDate.Value.Date)
                    {
                        return "Check-out time must be the greater than check-in time.";
                    }
                    if(AccommodationCheckInDate!=null && AccommodationCheckOutDate != null )
                    {
                        if (AccommodationCheckInTime.Ticks == AccommodationCheckOutTime.Ticks && AccommodationCheckInDate.Value.Date == AccommodationCheckOutDate.Value.Date)
                        {
                            return "Check-out time must not be the same as check-in time.";
                        }
                    }
                }


                else if (columnName == Address)
                    return TripAccommodationValidation.GetErrorMessage(Address, AccommodationAddress);

                return null;



            }
        }
        #endregion
    }
}

