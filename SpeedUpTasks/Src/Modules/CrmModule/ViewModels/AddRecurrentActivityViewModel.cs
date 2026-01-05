using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
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
using System.Globalization;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddRecurrentActivityViewModel : INotifyPropertyChanged
    {

        #region Tasklog
        //M051-14	(#60515) Allow to clear recurrent activity periodicity [adadibathina]

        #endregion

        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        private string windowHeader;
        private string selectedWeekDay;
        private bool isNew;
        private int selectedPeriodicity;
        private bool isSave;
        private int selectedIndexWeekDay;
        private ActivitiesRecurrence existRecurrentActivity;
        private string group;
        private string plant;
        private string weekDay;
        private int periodicity;
        private long idActivityRecurrance;
        private string siteNameWithoutCountry;
        private string country;
        private string region;
        private string salesOwner;
        private int idSite;
        private byte isSalesResponsible;
        private string visible;
        #endregion

        #region Properties     
        public List<object> PeriodicityList { get; set; }
        public List<object> WeekDaysList { get; set; }
        public ActivitiesRecurrence UpdateRecurrentActivity { get; set; }

        public int IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSite"));
            }
        }
        public string SalesOwner
        {
            get
            {
                return salesOwner;
            }

            set
            {
                salesOwner = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SalesOwner"));
            }
        }
        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }
        public string SiteNameWithoutCountry
        {
            get
            {
                return siteNameWithoutCountry;
            }

            set
            {
                siteNameWithoutCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SiteNameWithoutCountry"));
            }
        }

        public string Region
        {
            get
            {
                return region;
            }

            set
            {
                region = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Region"));
            }
        }
        public long IdActivityRecurrance
        {
            get { return idActivityRecurrance; }
            set
            {
                idActivityRecurrance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdActivityRecurrance"));
            }
        }
       
        public ActivitiesRecurrence ExistRecurrentActivity
        {
            get { return existRecurrentActivity; }
            set
            {
                existRecurrentActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistRecurrentActivity"));
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

        public string SelectedWeekDay
        {
            get { return selectedWeekDay; }
            set
            {
                selectedWeekDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWeekDay"));
            }
        }

        public int SelectedPeriodicity
        {
            get { return selectedPeriodicity; }
            set
            {
                selectedPeriodicity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPeriodicity"));
            }
        }

        public int SelectedIndexWeekDay
        {
            get { return selectedIndexWeekDay; }
            set
            {
                selectedIndexWeekDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexWeekDay"));
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



        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Group"));
            }
        }

        public string Plant
        {
            get
            {
                return plant;
            }

            set
            {
                plant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Plant"));
            }
        }
        public string WeekDay
        {
            get
            {
                return weekDay;
            }

            set
            {
                weekDay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeekDay"));
            }
        }
        public int Periodicity
        {
            get
            {
                return periodicity;
            }

            set
            {
                periodicity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Periodicity"));
            }
        }


        public byte IsSalesResponsible
        {
            get { return isSalesResponsible; }
            set
            {
                isSalesResponsible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSalesResponsible"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }
        #endregion

        #region Public ICommand
        public ICommand AddRecurrentActivityViewCancelButtonCommand { get; set; }
        public ICommand AddRecurrentActivityViewAccept { get; set; }
        public ICommand CommandTextInput { get; set; }
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

        #region Constructor
        public AddRecurrentActivityViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddRecurrentActivityViewModel()...", category: Category.Info, priority: Priority.Low);
                AddRecurrentActivityViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddRecurrentActivityViewAccept = new RelayCommand(new Action<object>(AddRecurrentActivity));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                //set hide/show shortcuts on permissions
                Visible = Visibility.Visible.ToString();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    Visible = Visibility.Hidden.ToString();
                }
                else
                {
                    Visible = Visibility.Visible.ToString();
                }
                GeosApplication.Instance.Logger.Log("Constructor AddRecurrentActivityViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddRecurrentActivityViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion



        #region Methods
        /// <summary>
        /// method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        /// Method to Fill Periodicity List
        /// </summary>
        private void FillPeriodicityList()
        {

            PeriodicityList = new List<object>();
            PeriodicityList.Add("---");
            PeriodicityList.Add(7);
            PeriodicityList.Add(14);
            PeriodicityList.Add(30);
            PeriodicityList.Add(60);
            PeriodicityList.Add(90);
            PeriodicityList.Add(120);

        }

        /// <summary>
        /// Method to Fill WeekDays List
        /// </summary>
        private void FillWeekDaysList()
        {

            WeekDaysList = new List<object>();
            WeekDaysList.Add("---");
            string[] names = new CultureInfo("en-GB").DateTimeFormat.DayNames;
            foreach (var item in names)
            {
                WeekDaysList.Add(item);
            }
            SelectedIndexWeekDay = 0;
        }




        // Method to get selected Recurrance Activity information
        public void EditInit(ActivitiesRecurrence selectedRecurranceActivity)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                ExistRecurrentActivity = selectedRecurranceActivity;
                FillPeriodicityList();
                FillWeekDaysList();
                Group = selectedRecurranceActivity.Group;
                SiteNameWithoutCountry = selectedRecurranceActivity.SiteNameWithoutCountry;
                Country = selectedRecurranceActivity.Country;
                Region = selectedRecurranceActivity.Region;
                SalesOwner = selectedRecurranceActivity.SalesOwner;
                IdSite = selectedRecurranceActivity.IdSite;
                IsSalesResponsible= selectedRecurranceActivity.IsSalesResponsible;
                if (PeriodicityList.IndexOf(ExistRecurrentActivity.Periodicity) < 0)
                    SelectedPeriodicity = 0;
                else
                    SelectedPeriodicity = PeriodicityList.IndexOf(ExistRecurrentActivity.Periodicity);
                if (WeekDaysList.IndexOf(ExistRecurrentActivity.WeekDay) < 0)
                    SelectedIndexWeekDay = 0;
                else
                    SelectedIndexWeekDay = WeekDaysList.IndexOf(ExistRecurrentActivity.WeekDay);
                IdActivityRecurrance = selectedRecurranceActivity.IdActivityRecurrence;


                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to update Recurrent Activity
        /// </summary>
        /// <param name="obj"></param>
        private void AddRecurrentActivity(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddRecurrentActivity ...", category: Category.Info, priority: Priority.Low);
                //M051-14
                // string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexWeekDay"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPeriodicity"));
                // M051 - 14
                //if (error != null)
                //{
                //    return;
                //}
             
                if (!IsNew)
                {
                    UpdateRecurrentActivity = new ActivitiesRecurrence()
                    {
                        Group = Group,
                        SiteNameWithoutCountry = SiteNameWithoutCountry,
                        Country = Country,
                        Region = Region,
                        SalesOwner = SalesOwner,
                        IdActivityType = 96,
                        // M051 - 14
                        // WeekDay = (string)WeekDaysList[SelectedIndexWeekDay],
                        // Periodicity = (int)PeriodicityList[SelectedPeriodicity],
                        IdSite = IdSite,
                        IdActivityRecurrence = IdActivityRecurrance,
                        IsSalesResponsible=IsSalesResponsible
                    };


                    UpdateRecurrentActivity.Periodicity = SelectedPeriodicity == 0 ?   null :  (int?)PeriodicityList[SelectedPeriodicity];

                    UpdateRecurrentActivity.WeekDay= SelectedIndexWeekDay==0 ? null : (string)WeekDaysList[SelectedIndexWeekDay];



                    UpdateRecurrentActivity = CrmStartUp.UpdateActivitiesRecurrence_V2031(UpdateRecurrentActivity);
                    IsSave = true;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RecurrentActivityInformationUpdateSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method AddRecurrentActivity()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddRecurrentActivity() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddRecurrentActivity() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddRecurrentActivity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

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
        //M051-14
        //bool allowValidation = false;
        //string EnableValidationAndGetError()
        //{
        //    allowValidation = true;
        //    string error = ((IDataErrorInfo)this).Error;
        //    if (!string.IsNullOrEmpty(error))
        //    {
        //        return error;
        //    }
        //    return null;
        //}

        //string IDataErrorInfo.Error
        //{
        //    get
        //    {
        //        if (!allowValidation) return null;
        //        IDataErrorInfo me = (IDataErrorInfo)this;
        //        string error =
        //             me[BindableBase.GetPropertyName(() => SelectedPeriodicity)] +
        //            me[BindableBase.GetPropertyName(() => SelectedIndexWeekDay)];


        //        if (!string.IsNullOrEmpty(error))
        //            return "Please check inputted data.";

        //        return null;
        //    }
        //}

        //string IDataErrorInfo.this[string columnName]
        //{
        //    get
        //    {
        //        if (!allowValidation) return null;
        //        string selectedWeekDay = BindableBase.GetPropertyName(() => SelectedIndexWeekDay);
        //        string periodicity = BindableBase.GetPropertyName(() => SelectedPeriodicity);

        //        if (columnName == selectedWeekDay)
        //        {
        //            return RecurrentActivityValidation.GetErrorMessage(selectedWeekDay, SelectedIndexWeekDay);
        //        }

        //        if (columnName == periodicity)
        //        {
        //            return RecurrentActivityValidation.GetErrorMessage(periodicity, SelectedPeriodicity);
        //        }

        //        return null;
        //    }
        //}

        #endregion
    }
}
