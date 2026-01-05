using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors; 
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SynchronizationClass;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.Helper;
using DevExpress.Data;
using Emdep.Geos.UI.Commands;
using Newtonsoft.Json;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid.TreeList;
using System.Globalization;
using DevExpress.Xpf.Accordion;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Microsoft.Win32;
using DevExpress.Spreadsheet;
using System.Text.RegularExpressions;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    class AddEditDeliveryTimeDistributionViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service


        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      // IERMService ERMService = new ERMServiceController("localhost:6699");
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
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

        #region Declarations
        List<DeliveryTimeDistributionModules> deliveryTimeDistributionModulesDeleteList = new List<DeliveryTimeDistributionModules>();
        UInt64 iddeliverytimedistribution;
        UInt64 idDeliveryTimeDistributionModule;
        private bool IsCopyName;
        private bool IsCopyDescription;

        private double dialogHeight;
        private double dialogWidth;

        private ObservableCollection<Language> languages;
        private Language languageSelected;

        private string description;
        //GEOS2-3854 gulab lakade for use log section
        private string tempdescription;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;

        private string name;
        //GEOS2-3854 gulab lakade for use log section
        private string tempname;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;

        MaximizedElementPosition maximizedElementPosition;

        private string windowHeader;
        private bool isNew;
        MaximizedElementPosition selectedAppearanceItem;
        private ObservableCollection<Site> plantList;
        private List<object> selectedPlant;
        private List<object> selectedPlant_Copy;

        private List<LookupValue> statusList;
        private int selectedStatusIndex;
        private LookupValue selectedStatus;

        private string selectedDTDCode;
        private ulong idDeliveryTimeDistribution;

        private bool isCheckedAllLanguages;
        private bool isNewDTDSave;
        private bool isUpdatedDTDSave;

        private DateTime creationDate;
        private DateTime? expiryDate;
        private DateTime? effectiveDate;
        private DateTime? lastUpdated;

        private bool isAll;

        private bool isPlantAddedOrRemove;
        private string error = string.Empty;
        private string informationError;

        private bool isFirstTimeLoad;
        private bool isSwitch;

        private bool? oldIsCheckedAllLanguages = null;
        private DateTime? oldEffectiveDate;
        private DateTime? oldExpiryDate;
        private string oldName;
        private string oldDescription;
        private string oldSelectedLanguage;
        private bool isEnabledSaveButton;
        private bool isEnabledAcceptButton;
        private bool isUserPermission;
        private bool isSaveButtonClicked;
        private Visibility saveButtonVisibility;
        private bool isInitializeFirstTime;

        private bool inActiveStatus;
        private List<GeosAppSetting> geosAppSettingList;
        //private ObservableCollection<WorkOperationByStages> workOperationMenulist;
        //private WorkOperationByStages selectedWorkOperationMenulist;
        //private ObservableCollection<WorkOperationByStages> clonedWorkOperationByStages;
        //private ObservableCollection<WorkOperation> workOperationsList_All;
        private ObservableCollection<Stages> getStages;
        TreeListControl treeListControlInstance;
        //private List<LookupValue> supplements;
        private DeliveryTimeDistribution clonedDeliveryTimeDistribution;
        private DataTable dttable;
        private DataTable dttableCopy;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        //private List<Tuple<string, float?>> supplementsBoxMenu;
        //private List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement;

        private ObservableCollection<ProductTypes> productTypesMenulistForGridLayout;

        private object tempModuleSelectedCopy;

        List<ICloneable> newList = new List<ICloneable>();

        private bool isInformationVisible;
        private string height;
        private Visibility isToggleButtonVisible;

        private ObservableCollection<ERMSOPModule> tmpModuleMenulist; //[GEOS2-3909] [Rupali Sarode] [04-11-2022]
        private ObservableCollection<ERMSOPModule> tmpModuleMenulistOriginal; //[GEOS2-4108] [Rupali Sarode] [05-01-2023]

        private string selectedTabName;
        private string oldTabName;

        private List<PlanningDateReviewStages> dTDStagesList;
        private List<PlanningDateReviewStages> tempDTDStagesList;
        private DataTable dataTableForGridLayoutModules;
        private ObservableCollection<BandItem> bands;

        private List<DeliveryTimeDistributionModules> tempLstDeliveryTimeDistributionModules = new List<DeliveryTimeDistributionModules>();
        private List<Template> templatesMenuList;
        private bool isGridOpen;
        private bool isBandOpen;
        private Visibility isTemplateDetailsViewVisible;
        List<ProductTypes> cptypesGridList;
        List<ProductTypes> cptypes;
        private Visibility isAccordionControlVisible;
        ObservableCollection<TemplateWithCPTypes> templateWithCPTypes;
        private ObservableCollection<TreeListColumn> columnsModuleOperationTime;
        ObservableCollection<ERMSOPModule> moduleMenulistCloned;
        ObservableCollection<ERMSOPModule> moduleMenulist;
        private string templateHeader;
        private Visibility showError;
        ObservableCollection<LogentriesbyDeliveryTimeDistribution> deliveryTimeDistributionAllChangeLogList;
        ObservableCollection<LogentriesbyDeliveryTimeDistribution> deliveryTimeDistributionChangeLogList;
        private bool IsAcceptableFlag;

        private AccordionControl dTDAccordion;
        #endregion

        #region Properties


        public UInt64 IdDeliveryTimeDistributionModule
        {
            get { return idDeliveryTimeDistributionModule; }
            set
            {
                idDeliveryTimeDistributionModule = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDeliveryTimeDistributionModule"));
            }
        }
        public ObservableCollection<Summary> GroupSummary { get; private set; }

        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public DeliveryTimeDistribution NewDTD { get; set; }
        public DeliveryTimeDistribution UpdatedDTD { get; set; }
        public DeliveryTimeDistribution ClonedDTD { get; set; }

        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }

        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }

        public Language LanguageSelected
        {
            get
            {
                return languageSelected;
            }
            set
            {
                languageSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelected"));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        public string Tempdescription
        {
            get { return tempdescription; }
            set
            {
                tempdescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tempdescription"));
            }
        }

        public string Description_en
        {
            get
            {
                return description_en;
            }
            set
            {
                description_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
            }
        }

        public string Description_es
        {
            get
            {
                return description_es;
            }
            set
            {
                description_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
            }
        }

        public string Description_fr
        {
            get
            {
                return description_fr;
            }
            set
            {
                description_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            }
        }

        public string Description_pt
        {
            get
            {
                return description_pt;
            }
            set
            {
                description_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
            }
        }

        public string Description_ro
        {
            get
            {
                return description_ro;
            }
            set
            {
                description_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
            }
        }

        public string Description_ru
        {
            get
            {
                return description_ru;
            }
            set
            {
                description_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
            }
        }

        public string Description_zh
        {
            get
            {
                return description_zh;
            }
            set
            {
                description_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public string Tempname
        {
            get
            {
                return tempname;
            }
            set
            {
                tempname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tempname"));
            }
        }

        public string Name_en
        {
            get
            {
                return name_en;
            }
            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
            }
        }

        public string Name_es
        {
            get
            {
                return name_es;
            }
            set
            {
                name_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_es"));
            }
        }

        public string Name_fr
        {
            get
            {
                return name_fr;
            }
            set
            {
                name_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_fr"));
            }
        }

        public string Name_pt
        {
            get
            {
                return name_pt;
            }
            set
            {
                name_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_pt"));
            }
        }

        public string Name_ro
        {
            get
            {
                return name_ro;
            }
            set
            {
                name_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ro"));
            }
        }

        public string Name_ru
        {
            get
            {
                return name_ru;
            }
            set
            {
                name_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ru"));
            }
        }

        public string Name_zh
        {
            get
            {
                return name_zh;
            }
            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
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

        public MaximizedElementPosition SelectedAppearanceItem
        {
            get { return selectedAppearanceItem; }
            set
            {
                selectedAppearanceItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearanceItem"));
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

        public List<object> SelectedPlant_Copy
        {
            get
            {
                return selectedPlant_Copy;
            }

            set
            {
                selectedPlant_Copy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant_Copy"));
            }
        }

        public List<LookupValue> StatusList
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

        public LookupValue SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }

        public ulong IdDeliveryTimeDistribution
        {
            get
            {
                return idDeliveryTimeDistribution;
            }
            set
            {
                idDeliveryTimeDistribution = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDeliveryTimeDistribution"));
            }
        }

        public string SelectedDTDCode
        {
            get
            {
                IsInformationVisible = true;
                return selectedDTDCode;
            }
            set
            {
                IsInformationVisible = true;
                selectedDTDCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDTDCode"));
            }
        }

        public bool IsCheckedAllLanguages
        {
            get
            {
                return isCheckedAllLanguages;
            }
            set
            {
                isCheckedAllLanguages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedAllLanguages"));
            }
        }

        public bool IsNewDTDSave
        {
            get
            {
                return isNewDTDSave;
            }

            set
            {
                isNewDTDSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewDTDSave"));
            }
        }

        public bool IsUpdatedDTDSave
        {
            get
            {
                return isUpdatedDTDSave;
            }

            set
            {
                isUpdatedDTDSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdatedDTDSave"));
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CreationDate"));
            }
        }

        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                expiryDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpiryDate"));
            }
        }

        public DateTime? EffectiveDate
        {
            get
            {
                return effectiveDate;
            }
            set
            {
                effectiveDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EffectiveDate"));
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                lastUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastUpdated"));
            }
        }

        public bool IsAll
        {
            get
            {
                return isAll;
            }

            set
            {
                isAll = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAll"));
            }
        }

        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }

        public LookupValue SelectedOriginalStatus { get; set; }

        public bool IsPlantAddedOrRemove
        {
            get
            {
                return isPlantAddedOrRemove;
            }

            set
            {
                isPlantAddedOrRemove = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlantAddedOrRemove"));
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

        string total;
        public string Total
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Total"));
            }
        }

        public bool IsFirstTimeLoad
        {
            get
            {
                return isFirstTimeLoad;
            }

            set
            {
                isFirstTimeLoad = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFirstTimeLoad"));
            }
        }

        public bool IsSwitch
        {
            get
            {
                return isSwitch;
            }

            set
            {
                isSwitch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSwitch"));
            }
        }

        public bool IsUserPermission
        {
            get
            {
                return isUserPermission;
            }

            set
            {
                isUserPermission = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUserPermission"));
            }
        }

        public bool IsEnabledAcceptButton
        {
            get { return isEnabledAcceptButton; }
            set
            {
                isEnabledAcceptButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledAcceptButton"));
            }
        }

        public bool IsEnabledSaveButton
        {
            get { return isEnabledSaveButton; }
            set
            {
                if (GeosApplication.Instance.IsEditDeliveryTimeDistributionERM)
                {
                    if (isEnabledSaveButton != value)
                    {
                        isEnabledSaveButton = value;
                    }
                }
                else
                {
                    isEnabledSaveButton = false;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledSaveButton"));
            }
        }
        public Visibility SaveButtonVisibility
        {
            get { return saveButtonVisibility; }
            set { saveButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("SaveButtonVisibility")); }
        }

        public bool InActiveStatus
        {
            get
            {
                return inActiveStatus;
            }

            set
            {
                inActiveStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InActiveStatus"));
            }
        }

        public DeliveryTimeDistribution ClonedDeliveryTimeDistribution
        {
            get
            {
                return clonedDeliveryTimeDistribution;
            }
            set
            {
                clonedDeliveryTimeDistribution = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedDeliveryTimeDistribution"));
            }
        }

        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }

        public ObservableCollection<Summary> TotalSummary { get; private set; }

        //public List<Tuple<string, float?>> SupplementsBoxMenu
        //{
        //    get { return supplementsBoxMenu; }
        //    set
        //    {
        //        supplementsBoxMenu = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SupplementsBoxMenu"));
        //    }
        //}
        //List<Tuple<string, float?>> allsupplementsBoxMenu;
        //public List<Tuple<string, float?>> AllSupplementsBoxMenu
        //{
        //    get { return allsupplementsBoxMenu; }
        //    set
        //    {
        //        allsupplementsBoxMenu = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("AllSupplementsBoxMenu"));
        //    }
        //}
        //public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement
        //{
        //    get { return lstStandardOperationsDictionarySupplement; }
        //    set
        //    {
        //        lstStandardOperationsDictionarySupplement = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionarySupplement"));
        //    }
        //}

        //public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement_Cloned
        //{
        //    get { return lstStandardOperationsDictionarySupplement_Cloned; }
        //    set
        //    {
        //        lstStandardOperationsDictionarySupplement_Cloned = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionarySupplement_Cloned"));
        //    }
        //}


        public bool IsInformationVisible
        {
            get
            {
                return isInformationVisible;
            }

            set
            {
                isInformationVisible = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsInformationVisible"));
            }
        }

        public string Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        public Visibility IsToggleButtonVisible
        {
            get
            {
                return isToggleButtonVisible;
            }

            set
            {
                isToggleButtonVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsToggleButtonVisible"));
            }
        }

        public ObservableCollection<ProductTypes> ProductTypesMenulistForGridLayout
        {
            get { return productTypesMenulistForGridLayout; }
            set
            {
                productTypesMenulistForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProductTypesMenulistForGridLayout"));
            }
        }


        public ProductTypes ClonedProductType { get; set; }

        public List<Template> TemplatesMenuList
        {
            get { return templatesMenuList; }
            set
            {
                templatesMenuList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplatesMenuList"));
            }
        }

        public bool IsGridOpen
        {
            get
            {
                return isGridOpen;
            }
            set
            {
                isGridOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGridOpen"));
            }
        }

        public bool IsBandOpen
        {
            get
            {
                return isBandOpen;
            }
            set
            {
                isBandOpen = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBandOpen"));
            }
        }

        private object selectedTemplate;
        public virtual object SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                //if (IsTemplateDetailsViewVisible == Visibility.Visible)
                //{
                //    SelectItemForTemplate();
                //}
                if (selectedTemplate != null)
                {
                    RetrieveDTDRateByModule();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplate"));
            }
        }

        private Visibility isAccordionControlVisibleLogs;
        public Visibility IsAccordionControlVisibleLogs
        {
            get { return isAccordionControlVisibleLogs; }
            set
            {
                isAccordionControlVisibleLogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisibleLogs"));
            }
        }


        public virtual object TempModuleSelectedCopy
        {
            get { return tempModuleSelectedCopy; }
            set
            {
                tempModuleSelectedCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempModuleSelectedCopy"));
            }
        }





        #region GEOS2-3855 gulab lakade
        //public ObservableCollection<LogentriesbyStandardOperationsDictionary> StandardOperationsDictionaryChangeLogList
        //{
        //    get { return standardOperationsDictionaryChangeLogList; }
        //    set
        //    {
        //        standardOperationsDictionaryChangeLogList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("StandardOperationsDictionaryChangeLogList"));
        //    }
        //}

        //public ObservableCollection<LogentriesbyStandardOperationsDictionary> StandardOperationsDictionaryAllChangeLogList
        //{
        //    get { return standardOperationsDictionaryAllChangeLogList; }
        //    set
        //    {
        //        standardOperationsDictionaryAllChangeLogList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("StandardOperationsDictionaryAllChangeLogList"));
        //    }
        //}
        #endregion


        public List<PlanningDateReviewStages> DTDStagesList
        {

            get
            {
                return dTDStagesList;
            }

            set
            {
                dTDStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTDStagesList"));
            }

        }

        public List<PlanningDateReviewStages> TempDTDStagesList
        {
            get
            { return tempDTDStagesList; }

            set
            {
                tempDTDStagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempDTDStagesList"));
            }
        }

        public DataTable DataTableForGridLayoutModules
        {
            get { return dataTableForGridLayoutModules; }
            set
            {
                dataTableForGridLayoutModules = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayoutModules"));
            }
        }


        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        public ObservableCollection<TemplateWithCPTypes> TemplateWithCPTypes
        {
            get { return templateWithCPTypes; }
            set
            {
                templateWithCPTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateWithCPTypes"));
            }
        }

        public List<ProductTypes> Cptypes
        {
            get { return cptypes; }
            set
            {
                cptypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Cptypes"));
            }
        }

        public Visibility IsTemplateDetailsViewVisible
        {
            get { return isTemplateDetailsViewVisible; }
            set
            {
                isTemplateDetailsViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTemplateDetailsViewVisible"));
            }
        }

        public List<ProductTypes> CptypesGridList
        {
            get { return cptypesGridList; }
            set
            {
                cptypesGridList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CptypesGridList"));
            }
        }

        public Visibility IsAccordionControlVisible
        {
            get { return isAccordionControlVisible; }
            set
            {
                isAccordionControlVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisible"));
            }
        }

        public string TemplateHeader
        {
            get { return templateHeader; }
            set
            {
                templateHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateHeader"));
            }
        }

        private Visibility ShowError
        {
            get { return showError; }
            set
            {
                showError = value;
                OnParameterChanged(new PropertyChangedEventArgs("ShowError"));
            }
        }

        public ObservableCollection<LogentriesbyDeliveryTimeDistribution> DeliveryTimeDistributionAllChangeLogList
        {
            get { return deliveryTimeDistributionAllChangeLogList; }
            set
            {
                deliveryTimeDistributionAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryTimeDistributionAllChangeLogList"));
            }
        }

        public ObservableCollection<LogentriesbyDeliveryTimeDistribution> DeliveryTimeDistributionChangeLogList
        {
            get { return deliveryTimeDistributionChangeLogList; }
            set
            {
                deliveryTimeDistributionChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeliveryTimeDistributionChangeLogList"));
            }
        }

        private float dTDRate;
        public float DTDRate
        {

            get { return dTDRate; }
            set
            {
                dTDRate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTDRate"));
            }
        }
        private Int32 idStage;
        public Int32 IdStage
        {

            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdStage"));
            }
        }

        private byte idCpType;
        public byte IdCpType
        {
            get { return idCpType; }
            set
            {
                idCpType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCpType"));
            }

        }
        //[rgadhave][GEOS2-5583][20-06-2024] 
        private Int64 idCpTypeNew;
        public Int64 IdCpTypeNew
        {
            get { return idCpTypeNew; }
            set
            {
                idCpTypeNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCpTypeNew"));
            }

        }
        private List<float> dTDRateList;
        public List<float> DTDRateList
        {
            get { return dTDRateList; }
            set
            {
                dTDRateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTDRateList"));
            }
        }

        public AccordionControl DTDAccordion
        {
            get { return dTDAccordion; }
            set
            {
                dTDAccordion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTDAccordion"));
            }
        }

        #endregion

        #region ICommands

        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand SetControlWidthUsingActualWidthCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }

        public ICommand ShowDescriptionViewCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        public ICommand SaveButtonCommand { get; set; }
        public ICommand ChangeStatusCommand { get; set; }
        public ICommand ChangeEffectiveDateCommand { get; set; }
        public ICommand ChangeExpiryDateCommand { get; set; }
        public ICommand ChangeCheckAllLanguagesCommand { get; set; }

        public ICommand ToogleButtonCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public ICommand CommandTemplateSelection { get; set; }


        public ICommand HidePanelCommand { get; set; }
        public ICommand SelectCPTypesCommand { get; set; }
        public ICommand CustomSummaryCommand { get; set; }
        public ICommand CommandCellValueChanged { get; set; }
        public ICommand CellValueChangedCommand { get; set; }
        public ICommand LogsHidePanelCommand { get; set; }
        public ICommand DTDRateValueChangedCommand { get; set; }
        public ICommand DTDRateValidateCellCommand { get; set; }
        public ICommand DTDRateCellValueChangingCommand { get; set; }
        public ICommand DTDRateValidateRowCommand { get; set; }
        public ICommand DeleteModuleCommand { get; set; }
        public ICommand GridLoadCommand { get; set; }

        #endregion


        #region Constructor

        public AddEditDeliveryTimeDistributionViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditDeliveryTimeDistributionViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                IsInformationVisible = true;
                IsToggleButtonVisible = Visibility.Visible;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);

                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveDescriptionByLanguge);
                ChangeNameCommand = new DelegateCommand<object>(SetNameToLanguage);
                SetControlWidthUsingActualWidthCommand = new DelegateCommand<object>(SetControlWidthUsingActualWidth);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                UncheckedCopyNameDescriptionCommand = new DelegateCommand<object>(UncheckedCopyNameDescription);
                ShowDescriptionViewCommand = new DelegateCommand<object>(ShowDescriptionViewCommandAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);

                SaveButtonVisibility = Visibility.Collapsed;

                SaveButtonCommand = new DelegateCommand(SaveButtonCommandAction);
                ChangeStatusCommand = new DelegateCommand<object>(ChangeStatusCommandAction);
                ChangeEffectiveDateCommand = new DelegateCommand(ChangeEffectiveDateCommandAction);
                ChangeExpiryDateCommand = new DelegateCommand(ChangeExpiryDateCommandAction);
                //  SupplementsValueChangedCommand = new DelegateCommand<object>(SupplementsValueChangedCommandAction);
                ToogleButtonCommand = new DelegateCommand(ToogleButtonCommandAction);
                isInitializeFirstTime = true;

                #region GEOS2-3246
                CommandTemplateSelection = new DelegateCommand(SelectItemForTemplate);
                HidePanelCommand = new RelayCommand(new Action<object>(HidePanel));
                IsTemplateDetailsViewVisible = Visibility.Visible;
                #endregion


                DeleteModuleCommand = new RelayCommand(new Action<object>(DeleteModuleCommandAction));
                LogsHidePanelCommand = new RelayCommand(new Action<object>(HideLogPanel));
                //  StandardOperationsDictionaryChangeLogList = new ObservableCollection<LogentriesbyStandardOperationsDictionary>();


                DTDRateValueChangedCommand = new DelegateCommand<object>(DTDRateValueChangedCommandAction);
                DTDRateValidateCellCommand = new DelegateCommand<GridCellValidationEventArgs>(DTDRateValidateCellCommandAction);
                DTDRateCellValueChangingCommand = new DelegateCommand<CellValueChangedEventArgs>(DTDRateCellValueChangingCommandAction);

                DTDRateValidateRowCommand = new DelegateCommand<GridRowValidationEventArgs>(DTDRateValidateRowCommandAction);
                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel);

                GridLoadCommand = new DelegateCommand<object>(GridLoadCommandAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor AddEditDeliveryTimeDistributionViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor AddEditDeliveryTimeDistributionViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods

        public void Init(string selectedHeader)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                // MaximizedElementPosition = MaximizedElementPosition.Right;
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][22/11/2023]
                FillLanguages();
                GetPlants();
                GetLatestDTDCode();
                FillStatusList();

                SaveButtonVisibility = Visibility.Collapsed;
                if (GeosApplication.Instance.IsEditDeliveryTimeDistributionERM)
                {
                    IsEnabledAcceptButton = true;
                }

                EffectiveDate = new DateTime(DateTime.Now.Year, 01, 01);
                ExpiryDate = new DateTime(DateTime.Now.Year, 12, 31);
                //FlagToDragDrop = false;  //[GEOS2-3909][Rupali Sarode][11-11-2022]

                IsTemplateDetailsViewVisible = Visibility.Visible;
                IsGridOpen = true;
                IsBandOpen = false;
                //AddEditColumnInModulesDataTable();
                //FillModuleDataTable();
                GetStagesList();
                AddColumnsToDataTablewithBandsinModules();
                FillTemplateMenulist(null);

                IsAcceptableFlag = false;

                ShowError = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        public void EditInit(string selectedHeader)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][22/11/2023]
                FillLanguages();
                GetPlants();
                GetLatestDTDCode();
                FillStatusList();

                SaveButtonVisibility = Visibility.Visible;
                if (GeosApplication.Instance.IsEditDeliveryTimeDistributionERM)
                {
                    IsEnabledAcceptButton = true;
                }

                EffectiveDate = new DateTime(DateTime.Now.Year + 1, 01, 01);
                ExpiryDate = new DateTime(DateTime.Now.Year + 1, 12, 31);
                Height = "0";

                IsTemplateDetailsViewVisible = Visibility.Visible;
                IsGridOpen = true;
                IsBandOpen = false;

                //  FillTemplateMenulist();

                GetStagesList();
                AddColumnsToDataTablewithBandsinModules();
                IsAcceptableFlag = false;
                ShowError = Visibility.Hidden;
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInitDTD(DeliveryTimeDistribution tempSelectedDeliveryTimeDistribution)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitDTD..."), category: Category.Info, priority: Priority.Low);

                //IERMService ERMService = new ERMServiceController("localhost:6699");

                //DeliveryTimeDistribution temp = (ERMService.GetDeliveryTimeDistributionDetail_V2490(tempSelectedDeliveryTimeDistribution.Iddeliverytimedistribution));
                DeliveryTimeDistribution temp = (ERMService.GetDeliveryTimeDistributionDetail_V2530(tempSelectedDeliveryTimeDistribution.Iddeliverytimedistribution));

                tempLstDeliveryTimeDistributionModules = temp.LstDeliveryTimeDistributionModules;

                ClonedDTD = (DeliveryTimeDistribution)temp.Clone();

                IdDeliveryTimeDistribution = temp.Iddeliverytimedistribution;
                SelectedDTDCode = temp.Code;

                Description = temp.Description;

                Tempdescription = temp.Description;

                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;

                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;

                Name = temp.Name;
                Tempname = temp.Name;

                oldName = string.IsNullOrEmpty(Name) ? "" : Name;

                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;

                CreationDate = temp.CreationDate;

                if (temp.ExpiryDate != DateTime.MinValue)
                {
                    ExpiryDate = temp.ExpiryDate;
                }
                if (temp.EffectiveDate != DateTime.MinValue)
                {
                    EffectiveDate = temp.EffectiveDate;
                }
                if (temp.LastUpdated != DateTime.MinValue)
                {
                    LastUpdated = temp.LastUpdated;
                }

                if (temp.PlantList != null)
                {
                    if (SelectedPlant == null)
                        SelectedPlant = new List<object>();
                    if (temp.Plants == "ALL")
                    {
                        SelectedPlant.AddRange(PlantList);
                    }
                    else
                    {
                        foreach (Site plant in temp.PlantList)
                        {
                            SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plant.IdSite));
                        }
                    }


                    SelectedPlant = new List<object>(SelectedPlant);
                    SelectedPlant_Copy = new List<object>(SelectedPlant);
                }
                else if (!string.IsNullOrEmpty(temp.Plants))
                {
                    if (SelectedPlant == null)
                        SelectedPlant = new List<object>();

                    if (temp.PlantList == null)
                        temp.PlantList = new List<Site>();

                    foreach (Site plant in PlantList)
                    {
                        if (temp.Plants == "ALL")
                        {
                            temp.PlantList.Add(plant);
                            SelectedPlant.Add(plant);
                        }
                        else
                        {
                            if (temp.Plants.Contains(plant.Name))
                            {
                                temp.PlantList.Add(plant);
                                SelectedPlant.Add(plant);
                            }
                        }
                    }

                    SelectedPlant = new List<object>(SelectedPlant);
                    SelectedPlant_Copy = new List<object>(SelectedPlant);
                }

                if (ClonedDTD.IdStatus == 1536)
                    InActiveStatus = true;
                else
                    InActiveStatus = false;

                if (StatusList != null)
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);

                SelectedOriginalStatus = SelectedStatus;

                if (Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCopyName = true;
                }
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh)
                {
                    IsCopyDescription = true;
                }

                if (IsCopyName == true && IsCopyDescription == true)
                {
                    IsCheckedAllLanguages = true;
                }
                else
                {
                    IsCheckedAllLanguages = false;
                }
                oldSelectedLanguage = LanguageSelected.TwoLetterISOLanguage;

                //List<byte> TempIDCptypes = temp.LstDeliveryTimeDistributionModules.Select(i => i.IdCpType).Distinct().ToList();
                List<long> TempIDCptypes = temp.LstDeliveryTimeDistributionModules.Select(i => i.IdCpTypeNew).Distinct().ToList();
                
                string tempIdCptypes = string.Empty;
                if (TempIDCptypes != null)
                {
                    if (TempIDCptypes.Count > 0)
                    {
                        foreach (byte obj in TempIDCptypes)
                            if (string.IsNullOrEmpty(tempIdCptypes))
                            {
                                tempIdCptypes = Convert.ToString(obj);
                            }
                            else
                            {
                                tempIdCptypes = tempIdCptypes + "," + obj;

                            }

                    }

                }

                FillTemplateMenulist(tempIdCptypes);


                #region GEOS2-5272 pallavi jadhav
                //  tempLstStandardOperationsDictionaryChangeLogList = temp.LstStandardOperationsDictionaryChangeLogList;
                DeliveryTimeDistributionChangeLogList = new ObservableCollection<LogentriesbyDeliveryTimeDistribution>(temp.LstDeliveryTimeDistributionChangeLogList.OrderByDescending(x => x.IdLogEntryByDTD).ToList());

                #endregion

                oldIsCheckedAllLanguages = IsCheckedAllLanguages;

                GeosApplication.Instance.Logger.Log(string.Format("Method EditInitDTD()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDTD() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditInitDTD() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditInitDTD()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void ChangeCheckAllLanguagesCommandAction()
        {
            if (oldIsCheckedAllLanguages != null && oldIsCheckedAllLanguages != IsCheckedAllLanguages)
            { oldIsCheckedAllLanguages = IsCheckedAllLanguages; IsEnabledSaveButton = true; }
        }

        private void AcceptButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);

            try
            {
                if (DataTableForGridLayoutModules != null && IsAcceptableFlag == false)
                {
                    var tempGreaterone = (from row in DataTableForGridLayoutModules.AsEnumerable()
                                          where row.Field<string>("Total") != null && row.Field<string>("Total") != "" && row.Field<string>("Total") != "1"
                                          select row).ToList();
                    if (tempGreaterone == null || tempGreaterone.Count() == 0)
                    {
                        DTDRateList = new List<float>();
                        InformationError = null;
                        string error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                        // PropertyChanged(this, new PropertyChangedEventArgs("Total"));

                        if (string.IsNullOrEmpty(error))
                            InformationError = null;
                        else
                            InformationError = "";

                        if (error != null)
                        {
                            return;
                        }


                        if (SelectedPlant == null || SelectedPlant.Count == 0)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditDTDPlantMandatoryValidation").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...atleast one plant.", category: Category.Info, priority: Priority.Low);
                            return;
                        }

                        var showMessageForStatusIsActive = false;
                        if (IsNew)
                        {
                            // shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
                            if (SelectedStatus.IdLookupValue == 1535)
                            {
                                showMessageForStatusIsActive = true;
                            }
                        }
                        else
                        {
                            // shubham[skadam] GEOS2-3243 Add Standard Operations Dictionary (Information) (#ERM07) [1/2]
                            if (ClonedDTD != null && ClonedDTD.IdStatus != 1535 && SelectedStatus.IdLookupValue == 1535) showMessageForStatusIsActive = true;

                            if (ClonedDTD != null && SelectedStatus.IdLookupValue == 1535 && IsPlantAddedOrRemove) showMessageForStatusIsActive = true;
                        }

                        #region GEOS2-3904 Rupali Sarode
                        if (DXSplashScreen.IsActive)
                        {
                            DXSplashScreen.Close();
                            Mouse.OverrideCursor = Cursors.Arrow;
                        }
                        #endregion

                        if (showMessageForStatusIsActive)
                        {
                            var plantsNames = string.Join(", ", SelectedPlant.Select(x => ((Site)x).Name).ToList<string>().ToArray());
                            var msg = string.Format(System.Windows.Application.Current.FindResource("AddEditDTDStatusIsActive").ToString(), plantsNames);

                            CustomMessageBox.Show(msg, Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log($"Method AcceptButtonCommandAction()... {msg}", category: Category.Info, priority: Priority.Low);

                        }

                        if (ExpiryDate == null || EffectiveDate > ExpiryDate)
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditDTDEffectiveAndExpiryValidation").ToString()),
                                Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                            GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...Due Date is mandatory and it must be equal or greater than Effective Date.",
                                category: Category.Info, priority: Priority.Low);

                            return;
                        }

                        #region Modules

                        List<DeliveryTimeDistributionModules> deliveryTimeDistributionModulesList = new List<DeliveryTimeDistributionModules>();

                        //  DeliveryTimeDistributionModules deliveryTimeDistributionModulesInsert = new DeliveryTimeDistributionModules();

                        for (int i = 0; i < DataTableForGridLayoutModules.Rows.Count; i++)
                        {
                            DataRow row = DataTableForGridLayoutModules.Rows[i];

                            try
                            {


                                for (int j = 0; j < DTDStagesList.Count; j++)
                                {
                                    try
                                    {
                                        PlanningDateReviewStages item = DTDStagesList[j];


                                        DeliveryTimeDistributionModules deliveryTimeDistributionModules = tempLstDeliveryTimeDistributionModules.Where(w => w.IdDeliveryTimeDistribution == IdDeliveryTimeDistribution && w.IdCpTypeNew == Convert.ToInt64(row["IdCpType"]) && w.IdStage == item.IdStage).FirstOrDefault();


                                        if (item.IdStage != null && item.IdStage != 0)
                                        {
                                            string stageColumnName = "Stages_" + item.IdStage;
                                            if (row.Table.Columns.Contains(stageColumnName))
                                            {

                                                object valueObj = row[stageColumnName];
                                                CultureInfo Inculture = CultureInfo.InvariantCulture;
                                                CultureInfo usCulture = new CultureInfo("en-US");
                                                string ValueString = string.Empty;
                                                object value = valueObj;
                                                try
                                                {


                                                    if (valueObj != null)
                                                    {
                                                        ValueString = Convert.ToString(valueObj);
                                                        if (!string.IsNullOrEmpty(ValueString))
                                                        {
                                                            if (ValueString.Contains(','))
                                                            {
                                                                string separattorINvariant = usCulture.NumberFormat.NumberDecimalSeparator;
                                                                double TotalInDouble = Convert.ToDouble(ValueString.Replace(",", separattorINvariant), Inculture);
                                                                value = TotalInDouble;
                                                            }
                                                            else
                                                            {
                                                                double TotalInDouble = Convert.ToDouble(ValueString, Inculture);
                                                                value = TotalInDouble;
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }

                                                double TotalDTDRateFinal = 0;
                                                if (row["Total"] != DBNull.Value)
                                                {
                                                    TotalDTDRateFinal = Convert.ToDouble(row["Total"]);
                                                }

                                                if (deliveryTimeDistributionModules == null && value != DBNull.Value && !Convert.IsDBNull(value))
                                                {
                                                    if (TotalDTDRateFinal != 1)
                                                    {
                                                        //start[gulab lakade][validation][04 03 2024]
                                                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditDTDRateValidation").ToString()),
                                                        //                       Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        //end[gulab lakade][validation][04 03 2024]
                                                        return;

                                                    }

                                                    if (TotalDTDRateFinal == 1)
                                                    {
                                                        try
                                                        {


                                                            //  DeliveryTimeDistributionModules deliveryTimeDistributionModules = new DeliveryTimeDistributionModules();
                                                            DeliveryTimeDistributionModules deliveryTimeDistributionModulesInsert = new DeliveryTimeDistributionModules();

                                                            deliveryTimeDistributionModulesInsert.TransactionOperation = ModelBase.TransactionOperations.Add;
                                                            deliveryTimeDistributionModulesInsert.IdDeliveryTimeDistribution = iddeliverytimedistribution;
                                                            deliveryTimeDistributionModulesInsert.IdStage = item.IdStage;
                                                            deliveryTimeDistributionModulesInsert.DTDRate = Convert.ToSingle(value);
                                                            deliveryTimeDistributionModulesInsert.IdCpTypeNew = Convert.ToInt64(row["IdCpType"]);
                                                            deliveryTimeDistributionModulesInsert.CpTypeName = Convert.ToString(row["Template"]);
                                                            deliveryTimeDistributionModulesInsert.Code = item.StageCode;
                                                            deliveryTimeDistributionModulesInsert.CreatedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                            DTDRate = deliveryTimeDistributionModulesInsert.DTDRate;
                                                            DTDRateList.Add(DTDRate);
                                                            IdStage = deliveryTimeDistributionModulesInsert.IdStage;
                                                            IdCpTypeNew = deliveryTimeDistributionModulesInsert.IdCpTypeNew;
                                                            deliveryTimeDistributionModulesList.Add(deliveryTimeDistributionModulesInsert);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                    }

                                                }

                                                else if (deliveryTimeDistributionModules != null && value != DBNull.Value && !Convert.IsDBNull(value))
                                                {

                                                    if (TotalDTDRateFinal != 1)
                                                    {
                                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEditDTDRateValidation").ToString()),
                                                                               Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                                        return;

                                                    }

                                                    if (TotalDTDRateFinal == 1)
                                                    {
                                                        if (deliveryTimeDistributionModules.DTDRate != null)
                                                        {
                                                            try
                                                            {


                                                                string oldvalue = Convert.ToString(deliveryTimeDistributionModules.DTDRate);
                                                                string newvalue = Convert.ToString(value);
                                                                //if (deliveryTimeDistributionModules.DTDRate != Convert.ToDouble(value))
                                                                if (oldvalue != newvalue)
                                                                //if (deliveryTimeDistributionModules != null && deliveryTimeDistributionModules.DTDRate != null && value == DBNull.Value)
                                                                {
                                                                    try
                                                                    {


                                                                        DeliveryTimeDistributionModules deliveryTimeDistributionModulesUpdate = new DeliveryTimeDistributionModules();
                                                                        deliveryTimeDistributionModulesUpdate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                                        deliveryTimeDistributionModulesUpdate.IdDeliveryTimeDistribution = iddeliverytimedistribution;
                                                                        deliveryTimeDistributionModulesUpdate.IdDeliveryTimeDistributionModule = idDeliveryTimeDistributionModule;
                                                                        deliveryTimeDistributionModulesUpdate.IdDeliveryTimeDistribution = iddeliverytimedistribution;
                                                                        deliveryTimeDistributionModulesUpdate.IdStage = item.IdStage;
                                                                        deliveryTimeDistributionModulesUpdate.DTDRate = Convert.ToSingle(value);
                                                                        deliveryTimeDistributionModulesUpdate.IdCpTypeNew = Convert.ToInt64(row["IdCpType"]);
                                                                        deliveryTimeDistributionModulesUpdate.CpTypeName = Convert.ToString(row["Template"]);
                                                                        deliveryTimeDistributionModulesUpdate.Code = item.StageCode;
                                                                        deliveryTimeDistributionModulesUpdate.ModifiedBy = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                                                                        DTDRate = deliveryTimeDistributionModulesUpdate.DTDRate;
                                                                        DTDRateList.Add(DTDRate);
                                                                        IdStage = deliveryTimeDistributionModulesUpdate.IdStage;
                                                                        IdCpTypeNew = deliveryTimeDistributionModulesUpdate.IdCpTypeNew;
                                                                        deliveryTimeDistributionModulesList.Add(deliveryTimeDistributionModulesUpdate);
                                                                    }
                                                                    catch (Exception ex)
                                                                    {

                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                        }
                                                    }


                                                }
                                                //  else if (deliveryTimeDistributionModules != null && deliveryTimeDistributionModules.DTDRate.HasValue && value == DBNull.Value)
                                                else if (deliveryTimeDistributionModules != null && !float.IsNaN(deliveryTimeDistributionModules.DTDRate) && value == DBNull.Value)
                                                {

                                                    DeliveryTimeDistributionModules deliveryTimeDistributionModulesDelete = new DeliveryTimeDistributionModules();
                                                    deliveryTimeDistributionModulesDelete.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                                    deliveryTimeDistributionModulesDelete.IdDeliveryTimeDistribution = IdDeliveryTimeDistribution;
                                                    deliveryTimeDistributionModulesDelete.IdDeliveryTimeDistributionModule = IdDeliveryTimeDistributionModule;
                                                    deliveryTimeDistributionModulesDelete.IdStage = item.IdStage;
                                                    DTDRate = deliveryTimeDistributionModulesDelete.DTDRate;
                                                    IdStage = deliveryTimeDistributionModulesDelete.IdStage;
                                                    IdCpTypeNew = deliveryTimeDistributionModulesDelete.IdCpTypeNew;
                                                    deliveryTimeDistributionModulesDelete.IdCpTypeNew = Convert.ToInt64(row["IdCpType"]);
                                                    deliveryTimeDistributionModulesDelete.CpTypeName = Convert.ToString(row["Template"]);
                                                    deliveryTimeDistributionModulesDelete.Code = item.StageCode;
                                                    deliveryTimeDistributionModulesList.Add(deliveryTimeDistributionModulesDelete);
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    }
                                  
                            }catch(Exception  ex)
                            {

                            }


                        }

                        List<DeliveryTimeDistributionModules> deliveryTimeDistributionModulesDeleteList = new List<DeliveryTimeDistributionModules>();

                        if (deliveryTimeDistributionModulesDeleteList.Count() > 0)
                        {
                            foreach (var eRMDTDModuleDelete in deliveryTimeDistributionModulesDeleteList)
                            {
                                DeliveryTimeDistributionModules deliveryTimeDistributionModulesDelete = new DeliveryTimeDistributionModules();

                                deliveryTimeDistributionModulesDelete.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                deliveryTimeDistributionModulesDelete.IdDeliveryTimeDistribution = eRMDTDModuleDelete.IdDeliveryTimeDistribution;
                                deliveryTimeDistributionModulesDelete.IdStage = eRMDTDModuleDelete.IdStage;
                                deliveryTimeDistributionModulesDelete.IdCpTypeNew = eRMDTDModuleDelete.IdCpTypeNew;
                                deliveryTimeDistributionModulesList.Add(deliveryTimeDistributionModulesDelete);
                            }
                        }



                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                        if (IsNew)
                        {
                            #region New

                            NewDTD = new DeliveryTimeDistribution();

                            if (IsCheckedAllLanguages == true)
                            {
                                NewDTD.Name = Name.Trim();
                                NewDTD.Name_es = Name.Trim();
                                NewDTD.Name_fr = Name.Trim();
                                NewDTD.Name_pt = Name.Trim();
                                NewDTD.Name_ro = Name.Trim();
                                NewDTD.Name_ru = Name.Trim();
                                NewDTD.Name_zh = Name.Trim();
                                NewDTD.Description = Description == null ? "" : Description.Trim();
                                NewDTD.Description_es = Description == null ? "" : Description.Trim();
                                NewDTD.Description_fr = Description == null ? "" : Description.Trim();
                                NewDTD.Description_pt = Description == null ? "" : Description.Trim();
                                NewDTD.Description_ro = Description == null ? "" : Description.Trim();
                                NewDTD.Description_ru = Description == null ? "" : Description.Trim();
                                NewDTD.Description_zh = Description == null ? "" : Description == null ? "" : Description.Trim();
                            }
                            else
                            {
                                NewDTD.Name = Name_en == null ? "" : Name_en.Trim();
                                NewDTD.Name_es = Name_es == null ? "" : Name_es.Trim();
                                NewDTD.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                                NewDTD.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                                NewDTD.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                                NewDTD.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                                NewDTD.Name_zh = Name_zh == null ? "" : Name_zh.Trim();
                                NewDTD.Description = Description_en == null ? "" : Description_en.Trim();
                                NewDTD.Description_es = Description_es == null ? "" : Description_es.Trim();
                                NewDTD.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                                NewDTD.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                                NewDTD.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                                NewDTD.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                                NewDTD.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                            }

                            NewDTD.Code = SelectedDTDCode;

                            if (ExpiryDate != null)
                                NewDTD.ExpiryDate = Convert.ToDateTime(ExpiryDate);

                            if (EffectiveDate != null)
                                NewDTD.EffectiveDate = Convert.ToDateTime(EffectiveDate);

                            NewDTD.IdStatus = (uint)SelectedStatus.IdLookupValue;
                            NewDTD.Status = SelectedStatus;

                            NewDTD.PlantList = new List<Site>();

                            foreach (Site site in SelectedPlant)
                            {
                                NewDTD.PlantList.Add(site);
                            }
                            List<UInt32> idSites = NewDTD.PlantList.Select(i => i.IdSite).ToList();

                            NewDTD.Plants = string.Join("\n", NewDTD.PlantList.Select(x => x.Name).ToList<string>().ToArray());

                            NewDTD.CreationDate = DateTime.Now;
                            NewDTD.IdCreator = (uint)GeosApplication.Instance.ActiveUser.IdUser;

                            #region GEOS2-3855 gulab lakade
                            NewDTD.LstDeliveryTimeDistributionModules = deliveryTimeDistributionModulesList;
                            AddChangedDeliveryTimeDistributionLogDetails("New");
                            NewDTD.LstDeliveryTimeDistributionChangeLogList = DeliveryTimeDistributionAllChangeLogList.ToList();

                            #endregion

                            //NewDTD = ERMService.AddDeliveryTimeDistribution_V2490(NewDTD);
                            NewDTD = ERMService.AddDeliveryTimeDistribution_V2530(NewDTD);
                            NewDTD.LastUpdated = NewDTD.CreationDate;
                            ///RefreshModuleGrid();
                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DTDAddSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            IsNewDTDSave = true;
                            RequestClose(null, null);

                            #endregion

                        }
                        else
                        {

                            #region Update

                            UpdatedDTD = new DeliveryTimeDistribution();
                            UpdatedDTD.Iddeliverytimedistribution = IdDeliveryTimeDistribution;

                            if (IsCheckedAllLanguages == true)
                            {
                                UpdatedDTD.Name = Name.Trim();
                                UpdatedDTD.Name_es = Name.Trim();
                                UpdatedDTD.Name_fr = Name.Trim();
                                UpdatedDTD.Name_pt = Name.Trim();
                                UpdatedDTD.Name_ro = Name.Trim();
                                UpdatedDTD.Name_ru = Name.Trim();
                                UpdatedDTD.Name_zh = Name.Trim();
                                UpdatedDTD.Description = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_es = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_fr = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_pt = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_ro = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_ru = Description == null ? "" : Description.Trim();
                                UpdatedDTD.Description_zh = Description == null ? "" : Description == null ? "" : Description.Trim();
                            }
                            else
                            {
                                UpdatedDTD.Name = Name_en == null ? "" : Name_en.Trim();
                                UpdatedDTD.Name_es = Name_es == null ? "" : Name_es.Trim();
                                UpdatedDTD.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                                UpdatedDTD.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                                UpdatedDTD.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                                UpdatedDTD.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                                UpdatedDTD.Name_zh = Name_zh == null ? "" : Name_zh.Trim();
                                UpdatedDTD.Description = Description_en == null ? "" : Description_en.Trim();
                                UpdatedDTD.Description_es = Description_es == null ? "" : Description_es.Trim();
                                UpdatedDTD.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                                UpdatedDTD.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                                UpdatedDTD.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                                UpdatedDTD.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                                UpdatedDTD.Description_zh = Description_zh == null ? "" : Description_zh.Trim();
                            }

                            UpdatedDTD.Code = SelectedDTDCode;

                            if (ExpiryDate != null)
                                UpdatedDTD.ExpiryDate = Convert.ToDateTime(ExpiryDate);

                            if (ExpiryDate != null)
                                UpdatedDTD.EffectiveDate = Convert.ToDateTime(EffectiveDate);

                            UpdatedDTD.IdStatus = (uint)SelectedStatus.IdLookupValue;
                            UpdatedDTD.Status = SelectedStatus;

                            UpdatedDTD.PlantList = new List<Site>();

                            UpdatedDTD.ModificationDate = DateTime.Now;
                            UpdatedDTD.IdModifier = (uint)GeosApplication.Instance.ActiveUser.IdUser;

                            if (IsPlantAddedOrRemove == true)
                            {
                                //add/remove plant 
                                UpdatedDTD.PlantList = new List<Site>();
                                if (ClonedDTD.PlantList == null) ClonedDTD.PlantList = new List<Site>();
                                UpdatedDTD.PlantList_Cloned = ClonedDTD.PlantList;
                                UpdatedDTD.DeletePlantList = new List<Site>();
                                UpdatedDTD.AddPlantList = new List<Site>();

                                foreach (Site site in SelectedPlant)
                                {
                                    UpdatedDTD.PlantList.Add(site);
                                }
                                foreach (Site temp in UpdatedDTD.PlantList_Cloned)
                                {
                                    if (!UpdatedDTD.PlantList.Any(x => x.IdSite == temp.IdSite))
                                    {
                                        Site site_dlt = (Site)temp.Clone();
                                        site_dlt.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                        UpdatedDTD.DeletePlantList.Add(site_dlt);
                                    }
                                }
                                foreach (Site temp1 in UpdatedDTD.PlantList)
                                {
                                    if (!UpdatedDTD.PlantList_Cloned.Any(x => x.IdSite == temp1.IdSite))
                                    {
                                        Site site_add = (Site)temp1.Clone();
                                        site_add.TransactionOperation = ModelBase.TransactionOperations.Add;
                                        UpdatedDTD.AddPlantList.Add(site_add);
                                    }
                                }
                            }
                            else
                            {
                                UpdatedDTD.PlantList = ClonedDTD.PlantList;
                            }


                            UpdatedDTD.Plants = string.Join("\n", UpdatedDTD.PlantList.Select(x => x.Name).ToList<string>().ToArray());

                            //     UpdatedDTD.LstStandardOperationsDictionarySupplement = LstStandardOperationsDictionarySupplement.Where(i => i.TransactionOperation != ModelBase.TransactionOperations.Modify).ToList();

                            //[001]
                            //   UpdatedSOD.LstStandardOperationsDictionaryModules = standardOperationsDictionaryModulesList;



                            #region GEOS2-3855 gulab lakade
                            UpdatedDTD.LstDeliveryTimeDistributionModules = deliveryTimeDistributionModulesList;
                            AddChangedDeliveryTimeDistributionLogDetails("Update");
                            UpdatedDTD.LstDeliveryTimeDistributionChangeLogList = DeliveryTimeDistributionAllChangeLogList.ToList();

                            //  UpdatedSOD.LstStandardOperationsDictionaryChangeLogList = StandardOperationsDictionaryChangeLogList.ToList();
                            #endregion


                            //GEOS2 - 4033 Spare Part Gulab Lakade
                            //   IsUpdatedDTDSave = ERMService.UpdateStandardOperationsDictionary_V2340(UpdatedSOD);
                            //IsUpdatedDTDSave = ERMService.UpdateDeliveryTimeDistribution_V2490(UpdatedDTD);
                           
                            IsUpdatedDTDSave = ERMService.UpdateDeliveryTimeDistribution_V2530(UpdatedDTD);
                            UpdatedDTD.LastUpdated = UpdatedDTD.ModificationDate.Value;

                            //RefreshModuleGrid();
                            // if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); } //Rupali Sarode 15Sept2022

                            //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SODUpdateMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                            //Rupali Sarode 15Sept2022
                            try
                            {
                                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DTDUpdateMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, null);
                            }
                            catch (Exception ex)
                            {

                            }
                            #region GEOS2-3882
                            RefreshModuleGrid();
                            //  RefreshModuleGridAfterSave();
                            #endregion

                            if (SaveButtonVisibility == Visibility.Visible && !isSaveButtonClicked)
                            {
                                IsUpdatedDTDSave = true;
                                RequestClose(null, null);

                            }


                            #endregion

                        }
                        IsEnabledSaveButton = false;
                        //SaveButtonVisibility = Visibility.Hidden;              
                        //RefreshModuleGrid();

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SaveButtonCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                //   var tempselectedCurrentitem = Convert.ToString(Saveselecteditem);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                isSaveButtonClicked = true;
                AcceptButtonCommandAction(null);
                IsEnabledSaveButton = false;
                isSaveButtonClicked = false;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SaveButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method SaveButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void ToogleButtonCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ToogleButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (IsInformationVisible == true)
                {
                    IsInformationVisible = false;
                }
                else
                {
                    IsInformationVisible = true;
                }
                GeosApplication.Instance.Logger.Log("Method ToogleButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method ToogleButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ShowDescriptionViewCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowDescriptionViewCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowDescriptionViewCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowDescriptionViewCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in ShowDescriptionViewCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetMaximizedElement()
        {
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PLM_Appearance"))
            {
                if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PLM_Appearance"].ToString()))
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
                }
                else
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PLM_Appearance"].ToString(), true);
                }
            }
            else
            {
                SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
            }
        }

        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(83);
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1537);
                SelectedStatusIndex = StatusList.FindIndex(x => x.IdLookupValue == 1537);

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

        private void GetLatestDTDCode()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetLatestDTDCode()...", category: Category.Info, priority: Priority.Low);
                //IERMService ERMService = new ERMServiceController("localhost:6699");

                SelectedDTDCode = ERMService.GetLatestDTDCode_V2490();

                GeosApplication.Instance.Logger.Log("Method GetLatestDTDCode()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetLatestDTDCode() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetLatestDTDCode() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetLatestDTDCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);

                PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());

                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedAllLanguages == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description = Description_en;
                        Name = Name_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description = Description_es;
                        Name = Name_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description = Description_fr;
                        Name = Name_fr;

                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description = Description_pt;
                        Name = Name_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description = Description_ro;
                        Name = Name_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description = Description_ru;
                        Name = Name_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description = Description_zh;
                        Name = Name_zh;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method RetrieveDescriptionByLanguge()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method RetrieveDescriptionByLanguge()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetNameToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedAllLanguages == false)
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        InformationError = string.Empty; ;
                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    }
                    else
                    {
                        InformationError = null;
                        PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    }
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Name_en = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Name_es = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Name_fr = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Name_pt = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Name_ro = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Name_ru = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Name_zh = Name;
                    }
                }

                if (oldName == null)
                {
                    oldName = Name;

                }
                else
                {
                    if (oldName != Name)
                    {
                        oldName = Name; IsEnabledSaveButton = true;
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetNameToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetControlWidthUsingActualWidth(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetControlWidthUsingActualWidth..."), category: Category.Info, priority: Priority.Low);

                System.Windows.Controls.Control control = (System.Windows.Controls.Control)obj;
                control.Width = control.ActualWidth;

                GeosApplication.Instance.Logger.Log(string.Format("Method SetControlWidthUsingActualWidth()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetControlWidthUsingActualWidth() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedAllLanguages == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description_zh = Description;
                    }
                }
                if (oldDescription == null)
                {
                    oldDescription = Description;
                }
                else
                {
                    if (oldDescription != Description)
                    {
                        oldDescription = Description; IsEnabledSaveButton = true;
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Name = Name_en;
                    Description = Description_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Name = Name_es;
                    Description = Description_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Name = Name_fr;
                    Description = Description_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Name = Name_pt;
                    Description = Description_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Name = Name_ro;
                    Description = Description_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Name = Name_ru;
                    Description = Description_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Name = Name_zh;
                    Description = Description_zh;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method UncheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguages()...", category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PLMService.GetLanguages());
                LanguageSelected = Languages.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method FillLanguages()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                if (IsEnabledSaveButton && SaveButtonVisibility == Visibility.Visible)
                {
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        AcceptButtonCommandAction(null);
                    }
                }
                IsEnabledSaveButton = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangePlantCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Normal && SelectedPlant != SelectedPlant_Copy)
                {

                    if (SelectedPlant == null)
                    {
                        SelectedPlant = new List<object>();
                    }

                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    SelectedPlant_Copy = SelectedPlant;
                    IsPlantAddedOrRemove = true;
                    IsEnabledSaveButton = true;
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ChangePlantCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangePlantCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeStatusCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeStatusCommandAction()...", category: Category.Info, priority: Priority.Low);
                ComboBoxEdit value = ((System.Windows.RoutedEventArgs)obj).OriginalSource as ComboBoxEdit;

                if (ClonedDTD == null || value.DisplayText == SelectedStatus.Value)
                {

                    return;
                }

                IsEnabledSaveButton = true;
                GeosApplication.Instance.Logger.Log("Method ChangeStatusCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeStatusCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeEffectiveDateCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeEffectiveDateCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (oldEffectiveDate == null)
                {
                    oldEffectiveDate = EffectiveDate;
                }
                else
                {
                    if (oldEffectiveDate != EffectiveDate)
                    {
                        oldEffectiveDate = EffectiveDate; IsEnabledSaveButton = true;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeEffectiveDateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeEffectiveDateCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeExpiryDateCommandAction()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeExpiryDateCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (oldExpiryDate == null)
                {
                    oldExpiryDate = ExpiryDate;
                }
                else
                {
                    if (oldExpiryDate != expiryDate)
                    {

                        oldExpiryDate = ExpiryDate; IsEnabledSaveButton = true;

                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeExpiryDateCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeExpiryDateCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillTemplateMenulist(string idCptypes)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTemplateMenulist()...", category: Category.Info, priority: Priority.Low);
                //IERMService ERMService = new ERMServiceController("localhost:6699");

                TemplatesMenuList = new List<Template>(ERMService.GetAllTemplates_DTD_V2490().ToList().Where(a => a.IdTemplate == null || a.IdTemplate != 24).ToList());

                var List = TemplatesMenuList.GroupBy(info => info.IdTemplate)
                                .Select(group => new
                                {
                                    Name = TemplatesMenuList.FirstOrDefault(a => a.IdTemplate == group.Key).Name,
                                    Count = TemplatesMenuList.Where(b => b.IdTemplate == null && b.IdTemplate != 24).Count(),
                                }).ToList();


                //ProductTypesMenulistForGridLayout = new ObservableCollection<ProductTypes>(ERMService.GetAllProductTypes_DTD_V2490(idCptypes).ToList().Where(a => a.Template == null || a.Template.IdTemplate != 24).ToList());
                ProductTypesMenulistForGridLayout = new ObservableCollection<ProductTypes>(ERMService.GetAllProductTypes_DTD_V2530(idCptypes).ToList().Where(a => a.Template == null || a.Template.IdTemplate != 24).ToList());

                if (TemplateWithCPTypes == null)
                    TemplateWithCPTypes = new ObservableCollection<TemplateWithCPTypes>();

                if (TemplateWithCPTypes.Count() == 0)
                {
                    // Sort templates, placing "NO GROUP" last
                    TemplatesMenuList = new List<Template>(
                        TemplatesMenuList
                            .OrderBy(template => template.Name == "NO GROUP")
                            .ThenBy(template => template.Name, new AlphanumericComparer())
                    );

                    foreach (var item in TemplatesMenuList)
                    {
                        TemplateWithCPTypes templateWithCPTypes = new TemplateWithCPTypes();

                        templateWithCPTypes.ProductTypesMenu = ProductTypesMenulistForGridLayout
                            .Where(w => w.Template != null && w.Template.Name.Equals(item.Name))
                            .OrderBy(option => option.Name, new AlphanumericComparer())
                            .ToList();

                        foreach (var item1 in templateWithCPTypes.ProductTypesMenu)
                        {
                            int CPTypecount = tempLstDeliveryTimeDistributionModules
                                .Where(x => x.IdCpTypeNew == (long)item1.IdCPType)
                                .ToList()
                                .Count();
                            if (CPTypecount > 0)
                                item1.Name = item1.Name + "  (" + 1 + ")";
                            else
                                item1.Name = item1.Name + "  (" + 0 + ")";
                        }


                        // Add count to the name
                        templateWithCPTypes.Name = $"{item.Name}  ({templateWithCPTypes.ProductTypesMenu.Count()})";

                        TemplateWithCPTypes.Add(templateWithCPTypes);
                    }
                }

                SelectedTemplate = TemplateWithCPTypes.First();

                GeosApplication.Instance.Logger.Log("Method FillTemplateMenulist()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplateMenulist() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in Method FillTemplateMenulist() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectItemForTemplate()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectItemForTemplate()...", category: Category.Info, priority: Priority.Low);



                GeosApplication.Instance.Logger.Log("Method SelectItemForTemplate()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectItemForTemplate()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void HidePanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HidePanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisible == Visibility.Collapsed)
                    IsAccordionControlVisible = Visibility.Visible;
                else
                    IsAccordionControlVisible = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HidePanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void RefreshModuleGrid()
        {
            try
            {
                //DeliveryTimeDistribution temp = (ERMService.GetDeliveryTimeDistributionDetail_V2490(ClonedDTD.Iddeliverytimedistribution));
                DeliveryTimeDistribution temp = (ERMService.GetDeliveryTimeDistributionDetail_V2530(ClonedDTD.Iddeliverytimedistribution));


                ClonedDTD = (DeliveryTimeDistribution)temp.Clone();

                Description = temp.Description;
                oldDescription = string.IsNullOrEmpty(Description) ? "" : Description;
                Description_en = temp.Description;
                Description_es = temp.Description_es;
                Description_fr = temp.Description_fr;
                Description_pt = temp.Description_pt;
                Description_ro = temp.Description_ro;
                Description_ru = temp.Description_ru;
                Description_zh = temp.Description_zh;

                Name = temp.Name;
                oldName = string.IsNullOrEmpty(Name) ? "" : Name;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;

                if (StatusList != null)
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.IdStatus);


                SelectedOriginalStatus = SelectedStatus;


                tempLstDeliveryTimeDistributionModules = new List<DeliveryTimeDistributionModules>();
                tempLstDeliveryTimeDistributionModules = temp.LstDeliveryTimeDistributionModules;

                DeliveryTimeDistributionChangeLogList = new ObservableCollection<LogentriesbyDeliveryTimeDistribution>(temp.LstDeliveryTimeDistributionChangeLogList.OrderByDescending(x => x.IdLogEntryByDTD).ToList());
                DeliveryTimeDistributionAllChangeLogList = null;
                DeliveryTimeDistributionAllChangeLogList = new ObservableCollection<LogentriesbyDeliveryTimeDistribution>();

                object oldSelectedTemplate = SelectedTemplate;



                TemplateWithCPTypes = null;
                TemplateWithCPTypes = new ObservableCollection<TemplateWithCPTypes>();
                if (TemplateWithCPTypes.Count() == 0)
                {

                    foreach (var item in TemplatesMenuList)
                    {
                        TemplateWithCPTypes templateWithCPTypes = new TemplateWithCPTypes();

                        templateWithCPTypes.ProductTypesMenu = ProductTypesMenulistForGridLayout
                            .Where(w => w.Template != null && w.Template.Name.Equals(item.Name))
                            .OrderBy(option => option.Name, new AlphanumericComparer())
                            .ToList();


                        foreach (var item1 in templateWithCPTypes.ProductTypesMenu)
                        {
                            int CPTypecount = tempLstDeliveryTimeDistributionModules
                                .Where(x => x.IdCpTypeNew == (long)item1.IdCPType)
                                .ToList()
                                .Count();
                            if (CPTypecount > 0)
                                item1.Name = (item1.Name.Contains("(0)")) ? item1.Name.Replace("(0)", "(1)") : item1.Name;
                            else if (CPTypecount == 0)
                                item1.Name = (item1.Name.Contains("(1)")) ? item1.Name.Replace("(1)", "(0)") : item1.Name;
                        }


                        // Add count to the name
                        templateWithCPTypes.Name = $"{item.Name}  ({templateWithCPTypes.ProductTypesMenu.Count()})";

                        TemplateWithCPTypes.Add(templateWithCPTypes);
                    }
                }


                //SelectedTemplateFlag = true;
                IsEnabledSaveButton = false;
               // SelectedTemplate = TemplateWithCPTypes.Where(i => i.Name == TemplateHeader).FirstOrDefault();
               //    SelectedTemplate = ProductTypesMenulistForGridLayout.Where(x => x.IdCPType == tempSelectedModule.IdCPType).FirstOrDefault();
                DTDAccordion.ItemsSource = TemplateWithCPTypes;
             //   SelectedTemplate = oldSelectedTemplate;
                
                foreach (TemplateWithCPTypes objAccordion in DTDAccordion.Items)
                {

                    if (objAccordion.Name == TemplateHeader)
                    {
                        DTDAccordion.ExpandItem(objAccordion);
                        break;
                    }
                    
                }
                //DTDAccordion.ExpandItem(SelectedTemplate);
                

            }
            catch (Exception ex)
            {

            }
        }


        public class AlphanumericComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                int chunkXStart = 0, chunkYStart = 0;

                while (chunkXStart < x.Length && chunkYStart < y.Length)
                {
                    // Find the start of the next chunk in each string
                    int chunkXEnd = chunkXStart + 1;
                    while (chunkXEnd < x.Length && char.IsDigit(x[chunkXEnd]) == char.IsDigit(x[chunkXStart]))
                        chunkXEnd++;

                    int chunkYEnd = chunkYStart + 1;
                    while (chunkYEnd < y.Length && char.IsDigit(y[chunkYEnd]) == char.IsDigit(y[chunkYStart]))
                        chunkYEnd++;

                    // Compare the current chunks
                    int result;
                    if (char.IsDigit(x[chunkXStart]) && char.IsDigit(y[chunkYStart]))
                    {
                        // If both chunks are numeric, compare as integers
                        result = int.Parse(x.Substring(chunkXStart, chunkXEnd - chunkXStart))
                            .CompareTo(int.Parse(y.Substring(chunkYStart, chunkYEnd - chunkYStart)));
                    }
                    else
                    {
                        // If at least one chunk is non-numeric, compare as strings
                        result = string.Compare(x, chunkXStart, y, chunkYStart, chunkXEnd - chunkXStart);
                    }

                    // If chunks are different, return the comparison result
                    if (result != 0)
                        return result;

                    // Move to the next chunks
                    chunkXStart = chunkXEnd;
                    chunkYStart = chunkYEnd;
                }

                // If one string is a prefix of the other, return the length comparison
                return x.Length - y.Length;
            }
        }



        //List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement_Cloned = new List<StandardOperationsDictionarySupplement>();
        // List<LogentriesbyStandardOperationsDictionary> tempLstStandardOperationsDictionaryChangeLogList = new List<LogentriesbyStandardOperationsDictionary>();

        public void AddChangedDeliveryTimeDistributionLogDetails(string LogValue)
        {
            try
            {
                DeliveryTimeDistributionAllChangeLogList = new ObservableCollection<LogentriesbyDeliveryTimeDistribution>();

                GeosApplication.Instance.Logger.Log("Method AddChangedDeliveryTimeDistributionLogDetails()...", category: Category.Info, priority: Priority.Low);
                try
                {
                    var temp = newList;
                    if (DeliveryTimeDistributionAllChangeLogList == null)
                        DeliveryTimeDistributionAllChangeLogList = new ObservableCollection<LogentriesbyDeliveryTimeDistribution>();
                    if (LogValue == "New")
                    {
                        #region 
                        string log = "Delivery Time Distribution " + Convert.ToString(SelectedDTDCode) + " has been added.";
                        DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        #endregion
                    }
                    else if (LogValue == "Update")
                    {


                        if (SelectedPlant != null && ClonedDTD.PlantList != null)
                        {
                            var Addplant = SelectedPlant.Select(x => ((Site)x).Name).Except(ClonedDTD.PlantList.Select(a => a.Name)).ToList();
                            if (Addplant.Count > 0)
                            {
                                foreach (var item in Addplant)
                                {
                                    string log = "The Plant " + Convert.ToString(item) + " has been added.";
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });

                                }
                            }
                            var Removeplant = ClonedDTD.PlantList.Select(a => a.Name).Except(SelectedPlant.Select(x => ((Site)x).Name)).ToList();
                            if (Removeplant.Count > 0)
                            {

                                foreach (var item in Removeplant)
                                {

                                    string log = "The Plant " + Convert.ToString(item) + " has been removed.";
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });

                                }
                            }

                        }


                        #region Name
                        if (IsCheckedAllLanguages == true)
                        {
                            //if ((Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh) )
                            //{
                            if ((Name_en != ClonedDTD.Name_es || Name_en != ClonedDTD.Name_fr || Name_en != ClonedDTD.Name_pt || Name_en != ClonedDTD.Name_ro || Name_en != ClonedDTD.Name_ru || Name_en != ClonedDTD.Name_zh))
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_en) && !string.IsNullOrEmpty(ClonedDTD.Name))
                                {
                                    log = "The Name for all language has been changed from " + Convert.ToString(ClonedDTD.Name) + " to " + Convert.ToString(Name) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_en) && !string.IsNullOrEmpty(ClonedDTD.Name))
                                {
                                    log = "The Name for all language has been changed from " + Convert.ToString(ClonedDTD.Name) + " to None.";
                                }
                                else
                                    if (!string.IsNullOrEmpty(Name_en) && string.IsNullOrEmpty(ClonedDTD.Name))
                                {
                                    log = "The Name for all language has been changed from None to " + Convert.ToString(Name) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }

                            }

                            //}

                        }
                        else
                        {
                            if (Name_en != ClonedDTD.Name)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ClonedDTD.Name))
                                {
                                    log = "The Name EN has been changed from " + Convert.ToString(ClonedDTD.Name) + " to " + Convert.ToString(Name) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name))
                                {
                                    log = "The Name EN has been changed from " + Convert.ToString(ClonedDTD.Name) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name))
                                {
                                    log = "The Name EN has been changed from None to " + Convert.ToString(Name) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_es != ClonedDTD.Name_es)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_es) && !string.IsNullOrEmpty(ClonedDTD.Name_es))
                                {
                                    log = "The Name ES has been changed from " + Convert.ToString(ClonedDTD.Name_es) + " to " + Convert.ToString(Name_es) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_es))
                                {
                                    log = "The Name ES has been changed from " + Convert.ToString(ClonedDTD.Name_es) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_es))
                                {
                                    log = "The Name ES has been changed from None to " + Convert.ToString(Name_es) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_fr != ClonedDTD.Name_fr)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_fr) && !string.IsNullOrEmpty(ClonedDTD.Name_fr))
                                {
                                    log = "The Name FR has been changed from " + Convert.ToString(ClonedDTD.Name_fr) + " to " + Convert.ToString(Name_fr) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_fr))
                                {
                                    log = "The Name FR has been changed from " + Convert.ToString(ClonedDTD.Name_fr) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_fr))
                                {
                                    log = "The Name FR has been changed from None to " + Convert.ToString(Name_fr) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_pt != ClonedDTD.Name_pt)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_pt) && !string.IsNullOrEmpty(ClonedDTD.Name_pt))
                                {
                                    log = "The Name PT has been changed from " + Convert.ToString(ClonedDTD.Name_pt) + " to " + Convert.ToString(Name_pt) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_pt))
                                {
                                    log = "The Name PT has been changed from " + Convert.ToString(ClonedDTD.Name_pt) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_pt))
                                {
                                    log = "The Name PT has been changed from None to " + Convert.ToString(Name_pt) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_ro != ClonedDTD.Name_ro)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_ro) && !string.IsNullOrEmpty(ClonedDTD.Name_ro))
                                {
                                    log = "The Name RO has been changed from " + Convert.ToString(ClonedDTD.Name_ro) + " to " + Convert.ToString(Name_ro) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_ro))
                                {
                                    log = "The Name RO has been changed from " + Convert.ToString(ClonedDTD.Name_ro) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_ro))
                                {
                                    log = "The Name RO has been changed from None to " + Convert.ToString(Name_ro) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_ru != ClonedDTD.Name_ru)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_ru) && !string.IsNullOrEmpty(ClonedDTD.Name_ru))
                                {
                                    log = "The Name RU has been changed from " + Convert.ToString(ClonedDTD.Name_ru) + " to " + Convert.ToString(Name_ru) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_ru))
                                {
                                    log = "The Name RU has been changed from " + Convert.ToString(ClonedDTD.Name_ru) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_ru))
                                {
                                    log = "The Name RU has been changed from None to " + Convert.ToString(Name_ru) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Name_zh != ClonedDTD.Name_zh)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Name_zh) && !string.IsNullOrEmpty(ClonedDTD.Name_zh))
                                {
                                    log = "The Name ZH has been changed from " + Convert.ToString(ClonedDTD.Name_zh) + " to " + Convert.ToString(Name_zh) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Name_zh))
                                {
                                    log = "The Name ZH has been changed from " + Convert.ToString(ClonedDTD.Name_zh) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Name_zh))
                                {
                                    log = "The Name ZH has been changed from None to " + Convert.ToString(Name_zh) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }
                        }
                        #endregion

                        #region Description
                        if (IsCheckedAllLanguages == true)
                        {
                            if (Description_en != ClonedDTD.Description_es || Description_en != ClonedDTD.Description_fr || Description_en != ClonedDTD.Description_pt || Description_en != ClonedDTD.Description_ro || Description_en != ClonedDTD.Description_ru || Description_en != ClonedDTD.Description_zh)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Convert.ToString(ClonedDTD.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                                {
                                    log = "The Description for all language has been changed from " + Convert.ToString(ClonedDTD.Description) + " to " + Convert.ToString(Description) + ".";
                                }
                                else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedDTD.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                                {
                                    log = "The Description for all language has been changed from None to " + Convert.ToString(Description) + ".";
                                }
                                else
                                    if (!string.IsNullOrEmpty(Convert.ToString(ClonedDTD.Description)) && string.IsNullOrEmpty(Convert.ToString(Description)))
                                {
                                    log = "The Description for all language has been changed from " + Convert.ToString(ClonedDTD.Description) + " to None.";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                        }
                        else
                        {
                            if (Description_en != ClonedDTD.Description)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(ClonedDTD.Description))
                                {
                                    log = "The Description EN has been changed from " + Convert.ToString(ClonedDTD.Description) + " to " + Convert.ToString(Description) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description))
                                {
                                    log = "The Description EN has been changed from " + Convert.ToString(ClonedDTD.Description) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description))
                                {
                                    log = "The Description EN has been changed from None to " + Convert.ToString(Description) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Description_es != ClonedDTD.Description_es)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_es) && !string.IsNullOrEmpty(ClonedDTD.Description_es))
                                {
                                    log = "The Description ES has been changed from " + Convert.ToString(ClonedDTD.Description_es) + " to " + Convert.ToString(Description_es) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_es))
                                {
                                    log = "The Description ES has been changed from " + Convert.ToString(ClonedDTD.Description_es) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_es))
                                {
                                    log = "The Description ES has been changed from None to " + Convert.ToString(Description_es) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Description_fr != ClonedDTD.Description_fr)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_fr) && !string.IsNullOrEmpty(ClonedDTD.Description_fr))
                                {
                                    log = "The Description FR has been changed from " + Convert.ToString(ClonedDTD.Description_fr) + " to " + Convert.ToString(Description_fr) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_fr))
                                {
                                    log = "The Description FR has been changed from " + Convert.ToString(ClonedDTD.Description_fr) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_fr))
                                {
                                    log = "The Description FR has been changed from None to " + Convert.ToString(Description_fr) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }
                            if (Description_pt != ClonedDTD.Description_pt)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_pt) && !string.IsNullOrEmpty(ClonedDTD.Description_pt))
                                {
                                    log = "The Description PT has been changed from " + Convert.ToString(ClonedDTD.Description_pt) + " to " + Convert.ToString(Description_pt) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_pt))
                                {
                                    log = "The Description PT has been changed from " + Convert.ToString(ClonedDTD.Description_pt) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_pt))
                                {
                                    log = "The Description PT has been changed from None to " + Convert.ToString(Description_pt) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Description_ro != ClonedDTD.Description_ro)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_ro) && !string.IsNullOrEmpty(ClonedDTD.Description_ro))
                                {
                                    log = "The Description RO has been changed from " + Convert.ToString(ClonedDTD.Description_ro) + " to " + Convert.ToString(Description_ro) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_ro))
                                {
                                    log = "The Description RO has been changed from " + Convert.ToString(ClonedDTD.Description_ro) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_ro))
                                {
                                    log = "The Description RO has been changed from None to " + Convert.ToString(Description_ro) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Description_ru != ClonedDTD.Description_ru)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_ru) && !string.IsNullOrEmpty(ClonedDTD.Description_ru))
                                {
                                    log = "The Description RU has been changed from " + Convert.ToString(ClonedDTD.Description_ru) + " to " + Convert.ToString(Description_ru) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_ru))
                                {
                                    log = "The Description RU has been changed from " + Convert.ToString(ClonedDTD.Description_ru) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_ru))
                                {
                                    log = "The Description RU has been changed from None to " + Convert.ToString(Description_ru) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Description_zh != ClonedDTD.Description_zh)
                            {
                                string log = string.Empty;
                                if (!string.IsNullOrEmpty(Description_zh) && !string.IsNullOrEmpty(ClonedDTD.Description_zh))
                                {
                                    log = "The Description ZH has been changed from " + Convert.ToString(ClonedDTD.Description_zh) + " to " + Convert.ToString(Description_zh) + ".";
                                }
                                else
                                    if (string.IsNullOrEmpty(Description_zh))
                                {
                                    log = "The Description ZH has been changed from " + Convert.ToString(ClonedDTD.Description_zh) + " to None.";
                                }
                                else
                                    if (string.IsNullOrEmpty(ClonedDTD.Description_zh))
                                {
                                    log = "The Description ZH has been changed from None to " + Convert.ToString(Description_zh) + ".";
                                }
                                if (!string.IsNullOrEmpty(log))
                                {
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }
                        }
                        #endregion

                        #region 

                        if (!string.IsNullOrEmpty(Convert.ToString(SelectedStatus.Value)) && (!string.IsNullOrEmpty(Convert.ToString(SelectedOriginalStatus.Value))) && (Convert.ToString(SelectedStatus.Value) != Convert.ToString(SelectedOriginalStatus.Value)))
                        {
                            string log = "The Status has been changed from " + Convert.ToString(Convert.ToString(SelectedOriginalStatus.Value)) + " to " + Convert.ToString(SelectedStatus.Value) + ".";
                            DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        }
                        #endregion

                        if ((Convert.ToString(EffectiveDate) != Convert.ToString(ClonedDTD.EffectiveDate)))
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(EffectiveDate)) && (!string.IsNullOrEmpty(Convert.ToString(ClonedDTD.EffectiveDate))))
                            {
                                if (Convert.ToString(ClonedDTD.EffectiveDate.ToString("dd-MM-yyyy")) == "01-01-0001")
                                {
                                    DateTime Effectivedate = Convert.ToDateTime(EffectiveDate);
                                    log = "The Effective Date has been changed from None to " + Effectivedate.ToString("dd-MM-yyyy") + ".";
                                }
                                else
                                {
                                    DateTime Effectivedate = Convert.ToDateTime(EffectiveDate);
                                    log = "The Effective Date has been changed from " + Convert.ToString(ClonedDTD.EffectiveDate.ToString("dd-MM-yyyy")) + " to " + Effectivedate.ToString("dd-MM-yyyy") + ".";
                                }

                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(EffectiveDate)) && !string.IsNullOrEmpty(Convert.ToString(ClonedDTD.EffectiveDate)))
                            {
                                if (Convert.ToString(ClonedDTD.EffectiveDate.ToString("dd-MM-yyyy")) != "01-01-0001")
                                {
                                    log = "The Effective Date has been changed from " + Convert.ToString(ClonedDTD.EffectiveDate.ToString("dd-MM-yyyy")) + " to None.";
                                }

                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedDTD.EffectiveDate)))
                            {
                                if (Convert.ToString(EffectiveDate) != "01-01-0001" || !string.IsNullOrEmpty(Convert.ToString(EffectiveDate)))
                                {
                                    DateTime Effectivedate = Convert.ToDateTime(EffectiveDate);
                                    log = "The Effective Date has been changed from None to " + Effectivedate.ToString("dd-MM-yyyy") + ".";
                                }
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }

                        }

                        if ((Convert.ToString(ExpiryDate) != Convert.ToString(ClonedDTD.ExpiryDate)))
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(ExpiryDate)))
                            {
                                if (Convert.ToString(ClonedDTD.ExpiryDate.ToString("dd-MM-yyyy")) == "01-01-0001")
                                {
                                    DateTime Expirydate = Convert.ToDateTime(ExpiryDate);
                                    log = "The Due Date has been changed from None to " + Expirydate.ToString("dd-MM-yyyy") + ".";
                                }
                                else
                                {
                                    DateTime Expirydate = Convert.ToDateTime(ExpiryDate);
                                    log = "The Due Date has been changed from " + Convert.ToString(Convert.ToString(ClonedDTD.ExpiryDate.ToString("dd-MM-yyyy"))) + " to " + Expirydate.ToString("dd-MM-yyyy") + ".";
                                }

                            }
                            else
                            if (string.IsNullOrEmpty(Convert.ToString(ExpiryDate)))
                            {
                                if (Convert.ToString(ClonedDTD.ExpiryDate.ToString("dd-MM-yyyy")) != "01-01-0001")
                                {
                                    log = "The Due Date has been changed from " + Convert.ToString(Convert.ToString(ClonedDTD.ExpiryDate.ToString("dd-MM-yyyy"))) + " to None.";
                                }

                            }
                            else
                                if (string.IsNullOrEmpty(Convert.ToString(ClonedDTD.ExpiryDate)))
                            {
                                if (Convert.ToString(ExpiryDate) != "01-01-0001" || !string.IsNullOrEmpty(Convert.ToString(ExpiryDate)))
                                {
                                    DateTime Expirydate = Convert.ToDateTime(ExpiryDate);
                                    log = "The Due Date has been changed from None to " + Expirydate.ToString("dd-MM-yyyy") + ".";
                                }


                            }

                            if (!string.IsNullOrEmpty(log))
                            {
                                DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        #region stages
                        if (UpdatedDTD.LstDeliveryTimeDistributionModules.Count > 0)
                        {
                            foreach (var item1 in UpdatedDTD.LstDeliveryTimeDistributionModules)
                            {
                                if (item1.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                {
                                    string log = "In Module " + Convert.ToString(item1.CpTypeName).Trim() + " DTD Rate for stage " + item1.Code + " has been deleted.";
                                    DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                                else
                                {
                                    var RemoveStage = ClonedDTD.LstDeliveryTimeDistributionModules.Where(a => a.DTDRate != item1.DTDRate && a.IdDeliveryTimeDistribution == IdDeliveryTimeDistribution && a.IdStage == item1.IdStage && a.IdCpTypeNew == item1.IdCpTypeNew).ToList();
                                    if (RemoveStage.Count > 0)
                                    {
                                        foreach (var item in RemoveStage)
                                        {
                                            string log = "In Module " + Convert.ToString(item1.CpTypeName).Trim() + " DTD Rate for the Stage " + Convert.ToString(item1.Code) + " has been changed from " + Convert.ToString(item.DTDRate) + " to " + Convert.ToString(item1.DTDRate);
                                            DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }
                                    }
                                    else
                                    {
                                        string log = "In Module " + Convert.ToString(item1.CpTypeName).Trim() + " for the Stage " + Convert.ToString(item1.Code) + " DTD Rate " + Convert.ToString(item1.DTDRate) + " has been added.";
                                        DeliveryTimeDistributionAllChangeLogList.Add(new LogentriesbyDeliveryTimeDistribution() { IdDeliveryTimeDistribution = idDeliveryTimeDistribution, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });

                                    }
                                }
                            }
                        }
                        #endregion

                    }

                }
                catch (Exception ex) { }

                GeosApplication.Instance.Logger.Log("Method AddChangedDeliveryTimeDistributionLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedDeliveryTimeDistributionLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void HideLogPanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HideLogPanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisibleLogs == Visibility.Collapsed)
                    IsAccordionControlVisibleLogs = Visibility.Visible;
                else
                    IsAccordionControlVisibleLogs = Visibility.Collapsed;
                GeosApplication.Instance.Logger.Log("Method HideLogPanel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HideLogPanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                me[BindableBase.GetPropertyName(() => Name)] +
                me[BindableBase.GetPropertyName(() => InformationError)];
                // me[BindableBase.GetPropertyName(() => Total)];


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
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);
                string total = BindableBase.GetPropertyName(() => Total);

                if (columnName == name)
                {
                    return AddEditDTDValidation.GetErrorMessage(name, Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditDTDValidation.GetErrorMessage(headerInformtionError, InformationError);
                }

                if (columnName == total)
                {
                    return AddEditDTDValidation.GetErrorMessage(total, Total);
                }


                return null;
            }
        }

        #endregion



        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()...", category: Category.Info, priority: Priority.Low);
                string TimePart = string.Empty;
                string DatePart = string.Empty;
                DatePart = DateTime.Now.ToShortDateString().Replace("/", "");
                DatePart = DatePart.Replace("-", "");
                DatePart = DatePart.Replace(".", "");

                TimePart = DateTime.Now.ToShortTimeString().Replace(":", "");
                TimePart = TimePart.Replace(" AM", "");
                TimePart = TimePart.Replace(" PM", "");

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                // saveFile.FileName = "ERMHistory_" + SelectedSODCode + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
                saveFile.FileName = "ERMHistory_" + SelectedDTDCode + "_" + DatePart + "_" + TimePart;
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

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
                    ResultFileName = (saveFile.FileName);
                    TableView ChangeLogTableView = ((TableView)obj);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);

                    //Wrap Text

                    var workbook = new Workbook();

                    workbook.LoadDocument(ResultFileName, DocumentFormat.Xlsx);
                    Worksheet worksheet = workbook.Worksheets[0];

                    workbook.BeginUpdate();

                    workbook.Worksheets[0].Columns[1].Width = 450;
                    workbook.Worksheets[0].Columns[2].Alignment.WrapText = true;
                    workbook.Worksheets[0].Columns[2].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                    workbook.EndUpdate();

                    workbook.SaveDocument(ResultFileName);

                    //

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void AddColumnsToDataTablewithBandsinModules()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTablewithBandsinModules ...", category: Category.Info, priority: Priority.Low);

        //        DataTableForGridLayoutModules = new DataTable();
        //        Bands = new ObservableCollection<BandItem>(); Bands.Clear();

        //        string TempTemplateName = string.Empty;

        //        if (SelectedTemplate != null)
        //        {
        //            if (SelectedTemplate is TemplateWithCPTypes)
        //            {
        //                TemplateWithCPTypes templateName = (TemplateWithCPTypes)SelectedTemplate;
        //                TempTemplateName = templateName.Name;
        //            }

        //        }
        //        BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = TempTemplateName, AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };

        //        band0.Columns = new ObservableCollection<ColumnItem>();
        //        band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Template", HeaderText = "", Width = 120, IsVertical = false, DTDSetting = DTDColumnTemplateSelector.DTDSettingType.TemplateName, Visible = true });

        //        Bands.Add(band0);

        //        DataTableForGridLayoutModules.Columns.Add("Template", typeof(string));

        //        DataTableForGridLayoutModules.Columns.Add("IdTemplate", typeof(Int32));

        //        DataTableForGridLayoutModules.Columns.Add("IdCPType", typeof(Int32));

        //        BandItem band1 = new BandItem() { BandName = "Stages", BandHeader = "Stages", Visible = true };
        //        band1.Columns = new ObservableCollection<ColumnItem>();

        //        foreach (PlanningDateReviewStages item in DTDStagesList)
        //        {
        //            band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Stages" + item.IdStage, Width = 60, HeaderText = item.StageCode, DTDSetting = DTDColumnTemplateSelector.DTDSettingType.Stages, Visible = true });

        //            DataTableForGridLayoutModules.Columns.Add("Stages" + item.IdStage, typeof(float));
        //        }

        //        Bands.Add(band1);

        //        Bands = new ObservableCollection<BandItem>(Bands);

        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTablewithBandsinModules executed Successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTablewithBandsinModules() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

        //    }

        //}

        private void AddColumnsToDataTablewithBandsinModules()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTablewithBandsinModules ...", category: Category.Info, priority: Priority.Low);

                DataTableForGridLayoutModules = new DataTable();
                Bands = new ObservableCollection<BandItem>(); Bands.Clear();

                // string TempTemplateName = string.Empty;

                if (SelectedTemplate != null)
                {
                    if (SelectedTemplate is TemplateWithCPTypes)
                    {
                        TemplateWithCPTypes templateName = (TemplateWithCPTypes)SelectedTemplate;
                        TemplateHeader = templateName.Name;

                        //int index = TemplateHeader.IndexOf('(');
                        //TemplateHeader = index >= 0 ? TemplateHeader.Substring(0, index) : TemplateHeader;

                    }
                }

                BandItem band0 = new BandItem() { BandName = "FirstRow", BandHeader = "", AllowBandMove = false, FixedStyle = FixedStyle.Left, Visible = true, AllowMoving = DevExpress.Utils.DefaultBoolean.False };

                band0.Columns = new ObservableCollection<ColumnItem>();
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "Template", HeaderText = "", Width = 170, IsVertical = false, DTDSetting = DTDColumnTemplateSelector.DTDSettingType.TemplateName, Visible = true });
                band0.Columns.Add(new ColumnItem() { ColumnFieldName = "IdCPtype", HeaderText = "", Width = 120, IsVertical = false, DTDSetting = DTDColumnTemplateSelector.DTDSettingType.TemplateName, Visible = true });

                Bands.Add(band0);

                DataTableForGridLayoutModules.Columns.Add("Template", typeof(string));

                DataTableForGridLayoutModules.Columns.Add("IdCPtype", typeof(Int32));
                DataTableForGridLayoutModules.Columns.Add("FlagError", typeof(string)); //start[gulab lakade][validation][04 03 2024]
                DataTableForGridLayoutModules.Columns.Add("FlagIsActive", typeof(bool));
                DataTableForGridLayoutModules.Columns.Add("IsDeleteDTD", typeof(bool)); //start[gulab lakade][validation][04 03 2024]

                BandItem band1 = new BandItem() { BandName = "Stages", BandHeader = "Stages", Visible = true };
                band1.Columns = new ObservableCollection<ColumnItem>();

                foreach (PlanningDateReviewStages item in DTDStagesList)
                {
                    band1.Columns.Add(new ColumnItem() { ColumnFieldName = "Stages_" + item.IdStage, Width = 45, HeaderText = item.StageCode, DTDSetting = DTDColumnTemplateSelector.DTDSettingType.Stages, Visible = true });

                    //DataTableForGridLayoutModules.Columns.Add("Stages_" + item.IdStage, typeof(float));
                    DataTableForGridLayoutModules.Columns.Add("Stages_" + item.IdStage, typeof(string));
                }
                Bands.Add(band1);

                BandItem band2 = new BandItem() { BandName = "Total", BandHeader = "Total", Visible = true };
                band2.Columns = new ObservableCollection<ColumnItem>();

                band2.Columns.Add(new ColumnItem() { ColumnFieldName = "Total", Width = 55, HeaderText = "", DTDSetting = DTDColumnTemplateSelector.DTDSettingType.Total, Visible = true });

                Total = "Total";

                DataTableForGridLayoutModules.Columns.Add("Total", typeof(string));
                //start[gulab lakade][validation][04 03 2024]
                band2.Columns.Add(new ColumnItem() { ColumnFieldName = "Error", Width = 45, HeaderText = "", DTDSetting = DTDColumnTemplateSelector.DTDSettingType.Error, Visible = true });

                DataTableForGridLayoutModules.Columns.Add("Error", typeof(string));
                //end[gulab lakade][validation][04 03 2024]
                Bands.Add(band2);

                BandItem band3 = new BandItem() { BandName = "", BandHeader = "", Visible = true };
                band3.Columns = new ObservableCollection<ColumnItem>();
                band3.Columns.Add(new ColumnItem() { ColumnFieldName = "DTDDelete", Width = 45, HeaderText = "", DTDSetting = DTDColumnTemplateSelector.DTDSettingType.DTDDelete, Visible = true });

                Bands = new ObservableCollection<BandItem>(Bands);
                Bands.Add(band3);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddColumnsToDataTablewithBandsinModules executed Successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in AddColumnsToDataTablewithBandsinModules() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }

        }
        #region Comment code
        //For selected plants, create stages list as per ActiveInPlants
        //private void CreateStagesList()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method CreateStagesList ...", category: Category.Info, priority: Priority.Low);


        //        TempDTDStagesList = new List<PlanningDateReviewStages>();

        //        TempDTDStagesList.AddRange(ERMService.GetProductionPlanningReviewStage_V2400());

        //        DTDStagesList = new List<PlanningDateReviewStages>();
        //        string[] ArrActiveInPlants;
        //        bool flagAddStage = false;

        //        if (SelectedPlant != null)
        //        {
        //            foreach (Site objPlant in SelectedPlant)
        //            {
        //                foreach (PlanningDateReviewStages objStage in TempDTDStagesList)
        //                {
        //                    flagAddStage = false;

        //                    if (objStage.ActiveInPlants == null || objStage.ActiveInPlants == "")
        //                    {
        //                        flagAddStage = true;
        //                    }
        //                    else
        //                    {
        //                        ArrActiveInPlants = objStage.ActiveInPlants.Split(',');
        //                        if (ArrActiveInPlants.Contains(Convert.ToString(objPlant.IdSite)))
        //                        {
        //                            flagAddStage = true;
        //                        }
        //                    }

        //                    if (flagAddStage == true)
        //                    {
        //                        if (!DTDStagesList.Exists(i => i.IdStage == objStage.IdStage))
        //                        {
        //                            DTDStagesList.Add(objStage);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        GeosApplication.Instance.Logger.Log("Method CreateStagesList executed Successfully", category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in CreateStagesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        #endregion Comment Code

        private void GetStagesList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetStagesList ...", category: Category.Info, priority: Priority.Low);

                DTDStagesList = new List<PlanningDateReviewStages>();

                DTDStagesList.AddRange(ERMService.GetProductionPlanningReviewStage_V2400());
                GeosApplication.Instance.Logger.Log("Method GetStagesList executed Successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in GetStagesList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        public void RetrieveDTDRateByModule()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RetrieveDTDRateByModule()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                DataTable DataTableForGridLayoutModulesCopy = new DataTable();

                if (IsEnabledSaveButton && SaveButtonVisibility == Visibility.Visible)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        //ISSavechanges = true;
                        IsEnabledSaveButton = false;
                        SaveButtonCommandAction();
                        //ISSavechanges = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    }
                }

                if (DataTableForGridLayoutModules != null)
                {
                    DataTableForGridLayoutModules.Clear();

                    DataTableForGridLayoutModulesCopy = DataTableForGridLayoutModules.Copy();

                    if (SelectedTemplate is TemplateWithCPTypes)
                    {
                        TemplateWithCPTypes templateName = (TemplateWithCPTypes)SelectedTemplate;
                        TemplateHeader = templateName.Name;

                        DataRow drow;

                        List<ProductTypes> TemplateNameList = templateName.ProductTypesMenu.ToList();
                        foreach (var objCpType in templateName.ProductTypesMenu.ToList())
                        {

                            List<DeliveryTimeDistributionModules> tempDeliveryTimeDistributionModules = tempLstDeliveryTimeDistributionModules.Where(w => w.IdCpTypeNew == (long)objCpType.IdCPType).ToList();
                            //  TemplateHeader = productTypes.Template.Name;


                            DataRow[] drExists;

                            string stage = string.Empty;

                            if (tempDeliveryTimeDistributionModules.Count == 0)
                            {
                                drow = DataTableForGridLayoutModulesCopy.NewRow();
                                int index = objCpType.Name.IndexOf('(');
                                string CPTypeName = objCpType.Name;
                                CPTypeName = index >= 0 ? CPTypeName.Substring(0, index) : CPTypeName;

                                drow["Template"] = CPTypeName;
                                drow["IdCPType"] = objCpType.IdCPType;
                                if (objCpType.IdStatus == 223)
                                    drow["FlagIsActive"] = true;
                                else
                                    drow["FlagIsActive"] = false;
                                DataTableForGridLayoutModulesCopy.Rows.Add(drow);
                            }

                            else if (tempDeliveryTimeDistributionModules.Count > 0)
                            {
                                foreach (DeliveryTimeDistributionModules objDTDModule in tempDeliveryTimeDistributionModules)
                                {
                                    drExists = DataTableForGridLayoutModulesCopy.Select("IdCPtype = " + objDTDModule.IdCpTypeNew);
                                    if (drExists.Length == 0)
                                        drow = DataTableForGridLayoutModulesCopy.NewRow();
                                    else
                                        drow = drExists.FirstOrDefault();


                                    drow["IdCPType"] = objDTDModule.IdCpTypeNew;

                                    int index = objDTDModule.CpTypeName.IndexOf('(');
                                    string CPTypeName = objDTDModule.CpTypeName;
                                    CPTypeName = index >= 0 ? CPTypeName.Substring(0, index) : CPTypeName;

                                    drow["Template"] = CPTypeName;
                                    if (objDTDModule.IdStatus == 223)
                                        drow["FlagIsActive"] = true;
                                    else
                                        drow["FlagIsActive"] = false;

                                    stage = "Stages_" + objDTDModule.IdStage;

                                    if (DataTableForGridLayoutModulesCopy.Columns.Contains(stage))
                                        drow[stage] = Convert.ToString(objDTDModule.DTDRate);

                                    if (drExists.Length == 0)
                                    {
                                        DataTableForGridLayoutModulesCopy.Rows.Add(drow);
                                    }
                                }
                            }
                        }
                    }

                    else if (SelectedTemplate is ProductTypes)
                    {
                        ProductTypes productTypes = (ProductTypes)SelectedTemplate;

                        List<DeliveryTimeDistributionModules> tempDeliveryTimeDistributionModules = tempLstDeliveryTimeDistributionModules.Where(w => w.IdCpTypeNew == (long)productTypes.IdCPType).ToList();
                        // TemplateHeader = productTypes.Template.Name;
                        TemplateHeader = TemplateWithCPTypes.Where(i => i.Name.Contains(productTypes.Template.Name)).FirstOrDefault().Name;

                        DataRow drow;
                        DataRow[] drExists;

                        string stage = string.Empty;

                        if (tempDeliveryTimeDistributionModules.Count == 0)
                        {
                            drow = DataTableForGridLayoutModulesCopy.NewRow();

                            int index = productTypes.Name.IndexOf('(');
                            string CPTypeName = productTypes.Name;
                            CPTypeName = index >= 0 ? CPTypeName.Substring(0, index) : CPTypeName;

                            drow["Template"] = CPTypeName;
                            drow["IdCPType"] = productTypes.IdCPType;
                            if (productTypes.IdStatus == 223)
                                drow["FlagIsActive"] = true;
                            else
                                drow["FlagIsActive"] = false;

                            DataTableForGridLayoutModulesCopy.Rows.Add(drow);
                        }

                        else if (tempDeliveryTimeDistributionModules.Count > 0)
                        {
                            foreach (DeliveryTimeDistributionModules objDTDModule in tempDeliveryTimeDistributionModules)
                            {
                                drExists = DataTableForGridLayoutModulesCopy.Select("IdCPtype = " + objDTDModule.IdCpTypeNew);
                                if (drExists.Length == 0)
                                    drow = DataTableForGridLayoutModulesCopy.NewRow();
                                else
                                    drow = drExists.FirstOrDefault();


                                drow["IdCPType"] = objDTDModule.IdCpTypeNew;

                                int index = objDTDModule.CpTypeName.IndexOf('(');
                                string CPTypeName = objDTDModule.CpTypeName;
                                CPTypeName = index >= 0 ? CPTypeName.Substring(0, index) : CPTypeName;

                                drow["Template"] = CPTypeName;
                                if (objDTDModule.IdStatus == 223)
                                    drow["FlagIsActive"] = true;
                                else
                                    drow["FlagIsActive"] = false;

                                stage = "Stages_" + objDTDModule.IdStage;

                                if (DataTableForGridLayoutModulesCopy.Columns.Contains(stage))
                                    drow[stage] = Convert.ToString(objDTDModule.DTDRate);

                                if (drExists.Length == 0)
                                {
                                    DataTableForGridLayoutModulesCopy.Rows.Add(drow);
                                }
                            }
                        }

                    }


                    //Add Total to Total Column

                    foreach (DataRow dr in DataTableForGridLayoutModulesCopy.Rows)
                    {
                        decimal TotalDTDRate = 0;
                        bool Flag = false;

                        foreach (PlanningDateReviewStages item in DTDStagesList)
                        {
                            string StageColumn = "Stages_" + item.IdStage;
                            if (dr[StageColumn] != DBNull.Value)
                            {
                                // TotalDTDRate = TotalDTDRate + Math.Round(Convert.ToDouble(dr[StageColumn]), 2);
                                TotalDTDRate = TotalDTDRate + Convert.ToDecimal(dr[StageColumn]);

                                Flag = true;
                            }
                        }
                        //start[gulab lakade][validation][04 03 2024]
                        if (Flag == true)   // If all stages contain null, then total should display null
                        {
                            // dr["Total"] = TotalDTDRate;
                            dr["Total"] = Convert.ToDouble(TotalDTDRate);
                            if (TotalDTDRate > 1)
                            {
                                dr["FlagError"] = "Visible";
                            }
                            else if (TotalDTDRate == null || TotalDTDRate == 0 || TotalDTDRate == 1)
                            {
                                dr["FlagError"] = "Hidden";
                            }
                            else
                            {
                                dr["FlagError"] = "Visible";
                            }
                            dr["IsDeleteDTD"] = "true";
                        }
                        else
                        {

                            dr["FlagError"] = "Hidden";
                            dr["IsDeleteDTD"] = "false";
                        }
                        //end[gulab lakade][validation]

                    }


                    DataTableForGridLayoutModules = DataTableForGridLayoutModulesCopy;
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveDTDRateByModule()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveDTDRateByModule() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        private void DTDRateValueChangedCommandAction(Object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DTDRateValueChangedCommandAction()...", category: Category.Info, priority: Priority.Low);
                CultureInfo CurrentCulture = new CultureInfo(System.Globalization.CultureInfo.CurrentCulture.ToString());
                if (obj == null)
                    return;

                DataRow dRow;
                DataRow Row = ((System.Data.DataRowView)((DevExpress.Xpf.Grid.RowEventArgs)obj).Row).Row;

                dRow = DataTableForGridLayoutModules.Select("IdCPtype = " + Convert.ToInt32(Row.ItemArray[1])).FirstOrDefault();

                double TotalRate = 0;
                //decimal TotalRate = 0;
                string ColumnName = string.Empty;
                bool checkNull = false;

                foreach (PlanningDateReviewStages item in DTDStagesList)
                {
                    ColumnName = "Stages_" + item.IdStage;
                    if (string.IsNullOrEmpty(dRow[ColumnName].ToString()))
                    {
                        TotalRate = TotalRate + 0;

                    }
                    else
                    {
                        //TotalRate = Math.Round(TotalRate + Math.Round(Convert.ToDouble(dRow[ColumnName]), 2), 1);
                        string separator = CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        string tempCellvalue = Convert.ToString(dRow[ColumnName]);
                        if (tempCellvalue.Contains(','))
                        {
                            tempCellvalue = tempCellvalue.Replace(",", separator);
                        }
                        else if (tempCellvalue.Contains('.'))
                        {
                            tempCellvalue = tempCellvalue.Replace(".", separator);
                        }
                        dRow[ColumnName] = tempCellvalue;
                        //TotalRate = TotalRate + Convert.ToDecimal(dRow[ColumnName]);
                        CultureInfo Inculture = CultureInfo.InvariantCulture;
                        CultureInfo usCulture = new CultureInfo("en-US");
                        double TotalInDouble = 0;
                        if (tempCellvalue.Contains(','))
                        {
                            string separattorINvariant = usCulture.NumberFormat.NumberDecimalSeparator;
                            TotalInDouble = Convert.ToDouble(tempCellvalue.Replace(",", separattorINvariant), Inculture);

                        }
                        else
                        {
                            TotalInDouble = Convert.ToDouble(tempCellvalue, Inculture);
                        }

                        double cellValue = Math.Round(Convert.ToDouble(TotalInDouble), 2);
                        TotalRate = TotalRate + cellValue;
                        checkNull = true;
                    }

                }

                string string_TotalRate = Convert.ToString(TotalRate, CurrentCulture);
                decimal decimal_TotalRate = Convert.ToDecimal(string_TotalRate);

                //if (TotalRate > 1)
                if (decimal_TotalRate > Convert.ToDecimal(1))
                {
                    // ShowError = Visibility.Visible;

                    Row["Total"] = Convert.ToString(TotalRate, CurrentCulture);
                    Total = TotalRate.ToString(CurrentCulture);
                    //Row["FlagError"] = true;
                    Row["FlagError"] = "Visible";//start[gulab lakade][validation][04 03 2024]

                }
                else if (decimal_TotalRate == null || decimal_TotalRate == 1)
                {
                    //ShowError = Visibility.Hidden;
                    if (checkNull == true)
                    {
                        Row["Total"] = Convert.ToString(TotalRate, CurrentCulture);
                    }
                    else
                    {
                        Row["Total"] = null;
                    }
                    //Row["FlagError"] = false;
                    Row["FlagError"] = "Hidden";//start[gulab lakade][validation][04 03 2024]
                    Total = TotalRate.ToString();
                }
                else
                {
                    //ShowError = Visibility.Hidden;
                    if (checkNull == true)
                    {
                        Row["Total"] = Convert.ToString(TotalRate, CurrentCulture);
                        Row["FlagError"] = "Visible";//start[gulab lakade][validation][04 03 2024]
                    }
                    else
                    {
                        Row["Total"] = null;
                        Row["FlagError"] = "Hidden";//start[gulab lakade][validation][04 03 2024]
                    }
                    //Row["FlagError"] = false;
                    //Row["FlagError"] = "Visible";//start[gulab lakade][validation][04 03 2024]
                    Total = TotalRate.ToString(CurrentCulture);
                }
                if (checkNull == true)
                {
                    Row["IsDeleteDTD"] = "true";//start[gulab lakade][validation][04 03 2024]
                }
                else
                {
                    Row["IsDeleteDTD"] = "false";
                }

                //Row["Total"] = TotalRate;

                IsEnabledSaveButton = true;

                GeosApplication.Instance.Logger.Log("Method DTDRateValueChangedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DTDRateValueChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }


        private void DTDRateValidateCellCommandAction(GridCellValidationEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DTDRateValidateCellCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsAcceptableFlag = false;

                if (e.Value.ToString() == "." || e.Value.ToString() == ",")
                    return;

                CultureInfo culture = CultureInfo.InvariantCulture;
                double cellValue = Math.Round(Convert.ToDouble(e.Value, culture), 2);

                //double cellValue = Convert.ToDouble(e.Value, 2);
                //float cellValue = (float)e.Value;
                if (cellValue >= 0 && cellValue <= 1)
                    return;

                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = $"DTD rate must be >= 0 and <= 1.";
                IsEnabledSaveButton = false;
                IsAcceptableFlag = true;

                GeosApplication.Instance.Logger.Log("Method DTDRateValidateCellCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DTDRateValidateCellCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void DTDRateCellValueChangingCommandAction(CellValueChangedEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DTDRateCellValueChangingCommandAction()...", category: Category.Info, priority: Priority.Low);
                //CultureInfo CurrentCulture = new CultureInfo(System.Globalization.CultureInfo.CurrentCulture.ToString());
                //DataRow row = ((DataRowView)e.Row).Row;
                //string tempvalue =Convert.ToString(e.Value);
                //if(tempvalue.Contains(',') )
                //{
                //    string tempchar = CurrentCulture.NumberFormat.NumberDecimalSeparator;
                //    tempvalue = tempvalue.Replace(",", tempchar);
                //}
                //else if(tempvalue.Contains('.'))
                //{
                //    string tempchar = CurrentCulture.NumberFormat.NumberDecimalSeparator;
                //    tempvalue = tempvalue.Replace(".", tempchar);
                //}
                //row[e.Column.FieldName] = tempvalue;

                GeosApplication.Instance.Logger.Log("Method DTDRateCellValueChangingCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DTDRateCellValueChangingCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        private void DTDRateValidateRowCommandAction(GridRowValidationEventArgs e)
        {


        }

        private void DeleteModuleCommandAction(object obj)
        {
            try
            {
                //  DeliveryTimeDistributionModules MainRow = ((DeliveryTimeDistributionModules)obj);

                DataRowView MainRow = (DataRowView)obj;

                GeosApplication.Instance.Logger.Log("Method DeleteModuleControlCommandAction()...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["DeliveryTimeDistributionDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {

                    IsEnabledSaveButton = true;
                    SaveButtonVisibility = Visibility.Visible;


                    DeliveryTimeDistributionModules deliveryTimeDistributionModulesDelete = new DeliveryTimeDistributionModules();
                    if (MainRow != null)
                    {


                        string Condition = "IdCpType = '" + Convert.ToByte(MainRow.Row["IdCpType"]) + "'";

                        DataRow selectedRows = DataTableForGridLayoutModules.Select(Condition).FirstOrDefault();

                        if (selectedRows != null)
                        {
                            foreach (PlanningDateReviewStages item in DTDStagesList)
                            {
                                if (MainRow.Row["Stages_" + item.IdStage] != DBNull.Value)
                                {

                                    deliveryTimeDistributionModulesDelete.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    deliveryTimeDistributionModulesDelete.IdDeliveryTimeDistribution = IdDeliveryTimeDistribution;
                                    deliveryTimeDistributionModulesDelete.IdCpTypeNew = Convert.ToByte(MainRow.Row["IdCpType"]);
                                    deliveryTimeDistributionModulesDelete.IdStage = item.IdStage;
                                    deliveryTimeDistributionModulesDeleteList.Add(deliveryTimeDistributionModulesDelete);

                                    selectedRows["Stages_" + item.IdStage] = DBNull.Value;
                                }
                            }
                            selectedRows["Total"] = DBNull.Value;
                            selectedRows["IsDeleteDTD"] = "false";
                        }

                        DataTableForGridLayoutModules.AcceptChanges();

                    }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DeliveryTimeDistributionDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteModuleControlCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteModuleControlCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteModuleControlCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteModuleControlCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        private void GridLoadCommandAction (object obj)
        {
            DTDAccordion = (AccordionControl)obj;
            
            foreach (TemplateWithCPTypes objAccordion in DTDAccordion.Items)
            {
                
              //  objAccordion.Name = "";
                DTDAccordion.ExpandItem(objAccordion);
                return;
            }

        }

        #endregion
    }
}
