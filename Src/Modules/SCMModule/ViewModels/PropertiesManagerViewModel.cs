using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    ///[SCM][nsatpute][29/06/2023] SCM - Properties Manager (1/4) https://helpdesk.emdep.com/browse/GEOS2-4498
    public class PropertiesManagerViewModel : ViewModelBase, INotifyPropertyChanged, IDisposable
    {
        //[nsatpute][10-07-2023] SCM - Properties Manager (2/4) GEOS2-4501

        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private ObservableCollection<Family> listFamily;
        private Family selectedistFamily;
        private ObservableCollection<ConnectorProperties> listConnectorProperties;
        private string language;
        private Family selectedFamily;//[Sudhir.Jangra][GEOS2-4500][10/07/2023]
        private bool isPropertyExpand; //[nsatpute][GEOS2-4501][11/07/2023]
        private ConnectorProperties selectedConnectorProperty; //[nsatpute][GEOS2-4499][13/07/2023]
        private Visibility visibleIsEnabled; //[nsatpute][GEOS2-4499][13/07/2023]
        private Visibility visibleDefaultValue;  //[nsatpute][GEOS2-4499][13/07/2023]
        private Visibility visibleCustomTextDefaultValue;
        private Visibility visibleLabelDefaultValue; //[nsatpute][GEOS2-4501][20/07/2023]
        private Visibility visibleMinValue; //[nsatpute][GEOS2-4499][13/07/2023]
        private Visibility visibleMaxValue; //[nsatpute][GEOS2-4499][13/07/2023]
        private ObservableCollection<Color> listColor;
        private Color selectedColor;
        private Visibility clearColor;
        private Visibility clearGender;
        private Visibility visibleColorDefaultValue;
        private Visibility visibleGenderDefaultValue;
        private Visibility visibleSealingDefaultValue;
        private Visibility visibleCustomboolDefaultValue;
        private bool isCheckedSealing;
        private bool isPropertyEnabled; //[nsatpute][GEOS2-4501][20/07/2023]
        private bool isInternalEnable;
        private ObservableCollection<Gender> listGender;
        private Gender selectedGender;
        //[nsatpute][GEOS2-4501][20/07/2023]
        private List<ConnectorProperties> _lstDetailsToSave;
        private string defaultValue;
        private string minValue;
        private string maxValue;
        private double dialogHeight;
        private double dialogWidth;
        string windowHeader;
        public bool IsPropertyChecked;
        private Family currentFamily;//[Sudhir.Jangra][GEOS2-4501]
        private string checkselectedvalue;//[gulab lakade][27 07 2023]
        private bool isEditButtonEnabled;//[Sudhir.Jangra][GEOS2-4502]
        private bool isAddButtonEnabled;//[Sudhir.Jangra]
        public bool CheckUncheckSelectedFamiles = false;
        public bool CheckUncheckSelectedConnector = false;
        private TreeListView propertyTreeListView;// [gulab lakade][propertytreelist view]
        private string connectorPropertyFilterString;
        #region rajashri GEOS2-5227 
        // private Visibility visibleIsEnabled; 
        private Visibility visibleCustomStringValue;
        private Visibility visibleCustomListValue;
        private Visibility visibleCustomBooleanValue;
        private ObservableCollection<LookUpValues> customListData;
        private Visibility clearCustomList;
        private Visibility visibleNumberDefaultValue;
        private LookUpValues selectedCustomList;
        List<GeosSettings> geosSettings;
        private bool isCheckedCustom;
        ObservableCollection<ValueKey> valueKey;
        //private Visibility visibleCustomStringDefaultValue;
        #endregion
        private bool isSCMEditPropertiesManager;//[pramod.misal][GEOS2-5483][27.05.2024]
        private bool isSCMEditPropertiesManagerBtn;//[pramod.misal][GEOS2-5483][27.05.2024]
        #endregion

        #region Properties
        //[pramod.misal][GEOS2-5483][27.05.2024]
        public bool IsSCMEditPropertiesManager
        {
            get { return isSCMEditPropertiesManager; }
            set
            {

                isSCMEditPropertiesManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditPropertiesManager"));

            }
        }
        //[pramod.misal][GEOS2-5483][27.05.2024]
        public bool IsSCMEditPropertiesManagerBtn
        {
            get { return isSCMEditPropertiesManagerBtn; }
            set
            {

                isSCMEditPropertiesManagerBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditPropertiesManagerBtn"));

            }
        }
        private List<ConnectorProperties> lstDetailsToSave
        {
           
            get
            {
                return _lstDetailsToSave;
            }
            set
            {
                _lstDetailsToSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("lstDetailsToSave"));
            }
        }
        public TreeListView PropertyTreeListView    // [gulab lakade][propertytreelist view]
        {
            get { return propertyTreeListView; }
            set
            {

                propertyTreeListView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PropertyTreeListView"));

            }
        }
        public ObservableCollection<Family> ListFamily
        {
            get
            {
                return listFamily;
            }
            set
            {
                listFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListFamily"));
            }
        }
        public ObservableCollection<ConnectorProperties> ListConnectorProperties
        {
            get { return listConnectorProperties; }
            set { listConnectorProperties = value; OnPropertyChanged(new PropertyChangedEventArgs("ListConnectorProperties")); }
        }
        //[nsatpute][GEOS2-4499][15/07/2023]
        public ObservableCollection<Color> ListColor
        {
            get { return listColor; }
            set { listColor = value; OnPropertyChanged(new PropertyChangedEventArgs("ListColor")); }
        }
        public bool boolFamily = false;
        public Family SelectedFamily
        {
            get
            {

                return selectedFamily;

            }
            set
            {

                selectedFamily = value;
                //CheckUncheckSelectedFamiles = false;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamily"));
                // Checkselectedvalue = "Family";
                //SelectedConnectorProperty = new ConnectorProperties();

                if (SelectedFamily.IsChecked == false)
                {
                    boolFamily = true;
                    selectedFamily.IsChecked = true;

                }
                if (SelectedConnectorProperty != null)
                {
                    SelectedConnectorProperty = new ConnectorProperties();

                }
                ConnectorPropertyFilterString = string.Empty;
                //SelectedFamiles = new ObservableCollection<Family>();

                // //SelectedFamiles.Add(selectedFamily);
                // //if (SelectedFamiles.Where(x => x.Id == selectedFamily.Id).ToList().Count == 0)
                // //{
                //     SelectedFamiles.Add(selectedFamily);
                // //}
                LoadActions();
                LoadProperties();
                HideActionControls();
                //PropertyTreeListView.ExpandAllNodes();
                //IsPropertyExpand = false;
                //CheckUncheckSelectedFamiles = false;
                if (PropertyTreeListView != null)
                {
                    if (ListConnectorProperties == null || ListConnectorProperties.Count == 0)
                    {
                        FillConnectorProperties();
                    }

                    PropertyTreeListView.ExpandAllNodes();
                    IsPropertyExpand = false;

                }
            }
        }
        private ObservableCollection<Family> selectedFamiles;
        public ObservableCollection<Family> SelectedFamiles
        {
            get { return selectedFamiles; }
            set
            {
                selectedFamiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamiles"));
                //Checkselectedvalue = "Family";

                //LoadActions();
                //LoadProperties();
            }
        }
        //[nsatpute][GEOS2-4499][13/07/2023]
        //[nsatpute][GEOS2-4501][20/07/2023]
        public ConnectorProperties SelectedConnectorProperty
        {
            get { return selectedConnectorProperty; }
            set
            {
                selectedConnectorProperty = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorProperty"));
               // LoadCheckedProperty();
                ReloadActionGrid(SelectedConnectorProperty);
                FillCustomList();
                Checkselectedvalue = "Connector";
                LoadActions();
                LoadEditButton();
                LoadAddButton();

            }
        }
        public ObservableCollection<Gender> ListGender
        {
            get { return listGender; }
            set
            {
                listGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGender"));
            }
        }
        public Visibility ClearGender
        {
            get { return clearGender; }
            set { clearGender = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearGender")); }
        }
      
        public Gender SelectedGender
        {
            get { return selectedGender; }
            set
            {
                selectedGender = value;
                if (value == null)
                {
                    ClearGender = Visibility.Hidden;
                    //[rdixit][29.11.2023][GEOS2-4955]
                    if (SelectedConnectorProperty.IdConnectorProperty == 3)
                    {
                        var temp = lstDetailsToSave.FirstOrDefault(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty == 3);
                        if (temp != null)
                        {
                            temp.DefaultValue = "";
                            temp.CanSaveRecord = "Save";
                        }
                    }               
                }
                    
                else
                    ClearGender = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGender"));
                if (SelectedGender != null)
                    AddDefaultValue(Convert.ToString(value.Id));
            }
        }
        public Visibility VisibleIsEnabled
        {
            get { return visibleIsEnabled; }
            set { visibleIsEnabled = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleIsEnabled")); }
        }
        //[nsatpute][GEOS2-4499][15/07/2023]
        public Visibility VisibleTextDefaultValue
        {
            get { return visibleDefaultValue; }
            set { visibleDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleTextDefaultValue")); }
        }
        public Visibility VisibleCustomTextDefaultValue
        {
            get { return visibleCustomTextDefaultValue; }
            set { visibleCustomTextDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleCustomTextDefaultValue")); }
        }
        //[nsatpute][GEOS2-4501][20/07/2023]
        public Visibility VisibleLabelDefaultValue
        {
            get { return visibleLabelDefaultValue; }
            set { visibleLabelDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleLabelDefaultValue")); }
        }
        public Visibility VisibleColorDefaultValue
        {
            get { return visibleColorDefaultValue; }
            set { visibleColorDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleColorDefaultValue")); }
        }
        public Visibility VisibleGenderDefaultValue
        {
            get { return visibleGenderDefaultValue; }
            set { visibleGenderDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleGenderDefaultValue")); }
        }
        public Visibility VisibleSealingDefaultValue
        {
            get { return visibleSealingDefaultValue; }
            set { visibleSealingDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleSealingDefaultValue")); }
        }
        public Visibility VisibleCustomboolDefaultValue
        {
            get { return visibleCustomboolDefaultValue; }
            set { visibleCustomboolDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleCustomboolDefaultValue")); }
        }
        public Visibility VisibleMinValue
        {
            get { return visibleMinValue; }
            set { visibleMinValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleMinValue")); }
        }
        public Visibility VisibleMaxValue
        {
            get { return visibleMaxValue; }
            set { visibleMaxValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleMaxValue")); }
        }
  
        //[nsatpute][GEOS2-4501][20/07/2023]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                if (value == null)
                {
                    ClearColor = Visibility.Hidden;
                    //[rdixit][29.11.2023][GEOS2-4955]
                    if (SelectedConnectorProperty.IdConnectorProperty == 1)
                    {
                        //[rdixit][11.21.2023][GEOS2-4953]
                        var temp = lstDetailsToSave.FirstOrDefault(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty == 1);
                        if (temp != null)
                        {
                            temp.DefaultValue = "";
                            temp.CanSaveRecord = "Save";
                        }                  
                    }
                }
                else
                    ClearColor = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColor"));
                if (SelectedColor != null)
                    AddDefaultValue(Convert.ToString(value.Id));
            }
        }
        public Visibility ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearColor")); }
        }
        //[nsatpute][GEOS2-4501][11/07/2023]
        public bool IsPropertyExpand
        {
            get { return isPropertyExpand; }
            set
            {
                isPropertyExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPropertyExpand"));
            }
        }
        public bool IsCheckedSealing
        {
            get
            {
                return isCheckedSealing;
            }
            set
            {
                isCheckedSealing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedSealing"));
                if (SelectedConnectorProperty != null)
                {
                    if (SelectedConnectorProperty.CategoryName == "Appearance")
                    {
                        AddDefaultValue(Convert.ToString(value));
                    }
                }

            }
        }

        //[nsatpute][GEOS2-4501][20/07/2023]
        public bool IsPropertyEnabled
        {
            get
            {
                return isPropertyEnabled;
            }
            set
            {
                isPropertyEnabled = value;
                AddIsEnabled(value);
                OnPropertyChanged(new PropertyChangedEventArgs("IsPropertyEnabled"));
            }
        }

        //[nsatpute][GEOS2-4501][20/07/2023]

        public string DefaultValue
        {
            get { return defaultValue; }
            set
            {
                defaultValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DefaultValue")); AddDefaultValue(value);
            }
        }
        public string MinValue
        {
            get { return minValue; }
            set
            {
                minValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinValue")); AddMinValue(value);
            }
        }
        public string MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxValue")); AddMaxValue(value);
            }
        }

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
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public Family CurrentFamily
        {
            get { return currentFamily; }
            set
            {
                currentFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentFamily"));
            }
        }
        public string Checkselectedvalue
        {
            get { return checkselectedvalue; }
            set
            {
                checkselectedvalue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Checkselectedvalue"));
            }
        }

        public bool IsEditButtonEnabled
        {
            get { return isEditButtonEnabled; }
            set
            {
                isEditButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditButtonEnabled"));
            }
        }
        public bool IsAddButtonEnabled
        {
            get { return isAddButtonEnabled; }
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonEnabled"));
            }
        }
        public string ConnectorPropertyFilterString
        {
            get { return connectorPropertyFilterString; }
            set
            {
                connectorPropertyFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorPropertyFilterString"));
            }
        }
        #region rajashri GEOS2-5227
        public ObservableCollection<ValueKey> ValueKey
        {
            get { return valueKey; }
            set
            {
                valueKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueKey"));
            }
        }
        public bool IsCheckedCustom
        {
            get
            {
                return isCheckedCustom;
            }
            set
            {
                isCheckedCustom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCustom"));
                if (SelectedConnectorProperty != null)
                {
                    if (SelectedConnectorProperty.CategoryName == "CUSTOM")
                    {
                        AddDefaultValue(Convert.ToString(value));
                    }
                }

            }
        }
        public Visibility VisibleNumberDefaultValue
        {
            get { return visibleNumberDefaultValue; }
            set { visibleNumberDefaultValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleNumberDefaultValue")); }
        }
        public Visibility ClearCustomList
        {
            get { return clearCustomList; }
            set { clearCustomList = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearCustomList")); }
        }
        public LookUpValues SelectedCustomList
        {
            get { return selectedCustomList; }
            set
            {
                selectedCustomList = value;
                if (value == null)
                {
                    ClearCustomList = Visibility.Hidden;
                }

                else
                    ClearCustomList = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomList"));
                if (SelectedCustomList != null)
                {
                    AddDefaultValue(Convert.ToString(value.IdLookupValue));
                  
                }
                else
                {
                    var temp = lstDetailsToSave.FirstOrDefault(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty);
                    if (temp != null)
                    {
                        temp.DefaultValue = "";
                        temp.CanSaveRecord = "Save";
                    }
                }
            }



        }

        public Visibility VisibleCustomStringValue
        {
            get { return visibleCustomStringValue; }
            set { visibleCustomStringValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleCustomStringValue")); }
        }
      
        public Visibility VisibleCustomListValue
        {
            get { return visibleCustomListValue; }
            set { visibleCustomListValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleCustomListValue")); }
        }
        public Visibility VisibleCustomBooleanValue
        {
            get { return visibleCustomBooleanValue; }
            set { visibleCustomBooleanValue = value; OnPropertyChanged(new PropertyChangedEventArgs("VisibleCustomBooleanValue")); }
        }
        public ObservableCollection<Data.Common.SCM.LookUpValues> CustomListData
        {
            
           get
            {
                return customListData;
            }
            set
            {
                customListData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomListData"));
            }
        }
        public List<GeosSettings> GeosSettings
        {
            get { return geosSettings; }
            set
            {
                geosSettings = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosSettings"));
            }
        }
        #endregion

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
        public void Dispose()
        {

        }
        #endregion

        #region Public ICommand
        public ICommand PropertiesManagerViewCancelButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-4502][06/07/2023]
        public ICommand ExpandAndCollapsePropertyCommand { get; set; } //[nsatpute][GEOS2-4501][11/07/2023]

        public ICommand EditCustomerCommand { get; set; }//[Sudhir.Jangra][GEOS2-4503][13/04/2023]

        public ICommand PropertyManagerViewAcceptButtonCommand { get; set; }//[Sudhir.Jangra][GEOS2-4501][17/07/2023]
        public ICommand FamilyCheckBoxSelectedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand FamilyUnCheckBoxSelectedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand PropertyCheckBoxSelectedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand PropertyUnCheckBoxSelectedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand DataContextChangedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand PropertyTreeListChangedCommand { get; set; } //[nsatpute][GEOS2-4501][26/07/2023]
        public ICommand ModuleGridParentMenuTreeList_CellValueChanged { get; set; }
        public ICommand FamilyCheckBoxCheckUnCheckCommand { get; set; }
        public ICommand LoadedPropertyTreeListChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public PropertiesManagerViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor PropertiesManagerViewModel ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 20;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 90;
                PropertiesManagerViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddButtonCommand = new RelayCommand(new Action<object>(AddButtonCommandAction));//[Sudhir.Jangra][GEOS2-4502][06/07/2023]
                ExpandAndCollapsePropertyCommand = new DelegateCommand<object>(ExpandAndCollapsePropertyCommandAction); //[nsatpute][GEOS2-4501][11/07/2023]
                EditCustomerCommand = new RelayCommand(new Action<object>(EditCustomerCommandAction));
                PropertyManagerViewAcceptButtonCommand = new RelayCommand(new Action<object>(PropertyManagerViewAcceptButtonCommandAction));
                FamilyCheckBoxSelectedCommand = new RelayCommand(new Action<object>(FamilyCheckBoxSelectedCommandAction)); //[nsatpute][GEOS2-4501][26/07/2023]
                FamilyUnCheckBoxSelectedCommand = new DelegateCommand<object>(FamilyUnCheckBoxSelectedCommandAction); //[nsatpute][GEOS2-4501][26/07/2023]
                PropertyCheckBoxSelectedCommand = new RelayCommand(new Action<object>(PropertyCheckBoxSelectedCommandAction)); //[nsatpute][GEOS2-4501][26/07/2023]
                PropertyUnCheckBoxSelectedCommand = new DelegateCommand<object>(PropertyUnCheckBoxSelectedCommandAction); //[nsatpute][GEOS2-4501][26/07/2023]       
                PropertyTreeListChangedCommand = new DelegateCommand<object>(PropertyTreeListChangedCommandAction); //[nsatpute][GEOS2-4501][26/07/2023]    
                FamilyCheckBoxCheckUnCheckCommand = new DelegateCommand<object>(FamilyCheckBoxCheckUnCheckCommandAction);
                LoadedPropertyTreeListChangedCommand = new DelegateCommand<object>(LoadedPropertyTreeListChangedCommandAction);
                //GetPropertyManager service updated with GetPropertyManager_V2460 by [rdixit][29.11.2023][GEOS2-4955]
                if (SCMCommon.Instance.lstDetailsToSave == null)//chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    LoadData.ReturnListDetails(SCMService);
                    lstDetailsToSave = SCMCommon.Instance.lstDetailsToSave;
                }
                else
                {
                    lstDetailsToSave = SCMCommon.Instance.lstDetailsToSave;
                }
                selectedConnectorProperty = lstDetailsToSave.FirstOrDefault();
                Init();
                //[pramod.misal][GEOS2-5483][27.05.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditPropertiesManager)
                {
                    IsSCMEditPropertiesManager = false;
                    IsSCMEditPropertiesManagerBtn = true;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    IsSCMEditPropertiesManager = true;
                    IsSCMEditPropertiesManagerBtn = false;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission && GeosApplication.Instance.IsSCMEditPropertiesManager)
                {
                    IsSCMEditPropertiesManager = false;
                    IsSCMEditPropertiesManagerBtn = true;
                }

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor PropertiesManagerViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PropertiesManagerViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        //[nsatpute][GEOS2-4499][15/07/2023]
        public void Init()
        {
            language = GeosApplication.Instance.CurrentCulture;
            GetGeosSettings();
            FillFamily();
            FillConnectorProperties();
            FillColor();
            FillGender();
           // FillCustomList();//rajashri
            HideActionControls();
          
            IsPropertyExpand = true;
            SelectedFamiles = new ObservableCollection<Family>();
        }
        private void FillFamily()
        {
            try
            {
                if (SCMCommon.Instance.ListFamily == null)//chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    LoadData.ReturnListFamily(SCMService);
                    ListFamily = SCMCommon.Instance.ListFamily;
                    
                    //SCMCommon.Instance.ListFamily = new ObservableCollection<Family>(SCMService.GetAllFamilies(language));
                    //ListFamily = SCMCommon.Instance.ListFamily;
                    //foreach (ConnectorProperties item1 in lstDetailsToSave)
                    //{
                    //    ListFamily.Where(x => x.Id == item1.IdFamily).ToList().ForEach(b => b.IsChecked = true);
                    //}
                }
                else
                {
                    ListFamily = SCMCommon.Instance.ListFamily;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillFamily()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillConnectorProperties()
        {
            try
            {
                if (SCMCommon.Instance.ListConnectorProperties == null)//chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    //ListConnectorProperties = new ObservableCollection<ConnectorProperties>(SCMService.GetConnectorProperties(language) 
                    SCMCommon.Instance.ListConnectorProperties = new ObservableCollection<ConnectorProperties>(SCMService.GetConnectorProperties_V2480(language));//rajashri GEOS2-5227
                    ListConnectorProperties = SCMCommon.Instance.ListConnectorProperties;
                    ListConnectorProperties.Where(x => x.IdConnectorCategory == 0).ToList().ForEach(b => b.IsParentCheckBoxVisible = Visibility.Hidden);
                }
                else
                {
                    ListConnectorProperties = SCMCommon.Instance.ListConnectorProperties;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillFamily()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void AddButtonCommandAction(object obj)//[Sudhir.Jangra][GEOS2-4502][06/07/2023]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddCustomPropertyView addCustomPropertyView = new AddCustomPropertyView();
                AddCustomPropertyViewModel addCustomPropertyViewModel = new AddCustomPropertyViewModel();
                EventHandler handle = delegate { addCustomPropertyView.Close(); };
                addCustomPropertyViewModel.RequestClose += handle;
                addCustomPropertyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCustomPropertyHeader").ToString();
                addCustomPropertyView.DataContext = addCustomPropertyViewModel;
                if (SelectedConnectorProperty == null) // [GEOS2-4962][Rupali Sarode][15-12-2023]
                {
                    // [GEOS2-4962][Rupali Sarode][15-12-2023]
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SCMPropertiesManagerPropertyNotSelected").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                }
                else // [GEOS2-4962][Rupali Sarode][15-12-2023]
                { 
                    addCustomPropertyViewModel.Init(SelectedConnectorProperty, ListConnectorProperties);
                    //addCustomPropertyViewModel.Init();
                    addCustomPropertyViewModel.IsNew = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    addCustomPropertyView.ShowDialogWindow();

                    if (addCustomPropertyViewModel.IsSave)
                    {
                        FillConnectorProperties();
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomPropertyView()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddButtonCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditCustomerCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction().", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                var Id = SelectedConnectorProperty.IdConnectorProperty;
                AddCustomPropertyView editCustomPropertyView = new AddCustomPropertyView();
                AddCustomPropertyViewModel editCustomPropertyViewModel = new AddCustomPropertyViewModel();
                EventHandler handle = delegate { editCustomPropertyView.Close(); };
                editCustomPropertyViewModel.RequestClose += handle;
                editCustomPropertyViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCustomPropertyHeader").ToString();
                editCustomPropertyView.DataContext = editCustomPropertyViewModel;
                editCustomPropertyViewModel.IsNew = false;
                editCustomPropertyViewModel.EditInit(SelectedConnectorProperty, ListConnectorProperties);
                //editCustomPropertyViewModel.EditInit(Id);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                editCustomPropertyView.ShowDialogWindow();

                if (editCustomPropertyViewModel.IsSave)
                {
                    int indexToUpdate = ListConnectorProperties.FindIndex(item => item.IdConnectorProperty == editCustomPropertyViewModel.UpdateCustomPropertyDetails.IdCustomConnectorProperty);
                    if (indexToUpdate >= 0)
                    {
                        ConnectorProperties existingItem = ListConnectorProperties[indexToUpdate];
                        existingItem.PropertyName = editCustomPropertyViewModel.UpdateCustomPropertyDetails.Name;
                    }
                }




                //var Id = SelectedConnectorProperty.IdConnectorProperty;
                //EditCustomPropertyView editCustomPropertyView = new EditCustomPropertyView();
                //   EditCustomPropertyViewModel editCustomPropertyViewModel = new EditCustomPropertyViewModel();
                //EventHandler handle = delegate { editCustomPropertyView.Close(); };
                //editCustomPropertyViewModel.RequestClose += handle;
                //editCustomPropertyView.DataContext = editCustomPropertyViewModel;
                //editCustomPropertyViewModel.Init(Id);
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                //editCustomPropertyView.ShowDialogWindow();


                //if (editCustomPropertyViewModel.IsSave)
                //{
                //    int indexToUpdate = ListConnectorProperties.FindIndex(item => item.IdConnectorProperty == editCustomPropertyViewModel.UpdateCustomPropertyDetails.IdCustomConnectorProperty);
                //    if (indexToUpdate >= 0)
                //    {
                //        ConnectorProperties existingItem = ListConnectorProperties[indexToUpdate];
                //        existingItem.PropertyName = editCustomPropertyViewModel.UpdateCustomPropertyDetails.Name;
                //    }
                //}
                GeosApplication.Instance.Logger.Log("Method EditCustomerCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditCustomerCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][11/07/2023]
        private void ExpandAndCollapsePropertyCommandAction(object obj)
        {
            DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
            PropertyTreeListView = t;
            if (IsPropertyExpand)
            {
                t.ExpandAllNodes();
                IsPropertyExpand = false;
            }
            else
            {
                t.CollapseAllNodes();
                IsPropertyExpand = true;
            }
        }
        //[nsatpute][GEOS2-4499][13/07/2023]
        private void ReloadActionGrid(ConnectorProperties SelectedConnectorProperty)
        {
            if (SelectedConnectorProperty != null)
            {
                switch (selectedConnectorProperty.PropertyName)
                {
                    case "Internal":
                    case "External":
                    case "Height":
                    case "Length":
                    case "Width":
                    case "Ways":
                        VisibleIsEnabled = Visibility.Visible;
                        VisibleTextDefaultValue = Visibility.Visible;
                        VisibleLabelDefaultValue = Visibility.Visible;
                        VisibleColorDefaultValue = Visibility.Hidden;
                        VisibleGenderDefaultValue = Visibility.Hidden;
                        VisibleSealingDefaultValue = Visibility.Hidden;
                        VisibleCustomboolDefaultValue = Visibility.Hidden;
                        VisibleMinValue = Visibility.Visible;
                        VisibleMaxValue = Visibility.Visible;
                        VisibleCustomListValue = Visibility.Hidden;
                        VisibleNumberDefaultValue = Visibility.Hidden;
                        VisibleCustomTextDefaultValue = Visibility.Hidden;
                        break;
                    case "Sealing":
                        VisibleIsEnabled = Visibility.Visible;
                        VisibleLabelDefaultValue = Visibility.Visible;
                        VisibleGenderDefaultValue = Visibility.Hidden;
                        VisibleTextDefaultValue = Visibility.Hidden;
                        VisibleColorDefaultValue = Visibility.Hidden;
                        VisibleSealingDefaultValue = Visibility.Visible;
                        VisibleMinValue = Visibility.Hidden;
                        VisibleCustomboolDefaultValue = Visibility.Hidden;
                        VisibleMaxValue = Visibility.Hidden;
                        VisibleCustomListValue = Visibility.Hidden;
                        VisibleNumberDefaultValue = Visibility.Hidden;
                        VisibleCustomTextDefaultValue = Visibility.Hidden;
                        break;
                    case "Gender":
                        VisibleIsEnabled = Visibility.Visible;
                        VisibleLabelDefaultValue = Visibility.Visible;
                        VisibleGenderDefaultValue = Visibility.Visible;
                        VisibleTextDefaultValue = Visibility.Hidden;
                        VisibleColorDefaultValue = Visibility.Hidden;
                        VisibleSealingDefaultValue = Visibility.Hidden;
                        VisibleMinValue = Visibility.Hidden;
                        VisibleCustomboolDefaultValue = Visibility.Hidden;
                        VisibleMaxValue = Visibility.Hidden;
                        VisibleCustomListValue = Visibility.Hidden;
                        VisibleNumberDefaultValue = Visibility.Hidden;
                        VisibleCustomTextDefaultValue = Visibility.Hidden;
                        break;
                    case "Shape":
                        VisibleIsEnabled = Visibility.Visible;
                        VisibleLabelDefaultValue = Visibility.Visible;
                        VisibleGenderDefaultValue = Visibility.Hidden;
                        VisibleTextDefaultValue = Visibility.Visible;
                        VisibleColorDefaultValue = Visibility.Hidden;
                        VisibleSealingDefaultValue = Visibility.Hidden;
                        VisibleMinValue = Visibility.Hidden;
                        VisibleMaxValue = Visibility.Hidden;
                        VisibleCustomboolDefaultValue = Visibility.Hidden;
                        VisibleCustomListValue = Visibility.Hidden;
                        VisibleNumberDefaultValue = Visibility.Hidden;
                        VisibleCustomTextDefaultValue = Visibility.Hidden;
                        break;
                    case "Color":
                        VisibleIsEnabled = Visibility.Visible;
                        VisibleLabelDefaultValue = Visibility.Visible;
                        VisibleGenderDefaultValue = Visibility.Hidden;
                        VisibleTextDefaultValue = Visibility.Hidden;
                        VisibleColorDefaultValue = Visibility.Visible;
                        VisibleSealingDefaultValue = Visibility.Hidden;
                        VisibleMinValue = Visibility.Hidden;
                        VisibleCustomboolDefaultValue = Visibility.Hidden;
                        VisibleMaxValue = Visibility.Hidden;
                        VisibleCustomListValue = Visibility.Hidden;
                        VisibleNumberDefaultValue = Visibility.Hidden;
                        VisibleCustomTextDefaultValue = Visibility.Hidden;
                        break;
                    default:
                        #region Rajashri GEOS2-5227
                        if (SelectedConnectorProperty.IdConnectorType == 1824)//string
                        {
                            VisibleIsEnabled = Visibility.Visible;
                            VisibleTextDefaultValue = Visibility.Hidden;
                            VisibleLabelDefaultValue = Visibility.Visible;
                            VisibleCustomListValue = Visibility.Hidden;
                            VisibleSealingDefaultValue = Visibility.Hidden;
                            VisibleMinValue = Visibility.Hidden;
                            VisibleCustomboolDefaultValue = Visibility.Hidden;
                            VisibleMaxValue = Visibility.Hidden;
                            VisibleNumberDefaultValue = Visibility.Hidden;
                            VisibleCustomTextDefaultValue = Visibility.Visible;
                        }
                        else if (SelectedConnectorProperty.IdConnectorType == 1825)//Number
                        {
                            VisibleIsEnabled = Visibility.Visible;
                            VisibleTextDefaultValue = Visibility.Hidden;
                            VisibleLabelDefaultValue = Visibility.Visible;
                            VisibleCustomListValue = Visibility.Hidden;
                            VisibleSealingDefaultValue = Visibility.Hidden;
                            VisibleCustomboolDefaultValue = Visibility.Hidden;
                            VisibleMinValue = Visibility.Visible;
                            VisibleMaxValue = Visibility.Visible;
                            VisibleNumberDefaultValue = Visibility.Visible;
                            VisibleCustomTextDefaultValue = Visibility.Hidden;
                        }
                        else if (SelectedConnectorProperty.IdConnectorType == 1826)//List
                        {
                            VisibleIsEnabled = Visibility.Visible;
                            VisibleCustomListValue = Visibility.Visible;
                            VisibleLabelDefaultValue = Visibility.Visible;
                            VisibleSealingDefaultValue = Visibility.Hidden;
                            VisibleMinValue = Visibility.Hidden;
                            VisibleCustomboolDefaultValue = Visibility.Hidden;
                            VisibleMaxValue = Visibility.Hidden;
                            VisibleTextDefaultValue = Visibility.Hidden;
                            VisibleCustomTextDefaultValue = Visibility.Hidden;
                            VisibleNumberDefaultValue = Visibility.Hidden;
                        }
                        else if (SelectedConnectorProperty.IdConnectorType == 1827)//Boolean
                        {
                            VisibleIsEnabled = Visibility.Visible;
                            VisibleSealingDefaultValue = Visibility.Hidden;
                            VisibleLabelDefaultValue = Visibility.Visible;
                            VisibleCustomListValue = Visibility.Hidden;
                            VisibleMinValue = Visibility.Hidden;
                            VisibleMaxValue = Visibility.Hidden;
                            VisibleCustomboolDefaultValue = Visibility.Visible;
                            VisibleTextDefaultValue = Visibility.Hidden;
                            VisibleNumberDefaultValue = Visibility.Hidden;
                            VisibleCustomTextDefaultValue = Visibility.Hidden;

                        }
                        else if (SelectedConnectorProperty.IdConnectorType == 1843)//Text
                        {
                            VisibleIsEnabled = Visibility.Visible;
                            VisibleTextDefaultValue = Visibility.Hidden;
                            VisibleCustomTextDefaultValue = Visibility.Visible;
                            VisibleLabelDefaultValue = Visibility.Visible;
                            VisibleCustomListValue = Visibility.Hidden;
                            VisibleSealingDefaultValue = Visibility.Hidden;
                            VisibleMinValue = Visibility.Hidden;
                            VisibleMaxValue = Visibility.Hidden;
                            VisibleNumberDefaultValue = Visibility.Hidden;
                        }
                        else
                        {
                            HideActionControls();
                        }
                        #endregion
                        break;
                }
            }
        }
        //[nsatpute][GEOS2-4499][15/07/2023]
        private void HideActionControls()
        {
            VisibleCustomTextDefaultValue = Visibility.Hidden;
            VisibleLabelDefaultValue = Visibility.Hidden;
            VisibleIsEnabled = Visibility.Hidden;
            VisibleGenderDefaultValue = Visibility.Hidden;
            VisibleTextDefaultValue = Visibility.Hidden;
            VisibleColorDefaultValue = Visibility.Hidden;
            VisibleSealingDefaultValue = Visibility.Hidden;
            VisibleMinValue = Visibility.Hidden;
            VisibleMaxValue = Visibility.Hidden;
            VisibleCustomListValue = Visibility.Hidden;
            VisibleNumberDefaultValue = Visibility.Hidden;
            VisibleCustomboolDefaultValue = Visibility.Hidden;
        }
        //[nsatpute][GEOS2-4499][15/07/2023]
        private void FillColor()
        {
            try
            {
                if (SCMCommon.Instance.ListColor == null)//chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    SCMCommon.Instance.ListColor = new ObservableCollection<Color>(SCMService.GetAllColors(language));
                    ListColor = SCMCommon.Instance.ListColor;
                    Color colorToRemove = ListColor.FirstOrDefault(x => x.Name == "None");
                    if (colorToRemove != null)//[Sudhir.Jangra][GEOS2-4963]
                    {
                        ListColor.Remove(colorToRemove);
                    }
                }
                else
                {
                    ListColor = SCMCommon.Instance.ListColor;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillColor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillColor() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4499][15/07/2023]
        private void FillGender()
        {
            try
            {
                if (SCMCommon.Instance.ListGender == null)//chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    SCMCommon.Instance.ListGender = new ObservableCollection<Gender>(SCMService.GetGender(language));
                    ListGender = SCMCommon.Instance.ListGender;
                }
                else
                {
                    ListGender = SCMCommon.Instance.ListGender;
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillGender()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillGender() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetGeosSettings()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetGeosSettings()...", category: Category.Info, priority: Priority.Low);
                if (SCMCommon.Instance.GeosSettings == null)    //chitra.girigosavi GEOS2-7867 23/04/2025
                {
                    GeosSettings = new List<Data.Common.GeosSettings>();
                    SCMCommon.Instance.GeosSettings = WorkbenchStartUp.GetSelectedGeosSettings_V2420("SCM_ConnectorList_LookupKeys");
                    GeosSettings = SCMCommon.Instance.GeosSettings;
                }
                else
                {
                    GeosSettings = SCMCommon.Instance.GeosSettings;
                }
                GeosApplication.Instance.Logger.Log("Method GetGeosSettings()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetGeosSettings() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetGeosSettings() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetGeosSettings() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillCustomList()
        {
            try
            {
                if (SelectedConnectorProperty != null)
                {
                    ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKeyOfCustomProperties_V2480(SelectedConnectorProperty.IdConnectorProperty));
                    int lookUpKey = ValueKey.Select(i => i.IdLookupKey).FirstOrDefault();
                    CustomListData = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(lookUpKey));
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method FillGender()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillGender() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Sudhir.Jangra][GEOS2-4501][17/07/2023]
        private void PropertyManagerViewAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PropertyManagerViewAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                bool data = lstDetailsToSave.Any(x => x.CanSaveRecord == "Save" || x.CanSaveRecord == "Delete");
                if (data == false)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PropertyNotAdded").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                else
                {
                    //Updated service from AddPropertyManager to AddPropertyManager_V2460 by [rdixit][29.11.2023][GEOS2-4955]
                    if (lstDetailsToSave != null)
                    {
                        //[rdixit][19.03.2024][GEOS2-5447]
                        var TestAndNumPropWithFalseValueList = lstDetailsToSave
                           .Where(x => (x.IdConnectorProperty == 1 || x.IdConnectorProperty == 3 || x.IdConnectorType == 1825 || x.IdConnectorType == 1843) && (x.DefaultValue?.ToLower() == "false" || x.DefaultValue?.ToLower() == "true"))
                           .ToList();

                        TestAndNumPropWithFalseValueList?.ForEach(y => y.DefaultValue = string.Empty);
                    }
                    SCMService.AddPropertyManager_V2460(lstDetailsToSave.Where(x => x.CanSaveRecord == "Save").ToList());                    
                    SCMService.DeletePropertyManager(lstDetailsToSave.Where(x => x.CanSaveRecord == "Delete").ToList());
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PropertyAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    //RequestClose(null, null); GEOS2-5359 rajashri -1b  After click on accept, the control of properties manager is automatically closed and this is not necessary. After click on Accept, the Properties Manager can remain opened and the user have to close it manually if he want to exit.
                }
                GeosApplication.Instance.Logger.Log("Method PropertyManagerViewAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PropertyManagerViewAcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                RequestClose(null, null);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PropertyManagerViewAcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PropertyManagerViewAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][20/07/2023]
        private void AddDefaultValue(string defaultVal)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDefaultValue()...", category: Category.Info, priority: Priority.Low);
                #region [GEOS2-4501][gulab lakade][27 07 2023]
                if (SelectedFamily != null && SelectedConnectorProperty != null && SelectedFamiles.Count > 0)
                {
                    foreach (var temprecord in SelectedFamiles)
                    {
                        if (lstDetailsToSave.Any(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                                    && x.IdFamily == temprecord.Id))
                        {
                            var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                                                    && x.IdFamily == temprecord.Id).FirstOrDefault();
                            templstDetails.DefaultValue = defaultVal;
                            if (SelectedConnectorProperty.IdConnectorCategory == 0)
                            {
                                templstDetails.CanSaveRecord = "Not Save";
                            }
                            else
                            {
                                templstDetails.CanSaveRecord = "Save";
                            }
                        }
                    }
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method AddDefaultValue()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCategory() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][20/07/2023]
        private void AddIsEnabled(bool enable)
        {
            try
            {

                if (SelectedFamily != null && SelectedConnectorProperty != null && SelectedFamiles.Count > 0)
                {
                    foreach (var temprecord in SelectedFamiles)
                    {
                        SelectedFamiles.Where(x => x.Id == temprecord.Id).ToList().ForEach(a => a.IsChecked = true);
                        if (lstDetailsToSave.Any(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                                            && x.IdFamily == temprecord.Id))
                        {
                            var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                              && x.IdFamily == temprecord.Id).FirstOrDefault();
                            templstDetails.IsEnabled = enable;
                            if (SelectedConnectorProperty.IdConnectorCategory == 0)
                            {
                                templstDetails.CanSaveRecord = "Not Save";
                            }
                            if (SelectedConnectorProperty.IsChecked == true)
                            {
                                templstDetails.CanSaveRecord = "Save";
                            }
                            ListFamily.Where(x => x.Id == temprecord.Id).ToList().ForEach(a => a.IsChecked = true);
                        }
                        else
                        {
                            #region [GEOS2-4501][gulab lakade][27 07 2023]
                            if (lstDetailsToSave.Any(x => x.IdConnectorProperty == 0
                                        && x.IdFamily == temprecord.Id))
                            {
                                var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == 0
                                      && x.IdFamily == temprecord.Id).FirstOrDefault();
                                templstDetails.IdConnectorProperty = SelectedConnectorProperty.IdConnectorProperty;
                                templstDetails.IsEnabled = enable;
                                if (SelectedConnectorProperty.IdConnectorCategory == 0)
                                {
                                    templstDetails.CanSaveRecord = "Not Save";
                                }
                                if (SelectedConnectorProperty.IsChecked == true)
                                {
                                    templstDetails.CanSaveRecord = "Save";
                                }
                                ListFamily.Where(x => x.Id == temprecord.Id).ToList().ForEach(a => a.IsChecked = true);

                            }
                            else
                            {
                                if (Checkselectedvalue != "Family" && SelectedConnectorProperty.IdConnectorProperty != 0)
                                {
                                    ConnectorProperties property = new ConnectorProperties();
                                    property.IsEnabled = enable;
                                    property.IdFamily = temprecord.Id;
                                    if (Checkselectedvalue == "Family")
                                    {
                                        property.IdConnectorProperty = 0;
                                    }
                                    else
                                    {
                                        property.IdConnectorProperty = SelectedConnectorProperty.IdConnectorProperty;
                                    }
                                    if (SelectedConnectorProperty.IdConnectorCategory == 0)
                                    {
                                        property.CanSaveRecord = "Not Save";
                                    }
                                    if (SelectedConnectorProperty.IsChecked == true)
                                    {
                                        property.CanSaveRecord = "Save";
                                        lstDetailsToSave.Add(property);
                                    }

                                    ListFamily.Where(x => x.Id == temprecord.Id).ToList().ForEach(a => a.IsChecked = true);
                                }
                                if (Checkselectedvalue == "Family")
                                {
                                    SelectedConnectorProperty.IdConnectorProperty = 0;
                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                    if (SelectedFamily != null && SelectedFamiles.Count > 0)
                {
                    foreach (var temprecord in SelectedFamiles)
                    {
                        #region [GEOS2-4501][gulab lakade][27 07 2023]
                        if (lstDetailsToSave.Any(x => x.IdConnectorProperty == 0
                                    && x.IdFamily == temprecord.Id))
                        {
                            var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == 0
                                  && x.IdFamily == temprecord.Id).FirstOrDefault();
                            templstDetails.IdConnectorProperty = SelectedConnectorProperty.IdConnectorProperty;
                            templstDetails.IsEnabled = enable;
                            if (SelectedConnectorProperty.IdConnectorCategory == 0)
                            {
                                templstDetails.CanSaveRecord = "Not Save";
                            }
                            if (SelectedConnectorProperty.IsChecked == true)
                            {
                                templstDetails.CanSaveRecord = "Save";
                           }                      
                        }
                        #endregion
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddIsEnabled()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddIsEnabled() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][20/07/2023] //[rdixit][29.11.2023][GEOS2-4955]
        private void AddMinValue(string mnValue)
        {
            try
            {

                foreach (var temprecord in SelectedFamiles)
                {
                    if (lstDetailsToSave.Any(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                               && x.IdFamily == temprecord.Id))
                    {
                        var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                       && x.IdFamily == temprecord.Id).FirstOrDefault();
                        templstDetails.MinValueNew = mnValue;
                        if (SelectedConnectorProperty.IdConnectorCategory == 0)
                        {
                            templstDetails.CanSaveRecord = "Not Save";
                        }
                        else
                        {
                            templstDetails.CanSaveRecord = "Save";
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddMinValue()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddMinValue() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][20/07/2023] //[rdixit][29.11.2023][GEOS2-4955]
        private void AddMaxValue(string mxValue)
        {
            try
            {
                #region [GEOS2-4501][gulab lakade][27 07 2023]
                foreach (var temprecord in SelectedFamiles)
                {
                    if (lstDetailsToSave.Any(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                               && x.IdFamily == temprecord.Id))
                    {

                        var templstDetails = lstDetailsToSave.Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                          && x.IdFamily == temprecord.Id).FirstOrDefault();
                        templstDetails.MaxValueNew = mxValue;
                        if (SelectedConnectorProperty.IdConnectorCategory == 0)
                        {
                            templstDetails.CanSaveRecord = "Not Save";
                        }
                        else
                        {
                            templstDetails.CanSaveRecord = "Save";
                        }
                    }
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method AddMaxValue()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddMaxValue() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][20/07/2023]
        private void LoadActions()
        {
            try
            {
                var temp = SelectedFamiles;
                if (Checkselectedvalue == "Family")
                {
                    #region [GEOS2-4501][gulab lakade][27 07 2023]
                    if (SelectedFamily != null)
                    {
                        if (lstDetailsToSave.Any(x => x.IdFamily == SelectedFamily.Id))
                        {
                            ConnectorProperties property = lstDetailsToSave.FirstOrDefault(x => x.IdFamily == SelectedFamily.Id);
                            IsPropertyChecked = true;//[Sudhir.Jangra][GEOS2-4501]
                        }
                        else if (SelectedConnectorProperty != null)
                        {
                            IsPropertyEnabled = true;
                            //[rdixit][29.11.2023][GEOS2-4955]
                            MinValue = string.Empty;
                            MaxValue = string.Empty;
                            DefaultValue = string.Empty;
                            SelectedGender = null;
                            SelectedColor = null;
                            SelectedCustomList = null;//rajashri
                            IsCheckedSealing = false;
                            IsCheckedCustom = false;
                        }
                    }
                }
                else
                if (Checkselectedvalue == "Connector")
                {
                       if (SelectedConnectorProperty != null && SelectedFamily != null && SelectedFamiles.Count > 0)
                    {

                        if (lstDetailsToSave.Any(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                            && x.IdFamily == SelectedFamily.Id))
                        {
                            ConnectorProperties property = lstDetailsToSave.FirstOrDefault(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty
                            && x.IdFamily == SelectedFamily.Id);
                            IsPropertyChecked = true;//[Sudhir.Jangra][GEOS2-4501]  
                            IsPropertyEnabled = property.IsEnabled;
                            //[rdixit][29.11.2023][GEOS2-4955]
                            MinValue = property.MinValueNew;
                            MaxValue = property.MaxValueNew;
                            switch (SelectedConnectorProperty.PropertyName)
                            {
                                case "Color":
                                    if (!string.IsNullOrEmpty(property.DefaultValue))
                                        SelectedColor = ListColor.FirstOrDefault(x => x.Id == Convert.ToUInt16(property.DefaultValue));
                                    break;
                                case "Gender":
                                    if (!string.IsNullOrEmpty(property.DefaultValue))
                                        SelectedGender = ListGender.FirstOrDefault(x => x.Id == Convert.ToUInt16(property.DefaultValue));
                                    break;
                                case "Sealing":
                                    if (!string.IsNullOrEmpty(property.DefaultValue)) { 
                                        IsCheckedSealing = Convert.ToBoolean(property.DefaultValue);}
                                    break;
                                default:
                                    if (SelectedConnectorProperty.IdConnectorType == 1827)//Boolean
                                    {
                                        if (!string.IsNullOrEmpty(property.DefaultValue))
                                        {
                                            IsCheckedCustom = Convert.ToBoolean(property.DefaultValue);
                                        }
                                    }
                                    else if (SelectedConnectorProperty.IdConnectorType == 1826)//List
                                    {
                                        FillCustomList();
                                        if (!string.IsNullOrEmpty(property.DefaultValue))
                                        SelectedCustomList = CustomListData.FirstOrDefault(x => x.IdLookupValue == Convert.ToUInt16(property.DefaultValue));
                                        
                                    }
                                    else
                                    {
                                        DefaultValue = property.DefaultValue;
                                    }
                                    break;
                            }
                           
                        }
                        else
                        {
                            IsPropertyEnabled = true;
                            MinValue = string.Empty;
                            MaxValue = string.Empty;
                            DefaultValue = string.Empty;
                            SelectedGender = null;
                            SelectedColor = null;
                            SelectedCustomList = null;
                            IsCheckedSealing = false;
                            IsCheckedCustom = false;
                       
                        }
                    }
                }
                #endregion
                GeosApplication.Instance.Logger.Log("Method LoadActions()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LoadActions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Sudhir.Jangra][GEOS2-4501]
        private void LoadProperties()
        {
            try
            {
                if (SelectedFamily != null && SelectedFamily.IsChecked == true)
                {

                    List<ConnectorProperties> filtering = new List<ConnectorProperties>(lstDetailsToSave.Where(a => a.IdFamily == SelectedFamily.Id && a.CanSaveRecord != "Delete"));
                    FillConnectorProperties();
                    foreach (var fil in filtering)
                    {
                        ListConnectorProperties.Where(x => x.IdConnectorProperty == fil.IdConnectorProperty).ToList().ForEach(v => v.IsChecked = true);

                    }
                }
                else
                {
                    IsPropertyExpand = true;
                    FillConnectorProperties();
                    SelectedConnectorProperty = new ConnectorProperties();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LoadActions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadCheckedProperty()
        {
            try
            {
                if (SelectedConnectorProperty != null && SelectedConnectorProperty.IsChecked != true)
                {
                    SelectedConnectorProperty.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LoadActions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][26/07/2023]
        private void FamilyCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var temprecord in SelectedFamiles)
                {
                    CheckUncheckSelectedFamiles = true;
                    SelectedFamily.IsChecked = true;

                    ListFamily.Where(x => x.Id == temprecord.Id).ToList().ForEach(b => b.IsChecked = true);
                    lstDetailsToSave.Where(x => x.IdFamily == temprecord.Id).ToList().ForEach(y => y.CanSaveRecord = "Save");
                }
                try
                {

                    DevExpress.Xpf.Grid.TreeListView t = (DevExpress.Xpf.Grid.TreeListView)obj;
                    if (IsPropertyExpand)
                    {
                        t.ExpandAllNodes();
                        IsPropertyExpand = false;
                    }
                    else
                    {
                        t.CollapseAllNodes();
                        IsPropertyExpand = true;
                        t.ExpandAllNodes();
                        IsPropertyExpand = false;
                    }

                }
                catch (Exception ex)
                {

                }

                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FamilyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][26/07/2023]
        private void FamilyUnCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FamilyUnCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                DevExpress.Xpf.Grid.TableView EventArg = (DevExpress.Xpf.Grid.TableView)obj;
                foreach (var temprecord in SelectedFamiles)
                {
                    if (CheckUncheckSelectedFamiles == true)
                    {
                        SelectedFamily.IsChecked = true;
                        CheckUncheckSelectedFamiles = false;
                    }
                    else
                    {
                        ListFamily.Where(x => x.Id == temprecord.Id).ToList().ForEach(b => b.IsChecked = false);
                        lstDetailsToSave.Where(x => x.IdFamily == temprecord.Id).ToList().ForEach(y => y.CanSaveRecord = "Delete");
                    }
                }
                GeosApplication.Instance.Logger.Log("Method FamilyUnCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FamilyUnCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][26/07/2023]
        private bool isScrolling = false;
        private void PropertyCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedConnectorProperty != null)
                {
                    DevExpress.Xpf.Grid.TreeListControl temp = (DevExpress.Xpf.Grid.TreeListControl)obj;
                    bool isChecked = (bool)SelectedConnectorProperty.GetType().GetProperty("IsChecked").GetValue(SelectedConnectorProperty);


                    if (isChecked == true)
                    {
                        SelectedConnectorProperty.IsChecked = true;
                        SelectedConnectorProperty = (ConnectorProperties)temp.SelectedItem;
                        if (SelectedConnectorProperty != null)
                        {
                            ListConnectorProperties
                            .Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty).ToList().ForEach(v => v.IsChecked = true);

                            lstDetailsToSave.Where(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty)
                                .ToList().ForEach(y => y.CanSaveRecord = "Save");

                            #region To add properties value if exist [rdixit][19.03.2024][GEOS2-5447]
                            var test = lstDetailsToSave.FirstOrDefault(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty && x.CanSaveRecord == "Save");
                            if (test != null)
                            {
                                test.IdConnectorCategory = SelectedConnectorProperty.IdConnectorCategory;
                                test.IdConnectorType = SelectedConnectorProperty.IdConnectorType;
                            }
                            #endregion
                        }
                    }
                    else
                    {

                        SelectedConnectorProperty.IsChecked = false;
                        SelectedConnectorProperty = (ConnectorProperties)temp.SelectedItem;
                        if (SelectedConnectorProperty != null)
                        {
                            ListConnectorProperties
                            .Where(x => x.IdConnectorProperty == SelectedConnectorProperty.IdConnectorProperty).ToList().ForEach(v => v.IsChecked = false);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method PropertyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PropertyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][GEOS2-4501][26/07/2023]
        private void PropertyUnCheckBoxSelectedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PropertyUnCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                DevExpress.Xpf.Grid.TreeListControl EventArg = (DevExpress.Xpf.Grid.TreeListControl)obj;
                if (EventArg.SelectedItem != null)
                {
                    if (CheckUncheckSelectedConnector == true)
                    {
                        SelectedConnectorProperty.IsChecked = true;
                        CheckUncheckSelectedConnector = false;
                    }
                    else
                    {
                        bool isChecked = (bool)SelectedConnectorProperty.GetType().GetProperty("IsChecked").GetValue(SelectedConnectorProperty);
                        int idConnectorProperty = ((Emdep.Geos.Data.Common.SCM.ConnectorProperties)EventArg.SelectedItem).IdConnectorProperty;

                        if (isChecked == false)
                        {
                          
                            ListConnectorProperties.Where(x => x.IdConnectorProperty == idConnectorProperty).ToList().
                                ForEach(v => v.IsChecked = false);
                            if (SelectedConnectorProperty.IdConnectorCategory == 0)
                            {
                                lstDetailsToSave.Where(x => x.IdFamily == SelectedFamily.Id && x.IdConnectorProperty ==
                                idConnectorProperty).ToList().ForEach(y => y.CanSaveRecord = "Not Delete");
                            }
                            else
                            {
                                foreach (var tempSelectedFamily in SelectedFamiles)
                                {
                                    if (lstDetailsToSave.Any(y => y.IdFamily == tempSelectedFamily.Id &&
                                 y.IdConnectorProperty == idConnectorProperty))
                                    {
                                        var selectedRecord = lstDetailsToSave.FirstOrDefault(y => y.IdFamily == tempSelectedFamily.Id &&
                                                           y.IdConnectorProperty == idConnectorProperty);

                                        if (selectedRecord != null)
                                        {
                                            // Perform your action with the selectedRecord
                                        }

                                    }
                                }
                                    foreach (var tempSelectedFamily in SelectedFamiles)
                                    {
                                        lstDetailsToSave.Where(x => x.IdFamily == tempSelectedFamily.Id && x.IdConnectorProperty == idConnectorProperty).ToList().ForEach(y => y.CanSaveRecord = "Delete");
                                    }
                            }
                        }
                        else
                        {
                            ListConnectorProperties.Where(x => x.IdConnectorProperty == idConnectorProperty).ToList().ForEach(v => v.IsChecked = true);
                            foreach (var tempSelectedFamily in SelectedFamiles)
                                {
                                    lstDetailsToSave.Where(x => x.IdFamily == tempSelectedFamily.Id && x.IdConnectorProperty == idConnectorProperty).ToList().ForEach(y => y.CanSaveRecord = "Not Delete");
                                }

                          
                        }

                    }
                }
                GeosApplication.Instance.Logger.Log("Method PropertyUnCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PropertyUnCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadEditButton()
        {
            try
            {
                if (SelectedConnectorProperty != null)
                {
                    #region old code
                    //if (SelectedConnectorProperty.CategoryName?.ToUpper() != "CUSTOM")
                    //{
                    //    IsEditButtonEnabled = false;
                    //}
                    //else
                    //{
                    //    IsEditButtonEnabled = true;
                    //}

                    #endregion 
                    //[pramod.misal][02-05-2024][GEOS2-5483]
                    if ((SelectedConnectorProperty.CategoryName?.ToUpper() != "CUSTOM" && !GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 94)) || 
                        (SelectedConnectorProperty.CategoryName?.ToUpper() != "CUSTOM" && !GeosApplication.Instance.IsSCMEditPropertiesManager))
                    {
                        IsEditButtonEnabled = false;
                    }
                    else if ((GeosApplication.Instance.IsSCMEditPropertiesManager && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM") ||
                         (GeosApplication.Instance.IsSCMPermissionAdmin && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM"))
                    {
                        IsEditButtonEnabled = true;
                    }
                    else if (GeosApplication.Instance.IsSCMEditPropertiesManager && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM")
                    {
                        IsEditButtonEnabled = false;
                    }
                   

                }
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in LoadEditButton() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadAddButton()
        {
            try
            {
                if (SelectedConnectorProperty != null)
                {
                    #region old code
                    //if (SelectedConnectorProperty.PropertyName?.ToUpper() != "CUSTOM")
                    //{
                    //    IsAddButtonEnabled = false; //Func off
                    //}
                    //else
                    //{
                    //    IsAddButtonEnabled = true;
                    //}


                    #endregion

                    if ((SelectedConnectorProperty.CategoryName?.ToUpper() != "CUSTOM" && !GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 94)) ||
                        (SelectedConnectorProperty.CategoryName?.ToUpper() != "CUSTOM" && !GeosApplication.Instance.IsSCMEditPropertiesManager))
                    {
                        IsAddButtonEnabled = false;
                    }
                    else if ((GeosApplication.Instance.IsSCMEditPropertiesManager && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM") ||
                        (GeosApplication.Instance.IsSCMPermissionAdmin && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM") )
                    {
                        IsAddButtonEnabled = true;
                    }
                    else if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        IsAddButtonEnabled = false;
                    }
                    



                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in LoadAddButton() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PropertyTreeListChangedCommandAction(object obj)
        {
            try
            {
                CheckUncheckSelectedConnector = false;
                #region old code
                //if (GeosApplication.Instance.IsSCMPermissionAdmin)
                //{
                //    IsAddButtonEnabled = true;
                //}
                //else
                //{
                //    IsAddButtonEnabled = false;
                //}
                #endregion
                //Pramod.misal GEOS2-5698 10-05-2024
                //if (GeosApplication.Instance.IsSCMPermissionAdmin || (GeosApplication.Instance.IsSCMEditPropertiesManager && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM"))
                //{
                //    IsAddButtonEnabled = false;
                //    IsEditButtonEnabled = true;
                //}

            
                if (SelectedConnectorProperty.PropertyName?.ToUpper() == "CUSTOM")
                {
                    IsAddButtonEnabled = true;
                    IsEditButtonEnabled = false;
                }
                if (GeosApplication.Instance.IsSCMViewConfigurationPermission && !GeosApplication.Instance.IsSCMEditPropertiesManager)
                {
                    IsAddButtonEnabled = false;
                    IsEditButtonEnabled = false;

                }
                if (SelectedConnectorProperty.PropertyName?.ToUpper() != "CUSTOM" && SelectedConnectorProperty.CategoryName?.ToUpper() == "CUSTOM")
                {
                    IsAddButtonEnabled = false;
                    IsEditButtonEnabled = true;
                }
                if (GeosApplication.Instance.IsSCMViewConfigurationPermission && !GeosApplication.Instance.IsSCMEditPropertiesManager && SelectedConnectorProperty.PropertyName?.ToUpper() != "CUSTOM" && SelectedConnectorProperty.PropertyName?.ToUpper() != "CUSTOM")
                {
                    IsAddButtonEnabled = false;
                    IsEditButtonEnabled = false;
                }
               


            }
            catch (Exception ex)
            {

            }
        }
        private void FamilyCheckBoxCheckUnCheckCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                TreeListNodeEventArgs selection = (TreeListNodeEventArgs)obj;
                Family row = (Family)selection.Row;
                if (row.IsChecked == true)
                {
                    lstDetailsToSave.Where(x => x.IdFamily == row.Id).ToList().ForEach(y => y.CanSaveRecord = "Save");
                    ListFamily.Where(x => x.Id == row.Id).ToList().ForEach(b => b.IsChecked = true);

                    try
                    {
                        var tempvalue = SelectedFamiles.Where(x => x.Id == row.Id).ToList();
                        if (boolFamily == false || tempvalue.Count == 0)
                        {
                            SelectedFamily = row;
                            FillConnectorProperties();
                            boolFamily = false;
                        }
                        Checkselectedvalue = "Family";
                        var temp1 = SelectedFamiles;

                        if (PropertyTreeListView != null)
                        {
                            if (ListConnectorProperties == null || ListConnectorProperties.Count == 0)
                            {
                                FillConnectorProperties();
                            }

                            PropertyTreeListView.ExpandAllNodes();
                            IsPropertyExpand = false;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (row.IsChecked == false)
                {
                    ListFamily.Where(x => x.Id == row.Id).ToList().ForEach(b => b.IsChecked = false);
                    lstDetailsToSave.Where(x => x.IdFamily == row.Id).ToList().ForEach(y => y.CanSaveRecord = "Delete");
                }
                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FamilyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void LoadedPropertyTreeListChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()...", category: Category.Info, priority: Priority.Low);
                DevExpress.Xpf.Grid.TreeListView tt = (DevExpress.Xpf.Grid.TreeListView)obj;
                propertyTreeListView = new TreeListView();
                propertyTreeListView = tt;
                propertyTreeListView.ExpandAllNodes();
                ListConnectorProperties = new ObservableCollection<ConnectorProperties>();
                GeosApplication.Instance.Logger.Log("Method FamilyCheckBoxSelectedCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FamilyCheckBoxSelectedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SCMShortcuts.Instance.IsActive = false;
                OpenWindowClickOnShortcutKey(obj);
                //SCMShortcuts.Instance.IsActive = false;
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void OpenWindowClickOnShortcutKey(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = "";
                if (obj.KeyboardDevice.Modifiers == ModifierKeys.None)
                {
                    ShortcutKey = obj.Key.ToString();
                }
                else
                {
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ShortcutKey = "ctrl";
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + shift";
                        }
                        else
                        {
                            ShortcutKey = "shift";
                        }
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + alt";
                        }
                        else
                        {
                            ShortcutKey = "alt";
                        }
                    }

                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + windows";
                        }
                        else
                        {
                            ShortcutKey = "windows";
                        }
                    }
                    if (obj.Key == Key.System)
                    {
                        if (obj.SystemKey.ToString().Contains("Left") || obj.SystemKey.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.SystemKey.ToString();
                        }
                    }
                    else
                    {
                        if (obj.Key.ToString().Contains("Left") || obj.Key.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.Key.ToString();
                        }
                    }
                }

                string[] Keys = ShortcutKey.Split('+');

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["NewSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["ModifiedSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Connectors3D"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Search"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMSampleRegistrationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Create"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["SearchManager"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    RequestClose(null, null);
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Properties"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    RequestClose(null, null);
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);

                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Families"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission ||
                               GeosApplication.Instance.IsSCMPermissionReadOnly ||
                               GeosApplication.Instance.IsSCMEditLocationsManager)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Locations"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }
                            }


                        }

                    }
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenWindowClickOnShortcutKey...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private int getComparedShortcutKeyCount(string[] Keys, string[] StoredKeys)
        {
            int count = 0;
            if (Keys.Count() == StoredKeys.Count())
            {
                foreach (string key in Keys)
                {
                    foreach (string storedKey in StoredKeys)
                    {
                        if (key.ToUpper().TrimStart().TrimEnd() == storedKey.ToUpper().TrimStart().TrimEnd())
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        #endregion
    }
    public static class LoadData
    {
        public static List<ConnectorProperties> LstDetailsToSave;
        public static List<Family> ListFamilies;
        public static async void ReturnListDetails(ISCMService SCMService)
        {
            SCMCommon.Instance.lstDetailsToSave = await ReturnPropertyList(SCMService);
        }

        public static async void ReturnListFamily(ISCMService SCMService)
        {
            SCMCommon.Instance.ListFamily = new ObservableCollection<Family>(await ReturnFamilyList(SCMService));
            foreach (ConnectorProperties item1 in SCMCommon.Instance.lstDetailsToSave)
            {
                SCMCommon.Instance.ListFamily.Where(x => x.Id == item1.IdFamily).ToList().ForEach(b => b.IsChecked = true);
            }
        }
        public static async Task<List<ConnectorProperties>> ReturnPropertyList(ISCMService SCMService)
        {
            var PropertiesList = SCMService.GetPropertyManager_V2490();
            return await Task.FromResult(PropertiesList);
        }

        public static async Task<List<Family>> ReturnFamilyList(ISCMService SCMService)
        {
            var ListFamilies = SCMService.GetAllFamilies_V2480(GeosApplication.Instance.CurrentCulture);
            return await Task.FromResult(ListFamilies);
        }
    }
}
