using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraSpreadsheet.Import.Xls;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Interface;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using Company = Emdep.Geos.Data.Common.SCM.Company;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[GEOS2-9552][rdixit][19.09.2025]
    public class ConnectorViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController("localhost:6699");
        #endregion

        #region public Events

        private string _selectedValue;

        public string SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedValue"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // Events

        #region Methods


        public void Dispose()
        {
        }

        #endregion // Methods

        #region Declarations
        //[GEOS2-9552][rdixit][19.09.2025]
        ObservableCollection<ConnectorProperties> allCustomFieldsList;
        ObservableCollection<ConnectorProperties> customFieldsList;
        private ObservableCollection<SimilarCharactersByConfiguration> similarCharactersList;
        List<object> selectedGenderList;
        List<object> selectedColorList;
        List<object> selectedSubfamilyList;
        List<object> selectedFamilyList;
        List<object> selectedCompanyList;
        private List<ConnectorProperties> _standardPropertiedbyFamily;
        private bool isFamilySelected;
        bool unSealedValue = false;
        double dialogHeight;
        double dialogWidth;
        Visibility isThumbnailsViewVisible;
        ObservableCollection<DocumentPanel> listDocumentPanelNew;
        string type;
        private List<CustomProperty> customList;
        private List<CustomProperty> customListByIdFamily;
        ObservableCollection<ConnectorSearch> connectorSearchList;
        ConnectorSearch selectedConnectorSearch;
        int selectedSearchIndex;
        string informationError;
        List<ConnectorProperties> prevProperties;
        string myFilterString;
        bool sealingDefaultValue;
        bool sealingflag;
        Components selectedComponent;
        private ObservableCollection<ConnectorSubFamily> newlistSubfamily;
        private ObservableCollection<DocumentPanel> listDocumentPanel;
        private ObservableCollection<ComponentType> componentTypeList;
        public ObservableCollection<Components> componentlist;
        private ObservableCollection<Company> listCompany;
        private ObservableCollection<Family> listFamily;
        private ObservableCollection<ConnectorSubFamily> listSubfamily;
        private ObservableCollection<Gender> listGender;
        private ObservableCollection<Data.Common.SCM.Color> listColor;
        private ObservableCollection<LookupValue> listShape;
        private LookupValue selectedShape;
        private bool isInternalEnable;
        private Visibility clearCompany;
        private Visibility clearFamily;
        private Visibility clearSubfamily;
        private Visibility clearColor;
        private Visibility clearGender;
        private Visibility clearShape;
        private string reference;
        private ConnectorProperties ways;
        private ConnectorProperties diameterInternal;
        private ConnectorProperties diameterExternal;
        private ConnectorProperties height;
        private ConnectorProperties length;
        private ConnectorProperties width;
        private string language;
        ConnectorProperties sealingProp = new ConnectorProperties();
        ConnectorProperties colorProp = new ConnectorProperties();
        ConnectorProperties genderProp = new ConnectorProperties();
        Visibility familyImg = Visibility.Hidden;
        Visibility subfamilyImg = Visibility.Hidden;
        ObservableCollection<ValueKey> valueKey;
        private ObservableCollection<ConnectorProperties> savedDetailsForConnector;
        bool isReadOnly;
        bool isBasicConnectorEditReadOnly;
        bool isBasicConnectorEditEnabled;
        bool isEnabled;
        public bool isEnabledCancelButton = false;
        List<Family> allFamilyList;
        private bool allItemsLoaded;
        #endregion

        #region Properties   
        public ObservableCollection<ConnectorProperties> AllCustomFieldsList
        {
            get { return allCustomFieldsList; }
            set
            {
                allCustomFieldsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllCustomFieldsList"));
            }
        }
        public ObservableCollection<ConnectorProperties> CustomFieldsList
        {
            get { return customFieldsList; }
            set
            {
                customFieldsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFieldsList"));
            }
        }
        public ObservableCollection<SimilarCharactersByConfiguration> SimilarCharactersList
        {
            get { return similarCharactersList; }
            set
            {
                similarCharactersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SimilarCharactersList"));
            }
        }
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnly"));
            }
        }
        public bool IsBasicConnectorEditEnabled
        {
            get { return isBasicConnectorEditEnabled; }
            set
            {
                isBasicConnectorEditEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBasicConnectorEditEnabled"));
            }
        }
        public bool IsBasicConnectorEditReadOnly
        {
            get { return isBasicConnectorEditReadOnly; }
            set
            {
                isBasicConnectorEditReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBasicConnectorEditReadOnly"));
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabled"));
            }
        }
        public ConnectorSearch SelectedConnectorSearch
        {
            get { return selectedConnectorSearch; }
            set
            {
                selectedConnectorSearch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorSearch"));
            }
        }
        public ObservableCollection<ConnectorSearch> ConnectorSearchList
        {
            get { return connectorSearchList; }
            set
            {
                connectorSearchList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorSearchList"));
            }
        }
        public int SelectedSearchIndex
        {
            get { return selectedSearchIndex; }
            set
            {
                selectedSearchIndex = value;
                if (SelectedSearchIndex != 0 && SelectedSearchIndex != -1)
                {
                    SelectedConnectorSearch = ConnectorSearchList[SelectedSearchIndex];
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedSearchIndex"));
                }
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
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public ObservableCollection<ComponentType> ComponentTypeList
        {
            get { return componentTypeList; }
            set
            {
                componentTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentTypeList"));
            }
        }
        public Components SelectedComponent
        {
            get { return selectedComponent; }
            set
            {
                selectedComponent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComponent"));
            }
        }
        public ObservableCollection<Components> Componentlist
        {
            get { return componentlist; }
            set
            {
                componentlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Componentlist"));
            }
        }
        public static ObservableCollection<Company> ListCompanyAll
        {
            get; set;
        }
        public ObservableCollection<Company> ListCompany
        {
            get { return listCompany; }
            set
            {
                listCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCompany"));
            }
        }
        public ObservableCollection<DocumentPanel> ListDocumentPanel
        {
            get { return listDocumentPanel; }
            set
            {
                listDocumentPanel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDocumentPanel"));
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
        public List<Family> AllFamilyList
        {
            get
            {
                return allFamilyList;
            }
            set
            {
                allFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllFamilyList"));
            }
        }
        public ObservableCollection<ConnectorSubFamily> ListSubfamily
        {
            get
            {
                return listSubfamily;
            }
            set
            {
                listSubfamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListSubfamily"));
            }
        }
        public ObservableCollection<ConnectorSubFamily> NewListSubfamily
        {
            get
            {
                return newlistSubfamily;
            }
            set
            {
                newlistSubfamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewListSubfamily"));
            }
        }
        public ObservableCollection<Data.Common.SCM.Color> ListColor
        {
            get { return listColor; }
            set { listColor = value; OnPropertyChanged(new PropertyChangedEventArgs("ListColor")); }
        }
        public ObservableCollection<LookupValue> ListShape
        {
            get { return listShape; }
            set
            {
                listShape = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListShape"));
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
        public List<ConnectorProperties> StandardPropertiedbyFamily
        {

            get
            {
                return _standardPropertiedbyFamily;
            }
            set
            {
                _standardPropertiedbyFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StandardPropertiedbyFamily"));
            }
        }
        //[rdixit][10.04.2024][GEOS2-5613]
        public Visibility FamilyImg
        {
            get
            {
                return familyImg;
            }
            set
            {
                familyImg = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FamilyImg"));
            }
        }
        public Visibility SubfamilyImg
        {
            get
            {
                return subfamilyImg;
            }
            set
            {
                subfamilyImg = value; OnPropertyChanged(new PropertyChangedEventArgs("SubfamilyImg"));
            }
        }
        #region GEOS5296 rdixit 29.02.2024
        public List<object> SelectedFamilyList
        {
            get { return selectedFamilyList; }
            set
            {
                selectedFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamilyList"));
                #region Close and Image Button Visibility [rdixit][10.04.2024][GEOS2-5613]
                if (value == null || selectedFamilyList?.Count == 0)
                {
                    ClearFamily = Visibility.Hidden;
                    NoFamilyAction();
                }
                else
                    ClearFamily = Visibility.Visible;
                //[rdixit][10.04.2024][GEOS2-5613]
                if (value == null || selectedFamilyList?.Count == 0 || selectedFamilyList?.Count > 1)
                    FamilyImg = Visibility.Hidden;
                else
                    FamilyImg = Visibility.Visible;

                #endregion

                #region [GEOS2-5447][rdixit][15.03.2024] 
                if ((SelectedCompanyList == null || SelectedCompanyList?.Count < 1) && (SelectedFamilyList == null || SelectedFamilyList?.Count < 1) && string.IsNullOrEmpty(Reference))
                {
                    IsFamilySelected = false;
                }
                else
                {
                    IsFamilySelected = true;
                }
                #endregion
            }
        }
        public List<object> SelectedSubfamilyList
        {
            get
            {
                return selectedSubfamilyList;
            }
            set
            {
                selectedSubfamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSubfamilyList"));
                #region Close and Image Button Visibility [rdixit][10.04.2024][GEOS2-5613]
                if (value == null || selectedSubfamilyList?.Count == 0)
                    ClearSubfamily = Visibility.Hidden;
                else
                    ClearSubfamily = Visibility.Visible;
                //[rdixit][10.04.2024][GEOS2-5613]
                if (value == null || selectedSubfamilyList?.Count == 0 || selectedSubfamilyList?.Count > 1)
                    SubfamilyImg = Visibility.Hidden;
                else
                    SubfamilyImg = Visibility.Visible;
                #endregion
            }
        }
        public List<object> SelectedColorList
        {
            get { return selectedColorList; }
            set
            {
                selectedColorList = value;
                if (value == null || selectedColorList?.Count == 0)
                    ClearColor = Visibility.Hidden;
                else
                    ClearColor = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColorList"));
            }
        }
        public List<object> SelectedGenderList
        {
            get { return selectedGenderList; }
            set
            {
                selectedGenderList = value;
                if (value == null || selectedGenderList?.Count == 0)
                    ClearGender = Visibility.Hidden;
                else
                    ClearGender = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGenderList"));
            }
        }
        public List<object> SelectedCompanyList
        {
            get { return selectedCompanyList; }
            set
            {
                selectedCompanyList = value;
                #region [GEOS2-5447][rdixit][15.03.2024]
                if ((SelectedCompanyList == null || SelectedCompanyList?.Count < 1) && (SelectedFamilyList == null || SelectedFamilyList?.Count < 1) && string.IsNullOrEmpty(Reference))
                {
                    IsFamilySelected = false;
                }
                else
                {
                    IsFamilySelected = true;
                }
                #endregion

                if (value == null || selectedCompanyList?.Count == 0)
                    ClearCompany = Visibility.Hidden;
                else
                    ClearCompany = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompanyList"));
            }
        }
        #endregion
        public LookupValue SelectedShape
        {
            get { return selectedShape; }
            set
            {
                selectedShape = value;
                if (value == null)
                    ClearShape = Visibility.Hidden;
                else
                    ClearShape = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedShape"));
            }
        }
        public bool SealingDefaultValue
        {
            get
            {
                return sealingDefaultValue;
            }
            set
            {
                sealingDefaultValue = value;
                if (sealingflag)
                {
                    if (SealingDefaultValue == true && UnSealedValue == true)//[rdixit][GEOS2-5160][12.01.2024]
                        UnSealedValue = false;
                    //else if (SealingDefaultValue == false && UnSealedValue == false)
                    //    UnSealedValue = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SealingDefaultValue"));
            }
        }
        public bool UnSealedValue
        {
            get
            {
                return unSealedValue;
            }
            set
            {
                unSealedValue = value;
                if (sealingflag)
                {
                    if (SealingDefaultValue == true && UnSealedValue == true)//[rdixit][GEOS2-5160][12.01.2024
                        SealingDefaultValue = false;
                    //else if (SealingDefaultValue == false && UnSealedValue == false)
                    //    SealingDefaultValue = true;
                }
                OnPropertyChanged(new PropertyChangedEventArgs("UnSealedValue"));
            }
        }
        public bool IsInternalEnable
        {
            get
            {
                return isInternalEnable;
            }
            set
            {
                isInternalEnable = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsInternalEnable"));
            }
        }
        public ConnectorProperties SealingProp
        {
            get { return sealingProp; }
            set
            {
                sealingProp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SealingProp"));
            }
        }
        public ConnectorProperties ColorProp
        {
            get { return colorProp; }
            set
            {
                colorProp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColorProp"));
            }
        }
        public ConnectorProperties GenderProp
        {
            get { return genderProp; }
            set
            {
                genderProp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GenderProp"));
            }
        }
        public Visibility ClearCompany
        {
            get { return clearCompany; }
            set
            {
                clearCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClearCompany"));
            }
        }
        public Visibility ClearFamily
        {
            get { return clearFamily; }
            set
            {
                clearFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClearFamily"));
            }
        }
        public Visibility ClearSubfamily
        {
            get { return clearSubfamily; }
            set { clearSubfamily = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearSubfamily")); }
        }
        public Visibility ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearColor")); }
        }
        public Visibility ClearGender
        {
            get { return clearGender; }
            set { clearGender = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearGender")); }
        }
        public Visibility ClearShape
        {
            get { return clearShape; }
            set { clearShape = value; OnPropertyChanged(new PropertyChangedEventArgs("ClearShape")); }
        }
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));

                #region [GEOS2-5447][rdixit][15.03.2024]
                if ((SelectedCompanyList == null || SelectedCompanyList?.Count < 1) && (SelectedFamilyList == null || SelectedFamilyList?.Count < 1) && string.IsNullOrEmpty(Reference))
                {
                    IsFamilySelected = false;
                }
                else
                {
                    IsFamilySelected = true;
                }
                #endregion           
            }
        }
        public ConnectorProperties Ways
        {
            get { return ways; }
            set { ways = value; OnPropertyChanged(new PropertyChangedEventArgs("Ways")); }
        }
        public ConnectorProperties DiameterInternal
        {
            get { return diameterInternal; }
            set { diameterInternal = value; OnPropertyChanged(new PropertyChangedEventArgs("DiameterInternal")); }
        }
        public ConnectorProperties DiameterExternal
        {
            get { return diameterExternal; }
            set { diameterExternal = value; OnPropertyChanged(new PropertyChangedEventArgs("DiameterExternal")); }
        }
        public ConnectorProperties Height
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
        public ConnectorProperties Length
        {
            get { return length; }
            set { length = value; OnPropertyChanged(new PropertyChangedEventArgs("Length")); }
        }
        public ConnectorProperties Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged(new PropertyChangedEventArgs("Width")); }
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
        public Visibility IsThumbnailsViewVisible
        {
            get
            {
                return isThumbnailsViewVisible;
            }

            set
            {
                isThumbnailsViewVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsThumbnailsViewVisible"));
            }
        }
        public ObservableCollection<DocumentPanel> ListDocumentPanelNew
        {
            get { return listDocumentPanelNew; }
            set
            {
                listDocumentPanelNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDocumentPanelNew"));
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
            }
        }
        public bool IsFamilySelected
        {
            get { return isFamilySelected; }
            set
            {
                isFamilySelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFamilySelected"));
            }
        }
        private bool showDeleted;
        public bool ShowDeleted
        {
            get { return showDeleted; }
            set
            {
                showDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowDeleted"));
                if (showDeleted==true)
                {

                }
            }
        }
        #endregion

        #region Public Icommand
        public ICommand SearchCommand { get; private set; }
        public ICommand ShowCardViewCommand { get; set; }
        public ICommand ShowGridViewCommand { get; set; }
        public ICommand EditConnectorViewAcceptButtonCommand { get; set; }
        public ICommand FamilyPopupClosedCommand { get; set; }
        public ICommand OpenSubFamilyImagesCommand { get; set; }
        public ICommand OpenFamilyImagesCommand { get; set; }
        public ICommand AddNewComponentFilerButtonCommand { get; set; }
        public ICommand DeleteComponentFilterCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand PrintLabeltCommand { get; set; } //[GEOS2-8448][rdixit][23.09.2025]
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public ICommand DeleteConnectorButtonCommand { get; set; }
        #endregion

        #region Constructor
        public ConnectorViewModel()
        {
            try
            {
                //[GEOS2-5160][rdixit][17.01.2024]
                IsInternalEnable = true;
                sealingflag = false;
                SCMCommon.Instance.IsPinned = false;
                SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>();
                CancelButtonCommand = new DelegateCommand<object>(CloseTab);
                ClearCommand = new DelegateCommand<object>(ClearCommandAction);
                SearchCommand = new DelegateCommand<object>(SearchCommandAction);
                AddNewComponentFilerButtonCommand = new DelegateCommand<object>(AddNewComponentFilerAction);
                DeleteComponentFilterCommand = new RelayCommand(new Action<object>(DeleteComponentFilterItem));
                OpenFamilyImagesCommand = new DelegateCommand<object>(OpenFamilyImagesAction);
                OpenSubFamilyImagesCommand = new DelegateCommand<object>(OpenSubFamilyImagesAction);
                ShowCardViewCommand = new RelayCommand(new Action<object>(CardViewAction));
                ShowGridViewCommand = new RelayCommand(new Action<object>(GridViewAction));
                FamilyPopupClosedCommand = new DelegateCommand<object>(FamilyPopupClosedAction);
                EditConnectorViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditConnector));//pramod.misal GEOS2-5479 30-04-2024
                DeleteConnectorButtonCommand = new RelayCommand(new Action<object>(DeleteConnector));  //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
                PrintLabeltCommand = new RelayCommand(new Action<object>(PrintLabeltCommandAction));
                GeosApplication.Instance.Logger.Log(string.Format("Method ConnectorViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods
        public void CloseTab(object tab)
        {
            if (tab != null)
            {
                ITabViewModel tab1 = (ITabViewModel)tab;
                var Gridview = tab;
                if (Gridview is ConnectorDetailViewMode detailVM)
                {
                    detailVM.EditConnectorViewCancelButtonCommand?.Execute(null);// call the command
                }

                if (SCMCommon.Instance.TabIndex != -1 && SCMCommon.Instance.Tabs != null)
                {
                    SCMCommon.Instance.Tabs.Remove(tab1);
                    SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>(SCMCommon.Instance.Tabs);
                }
            }
        }
        //[rdixit][GEOS2-6984][27.03.2025]
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...Started", category: Category.Info, priority: Priority.Low);
                language = GeosApplication.Instance.CurrentCulture;
                /*
                SetPermission();
                FillCompany();
                FillFamily();
                FillSubfamily();
                FillColor();
                FillShape();
                FillGender();
                FillCustomData();
                FillComponentType();
                */
                var tasks = new List<Task>();
                // Run all methods in parallel
                tasks.Add(Task.Run(() => SetPermission()));
                tasks.Add(Task.Run(() => FillCompany()));
                tasks.Add(Task.Run(() => FillFamily()));
                tasks.Add(Task.Run(() => FillSubfamily()));
                tasks.Add(Task.Run(() => FillColor()));
                tasks.Add(Task.Run(() => FillShape()));
                tasks.Add(Task.Run(() => FillGender()));
                Task.WaitAll(tasks.ToArray());

                AllCustomFieldsList = new ObservableCollection<ConnectorProperties>(SCMService.GetCustomeProperties_V2670());
                CustomFieldsList = new ObservableCollection<ConnectorProperties>();//new ObservableCollection<ConnectorProperties>(AllCustomFieldsList.Select(i => (ConnectorProperties)i.Clone()));
                FillComponentType();

                ClearCompany = Visibility.Hidden;
                ClearFamily = Visibility.Hidden;
                ClearSubfamily = Visibility.Hidden;
                ClearColor = Visibility.Hidden;
                ClearGender = Visibility.Hidden;
                ClearShape = Visibility.Hidden;
                IsThumbnailsViewVisible = Visibility.Hidden;
                DialogWidth = SystemParameters.WorkArea.Width - 20;
                DialogHeight = SystemParameters.WorkArea.Height - 90;
                SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
                ListDocumentPanel = new ObservableCollection<DocumentPanel>();
                ListDocumentPanelNew = new ObservableCollection<DocumentPanel>();
                Componentlist = new ObservableCollection<Components>();
                Type = "GridView";
                MyFilterString = string.Empty;
                #region empty all
                Ways = new ConnectorProperties() { IsEnabled = true };
                DiameterInternal = new ConnectorProperties() { IsEnabled = true };
                DiameterExternal = new ConnectorProperties() { IsEnabled = true };
                Height = new ConnectorProperties() { IsEnabled = true };
                Length = new ConnectorProperties() { IsEnabled = true };
                Width = new ConnectorProperties() { IsEnabled = true };
                ColorProp = new ConnectorProperties() { IsEnabled = true };
                SealingProp = new ConnectorProperties() { IsEnabled = true };
                GenderProp = new ConnectorProperties() { IsEnabled = true };
                SelectedGenderList = new List<object>();
                SelectedColorList = new List<object>();
                if (SelectedFamilyList == null || SelectedFamilyList?.Count == 0)
                    NewListSubfamily = new ObservableCollection<ConnectorSubFamily>();
                IsInternalEnable = true;
                sealingflag = false;
                SealingDefaultValue = false;
                UnSealedValue = false;
                sealingflag = true;
                SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
                //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
                #endregion
                try
                {
                    CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                GeosApplication.Instance.Logger.Log("Method Init()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][20.05.2024][GEOS2-5477]        
        private void EditConnector(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditConnector()...", category: Category.Info, priority: Priority.Low);
                var Gridview = SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                if (Gridview is ConnectorDetailViewMode detailVM)
                {
                    detailVM.EditConnectorViewAcceptButtonCommand?.Execute(null);// call the command
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditConnector() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditConnector() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
        }
        //[rdixit][GEOS2-5476][02.05.2024]   
        public void SetPermission()
        {
            if (GeosApplication.Instance.IsSCMPermissionAdmin)
            {
                IsReadOnly = false;
                IsEnabled = true;
            }
            else if (GeosApplication.Instance.IsSCMPermissionReadOnly)
            {
                IsEnabled = false;
                IsReadOnly = true;
            }
            else
            {
                IsEnabled = false;
                IsReadOnly = true;
            }

            if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
            {
                IsBasicConnectorEditReadOnly = false;
                IsBasicConnectorEditEnabled = true;
                //IsEnabled = true;
            }
            else
            {
                IsBasicConnectorEditEnabled = false;
                IsBasicConnectorEditReadOnly = true;
                //IsEnabled = false;
            }
        }
        private void FillCompany()
        {
            try
            {
                //[GEOS2-5296][rdixit][11.03.2024] [rdixit][GEOS2-6984][27.03.2025]
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                if (ListCompanyAll == null)
                    ListCompanyAll = new ObservableCollection<Company>(SCMServiceThreadLocal.GetAllCompany_V2490());
                ListCompany = new ObservableCollection<Company>(ListCompanyAll);
                SelectedCompanyList = null;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCompany()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCompany() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillFamily()
        {
            try
            {

                //Service GetAllFamilies Updated with GetAllFamilies_V2450 by [GEOS2-4958][rdixit][20.10.2023]
                //Service GetAllFamilies_V2450 Updated with GetAllFamilies_V2480 by [rdixit][GEOS2-5148,5149,5150][29.01.2024]

                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                if (!string.IsNullOrEmpty(SCMCommon.Instance.Header))
                {
                    AllFamilyList = SCMServiceThreadLocal.GetAllFamilies_V2480(language);
                    ListFamily = new ObservableCollection<Family>(AllFamilyList.Where(i => SCMCommon.Instance.SelectedTypeList.Any(k => k.IdLookupValue == i.IdType)).ToList());
                    SCMCommon.Instance.ListFamily = new ObservableCollection<Family>(AllFamilyList);
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method FillFamily()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillSubfamily()
        {
            try
            {
                SelectedSubfamilyList = new List<object>();
                //[rdixit][01.09.2023][GEOS2-4565] Updated class SubFamily to ConnectorSubFamily Hence versionwise method created using new class
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ListSubfamily = new ObservableCollection<ConnectorSubFamily>(SCMServiceThreadLocal.GetAllSubfamilies_V2480(language));//[rdixit][29.1.2024][GEOS2-5148,5149,5150]
                if (NewListSubfamily?.Count == 1)
                    SelectedSubfamilyList.AddRange(NewListSubfamily.Select(i => (object)i).ToList());
                else
                    SelectedSubfamilyList = null;
                SCMCommon.Instance.ListSubfamily = ListSubfamily;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillSubfamily()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillSubfamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillColor()
        {
            try
            {
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ListColor = new ObservableCollection<Data.Common.SCM.Color>(SCMServiceThreadLocal.GetAllColors(language));
                Data.Common.SCM.Color colorToRemove = ListColor.FirstOrDefault(x => x.Name == "None");
                if (colorToRemove != null)//[Sudhir.Jangra][GEOS2-4963]
                {
                    ListColor.Remove(colorToRemove);
                }
                SCMCommon.Instance.ListColor = ListColor;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillColor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillColor() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillShape()
        {
            try
            {
                ICrmService CRMServiceThreadingLocal = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ListShape = new ObservableCollection<LookupValue>(CRMServiceThreadingLocal.GetLookupValues(113));
                //SelectedShape = ListShape[0];
                GeosApplication.Instance.Logger.Log(string.Format("Method FillShape()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillShape() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillGender()
        {
            try
            {
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //[rdixit][GEOS2-4399][23.06.2023]
                ListGender = new ObservableCollection<Gender>(SCMServiceThreadLocal.GetGender(language));
                SCMCommon.Instance.ListGender = ListGender;
                SelectedGenderList = new List<object>();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillGender()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillGender() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][14.09.2023][GEOS2-4602]
        private void FillComponentType()
        {
            try
            {
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ComponentTypeList = new ObservableCollection<ComponentType>(SCMServiceThreadLocal.GetAllComponentTypes());
                GeosApplication.Instance.Logger.Log(string.Format("Method FillColor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillComponentType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillComponentType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillComponentType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ClearCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ClearCommandAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                //[GEOS2-5160][rdixit][17.01.2024]
                sealingflag = false;
                SealingDefaultValue = false;
                UnSealedValue = false;
                sealingflag = true;
                IsInternalEnable = true;
                SelectedCompanyList = null;
                SelectedFamilyList = new List<object>();
                SelectedSubfamilyList = new List<object>();
                SelectedColorList = new List<object>();
                SelectedGenderList = new List<object>();
                Reference = null;
                Ways = new ConnectorProperties() { IsEnabled = true };
                DiameterInternal = new ConnectorProperties() { IsEnabled = true };
                DiameterExternal = new ConnectorProperties() { IsEnabled = true };
                Height = new ConnectorProperties() { IsEnabled = true };
                Length = new ConnectorProperties() { IsEnabled = true };
                Width = new ConnectorProperties() { IsEnabled = true };
                ColorProp = new ConnectorProperties() { IsEnabled = true };
                SealingProp = new ConnectorProperties() { IsEnabled = true };
                GenderProp = new ConnectorProperties() { IsEnabled = true };
                Componentlist = new ObservableCollection<Components>();
                IsFamilySelected = false;
                CustomFieldsList = new ObservableCollection<ConnectorProperties>();
                NewListSubfamily = new ObservableCollection<ConnectorSubFamily>();
                SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
                SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
                GeosApplication.Instance.Logger.Log("Method ClearCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ClearCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private async void SearchCommandAction(object obj)
        {
            try
            {
                if (!IsFamilySelected)
                {
                    CustomMessageBox.Show(Application.Current.Resources["SelectFamilyConnector"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    return;
                }
                else
                {
                    ShowPleaseWait();
                    GeosApplication.Instance.Logger.Log("Method SearchCommandAction ...", category: Category.Info, priority: Priority.Low);
                    string CustomString = string.Empty;
                    string TempCustomString = string.Empty;
                    //[GEOS2-4600][rdixit][20.09.2023]
                    InformationError = null;
                    string error = EnableValidationAndGetError();
                    if (string.IsNullOrEmpty(error))
                        InformationError = null;
                    else
                        InformationError = "";
                    if (error != null)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        return;
                    }
                    await NoSearchfilterForFamily();


                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SearchCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        void ShowPleaseWait()
        {
            #region
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
                        Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;//rajashri GEOS2-5106
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            #endregion
        }
        //[GEOS2-4602][rdixit[13.09.2023]
        private void AddNewComponentFilerAction(object obj)
        {
            try
            {
                GridControl gridControl = (GridControl)obj;
                Components comp = new Components();
                comp.ColorList = new ObservableCollection<Data.Common.SCM.Color>(ListColor.Select(i => (Data.Common.SCM.Color)i.Clone()).ToList());
                comp.ComponentTypeList = new ObservableCollection<ComponentType>(ComponentTypeList.Select(i => (ComponentType)i.Clone()).ToList());
                comp.ConditionList = new ObservableCollection<string>() { "Present", "Not Present" };
                comp.SelectedCondition = "Present";
                comp.SelectedColor = null;
                comp.SelectedComponentType = null;
                Componentlist.Add(comp);
                SelectedComponent = Componentlist.FirstOrDefault();
                gridControl.RefreshData();
                gridControl.RefreshRow(Componentlist.Count);
                gridControl.View.UpdateLayout();

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewComponentFilerAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-4602][rdixit[13.09.2023]
        private void DeleteComponentFilterItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                Components testselection = (Components)gridControl.SelectedItem;
                Componentlist.Remove(testselection);
                Componentlist = new ObservableCollection<Components>(Componentlist.ToList());
                SelectedComponent = Componentlist.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method DeleteDetectionItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteDetectionItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteDetectionItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GridViewAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridViewAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                if (SCMCommon.Instance.Tabs != null && SCMCommon.Instance.Tabs.Count > 0 && SCMCommon.Instance.TabIndex > -1 && SCMCommon.Instance.TabIndex < SCMCommon.Instance.Tabs.Count)
                {
                    var Gridview = SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                    if (Gridview != null && Gridview is ConnectorsListViewModel)
                    {
                        var grouplist = ((ConnectorsListViewModel)Gridview).ConnectorSearchList;
                        if (grouplist != null)
                        {
                            foreach (var item in grouplist)
                            {
                                if (item.ConnectorList != null)
                                {
                                    item.IsTableView = true;
                                }
                            }
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method GridViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GridViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CardViewAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method CardViewAction ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                if (SCMCommon.Instance.Tabs != null && SCMCommon.Instance.Tabs.Count > 0 && SCMCommon.Instance.TabIndex > -1 && SCMCommon.Instance.TabIndex < SCMCommon.Instance.Tabs.Count)
                {
                    var Gridview = SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                    if (Gridview != null)
                    {
                        var grouplist = ((ConnectorsListViewModel)Gridview).ConnectorSearchList;
                        if (grouplist != null)
                        {
                            foreach (var item in grouplist)
                            {
                                item.IsTableView = false;
                            }
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method CardViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method CardViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region [GEOS2-4599][rdixit][15.09.2023]
        private void OpenFamilyImagesAction(object obj)
        {
            try
            {
                if (SelectedFamilyList?.Count == 1)
                {
                    Family Fam = (Family)SelectedFamilyList.FirstOrDefault();
                    if (Fam.Id > 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method OpenFamilyImagesAction()...", category: Category.Info, priority: Priority.Low);
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        FamilyAndSubFamilyImagesView FamilyAndSubFamilyImagesView = new FamilyAndSubFamilyImagesView();
                        FamilyAndSubFamilyImagesViewModel FamilyAndSubFamilyImagesViewModel = new FamilyAndSubFamilyImagesViewModel();
                        EventHandler handle = delegate { FamilyAndSubFamilyImagesView.Close(); };
                        FamilyAndSubFamilyImagesViewModel.RequestClose += handle;
                        FamilyAndSubFamilyImagesViewModel.FamilyInit(Fam);
                        FamilyAndSubFamilyImagesView.DataContext = FamilyAndSubFamilyImagesViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        FamilyAndSubFamilyImagesView.ShowDialogWindow();
                        GeosApplication.Instance.Logger.Log("Method OpenFamilyImagesAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenDetectionImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenSubFamilyImagesAction(object obj)
        {
            try
            {
                if (SelectedSubfamilyList?.Count == 1)
                {
                    ConnectorSubFamily subfam = ((ConnectorSubFamily)SelectedSubfamilyList.FirstOrDefault());
                    if (subfam.Id > 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method OpenSubFamilyImagesAction()...", category: Category.Info, priority: Priority.Low);
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        FamilyAndSubFamilyImagesView FamilyAndSubFamilyImagesView = new FamilyAndSubFamilyImagesView();
                        FamilyAndSubFamilyImagesViewModel FamilyAndSubFamilyImagesViewModel = new FamilyAndSubFamilyImagesViewModel();
                        EventHandler handle = delegate { FamilyAndSubFamilyImagesView.Close(); };
                        FamilyAndSubFamilyImagesViewModel.RequestClose += handle;
                        FamilyAndSubFamilyImagesViewModel.FamilyInit(subfam);
                        FamilyAndSubFamilyImagesView.DataContext = FamilyAndSubFamilyImagesViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        FamilyAndSubFamilyImagesView.ShowDialogWindow();
                        GeosApplication.Instance.Logger.Log("Method OpenSubFamilyImagesAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OpenDetectionImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        //[GEOS2-4600][rdixit][20.09.2023]
        private void FamilyPopupClosedAction(object obj)
        {
            try
            {
                SelectedColorList = new List<object>();
                SelectedGenderList = new List<object>();

                if (SelectedFamilyList != null)
                {
                    sealingflag = true;

                    if (SelectedFamilyList.Count == 1)
                    {
                        Family selectedfam = ((Family)SelectedFamilyList.FirstOrDefault());
                        #region Single family selected [rdixit][05.03.2024][GEOS2-5295]
                        List<ConnectorProperties> PropertiesForSelectedFamily = SCMService.GetPropertyManager_V2490()?.Where(i => i.IdFamily == selectedfam.Id).ToList();

                        #region 1 is for color               
                        ColorProp = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 1);
                        if (ColorProp != null && ListColor != null && !string.IsNullOrEmpty(ColorProp.DefaultValue))
                            SelectedColorList = new List<object>(ListColor.Where(i => i.Id == Convert.ToInt32(ColorProp.DefaultValue)).ToList());

                        #endregion

                        #region 2 is for NumWays               
                        Ways = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 2);
                        if (Ways != null)
                        {
                            if ((string.IsNullOrEmpty(Ways.MinValueNew) && string.IsNullOrEmpty(Ways.MaxValueNew)) && !string.IsNullOrEmpty(Ways.DefaultValue))
                            {
                                Ways.MinValueNew = Ways.DefaultValue;
                                Ways.MaxValueNew = Ways.DefaultValue;
                            }
                        }
                        #endregion

                        #region 3 is for Gender               
                        GenderProp = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 3);
                        if (GenderProp != null)
                        {
                            if (GenderProp != null && ListGender != null && !string.IsNullOrEmpty(GenderProp.DefaultValue))
                                SelectedGenderList = new List<object>(ListGender.Where(i => i.Id == Convert.ToInt32(GenderProp.DefaultValue)).ToList());
                        }
                        #endregion

                        #region 5 is for sealing
                        SealingProp = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 5);

                        if (SealingProp != null)
                        {
                            SealingDefaultValue = SealingProp.DefaultValue.ToLower() == "true" ? true : false;
                            IsInternalEnable = SealingProp.IsEnabled;
                        }

                        #endregion

                        #region 6 is for Height
                        Height = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 6);
                        if (Height != null)
                        {
                            if ((string.IsNullOrEmpty(Height.MinValueNew) && string.IsNullOrEmpty(Height.MaxValueNew)) && !string.IsNullOrEmpty(Height.DefaultValue))
                            {
                                Height.MinValueNew = Height.DefaultValue;
                                Height.MaxValueNew = Height.DefaultValue;
                            }
                        }
                        #endregion

                        #region 7 is for Length
                        Length = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 7);
                        if (Length != null)
                        {
                            if ((string.IsNullOrEmpty(Length.MinValueNew) && string.IsNullOrEmpty(Length.MaxValueNew)) && !string.IsNullOrEmpty(Length.DefaultValue))
                            {
                                Length.MinValueNew = Length.DefaultValue;
                                Length.MaxValueNew = Length.DefaultValue;
                            }
                        }
                        #endregion

                        #region 8 is for Width
                        Width = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 8);
                        if (Width != null)
                        {
                            if ((string.IsNullOrEmpty(Width.MinValueNew) && string.IsNullOrEmpty(Width.MaxValueNew)) && !string.IsNullOrEmpty(Width.DefaultValue))
                            {
                                Width.MinValueNew = Width.DefaultValue;
                                Width.MaxValueNew = Width.DefaultValue;
                            }
                        }
                        #endregion

                        #region 9 is for DiameterInternal
                        DiameterInternal = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 9);
                        if (DiameterInternal != null)
                        {
                            if ((string.IsNullOrEmpty(DiameterInternal.MinValueNew) && string.IsNullOrEmpty(DiameterInternal.MaxValueNew)) && !string.IsNullOrEmpty(DiameterInternal.DefaultValue))
                            {
                                DiameterInternal.MinValueNew = DiameterInternal.DefaultValue;
                                DiameterInternal.MaxValueNew = DiameterInternal.DefaultValue;
                            }
                        }
                        #endregion

                        #region 10 is for DiameterExternal
                        DiameterExternal = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 10);
                        if (DiameterExternal != null)
                        {
                            if ((string.IsNullOrEmpty(DiameterExternal.MinValueNew) && string.IsNullOrEmpty(DiameterExternal.MaxValueNew)) && !string.IsNullOrEmpty(DiameterExternal.DefaultValue))
                            {
                                DiameterExternal.MinValueNew = DiameterExternal.DefaultValue;
                                DiameterExternal.MaxValueNew = DiameterExternal.DefaultValue;
                            }
                        }
                        #endregion

                        #region Custom
                        //CustomListByIdFamily = new List<CustomProperty>();
                        //if (CustomList.Count > 0)
                        //{
                        //    CustomListByIdFamily = CustomList.Where(x => x.IdFamily == selectedfam.Id).ToList();
                        //}
                        //// StandardPropertiedbyFamily = SCMService.GetPropertyManagerByFamily_V2480();
                        ////[rdixit][GEOS2-5296][29.02.2024] //[rdixit][05.03.2024][GEOS2-5295]
                        //StandardPropertiedbyFamily = SCMService.GetPropertyManagerByFamily_V2490(selectedfam.Id);
                        //ConnectorView connectorView = new ConnectorView();
                        //connectorView.AllCustomData(grid, CustomListByIdFamily, CustomList, selectedfam, StandardPropertiedbyFamily);
                        List<ConnectorProperties> temp = new List<ConnectorProperties>();
                        foreach (Family item in SelectedFamilyList)
                        {
                            temp.AddRange(SCMService.GetPropertyManagerByFamily_V2490(item.Id));
                        }
                        CustomFieldsList = new ObservableCollection<ConnectorProperties>(temp?.Where(i => i.IsCustomProperty));
                        #endregion

                        IsFamilySelected = true;
                        NewListSubfamily = new ObservableCollection<ConnectorSubFamily>(ListSubfamily.Where(w => w.IdFamily == selectedfam.Id).ToList());
                        #endregion
                    }
                    else if (SelectedFamilyList.Count > 1)
                    {
                        #region More than one family //[rdixit][05.03.2024][GEOS2-5295]
                        List<ConnectorProperties> PropertiesForSelectedFamily = SCMService.GetPropertyManager_V2490()?.Where(i => SelectedFamilyList.Select(k => ((Family)k).Id).Contains(i.IdFamily)).ToList();

                        #region 1 is for color               
                        List<ConnectorProperties> color = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 1).ToList();
                        if (color == null || color?.Count == 0)
                            ColorProp = null;
                        else
                            ColorProp = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 2 is for NumWays  
                        List<ConnectorProperties> way = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 2).ToList();
                        if (way == null || way?.Count == 0)
                            Ways = null;
                        else
                            Ways = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 3 is for Gender  
                        List<ConnectorProperties> Gender = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 3).ToList();
                        if (Gender == null || Gender?.Count == 0)
                            GenderProp = null;
                        else
                            GenderProp = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 5 is for sealing                       
                        List<ConnectorProperties> Sealing = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 5).ToList();
                        if (Sealing == null || Sealing?.Count == 0)
                            SealingProp = null;
                        else
                            SealingProp = new ConnectorProperties() { IsEnabled = true };
                        if (SealingProp != null)
                        {
                            SealingDefaultValue = SealingProp.DefaultValue.ToLower() == "true" ? true : false;
                            IsInternalEnable = SealingProp.IsEnabled;
                        }
                        #endregion

                        #region 6 is for Height
                        List<ConnectorProperties> HeightList = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 6).ToList();
                        if (HeightList == null || HeightList?.Count == 0)
                            Height = null;
                        else
                            Height = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 7 is for Length                 
                        List<ConnectorProperties> LengthList = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 7).ToList();
                        if (LengthList == null || LengthList?.Count == 0)
                            Length = null;
                        else
                            Length = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 8 is for Width                       
                        List<ConnectorProperties> WidthList = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 8).ToList();
                        if (WidthList == null || WidthList?.Count == 0)
                            Width = null;
                        else
                            Width = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 9 is for DiameterInternal                  
                        List<ConnectorProperties> DiameterInt = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 9).ToList();
                        if (DiameterInt == null || DiameterInt?.Count == 0)
                            DiameterInternal = null;
                        else
                            DiameterInternal = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region 10 is for DiameterExternal                    
                        List<ConnectorProperties> DiameterExt = PropertiesForSelectedFamily.Where(i => i.IdConnectorProperty == 10).ToList();
                        if (DiameterExt == null || DiameterExt?.Count == 0)
                            DiameterExternal = null;
                        else
                            DiameterExternal = new ConnectorProperties() { IsEnabled = true };
                        #endregion

                        #region Custom [rdixit][05.03.2024][GEOS2-5295]
                        try
                        {
                            List<ConnectorProperties> temp = new List<ConnectorProperties>();
                            foreach (Family item in SelectedFamilyList)
                            {
                                temp.AddRange(SCMService.GetPropertyManagerByFamily_V2490(item.Id));
                            }
                            CustomFieldsList = new ObservableCollection<ConnectorProperties>(temp?.Where(i => i.IsCustomProperty).GroupBy(i => i.PropertyName).Select(g => g.First()));
                        }

                        catch (Exception ex)
                        {

                        }
                        #endregion

                        IsFamilySelected = true;

                        NewListSubfamily = new ObservableCollection<ConnectorSubFamily>(ListSubfamily.Where(w => SelectedFamilyList.Select(i => ((Family)i).Id).Contains(w.IdFamily)).ToList());
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FamilyPopupClosedAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NoFamilyAction()
        {
            Ways = new ConnectorProperties() { IsEnabled = true };
            DiameterInternal = new ConnectorProperties() { IsEnabled = true };
            DiameterExternal = new ConnectorProperties() { IsEnabled = true };
            Height = new ConnectorProperties() { IsEnabled = true };
            Length = new ConnectorProperties() { IsEnabled = true };
            Width = new ConnectorProperties() { IsEnabled = true };
            ColorProp = new ConnectorProperties() { IsEnabled = true };
            SealingProp = new ConnectorProperties() { IsEnabled = true };
            GenderProp = new ConnectorProperties() { IsEnabled = true };
            SelectedGenderList = new List<object>();
            SelectedColorList = new List<object>();
            if (SelectedFamilyList == null || SelectedFamilyList?.Count == 0)
                NewListSubfamily = new ObservableCollection<ConnectorSubFamily>();
            //[GEOS2-5160][rdixit][17.01.2024]
            IsInternalEnable = true;
            sealingflag = false;
            SealingDefaultValue = false;
            UnSealedValue = false;
            sealingflag = true;
            CustomFieldsList = new ObservableCollection<ConnectorProperties>();
            SCMCommon.Instance.CustomProperties = new Dictionary<string, string>();
            SCMCommon.Instance.CustomPropertiesMin = new Dictionary<string, string>();
            SCMCommon.Instance.CustomPropertiesMax = new Dictionary<string, string>();
            SCMCommon.Instance.CustomPropertiesList = new Dictionary<string, List<string>>();
        }
        public Dictionary<char, char> GetSimilarities()
        {
            Dictionary<char, char> charsimilarity = new Dictionary<char, char>();
            if (SimilarCharactersList != null)
            {
                foreach (var item in SimilarCharactersList)
                {
                    try
                    {
                        if (item?.CharacterA != null && item?.CharacterB != null)
                        {
                            char charA = Convert.ToChar(item.CharacterA);
                            if (!charsimilarity.ContainsKey(charA))
                            {
                                char charB = Convert.ToChar(item.CharacterB);
                                charsimilarity.Add(charA, charB);
                            }
                        }
                    }
                    catch { }
                }
            }
            return charsimilarity;
        }
        //[GEOS2-8448][rdixit][23.09.2025]
        private void PrintLabeltCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintLabeltCommandAction()...", category: Category.Info, priority: Priority.Low);
                ConnectorDetailViewMode connector = (ConnectorDetailViewMode)SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                PrintLabelView PrintLabelView = new PrintLabelView();
                PrintLabelViewModel PrintLabelViewModel = new PrintLabelViewModel();
             
                EventHandler handle = delegate { PrintLabelView.Close(); };
                PrintLabelViewModel.RequestClose += handle;
                PrintLabelView.DataContext = PrintLabelViewModel;
                PrintLabelViewModel.Init(connector);
                //PrintLabelViewModel.IsNew = true;
                //var ownerInfo = (detailView as FrameworkElement);
                //PrintLabelView.Owner = Window.GetWindow(ownerInfo);
                PrintLabelView.ShowDialogWindow();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLabeltCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLabeltCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EdPrintLabeltCommandActionitConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpinhttps://www.youtube.com/watch?list=RDM5DSs6vUBpU&v=jRAtdxz8S6M
        }
        #endregion

        #region [GEOS2-4974][GEOS2-4975][GEOS2-4977][rdixit][21.02.2024]
        async Task NoSearchfilterForFamily()
        {
            try
            {
                var dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

                var Connectors = new Connectors { Ref = Reference };
                Connectors.ShowDeleted = ShowDeleted;

                SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>(SCMService.GetSimilarCharachtersByConfiguration());   //[rdixit][19.06.2025][GEOS2-6645]

                Dictionary<char, char> charsimilarity = GetSimilarities();
                bool temp = CustomeFilter(Connectors);
                StringBuilder str = new StringBuilder();

                if (temp || (!string.IsNullOrEmpty(Connectors.Ref)) || SelectedCompanyList?.Count > 0)
                {
                    if (SelectedCompanyList?.Count > 0)
                        Connectors.SelectedCompanyList = SelectedCompanyList.Select(i => (Company)i).ToList();
                    if (SelectedSubfamilyList?.Count > 0)
                        Connectors.SelectedSubfamilyList = SelectedSubfamilyList.Select(i => (ConnectorSubFamily)i).ToList();
                    if (SelectedColorList?.Count > 0)
                        Connectors.SelectedColorList = SelectedColorList.Select(i => (Data.Common.SCM.Color)i).ToList();
                    if (SelectedGenderList?.Count > 0)
                        Connectors.SelectedGenderList = SelectedGenderList.Select(i => (Gender)i).ToList();

                    Connectors.ConnectorType = string.Join(",", SCMCommon.Instance.SelectedTypeList.Select(i => $"'{i.IdLookupValue}'"));
                    Connectors.SelectedShape = SelectedShape;
                    if (Ways != null) Connectors.WaysProp = Ways;
                    if (DiameterInternal != null) Connectors.DiameterInternalProp = DiameterInternal;
                    if (DiameterExternal != null) Connectors.DiameterExternalProp = DiameterExternal;
                    if (Height != null) Connectors.HeightProp = Height;
                    if (Length != null) Connectors.LengthProp = Length;
                    if (Width != null) Connectors.WidthProp = Width;
                    Connectors.SealingSearch = (SealingDefaultValue == false && UnSealedValue == false) ? 2 : (SealingDefaultValue ? 1 : 0);

                    GetComponentCondition(str);
                    Connectors.Ref = Reference;

                    // ----- 1) Normal search: create VM + placeholder tab immediately -----
                    var vm = ViewModelSource.Create(() => new ConnectorsListViewModel
                    {
                        TabName = "Result Search [Loading...]",
                        ParentViewModel = this
                    });

                    var normalGroup = new SearchConnector
                    {
                        Header = "Loading..",
                        ConnectorList = new ObservableCollection<Connectors>(),
                        IsTableView = GeosApplication.Instance.UserSettings["DefaultView"] != "Thumbnails"
                    };

                    vm.ConnectorSearchList.Add(normalGroup);
                    SCMCommon.Instance.Tabs.Add(vm);
                    SCMCommon.Instance.IsPinned = false;
                    SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.Count - 1;

                    // Now fetch first batch in background
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
                            //var firstBatch = SCMService.GetAllConnectors_V2670(Connectors, str.ToString(), false);
                            //SCMService = new SCMServiceController("localhost:6699");
                            var firstBatch = SCMService.GetAllConnectors_V2680(Connectors, str.ToString(), false);
                            dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (firstBatch?.Count > 0)
                                {
                                    foreach (var c in firstBatch)
                                        normalGroup.ConnectorList.Add(c);

                                    normalGroup.Header = $"Normal Search [{normalGroup.ConnectorList.Count}]";
                                    vm.TabName = firstBatch.Count == 1
                                        ? firstBatch.First().Ref
                                        : $"Result Search [{normalGroup.ConnectorList.Count}]";

                                    _ = Task.Run(() => PrefetchConnectorImages(normalGroup.ConnectorList));
                                }
                                else
                                {
                                    normalGroup.Header = "Normal Search [0]";
                                    vm.TabName = "Result Search [0]";
                                }
                            }));

                            // ----- 2) Load rest of normal search async -----
                            if (firstBatch?.Count > 99)
                            {
                                //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
                                //var rest = SCMService.GetAllConnectors_V2670(Connectors, str.ToString(), true);
                                //SCMService = new SCMServiceController("localhost:6699");
                                var rest = SCMService.GetAllConnectors_V2680(Connectors, str.ToString(), true);
                                if (rest?.Count > 0)
                                {
                                    dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        var filtered = rest
                                            .Where(sim => !normalGroup.ConnectorList.Any(n => n.IdConnector == sim.IdConnector))
                                            .ToList();

                                        foreach (var c in filtered)
                                            normalGroup.ConnectorList.Add(c);

                                        normalGroup.Header = $"Normal Search [{normalGroup.ConnectorList.Count}]";
                                        vm.TabName = $"Result Search [{vm.ConnectorSearchList.Sum(g => g.ConnectorList?.Count ?? 0)}]";

                                        _ = Task.Run(() => PrefetchConnectorImages(normalGroup.ConnectorList));
                                    }));
                                }
                                else
                                {
                                    dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        normalGroup.Header = $"Normal Search [{normalGroup.ConnectorList.Count}]";
                                    }));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log($"Error loading first/extra batch: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                        }
                    });

                    // ----- 3) Similar search (unchanged from your code, still async) -----
                    if (!string.IsNullOrEmpty(Reference))
                    {
                        var variations = new List<string>();
                        GenerateVariationsHelper(Reference, 0, variations, charsimilarity);
                        variations.Remove(Reference);

                        if (variations?.Count > 0)
                        {
                            var connectorsSimilar = new Connectors { Ref = string.Join(",", variations) };

                            _ = Task.Run(() =>
                            {
                                try
                                {
                                    //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
                                    // var firstBatchSim = SCMService.GetAllConnectors_V2670(connectorsSimilar, str.ToString(), false);
                                   // SCMService = new SCMServiceController("localhost:6699");
                                    var firstBatchSim = SCMService.GetAllConnectors_V2680(connectorsSimilar, str.ToString(), false);
                                    if (firstBatchSim?.Count > 0)
                                    {
                                        dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            var filtered = firstBatchSim
                                                .Where(sim => !normalGroup.ConnectorList.Any(n => n.IdConnector == sim.IdConnector))
                                                .ToList();

                                            if (filtered.Count == 0) return;

                                            var similarGroup = new SearchConnector
                                            {
                                                Header = $"Similar Search [{filtered.Count}]",
                                                ConnectorList = new ObservableCollection<Connectors>(filtered),
                                                IsTableView = GeosApplication.Instance.UserSettings["DefaultView"] != "Thumbnails"
                                            };
                                            vm.ConnectorSearchList.Add(similarGroup);

                                            vm.TabName = $"Result Search [{vm.ConnectorSearchList.Sum(g => g.ConnectorList?.Count ?? 0)}]";

                                            _ = Task.Run(() => PrefetchConnectorImages(similarGroup.ConnectorList));
                                            // load rest async
                                            _ = Task.Run(() =>
                                            {
                                                try
                                                {
                                                    //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
                                                    //var restSim = SCMService.GetAllConnectors_V2670(connectorsSimilar, str.ToString(), true);
                                                  //  SCMService = new SCMServiceController("localhost:6699");
                                                    var restSim = SCMService.GetAllConnectors_V2680(connectorsSimilar, str.ToString(), true);
                                                    if (restSim?.Count > 0)
                                                    {
                                                        var newItems = restSim
                                                            .Where(r => !normalGroup.ConnectorList.Any(c => c.IdConnector == r.IdConnector)
                                                            && !similarGroup.ConnectorList.Any(c => c.IdConnector == r.IdConnector)).ToList();
                                                        
                                                        if (newItems.Count > 0)
                                                        {
                                                            dispatcher.BeginInvoke(new Action(() =>
                                                            {
                                                                foreach (var c in newItems)
                                                                    similarGroup.ConnectorList.Add(c);

                                                                similarGroup.Header = $"Similar Search [{similarGroup.ConnectorList.Count}]";
                                                                vm.TabName = $"Result Search [{vm.ConnectorSearchList.Sum(g => g.ConnectorList?.Count ?? 0)}]";

                                                                _ = Task.Run(() => PrefetchConnectorImages(similarGroup.ConnectorList));
                                                            }));
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GeosApplication.Instance.Logger.Log($"Error loading rest of similar search: {ex.Message}",
                                                        category: Category.Exception, priority: Priority.Low);
                                                }
                                            });
                                        }));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log($"Error fetching similar results: {ex.Message}",
                                        category: Category.Exception, priority: Priority.Low);
                                }
                            });
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SearchCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in method SearchCommandAction() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-4974][GEOS2-4975][GEOS2-4977][rdixit][21.02.2024]
        private void PrefetchConnectorImages(IEnumerable<Connectors> connectors)
        {
            var dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            foreach (var connector in connectors)
            {
                Task.Run(() =>
                {
                    try
                    {
                        if (!GeosApplication.isServiceDown && !string.IsNullOrEmpty(connector.ConnectorsImagePath))
                        {
                            byte[] bytes = null;
                            if (GeosApplication.ImageUrlBytePair == null)
                                GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                            if (GeosApplication.ImageUrlBytePair?.TryGetValue(connector.ConnectorsImagePath, out bytes) == true)
                            {
                                dispatcher.BeginInvoke(new Action(() => { connector.ConnectorsImageInBytes = bytes; }));
                            }
                            else
                            {
                                try
                                {
                                    bytes = GeosApplication.IsImageURLException == false
                                        ? ImageUtil.GetImageByWebClient(connector.ConnectorsImagePath)
                                        : GeosService.GetImagesByUrl(connector.ConnectorsImagePath);

                                    dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        connector.ConnectorsImageInBytes = bytes;
                                        GeosApplication.ImageUrlBytePair[connector.ConnectorsImagePath] = bytes;
                                    }));
                                }
                                catch
                                {
                                    // fallback for VPN scenario
                                    if (!GeosApplication.IsImageURLException)
                                        GeosApplication.IsImageURLException = true;

                                    try
                                    {
                                        bytes = GeosService.GetImagesByUrl(connector.ConnectorsImagePath);
                                        dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            connector.ConnectorsImageInBytes = bytes;
                                            GeosApplication.ImageUrlBytePair[connector.ConnectorsImagePath] = bytes;
                                        }));
                                    }
                                    catch
                                    {
                                        GeosApplication.isServiceDown = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log($"Image prefetch failed for {connector.Ref}: {ex.Message}",category: Category.Exception, priority: Priority.Low);
                    }
                });
            }
        }



        //[rdixit][19.06.2025][GEOS2-6645]
        static void GenerateVariationsHelper(string _ref, int pos, List<string> vars, Dictionary<char, char> similarity)
        {
            try
            {
                if (pos == _ref.Length)
                {
                    vars.Add(_ref);
                    return;
                }
                char currentChar = _ref[pos];
                if (similarity != null)
                {
                    if (similarity.ContainsKey(currentChar))
                        GenerateVariationsHelper(_ref.Substring(0, pos) + similarity[currentChar] + _ref.Substring(pos + 1), pos + 1, vars, similarity);
                    else if (similarity.ContainsValue(currentChar))
                        GenerateVariationsHelper(_ref.Substring(0, pos) + similarity.FirstOrDefault(i => i.Value == currentChar).Key + _ref.Substring(pos + 1), pos + 1, vars, similarity);
                    GenerateVariationsHelper(_ref, pos + 1, vars, similarity);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method GenerateVariationsHelper." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }

        bool CustomeFilter(Connectors Connectors)
        {
            try
            {
                if (SelectedFamilyList?.Count > 0)
                {
                    List<ConnectorProperties> propertyFamilyList = new List<ConnectorProperties>();
                    string SelectedFamilies = string.Join(",", SelectedFamilyList.Select(i => ((Family)i).Id));//[GEOS2-7855][rani dhamankar][15-05-2025]
                    propertyFamilyList = SCMService.GetPropertyManagerByFamily_V2640(SelectedFamilies);//[GEOS2-7855][rani dhamankar][15-05-2025]
                    List<uint> validfamily = SelectedFamilyList.Select(i => ((Family)i).Id).ToList();

                    foreach (var item in SelectedFamilyList)
                    {
                        //[rdixit][05.03.2024][GEOS2-5295]                        
                        uint familyId = ((Family)item).Id;//[GEOS2-7855][rani dhamankar][15-05-2025]
                        StandardPropertiedbyFamily = propertyFamilyList.Where(a => a.IdFamily == familyId).ToList();//[GEOS2-7855][rani dhamankar][15-05-2025]
                        var CustomProp = StandardPropertiedbyFamily.Where(i => !string.IsNullOrEmpty(i.DefaultValue) && i.IsCustomProperty).ToList();
                        if (StandardPropertiedbyFamily != null)
                        {
                            foreach (var prop in CustomProp)
                            {
                                if (prop.IdConnectorType == 1825 && !string.IsNullOrEmpty(prop.DefaultValue))//number
                                {
                                    #region
                                    KeyValuePair<string, string>? minval = SCMCommon.Instance.CustomPropertiesMin.Where(i => i.Key == prop.PropertyName).FirstOrDefault();
                                    KeyValuePair<string, string>? maxval = SCMCommon.Instance.CustomPropertiesMax.Where(i => i.Key == prop.PropertyName).FirstOrDefault();
                                    if (minval.HasValue && !string.IsNullOrEmpty(minval.Value.Value) && maxval.HasValue && !string.IsNullOrEmpty(maxval.Value.Value))
                                    {
                                        if (!(Convert.ToInt32(minval.Value.Value) <= Convert.ToInt32(prop.DefaultValue) && Convert.ToInt32(prop.DefaultValue) <= Convert.ToInt32(maxval.Value.Value)))
                                            validfamily.Remove(((Family)item).Id);
                                    }
                                    else if ((minval.HasValue && !string.IsNullOrEmpty(minval.Value.Value)) && (!maxval.HasValue && string.IsNullOrEmpty(maxval.Value.Value)))
                                    {
                                        if (!(Convert.ToInt32(minval.Value.Value) <= Convert.ToInt32(prop.DefaultValue)))
                                            validfamily.Remove(((Family)item).Id);
                                    }
                                    else if ((!minval.HasValue && string.IsNullOrEmpty(minval.Value.Value)) && ((maxval.HasValue) && !string.IsNullOrEmpty(maxval.Value.Value)))
                                    {
                                        if (!(Convert.ToInt32(prop.DefaultValue) <= Convert.ToInt32(maxval.Value.Value)))
                                            validfamily.Remove(((Family)item).Id);
                                    }
                                    #endregion
                                }
                                else if (prop.IdConnectorType == 1843 && !string.IsNullOrEmpty(prop.DefaultValue))//text
                                {
                                    #region
                                    KeyValuePair<string, string>? text = SCMCommon.Instance.CustomProperties.Where(i => i.Key == prop.PropertyName).FirstOrDefault();
                                    if (text.HasValue && !string.IsNullOrEmpty(text.Value.Value))
                                    {
                                        if (prop.DefaultValue?.ToLower() != text.Value.Value.ToLower())
                                            validfamily.Remove(((Family)item).Id);
                                    }
                                    #endregion
                                }
                                else if (prop.IdConnectorType == 1826 && !string.IsNullOrEmpty(prop.DefaultValue))//list
                                {
                                    #region
                                    KeyValuePair<string, List<string>>? selectedstringEntry = new KeyValuePair<string, List<string>>();
                                    selectedstringEntry = SCMCommon.Instance.CustomPropertiesList.Where(i => i.Key == prop.PropertyName).FirstOrDefault();

                                    if (selectedstringEntry != null && selectedstringEntry.HasValue && selectedstringEntry.Value.Value != null)
                                    {
                                        var selectedstring = selectedstringEntry.Value.Value;
                                        if (!selectedstring.Any(i => i.ToLower() == prop.DefaultValue.ToLower()))
                                        {
                                            validfamily.Remove(((Family)item).Id);
                                        }
                                    }
                                    #endregion
                                }
                                else if (prop.IdConnectorType == 1827 && !string.IsNullOrEmpty(prop.DefaultValue))//boolean
                                {
                                    #region
                                    KeyValuePair<string, string>? text = SCMCommon.Instance.CustomProperties.Where(i => i.Key == prop.PropertyName).FirstOrDefault();
                                    if (text.HasValue && !string.IsNullOrEmpty(text.Value.Value))
                                    {
                                        if (prop.DefaultValue?.ToLower() != text.Value.Value.ToLower())
                                            validfamily.Remove(((Family)item).Id);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    var FinalFamilyList = SelectedFamilyList.Where(i => validfamily.Any(k => k == ((Family)i).Id))?.ToList();
                    if (FinalFamilyList?.Count > 0)
                        Connectors.SelectedFamilyList = FinalFamilyList.Select(i => (Family)i)?.ToList();

                    return (validfamily?.Count > 0 && SelectedFamilyList?.Count > 0) || SelectedFamilyList?.Count == 0;
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CustomeFilter() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return false;
        }

        void GetComponentCondition(StringBuilder str)
        {
            try
            {
                #region [GEOS2-6601][16.11.2024][rdixit]
                if (Componentlist?.Count > 0)
                {
                    str.Append(" where ");
                    var allConditions = new List<string>();

                    foreach (var item in Componentlist)
                    {
                        var conditions = new List<string>();

                        if (item.SelectedCondition == "Present")
                        {
                            if (!string.IsNullOrEmpty(item.SelectedComponentType))
                            {
                                var IdType = item.ComponentTypeList.FirstOrDefault(i => i.Name == item.SelectedComponentType)?.IdType;
                                if (IdType != null)
                                {
                                    conditions.Add($"IdType = {IdType}");
                                }
                            }

                            if (!string.IsNullOrEmpty(item.SelectedColor))
                            {
                                var IdColor = item.ColorList.FirstOrDefault(i => i.Name == item.SelectedColor)?.Id;
                                if (IdColor != null)
                                {
                                    conditions.Add($"IdColor = {IdColor}");
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Reference))
                            {
                                conditions.Add($"Ref = '{item.Reference}'");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.SelectedComponentType))
                            {
                                var IdType = item.ComponentTypeList.FirstOrDefault(i => i.Name == item.SelectedComponentType)?.IdType;
                                if (IdType != null)
                                {
                                    conditions.Add($"IdType <> {IdType}");
                                }
                            }

                            if (!string.IsNullOrEmpty(item.SelectedColor))
                            {
                                var IdColor = item.ColorList.FirstOrDefault(i => i.Name == item.SelectedColor)?.Id;
                                if (IdColor != null)
                                {
                                    conditions.Add($"IdColor <> {IdColor}");
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Reference))
                            {
                                conditions.Add($"Ref <> '{item.Reference}'");
                            }
                        }

                        if (conditions.Any())
                        {
                            allConditions.Add("IdConnector IN ( SELECT IdConnector FROM componentsbyconnector WHERE " + string.Join(" AND ", conditions) + ")");
                        }
                    }

                    if (allConditions.Any())
                    {
                        str.Append(string.Join(" AND ", allConditions));
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetComponentCondition() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void DeleteConnector(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteConnector()...", category: Category.Info, priority: Priority.Low);
                var Gridview = SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                if (Gridview is ConnectorDetailViewMode detailVM)
                {
                    detailVM.DeleteConnectorButtonCommand?.Execute(null);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteConnector() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteConnector() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation  [GEOS2-4600][rdixit][20.09.2023]
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
                me["Ways.DefaultValue"] +
                me["Height.DefaultValue"] +
                me["Length.DefaultValue"] +
                me["Width.DefaultValue"] +
                me["DiameterInternal.DefaultValue"] +
                me["DiameterExternal.DefaultValue"];

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
                string ways = "Ways.DefaultValue";
                string diameterInternal = "DiameterInternal.DefaultValue";
                string diameterExternal = "DiameterExternal.DefaultValue";
                string height = "Height.DefaultValue";
                string length = "Length.DefaultValue";
                string width = "Width.DefaultValue";

                if (columnName == ways)
                {
                    if (Ways != null)
                        return SCMConnectorValidation.GetErrorMessage(ways, Ways);
                }
                if (columnName == diameterInternal)
                {
                    if (DiameterInternal != null)
                        return SCMConnectorValidation.GetErrorMessage(diameterInternal, DiameterInternal);
                }
                if (columnName == diameterExternal)
                {
                    if (DiameterExternal != null)
                        return SCMConnectorValidation.GetErrorMessage(diameterExternal, DiameterExternal);
                }
                if (columnName == height)
                {
                    if (Height != null)
                        return SCMConnectorValidation.GetErrorMessage(height, Height);
                }
                if (columnName == length)
                {
                    if (Length != null)
                        return SCMConnectorValidation.GetErrorMessage(length, Length);
                }
                if (columnName == width)
                {
                    if (Width != null)
                        return SCMConnectorValidation.GetErrorMessage(width, Width);
                }
                return null;
            }
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][20-05-2025][GEOS2-7996]
        //[rdixit][19.06.2025][GEOS2-6645]
        public async Task LoadMoreConnectorsAsync(ConnectorSearch connectorSearch1, ConnectorSearch connectorSearch, Connectors connectors, StringBuilder str)
        {
            if (allItemsLoaded) return;

            try
            {

                //var newConnectors = await Task.Run(() =>
                //    SCMService.GetAllConnectors_V2670(connectors, str.ToString(), true));
                //[Rahul.Gadhave][GEOS2-9556][Date:05/11/2025]
               // SCMService = new SCMServiceController("localhost:6699");
                var newConnectors = await Task.Run(() =>
                    SCMService.GetAllConnectors_V2680(connectors, str.ToString(), true));

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    connectorSearch.ConnectorList = new ObservableCollection<Connectors>(connectorSearch.ConnectorList.Concat(newConnectors));
                    connectorSearch1.Header = "Result Search [" + connectorSearch1.ConnectorSearchList.Sum(i => i.ConnectorList.Count) + "]";
                });
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadMoreConnectorsAsync...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

    }
}