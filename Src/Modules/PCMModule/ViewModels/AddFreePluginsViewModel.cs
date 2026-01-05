using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
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
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.PCM;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Validations;

namespace Emdep.Geos.Modules.PCM.ViewModels
{

    //[PRAMOD.MISAL][GEOS2-4443][29-08-2023]
    public class AddFreePluginsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {

        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        // IPCMService PCMService = new PCMServiceController("localhost:6699");
        // IPLMService PLMService = new PLMServiceController("localhost:6699");
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
        FreePlugins selectedItem;
        private bool isNew;
        private bool isSave;
        private string windowHeader;
        private ObservableCollection<Group> groupList;
        private Group selectedGroup;
        private FreePlugins selectedFreePluginName;
        private ObservableCollection<Region> regionList;
        private List<object> selectedCountry;
        private List<object> selectedPlant;
        private List<object> combinedSaveList;
        private int isFilterStatus;
        private List<object> selectedRegion;
        private ObservableCollection<Country> countryList;
        private ObservableCollection<Site> plantList;
        private string informationError;
        private ObservableCollection<FreePlugins> freePluginNameslist;
        private List<Site> selectedPlant_Save;
        private Site selectedPlant_Edit;
        private Country selectedCountry_edit;

        private string selectedPluginsName_Save;
        private Region selectedRegion_Edit;
        private List<Group> selectedGroup_Save;
        private long idplugin;
        private FreePlugins newFreePluginsList;
        private FreePlugins updateFreePluginsList;
        private string error = string.Empty;

        private ObservableCollection<FreePlugins> freePluginGridList;
        public int IdRegion { get; set; }
        public int IdCountry { get; set; }
        public uint IdPlant { get; set; }

        public FreePlugins NewFreeplugins { get; set; }

        private Int64 idRegionPrevious { get; set; }
        private Int64 idCountryPrevious { get; set; }
        private Int64 idPlantPrevious { get; set; }

        private bool isRetrive;
        #endregion


        #region Property       
        public FreePlugins SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
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


        public long Idplugin
        {
            get
            {
                return idplugin;
            }
            set
            {
                idplugin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Idplugin"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
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

        public ObservableCollection<FreePlugins> FreePluginNamesList
        {
            get
            {
                return freePluginNameslist;
            }

            set
            {
                freePluginNameslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FreePluginNamesList"));
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

        //public List<object> SelectedGroup
        //{
        //    get
        //    {
        //        return selectedGroup;
        //    }
        //    set
        //    {
        //        selectedGroup = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup"));
        //    }
        //}

        public FreePlugins SelectedFreePluginName
        {
            get
            {
                return selectedFreePluginName;
            }
            set
            {
                selectedFreePluginName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFreePluginName"));
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
                //OnPropertyChanged(nameof(SelectedRegion));
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

        public Country SelectedCountry_Edit
        {
            get
            {
                return selectedCountry_edit;
            }

            set
            {
                selectedCountry_edit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry_Edit"));
            }
        }

        public string SelectedPluginsName_Save
        {
            get
            {
                return selectedPluginsName_Save;
            }

            set
            {
                selectedPluginsName_Save = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPluginsName_Save"));
            }
        }

        public Region SelectedRegion_Edit
        {
            get
            {
                return selectedRegion_Edit;
            }

            set
            {
                selectedRegion_Edit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRegion_Edit"));
            }
        }

        public List<object> CombinedSaveList
        {
            get
            {
                return combinedSaveList;
            }

            set
            {
                combinedSaveList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CombinedSaveList"));
            }
        }

        public List<Group> SelectedGroup_Save
        {
            get
            {
                return selectedGroup_Save;
            }

            set
            {
                selectedGroup_Save = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGroup_Save"));
            }
        }

        public FreePlugins NewFreePluginsList
        {
            get
            {
                return newFreePluginsList;
            }
            set
            {
                newFreePluginsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewFreePluginsList"));
            }
        }
        public FreePlugins UpdateFreePluginsList
        {
            get { return updateFreePluginsList; }
            set
            {
                updateFreePluginsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateFreePluginsList"));
            }
        }
        public bool Isadd { get; set; }
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

        public ObservableCollection<FreePlugins> FreePluginGridList
        {
            get { return freePluginGridList; }
            set
            {
                freePluginGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FreePluginGridList"));
            }
        }

        public bool IsRetrive
        {
            get { return isRetrive; }
            set
            {
                isRetrive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRetrive"));
            }
        }

        public FreePlugins SelectedFreePlugins { get; set; }
        public FreePlugins NewfreePlugins { get; set; }

        bool isEdit =false;
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }

        private ObservableCollection<Site> allPlantList;
        public ObservableCollection<Site> AllPlantList
        {
            get
            {
                return allPlantList;
            }

            set
            {
                allPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantList"));
            }
        }

        public ObservableCollection<FreePlugins> FinalfreePlugins { get; set; }
        #endregion


        #region ICommand

        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand SelectedGroupIndexChangedCommand { get; set; }
        public ICommand ChangeRegionCommand { get; set; }
        public ICommand ChangeCountryCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand AcceptAddFreePluginsActionCommand { get; set; }






        #endregion


        #region Constructor
        public AddFreePluginsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditPCMArticleImageViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptAddFreePluginsActionCommand = new DelegateCommand<object>(SaveFreePluginsActionNew);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                SelectedGroupIndexChangedCommand = new DelegateCommand<object>(SelectedGroupIndexChangedCommandAction);
                ChangeRegionCommand = new DelegateCommand<object>(ChangeRegionCommandAction);
                ChangeCountryCommand = new DelegateCommand<object>(ChangeCountryCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor AddEditCPLCustomerViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
                FillRegionCountryplnat();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditCPLCustomerViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion



        #region Methods


        public void FillRegionCountryplnat()
        {
            RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(0, "0", "0"));
            CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(0, "0", "0"));
            PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(0, "0", "0"));
            #region RemoveCountryNameFromPlantList
            foreach (Site itemPlant in PlantList)
            {
                try
                {
                    string[] parts = itemPlant.Name.Split('(');
                    itemPlant.Name = parts[0];
                }
                catch (Exception) { }
            }
            #endregion

        }
        public void Init()
        {
            try
            {
                FillGroups();
                FillpluginsNames();
                AllPlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountries() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillpluginsNames()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillpluginsName() ...", category: Category.Info, priority: Priority.Low);

                FreePluginNamesList = new ObservableCollection<FreePlugins>(PCMService.GetHardlockFreePluginNames());

                FreePluginNamesList.Insert(0, new FreePlugins() { Name = "---", IdPlugin = 0 });

                //FreePluginNamesList.Select(fp => fp.Name).Distinct().ToList();

                SelectedFreePluginName = FreePluginNamesList.FirstOrDefault();

                //FreePluginNamesList.Select(s => s.Name.Distinct());

                GeosApplication.Instance.Logger.Log("Method FillpluginsName() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillpluginsName() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillpluginsName() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillpluginsName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-4443][31-08-2023]
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

        //[pramod.misal][GEOS2-4443][31-08-2023]
        private void FillRegions()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegions ...", category: Category.Info, priority: Priority.Low);

                string CountryNames = "0";
                string SiteNames = "0";
                string NameBySiteId = "0";
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

                        try
                        {
                            if (NameBySiteId == "0")
                                NameBySiteId = AllPlantList.Where(w => w.IdSite == site.IdSite).FirstOrDefault().Name;
                            else
                                NameBySiteId = NameBySiteId + "," + AllPlantList.Where(w => w.IdSite == site.IdSite).FirstOrDefault().Name;
                        }
                        catch (Exception ex)  {  }
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Region> SelectedRegionList = new List<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, CountryNames, NameBySiteId));
                SelectedRegion = new List<object>();
                foreach (Region reg in SelectedRegionList)
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

        //[pramod.misal][GEOS2-4443][31-08-2023]
        private void FillCountries()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountries ...", category: Category.Info, priority: Priority.Low);

                string RegionNames = "0";
                string SiteNames = "0";
                string NameBySiteId = "0";
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

                        try
                        {
                            if (NameBySiteId == "0")
                                NameBySiteId = AllPlantList.Where(w => w.IdSite == site.IdSite).FirstOrDefault().Name;
                            else
                                NameBySiteId = NameBySiteId + "," + AllPlantList.Where(w => w.IdSite == site.IdSite).FirstOrDefault().Name;
                        }
                        catch (Exception ex){}
                    }
                }
                else
                {
                    SiteNames = "0";
                }

                List<Country> SelectedCountryList = new List<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, RegionNames, NameBySiteId));

