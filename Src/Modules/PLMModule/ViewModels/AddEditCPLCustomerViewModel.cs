using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
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

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    public class AddEditCPLCustomerViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        DiscountCustomers discountCustomer;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private bool isDiscountCustomer ;
        private ulong idCPLCustomer;
        private ObservableCollection<Group> groupList;
        private Group selectedGroup;
        private ObservableCollection<Region> regionList;
        private List<object> selectedRegion;
        private ObservableCollection<Country> countryList;
        private List<object> selectedCountry;
        private ObservableCollection<Site> plantList;
        private List<object> selectedPlant;

        private CPLCustomer customer;

        private string error = string.Empty;
        private string informationError;

        private int isFilterStatus;

        private List<Region> selectedRegion_Save;
        private List<Country> selectedCountry_Save;
        private List<Site> selectedPlant_Save;


        private List<CPLCustomer> cPLCustomerList;

        private CPLCustomer selectedCustomer;

        private bool isRetrive;

        private List<CPLCustomer> cPLCustomers;
        DiscountCustomers selectedDiscountCustomer;
        List<DiscountCustomers> discountCustomers;
        ulong idDiscountCustomer;

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

        public ulong IdCPLCustomer
        {
            get
            {
                return idCPLCustomer;
            }

            set
            {
                idCPLCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCPLCustomer"));
            }
        }

        public ObservableCollection<Group> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList"));
            }
        }

        public Group SelectedGroup
        {
            get
            {
                return selectedGroup;
            }

            set
            {
                selectedGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
            }
        }

        public ObservableCollection<Region> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));
            }
        }

        public List<object> SelectedRegion
        {
            get
            {
                return selectedRegion;
            }

            set
            {
                selectedRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion"));
            }
        }

        public ObservableCollection<Country> CountryList
        {
            get
            {
                return countryList;
            }

            set
            {
                countryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryList"));
            }
        }

        public List<object> SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                selectedCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry"));
            }
        }

        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }

        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }

        public CPLCustomer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customer"));
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

        public int IsFilterStatus
        {
            get
            {
                return isFilterStatus;
            }

            set
            {
                isFilterStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFilterStatus"));
            }
        }

        public List<Region> SelectedRegion_Save
        {
            get
            {
                return selectedRegion_Save;
            }

            set
            {
                selectedRegion_Save = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion_Save"));
            }
        }

        public List<Country> SelectedCountry_Save
        {
            get
            {
                return selectedCountry_Save;
            }

            set
            {
                selectedCountry_Save = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry_Save"));
            }
        }

        public List<Site> SelectedPlant_Save
        {
            get
            {
                return selectedPlant_Save;
            }

            set
            {
                selectedPlant_Save = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant_Save"));
            }
        }

        public List<CPLCustomer> CPLCustomerList
        {
            get
            {
                return cPLCustomerList;
            }

            set
            {
                cPLCustomerList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLCustomerList"));
            }
        }
        public CPLCustomer SelectedCustomer
        {
            get
            {
                return selectedCustomer;
            }

            set
            {
                selectedCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomer"));
            }
        }


        public bool IsRetrive
        {
            get
            {
                return isRetrive;
            }

            set
            {
                isRetrive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRetrive"));
            }
        }

        public List<CPLCustomer> CPLCustomers
        {
            get
            {
                return cPLCustomers;
            }

            set
            {
                cPLCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPLCustomers"));
            }
        }

        public bool IsDiscountCustomer
        {
            get
            {
                return isDiscountCustomer;
            }
            set
            {
                isDiscountCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDiscountCustomer"));
            }
        }

        public List<DiscountCustomers> DiscountCustomers
        {
            get
            {
                return discountCustomers;
            }

            set
            {
                discountCustomers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountCustomers"));
            }
        }
        public DiscountCustomers DiscountCustomer
        {
            get
            {
                return discountCustomer;
            }

            set
            {
                discountCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountCustomer"));
            }
        }

        public DiscountCustomers SelectedDiscountCustomer
        {
            get
            {
                return selectedDiscountCustomer;
            }

            set
            {
                selectedDiscountCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDiscountCustomer"));
            }
        }
        public ulong IdDiscountCustomer
        {
            get
            {
                return idDiscountCustomer;
            }

            set
            {
                idDiscountCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDiscountCustomer"));
            }
        }
        #endregion

        #region ICommand
        public ICommand AcceptCPLCustomerActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand SelectedGroupIndexChangedCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCountryCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }



        #endregion

        #region Constructor

        public AddEditCPLCustomerViewModel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditPCMArticleImageViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptCPLCustomerActionCommand = new DelegateCommand<object>(SaveCPLCustomerAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                SelectedGroupIndexChangedCommand = new DelegateCommand<object>(SelectedGroupIndexChangedCommandAction);
                ChangeRegionCommand = new DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCountryCommand = new DelegateCommand<object>(ChangeCountryCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);

                FillGroups();

                GetCPLCustomers();

                GeosApplication.Instance.Logger.Log("Constructor AddEditCPLCustomerViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditCPLCustomerViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }





        #endregion

        #region Methods
        private void GetCPLCustomers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetCPLCustomers()...", category: Category.Info, priority: Priority.Low);

                CPLCustomers = new List<CPLCustomer>(PLMService.GetCPLCustomers());
                GeosApplication.Instance.Logger.Log("Method GetCPLCustomers()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCPLCustomers() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetCPLCustomers() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetCPLCustomerAll() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SaveCPLCustomerAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveCPLCustomerAction()...", category: Category.Info, priority: Priority.Low);
                InformationError = null;
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedRegion"));

                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";

                if (error != null)
                {
                    return;
                }

                //Plant
                SelectedPlant_Save = new List<Site>();
                if (SelectedPlant != null)
                {
                    foreach (Site site in SelectedPlant)
                    {
                        SelectedPlant_Save.Add(site);
                    }
                }
                //get exists 
                List<Site> SitesExist = new List<Site>();

                //Country
                SelectedCountry_Save = new List<Country>();
                if (SelectedCountry != null)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        SelectedCountry_Save.Add(country);
                    }
                }

                //Region
                SelectedRegion_Save = new List<Region>();
                if (SelectedRegion != null)
                {
                    foreach (Region region in SelectedRegion)
                    {
                        SelectedRegion_Save.Add(region);
                    }
                }

                //get exists 
                List<Country> CountriesExist = new List<Country>();

                //all record
                #region Discount_Customer
                if (IsDiscountCustomer == true)
                {
                    if (SelectedRegion != null)
                    {
                        if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdPlant == null && a.IdRegion == null && a.IdCountry == null))
                        {
                            if (IsNew == false && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdPlant == null && a.IdRegion == null && a.IdCountry == null).Count() != 1)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            else if (IsNew == true)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup))
                        {
                            if (IsNew == false && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup).Count() != 1)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            else if (IsNew == true && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup).Count() > 0)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }

                    if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup))
                    {
                        if (SelectedRegion != null)
                        {
                            if (SelectedRegion_Save.Count > 0)
                            {
                                foreach (Region itemRegion in SelectedRegion_Save)
                                {
                                    if (itemRegion != null)
                                    {
                                        if (SelectedCountry != null)
                                        {
                                            if (SelectedCountry_Save.Count > 0)
                                            {
                                                foreach (Country itemCountry in SelectedCountry_Save)
                                                {
                                                    if (itemCountry != null)
                                                    {
                                                        if (SelectedPlant_Save.Any(i => i == null))
                                                        {
                                                            if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == itemCountry.IdCountry))
                                                            {
                                                                if (IsNew == false && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == itemCountry.IdCountry).Count() != 1)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                                    return;
                                                                }
                                                                else if (IsNew == true)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == null))
                                                {
                                                    if (IsNew == false && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == null).Count() != 1)
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;
                                                    }
                                                    else if (IsNew == true)
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (DiscountCustomers.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == null))
                                {
                                    if (IsNew == false && DiscountCustomers.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == null).Count() != 1)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        return;
                                    }
                                    else if (IsNew == true)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        return;
                                    }

                                }
                            }

                        }
                    }
                    foreach (Site site in SelectedPlant_Save)
                    {
                        if (site != null)
                            if (DiscountCustomers.Any(a => a.IdGroup == site.IdGroup && a.IdPlant == site.IdSite && a.IdRegion == site.IdRegion && a.IdCountry == site.IdCountry))
                            {
                                SitesExist.Add(site);
                            }
                    }
                }
                #endregion
                #region CustomerPrice
                else
                {
                    if (SelectedRegion != null)
                    {
                        if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdPlant == null && a.IdRegion == null && a.IdCountry == null))
                        {
                            if (IsNew == false && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdPlant == null && a.IdRegion == null && a.IdCountry == null).Count() != 1)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            else if (IsNew == true)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup))
                        {
                            if (IsNew == false && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup).Count() != 1)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            else if (IsNew == true && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup).Count() > 0)
                            {
                                //msg
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                    }

                    if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup))
                    {
                        if (SelectedRegion != null)
                        {
                            if (SelectedRegion_Save.Count > 0)
                            {
                                foreach (Region itemRegion in SelectedRegion_Save)
                                {
                                    if (itemRegion != null)
                                    {
                                        if (SelectedCountry != null)
                                        {
                                            if (SelectedCountry_Save.Count > 0)
                                            {
                                                foreach (Country itemCountry in SelectedCountry_Save)
                                                {
                                                    if (itemCountry != null)
                                                    {
                                                        if (SelectedPlant_Save.Any(i => i == null))
                                                        {
                                                            if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == itemCountry.IdCountry))
                                                            {
                                                                if (IsNew == false && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == itemCountry.IdCountry).Count() != 1)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                                    return;
                                                                }
                                                                else if (IsNew == true)
                                                                {
                                                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == null))
                                                {
                                                    if (IsNew == false && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == itemRegion.IdRegion && a.IdCountry == null).Count() != 1)
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;
                                                    }
                                                    else if (IsNew == true)
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (CPLCustomerList.Any(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == null))
                                {
                                    if (IsNew == false && CPLCustomerList.Where(a => a.IdGroup == SelectedGroup.IdGroup && a.IdRegion == null).Count() != 1)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        return;
                                    }
                                    else if (IsNew == true)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExistAll").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        return;
                                    }

                                }
                            }

                        }
                    }


                    foreach (Site site in SelectedPlant_Save)
                    {
                        if (site != null)
                            if (CPLCustomerList.Any(a => a.IdGroup == site.IdGroup && a.IdPlant == site.IdSite && a.IdRegion == site.IdRegion && a.IdCountry == site.IdCountry))
                            {
                                SitesExist.Add(site);
                            }
                    }
                }
                #endregion
                if (SitesExist.Count > 0)
                {
                    if (IsNew == false && SitesExist.Count == 1)
                    {
                        //check this is edit row
                        if (!SitesExist.Any(a => a.IdGroup == SelectedCustomer.IdGroup && a.IdSite == SelectedCustomer.IdPlant && a.IdRegion == SelectedCustomer.IdRegion && a.IdCountry == SelectedCustomer.IdCountry))
                        {
                            //msg
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExist").ToString(), string.Join(",", SitesExist.Select(a => a.Name))), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                    else if (IsNew == true)
                    {
                        //msg
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CustomerExist").ToString(), string.Join(",", SitesExist.Select(a => a.Name))), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                }
                if (IsDiscountCustomer)
                {
                    DiscountCustomer = new DiscountCustomers();
                    DiscountCustomer.IdCustomerDiscountCustomer = IdDiscountCustomer;
                    DiscountCustomer.IdGroup = Convert.ToUInt32(SelectedGroup.IdGroup);
                    DiscountCustomer.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    DiscountCustomer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                }
                else
                {
                    Customer = new CPLCustomer();
                    Customer.IdCustomerPriceListCustomer = IdCPLCustomer;
                    Customer.IdGroup = Convert.ToUInt32(SelectedGroup.IdGroup);
                    Customer.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    Customer.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                }
                //region
                SelectedRegion_Save = new List<Region>();
                if (SelectedRegion != null)
                {
                    foreach (Region region in SelectedRegion)
                    {
                        SelectedRegion_Save.Add(region);
                    }
                }
                //country
                SelectedCountry_Save = new List<Country>();
                if (SelectedCountry != null)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        SelectedCountry_Save.Add(country);
                    }
                }



                IsSave = true;

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method SaveCPLCustomerAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveCPLCustomerAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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

        public void EditInit(CPLCustomer Customer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                SelectedCustomer = Customer;
                IdCPLCustomer = Customer.IdCustomerPriceListCustomer;

                SelectedGroup = new Group();
                SelectedGroup =GroupList.FirstOrDefault(a=>a.IdGroup== Convert.ToInt32(Customer.IdGroup));
                
                IsRetrive = true;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(DiscountCustomers Customer)//[rdixit][28.09.2022][GEOS2-3101]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                SelectedDiscountCustomer = Customer;
                IdDiscountCustomer = Customer.IdCustomerDiscountCustomer;

                SelectedGroup = new Group();
                SelectedGroup = GroupList.FirstOrDefault(a => a.IdGroup == Convert.ToInt32(Customer.IdGroup));

                SelectedCountry = new List<object>();
               // SelectedCountry = CountryList.FirstOrDefault(a => a.IdCountry == Convert.ToInt32(Customer.IdCountry));
                IsRetrive = true;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedGroupIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedGroupIndexChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsRetrive == false)
                {
                    RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));

                    //SelectedRegion = new List<object>();
                    //foreach (Region reg in RegionList)
                    //{
                    //    SelectedRegion.Add(reg);
                    //}
                    //SelectedRegion = new List<object>(SelectedRegion);


                    //SelectedCountry = new List<object>();
                    //foreach (Country cntry in CountryList)
                    //{
                    //    SelectedCountry.Add(cntry);
                    //}
                    //SelectedCountry = new List<object>(SelectedCountry);

                    //SelectedPlant = new List<object>();
                    //foreach (Site plnt in PlantList)
                    //{
                    //    SelectedPlant.Add(plnt);
                    //}
                    //SelectedPlant = new List<object>(SelectedPlant);
                }
                else
                {
                    RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));
                    if (IsDiscountCustomer)//[rdixit][28.09.2022][GEOS2-3101]
                    {
                        SelectedRegion = new List<object>();
                        SelectedRegion.Add(RegionList.FirstOrDefault(a => a.IdRegion == SelectedDiscountCustomer.IdRegion));
                        SelectedRegion = new List<object>(SelectedRegion);

                        SelectedCountry = new List<object>();
                        SelectedCountry.Add(CountryList.FirstOrDefault(a => a.IdCountry == SelectedDiscountCustomer.IdCountry));
                        SelectedCountry = new List<object>(SelectedCountry);

                        SelectedPlant = new List<object>();
                        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == SelectedDiscountCustomer.IdPlant));
                        SelectedPlant = new List<object>(SelectedPlant);
                    }
                    else
                    {
                        SelectedRegion = new List<object>();
                        SelectedRegion.Add(RegionList.FirstOrDefault(a => a.IdRegion == SelectedCustomer.IdRegion));
                        SelectedRegion = new List<object>(SelectedRegion);

                        SelectedCountry = new List<object>();
                        SelectedCountry.Add(CountryList.FirstOrDefault(a => a.IdCountry == SelectedCustomer.IdCountry));
                        SelectedCountry = new List<object>(SelectedCountry);

                        SelectedPlant = new List<object>();
                        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == SelectedCustomer.IdPlant));
                        SelectedPlant = new List<object>(SelectedPlant);
                    }
                    IsRetrive = false;
                }
                
                GeosApplication.Instance.Logger.Log("Method SelectedGroupIndexChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SelectedGroupIndexChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
    

        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 1;
                    if (SelectedRegion != null)
                    {
                        //FillCountries();
                        //FillPlants();
                    }
                    else
                    {
                        SelectedCountry = new List<object>();
                        SelectedPlant = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeRegionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeCountryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 2;
                    if (SelectedCountry != null)
                    {
                       //[adhatkar][22-02-2021] comment because luis said that no need to set group by country
                        //List<object> countries = new List<object>(SelectedCountry);

                        //Country selectedCountry_First = (Country)countries.FirstOrDefault();
                        //SelectedGroup = GroupList.FirstOrDefault(a => a.IdGroup == selectedCountry_First.IdGroup);

                        //SelectedCountry = new List<object>();
                        //foreach (Country cntry in CountryList)
                        //{
                        //    if (countries.Cast<Country>().Any(a => a.IdCountry == cntry.IdCountry))
                        //    {
                        //        SelectedCountry.Add(cntry);
                        //    }
                        //}
                        //SelectedCountry = new List<object>(SelectedCountry);

                        FillRegions();
                    }
                    else
                    {
                        //SelectedRegion = new List<object>();
                        SelectedPlant = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeCountryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangePlantCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    IsFilterStatus = 3;
                    if (SelectedPlant != null)
                    {
                        List<object> plants = new List<object>(SelectedPlant);

                        Site selectedPlant_First = (Site)plants.FirstOrDefault();

                        SelectedGroup = GroupList.FirstOrDefault(a => a.IdGroup == selectedPlant_First.IdGroup);

                        SelectedPlant = new List<object>();
                        foreach (Site plnt in PlantList)
                        {
                            if (plants.Cast<Site>().Any(a => a.IdSite == plnt.IdSite))
                            {
                                SelectedPlant.Add(plnt);
                            }
                        }
                        SelectedPlant = new List<object>(SelectedPlant);

                        FillRegions();
                        FillCountries();
                    }
                    else
                    {
                        //SelectedRegion = new List<object>();
                        //SelectedCountry = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillGroups()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroups ...", category: Category.Info, priority: Priority.Low);
                GroupList = new ObservableCollection<Group>(PLMService.GetGroups());
                GroupList.Insert(0, new Group() { GroupName = "", IdGroup = 0 });
                SelectedGroup = GroupList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillGroups() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroups() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

                string CountryNames = "0";
                string SiteNames = "0";
                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                if (SelectedPlant == null)
                    SelectedPlant = new List<object>();

                if (IsFilterStatus == 2)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        if (CountryNames == "0")
                            CountryNames = country.Name;
                        else
                            CountryNames = CountryNames + "," + country.Name;
                    }
                }
                else
                {
                    CountryNames = "0";
                }

                if (isFilterStatus == 3)
                {
                    foreach (Site site in SelectedPlant)
                    {
                        if (SiteNames == "0")
                            SiteNames = site.Name;
                        else
                            SiteNames = SiteNames + "," + site.Name;
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Region> SelectedRegionList = new List<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, CountryNames,SiteNames));

                SelectedRegion = new List<object>();
                foreach (Region reg in SelectedRegionList)
                {
                    SelectedRegion.Add(RegionList.FirstOrDefault(a=>a.IdRegion==reg.IdRegion));
                }

                SelectedRegion = new List<object>(SelectedRegion);
                GeosApplication.Instance.Logger.Log("Method FillRegions() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegions() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountries ...", category: Category.Info, priority: Priority.Low);

                string RegionNames = "0";
                string SiteNames = "0";
                if (SelectedRegion == null)
                    SelectedRegion = new List<object>();

                if (SelectedPlant == null)
                    SelectedPlant = new List<object>();

                if (isFilterStatus == 1)
                {
                    foreach (Region Region in SelectedRegion)
                    {
                        if (RegionNames == "0")
                            RegionNames = Region.RegionName;
                        else
                            RegionNames = RegionNames + "," + Region.RegionName;
                    }
                }
                else
                {
                    RegionNames = "0";
                }

                if (isFilterStatus == 3)
                {
                    foreach (Site site in SelectedPlant)
                    {
                        if (SiteNames == "0")
                            SiteNames = site.Name;
                        else
                            SiteNames = SiteNames + "," + site.Name;
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Country> SelectedCountryList = new List<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, RegionNames, SiteNames));

                SelectedCountry = new List<object>();
                foreach (Country cntry in SelectedCountryList)
                {
                    SelectedCountry.Add(CountryList.FirstOrDefault(a=>a.IdCountry==cntry.IdCountry));
                }
                SelectedCountry = new List<object>(SelectedCountry);
                GeosApplication.Instance.Logger.Log("Method FillCountries() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPlants ...", category: Category.Info, priority: Priority.Low);

                string RegionNames = "0";
                string CountryNames = "0";
                if (SelectedRegion == null)
                    SelectedRegion = new List<object>();

                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                if (isFilterStatus == 1)
                {
                    foreach (Region Region in SelectedRegion)
                    {
                        if (RegionNames == "0")
                            RegionNames = Region.RegionName;
                        else
                            RegionNames = RegionNames + "," + Region.RegionName;
                    }
                }
                else
                {
                    RegionNames = "0";
                }

                if (IsFilterStatus == 2)
                {
                    foreach (Country country in SelectedCountry)
                    {
                        if (CountryNames == "0")
                            CountryNames = country.Name;
                        else
                            CountryNames = CountryNames + "," + country.Name;
                    }
                }
                else
                {
                    CountryNames = "0";
                }


                List<Site> SelectedPlantList = new List<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, RegionNames, CountryNames));

                SelectedPlant = new List<object>();
                foreach (Site plnt in SelectedPlantList)
                {
                    SelectedPlant.Add(PlantList.FirstOrDefault(a=>a.IdSite==plnt.IdSite));
                }
                SelectedPlant = new List<object>(SelectedPlant);
                GeosApplication.Instance.Logger.Log("Method FillPlants() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                me[BindableBase.GetPropertyName(() => SelectedRegion)];


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

                string selectedRegion = BindableBase.GetPropertyName(() => SelectedRegion);

                if (columnName == selectedRegion)
                {
                    return AddEditCPLCustomerValidation.GetErrorMessage(selectedRegion, SelectedRegion);
                }
                return null;
            }
        }
        #endregion
    }
}
