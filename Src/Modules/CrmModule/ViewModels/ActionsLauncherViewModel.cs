using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.Crm.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    /// <summary>
    /// ViewModel Created In Task GEOS2-4274 by rdixit on 22.05.2023
    /// </summary>
    public class ActionsLauncherViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        //IPLMService PLMService = new PLMServiceController("localhost:6699");
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
        List<object> selectedSource;
        private ObservableCollection<LookupValue> sourceList;
        ObservableCollection<Tag> tagList;
        ActionPlanItem actionPlanItem;
        DateTime dueDate;
        ObservableCollection<People> filteredSalesUsers;
        int selectedReporter;
        User selectedActionReporter;
        ObservableCollection<User> reporterList;
        ObservableCollection<People> listSalesUsers;
        List<Site> selectedCustomerSite_Save;
        ObservableCollection<SitesWithCustomer> customerSiteList;
        ObservableCollection<SitesWithCustomer> filteredCustomerSiteList;
        ObservableCollection<Regions> regionList;
        List<object> selectedRegion_Save;
        List<object> selectedRegion;

        ObservableCollection<Country> countryList;
        List<Country> selectedCountry_Save;
        List<object> selectedCountry;
        string tag;
        string subject;
        string error = string.Empty;
        string informationError;
        bool allowValidation = false;
        bool isChecked;
        string salesownerforplant = "0";//chitra.girigosavi GEOS2-6695 04/04/2025
        #region//chitra.girigosavi GEOS2-7242 11/04/2025
        ObservableCollection<LookupValue> statusList;
        private int selectedStatusIndex;
        LookupValue selectedStatus;
        private string status;
        #endregion
        #endregion

        #region Properties        
        public bool isHandlingProgrammaticChange { get; set; }
        public int? PlantMainCheck;
        public int? PlantMainUnCheck;
        public bool IsSave { get; set; }
        public bool IsExpand { get; set; }
        public bool AllPlant { get; set; }
        public bool AllSaleOwner { get; set; }
        public bool ShowValidation { get; set; }
        public ActionPlanItem ActionPlanItem
        {
            get { return actionPlanItem; }
            set
            {
                actionPlanItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionPlanItem"));
            }
        }
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }
        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
                //if(TagList!=null)
                //ShowPopupAsPerFirstName(Tag);
                OnPropertyChanged(new PropertyChangedEventArgs("Tag"));
            }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; OnPropertyChanged(new PropertyChangedEventArgs("Subject")); }
        }
        public ObservableCollection<User> ReporterList
        {
            get { return reporterList; }
            set { reporterList = value; OnPropertyChanged(new PropertyChangedEventArgs("ReporterList")); }
        }
        public User SelectedActionReporter
        {
            get
            {
                return selectedActionReporter;
            }

            set
            {
                selectedActionReporter = value;
                if (SelectedActionReporter != null)
                {
                    if (ReporterList != null)
                    {
                        SelectedReporter = ReporterList.IndexOf(SelectedActionReporter);
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActionReporter"));
            }
        }
        public int SelectedReporter
        {
            get
            {
                return selectedReporter;
            }

            set
            {
                selectedReporter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReporter"));
            }
        }

        public ObservableCollection<People> FilteredSalesUsers
        {
            get { return filteredSalesUsers; }
            set { filteredSalesUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("FilteredSalesUsers")); }
        }
        public ObservableCollection<People> ListSalesUsers
        {
            get { return listSalesUsers; }
            set { listSalesUsers = value; OnPropertyChanged(new PropertyChangedEventArgs("ListSalesUsers")); }
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
        public ObservableCollection<Regions> RegionList
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
        public ObservableCollection<SitesWithCustomer> CustomerSiteList
        {
            get
            {
                return customerSiteList;
            }

            set
            {
                customerSiteList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerSiteList"));
            }
        }
        public ObservableCollection<SitesWithCustomer> FilteredCustomerSiteList
        {
            get
            {
                return filteredCustomerSiteList;
            }

            set
            {
                filteredCustomerSiteList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilteredCustomerSiteList"));
            }
        }
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsChecked"));
            }
        }

        public ObservableCollection<Tag> TagList
        {
            get
            {
                return tagList;
            }
            set
            {
                tagList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TagList"));
            }
        }
        ObservableCollection<Tag> filteredTags;
        public ObservableCollection<Tag> FilteredTags
        {
            get
            {
                return filteredTags;
            }
            set
            {
                filteredTags = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilteredTags"));
            }
        }
        //[GEOS2-4734][07.08.2023][rdixit]
        Tag selectedTag;
        public Tag SelectedTag
        {
            get
            {
                return selectedTag;
            }
            set
            {
                selectedTag = value;
                if (SelectedTag != null)
                {
                    Tag = SelectedTag.Name;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTag"));
            }
        }

        public ObservableCollection<LookupValue> SourceList
        {
            get { return sourceList; }
            set
            {
                sourceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SourceList"));
            }
        }

        public List<object> SelectedSource
        {
            get
            {
                return selectedSource;
            }

            set
            {
                selectedSource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSource"));
            }
        }

        //chitra.girigosavi GEOS2-6695 04/04/2025
        public string SalesOwnerForPlant
        {
            get { return salesownerforplant; }
            set { salesownerforplant = value; OnPropertyChanged(new PropertyChangedEventArgs("SalesOwnerForPlant")); }
        }

        #region//chitra.girigosavi GEOS2-7242 11/04/2025
        public ObservableCollection<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }

        public int SelectedStatusIndex
        {
            get { return selectedStatusIndex; }
            set
            {
                selectedStatusIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatusIndex"));
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (value != null)
                {
                    status = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs("Status"));
                }
            }
        }
        public LookupValue SelectedStatus
        {
            get
            {
                return selectedStatus;
            }

            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }

        private List<object> selectedStatuses;


        public List<object> SelectedStatuses
        {
            get
            {
                return selectedStatuses;
            }

            set
            {
                selectedStatuses = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatuses"));
            

            }
        }


        #endregion
        #endregion

        #region ICommand
        public ICommand CollapseExpand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCountryCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand PlantUnCheckBoxSelectedCommand { get; set; }
        public ICommand PlantCheckBoxSelectedCommand { get; set; }
        public ICommand SalesOwnerUnCheckBoxSelectedCommand { get; set; }
        public ICommand SalesOwnerCheckBoxSelectedCommand { get; set; }
        public ICommand plantSelectUnselectCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        public ICommand ChangeSourceCommand { get; set; }
        public ICommand ChangeStatusCommand { get; set; }//chitra.girigosavi GEOS2-7242 11/04/2025
        #endregion

        #region Constructor
        public ActionsLauncherViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

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
                SalesOwnerCheckBoxSelectedCommand = new RelayCommand(new Action<object>(SalesOwnerCheckBoxSelectedCommandAction));
                SalesOwnerUnCheckBoxSelectedCommand = new DelegateCommand<object>(SalesOwnerUnCheckBoxSelectedCommandAction);
                AcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AcceptButtonCommandAction);
                ChangeRegionCommand = new DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCountryCommand = new DelegateCommand<object>(ChangeCountryCommandAction);
                ChangeSourceCommand = new DelegateCommand<object>(ChangeSourceCommandAction);   //[GEOS2-6446][05.11.2024][rdixit]
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                CollapseExpand = new DelegateCommand<object>(ExpandAndCollapsCommandAction);
                plantSelectUnselectCommand = new DelegateCommand<object>(plantSelectUnselectCommandAction);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChanging);
                ChangeStatusCommand = new DelegateCommand<object>(ChangeStatusCommandAction);//chitra.girigosavi GEOS2-7242 11/04/2025
                IsChecked = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor EditProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditProductTypeViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        void AcceptButtonCommandAction(object obj)
        {
            try
            {
                string error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Subject"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedReporter"));
                PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedStatusIndex"));

                if (error != null)
                {
                    return;
                }
                else
                {
                    List<SitesWithCustomer> CustomerSiteList = FilteredCustomerSiteList.Where(p => p.IsChecked == true && p.IdSite != null && p.IdSite != 0).ToList();
                    List<SitesWithCustomer> FinalList = CustomerSiteList.Where(j => j.SalesOwnerList != null).Select(i => (SitesWithCustomer)i.Clone()).ToList();

                    foreach (var plant in CustomerSiteList)
                    {
                        if (plant.SalesOwnerList != null)
                        {
                            FinalList.FirstOrDefault(i => i.IdSite == plant.IdSite).SalesOwnerList =
                                new ObservableCollection<People>(plant.SalesOwnerList.Where(i => FilteredSalesUsers.Where(s => s.IsChecked).Any(j => j.IdPerson == i.IdPerson)).ToList());
                        }
                    }
                    ActionPlanItem = new ActionPlanItem();
                    ActionPlanItem.IdActionPlan = 1;
                    ActionPlanItem.IdReporter = ReporterList[SelectedReporter].IdUser;
                    ActionPlanItem.Reporter = ReporterList[SelectedReporter].FullName;
                    ActionPlanItem.Title = Subject;
                    ActionPlanItem.Tag = new Data.Common.Tag();
                    ActionPlanItem.Tag.Name = Tag;
                    ActionPlanItem.CurrentDueDate = DueDate;
                    ActionPlanItem.ExpectedDueDate = DueDate;
                    ActionPlanItem.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    ActionPlanItem.OpenDate = GeosApplication.Instance.ServerDateTime.Date;
                    if (ActionPlanItem.Status == null)//chitra.girigosavi[GEOS2-7242][14/04/2025]
                    {
                        ActionPlanItem.Status = new LookupValue();
                    }
                    if (SelectedStatus != null)
                    {
                        ActionPlanItem.Status.IdLookupValue = SelectedStatus.IdLookupValue;
                    }
                    


                    ActionPlanItem.TransactionOperation = ModelBase.TransactionOperations.Add;
                    //Service ActionLauncherAdd updated with ActionLauncherAdd_V2500 by [GEOS2-5490][rdixit][15.03.2024]
                    //ActionPlanItem = CrmStartUp.ActionLauncherAdd_V2500(ActionPlanItem, FinalList);
                    //ActionPlanItem = CrmStartUp.ActionLauncherAdd_V2630(ActionPlanItem, FinalList);//chitra.girigosavi[GEOS2-7242][14/04/2025]
                    ActionPlanItem = CrmStartUp.ActionLauncherAdd_V2640(ActionPlanItem, FinalList);//chitra.girigosavi[GEOS2-8129][19/05/2025]

                    IsSave = true;

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActionsAddedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    if (IsSave)
                    {
                        RequestClose(null, null);
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public void Init()
        {
            try
            {
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

                ShowValidation = false;
                IsSave = false;
                Tag = string.Empty;
                FillTagList();
                Subject = string.Empty;
                DueDate = DateTime.Today;
                PlantMainCheck = PlantMainUnCheck = 0;
                SelectedReporter = 0;
                RegionList = new ObservableCollection<Regions>(CrmStartUp.GetRegionsByGroupAndCountryAndSites(0, "0", "0"));
                CountryList = new ObservableCollection<Country>(CrmStartUp.GetCountriesByGroupAndRegionAndSites(0, "0", "0"));
                FillSites();
                FillUsers();
                FillReporterList();
                FillSourceList();
                //FillStatusList(FilteredCustomerSiteList);//chitra.girigosavi GEOS2-7242 11/04/2025
                //[pramod.misal][GEOS2-8169][17.07.2025]
                FillStatusList(FilteredCustomerSiteList);

                ShowValidation = true;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #region
        //Methods updated in task //[rdixit][GEOS2-4655][GEOS2-465][18.07.2023] 
        #endregion
        void temp(int idperson, bool flag)
        {

            List<int> test = FilteredSalesUsers.Where(i => i.IsChecked).Select(j => j.IdPerson).ToList();
            if (flag)
                test.Add(idperson);
            else
                test.Remove(idperson);
            SalesOwnerForPlant = string.Join(",", test);
            ObservableCollection<Site> plantList = new ObservableCollection<Site>();
            try
            {
                //CrmStartUp = new CrmServiceController("localhost:6699");
                if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // Only SelectedSource is not null
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // Only SelectedRegion is not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // Only SelectedCountry is not null
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", country, SalesOwnerForPlant, "0"));

                }
                #region chitra.girigosavi GEOS2-7242 10/04/2025
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // Only SelectedStatus is not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", "0", SalesOwnerForPlant, status));

                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedSource and Selectedstatus are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, status));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedRegion and SelectedStatus are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, "0", SalesOwnerForPlant, status));

                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedStatus and SelectedCountry are not null
                   // string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, status));

                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", country, SalesOwnerForPlant, status));
                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedSource, SelectedStatus and SelectedRegion are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, regions, "0", SalesOwnerForPlant, status));

                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedSource, SelectedStatus and SelectedCountry are not null
                   // string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", country, SalesOwnerForPlant, status));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedRegion SelectedStatus and SelectedCountry are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, country, SalesOwnerForPlant, status));

                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedSource, SelectedRegion and SelectedCountry are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, regions, country, SalesOwnerForPlant, "0"));

                }
                #endregion
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // SelectedSource and SelectedRegion are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, "0"));

                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, regions, "0", SalesOwnerForPlant, "0"));
                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedSource and SelectedCountry are not null
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", country, SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedRegion and SelectedCountry are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, country, SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // All are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, regions, country, SalesOwnerForPlant, status));

                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses == null)
                {
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", "0", SalesOwnerForPlant, "0"));

                }
                if ((SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SalesOwnerForPlant == "" && SelectedStatuses == null))
                {
                    FilteredCustomerSiteList = CustomerSiteList;
                    bool allExists = FilteredCustomerSiteList.Any(x => x.IdSite == 0 && x.Name == "(ALL)");
                    if (!allExists)
                    {
                        FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                    }
                    foreach (var item in FilteredCustomerSiteList)
                    {
                        item.IsChecked = false;
                    }
                }
                else
                {
                    FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(CustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdSite == item1.IdSite) || item1.IdSite == null));
                    FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(FilteredCustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdGroup == item1.IdGroup)));

                    foreach (var item in CustomerSiteList)
                    {
                        item.IsChecked = false;
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            // Ensure checked items stay checked
            if (test.Count > 0)
            {
                FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = true);
            }
        }

        //void temp_old(int idperson, bool flag)
        //{

        //    List<int> test = FilteredSalesUsers.Where(i => i.IsChecked).Select(j => j.IdPerson).ToList();
        //    if (flag)
        //        test.Add(idperson);
        //    else
        //        test.Remove(idperson);
        //    SalesOwnerForPlant = string.Join(",", test);
        //    ObservableCollection<Site> plantList = new ObservableCollection<Site>();
        //    try
        //    {
        //        //CrmStartUp = new CrmServiceController("localhost:6699");
        //        if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatus == null)
        //        {
        //            // Only SelectedSource is not null
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", "0"));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));
        //        }
        //        else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatus == null)
        //        {
        //            // Only SelectedRegion is not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, "0"));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, "0"));
        //        }
        //        else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatus == null)
        //        {
        //            // Only SelectedCountry is not null
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", "0", country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, "0"));
        //        }
        //        #region chitra.girigosavi GEOS2-7242 10/04/2025
        //        else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatus != null)
        //        {
        //            // Only SelectedStatus is not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatus != null)
        //        {
        //            // SelectedSource and Selectedstatus are not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatus != null)
        //        {
        //            // SelectedRegion and SelectedStatus are not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatus != null)
        //        {
        //            // SelectedStatus and SelectedCountry are not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatus != null)
        //        {
        //            // SelectedSource, SelectedStatus and SelectedRegion are not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatus != null)
        //        {
        //            // SelectedSource, SelectedStatus and SelectedCountry are not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatus != null)
        //        {
        //            // SelectedRegion SelectedStatus and SelectedCountry are not null
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatus == null)
        //        {
        //            // SelectedSource, SelectedRegion and SelectedCountry are not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, "0"));
        //        }
        //        #endregion
        //        else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatus == null)
        //        {
        //            // SelectedSource and SelectedRegion are not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, "0"));
        //        }
        //        else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatus == null)
        //        {
        //            // SelectedSource and SelectedCountry are not null
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, "0"));
        //        }
        //        else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatus == null)
        //        {
        //            // SelectedRegion and SelectedCountry are not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, "0"));
        //        }
        //        else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatus != null)
        //        {
        //            // All are not null
        //            string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
        //            string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
        //            string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
        //            string status = SelectedStatus.IdLookupValue.ToString();
        //            //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, country));
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, status));
        //        }
        //        else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatus == null)
        //        {
        //            plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, "0"));
        //        }
        //        if ((SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SalesOwnerForPlant == "" && SelectedStatus == null))
        //        {
        //            FilteredCustomerSiteList = CustomerSiteList;
        //            bool allExists = FilteredCustomerSiteList.Any(x => x.IdSite == 0 && x.Name == "(ALL)");
        //            if (!allExists)
        //            {
        //                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
        //            }
        //            foreach (var item in FilteredCustomerSiteList)
        //            {
        //                item.IsChecked = false;
        //            }
        //        }
        //        else
        //        {
        //            FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(CustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdSite == item1.IdSite) || item1.IdSite == null));
        //            FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(FilteredCustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdGroup == item1.IdGroup)));

        //            foreach (var item in CustomerSiteList)
        //            {
        //                item.IsChecked = false;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FilterData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //    // Ensure checked items stay checked
        //    if (test.Count > 0)
        //    {
        //        FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = true);
        //    }
        //}
        private void SalesOwnerCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesOwnerCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                DevExpress.Xpf.Grid.TableView EventArg = (DevExpress.Xpf.Grid.TableView)obj;

                if (EventArg.ActiveEditor != null)
                {
                    isHandlingProgrammaticChange = true;

                    People CheckedSO = (People)((ObservableCollection<object>)(EventArg.SelectedRows)).FirstOrDefault();
                    if (CheckedSO.IdPerson == 0)
                    {
                        AllSaleOwner = false;
                        FilteredSalesUsers.ToList().ForEach(i => i.IsChecked = true);
                        FilteredCustomerSiteList.ToList().ForEach(site =>
                        {
                            if (site.SalesOwnerList != null && site.SalesOwnerList.Any(owner => FilteredSalesUsers.Any(user => user.IdPerson == owner.IdPerson)))
                            {
                                site.IsChecked = true;
                            }
                        });
                    }
                    else if (CheckedSO.SitesList != null)
                    {
                        temp(CheckedSO.IdPerson, true);
                    }
                    isHandlingProgrammaticChange = false;
                }
                GeosApplication.Instance.Logger.Log("Method SalesOwnerCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerCheckBoxSelectedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerCheckBoxSelectedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SalesOwnerUnCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SalesOwnerUnCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                DevExpress.Xpf.Grid.TableView EventArg = (DevExpress.Xpf.Grid.TableView)obj;
                if ((EventArg.ActiveEditor != null))
                {
                    isHandlingProgrammaticChange = true;
                    People CheckedSO = (People)((ObservableCollection<object>)(EventArg.SelectedRows)).FirstOrDefault();

                    #region (ALL)
                    if (CheckedSO.IdPerson == 0)
                    {
                        if (!AllSaleOwner)
                        {
                            AllSaleOwner = true;
                            FilteredCustomerSiteList.ToList().ForEach(site =>
                            {
                                if (site.SalesOwnerList != null && site.SalesOwnerList.Any(owner => FilteredSalesUsers.Any(user => user.IdPerson == owner.IdPerson)))
                                {
                                    site.IsChecked = false;
                                }
                            });
                            FilteredSalesUsers.ToList().ForEach(i => i.IsChecked = false);
                        }
                        else
                        {
                            isHandlingProgrammaticChange = false;
                            return;
                        }
                    }
                    #endregion
                    else
                    {
                        temp(CheckedSO.IdPerson, false);
                    }

                    isHandlingProgrammaticChange = false;
                }

                GeosApplication.Instance.Logger.Log("Method SalesOwnerUnCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerUnCheckBoxSelectedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerUnCheckBoxSelectedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SalesOwnerUnCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #region Commented
        //private void PlantCheckBoxSelectedCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PlantCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        DevExpress.Xpf.Grid.TreeListView EventArg = (DevExpress.Xpf.Grid.TreeListView)obj;

        //        if (EventArg.ActiveEditor != null)
        //        {
        //            SitesWithCustomer CheckSite = (SitesWithCustomer)((ObservableCollection<object>)(EventArg.SelectedRows)).FirstOrDefault();
        //            if (CheckSite.Name == "(ALL)")
        //            {
        //                FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = true);
        //                FilteredSalesUsers.ToList().ForEach(i => i.IsChecked = true);
        //            }
        //            if ((PlantMainCheck != CheckSite.IdSite))
        //            {
        //                PlantMainCheck = CheckSite.IdSite;
        //                List<SitesWithCustomer> CheckedSiteList = new List<SitesWithCustomer>();
        //                //If Parent Selected
        //                if (CheckSite.IdSite == null && CheckSite.IdGroup != 0)
        //                {
        //                    CheckedSiteList = FilteredCustomerSiteList.Where(i => i.IdGroup == CheckSite.IdGroup && i.IdSite != null).ToList();

        //                    #region to get all sites for the parent and sales owner for every site
        //                    foreach (var CheckedSite in CheckedSiteList)
        //                    {
        //                        if (CheckedSite.SalesOwnerList?.Count > 0)
        //                        {
        //                            FilteredSalesUsers.Where(i => CheckedSite.SalesOwnerList.Any(j => j.IdPerson == i.IdPerson)).ToList().ForEach(k => k.IsChecked = true);
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                //If Child Selected
        //                else
        //                {
        //                    CheckSite.IsChecked = true;
        //                    List<SitesWithCustomer> OnlySitesList = FilteredCustomerSiteList.Where(p => p.IdSite != 0 && p.IdSite != null).Where(i => i.IdGroup == CheckSite.IdGroup && i.IsChecked == false).ToList();
        //                    if ((OnlySitesList == null ? true : OnlySitesList.Count == 0))
        //                    {
        //                        FilteredCustomerSiteList.FirstOrDefault(i => i.IdGroup == CheckSite.IdGroup && (i.IdSite == 0 || i.IdSite == null)).IsChecked = true;
        //                    }

        //                    #region to get all sites for every sales owner
        //                    if (CheckSite.SalesOwnerList != null)
        //                        FilteredSalesUsers.Where(i => CheckSite.SalesOwnerList.Any(j => j.IdPerson == i.IdPerson)).ToList().ForEach(k => k.IsChecked = true);

        //                    #endregion
        //                }
        //                CheckedSiteList.ForEach(i => i.IsChecked = true);
        //            }
        //            if (PlantMainCheck == null || CheckSite.IdSite == PlantMainCheck)
        //                PlantMainCheck = 0;
        //        }
        //        GeosApplication.Instance.Logger.Log("Method PlantCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantCheckBoxSelectedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantCheckBoxSelectedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //private void PlantUnCheckBoxSelectedCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method PlantUnCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
        //        DevExpress.Xpf.Grid.TreeListView EventArg = (DevExpress.Xpf.Grid.TreeListView)obj;
        //        if (EventArg.ActiveEditor != null)
        //        {
        //            SitesWithCustomer CheckSite = (SitesWithCustomer)((ObservableCollection<object>)(EventArg.SelectedRows)).FirstOrDefault();
        //            #region (ALL)
        //            if (CheckSite.Name == "(ALL)")
        //            {
        //                if (AllPlant == false)
        //                {
        //                    FilteredSalesUsers.ToList().ForEach(i => i.IsChecked = false);
        //                    AllPlant = true;
        //                    FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = false);
        //                }
        //            }

        //            #endregion
        //            CheckSite.IsChecked = false;
        //            if (PlantMainUnCheck != ((CheckSite.IdSite == null) ? (int?)CheckSite.IdGroup : CheckSite.IdSite))
        //            {
        //                PlantMainUnCheck = ((CheckSite.IdSite == null) ? (int?)CheckSite.IdGroup : CheckSite.IdSite);
        //                List<SitesWithCustomer> CheckedSiteList = new List<SitesWithCustomer>();
        //                if (CheckSite.IdSite == null && CheckSite.IdGroup != 0)
        //                {
        //                    CheckedSiteList = FilteredCustomerSiteList.Where(i => i.IdGroup == CheckSite.IdGroup && i.IdSite != null).ToList();
        //                    CheckedSiteList.ForEach(i => i.IsChecked = false);
        //                }
        //                else
        //                {
        //                    CheckedSiteList.Add(CheckSite);

        //                    //To deselect treelist parent 
        //                    FilteredCustomerSiteList.FirstOrDefault(i => i.IdGroup == CheckSite.IdGroup && (i.IdSite == 0 || i.IdSite == null)).IsChecked = false;

        //                }
        //                //List<People> selectedSO = FilteredSalesUsers.Where(salesOwner => CheckedSiteList.Any(checkSite => checkSite.SalesOwnerList != null
        //                //                           && checkSite.SalesOwnerList.Any(user => user.IdPerson == salesOwner.IdPerson) && salesOwner.IsChecked)).ToList();

        //                foreach (People Checkedso in FilteredSalesUsers.Where(salesOwner => CheckedSiteList.Any(checkSite => checkSite.SalesOwnerList != null
        //                                            && checkSite.SalesOwnerList.Any(user => user.IdPerson == salesOwner.IdPerson) && salesOwner.IsChecked)).ToList())
        //                {
        //                    var test = FilteredCustomerSiteList.Where(i => Checkedso.SitesList.Any(j => j.IdSite == i.IdSite) && i.IsChecked).ToList();
        //                    if (test.Count == 0)
        //                        Checkedso.IsChecked = false;
        //                }
        //            }
        //            if (PlantMainUnCheck == null || ((CheckSite.IdSite == null) ? (int?)CheckSite.IdGroup : CheckSite.IdSite) == PlantMainUnCheck)
        //                PlantMainUnCheck = 0;

        //        }
        //        FilteredCustomerSiteList.FirstOrDefault(i => i.Name == "(ALL)").IsChecked = false;
        //        GeosApplication.Instance.Logger.Log("Method PlantUnCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantUnCheckBoxSelectedCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantUnCheckBoxSelectedCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in PlantUnCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //private void ShowPopupAsPerFirstName(string fName)
        //{
        //    GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName ...", category: Category.Info, priority: Priority.Low);
        //    var filteredTags = TagList.Where(t => t.Name.StartsWith(fName)).ToList();

        //    FilteredTags = new ObservableCollection<Tag>(filteredTags);

        //    GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName() executed successfully", category: Category.Info, priority: Priority.Low);
        //}
        #endregion
        private void plantSelectUnselectCommandAction(object obj)
        {
            if (!isHandlingProgrammaticChange)
            {
                TreeListNodeEventArgs selection = (TreeListNodeEventArgs)obj;
                SitesWithCustomer row = (SitesWithCustomer)selection.Row;
                if (row.Name == "(ALL)" && row.IsChecked == true)
                {
                    FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = true);
                    return;
                }
                if (row.Name == "(ALL)" && row.IsChecked == false)
                {
                    FilteredCustomerSiteList.ToList().ForEach(i => i.IsChecked = false);
                    return;
                }
                if (row.IsChecked == true && Convert.ToInt32(row.IdSite) != 0)
                {
                    #region to select sites for every sales owner
                    if (row.SalesOwnerList != null)
                        FilteredSalesUsers.Where(i => row.SalesOwnerList.Any(j => j.IdPerson == i.IdPerson)).ToList().ForEach(k => k.IsChecked = true);
                    #endregion
                    // To select (All)
                    if (FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList() != null && FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList()?.Count > 0)
                    {
                        if (!FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList().Any(i => i.IsChecked == false))
                            FilteredSalesUsers.FirstOrDefault(i => i.IdPerson == 0).IsChecked = true;
                    }

                }
                else if (row.IsChecked == false)
                {
                    if (Convert.ToInt32(row.IdSite) != 0)
                    {
                        #region to deselect sites for every sales owner
                        List<People> SalesUsersList = FilteredSalesUsers.Where(salesOwner => row.SalesOwnerList != null && row.SalesOwnerList.Any(user => user.IdPerson == salesOwner.IdPerson) && salesOwner.IsChecked).ToList();
                        foreach (People Checkedso in SalesUsersList)
                        {
                            if (Checkedso.SitesList != null)
                            {
                                var CheckedSiteExistFortheSO = FilteredCustomerSiteList.Where(i => Checkedso.SitesList.Any(j => j.IdSite == i.IdSite) && i.IsChecked).ToList();
                                if (CheckedSiteExistFortheSO.Count == 0)
                                {
                                    Checkedso.IsChecked = false;
                                }
                            }
                        }
                        #endregion
                        // To deselect (All)
                        if (FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList() != null && FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList()?.Count > 0)
                        {
                            if (FilteredSalesUsers.Where(j => j.IdPerson != 0).ToList().Any(i => i.IsChecked == false))
                                FilteredSalesUsers.FirstOrDefault(i => i.IdPerson == 0).IsChecked = false;
                        }
                    }
                }
            }
        }
        private void FillSites()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSites ...", category: Category.Info, priority: Priority.Low);
                //Service Method changed GetCustomerWithSites with GetCustomerWithSites_V2410 by [rdixit][GEOS2-4652][11.07.2023]
                //CrmStartUp = new CrmServiceController("localhost:6699");
                //CustomerSiteList = new ObservableCollection<SitesWithCustomer>(CrmStartUp.GetWithSiteswithSalesUsers());
                //[pramod.misal][GEOS2-8169][17.07.2025] updated from GetWithSiteswithSalesUsers to GetWithSiteswithSalesUsers_V2660
                CustomerSiteList = new ObservableCollection<SitesWithCustomer>(CrmStartUp.GetWithSiteswithSalesUsers_V2660());

                CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(CustomerSiteList.Select(item => (SitesWithCustomer)item.Clone()));
                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                GeosApplication.Instance.Logger.Log("Method FillSites() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSites() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSites() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }
        private void FillUsers()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillUsers ...", category: Category.Info, priority: Priority.Low);
                ListSalesUsers = new ObservableCollection<People>(CrmStartUp.GetAllActivePeoplesWithSites());
                FilteredSalesUsers = new ObservableCollection<People>(ListSalesUsers.Select(item => (People)item.Clone()));
                FilteredSalesUsers.Insert(0, new People() { IsChecked = false, Name = "(ALL)", IdPerson = 0 });
                GeosApplication.Instance.Logger.Log("Method FillUsers() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillUsers() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
        }

        private void FillReporterList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillReporterList ...", category: Category.Info, priority: Priority.Low);
                ReporterList = new ObservableCollection<User>(CrmStartUp.GetSalesAndCommericalUsers());
                ReporterList.Insert(0, new User() { FirstName = "", LastName = "", IdUser = 0 });
                SelectedActionReporter = ReporterList.FirstOrDefault();
                SelectedReporter = 0;
                GeosApplication.Instance.Logger.Log("Method FillReporterList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

                string CountryNames = "0";

                if (SelectedCountry == null)
                    SelectedCountry = new List<object>();

                foreach (Country country in SelectedCountry)
                {
                    if (CountryNames == "0")
                        CountryNames = country.Name;
                    else
                        CountryNames = CountryNames + "," + country.Name;
                }

                List<Regions> SelectedRegionList = new List<Regions>(CrmStartUp.GetRegionsByGroupAndCountryAndSites(0, CountryNames, "0"));

                SelectedRegion = new List<object>();
                foreach (Regions reg in SelectedRegionList)
                {
                    SelectedRegion.Add(RegionList.FirstOrDefault(a => a.IdRegion == reg.IdRegion));
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
        //[GEOS2-6446][05.11.2024][rdixit]
        private void FillSourceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSourceList ...", category: Category.Info, priority: Priority.Low);
                SourceList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(126));
                GeosApplication.Instance.Logger.Log("Method FillSourceList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSourceList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeRegionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    //[GEOS2-6446][05.11.2024][rdixit]
                    if (SelectedRegion != null)
                    {
                        string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.RegionName));
                        CountryList = new ObservableCollection<Country>(CrmStartUp.GetCountriesByGroupAndRegionAndSites(0, (string.IsNullOrWhiteSpace(regions) ? "0" : regions), "0"));
                    }
                    ObservableCollection<Site> plantList = new ObservableCollection<Site>();
                    if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null)
                    {
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                        foreach (var item in FilteredSalesUsers)
                        {
                            item.IsChecked = false;
                        }
                        FilteredCustomerSiteList = CustomerSiteList;
                        FilteredSalesUsers = ListSalesUsers;
                    }
                    else
                    {
                        SalesOwnerForPlant = "0";
                        FilterData();
                        if (FilteredCustomerSiteList != null)
                        {
                            if (!FilteredSalesUsers.Any(i => i.Name == "(ALL)"))
                                FilteredSalesUsers.Insert(0, new People() { IsChecked = false, Name = "(ALL)", IdPerson = 0 });
                            if (!FilteredCustomerSiteList.Any(i => i.Name == "(ALL)"))
                                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                            foreach (var item in FilteredCustomerSiteList)
                            {
                                item.IsChecked = false;
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeRegionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeRegionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeRegionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeRegionCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeCountryCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    //[GEOS2-6446][05.11.2024][rdixit]
                    if (SelectedCountry != null)
                        FillRegions();
                    ObservableCollection<Site> plantList = new ObservableCollection<Site>();
                    if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null)
                    {
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                        foreach (var item in FilteredSalesUsers)
                        {
                            item.IsChecked = false;
                        }
                        FilteredCustomerSiteList = CustomerSiteList;
                        FilteredSalesUsers = ListSalesUsers;
                    }
                    else
                    {
                        FilterData();
                        if (FilteredCustomerSiteList != null)
                        {
                            if (!FilteredSalesUsers.Any(i => i.Name == "(ALL)"))
                                FilteredSalesUsers.Insert(0, new People() { IsChecked = false, Name = "(ALL)", IdPerson = 0 });
                            if (!FilteredCustomerSiteList.Any(i => i.Name == "(ALL)"))
                                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeCountryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCountryCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCountryCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeCountryCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeSourceCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeSourceCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    //[GEOS2-6446][05.11.2024][rdixit]
                    ObservableCollection<Site> plantList = new ObservableCollection<Site>();
                    if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null)
                    {
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                        foreach (var item in FilteredSalesUsers)
                        {
                            item.IsChecked = false;
                        }
                        FilteredCustomerSiteList = CustomerSiteList;
                        FilteredSalesUsers = ListSalesUsers;
                    }
                    else
                    {
                        SalesOwnerForPlant = "0";
                        FilterData();
                        if (FilteredCustomerSiteList != null)
                        {
                            if (!FilteredSalesUsers.Any(i => i.Name == "(ALL)"))
                                FilteredSalesUsers.Insert(0, new People() { IsChecked = false, Name = "(ALL)", IdPerson = 0 });
                            if (!FilteredCustomerSiteList.Any(i => i.Name == "(ALL)"))
                                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                        }
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeSourceCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        void FilterData()   //[GEOS2-6446][05.11.2024][rdixit]
        {
            ObservableCollection<Site> plantList = new ObservableCollection<Site>();
            try
            {
                #region
                //if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null)
                //{
                //    // Only SelectedSource is not null
                //    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", "0"));
                //}
                //else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null)
                //{
                //    // Only SelectedRegion is not null
                //    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, "0"));
                //}
                //else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null)
                //{
                //    // Only SelectedCountry is not null
                //    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", "0", country));
                //}
                //else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null)
                //{
                //    // SelectedSource and SelectedRegion are not null
                //    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                //    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                //}
                //else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null)
                //{
                //    // SelectedSource and SelectedCountry are not null
                //    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                //    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                //}
                //else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null)
                //{
                //    // SelectedRegion and SelectedCountry are not null
                //    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                //    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                //}
                //else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null)
                //{
                //    // All are not null
                //    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                //    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                //    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                //    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, country));
                //}
                #endregion
                //CrmStartUp = new CrmServiceController("localhost:6699");
                if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // Only SelectedSource is not null
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // Only SelectedRegion is not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, "0", SalesOwnerForPlant, "0"));

                   

                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // Only SelectedCountry is not null
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", country, SalesOwnerForPlant, "0"));          

                }
                #region chitra.girigosavi GEOS2-7242 10/04/2025
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // Only SelectedStatus is not null
                    //string status = SelectedStatuses.IdLookupValue.ToString();
                  
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    //string status = string.Join(",", SelectedStatuses.Cast<SitesWithCustomer>().Select(r => r.IdStatus));
                    //CrmStartUp = new CrmServiceController("localhost:6699");
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", "0", SalesOwnerForPlant, status));


                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedSource and Selectedstatus are not null
                   // string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedRegion and SelectedStatus are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());

                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, "0", SalesOwnerForPlant, status));


                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedStatus and SelectedCountry are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", country, SalesOwnerForPlant, status));


                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses != null)
                {
                    // SelectedSource, SelectedStatus and SelectedRegion are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedSource, SelectedStatus and SelectedCountry are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // SelectedRegion SelectedStatus and SelectedCountry are not null
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, country, SalesOwnerForPlant, status));


                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedSource, SelectedRegion and SelectedCountry are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                #endregion
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry == null && SelectedStatuses == null)
                {
                    // SelectedSource and SelectedRegion are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, "0"));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource != null && SelectedRegion == null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedSource and SelectedCountry are not null
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, "0", country));
                   // plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, "0", country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, "0", "0", SalesOwnerForPlant, "0"));

                }
                else if (SelectedSource == null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses == null)
                {
                    // SelectedRegion and SelectedCountry are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580("0", regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", regions, country, SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", regions, country, SalesOwnerForPlant, "0"));


                }
                else if (SelectedSource != null && SelectedRegion != null && SelectedCountry != null && SelectedStatuses != null)
                {
                    // All are not null
                    string regions = string.Join(",", SelectedRegion.Cast<Regions>().Select(r => r.IdRegion));
                    string country = string.Join(",", SelectedCountry.Cast<Country>().Select(c => c.IdCountry));
                    string sourceIdList = string.Join(",", SelectedSource.Cast<int>());
                    //string status = SelectedStatus.IdLookupValue.ToString();
                    string status = string.Join(",", SelectedStatuses.Cast<int>());


                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2580(sourceIdList, regions, country));
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630(sourceIdList, regions, country, SalesOwnerForPlant, status));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660(sourceIdList, regions, country, SalesOwnerForPlant, status));

                }
                else if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses == null)
                {
                    //plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2630("0", "0", "0", SalesOwnerForPlant, "0"));
                    //[pramod.misal][GEOS2-8169][17.07.2025]
                    plantList = new ObservableCollection<Site>(CrmStartUp.GetPlantsByIdSourceList_V2660("0", "0", "0", SalesOwnerForPlant, "0"));

                }
                FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(CustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdSite == item1.IdSite) || item1.IdSite == null));
                FilteredCustomerSiteList = new ObservableCollection<SitesWithCustomer>(FilteredCustomerSiteList.Where(item1 => plantList.Any(item2 => item2.IdGroup == item1.IdGroup)));
                FilterSalesOwner(FilteredCustomerSiteList);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterData() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[GEOS2-6446][05.11.2024][rdixit]
        void FilterSalesOwner(ObservableCollection<SitesWithCustomer> SitesWithCustomer)
        {
            try
            {
                if (SitesWithCustomer != null)
                {
                    string distinctSiteIds = string.Join(",", (SitesWithCustomer?.Where(i => i != null && i.IdSite != null && i.IdSite != 0)
                        .Select(i => i.IdSite.Value).Distinct().Select(id => id.ToString()) ?? Enumerable.Empty<string>()));

                    //service GetActivePeoplesBySiteList updated with GetActivePeoplesBySiteList_V2410 by [rdixit][25.07.2023]
                    var SalesOwnerwithSites = ListSalesUsers.Where(i => i.SitesList != null && i.SitesList?.Count > 0);
                    //FilteredSalesUsers = new ObservableCollection<People>(CrmStartUp.GetActivePeoplesBySiteList_V2410(distinctSiteIds));
                    FilteredSalesUsers = new ObservableCollection<People>(SalesOwnerwithSites.Where(i => i.SitesList.Any(j => FilteredCustomerSiteList.Any(k => k.IdSite == j.IdSite))).ToList());
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterSalesOwner() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FilterSalesOwner() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FilterSalesOwner() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rdixit][GEOS2-4651][11.07.2023]
        private void ExpandAndCollapsCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            if (IsExpand)
            {
                t.ExpandAllNodes();
                IsExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsExpand = true;
            }
        }

        private void FillTagList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTagList ...", category: Category.Info, priority: Priority.Low);
                TagList = new ObservableCollection<Tag>(CrmStartUp.GetAllTags().OrderBy(r => r.Name).ToList());
                FilteredTags = new ObservableCollection<Tag>(TagList.Select(i => (Tag)i.Clone()).ToList());
                GeosApplication.Instance.Logger.Log("Method FillTagList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillTagList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in FillTagList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in FillReporterList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OnEditValueChanging(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnEditValueChanging ...", category: Category.Info, priority: Priority.Low);
                var newInput = (string)e.NewValue;
                var filteredTags = TagList.Where(t => t.Name.ToLower().Contains(newInput?.ToLower())).ToList(); //[GEOS2-4734][07.08.2023][rdixit]
                FilteredTags = new ObservableCollection<Tag>(filteredTags);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in OnEditValueChanging() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #region[pramod.misal][GEOS2-8169][17.07.2025]
        //[pramod.misal][GEOS2-8169][17.07.2025]
        private void FillStatusList(ObservableCollection<SitesWithCustomer> FilteredCustomerSiteList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());             
                IList<LookupValue> tempStatusList = CrmStartUp.GetLookupValues(177);
                var validStatusIds = new HashSet<int>(FilteredCustomerSiteList.Where(site => site != null).Select(site => site.IdStatus).Distinct());              
                var filteredStatusList = tempStatusList.Where(status => validStatusIds.Contains(status.IdLookupValue)).ToList();
                StatusList = new ObservableCollection<LookupValue>(filteredStatusList);
                SelectedStatusIndex = -1;

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void ChangeStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeSourceCommandAction()...", category: Category.Info, priority: Priority.Low);
              
                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal)
                {
                    //SelectedStatusIndex = StatusList.IndexOf(StatusList.FirstOrDefault(i = > i.))
                    ObservableCollection<Site> plantList = new ObservableCollection<Site>();
                    if (SelectedSource == null && SelectedRegion == null && SelectedCountry == null && SelectedStatuses == null)
                    {
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                        foreach (var item in FilteredSalesUsers)
                        {
                            item.IsChecked = false;
                        }
                        FilteredCustomerSiteList = CustomerSiteList;
                        FilteredSalesUsers = ListSalesUsers;
                    }
                    else
                    {
                        SalesOwnerForPlant = "0";
                        FilterData();
                        if (FilteredCustomerSiteList != null)
                        {
                            if (!FilteredSalesUsers.Any(i => i.Name == "(ALL)"))
                                FilteredSalesUsers.Insert(0, new People() { IsChecked = false, Name = "(ALL)", IdPerson = 0 });
                            if (!FilteredCustomerSiteList.Any(i => i.Name == "(ALL)"))
                                FilteredCustomerSiteList.Insert(0, new SitesWithCustomer() { IsChecked = false, Name = "(ALL)", IdSite = 0, Key = "(ALL)", Parent = "(ALL)" });
                        }
                        foreach (var item in FilteredCustomerSiteList)
                        {
                            item.IsChecked = false;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeSourceCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ChangeSourceCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion

        #region Validation



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
                    me[BindableBase.GetPropertyName(() => Subject)] +
                    me[BindableBase.GetPropertyName(() => SelectedActionReporter)] +
                    me[BindableBase.GetPropertyName(() => DueDate)];
                    //+
                    //me[BindableBase.GetPropertyName(() => SelectedStatusIndex)];//chitra.girigosavi[GEOS2-7242][14/04/2025]

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
                string DueDateProp = BindableBase.GetPropertyName(() => DueDate);
                string SubjectProp = BindableBase.GetPropertyName(() => Subject);
                string SelectedReporterProp = BindableBase.GetPropertyName(() => SelectedReporter);
                //string SelectedStatusIndexProp = BindableBase.GetPropertyName(() => SelectedStatusIndex); //chitra.girigosavi[GEOS2-7242][14/04/2025]

                if (ShowValidation)
                {
                    if (columnName == SelectedReporterProp || columnName == "SelectedActionReporter")
                        return ActionValidation.GetErrorMessage(SelectedReporterProp, SelectedReporter);

                    if (columnName == SubjectProp)
                        return ActionValidation.GetErrorMessage(SubjectProp, Subject);

                    if (columnName == DueDateProp)
                        return ActionValidation.GetErrorMessage(DueDateProp, DueDate);
                    //chitra.girigosavi[GEOS2-7242][14/04/2025]
                    //if (columnName == SelectedStatusIndexProp)
                    //    return ActionValidation.GetErrorMessage(SelectedStatusIndexProp, SelectedStatusIndex);
                }
                return null;
            }
        }
        #endregion
    }
}