                SelectedCountry = new List<object>();
                foreach (Country cntry in SelectedCountryList)
                {
                    SelectedCountry.Add(CountryList.FirstOrDefault(a => a.IdCountry == cntry.IdCountry));
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
        //[pramod.misal][GEOS2-4443][31-08-2023]
        //private void FillPlants()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method FillPlants ...", category: Category.Info, priority: Priority.Low);

        //        string RegionNames = "0";
        //        string CountryNames = "0";
        //        if (SelectedRegion == null)
        //            SelectedRegion = new List<object>();

        //        if (SelectedCountry == null)
        //            SelectedCountry = new List<object>();

        //        if (isFilterStatus == 1)
        //        {
        //            foreach (Region Region in SelectedRegion)
        //            {
        //                if (RegionNames == "0")
        //                    RegionNames = Region.RegionName;
        //                else
        //                    RegionNames = RegionNames + "," + Region.RegionName;
        //            }
        //        }
        //        else
        //        {
        //            RegionNames = "0";
        //        }

        //        if (IsFilterStatus == 2)
        //        {
        //            foreach (Country country in SelectedCountry)
        //            {
        //                if (CountryNames == "0")
        //                    CountryNames = country.Name;
        //                else
        //                    CountryNames = CountryNames + "," + country.Name;
        //            }
        //        }
        //        else
        //        {
        //            CountryNames = "0";
        //        }


        //        List<Site> SelectedPlantList = new List<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, RegionNames, CountryNames));              
        //        SelectedPlant = new List<object>();
        //        foreach (Site plnt in SelectedPlantList)
        //        {
        //            SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plnt.IdSite));
        //        }
        //        SelectedPlant = new List<object>(SelectedPlant);
        //        GeosApplication.Instance.Logger.Log("Method FillPlants() executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (FaultException<ServiceException> ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
        //        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
        //    }
        //    catch (ServiceUnexceptedException ex)
        //    {
        //        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in FillPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

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
                    #region RemoveCountryNameFromPlantList
                    foreach (Site itemPlant in PlantList)
                    {
                        try
                        {
                            string[] parts = itemPlant.Name.Split('(');
                            itemPlant.Name = parts[0];
                        }
                        catch (Exception) { }
                    }
                    #endregion
                }
                else
                {
                    RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2110(SelectedGroup.IdGroup, "0", "0"));
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));
                    #region RemoveCountryNameFromPlantList
                    foreach (Site itemPlant in PlantList)
                    {
                        try
                        {
                            string[] parts = itemPlant.Name.Split('(');
                            itemPlant.Name = parts[0];
                        }
                        catch (Exception) { }
                    }
                    #endregion


                    // RegionList.Insert(0, new Region {RegionName="ALL"});
                    // CountryList.Insert(0, new Country { Name = "ALL" });
                    //  PlantList.Insert(0, new Site { Name = "ALL" });

                    if (SelectedItem.SelectedRegion == null && SelectedItem.Region.Equals("All", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedRegion = new List<object>();
                        //if (RegionList.Count!=1)
                        //foreach (var item in RegionList)
                        //{
                        //    SelectedRegion.Add(item);
                        //}
                        SelectedRegion = new List<object>(SelectedRegion);
                        idRegionPrevious = 0;
                    }
                    else
                    {
                        SelectedRegion = new List<object>();
                        SelectedRegion.Add(RegionList.FirstOrDefault(a => a.RegionName.ToLower().Contains(SelectedItem.Region.ToLower())));
                        SelectedRegion = new List<object>(SelectedRegion);
                        idRegionPrevious = SelectedItem.IdRegion;
                    }

                    if (SelectedItem.SelectedCountry == null && SelectedItem.Country.Equals("All", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedCountry = new List<object>();
						//if (CountryList.Count != 1)
                        //foreach (var item in CountryList)
                        //{
                        //    SelectedCountry.Add(item);
                        //}
                        SelectedCountry = new List<object>(SelectedCountry);
                        idCountryPrevious = 0;
                    }
                    else
                    {
                        SelectedCountry = new List<object>();
                        SelectedCountry.Add(countryList.FirstOrDefault(a => a.Name.ToLower().Contains(SelectedItem.Country.ToLower())));
                        SelectedCountry = new List<object>(SelectedCountry);
                        idCountryPrevious = SelectedItem.IdCountry;
                    }


                    if (SelectedItem.SelectedPlant == null && SelectedItem.Plant.Equals("All", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectedPlant = new List<object>();
						//if (CountryList.Count != 1)
                        //foreach (var item in PlantList)
                        //{
                        //    SelectedPlant.Add(item);
                        //}
                        SelectedPlant = new List<object>(SelectedPlant);
                        idPlantPrevious = 0;
                    }
                    else
                    {
                        SelectedPlant = new List<object>();
                        SelectedPlant.Add(plantList.FirstOrDefault(a => a.Name.ToLower().Contains(SelectedItem.Plant.ToLower())));
                        SelectedPlant = new List<object>(SelectedPlant);
                        idPlantPrevious = SelectedItem.IdSite;
                    }
                    IsRetrive = false;
                }



                //try
                //{
                //    #region GEOS2-4443
                //    //Shubham[skadam]  Management of hardlock licenses in PCM (4/5) 09 10 2023
                //    try
                //    {
                //        if (IsEdit)
                //        {
                //            if (SelectedFreePlugins != null)
                //            {
                //                RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(Convert.ToInt32(SelectedFreePlugins.IdCustomer), "0", "0"));
                //                CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2350(Convert.ToInt32(SelectedFreePlugins.IdCustomer), "0", "0"));
                //                PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(Convert.ToInt32(SelectedFreePlugins.IdCustomer), "0", "0"));
                //                SelectedFreePluginName.Name = SelectedItem.Name;

                //                SelectedGroup = GroupList.Where(s => s.IdGroup == SelectedItem.IdCustomer).FirstOrDefault();
                //                SelectedRegion = new List<object>(RegionList.Where(s => s.IdRegion == SelectedFreePlugins.IdRegion));
                //                SelectedCountry = new List<object>(CountryList.Where(s => s.IdCountry == SelectedFreePlugins.IdCountry));
                //                SelectedPlant = new List<object>(PlantList.Where(s => s.IdSite == SelectedFreePlugins.IdSite));
                //            }
                //            IsEdit = false;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //    #endregion
                //}
                //catch (Exception ex)
                //{
                //}


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error AddEditCPLCustomerViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

                        FillRegions();
                    }
                    else
                    {
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

        //chitra[cgirigosavi][GEOS2-4443][15/09/2023]
        private void SaveFreePluginsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                //allowValidation = true;

                //if (SelectedArticleList.Reference.ToString().Equals("---"))
                //{
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedFreePluginName"));
                if (error != null)
                {
                    return;
                }
                List<Region> selectedRegion = new List<Region>();
                List<Country> selectedCountry = new List<Country>();
                List<Site> selectedPlant = new List<Site>();
                if (SelectedRegion != null && (!SelectedRegion.All(x => x == null)))
                {
                    selectedRegion = SelectedRegion.Cast<Region>().ToList();
                }
                if (SelectedCountry != null && (!SelectedCountry.All(x => x == null)))
                {
                    selectedCountry = SelectedCountry.Cast<Country>().ToList();
                }
                if (SelectedPlant != null && (!SelectedPlant.All(x => x == null)))
                {
                    selectedPlant = SelectedPlant.Cast<Site>().ToList();
                }


                if (selectedRegion.Count == RegionList.Count)
                {
                    IdRegion = 0;
                }
                else
                {
                    foreach (var item in selectedRegion)
                    {
                        IdRegion = item.IdRegion;
                    }
                    // IdRegion = selectedRegion.FirstOrDefault().IdRegion;
                }
                if (selectedCountry.Count == CountryList.Count)
                {
                    IdCountry = 0;
                }
                else
                {
                    foreach (var item in selectedCountry)
                    {
                        IdCountry = item.IdCountry;
                    }
                    // IdCountry = selectedCountry.FirstOrDefault().IdCountry;
                }
                if (selectedPlant.Count == PlantList.Count)
                {
                    IdPlant = 0;
                }
                else
                {
                    foreach (var item in selectedPlant)
                    {
                        IdPlant = item.IdSite;
                    }
                    //IdPlant = selectedPlant.FirstOrDefault().IdSite;
                }

                FreePlugins tempData = new FreePlugins();

                tempData = FreePluginGridList.FirstOrDefault(x => x.IdPlugin == SelectedFreePluginName.IdPlugin && x.Group == SelectedGroup.GroupName && x.IdRegion == IdRegion &&
                  x.IdCountry == IdCountry && x.IdSite == IdPlant);
                if (tempData == null)
                {
                    if (IsNew)
                    {



                        NewFreePluginsList = new FreePlugins();
                        NewFreePluginsList.TransactionOperation = ModelBase.TransactionOperations.Add;
                        NewFreePluginsList.Name = SelectedFreePluginName.Name;
                        NewFreePluginsList.Group = SelectedGroup.GroupName;
                        NewFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;
                        NewFreePluginsList.IdCustomer = SelectedGroup.IdGroup;



                        if (SelectedFreePluginName != null)
                        {

                            NewFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;

                        }
                        if (SelectedRegion != null && SelectedRegion.Count > 0)
                        {
                            if (SelectedRegion.Count == RegionList.Count && RegionList.Count > 1 || RegionList.Count == 1)
                            {
                                SelectedRegion = null;
                            }
                            else
                            {
                                //Region
                                if (SelectedRegion != null)
                                {
                                    // List<Region> selected = SelectedRegion.Cast<Region>().ToList();
                                    foreach (var item in selectedRegion)
                                    {
                                        if (NewFreePluginsList.SelectedRegion == null)
                                            NewFreePluginsList.SelectedRegion = new List<int>();
                                        NewFreePluginsList.SelectedRegion.Add(item.IdRegion);
                                        NewFreePluginsList.IdRegion = item.IdRegion;
                                        // var temp = RegionList.Where(x => x.IdRegion == item.IdRegion);
                                        //NewFreePluginsList.Region = temp.ToString();
                                    }

                                }
                            }
                        }
                        else
                        {
                            SelectedRegion = null;
                        }

                        if (SelectedCountry != null && SelectedCountry.Count > 0)
                        {
                            if (SelectedCountry.Count == CountryList.Count && CountryList.Count > 1 || CountryList.Count == 1)
                            {
                                SelectedCountry = null;
                            }
                            else
                            {
                                //Country
                                if (SelectedCountry != null)
                                {
                                    //  List<Country> selected = SelectedCountry.Cast<Country>().ToList();
                                    foreach (var item in selectedCountry)
                                    {
                                        if (NewFreePluginsList.SelectedCountry == null)
                                            NewFreePluginsList.SelectedCountry = new List<int>();
                                        NewFreePluginsList.SelectedCountry.Add(item.IdCountry);
                                        NewFreePluginsList.IdCountry = item.IdCountry;
                                    }
                                }
                            }
                        }
                        else
                        {
                            SelectedCountry = null;
                        }

                        if (SelectedPlant != null && SelectedPlant.Count > 0)
                        {
                            if (SelectedPlant.Count == PlantList.Count && PlantList.Count > 1 || PlantList.Count == 1)
                            {
                                SelectedPlant = null;
                            }
                            else
                            {
                                //Plant
                                if (SelectedPlant != null)
                                {
                                    // List<Site> selected = SelectedPlant.Cast<Site>().ToList();
                                    foreach (var item in selectedPlant)
                                    {
                                        if (NewFreePluginsList.SelectedPlant == null)
                                            NewFreePluginsList.SelectedPlant = new List<UInt32>();
                                        NewFreePluginsList.SelectedPlant.Add(item.IdSite);
                                        NewFreePluginsList.IdSite = item.IdSite;
                                    }
                                }
                            }
                        }
                        else
                        {
                            SelectedPlant = null;
                        }




                        NewFreePluginsList = PCMService.AddUpdateFreePlugins(NewFreePluginsList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FreePluginAddedSuccessful").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        IsSave = true;
                        RequestClose(null, null);

                    }
                    else
                    {
                        UpdateFreePluginsList = new FreePlugins();
                        UpdateFreePluginsList.TransactionOperation = ModelBase.TransactionOperations.Update;
                        UpdateFreePluginsList.Name = SelectedFreePluginName.Name;
                        UpdateFreePluginsList.Group = SelectedGroup.GroupName;
                        UpdateFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;
                        UpdateFreePluginsList.IdCustomer = SelectedGroup.IdGroup;
                        UpdateFreePluginsList.IdCustomerPrevious = SelectedFreePluginName.IdCustomer;
                        UpdateFreePluginsList.IdRegionPrevious = idRegionPrevious;
                        UpdateFreePluginsList.IdCountryPrevious = idCountryPrevious;
                        UpdateFreePluginsList.IdPlantPrevious = idPlantPrevious;


                        //UpdateFreePluginsList.Region = SelectedItem.Region;
                        //UpdateFreePluginsList.Country = SelectedItem.Country;
                        //UpdateFreePluginsList.Plant = SelectedItem.Plant;

                        if (SelectedFreePluginName != null)
                        {

                            UpdateFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;

                        }
                        if (SelectedRegion != null && SelectedRegion.Count > 0)
                        {
                            if (SelectedRegion.Count == RegionList.Count && RegionList.Count > 1 || RegionList.Count == 1)
                            {
                                SelectedRegion = null;
                            }
                            else
                            {
                                //Region
                                if (SelectedRegion != null)
                                {
                                    // List<Region> selected = SelectedRegion.Cast<Region>().ToList();
                                    foreach (var item in selectedRegion)
                                    {
                                        if (UpdateFreePluginsList.SelectedRegion == null)
                                            UpdateFreePluginsList.SelectedRegion = new List<int>();
                                        UpdateFreePluginsList.SelectedRegion.Add(item.IdRegion);
                                        UpdateFreePluginsList.IdRegion = item.IdRegion;
                                    }

                                }
                            }
                        }
                        else
                        {
                            SelectedRegion = null;
                        }

                        if (SelectedCountry != null && SelectedCountry.Count > 0)
                        {
                            if (SelectedCountry.Count == CountryList.Count && CountryList.Count > 1 || CountryList.Count == 1)
                            {
                                SelectedCountry = null;
                            }
                            else
                            {
                                //Country
                                if (SelectedCountry != null)
                                {
                                    // List<Country> selected = SelectedCountry.Cast<Country>().ToList();
                                    foreach (var item in selectedCountry)
                                    {
                                        if (UpdateFreePluginsList.SelectedCountry == null)
                                            UpdateFreePluginsList.SelectedCountry = new List<int>();
                                        UpdateFreePluginsList.SelectedCountry.Add(item.IdCountry);
                                        UpdateFreePluginsList.IdCountry = item.IdCountry;
                                    }

                                }
                            }
                        }
                        else
                        {
                            SelectedCountry = null;
                        }


                        if (SelectedPlant != null && SelectedPlant.Count > 0)
                        {
                            if (SelectedPlant.Count == PlantList.Count && PlantList.Count > 1 || PlantList.Count == 1)
                            {
                                SelectedPlant = null;
                            }
                            else
                            {
                                //Plant
                                if (SelectedPlant != null)
                                {
                                    // List<Site> selected = SelectedPlant.Cast<Site>().ToList();
                                    foreach (var item in selectedPlant)
                                    {
                                        if (UpdateFreePluginsList.SelectedPlant == null)
                                            UpdateFreePluginsList.SelectedPlant = new List<UInt32>();
                                        UpdateFreePluginsList.SelectedPlant.Add(item.IdSite);
                                        UpdateFreePluginsList.IdSite = item.IdSite;
                                    }

                                }
                            }
                        }
                        else
                        {
                            SelectedPlant = null;
                        }



                        UpdateFreePluginsList = PCMService.AddUpdateFreePlugins(UpdateFreePluginsList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FreePluginUpdatedSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        IsSave = true;
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddFreePluginInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                // }



                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveFreePluginsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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




        //chitra[cgirigosavi][GEOS2-4443][08/09/2023]
        public void EditINIT(FreePlugins editdata)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditINIT()...", category: Category.Info, priority: Priority.Low);
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

                //FreePluginNamesList.FirstOrDefault().Name = editdata.Name;
                //SelectedFreePluginName=
                //SelectedFreePluginName.Name = editdata.Name;
                FreePluginNamesList = new ObservableCollection<FreePlugins>();

                FreePluginNamesList.Add(editdata);
                SelectedFreePluginName = FreePluginNamesList.FirstOrDefault();
                FillGroups();
                SelectedItem = editdata;
                Idplugin = editdata.IdPlugin;

                SelectedFreePluginName.Name = SelectedItem.Name;
                //SelectedGroup.GroupName = SelectedItem.Group;
                SelectedFreePluginName.IdPlugin = Idplugin;
                // SelectedFreePluginName.IdPlugin = SelectedItem.IdPlugin;
                SelectedFreePluginName.IdCustomer = SelectedItem.IdCustomer;
                IsRetrive = true;
                //SelectedFreePluginName.Name = editdata.Name;
                #region GEOS2-4443
                //Shubham[skadam]  Management of hardlock licenses in PCM (4/5) 09 10 2023
                try
                {
                    if (editdata != null)
                    {
                        RegionList = new ObservableCollection<Region>(PLMService.GetRegionsByGroupAndCountryAndSites_V2110(Convert.ToInt32(editdata.IdCustomer), "0", "0"));
                        CountryList = new ObservableCollection<Country>(PLMService.GetCountriesByGroupAndRegionAndSites_V2350(Convert.ToInt32(editdata.IdCustomer), "0", "0"));
                        PlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(Convert.ToInt32(editdata.IdCustomer), "0", "0"));
                        AllPlantList = new ObservableCollection<Site>(PLMService.GetPlantsByGroupAndRegionAndCountry_V2110(SelectedGroup.IdGroup, "0", "0"));
                        #region RemoveCountryNameFromPlantList
                        foreach (Site itemPlant in PlantList)
                        {
                            try
                            {
                                string[] parts = itemPlant.Name.Split('(');
                                itemPlant.Name = parts[0];
                            }
                            catch (Exception) { }
                        }
                        #endregion
                        SelectedFreePluginName.Name = SelectedItem.Name;

                        SelectedGroup = GroupList.Where(s => s.IdGroup==SelectedItem.IdCustomer).FirstOrDefault();
                        SelectedRegion = new List<object>(RegionList.Where(s => s.IdRegion==editdata.IdRegion));
                        SelectedCountry = new List<object>(CountryList.Where(s => s.IdCountry==editdata.IdCountry));
                        SelectedPlant = new List<object>(PlantList.Where(s => s.IdSite==editdata.IdSite));
                    }
                    SelectedFreePlugins = editdata;
                    IsEdit = true;
                }
                catch (Exception ex)
                {
                }
                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditINIT()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void SaveFreePluginsActionNew(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedFreePluginName"));
                if (error != null)
                {
                    return;
                }
                List<Region> selectedRegion = new List<Region>();
                List<Country> selectedCountry = new List<Country>();
                List<Site> selectedPlant = new List<Site>();
                if (SelectedRegion != null && (!SelectedRegion.All(x => x == null)))
                {
                    //if (RegionList.Count== SelectedRegion.Count)
                    //{
                    //    selectedRegion = new List<Region>();
                    //}
                    //else
                    //{
                    //    selectedRegion = SelectedRegion.Cast<Region>().ToList();
                    //}
                    selectedRegion = SelectedRegion.Cast<Region>().ToList();
                }
                if (SelectedCountry != null && (!SelectedCountry.All(x => x == null)))
                {
                    //if (CountryList.Count == SelectedCountry.Count)
                    //{
                    //    selectedCountry = new List<Country>();
                    //}
                    //else
                    //{
                    //    selectedCountry = SelectedCountry.Cast<Country>().ToList();
                    //}
                    selectedCountry = SelectedCountry.Cast<Country>().ToList();

                }
                if (SelectedPlant != null && (!SelectedPlant.All(x => x == null)))
                {
                    selectedPlant = SelectedPlant.Cast<Site>().ToList();
                    //if (PlantList.Count == SelectedPlant.Count)
                    //{
                    //    selectedPlant = new List<Site>();
                    //}
                    //else
                    //{
                    //    selectedPlant = SelectedPlant.Cast<Site>().ToList();
                    //}

                }
                FreePlugins tempData = new FreePlugins();
                //tempData = FreePluginGridList.FirstOrDefault(x => x.IdPlugin == SelectedFreePluginName.IdPlugin && x.Group == SelectedGroup.GroupName && x.IdRegion == IdRegion &&
                //  x.IdCountry == IdCountry && x.IdSite == IdPlant);

               
                if (tempData!=null)
                {
                    if (IsNew)
                    {
                        tempData = FreePluginGridList.FirstOrDefault(x => x.IdPlugin == SelectedFreePluginName.IdPlugin && x.IdCustomer == SelectedGroup.IdGroup
                         && selectedRegion.Any(a => a.IdRegion == x.IdRegion)
                         && selectedCountry.Any(a => a.IdCountry == x.IdCountry)
                         && selectedPlant.Any(a => a.IdSite == x.IdSite));
                        if (tempData == null)
                        {

                        }
                        else
                        {
                           
                            IsSave = false;
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddFreePluginInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                        ObservableCollection<FreePlugins> tempFreePluginGridList = new ObservableCollection<FreePlugins>();
                        NewFreePluginsList = new FreePlugins();
                        NewFreePluginsList.TransactionOperation = ModelBase.TransactionOperations.Add;
                        NewFreePluginsList.Name = SelectedFreePluginName.Name;
                        NewFreePluginsList.Group = SelectedGroup.GroupName;
                        NewFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;
                        NewFreePluginsList.IdCustomer = SelectedGroup.IdGroup;



                        if (SelectedGroup.IdGroup == 0)
                        {
                            foreach (Site site in SelectedPlant)
                            {
                                if (site != null)
                                {
                                    if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdSite == site.IdSite))
                                    {
                                        FreePlugins freePlugins = new FreePlugins();
                                        freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                        freePlugins.IdCustomer = site.IdGroup;
                                        freePlugins.Group = "ALL";
                                        freePlugins.IdRegion = (Int32)site.IdRegion;
                                        freePlugins.Region = site.RegionName;
                                        freePlugins.IdCountry = site.IdCountry;
                                        //freePlugins.Country = new Country();
                                        freePlugins.Country = site.CountryName;
                                        freePlugins.IdSite = site.IdSite;
                                        //freePlugins.Plant = new Site();
                                        freePlugins.Plant = site.Name;
                                        freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                        //freePlugins.IsIncluded = 0;
                                        tempFreePluginGridList.Add(freePlugins);
                                    }
                                }
                            }

                            if (SelectedPlant == null)
                            {

                                foreach (Country country in selectedCountry)
                                {
                                    if (country != null)
                                    {
                                        foreach (Region region in selectedRegion)
                                        {
                                            if (region != null)
                                            {
                                                if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdRegion == RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                                {
                                                    FreePlugins freePlugins = new FreePlugins();
                                                    freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                    freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                    freePlugins.Group = "ALL";
                                                    freePlugins.IdRegion = Convert.ToInt32(country.IdRegion);
                                                    freePlugins.Region = RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                    freePlugins.IdCountry = country.IdCountry;
                                                    //freePlugins.Country = new Country();
                                                    freePlugins.Country = country.Name;
                                                    freePlugins.IdSite = 0;
                                                    //freePlugins.Plant = new Site();
                                                    freePlugins.Plant = "ALL";
                                                    //freePlugins.IsIncluded = 0;
                                                    freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                    tempFreePluginGridList.Add(freePlugins);
                                                }
                                            }
                                        }
                                    }
                                }


                                foreach (Region region in selectedRegion)
                                {
                                    if (region != null)
                                    {
                                        if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdRegion == region.IdRegion))
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                            freePlugins.Group = "ALL";
                                            freePlugins.IdRegion = Convert.ToInt32(region.IdRegion);
                                            freePlugins.Region = region.RegionName;
                                            freePlugins.IdCountry = 0;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = "ALL";
                                            freePlugins.IdSite = 0;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = "ALL";
                                            //freePlugins.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SelectedPlant != null && selectedPlant.Count!=0)
                            {
                                foreach (Site site in SelectedPlant)
                                {
                                    if (site != null)
                                    {
                                        if (!tempFreePluginGridList.Any(ccl => ccl.IdSite == site.IdSite))
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = site.IdGroup;
                                            freePlugins.Group = site.GroupName;
                                            freePlugins.IdRegion = (Int32)site.IdRegion;
                                            freePlugins.Region = site.RegionName;
                                            freePlugins.IdCountry = site.IdCountry;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = site.CountryName;
                                            freePlugins.IdSite = site.IdSite;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = site.Name;
                                            //customer.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }
                            }

                            if (SelectedPlant == null)
                            {
                                if (selectedCountry != null)
                                {
                                    foreach (Country country in selectedCountry)
                                    {
                                        if (country != null)
                                        {
                                            if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == (uint)SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry))
                                            {
                                                FreePlugins freePlugins = new FreePlugins();
                                                freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                freePlugins.Group = SelectedGroup.GroupName;
                                                freePlugins.IdRegion = Convert.ToInt32(country.IdRegion);
                                                freePlugins.Region = RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                freePlugins.IdCountry = country.IdCountry;
                                                //freePlugins.Country = new Country();
                                                freePlugins.Country = country.Name;
                                                freePlugins.IdSite = 0;
                                                //freePlugins.Plant = new Site();
                                                freePlugins.Plant = "ALL";
                                                //freePlugins.IsIncluded = 0;
                                                freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                tempFreePluginGridList.Add(freePlugins);
                                            }
                                        }
                                    }
                                }

                                if (selectedRegion != null)
                                {
                                    foreach (Region region in selectedRegion)
                                    {
                                        if (region != null)
                                        {
                                            if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == (uint)SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion))
                                            {
                                                FreePlugins freePlugins = new FreePlugins();
                                                freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                freePlugins.Group = SelectedGroup.GroupName;
                                                freePlugins.IdRegion = Convert.ToInt32(region.IdRegion);
                                                freePlugins.Region = region.RegionName;
                                                freePlugins.IdCountry = 0;
                                                // freePlugins.Country = new Country();
                                                freePlugins.Country = "ALL";
                                                freePlugins.IdSite = 0;
                                                //freePlugins.Plant = new Site();
                                                freePlugins.Plant = "ALL";
                                                //freePlugins.IsIncluded = 0;
                                                freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                tempFreePluginGridList.Add(freePlugins);
                                            }
                                        }
                                        else
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                            freePlugins.Group = SelectedGroup.GroupName;
                                            freePlugins.IdRegion = 0;
                                            freePlugins.Region = "ALL";
                                            freePlugins.IdCountry = 0;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = "ALL";
                                            freePlugins.IdSite = 0;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = "ALL";
                                            //freePlugins.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }


                            }

                        }
                        if (tempFreePluginGridList.Count == 0)
                        {
                            FreePlugins freePlugins = new FreePlugins();
                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                            freePlugins.Group = SelectedGroup.GroupName;
                            freePlugins.IdRegion = 0;
                            freePlugins.Region = "ALL";
                            freePlugins.IdCountry = 0;
                            //freePlugins.Country = new Country();
                            freePlugins.Country = "ALL";
                            freePlugins.IdSite = 0;
                            //freePlugins.Plant = new Site();
                            freePlugins.Plant = "ALL";
                            //freePlugins.IsIncluded = 0;
                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                            tempFreePluginGridList.Add(freePlugins);
                        }
                        tempFreePluginGridList = new ObservableCollection<FreePlugins>(tempFreePluginGridList.OrderBy(a => a.Group));

                        List<FreePlugins> IsCheckedList = tempFreePluginGridList.ToList();
                        #region MyRegion
                        //Group = (from x in IsCheckedList select x.Group).Distinct().Count();
                        //Region = (from x in IsCheckedList select x.Region).Distinct().Count();
                        //Country = (from x in IsCheckedList select x.Country).Distinct().Count();
                        //Plant = (from x in IsCheckedList select x.Plant).Distinct().Count();

                        //Groups = String.Join(", ", IsCheckedList.Select(a => a.Group).Distinct());
                        //Regions = String.Join(", ", IsCheckedList.Select(a => a.Region).Distinct());
                        //Countries = String.Join(", ", IsCheckedList.Select(a => a.Country).Distinct());
                        //Plants = String.Join(", ", IsCheckedList.Select(a => a.Plant).Distinct());
                        #endregion
                        //PCMService = new PCMServiceController("localhost:6699");
                        bool NewFreePluginsResult = PCMService.AddUpdateFreePlugins_V2440(tempFreePluginGridList.ToList());
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FreePluginAddedSuccessful").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsSave = true;
                        RequestClose(null, null);
                        NewfreePlugins = new FreePlugins();
                        NewfreePlugins = tempFreePluginGridList.LastOrDefault();
                        FinalfreePlugins = new ObservableCollection<FreePlugins>(tempFreePluginGridList);
                    }
                    else
                    {

                        //if (SelectedPlant.FirstOrDefault().IdPlugin != SelectedFreePluginName.IdPlugin && SelectedPlant.FirstOrDefault().IdCustomer != SelectedFreePluginName.IdCustomer &&
                        //    SelectedPlant.FirstOrDefault().IdRegion != SelectedFreePluginName.IdRegion && SelectedPlant.FirstOrDefault().IdCountry != SelectedFreePluginName.IdCountry &&
                        //    SelectedPlant.FirstOrDefault().IdSite != SelectedFreePluginName.IdSite
                        //    )
                        //{

                        //}
                        UpdateFreePluginsList = new FreePlugins();
                        UpdateFreePluginsList.TransactionOperation = ModelBase.TransactionOperations.Add;
                        UpdateFreePluginsList.Name = SelectedFreePluginName.Name;
                        UpdateFreePluginsList.Group = SelectedGroup.GroupName;
                        UpdateFreePluginsList.IdPlugin = SelectedFreePluginName.IdPlugin;
                        UpdateFreePluginsList.IdCustomer = SelectedGroup.IdGroup;
                        UpdateFreePluginsList.IdCustomerPrevious = SelectedFreePluginName.IdCustomer;
                        //UpdateFreePluginsList.IdRegionPrevious = idRegionPrevious;
                        //UpdateFreePluginsList.IdCountryPrevious = idCountryPrevious;
                        //UpdateFreePluginsList.IdPlantPrevious = idPlantPrevious;

                        ObservableCollection<FreePlugins> tempFreePluginGridList = new ObservableCollection<FreePlugins>();
                        if (selectedRegion.Count == 0 && selectedCountry.Count == 0 && selectedPlant.Count == 0)
                        {
                            FreePlugins freePlugins = new FreePlugins();
                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                            freePlugins.Group = SelectedGroup.GroupName;
                            freePlugins.IdRegion = 0;
                            freePlugins.Region = "ALL";
                            freePlugins.IdCountry = 0;
                            //freePlugins.Country = new Country();
                            freePlugins.Country = "ALL";
                            freePlugins.IdSite = 0;
                            //freePlugins.Plant = new Site();
                            freePlugins.Plant = "ALL";
                            //freePlugins.IsIncluded = 0;
                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                            tempFreePluginGridList.Add(freePlugins);
                        }
                        else if (SelectedGroup.IdGroup == 0)
                        {
                            foreach (Site site in SelectedPlant)
                            {
                                if (site != null)
                                {
                                    if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdSite == site.IdSite))
                                    {
                                        FreePlugins freePlugins = new FreePlugins();
                                        freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                        freePlugins.IdCustomer = site.IdGroup;
                                        freePlugins.Group = "ALL";
                                        freePlugins.IdRegion = (Int32)site.IdRegion;
                                        freePlugins.Region = site.RegionName;
                                        freePlugins.IdCountry = site.IdCountry;
                                        //freePlugins.Country = new Country();
                                        freePlugins.Country = site.CountryName;
                                        freePlugins.IdSite = site.IdSite;
                                        //freePlugins.Plant = new Site();
                                        freePlugins.Plant = site.Name;
                                        freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                        //freePlugins.IsIncluded = 0;
                                        tempFreePluginGridList.Add(freePlugins);
                                    }
                                }
                            }

                            if (SelectedPlant == null)
                            {

                                foreach (Country country in selectedCountry)
                                {
                                    if (country != null)
                                    {
                                        foreach (Region region in selectedRegion)
                                        {
                                            if (region != null)
                                            {
                                                if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdRegion == RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().IdRegion && ccl.IdCountry == country.IdCountry))
                                                {
                                                    FreePlugins freePlugins = new FreePlugins();
                                                    freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                    freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                    freePlugins.Group = "ALL";
                                                    freePlugins.IdRegion = Convert.ToInt32(country.IdRegion);
                                                    freePlugins.Region = RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                    freePlugins.IdCountry = country.IdCountry;
                                                    //freePlugins.Country = new Country();
                                                    freePlugins.Country = country.Name;
                                                    freePlugins.IdSite = 0;
                                                    //freePlugins.Plant = new Site();
                                                    freePlugins.Plant = "ALL";
                                                    //freePlugins.IsIncluded = 0;
                                                    freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                    tempFreePluginGridList.Add(freePlugins);
                                                }
                                            }
                                        }
                                    }
                                }


                                foreach (Region region in selectedRegion)
                                {
                                    if (region != null)
                                    {
                                        if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == 0 && ccl.IdRegion == region.IdRegion))
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                            freePlugins.Group = "ALL";
                                            freePlugins.IdRegion = Convert.ToInt32(region.IdRegion);
                                            freePlugins.Region = region.RegionName;
                                            freePlugins.IdCountry = 0;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = "ALL";
                                            freePlugins.IdSite = 0;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = "ALL";
                                            //freePlugins.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SelectedPlant != null && selectedPlant.Count != 0)
                            {
                                foreach (Site site in SelectedPlant)
                                {
                                    if (site != null)
                                    {
                                        if (!tempFreePluginGridList.Any(ccl => ccl.IdSite == site.IdSite))
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = site.IdGroup;
                                            freePlugins.Group = site.GroupName;
                                            freePlugins.IdRegion = (Int32)site.IdRegion;
                                            freePlugins.Region = site.RegionName;
                                            freePlugins.IdCountry = site.IdCountry;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = site.CountryName;
                                            freePlugins.IdSite = site.IdSite;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = site.Name;
                                            //customer.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }
                            }

                            if (SelectedPlant == null)
                            {
                                if (selectedCountry != null)
                                {
                                    foreach (Country country in selectedCountry)
                                    {
                                        if (country != null)
                                        {
                                            if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == (uint)SelectedGroup.IdGroup && ccl.IdCountry == country.IdCountry))
                                            {
                                                FreePlugins freePlugins = new FreePlugins();
                                                freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                freePlugins.Group = SelectedGroup.GroupName;
                                                freePlugins.IdRegion = Convert.ToInt32(country.IdRegion);
                                                freePlugins.Region = RegionList.Where(i => i.IdRegion == Convert.ToUInt32(country.IdRegion)).FirstOrDefault().RegionName;
                                                freePlugins.IdCountry = country.IdCountry;
                                                //freePlugins.Country = new Country();
                                                freePlugins.Country = country.Name;
                                                freePlugins.IdSite = 0;
                                                //freePlugins.Plant = new Site();
                                                freePlugins.Plant = "ALL";
                                                //freePlugins.IsIncluded = 0;
                                                freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                tempFreePluginGridList.Add(freePlugins);
                                            }
                                        }
                                    }
                                }

                                if (selectedRegion != null)
                                {
                                    foreach (Region region in selectedRegion)
                                    {
                                        if (region != null)
                                        {
                                            if (!tempFreePluginGridList.Any(ccl => ccl.IdCustomer == (uint)SelectedGroup.IdGroup && ccl.IdRegion == region.IdRegion))
                                            {
                                                FreePlugins freePlugins = new FreePlugins();
                                                freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                                freePlugins.Group = SelectedGroup.GroupName;
                                                freePlugins.IdRegion = Convert.ToInt32(region.IdRegion);
                                                freePlugins.Region = region.RegionName;
                                                freePlugins.IdCountry = 0;
                                                // freePlugins.Country = new Country();
                                                freePlugins.Country = "ALL";
                                                freePlugins.IdSite = 0;
                                                //freePlugins.Plant = new Site();
                                                freePlugins.Plant = "ALL";
                                                //freePlugins.IsIncluded = 0;
                                                freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                                tempFreePluginGridList.Add(freePlugins);
                                            }
                                        }
                                        else
                                        {
                                            FreePlugins freePlugins = new FreePlugins();
                                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                                            freePlugins.Group = SelectedGroup.GroupName;
                                            freePlugins.IdRegion = 0;
                                            freePlugins.Region = "ALL";
                                            freePlugins.IdCountry = 0;
                                            //freePlugins.Country = new Country();
                                            freePlugins.Country = "ALL";
                                            freePlugins.IdSite = 0;
                                            //freePlugins.Plant = new Site();
                                            freePlugins.Plant = "ALL";
                                            //freePlugins.IsIncluded = 0;
                                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                                            tempFreePluginGridList.Add(freePlugins);
                                        }
                                    }
                                }


                            }

                        }
                        if (tempFreePluginGridList.Count == 0)
                        {
                            FreePlugins freePlugins = new FreePlugins();
                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Add;
                            freePlugins.IdCustomer = (uint)SelectedGroup.IdGroup;
                            freePlugins.Group = SelectedGroup.GroupName;
                            freePlugins.IdRegion = 0;
                            freePlugins.Region = "ALL";
                            freePlugins.IdCountry = 0;
                            //freePlugins.Country = new Country();
                            freePlugins.Country = "ALL";
                            freePlugins.IdSite = 0;
                            //freePlugins.Plant = new Site();
                            freePlugins.Plant = "ALL";
                            //freePlugins.IsIncluded = 0;
                            freePlugins.IdPlugin = SelectedFreePluginName.IdPlugin;
                            tempFreePluginGridList.Add(freePlugins);
                        }
                        if (SelectedFreePlugins!=null)
                        {
                            FreePlugins freePlugins = new FreePlugins();
                            freePlugins.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            freePlugins.IdCustomer = (uint)SelectedFreePlugins.IdCustomer;
                            freePlugins.Group = SelectedFreePlugins.Group;
                            freePlugins.IdRegion = SelectedFreePlugins.IdRegion;
                            freePlugins.Region = "ALL";
                            freePlugins.IdCountry = SelectedFreePlugins.IdCountry;
                            //freePlugins.Country = new Country();
                            freePlugins.Country = "ALL";
                            freePlugins.IdSite = SelectedFreePlugins.IdSite;
                            //freePlugins.Plant = new Site();
                            freePlugins.Plant = "ALL";
                            //freePlugins.IsIncluded = 0;
                            freePlugins.IdPlugin = SelectedFreePlugins.IdPlugin;
                            //if (tempFreePluginGridList.FirstOrDefault().IdPlugin != freePlugins.IdPlugin && tempFreePluginGridList.FirstOrDefault().IdCustomer != freePlugins.IdCustomer &&
                            //    tempFreePluginGridList.FirstOrDefault().IdRegion != freePlugins.IdRegion && tempFreePluginGridList.FirstOrDefault().IdCountry != freePlugins.IdCountry &&
                            //    tempFreePluginGridList.FirstOrDefault().IdSite != freePlugins.IdSite
                            //    )
                            //{

                            //}
                            try
                            {
                                ObservableCollection<FreePlugins> tempFreePluginGridListNew = new ObservableCollection<FreePlugins>();
                                tempFreePluginGridListNew = new ObservableCollection<FreePlugins>(tempFreePluginGridList);
                                tempFreePluginGridList = new ObservableCollection<FreePlugins>();
                                tempFreePluginGridList.Add(freePlugins);
                                foreach (FreePlugins FreePlugins in tempFreePluginGridListNew)
                                {
                                    tempFreePluginGridList.Add(FreePlugins);
                                }
                            }
                            catch (Exception ex)  {  }

                        }
                        //tempFreePluginGridList = new ObservableCollection<FreePlugins>(tempFreePluginGridList.OrderBy(a => a.Group));
                        //List<FreePlugins> IsCheckedList = tempFreePluginGridList.ToList();
                        //PCMService = new PCMServiceController("localhost:6699");
                        bool NewFreePluginsResult = PCMService.AddUpdateFreePlugins_V2440(tempFreePluginGridList.ToList());
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("FreePluginUpdatedSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsSave = true;
                        RequestClose(null, null);
                        NewfreePlugins = new FreePlugins();
                        NewfreePlugins = tempFreePluginGridList.FirstOrDefault();
                        FinalfreePlugins = new ObservableCollection<FreePlugins>(tempFreePluginGridList);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddFreePluginInformationExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                // }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveFreePluginsAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                me[BindableBase.GetPropertyName(() => SelectedFreePluginName)];


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

                string selectedname = BindableBase.GetPropertyName(() => SelectedFreePluginName);

                if (columnName == selectedname)
                {
                    return AddFreePluginsValidation.GetErrorMessage(selectedname, SelectedFreePluginName.Name);
                }
                return null;
            }
        }
        #endregion


    }
}
