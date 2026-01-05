using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Office.Utils;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
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
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using Color = Emdep.Geos.Data.Common.SCM.Color;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class SampleRegistrationViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        //[rdixit][GEOS2-5802][05.09.2024]

        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMService1 = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMService2 = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString()); 
        ISCMService SCMService3 = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region public Events

        private string _selectedValue;
        public event EventHandler RequestClose;
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

        #endregion // Events

        #region Methods


        public void Dispose()
        {
        }

        #endregion // Methods

        #region Declarations
        ConnectorsListViewModel vm; //[rdixit][14.10.2025][GEOS2-8895]
        double cardImageSize;
        bool isAcceptButtonEnable;
        bool isSave;
        bool isAddEnable;
        List<ConnectorProperties> propertiesForSelectedFamily;
        List<SimilarColorsByConfiguration> allColorSimilarityList;
        List<ConnectorSearch> originalconnectorSearchList;
        List<ConnectorWorkflowTransitions> connectorStatusTransition;
        List<ConnectorWorkflowStatus> connectorStatus;
        DataTable dtDrawing;
        DataTable dtDrawingCopy;
        string drawingFilterString;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        List<ConnectorWorkflowStatus> statusList;
        List<ConnectorWorkflowTransitions> workflowTransitionList;
        Family selectedFamily;
        ConnectorSubFamily selectedSubfamily;
        Data.Common.SCM.Color selectedColor;
        Gender selectedGender;
        private List<ConnectorProperties> _standardPropertiedbyFamily;
        private ObservableCollection<ConfigurationFamily> configurationList;
        List<Connectors> connectorsList = new List<Connectors>();
        static List<Connectors> staticConnectorsList;//[Sudhir.Jangra]
        private Data.Common.SCM.ValueType selectedNumber;
        private bool isFamilySelected;
        bool unSealedValue = false;
        double dialogHeight;
        double dialogWidth;
        Visibility isThumbnailsViewVisible;
        ObservableCollection<DocumentPanel> listDocumentPanelNew;
        string type;
        private List<CustomProperty> customList;
        private Data.Common.SCM.ValueType selectedCustom;
        private ObservableCollection<ConnectorProperties> customListByIdFamily;
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
        private bool isSealingEnabled;//[Sudhir.Jangra]
        ConnectorProperties sealingProp = new ConnectorProperties();
        ConnectorProperties colorProp = new ConnectorProperties();
        ConnectorProperties genderProp = new ConnectorProperties();
        ObservableCollection<SCMConnectorImage> imagesList1;
        string imageDescription;
        SCMConnectorImage selectedImage;
        private Visibility isEditConnectorView;//[Sudhir.Jangra]
        string currentImageCount;
        private List<ConnectorProperties> customePropertiesList;
        Visibility familyImg = Visibility.Hidden;
        Visibility subfamilyImg = Visibility.Hidden;
        ObservableCollection<ValueKey> valueKey;
        private ConnectorAttachements selectedConnectorAttachementFiles;//[pramod.misal][GEOS2-5387][10/04/2024]
        private SCMConnectorImage maximizedElement;
        private SCMConnectorImage selectedConnectorImage;
        private ObservableCollection<ConnectorProperties> savedDetailsForConnector;
        byte[] userProfileImageByte = null;//[pramod.misal]
        private ImageSource userProfileImage;//[pramod.misal]
        bool isReadOnly;
        bool isBasicConnectorEditReadOnly;
        bool isBasicConnectorEditEnabled;
        bool isEnabled;
        bool isUpdate;
        Int64 idconnector;
        private bool isPinned = false; //[pramod.misal][GEOS2-5700][13.05.2024]

        private List<ConnectorLogEntry> addCommentsList;//[pramod.Misal][GEOS2-][07-05-2024]
        private ConnectorLogEntry selectedComment;//[pramod.Misal][GEOS2-][07-05-2024]
        private ObservableCollection<ConnectorLogEntry> conConnectorCommentsList;//[pramod.Misal][GEOS2-][07-05-2024]
        private List<ConnectorLogEntry> updatedCommentList;//[pramod.misal][GEOS2-][08-05-2024]
        private ObservableCollection<ConnectorLogEntry> deleteCommentsList;//[pramod.misal][GEOS2-4935][08-05-2024]
        private bool isDeleted;//[pramod.misal][GEOS2-4935][08-05-2024]
        private string commentText;//[pramod.misal][GEOS2-4935][08-05-2024]
        bool isUpdated;//[pramod.misal][GEOS2-4935][08-05-2024]
        private string commentFullNameText;
        private DateTime? commentDateTimeText;
        private ConnectorAttachements selectedConnectorFile;
        public bool isEnabledCancelButton = false;
        private ConnectorSearch clonedConnector;
        private int selectedImageIndex;


        //[rani dhamankar][21-05-2025][GEOS2-8133]
        bool allowPaging;
        int resultPages;

        #endregion

        #region Properties 
        //[rdixit][14.04.2025][GEOS2-6631]
        public double CardImageSize
        {
            get { return cardImageSize; }
            set
            {
                cardImageSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CardImageSize"));
            }
        }
        //[GEOS2-5803] [rdixit][13.09.2024]
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public bool IsAcceptButtonEnable
        {
            get
            {
                return isAcceptButtonEnable;
            }

            set
            {
                isAcceptButtonEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptButtonEnable"));
            }
        }
        public bool IsAddEnable
        {
            get
            {
                return isAddEnable;
            }

            set
            {
                isAddEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddEnable"));
            }
        }
        public int SelectedImageIndex
        {
            get
            {
                return selectedImageIndex;
            }

            set
            {
                selectedImageIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImageIndex"));
            }
        }
        public List<ConnectorSearch> OriginalConnectorSearchList
        {
            get { return originalconnectorSearchList; }
            set
            {
                originalconnectorSearchList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalConnectorSearchList"));
            }
        }
        public List<ConnectorWorkflowTransitions> ConnectorStatusTransition
        {
            get { return connectorStatusTransition; }
            set
            {
                connectorStatusTransition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorStatusTransition"));
            }
        }
        public List<ConnectorWorkflowStatus> ConnectorStatus
        {
            get { return connectorStatus; }
            set
            {
                connectorStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorStatus"));
            }
        }
        public bool ImageOpen
        {
            get; set;
        }
        public bool IsUpdate
        {
            get { return isUpdate; }
            set
            {
                isUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate"));
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
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public DataTable DtDrawing
        {
            get { return dtDrawing; }
            set
            {
                dtDrawing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDrawing"));
            }
        }
        public DataTable DtDrawingCopy
        {
            get { return dtDrawingCopy; }
            set
            {
                dtDrawingCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDrawingCopy"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public List<ConnectorWorkflowStatus> StatusList
        {
            get
            {
                return statusList;
            }

            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        public List<ConnectorProperties> CustomePropertiesList
        {
            get
            {
                return customePropertiesList;
            }

            set
            {
                customePropertiesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomePropertiesList"));
            }
        }
        public List<ConnectorWorkflowTransitions> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        public ConnectorSearch SelectedConnectorSearch
        {
            get { return selectedConnectorSearch; }
            set
            {
                selectedConnectorSearch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorSearch"));
                if (SelectedConnectorSearch?.ConnectorDrawingList?.Count > 0)
                {
                    FillDrawingColumns(SelectedConnectorSearch.ConnectorDrawingList);
                    FillDrawingData(SelectedConnectorSearch.ConnectorDrawingList);
                }
                //[GEOS2-5803] [rdixit][13.09.2024]
                if (SelectedConnectorSearch?.IdConnector == 0)                
                    IsAcceptButtonEnable = true;                
                else
                    IsAcceptButtonEnable = false;
            }
        }
        public ObservableCollection<ConfigurationFamily> ConfigurationList
        {
            get { return configurationList; }
            set
            {
                configurationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfigurationList"));
            }
        }
        public List<Connectors> ConnectorsList
        {
            get { return connectorsList; }
            set
            {
                connectorsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorsList"));
            }
        }
        public static List<Connectors> StaticConnectorsList
        {
            get { return staticConnectorsList; }
            set
            {
                staticConnectorsList = value;

            }
        }
        public ObservableCollection<ConnectorSearch> ConnectorSearchList
        {
            get { return connectorSearchList; }
            set
            {
                connectorSearchList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorSearchList"));
                SelectedSearchIndex = ConnectorSearchList!=null? ConnectorSearchList.Count-1:-1;
            }
        }
        public int SelectedSearchIndex
        {
            get { return selectedSearchIndex; }
            set
            {
                selectedSearchIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSearchIndex"));
                if (SelectedSearchIndex !=0 && SelectedSearchIndex != -1 && SelectedSearchIndex < ConnectorSearchList?.Count)
                {
                    SelectedConnectorSearch = ConnectorSearchList[SelectedSearchIndex];
                }
            }
        }
        public List<ConnectorProperties> PrevProperties
        {
            get { return prevProperties; }
            set
            {
                prevProperties = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrevProperties"));
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
        public List<SimilarColorsByConfiguration> AllColorSimilarityList
        {
            get { return allColorSimilarityList; }
            set
            {
                allColorSimilarityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllColorSimilarityList"));
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
        public Family SelectedFamily
        {
            get { return selectedFamily; }
            set
            {
                selectedFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamily"));
                IsAddEnable = false;
                #region Close and Image Button Visibility [rdixit][10.04.2024][GEOS2-5613]
                if (value == null)
                {
                    ClearFamily = Visibility.Hidden;
                    ClearCommandAction(null);
                }
                else
                    ClearFamily = Visibility.Visible;

                if (value == null)
                    FamilyImg = Visibility.Hidden;
                else
                    FamilyImg = Visibility.Visible;

                #endregion

                #region [GEOS2-5447][rdixit][15.03.2024] 
                if (SelectedFamily == null)
                {
                    IsFamilySelected = false;
                }                
                #endregion
            }
        }
        public ConnectorSubFamily SelectedSubfamily
        {
            get
            {
                return selectedSubfamily;
            }
            set
            {
                selectedSubfamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSubfamily"));

                if (value == null)
                    ClearSubfamily = Visibility.Hidden;
                else
                    ClearSubfamily = Visibility.Visible;

                if (value == null)
                    SubfamilyImg = Visibility.Hidden;
                else
                    SubfamilyImg = Visibility.Visible;
                IsSearchButtonVisible(null);
            }
        }
        public Data.Common.SCM.Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                if (value == null)
                    ClearColor = Visibility.Hidden;
                else
                    ClearColor = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColor"));
                IsSearchButtonVisible(null);
            }
        }
        public Gender SelectedGender
        {
            get { return selectedGender; }
            set
            {
                selectedGender = value;
                if (value == null)
                    ClearGender = Visibility.Hidden;
                else
                    ClearGender = Visibility.Visible;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGender"));
                IsSearchButtonVisible(null);
            }
        }
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
        public ConnectorProperties Ways
        {
            get { return ways; }
            set { ways = value; OnPropertyChanged(new PropertyChangedEventArgs("Ways"));
            }
        }
        public ConnectorProperties DiameterInternal
        {
            get { return diameterInternal; }
            set { diameterInternal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiameterInternal"));           
            }
        }
        public ConnectorProperties DiameterExternal
        {
            get { return diameterExternal; }
            set { diameterExternal = value; OnPropertyChanged(new PropertyChangedEventArgs("DiameterExternal"));}
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
            set { length = value; OnPropertyChanged(new PropertyChangedEventArgs("Length"));}
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
        public List<CustomProperty> CustomList
        {
            get { return customList; }
            set
            {
                customList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomList"));
            }
        }
        public ObservableCollection<ConnectorProperties> CustomListByIdFamily
        {
            get { return customListByIdFamily; }
            set
            {
                customListByIdFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomListByIdFamily"));
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
        public ObservableCollection<ValueKey> ValueKey
        {
            get { return valueKey; }
            set
            {
                valueKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ValueKey"));
            }
        }
        public ConnectorAttachements SelectedConnectorAttachementFiles
        {
            get
            {
                return selectedConnectorAttachementFiles;
            }

            set
            {
                selectedConnectorAttachementFiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorAttachementFiles"));
            }
        }
        public SCMConnectorImage MaximizedElement
        {
            get
            {
                return maximizedElement;
            }
            set
            {
                maximizedElement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElement"));
            }
        }
        public SCMConnectorImage SelectedConnectorImage
        {
            get { return selectedConnectorImage; }
            set
            {
                selectedConnectorImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorImage"));
            }
        }
        public ObservableCollection<ConnectorProperties> SavedDetailsForConnector
        {
            get { return savedDetailsForConnector; }
            set
            {
                savedDetailsForConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SavedDetailsForConnector"));
            }
        }
        public byte[] UserProfileImageByte
        {
            get { return userProfileImageByte; }
            set
            {
                userProfileImageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImageByte"));
            }
        }
        public bool IsPinned
        {
            get { return isPinned; }
            set
            {
                isPinned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPinned"));
            }
        }
        public ConnectorLogEntry SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        public ObservableCollection<ConnectorLogEntry> ConConnectorCommentsList
        {
            get { return conConnectorCommentsList; }
            set
            {
                conConnectorCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConConnectorCommentsList"));

            }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }
        public DateTime? CommentDateTimeText
        {
            get { return commentDateTimeText; }
            set
            {
                commentDateTimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentDateTimeText"));
            }
        }
        public string CommentFullNameText
        {
            get { return commentFullNameText; }
            set
            {
                commentFullNameText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentFullNameText"));
            }
        }
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }
        public ConnectorAttachements SelectedConnectorFile
        {
            get
            {
                return selectedConnectorFile;
            }

            set
            {
                selectedConnectorFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorFile"));
            }
        }
        public bool IsEnabledCancelButton
        {
            get { return isEnabledCancelButton; }
            set
            {

                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
            }
        }
        public ConnectorSearch ClonedConnector
        {
            get
            {
                return clonedConnector;
            }

            set
            {
                clonedConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedConnector"));
            }
        }

        public List<ConnectorProperties> PropertiesForSelectedFamily
        {
            get { return propertiesForSelectedFamily; }
            set
            {
                propertiesForSelectedFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PropertiesForSelectedFamily"));
            }
        }
        //[rani dhamankar][21-05-2025][GEOS2-8133]
        public bool AllowPaging
        {
            get
            {
                return allowPaging;
            }

            set
            {
                allowPaging = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowPaging"));
            }
        }

        public int ResultPages
        {
            get
            {
                return resultPages;
            }

            set
            {
                resultPages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultPages"));
            }
        }
        #endregion

        #region Public Icommand
        //[rdixit][14.10.2025][GEOS2-8895]     
        public ICommand EditConnectorViewAcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SearchButtonVisible { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand DeleteComponentFilterCommand { get; set; }
        public ICommand AddNewComponentFilerButtonCommand { get; set; }
        public ICommand ShowCardViewCommand { get; set; }
        public ICommand ShowGridViewCommand { get; set; }
        public ICommand OpenFamilyImagesCommand { get; set; }
        public ICommand OpenSubFamilyImagesCommand { get; set; }
        public ICommand FamilyPopupClosedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        #region Constructor
        public SampleRegistrationViewModel()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //[rdixit][14.10.2025][GEOS2-8895]
                Init();
                ClearCommand = new DelegateCommand<object>(ClearCommandAction);
                SearchCommand = new DelegateCommand<object>(SearchCommandAction);
                AddNewComponentFilerButtonCommand = new DelegateCommand<object>(AddNewComponentFilerAction);
                DeleteComponentFilterCommand = new RelayCommand(new Action<object>(DeleteComponentFilterItem));
                OpenFamilyImagesCommand = new DelegateCommand<object>(OpenFamilyImagesAction);
                OpenSubFamilyImagesCommand = new DelegateCommand<object>(OpenSubFamilyImagesAction);
                ShowCardViewCommand = new RelayCommand(new Action<object>(CardViewAction));
                ShowGridViewCommand = new RelayCommand(new Action<object>(GridViewAction));
                FamilyPopupClosedCommand = new DelegateCommand<object>(FamilyPopupClosedAction);
                SearchButtonVisible = new DelegateCommand<object>(IsSearchButtonVisible);
                AddCommand = new DelegateCommand<object>(AddCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseTab);
                EditConnectorViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditConnector));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                IsThumbnailsViewVisible = Visibility.Hidden;
                DialogWidth = SystemParameters.WorkArea.Width - 20;
                DialogHeight = SystemParameters.WorkArea.Height - 90;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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

        //[rdixit][20.05.2024][GEOS2-5477]       //[rdixit][14.10.2025][GEOS2-8895]  
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

        //[rdixit][14.10.2025][GEOS2-8895]
        void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...Started", category: Category.Info, priority: Priority.Low);

                SCMCommon.Instance.IsPinned = false;
                SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>();
                language = GeosApplication.Instance.CurrentCulture;

                SetPermission();
                FillFamily();
                FillSubfamily();
                FillCustomData();
                FillComponentType();
                FillColor();
                FillShape();
                FillGender();
                FillAllColorSimilarity();
                FillAllFamilySearchConfigurations();
                ConnectorWorkflow();//[GEOS2-5803][rdixit][13.09.2024]
                ClearCommandAction(null);

                ClearCompany = Visibility.Hidden;
                ClearFamily = Visibility.Hidden;
                ClearSubfamily = Visibility.Hidden;
                ClearColor = Visibility.Hidden;
                ClearGender = Visibility.Hidden;
                ClearShape = Visibility.Hidden;
                MyFilterString = string.Empty;

                GeosApplication.Instance.Logger.Log("Method Init()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
 
        //[rdixit][14.10.2025][GEOS2-8895]
        public void CloseTab(object tab)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseTab()...Started", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method CloseTab()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseTab()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[GEOS2-5803][rdixit][13.09.2024]
        void AddCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCommandAction()...Started", category: Category.Info, priority: Priority.Low);
           
                AddConnectorViewModel addConnectorViewModel = new AddConnectorViewModel();
                AddConnectorView addConnectorView = new AddConnectorView();              
                EventHandler handle = delegate { addConnectorView.Close(); };
                addConnectorViewModel.RequestClose += handle;
                addConnectorViewModel.NewConnector = ApplyPropertyToNewConnector();
                addConnectorViewModel.Init();
                addConnectorView.DataContext = addConnectorViewModel;
                addConnectorView.ShowDialog();
                if (addConnectorViewModel.IsSave)
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

                    IsAddEnable = false;
                    var vm = ViewModelSource.Create<ConnectorDetailViewMode>();
                    vm.Init(addConnectorViewModel.Connector);
                    SCMCommon.Instance.Tabs.Add(vm);
                    SCMCommon.Instance.IsPinned = true;
                    SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>(SCMCommon.Instance.Tabs);
                    SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.Count > 0 ? SCMCommon.Instance.Tabs.Count - 1 : 0;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                GeosApplication.Instance.Logger.Log("Method AddCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        ConnectorSearch ApplyPropertyToNewConnector()
        {
            ConnectorSearch conn = new ConnectorSearch();
            try
            {
                GeosApplication.Instance.Logger.Log("Method ApplyPropertyToNewConnector()...", category: Category.Info, priority: Priority.Low);
                conn.SelectedColor = SelectedColor;
                conn.NumWays = Convert.ToInt32(Ways?.DefaultValue);
                conn.SelectedGender = SelectedGender;
                conn.Description = string.Empty;
                conn.InternalDiameter = Convert.ToInt32(DiameterInternal?.DefaultValue);
                conn.ExternalDiameter = Convert.ToInt32(DiameterExternal?.DefaultValue);
                conn.Height = Convert.ToDouble(Height?.DefaultValue);
                conn.Length = Convert.ToDouble(Length?.DefaultValue);
                conn.Width = Convert.ToDouble(Width?.DefaultValue);
                conn.SelectedFamily = SelectedFamily;
                conn.SelectedSubFamily = SelectedSubfamily;
                conn.IsSealed = SealingDefaultValue;
                conn.IsUnSealed = UnSealedValue;
                conn.ReferenceStatus = StatusList.FirstOrDefault(i => i.IdWorkflowStatus == 57).Name;
                conn.ReferenceStatusHtmlColor = StatusList.FirstOrDefault(i => i.IdWorkflowStatus == 57).HtmlColor;
                conn.SelectedReferenceStatus = StatusList.FirstOrDefault(i => i.IdWorkflowStatus == 57);
                conn.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                conn.ModifiedIn = DateTime.Now;
                conn.IdSite = GeosApplication.Instance.ActiveUser.Company.IdCompany;
                GeosApplication.Instance.Logger.Log("Method ApplyPropertyToNewConnector()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ApplyPropertyToNewConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return conn;
        }
        void IsSearchButtonVisible(object obj)
        {
            IsFamilySelected=
                (SealingProp == null || !string.IsNullOrEmpty(SealingProp.DefaultValue)) && (ColorProp == null || ListColor.Any(i => i.Id == SelectedColor?.Id)) &&
                (GenderProp == null || ListGender.Any(i => i.Id == SelectedGender?.Id)) && (Ways == null || !string.IsNullOrEmpty(Ways.DefaultValue)) &&
                (DiameterInternal == null || !string.IsNullOrEmpty(DiameterInternal.DefaultValue)) && (DiameterExternal == null || !string.IsNullOrEmpty(DiameterExternal.DefaultValue)) &&
                (Height == null || !string.IsNullOrEmpty(Height.DefaultValue)) && (Length == null || !string.IsNullOrEmpty(Length.DefaultValue)) &&
                (Width == null || !string.IsNullOrEmpty(Width.DefaultValue)) && (NewListSubfamily==null|| NewListSubfamily?.Count == 0 || NewListSubfamily.Any(i=>i.Id == SelectedSubfamily?.Id));
        }
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
        private void FillFamily()
        {
            try
            {
                ListFamily = new ObservableCollection<Family>(SCMService.GetAllFamilies_V2480(language));
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
                SelectedSubfamily = null;
                ListSubfamily = new ObservableCollection<ConnectorSubFamily>(SCMService.GetAllSubfamilies_V2480(language));
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
                ListColor = new ObservableCollection<Data.Common.SCM.Color>(SCMService.GetAllColors(language));
                Data.Common.SCM.Color colorToRemove = ListColor.FirstOrDefault(x => x.Name == "None");
                if (colorToRemove != null)//[Sudhir.Jangra][GEOS2-4963]
                {
                    ListColor.Remove(colorToRemove);
                }
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
                ListShape = new ObservableCollection<LookupValue>(CRMService.GetLookupValues(113));
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
                ListGender = new ObservableCollection<Gender>(SCMService.GetGender(language));
                SelectedGender = new Gender();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillGender()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillGender() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillComponentType()
        {
            try
            {
                ComponentTypeList = new ObservableCollection<ComponentType>(SCMService.GetAllComponentTypes());
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
                //[GEOS2-5160][rdixit][17.01.2024] //[rdixit][14.10.2025][GEOS2-8895]
                SCMCommon.Instance.IsPinned = false;
                SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>();
                IsAcceptButtonEnable = false;
                IsAddEnable = false;
                sealingflag = false;
                SealingDefaultValue = false;
                UnSealedValue = false;
                sealingflag = true;
                IsInternalEnable = true;
                if (SelectedFamily != null)
                    SelectedFamily = null;
                SelectedSubfamily = null;
                SelectedColor = null;
                SelectedGender = null;
                SealingDefaultValue = false;
                UnSealedValue = false;
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
                NewListSubfamily = new ObservableCollection<ConnectorSubFamily>();
                ConnectorSearchList = new ObservableCollection<ConnectorSearch>();
                CustomListByIdFamily = new ObservableCollection<ConnectorProperties>(SCMService.GetAllConnectorCustomProperties_V2560());            
                GeosApplication.Instance.Logger.Log("Method ClearCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ClearCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillAllColorSimilarity()
        {
            try
            {
                AllColorSimilarityList = SCMService.GetSimilarColorsByConfiguration();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillAllColorSimilarity()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillAllColorSimilarity() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchCommandAction(object obj)
        {
            try
            {
                if(!IsFamilySelected)
                { return; }

                #region [rani dhamankar][21-05-2025][GEOS2-8133]
                if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                    AllowPaging = Convert.ToBoolean(GeosApplication.Instance.UserSettings["AllowPaging"]);
                if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                    ResultPages = Convert.ToInt32(GeosApplication.Instance.UserSettings["ResultPages"]);
                else
                    ResultPages = 10;
                #endregion

                //[rdixit][14.05.2025][GEOS2-7936] //[GEOS2-8036][13.05.2025][rdixit]
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImageSize"))
                {
                    CardImageSize = Convert.ToDouble(GeosApplication.Instance.UserSettings["ImageSize"]);
                }
                else
                    CardImageSize = 200;
                if (SelectedFamily==null|| SelectedFamily?.Id < 1)
                {
                    CustomMessageBox.Show(Application.Current.Resources["SelectFamilySampleRegister"].ToString(), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    return;
                }
                else
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
                            DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;//rajashri GEOS2-5106
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }
                    #endregion
                    GeosApplication.Instance.Logger.Log("Method SearchCommandAction ...", category: Category.Info, priority: Priority.Low);
                    string CustomString = string.Empty;
                    string TempCustomString = string.Empty;
                    IsSave = false;

                    //[rdixit][14.10.2025][GEOS2-8895]
                    SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>();
                    StaticConnectorsList = new List<Connectors>();
                    ConnectorsList = new List<Connectors>();
                    NoSearchfilterForFamily();
                    #region Old Logic
                    ////[GEOS2-6544][rdixit][25.09.2025]

                    //if (ConnectorsList == null)
                    //    ConnectorsList = new List<Connectors>();

                    //if (ConnectorSearchList == null)
                    //    ConnectorSearchList = new ObservableCollection<ConnectorSearch>();

                    //RunSearchAsync();


                    //
                    ////if (ConfigurationList != null)
                    ////{
                    ////    if (ConfigurationList.Any(k => k.IdFamily == SelectedFamily.Id))
                    ////    {
                    ////        var searchConfig = ConfigurationList.FirstOrDefault(k => k.IdFamily == SelectedFamily.Id);
                    ////        #region Config
                    ////        if (searchConfig != null)
                    ////        {
                    ////            for (int page = 1; page <= searchConfig.NoOfPages; page++)
                    ////            {
                    ////                //[GEOS2-6857][21.01.2025][rdixit]
                    ////                try
                    ////                {
                    ////                    Connectors Connectors = new Connectors();
                    ////                    Connectors.SelectedSubfamily = SelectedSubfamily;
                    ////                    Connectors.SelectedFamily = SelectedFamily;
                    ////                    Connectors.SelectedColorList = new List<Data.Common.SCM.Color>();
                    ////                    if (SelectedColor != null)
                    ////                    {
                    ////                        //Added color similarities for both A and B color
                    ////                        searchConfig.ColorSimilarityList = AllColorSimilarityList?.Where(i => i.IdColorA == SelectedColor.Id || i.IdColorB == SelectedColor.Id)?.ToList();
                    ////                        if (searchConfig.ColorSimilarityList != null && searchConfig.ColorPages.Split(',').ToList().Select(k => Convert.ToInt32(k)).Any(p => p == page))
                    ////                        {
                    ////                            Connectors.SelectedColorList.Add(ListColor.FirstOrDefault(p => p.Id == SelectedColor.Id));
                    ////                            foreach (var item in searchConfig.ColorSimilarityList)
                    ////                            {
                    ////                                Connectors.SelectedColorList.Add(ListColor.FirstOrDefault(p => p.Id == item.IdColorB));
                    ////                            }
                    ////                        }
                    ////                        else
                    ////                            Connectors.SelectedColorList.Add(SelectedColor);
                    ////                    }
                    ////                    Connectors.SelectedGender = SelectedGender;
                    ////                    Connectors.SelectedShape = SelectedShape;
                    ////                    List<int> test = searchConfig.WaysPages.Split(',').ToList().Select(k => Convert.ToInt32(k)).ToList();
                    ////                    if (Ways != null && !string.IsNullOrEmpty(Ways.DefaultValue) && test.Any(p => p == page))
                    ////                    {
                    ////                        Connectors.WaysProp = new ConnectorProperties()
                    ////                        {
                    ////                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Ways.DefaultValue) - searchConfig.WayMargin)),
                    ////                            MaxValue = Convert.ToUInt32(Convert.ToInt32(Ways.DefaultValue) + searchConfig.WayMargin)
                    ////                        };
                    ////                    }
                    ////                    else if (Ways != null && !string.IsNullOrEmpty(Ways.DefaultValue))
                    ////                    {
                    ////                        Connectors.WaysProp = new ConnectorProperties()
                    ////                        {
                    ////                            MinValue = Convert.ToUInt32(Ways.DefaultValue),
                    ////                            MaxValue = Convert.ToUInt32(Ways.DefaultValue)
                    ////                        };
                    ////                    }

                    ////                    #region SIZE
                    ////                    if (searchConfig.SizePages.Split(',').ToList().Select(k => Convert.ToInt32(k)).Any(p => p == page))
                    ////                    {
                    ////                        if (DiameterInternal != null && !string.IsNullOrEmpty(DiameterInternal.DefaultValue))
                    ////                        {
                    ////                            Connectors.DiameterInternalProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(DiameterInternal.DefaultValue) - searchConfig.Internal)),
                    ////                                MaxValue = Convert.ToUInt32(Convert.ToInt32(DiameterInternal.DefaultValue) + searchConfig.Internal)
                    ////                            };
                    ////                        }
                    ////                        else if (DiameterInternal != null && !string.IsNullOrEmpty(DiameterInternal.DefaultValue))
                    ////                        {
                    ////                            Connectors.WaysProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(DiameterInternal.DefaultValue),
                    ////                                MaxValue = Convert.ToUInt32(DiameterInternal.DefaultValue)
                    ////                            };
                    ////                        }


                    ////                        if (DiameterExternal != null && !string.IsNullOrEmpty(DiameterExternal.DefaultValue))
                    ////                        {
                    ////                            Connectors.DiameterExternalProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(DiameterExternal.DefaultValue) - searchConfig.External)),
                    ////                                MaxValue = Convert.ToUInt32(Convert.ToInt32(DiameterExternal.DefaultValue) + searchConfig.External)
                    ////                            };
                    ////                        }
                    ////                        else if (DiameterExternal != null && !string.IsNullOrEmpty(DiameterExternal.DefaultValue))
                    ////                        {
                    ////                            Connectors.DiameterExternalProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(DiameterExternal.DefaultValue),
                    ////                                MaxValue = Convert.ToUInt32(DiameterExternal.DefaultValue)
                    ////                            };
                    ////                        }

                    ////                        if (Height != null && !string.IsNullOrEmpty(Height.DefaultValue))
                    ////                        {
                    ////                            Connectors.HeightProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Height.DefaultValue) - searchConfig.Height)),
                    ////                                MaxValue = Convert.ToUInt32(Convert.ToInt32(Height.DefaultValue) + searchConfig.Height)
                    ////                            };
                    ////                        }
                    ////                        else if (Height != null && !string.IsNullOrEmpty(Height.DefaultValue))
                    ////                        {
                    ////                            Connectors.HeightProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Height.DefaultValue),
                    ////                                MaxValue = Convert.ToUInt32(Height.DefaultValue)
                    ////                            };
                    ////                        }

                    ////                        if (Length != null && !string.IsNullOrEmpty(Length.DefaultValue))
                    ////                        {
                    ////                            Connectors.LengthProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Length.DefaultValue) - searchConfig.Length)),
                    ////                                MaxValue = Convert.ToUInt32(Convert.ToInt32(Length.DefaultValue) + searchConfig.Length)
                    ////                            };
                    ////                        }
                    ////                        else if (Length != null && !string.IsNullOrEmpty(Length.DefaultValue))
                    ////                        {
                    ////                            Connectors.LengthProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Length.DefaultValue),
                    ////                                MaxValue = Convert.ToUInt32(Length.DefaultValue)
                    ////                            };
                    ////                        }

                    ////                        if (Width != null && !string.IsNullOrEmpty(Width.DefaultValue))
                    ////                        {
                    ////                            Connectors.WidthProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Width.DefaultValue) - searchConfig.Width)),
                    ////                                MaxValue = Convert.ToUInt32(Convert.ToInt32(Width.DefaultValue) + searchConfig.Width)
                    ////                            };
                    ////                        }
                    ////                        else if (Width != null && !string.IsNullOrEmpty(Width.DefaultValue))
                    ////                        {
                    ////                            Connectors.WidthProp = new ConnectorProperties()
                    ////                            {
                    ////                                MinValue = Convert.ToUInt32(Width.DefaultValue),
                    ////                                MaxValue = Convert.ToUInt32(Width.DefaultValue)
                    ////                            };
                    ////                        }
                    ////                    }
                    ////                    #endregion

                    ////                    if (SealingDefaultValue == false && UnSealedValue == false)
                    ////                        Connectors.SealingSearch = 2;
                    ////                    else
                    ////                        Connectors.SealingSearch = SealingDefaultValue == true ? 1 : 0;

                    ////                    ConnectorsList = SCMService.GetConnectorsBySearchConfiguration_V2560(Connectors);
                    ////                    List<int> comppage = searchConfig.CompPages?.Split(',').ToList().Select(k => Convert.ToInt32(k)).ToList();
                    ////                    if (comppage != null && searchConfig.ComponentsList != null && comppage.Any(p => p == page))
                    ////                        ConnectorsList = ComponentFilter1(searchConfig.ComponentsList);

                    ////                    if (ConnectorsList == null)
                    ////                        ConnectorsList = new List<Connectors>();
                    ////                    if (StaticConnectorsList == null)
                    ////                        StaticConnectorsList = new List<Connectors>();

                    ////                    if (ConnectorSearchList == null)
                    ////                        ConnectorSearchList = new ObservableCollection<ConnectorSearch>();

                    ////                    var NewConnectorList = new ObservableCollection<Connectors>(ConnectorsList?.Where(i =>
                    ////                    ConnectorSearchList?.All(k => k.ConnectorList?.All(j => j.IdConnector != i.IdConnector) ?? true) ?? true
                    ////                    ) ?? Enumerable.Empty<Connectors>());

                    ////                    StaticConnectorsList.AddRange(NewConnectorList);
                    ////                    #region Code commented to impliment paging concept
                    ////                    //[rdixit][14.04.2025][GEOS2-6631]
                    ////                    //[rdixit][13.05.2025][GEOS2-7936]
                    ////                    //var newList = ApplyMyPreferences(NewConnectorList?.ToList());
                    ////                    //foreach (var item in newList)
                    ////                    //{
                    ////                    //    ConnectorSearchList.Add(item);
                    ////                    //}
                    ////                    //ConnectorSearchList = new ObservableCollection<ConnectorSearch>(NewConnectorList);
                    ////                    #endregion

                    ////                    bool isThumbnails = false;
                    ////                    if (GeosApplication.Instance.UserSettings.ContainsKey("DefaultView"))
                    ////                        isThumbnails = (GeosApplication.Instance.UserSettings["DefaultView"] == "Thumbnails");

                    ////                    ConnectorSearch conSearch = new ConnectorSearch()
                    ////                    {
                    ////                        Header = $"Result Search [{NewConnectorList?.Count}]",
                    ////                        ConnectorList = NewConnectorList,
                    ////                        IsCardView = isThumbnails ? Visibility.Visible : Visibility.Collapsed,
                    ////                        IsTableView = isThumbnails ? Visibility.Collapsed : Visibility.Visible
                    ////                    };

                    ////                    ConnectorSearchList.Add(conSearch);
                    ////                    //LoadMoreConnectorsAsync(conSearch, Connectors).ConfigureAwait(false); //[rdixit][GEOS2-8133][26.05.2025]
                    ////                    SelectedSearchIndex = ConnectorSearchList.Count - 1; //[GEOS2-5155][rdixit][11.01.2024]
                    ////                }
                    ////                catch (Exception ex)
                    ////                {
                    ////                    GeosApplication.Instance.Logger.Log("Get an error in SearchCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    ////                }
                    ////            }
                    ////        }
                    ////        #endregion
                    ////    }
                    ////}
                    //
                    #endregion
                    if (ConnectorSearchList != null)//[rdixit][GEOS2-9013][10.09.2025]                 
                        IsAddEnable = true;                                   
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SearchCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }

        //[GEOS2-6544][rdixit][25.09.2025]
        public async Task RunSearchAsync()
        {
            if (SelectedColor != null)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method RunSearchAsync Color Similarity()...", category: Category.Info, priority: Priority.Low);

                    Connectors connectors = CreateConnectorsWithBasicProperties();
                    connectors = SetWaysDiameterSizeDefaultValues(connectors);

                    // Added color similarities for both A and B color
                    var colorSimilarityList = AllColorSimilarityList?
                        .Where(i => i.IdColorA == SelectedColor.Id || i.IdColorB == SelectedColor.Id)
                        .ToList();

                    connectors.SelectedColorList = new List<Color>();

                    if (colorSimilarityList != null && colorSimilarityList.Any())
                    {
                        var colorIds = new HashSet<int> { SelectedColor.Id };
                        foreach (var item in colorSimilarityList)
                        {
                            colorIds.Add(item.IdColorB);
                        }

                        connectors.SelectedColorList = ListColor.Where(p => colorIds.Contains(p.Id)).ToList();
                    }
                    else
                    {
                        connectors.SelectedColorList.Add(SelectedColor);
                    }

                    // Exclude SelectedColor itself
                    connectors.SelectedColorList = connectors.SelectedColorList?.Where(j => j.Id != SelectedColor.Id).ToList();

                    List<Connectors> similarityConnectorsList = await Task.Run(() => SCMService1.GetConnectorsBySearchConfiguration_V2560(connectors));

                    GenerateTab(similarityConnectorsList, Application.Current.FindResource("SCMAppearance").ToString());
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in RunSearchAsync Color similarity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            var searchConfig = ConfigurationList?.FirstOrDefault(k => k.IdFamily == SelectedFamily.Id);

            if (searchConfig != null)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method RunSearchAsync Size Similarity()...", category: Category.Info, priority: Priority.Low);
                    Connectors connectors = CreateConnectorsWithBasicProperties();
                    connectors.SelectedColorList = new List<Color>() { SelectedColor };

                    #region Ways
                    if (Ways != null && !string.IsNullOrEmpty(Ways.DefaultValue))
                    {
                        connectors.WaysProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Ways.DefaultValue) - searchConfig.WayMargin)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(Ways.DefaultValue) + searchConfig.WayMargin)
                        };
                    }
                    #endregion

                    #region SIZE
                    if (DiameterInternal != null && !string.IsNullOrEmpty(DiameterInternal.DefaultValue))
                    {
                        connectors.DiameterInternalProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(DiameterInternal.DefaultValue) - searchConfig.Internal)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(DiameterInternal.DefaultValue) + searchConfig.Internal)
                        };
                    }

                    if (DiameterExternal != null && !string.IsNullOrEmpty(DiameterExternal.DefaultValue))
                    {
                        connectors.DiameterExternalProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(DiameterExternal.DefaultValue) - searchConfig.External)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(DiameterExternal.DefaultValue) + searchConfig.External)
                        };
                    }

                    if (Height != null && !string.IsNullOrEmpty(Height.DefaultValue))
                    {
                        connectors.HeightProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Height.DefaultValue) - searchConfig.Height)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(Height.DefaultValue) + searchConfig.Height)
                        };
                    }

                    if (Length != null && !string.IsNullOrEmpty(Length.DefaultValue))
                    {
                        connectors.LengthProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Length.DefaultValue) - searchConfig.Length)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(Length.DefaultValue) + searchConfig.Length)
                        };
                    }

                    if (Width != null && !string.IsNullOrEmpty(Width.DefaultValue))
                    {
                        connectors.WidthProp = new ConnectorProperties()
                        {
                            MinValue = Convert.ToUInt32(Math.Max(0, Convert.ToInt32(Width.DefaultValue) - searchConfig.Width)),
                            MaxValue = Convert.ToUInt32(Convert.ToInt32(Width.DefaultValue) + searchConfig.Width)
                        };
                    }
                    #endregion

                    List<Connectors> similarityConnectorsList = await Task.Run(() => SCMService2.GetConnectorsBySearchConfiguration_V2560(connectors));

                    GenerateTab(similarityConnectorsList, Application.Current.FindResource("SCMSearchWaysMargin").ToString());
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in RunSearchAsync Size similarity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }

            if (searchConfig != null)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method RunSearchAsync Component Similarity()...", category: Category.Info, priority: Priority.Low);
                    Connectors connectors = CreateConnectorsWithBasicProperties();
                    connectors = SetWaysDiameterSizeDefaultValues(connectors);
                    connectors.SelectedColorList = new List<Color>() { SelectedColor };

                    if (searchConfig.ComponentsList != null)
                    {
                        connectors.Componentlist = new List<Components>();
                        foreach (var item in searchConfig.ComponentsList)
                        {
                            var color = ListColor.FirstOrDefault(i => i.Id == item.IdColor);
                            var type = ComponentTypeList.FirstOrDefault(i => i.IdType == item.IdType);

                            connectors.Componentlist.Add(new Components()
                            {
                                SelectedColor = color?.Name,
                                Reference = item.Reference,
                                SelectedComponentType = type?.Name,
                                SelectedCondition = item.SelectedCondition?.ToLower() == "is" ? "Present" : "Not Present"
                            });
                        }

                        List<Connectors> similarityConnectorsList =
                            await Task.Run(() => SCMService3.GetConnectorsBySearchConfiguration_V2560(connectors));

                        similarityConnectorsList = ComponentFilter(similarityConnectorsList, connectors.Componentlist);
                        GenerateTab(similarityConnectorsList, Application.Current.FindResource("Components").ToString());
                    }
                    else
                    {
                        GenerateTab(new List<Connectors>(), Application.Current.FindResource("Components").ToString());
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in RunSearchAsync Component similarity() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        //[GEOS2-6544][rdixit][25.09.2025]
        private Connectors CreateConnectorsWithBasicProperties()
        {
            Connectors con = new Connectors();

            try
            {
                con.SelectedSubfamily = SelectedSubfamily;
                con.SelectedFamily = SelectedFamily;
                con.SelectedGender = SelectedGender;
                con.SelectedShape = SelectedShape;
                if (!SealingDefaultValue && !UnSealedValue)
                {
                    con.SealingSearch = 2;
                }
                else if (SealingDefaultValue)
                {
                    con.SealingSearch = 1;
                }
                else
                {
                    con.SealingSearch = 0;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreateConnectorsWithBasicProperties() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return con;
        }

        //[GEOS2-6544][rdixit][25.09.2025]
        private Connectors SetWaysDiameterSizeDefaultValues(Connectors Connectors)
        {
            try
            {
                if (Ways != null && !string.IsNullOrEmpty(Ways.DefaultValue))
                {
                    Connectors.WaysProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(Ways.DefaultValue),
                        MaxValue = Convert.ToUInt32(Ways.DefaultValue)
                    };
                }
                if (DiameterInternal != null && !string.IsNullOrEmpty(DiameterInternal.DefaultValue))
                {
                    Connectors.DiameterInternalProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(DiameterInternal.DefaultValue),
                        MaxValue = Convert.ToUInt32(DiameterInternal.DefaultValue)
                    };
                }
                if (DiameterExternal != null && !string.IsNullOrEmpty(DiameterExternal.DefaultValue))
                {
                    Connectors.DiameterExternalProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(DiameterExternal.DefaultValue),
                        MaxValue = Convert.ToUInt32(DiameterExternal.DefaultValue)
                    };
                }
                if (Height != null && !string.IsNullOrEmpty(Height.DefaultValue))
                {
                    Connectors.HeightProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(Height.DefaultValue),
                        MaxValue = Convert.ToUInt32(Height.DefaultValue)
                    };
                }
                if (Length != null && !string.IsNullOrEmpty(Length.DefaultValue))
                {
                    Connectors.LengthProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(Length.DefaultValue),
                        MaxValue = Convert.ToUInt32(Length.DefaultValue)
                    };
                }
                if (Width != null && !string.IsNullOrEmpty(Width.DefaultValue))
                {
                    Connectors.WidthProp = new ConnectorProperties()
                    {
                        MinValue = Convert.ToUInt32(Width.DefaultValue),
                        MaxValue = Convert.ToUInt32(Width.DefaultValue)
                    };
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetWaysDiameterSizeDefaultValues() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return Connectors;
        }

        //[GEOS2-6544][rdixit][25.09.2025]
        void GenerateTab(List<Connectors> similarityConnectorsList, string header_name)
        {
            try
            {

                //[rdixit][14.10.2025][GEOS2-8895]
                ObservableCollection<Connectors> NewConnectorList = new ObservableCollection<Connectors>(similarityConnectorsList?
                    .Where(i => StaticConnectorsList?.All(j => j.IdConnector != i.IdConnector) ?? true) ?? new List<Connectors>());

                StaticConnectorsList.AddRange(NewConnectorList);


                var normalGroup = new SearchConnector
                {
                    Header = "",
                    ConnectorList = new ObservableCollection<Connectors>(NewConnectorList),
                    IsTableView = GeosApplication.Instance.UserSettings["DefaultView"] != "Thumbnails"
                };
                var ResultTab = ViewModelSource.Create(() => new ConnectorsListViewModel
                {
                    TabName = $"{header_name} [{NewConnectorList.Count}]",
                    ParentViewModel = this
                });
                ResultTab.ConnectorSearchList.Add(normalGroup);
                ResultTab.LoadImageAsync();
                SCMCommon.Instance.Tabs.Add(ResultTab);
                SCMCommon.Instance.IsPinned = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GenerateTab() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
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
        private void DeleteComponentFilterItem(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteComponentFilterItem()...", category: Category.Info, priority: Priority.Low);

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                Components testselection = (Components)gridControl.SelectedItem;
                Componentlist.Remove(testselection);
                Componentlist = new ObservableCollection<Components>(Componentlist.ToList());
                SelectedComponent = Componentlist.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method DeleteComponentFilterItem()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteComponentFilterItem() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteComponentFilterItem() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteComponentFilterItem()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GridViewAction(object obj) //[rdixit][14.10.2025][GEOS2-8895]
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction ...", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method ShowGridViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowGridViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CardViewAction(object obj) //[rdixit][14.10.2025][GEOS2-8895]
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewAction ...", category: Category.Info, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log("Method ShowSchedulerViewAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSchedulerViewAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillAllFamilySearchConfigurations()
        {
            try
            {
                ConfigurationList = new ObservableCollection<ConfigurationFamily>(SCMService.GetConfigurationsForSearchFilters_V2490());
                GeosApplication.Instance.Logger.Log(string.Format("Method FillAllFamilySearchConfigurations()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillAllFamilySearchConfigurations() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenFamilyImagesAction(object obj)
        {
            try
            {
                if (SelectedFamily != null)
                {
                    Family Fam = SelectedFamily;
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
                if (SelectedSubfamily != null)
                {
                    if (SelectedSubfamily.Id > 0)
                    {
                        GeosApplication.Instance.Logger.Log("Method OpenSubFamilyImagesAction()...", category: Category.Info, priority: Priority.Low);
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        FamilyAndSubFamilyImagesView FamilyAndSubFamilyImagesView = new FamilyAndSubFamilyImagesView();
                        FamilyAndSubFamilyImagesViewModel FamilyAndSubFamilyImagesViewModel = new FamilyAndSubFamilyImagesViewModel();
                        EventHandler handle = delegate { FamilyAndSubFamilyImagesView.Close(); };
                        FamilyAndSubFamilyImagesViewModel.RequestClose += handle;
                        FamilyAndSubFamilyImagesViewModel.FamilyInit(SelectedSubfamily);
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
        private void FamilyPopupClosedAction(object obj)
        {
            try
            {
                if (PrevProperties == null)
                    PrevProperties = new List<ConnectorProperties>();
                SelectedColor = null;
                SelectedGender = null;
                SealingDefaultValue = false;
                UnSealedValue = false;
                if (SelectedFamily != null)
                {
                    sealingflag = true;

                    Family selectedfam = SelectedFamily;
                    #region Single family selected [rdixit][05.03.2024][GEOS2-5295]
                    PropertiesForSelectedFamily = SCMService.GetPropertyManager_V2490()?.Where(i => i.IdFamily == selectedfam.Id).ToList();

                    #region 1 is for color               
                    ColorProp = PropertiesForSelectedFamily.FirstOrDefault(i => i.IdConnectorProperty == 1);
                    if (ColorProp != null && ListColor != null && !string.IsNullOrEmpty(ColorProp.DefaultValue))
                        SelectedColor = ListColor.FirstOrDefault(i => i.Id == Convert.ToInt32(ColorProp.DefaultValue));

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
                            SelectedGender = ListGender.FirstOrDefault(i => i.Id == Convert.ToInt32(GenderProp.DefaultValue));
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
                    NewListSubfamily = new ObservableCollection<ConnectorSubFamily>(ListSubfamily.Where(w => w.IdFamily == selectedfam.Id).ToList());
                    IsSearchButtonVisible(null);
               
                    CustomListByIdFamily = new ObservableCollection<ConnectorProperties>(SCMService.GetConnectorCustomPropertiesByFamily_V2500(Convert.ToUInt32(SelectedFamily.Id)));
                    FillCustomecontrolsource();                  
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FamilyPopupClosedAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void FillCustomecontrolsource()
        {
            if (CustomListByIdFamily != null)
            {
                List<int> idfamily = new List<int>();
                List<int> idLookupKey = new List<int>();
                foreach (var item in CustomListByIdFamily)
                {
                    if (item.IdConnectorType == 1826)
                    {
                        idfamily.Add(item.IdConnectorProperty);
                    }
                }

                foreach (var item in idfamily)
                {
                    ValueKey = new ObservableCollection<ValueKey>(SCMService.GetValueKeyOfCustomProperties_V2480(item));
                    int lookUpKey = ValueKey.Select(i => i.IdLookupKey).FirstOrDefault();
                    CustomListByIdFamily.Where(x => x.IdConnectorProperty == item).ToList()
                        .ForEach(a => a.CustomFieldComboboxList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(lookUpKey)));
                    CustomListByIdFamily.Where(x => x.IdConnectorProperty == item).ToList()
                     .ForEach(a => a.SelectedcustomField = a.CustomFieldComboboxList.FirstOrDefault(c => c.Value_en == CustomListByIdFamily.Where(x => x.IdConnectorProperty == item).FirstOrDefault().DefaultValue));
                }
            }
        }
        private void FillCustomData()
        {
            try
            {
                CustomList = new List<CustomProperty>(SCMService.GetAllCustomeData());
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomData()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCustomData() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        void NoSearchfilterForFamily()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method NoSearchfilterForFamily() ....", category: Category.Info, priority: Priority.Low);
                Connectors Connectors = new Connectors();
                Connectors.SelectedSubfamily = SelectedSubfamily;
                Connectors.SelectedFamily = SelectedFamily;
                Connectors.SelectedColor = SelectedColor;
                Connectors.SelectedGender = SelectedGender;
                Connectors.SelectedShape = SelectedShape;
                //[rdixit][14.04.2025][GEOS2-6631]
                if (Ways != null)
                {
                    if (Ways.DefaultValue?.ToString() == "true" || Ways.DefaultValue?.ToString() == "false")
                        Ways.DefaultValue = "0";
                    Connectors.Ways = Ways.DefaultValue;
                }
                if (DiameterInternal != null)
                    Connectors.DiameterInternal = DiameterInternal.DefaultValue;
                if (DiameterExternal != null)
                    Connectors.DiameterExternal = DiameterExternal.DefaultValue;
                if (Height != null)
                    Connectors.Height = Height.DefaultValue;
                if (Length != null)
                    Connectors.Length = Length.DefaultValue;
                if (Width != null)
                    Connectors.Width = Width.DefaultValue;
                if (SealingDefaultValue == false && UnSealedValue == false)
                    Connectors.SealingSearch = 2;
                else
                    Connectors.SealingSearch = SealingDefaultValue == true ? 1 : 0;

                ConnectorsList = SCMService.GetSampleRegistrationAllConnectors_V2560(Connectors);
                
                ConnectorsList = ComponentFilter(ConnectorsList, Componentlist?.ToList());

                if (ConnectorsList == null)
                    ConnectorsList = new List<Connectors>();
                if (StaticConnectorsList == null)
                    StaticConnectorsList = new List<Connectors>();

                StaticConnectorsList.AddRange(ConnectorsList);


                //[rdixit][14.10.2025][GEOS2-8895]
                // ----- 1) Normal search: create VM + placeholder tab immediately -----
                vm = ViewModelSource.Create(() => new ConnectorsListViewModel
                {
                    TabName = Application.Current.FindResource("ExactMatch").ToString() + " [" + ConnectorsList.Count + "]",
                    ParentViewModel = this
                });

                var normalGroup = new SearchConnector
                {
                    Header = "",
                    ConnectorList = new ObservableCollection<Connectors>(ConnectorsList),
                    IsTableView = GeosApplication.Instance.UserSettings["DefaultView"] != "Thumbnails"
                };

                vm.ConnectorSearchList.Add(normalGroup);
                vm.LoadImageAsync();
                SCMCommon.Instance.Tabs.Add(vm);
                SCMCommon.Instance.IsPinned = false;
                SCMCommon.Instance.TabIndex = 0;

                RunSearchAsync();

                if (SCMCommon.Instance.Tabs?.Count > 0)//[rdixit][GEOS2-9013][10.09.2025]                 
                    IsAddEnable = true;

                #region
                //if (ConnectorSearchList == null)
                //    ConnectorSearchList = new ObservableCollection<ConnectorSearch>();

                //bool isThumbnails = false;
                //if (GeosApplication.Instance.UserSettings.ContainsKey("DefaultView"))
                //    isThumbnails = (GeosApplication.Instance.UserSettings["DefaultView"] == "Thumbnails");

                ////[GEOS2-6544][rdixit][25.09.2025]
                //ConnectorSearch conSearch = new ConnectorSearch()
                //{
                //    Header = Application.Current.FindResource("ExactMatch").ToString() + " [" + ConnectorsList?.Count + "]",
                //    ConnectorList = new ObservableCollection<Connectors>(ConnectorsList),
                //    IsCardView = isThumbnails ? Visibility.Visible : Visibility.Collapsed,
                //    IsTableView = isThumbnails ? Visibility.Collapsed : Visibility.Visible
                //};
                //ConnectorSearchList.Add(conSearch);
                //ConnectorSearchList = new ObservableCollection<ConnectorSearch>(ConnectorSearchList);
                ////LoadMoreConnectorsAsync(conSearch, Connectors).ConfigureAwait(false);
                //SelectedSearchIndex = ConnectorSearchList.Count - 1; //[GEOS2-5155][rdixit][11.01.2024]
                #endregion

                GeosApplication.Instance.Logger.Log("Method NoSearchfilterForFamily() executed successfully", category: Category.Info, priority: Priority.Low);            
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method NoSearchfilterForFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ConnectorWorkflow()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConnectorWorkflow()...", category: Category.Info, priority: Priority.Low);
                StatusList = new List<ConnectorWorkflowStatus>(SCMService.GetAllConnectorStatus());
                WorkflowTransitionList = new List<ConnectorWorkflowTransitions>(SCMService.GetAllWorkflowTransitions());            
                GeosApplication.Instance.Logger.Log("Method ConnectorWorkflow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorWorkflow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorWorkflow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorWorkflow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        void FillDrawingColumns(ObservableCollection<ScmDrawing> DrawingList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillDrawingColumns().... "), category: Category.Info, priority: Priority.Low);
                Columns = new ObservableCollection<Column>();
                DtDrawingCopy = new DataTable();
                Column c = new Column();

                Columns.Add(new Column() { FieldName = "IdDrawing", HeaderText = "IdDrawing", Settings = SettingsType.IdDrawing, AllowCellMerge = false, Width = 70, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true });
                DtDrawingCopy.Columns.Add("IdDrawing", typeof(int));

                #region Detections
                List<string> Detections = new List<string>();
                Detections = DrawingList.Where(item => item?.DetectionList != null).SelectMany(item => item.DetectionList.Select(det => det.Name?.ToLower())).Distinct().ToList();
                foreach (var item in Detections)
                {
                    c = new Column() { FieldName = item, HeaderText = item, Settings = SettingsType.Default, AllowCellMerge = false, Width = 55, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true };
                    Columns.Add(c);
                    DtDrawingCopy.Columns.Add(item, typeof(int));
                }
                #endregion

                #region CpType&Template Columns [rdixit][29.08.2024][Beta changes]
                List<Tuple<byte, string>> CptypeColumns = DrawingList?.Where(i => i.CptypeName != null)?
                            .Select(i => Tuple.Create(i.IdCPType, i.CptypeName))?.Distinct()?.ToList();

                List<Tuple<byte, string>> TemplateColumns = DrawingList?.Where(i => i.TemplateName != null)?
                   .Select(i => Tuple.Create(i.IdTemplate, i.TemplateName))?.Distinct()?.ToList();

                if (CptypeColumns?.Count > 0)
                {
                    foreach (var item in CptypeColumns)
                    {
                        c = new Column() { FieldName = item.Item2, HeaderText = item.Item2, Settings = SettingsType.Name, AllowCellMerge = false, Width = 45, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true };
                        Columns.Add(c);
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                    }
                }
                if (TemplateColumns?.Count > 0)
                {
                    foreach (var item in TemplateColumns)
                    {
                        c = new Column() { FieldName = item.Item2, HeaderText = item.Item2, Settings = SettingsType.Default, AllowCellMerge = false, Width = 40, AllowEditing = false, Visible = true, IsVertical = true, FixedWidth = true };
                        Columns.Add(c);
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                    }
                }
                #endregion

                Columns.Add(new Column() { FieldName = "Comments", HeaderText = "Comments", Width = 200, IsVertical = true, Settings = SettingsType.Default, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });
                Columns.Add(new Column() { FieldName = "Path", HeaderText = "Path", Width = 45, IsVertical = true, Settings = SettingsType.Image, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });
                Columns.Add(new Column() { FieldName = "Site", HeaderText = "Site", Width = 200, IsVertical = true, Settings = SettingsType.Default, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });
                Columns.Add(new Column() { FieldName = "Created By", HeaderText = "Created By", Width = 300, IsVertical = true, Settings = SettingsType.Default, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });
                Columns.Add(new Column() { FieldName = "Modified By", HeaderText = "Modified By", Width = 300, IsVertical = true, Settings = SettingsType.Default, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });
                Columns.Add(new Column() { FieldName = "Debugged", HeaderText = "Debugged", Width = 20, IsVertical = true, Settings = SettingsType.IsChecked, AllowCellMerge = false, AllowEditing = false, Visible = true, FixedWidth = true });

                DtDrawingCopy.Columns.Add("Comments", typeof(string));
                DtDrawingCopy.Columns.Add("Path", typeof(string));
                DtDrawingCopy.Columns.Add("Site", typeof(string));
                DtDrawingCopy.Columns.Add("Created By", typeof(string));
                DtDrawingCopy.Columns.Add("Modified By", typeof(string));
                DtDrawingCopy.Columns.Add("Debugged", typeof(bool));
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDrawingColumns() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log(string.Format("Method FillDrawingColumns().... Executed"), category: Category.Info, priority: Priority.Low);
        }
        void FillDrawingData(ObservableCollection<ScmDrawing> DrawingList)
        {
            for (int i = 0; i < DrawingList.Count; i++)
            {
                try
                {
                    DataRow dr = DtDrawingCopy.NewRow();
                    dr["IdDrawing"] = DrawingList[i].IdDrawing;
                    dr["Comments"] = DrawingList[i].Comments;
                    dr["Path"] = "W:" + DrawingList[i].Path;
                    dr["Site"] = DrawingList[i].SiteName;
                    dr["Created By"] = DrawingList[i].CreatedBy + " (" + DrawingList[i].SiteNameC + ")" + DrawingList[i].CreatedIn?.ToShortDateString();
                    dr["Modified By"] = DrawingList[i].ModifiedBy + " (" + DrawingList[i].SiteNameM + ")" + DrawingList[i].ModifiedIn?.ToShortDateString();
                    dr["Debugged"] = DrawingList[i].Debugged;
                    dr[DrawingList[i].CptypeName] = "X";
                    dr[DrawingList[i].TemplateName] = "X";
                    if (DrawingList[i].DetectionList != null)
                    {
                        foreach (var item in DrawingList[i].DetectionList)
                        {
                            dr[item.Name] = item.Quantity;
                        }
                    }
                    DtDrawingCopy.Rows.Add(dr);
                }
                catch (Exception ex)
                {

                }
            }
            DtDrawing = DtDrawingCopy;
        }
        public List<Connectors> ComponentFilter(List<Connectors> similarityConnectorsList, List<Components> componentlist)
        {
            List<Connectors> FilterList = new List<Connectors>(similarityConnectorsList);
            try
            {
                if (componentlist?.Count > 0)
                {
                    var CompairConnectorsList = similarityConnectorsList.ToList();

                    foreach (var item in componentlist)
                    {
                        if (item.SelectedCondition == "Present")
                        {
                            #region IN
                            if ((!string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) && (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // All fields are not empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p =>
                                        p.SelectedColor == item.SelectedColor && p.Reference == item.Reference && p.SelectedComponentType == item.SelectedComponentType);
                                    }
                                    else return false;
                                }));
                            }
                            else if ((!string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                (string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor and SelectedComponentType are not empty, but Reference is empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p =>
                                            p.SelectedColor == item.SelectedColor && p.SelectedComponentType == item.SelectedComponentType
                                        );
                                    }
                                    else return false;
                                }));
                            }

                            else if ((string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                     (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor is empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p =>
                                        p.Reference == item.Reference && p.SelectedComponentType == item.SelectedComponentType);
                                    }
                                    else return false;
                                }));
                            }

                            else if ((!string.IsNullOrEmpty(item.SelectedColor)) && (string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                    (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedComponentType is empty                                                    
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p =>
                                        p.Reference == item.Reference && p.SelectedColor == item.SelectedColor);
                                    }
                                    else return false;
                                }));
                            }

                            else if ((string.IsNullOrEmpty(item.SelectedColor)) && (string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                     (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor and SelectedComponentType are empty                                                
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p => p.Reference == item.Reference);
                                    }
                                    else return false;
                                }));
                            }

                            else if (!string.IsNullOrEmpty(item.SelectedColor) && string.IsNullOrEmpty(item.SelectedComponentType)
                                && string.IsNullOrEmpty(item.Reference))
                            {
                                // Only SelectedColor is not empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p => p.SelectedColor == item.SelectedColor);
                                    }
                                    else return false;
                                }));
                            }
                            else if ((string.IsNullOrEmpty(item.SelectedColor)) &&
                               (!string.IsNullOrEmpty(item.SelectedComponentType)) && (string.IsNullOrEmpty(item.Reference)))
                            {
                                // Only SelectedComponentType is not empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return i.Componentlist.Any(p => p.SelectedComponentType == item.SelectedComponentType);
                                    }
                                    else return false;
                                }));
                            }
                            #endregion
                        }
                        else
                        {
                            #region NOT IN
                            if ((!string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) && (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // All fields are not empty                                 
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p =>
                                        p.SelectedColor == item.SelectedColor && p.Reference == item.Reference && p.SelectedComponentType == item.SelectedComponentType));
                                    }
                                    else return false;
                                }));
                            }
                            else if ((!string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                (string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor and SelectedComponentType are not empty, but Reference is empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p => p.SelectedColor == item.SelectedColor && p.SelectedComponentType == item.SelectedComponentType));
                                    }
                                    else return false;
                                }));
                            }

                            else if ((string.IsNullOrEmpty(item.SelectedColor)) && (!string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                     (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor is empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p =>
                                        p.Reference == item.Reference && p.SelectedComponentType == item.SelectedComponentType));
                                    }
                                    else return false;
                                }));
                            }

                            else if ((!string.IsNullOrEmpty(item.SelectedColor)) && (string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                    (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedComponentType is empty                                                    
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p =>
                                        p.Reference == item.Reference && p.SelectedColor == item.SelectedColor));
                                    }
                                    else return false;
                                }));
                            }

                            else if ((string.IsNullOrEmpty(item.SelectedColor)) && (string.IsNullOrEmpty(item.SelectedComponentType)) &&
                                (!string.IsNullOrEmpty(item.Reference)))
                            {
                                // SelectedColor and SelectedComponentType are empty                                                
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p => p.Reference == item.Reference));
                                    }
                                    else return false;
                                }));
                            }

                            else if (!string.IsNullOrEmpty(item.SelectedColor) && string.IsNullOrEmpty(item.SelectedComponentType)
                                && string.IsNullOrEmpty(item.Reference))
                            {
                                // Only SelectedColor is not empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p => p.SelectedColor == item.SelectedColor));
                                    }
                                    else return false;
                                }));
                            }
                            else if ((string.IsNullOrEmpty(item.SelectedColor)) &&
                               (!string.IsNullOrEmpty(item.SelectedComponentType)) && (string.IsNullOrEmpty(item.Reference)))
                            {
                                // Only SelectedComponentType is not empty
                                FilterList = new List<Connectors>(FilterList.Where(i =>
                                {
                                    if (i.Componentlist != null)
                                    {
                                        return !(i.Componentlist.Any(p => p.SelectedComponentType == item.SelectedComponentType));
                                    }
                                    else return false;
                                }));
                            }
                            #endregion
                        }
                    }
                }
                else
                    FilterList = similarityConnectorsList.ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ComponentFilter() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return FilterList;
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
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
        #endregion
    }
}
