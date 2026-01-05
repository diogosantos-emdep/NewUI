using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
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
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = Emdep.Geos.Data.Common.SCM.Color;
using Column = Emdep.Geos.UI.Helper.Column;
using Company = Emdep.Geos.Data.Common.SCM.Company;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[GEOS2-9552][rdixit][19.09.2025]
    public class ConnectorDetailViewMode : ViewModelBase, INotifyPropertyChanged, ITabViewModel, IDisposable
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMServiceCompanyWise = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region public Events              
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events

        #region Fields     
        Connectors connector;
        Connectors originalconnector;
        private ConnectorLogEntry selectedComment;
        ObservableCollection<ValueKey> valueKey;
        byte[] userProfileImageByte = null;//[pramod.misal]
        private ImageSource userProfileImage;//[pramod.misal]
        ConnectorReference selectedReference;
        ObservableCollection<ConnectorSearch> connectorSearchList;
        ObservableCollection<ConnectorReference> deletedReferencesList;
        string header;
        ObservableCollection<ConnectorLogEntry> connectorChangeLogList;
        ObservableCollection<Connectors> connectorList;
        Visibility isTableView = Visibility.Visible;
        Visibility isCardView = Visibility.Collapsed;
        Visibility isEditConnectorView = Visibility.Collapsed;
        ObservableCollection<SCMConnectorImage> imagesList;
        string currentImageCount;
        string imageDescription;
        SCMConnectorImage selectedImage;
        string referenceStatus;
        string referenceStatusHtmlColor;
        private double dialogHeight;
        private double dialogWidth;
        private Int32 numWays;
        private string reference;
        private string description;
        private double internalDiameter;
        private double externalDiameter;
        private double height;
        private double length;
        private double width;
        private Family selectedFamily;
        private ConnectorSubFamily selectedSubFamily;
        Color selectedColor;
        Gender selectedGender;
        bool isSealed;
        bool isUnSealed;
        ObservableCollection<ConnectorSubFamily> listSubfamily;
        ObservableCollection<ConnectorReference> referencesList;
        private ObservableCollection<ConnectorProperties> customFieldsList;//[Sudhir.Jangra][GEOS2-5374]
        private ConnectorProperties selectedCustomField;//[Sudhir.Jangra][GEOS2-5374]
        ObservableCollection<ConnectorComponents> componentsList;
        string referenceCaption;
        string componentCaption;
        ObservableCollection<Connectors> linkedConnectorList;
        ObservableCollection<ConnectorAttachements> connectorAttachementFilesList;//[pramod.misal][GEOS2-5387][08-04-2024]
        ObservableCollection<ConnectorAttachements> connectorAttachementDeletedFilesList;
        private ConnectorAttachements selectedConnectorAttachementFiles;
        string linkedConnectorCaption;
        private ObservableCollection<SCMConnectorImage> connectorsImageList;//[Sudhir.Jangra][GEOS2-5384]
        private Visibility isColorVisible;
        private Visibility isWayVisible;
        private Visibility isGenderVisible;
        private Visibility isSealVisible;
        private Visibility isDiameterVisible;
        private Visibility isSizeVisible;
        private Visibility isExternalVisible;
        private Visibility isInternalVisible;
        private Visibility isHeightVisible;
        private Visibility isLengthVisible;
        private Visibility isWidthVisible;
        private bool isColorEnabled;
        private bool isWaysEnabled;
        private bool isGenderEnabled;
        private bool isSealingEnabled;
        private bool isInternalEnabled;
        private bool isExternalEnabled;
        private bool isLengthEnabled;
        private bool isHeightEnabled;
        private bool isWidthEnabled;
        private ObservableCollection<ConnectorLocation> locationList;
        private ObservableCollection<ConnectorLogEntry> connectorCommentsList;
        private string locationCaption;
        private string drawingCaption;
        ObservableCollection<ScmDrawing> connectorDrawingList;
        private ObservableCollection<ConnectorWorkflowStatus> referenceStatusList;
        private ConnectorWorkflowStatus selectedReferenceStatus;
        ObservableCollection<ConnectorWorkflowTransitions> workflowTransitionList;
        Int64 idConnector;
        List<ConnectorLogEntry> updatedChangeLogList;
        DateTime? modifiedIn;
        UInt32 modifiedBy;
        private List<ConnectorLogEntry> addCommentsList;//[pramod.Misal][GEOS2-][07-05-2024]
        private List<ConnectorLogEntry> updatedCommentList;//[pramod.Misal][GEOS2-][07-05-2024]
        private List<ConnectorLogEntry> deleteCommentsList;//[pramod.Misal][GEOS2-][07-05-2024]
        int idSite;
        ConnectorComponents selectedComponents;
        List<ConnectorComponents> deletedComponentList;
        List<Connectors> deletedLinkedConnectorList;
        Connectors selectedLink;
        private bool isDescriptionEnabled;
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        ConnectorLocation selecedLocation;
        SCMConnectorImage selectedConnectorImage;
        private List<ConnectorLocation> deletedLocationList;
        private List<SCMConnectorImage> deletedImageList;
        Visibility isCustomFieldsVisible;
        private string language;
        DataTable dtDrawing;
        DataTable dtDrawingCopy;
        private ObservableCollection<Column> columns;
        ObservableCollection<BandItem> bands;
        private SCMConnectorImage maximizedElement;
        bool isEnabledCancelButton;
        private ConnectorAttachements selectedConnectorFile;
        ObservableCollection<Family> listFamily;
        ObservableCollection<ConnectorSubFamily> newlistSubfamily;
        ObservableCollection<LookupValue> listShape;
        bool isReadOnly;
        bool isBasicConnectorEditReadOnly;
        bool isBasicConnectorEditEnabled;
        bool isEnabled;
        bool isUpdate;
        List<ConnectorWorkflowStatus> statusList;
        private List<ConnectorProperties> customePropertiesList;
        List<ConnectorWorkflowTransitions> connectorStatusTransition;
        List<ConnectorWorkflowStatus> connectorStatus;
        private ObservableCollection<ComponentType> componentTypeList;
        private ObservableCollection<Data.Common.SCM.Color> listColor;
        List<Family> allFamilyList;
        ObservableCollection<Gender> listGender;
        #endregion

        #region Properties  
        public bool IsUpdated
        {
            get { return isUpdate; }
            set
            {
                isUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
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
        public Connectors Connector
        {
            get { return connector; }
            set
            {
                connector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Connector"));
            }
        }
        public Connectors Originalconnector
        {
            get { return originalconnector; }
            set
            {
                originalconnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Originalconnector"));
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
        public byte[] UserProfileImageByte
        {
            get { return userProfileImageByte; }
            set
            {
                userProfileImageByte = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImageByte"));
            }
        }
        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
            }
        }       
        public List<Family> AllFamilyList
        {
            get { return allFamilyList; }
            set
            {
                allFamilyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGender"));
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
        public ObservableCollection<Color> ListColor
        {
            get { return listColor; }
            set { listColor = value; OnPropertyChanged(new PropertyChangedEventArgs("ListColor")); }
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
        public Visibility IsCustomFieldsVisible
        {
            get { return isCustomFieldsVisible; }
            set
            {
                isCustomFieldsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomFieldsVisible"));
            }
        }        
        public bool IsDescriptionEnabled
        {
            get { return isDescriptionEnabled; }
            set
            {
                isDescriptionEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDescriptionEnabled"));
            }
        }        
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }
        Visibility imageNameVisibility;
        public Visibility ImageNameVisibility
        {
            get
            {
                return imageNameVisibility;
            }
            set
            {
                imageNameVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageNameVisibility"));
            }
        }
        public string CurrentImageCount
        {
            get { return currentImageCount; }
            set
            {
                currentImageCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentImageCount"));
            }
        }          
        public SCMConnectorImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                if (SelectedImage != null)
                {
                    CurrentImageCount = SelectedImage?.Position + "/" + ImagesList?.Count;
                    ImageNameVisibility = Visibility.Visible;
                }              
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImage"));
            }
        }        
        public ObservableCollection<Connectors> ConnectorList
        {
            get
            {
                return connectorList;
            }

            set
            {
                connectorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorList"));
            }
        }        
        public ObservableCollection<SCMConnectorImage> ImagesList
        {
            get
            {
                return imagesList;
            }
            set
            {
                imagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagesList"));
            }
        }        
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }        
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }        
        public double InternalDiameter
        {
            get { return internalDiameter; }
            set
            {
                internalDiameter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InternalDiameter"));
            }
        }        
        public double ExternalDiameter
        {
            get { return externalDiameter; }
            set
            {
                externalDiameter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExternalDiameter"));
            }
        }        
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }        
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
            }
        }        
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }        
        public Family SelectedFamily
        {
            get { return selectedFamily; }
            set
            {
                selectedFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFamily"));
            }
        }        
        public Gender SelectedGender
        {
            get { return selectedGender; }
            set
            {
                selectedGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGender"));
            }
        }        
        public ConnectorSubFamily SelectedSubFamily
        {
            get { return selectedSubFamily; }
            set
            {
                selectedSubFamily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSubFamily"));
            }
        }        
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColor"));
            }
        }        
        public bool IsSealed
        {
            get
            {
                return isSealed;
            }
            set
            {
                isSealed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSealed"));
            }
        }        
        public bool IsUnSealed
        {
            get
            {
                return isUnSealed;
            }
            set
            {
                isUnSealed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUnSealed"));
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
        public Int32 NumWays
        {
            get
            {
                return numWays;
            }
            set
            {
                numWays = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NumWays"));
            }
        }        
        public ObservableCollection<ConnectorReference> ReferencesList
        {
            get { return referencesList; }
            set
            {
                referencesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferencesList"));
            }
        }        
        public ConnectorReference SelectedReference
        {
            get { return selectedReference; }
            set
            {
                selectedReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReference"));
            }
        }        
        public ObservableCollection<ConnectorReference> DeletedReferencesList
        {
            get { return deletedReferencesList; }
            set
            {
                deletedReferencesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedReferencesList"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5374]        
        public ObservableCollection<ConnectorProperties> CustomFieldsList
        {
            get { return customFieldsList; }
            set
            {
                customFieldsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomFieldsList"));
            }
        }
        //[Sudhir.Jangra][GEOS2-5374]        
        public ConnectorProperties SelectedCustomField
        {
            get { return selectedCustomField; }
            set
            {
                selectedCustomField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomField"));
            }
        }        
        public ObservableCollection<ConnectorComponents> ComponentsList
        {
            get { return componentsList; }
            set
            {
                componentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentsList"));
            }
        }        
        public string ReferenceCaption
        {
            get
            {
                return referenceCaption;
            }
            set
            {
                referenceCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceCaption"));
            }
        }        
        public string ComponentCaption
        {
            get
            {
                return componentCaption;
            }
            set
            {
                componentCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentCaption"));
            }
        }        
        public ObservableCollection<Connectors> LinkedConnectorList
        {
            get { return linkedConnectorList; }
            set
            {
                linkedConnectorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedConnectorList"));
            }
        }
        //[pramod.misal][GEOS2-5387][08-04-2024]        
        public ObservableCollection<ConnectorAttachements> ConnectorAttachementFilesList
        {
            get { return connectorAttachementFilesList; }
            set
            {
                connectorAttachementFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorAttachementFilesList"));
            }
        }     
        public ObservableCollection<ConnectorAttachements> ConnectorAttachementDeletedFilesList
        {
            get { return connectorAttachementDeletedFilesList; }
            set
            {
                connectorAttachementDeletedFilesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorAttachementDeletedFilesList"));
            }
        }        
        public ConnectorAttachements SelectedConnectorAttachementFiles
        {
            get { return selectedConnectorAttachementFiles; }
            set
            {
                selectedConnectorAttachementFiles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnectorAttachementFiles"));
            }
        }        
        public string LinkedConnectorCaption
        {
            get
            {
                return linkedConnectorCaption;
            }
            set
            {
                linkedConnectorCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedConnectorCaption"));
            }
        }

        #region Visibility        
        public Visibility IsTableView
        {
            get
            {
                return isTableView;
            }

            set
            {
                isTableView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTableView"));
                if (IsTableView == Visibility.Visible)
                    IsCardView = Visibility.Collapsed;
                else
                    IsCardView = Visibility.Visible;
            }
        }        
        public Visibility IsCardView
        {
            get
            {
                return isCardView;
            }

            set
            {
                isCardView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCardView"));
            }
        }        
        public Visibility IsEditConnectorView
        {
            get { return isEditConnectorView; }
            set
            {
                isEditConnectorView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditConnectorView"));
            }
        }        
        public Visibility IsWayVisible
        {
            get { return isWayVisible; }
            set
            {
                isWayVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWayVisible"));
            }
        }        
        public Visibility IsGenderVisible
        {
            get { return isGenderVisible; }
            set
            {
                isGenderVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGenderVisible"));
            }
        }        
        public Visibility IsSealVisible
        {
            get { return isSealVisible; }
            set
            {
                isSealVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSealVisible"));
            }
        }        
        public Visibility IsDiameterVisible
        {
            get { return isDiameterVisible; }
            set
            {
                isDiameterVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDiameterVisible"));
            }
        }        
        public Visibility IsSizeVisible
        {
            get { return isSizeVisible; }
            set
            {
                isSizeVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSizeVisible"));
            }
        }        
        public Visibility IsColorVisible
        {
            get { return isColorVisible; }
            set
            {
                isColorVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColorVisible"));
            }
        }        
        public Visibility IsInternalVisible
        {
            get { return isInternalVisible; }
            set
            {
                isInternalVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInternalVisible"));
            }
        }        
        public Visibility IsExternalVisible
        {
            get { return isExternalVisible; }
            set
            {
                isExternalVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExternalVisible"));
            }
        }        
        public Visibility IsHeightVisible
        {
            get { return isHeightVisible; }
            set
            {
                isHeightVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHeightVisible"));
            }
        }        
        public Visibility IsLengthVisible
        {
            get { return isLengthVisible; }
            set
            {
                isLengthVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLengthVisible"));
            }
        }        
        public Visibility IsWidthVisible
        {
            get { return isWidthVisible; }
            set
            {
                isWidthVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWidthVisible"));
            }
        }      
        //[Sudhir.Jangra][GEOS2-5384]        
        public ObservableCollection<SCMConnectorImage> ConnectorsImageList
        {
            get { return connectorsImageList; }
            set
            {
                connectorsImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorsImageList"));
            }
        }

        #region Enabled
        
        public bool IsColorEnabled
        {
            get { return isColorEnabled; }
            set
            {
                isColorEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsColorEnabled"));
            }
        }
        
        public bool IsWaysEnabled
        {
            get { return isWaysEnabled; }
            set
            {
                isWaysEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWaysEnabled"));
            }
        }        
        public bool IsGenderEnabled
        {
            get { return isGenderEnabled; }
            set
            {
                isGenderEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGenderEnabled"));
            }
        }        
        public bool IsSealingEnabled
        {
            get { return isSealingEnabled; }
            set
            {
                isSealingEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSealingEnabled"));
            }
        }        
        public bool IsInternalEnabled
        {
            get { return isInternalEnabled; }
            set
            {
                isInternalEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInternalEnabled"));
            }
        }        
        public bool IsExternalEnabled
        {
            get { return isExternalEnabled; }
            set
            {
                isExternalEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExternalEnabled"));
            }
        }        
        public bool IsLengthEnabled
        {
            get { return isLengthEnabled; }
            set
            {
                isLengthEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLengthEnabled"));
            }
        }        
        public bool IsHeightEnabled
        {
            get { return isHeightEnabled; }
            set
            {
                isHeightEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHeightEnabled"));
            }
        }
        public bool IsWidthEnabled
        {
            get { return isWidthEnabled; }
            set
            {
                isWidthEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsWidthEnabled"));
            }
        }
        #endregion        
        public ObservableCollection<ConnectorLocation> LocationList
        {
            get
            {
                return locationList;
            }

            set
            {
                locationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationList"));
            }
        }        
        public string LocationCaption
        {
            get
            {
                return locationCaption;
            }
            set
            {
                locationCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LocationCaption"));
            }
        }        
        public ObservableCollection<ConnectorLogEntry> ConnectorChangeLogList
        {
            get { return connectorChangeLogList; }
            set
            {
                connectorChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorChangeLogList"));
            }
        }
        //[pramod.misal][GEOS2-5391][22.04.2024]        
        public ObservableCollection<ConnectorLogEntry> ConnectorCommentsList
        {
            get { return connectorCommentsList; }
            set
            {
                connectorCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorCommentsList"));
            }
        }        
        public ObservableCollection<ScmDrawing> ConnectorDrawingList
        {
            get { return connectorDrawingList; }
            set
            {
                connectorDrawingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorDrawingList"));
            }
        }        
        public string DrawingCaption
        {
            get
            {
                return drawingCaption;
            }

            set
            {
                drawingCaption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DrawingCaption"));
            }
        }        
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged(new PropertyChangedEventArgs("IdConnector")); }
        }        
        public ObservableCollection<ConnectorWorkflowStatus> ReferenceStatusList
        {
            get { return referenceStatusList; }
            set
            {
                referenceStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceStatusList"));
            }
        }        
        public ConnectorWorkflowStatus SelectedReferenceStatus
        {
            get { return selectedReferenceStatus; }
            set
            {
                selectedReferenceStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReferenceStatus"));
            }
        }
        public ObservableCollection<ConnectorWorkflowTransitions> WorkflowTransitionList
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
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedBy"));
            }
        }        
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedIn"));
            }
        }        
        public List<ConnectorLogEntry> UpdatedChangeLogList
        {
            get { return updatedChangeLogList; }
            set
            {
                updatedChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedChangeLogList"));
            }
        }
        public List<ConnectorLogEntry> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));

            }
        }
        public List<ConnectorLogEntry> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }
        public List<ConnectorLogEntry> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));

            }
        }        
        public ConnectorComponents SelectedComponents
        {
            get { return selectedComponents; }
            set
            {
                selectedComponents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComponents"));
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        
        public ConnectorLocation Selectedlocation
        {
            get { return selecedLocation; }
            set
            {
                selecedLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selectedlocation"));
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        
        public SCMConnectorImage SelectedConnectorImage
        {
            get { return selectedConnectorImage; }
            set
            {
                selectedConnectorImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Selectedlocation"));
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        
        public List<ConnectorComponents> DeletedComponentList
        {
            get { return deletedComponentList; }
            set
            {
                deletedComponentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedComponentList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        
        public List<ConnectorLocation> DeletedLocationList
        {
            get { return deletedLocationList; }
            set
            {
                deletedLocationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedLocationList"));
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        
        public List<SCMConnectorImage> DeletedImageList
        {
            get { return deletedImageList; }
            set
            {
                deletedImageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedImageList"));
            }
        }
        
        public List<Connectors> DeletedLinkedConnectorList
        {
            get { return deletedLinkedConnectorList; }
            set
            {
                deletedLinkedConnectorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeletedLinkedConnectorList"));
            }
        }        
        public Connectors SelectedLink
        {
            get { return selectedLink; }
            set
            {
                selectedLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLink"));
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
        public virtual object ParentViewModel { get; set; }
        public virtual string TabName { get; set; }
        public virtual object TabContent { get; protected set; }
        #endregion
           
        public ObservableCollection<LookupValue> ListShape
        {
            get { return listShape; }
            set
            {
                listShape = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListShape"));
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
        public bool IsEnabledCancelButton//[pramod.misal][GEOS2-3132][14/02/2023]
        {
            get { return isEnabledCancelButton; }
            set
            {

                isEnabledCancelButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCancelButton"));
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
        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }
        public ObservableCollection<Column> Columns
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
        public bool ImageOpen
        {
            get; set;
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
        #endregion

        #region Public Icommand
        public ICommand EditConnectorReferenceCommand { get; set; }
        public ICommand DeleteConLinkCommand { get; set; }
        public ICommand AddConnLinkButtonCommand { get; set; }
        public ICommand SealedCheckedCommand { get; set; }
        public ICommand UnSealedCheckedCommand { get; set; }
        public ICommand DeleteConComponentCommand { get; set; }
        public ICommand AddConnComponentButtonCommand { get; set; }
        public ICommand TransferReferenceCommand { get; set; } //[nsatpute][16.07.2025][GEOS2-8090]
        public ICommand DeleteConReferenceCommand { get; set; }
        public ICommand AddConnReferenceButtonCommand { get; set; }
        public ICommand ChangeFamilyCommand { get; set; }
        public ICommand layoutImagesLoadedCommand { get; set; }
        public ICommand ImageLeftArrowCommand { get; set; }
        public ICommand ImageRightArrowCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ContextMenuCommand { get; set; }
        public ICommand DocumentContainerChangedCommand { get; set; }       
        public ICommand DeleteComponentFilterCommand { get; set; }// GEOS2-4602 rdixit 08.09.2023
        public ICommand AddNewComponentFilerButtonCommand { get; set; }
        public ICommand ShowCardViewCommand { get; set; }
        public ICommand ShowGridViewCommand { get; set; }          
        public ICommand OpenFamilyImagesCommand { get; set; }   // GEOS2-4599 rdixit 08.09.2023
        public ICommand OpenSubFamilyImagesCommand { get; set; }   // GEOS2-4599 rdixit 08.09.2023
        public ICommand FamilyPopupClosedCommand { get; set; }
        public ICommand OpenWorkflowDiagramCommand { get; set; }
        public ICommand DetailsImageClickCommand { get; set; }        
        public ICommand EditConnectorViewCancelButtonCommand { get; set; }//[pramod.misal][GEOS2-5374][18-03-2024]
        public ICommand OpenPDFDocumentCommand { get; set; }
        public ICommand OpenSelectedImageCommand { get; set; }
        public ICommand ChangeLogExportToExcelCommand { get; set; }
        public ICommand OpenArticleByDrawingCommand { get; set; }
        public ICommand OpenIDrawingPathCommand { get; set; }
        public ICommand EditConnectorViewAcceptButtonCommand { get; set; } //pramod.misal GEOS2-5479 30-04-2024
        public ICommand AddCommentsCommand { get; set; }//[pramod.misal][07-05-2024]
        public ICommand CommentsGridDoubleClickCommand { get; set; }// [pramod.misal][08-05-2024]
        public ICommand DeleteCommentRowCommand { get; set; }// [pramod.misal][08-05-2024]
        public ICommand AddFileCommand { get; set; }// [pramod.misal][15-05-2024]
        public ICommand EditFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }        
        public ICommand DeleteLocationsCommand { get; set; }//[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        public ICommand DeleteImageCommand { get; set; }
        public ICommand AddConnLocationButtonCommand { get; set; } //[rushikesh.gaikwad][GEOS2-5752][05.08.2024]
        public ICommand DeleteConLocationCommand { get; set; }
        public ICommand EditLocationsCommand { get; set; }
        public ICommand DownloadImageCommand { get; set; } //[pramod.misal][GEOS2-5754][27.08.2024]  
        public ICommand EditImageCommand { get; set; } //[pramod.misal][GEOS2-5754][27.08.2024]  
        public ICommand AddImageCommand { get; set; }  //[pramod.misal][GEOS2-5754][27.08.2024]  
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]

        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        public ICommand DeleteConnectorButtonCommand { get; set; }


        #endregion

        #region Constructor
        public ConnectorDetailViewMode()
        {
            //IsInternalEnable = true;
            //sealingflag = false;
            ImageNameVisibility = Visibility.Collapsed;
            EditConnectorReferenceCommand = new RelayCommand(new Action<object>(EditConnectorReferenceCommandAction));
            EditConnectorViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditConnector));
            DeleteConLinkCommand = new DelegateCommand<object>(DeleteConLink);
            AddConnLinkButtonCommand = new DelegateCommand<object>(AddConnLinks);
            SealedCheckedCommand = new DelegateCommand<object>(SealedCheckedCommandAction);
            UnSealedCheckedCommand = new DelegateCommand<object>(UnSealedCheckedCommandAction);        
            DeleteConComponentCommand = new DelegateCommand<object>(DeleteConComponent);           
            DeleteLocationsCommand = new RelayCommand(new Action<object>(DeleteLocationsItems)); //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
            DeleteImageCommand = new RelayCommand(new Action<object>(DeleteConnectorImage));
            AddConnComponentButtonCommand = new DelegateCommand<object>(AddConnComponent);
            TransferReferenceCommand = new DelegateCommand<object>(TransferReferenceCommandAction); //[nsatpute][16.07.2025][GEOS2-8090]
            DeleteConReferenceCommand = new DelegateCommand<object>(DeleteConReference);
            AddConnReferenceButtonCommand = new DelegateCommand<object>(AddConnReference);
            ChangeFamilyCommand = new DelegateCommand<object>(FamilyChangeEvent);
            layoutImagesLoadedCommand = new DelegateCommand<object>(OpenContentIamge); //[rdixit][06.05.2024][GEOS2-5384]
            ChangeLogExportToExcelCommand = new DelegateCommand<object>(ChangeLogExportToExcel);
            ImageLeftArrowCommand = new DelegateCommand<object>(ImageLeftArrowCommandAction);
            ImageRightArrowCommand = new DelegateCommand<object>(ImageRightArrowCommandAction);                                    
            OpenWorkflowDiagramCommand = new DelegateCommand<object>(OpenWorkflowDiagramCommandAction);//[GEOS2-5380][rdixit][02.04.2024]
            DetailsImageClickCommand = new RelayCommand(new Action<object>(DetailsImageClickCommandAction));
            EditConnectorViewCancelButtonCommand = new DelegateCommand<object>(CloseTabMethod); //[pramod.misal][15-03-2024][GEOS2-5377]
            OpenPDFDocumentCommand = new DelegateCommand<object>(OpenPDFDocument);//[pramod.misal][11-04-2024][GEOS2-5387]
            OpenSelectedImageCommand = new DelegateCommand<object>(OpenSelectedImageAction);
            OpenArticleByDrawingCommand = new DelegateCommand<object>(OpenArticleByDrawingActionCommand);
            OpenIDrawingPathCommand = new DelegateCommand<object>(OpenIDrawingPath);
            //pramod.misal GEOS2-5479 30-04-2024
            AddCommentsCommand = new RelayCommand(new Action<object>(AddCommentsCommandAction));//[pramod.misal][GEOS2-4935]
            CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);//[pramod.misal][GEOS2-4935]
            DeleteCommentRowCommand = new RelayCommand(new Action<object>(DeleteCommentRowCommandAction));//[pramod.misal][GEOS2-4935]
            AddFileCommand = new DelegateCommand<object>(AddFileAction);//[pramod.misal][GEOS2-5755][16.05.2024]
            EditFileCommand = new DelegateCommand<object>(EditFileAction);//[pramod.misal][GEOS2-5755][16.05.2024]
            DeleteFileCommand = new DelegateCommand<object>(DeleteFileAction);//[pramod.misal][GEOS2-5755][16.05.2024]
            AddConnLocationButtonCommand = new DelegateCommand<object>(AddConnLocation); //[rushikesh.gaikwad][GEOS2-5752][05.08.2024]
            EditLocationsCommand = new DelegateCommand<object>(EditLocationsAction);
            DownloadImageCommand = new DelegateCommand<object>(DownloadImageAction);//[pramod.misal][GEOS2-5754][27.08.2024]  
            EditImageCommand = new DelegateCommand<object>(EditImageAction);//[pramod.misal][GEOS2-5754][27.08.2024] 
            AddImageCommand = new DelegateCommand<object>(AddImageAction);//[pramod.misal][GEOS2-5754][27.08.2024]      
            DeleteConnectorButtonCommand = new RelayCommand(new Action<object>(DeleteConnector));  //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
            GeosApplication.Instance.Logger.Log("ConnectorDetailViewMode constructor executed", Category.Info, Priority.Low);
        }
        #endregion

        #region Methods
        private void EditConnectorReferenceCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditConnectorReferenceCommandAction()...", category: Category.Info, priority: Priority.Low);
                //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
                if (obj != null)
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

                    TableView detailView = (TableView)obj;
                    var SelectedConnector = (Connectors)detailView.DataControl.CurrentItem;

                    if (SCMCommon.Instance.Tabs.Any(i => i.TabName == SelectedConnector.Ref && i is ConnectorDetailViewMode))
                    {
                        SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.IndexOf(SCMCommon.Instance.Tabs.First(i => i.TabName == SelectedConnector.Ref && i is ConnectorDetailViewMode));
                    }
                    else
                    {
                        var vm = ViewModelSource.Create<ConnectorDetailViewMode>();
                        vm.Init(SelectedConnector);
                        SCMCommon.Instance.Tabs.Add(vm);
                        SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>(SCMCommon.Instance.Tabs);
                        SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.Count > 0 ? SCMCommon.Instance.Tabs.Count - 1 : 0;
                        SCMCommon.Instance.IsPinned = true;
                    }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditConnectorReferenceCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditConnectorReferenceCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditConnectorReferenceCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
           
        }
        public async Task Init(Connectors connector)
        {
            try
            {
                language = GeosApplication.Instance.CurrentCulture;
                TabName = connector.Ref;
                GeosApplication.Instance.Logger.Log("Init() executed successfully", Category.Info, Priority.Low);
                if (connector == null) return;
                connector = SCMService.GetConnectorProperties_V2670(connector);

                var tasks = new List<Task>();
                tasks.Add(Task.Run(() => SetPermission()));
                tasks.Add(Task.Run(() => FillFamily()));
                tasks.Add(Task.Run(() => FillSubfamily()));
                tasks.Add(Task.Run(() => FillComponentType()));
                tasks.Add(Task.Run(() => FillColor()));
                tasks.Add(Task.Run(() => FillShape()));
                tasks.Add(Task.Run(() => FillGender()));
                tasks.Add(Task.Run(() => FillWorkflowTransitions()));
                tasks.Add(Task.Run(() => FillStatus()));               
                Task.WaitAll(tasks.ToArray());
                await ConnDetailsAsync(connector);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log($"Error in method Init() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }
        public async Task ConnDetailsAsync(Connectors ConnectorDetails)
        {
            try
            {
                IsSealed = ConnectorDetails.IsSealed;
                IsUnSealed = ConnectorDetails.IsUnSealed;
                // Set basic properties
                IdConnector = ConnectorDetails.IdConnector;
                Description = ConnectorDetails.Description;
                Reference = ConnectorDetails.Ref;
                InternalDiameter = ConnectorDetails.InternalDiameter;
                ExternalDiameter = ConnectorDetails.ExternalDiameter;
                Height = ConnectorDetails.Sheight;
                Length = ConnectorDetails.Slength;
                Width = ConnectorDetails.SWidth;
                SelectedFamily = ListFamily.FirstOrDefault(i => i.Id == ConnectorDetails.IdFamily);
                SelectedGender = ListGender.FirstOrDefault(i => i.Id == ConnectorDetails.IdGender);
                SelectedColor = ListColor.FirstOrDefault(i => i.Id == ConnectorDetails.IdColor);
                ListSubfamily = new ObservableCollection<ConnectorSubFamily>(ListSubfamily?.Where(i => i.IdFamily == ConnectorDetails.IdFamily).ToList());
                SelectedSubFamily = ListSubfamily.FirstOrDefault(i => i.Id == ConnectorDetails.IdSubFamily);
                NumWays = ConnectorDetails.NumWays;

                await LoadConnectorDetailsAsync(ConnectorDetails);
                               
                LoadImageSection(ConnectorDetails);
                FillCustomFields();
                FillCustomFieldFamily();
                FillDrawingColumns(ConnectorDrawingList);
                FillDrawingData(ConnectorDrawingList);

                ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>(ConnectorDetails.CommentsList);
                SetUserProfileImage(ConnectorCommentsList);
                ConnectorCommentsList = ConnectorCommentsList;

                if (ConnectorDetails.IsSealingEnabled == true)
                {
                    IsSealed = true;
                    IsUnSealed = false;
                }
                else
                {
                    IsSealed = false;
                    IsUnSealed = true;
                }
                if (ImagesList != null)
                    ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList.OrderBy(i => i.IdPictureType).ThenBy(i => i.Position).ToList());
                CustomePropertiesList = SCMService.GetConnectorCustomPropertiesByFamily_V2500(Convert.ToUInt32(ConnectorDetails.IdFamily));
                Originalconnector = (Connectors)ConnectorDetails.Clone();//connector to save original propties                
                GeosApplication.Instance.Logger.Log("Method ConnDetailsAsync...", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in method ConnDetailsAsync() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in method ConnDetailsAsync() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in method ConnDetailsAsync() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }
        private async Task LoadConnectorDetailsAsync(Connectors connectorDetails)
        {
            if (connectorDetails == null) return;

            var tasks = new List<Task>();

            // Task 1: References List
            tasks.Add(Task.Run(() =>
            {
                var refs = connectorDetails.ConnectorReferenceList ?? new List<ConnectorReference>();
                foreach (var j in refs)
                {
                    try
                    {
                        j.Company = j.CompanyList?.FirstOrDefault(i => i.Id == j.IdCustomer);
                        j.IsDelVisible = HasBasicPermission(j.CreatorId);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error processing ReferencesList: " + ex.Message, Category.Exception, Priority.Low);
                    }
                }

                DispatcherInvokeSafe(() =>
                {
                    ReferencesList = new ObservableCollection<ConnectorReference>(refs);
                    ReferenceCaption = GetCaption("EditConnectorAutoHideReferencesHeader", refs.Count);
                });
            }));

            // Task 2: Components List
            tasks.Add(Task.Run(() =>
            {
                var comps = connectorDetails.ConnectorComponentslist ?? new List<ConnectorComponents>();
                foreach (var i in comps)
                {
                    try
                    {
                        i.Type = ComponentTypeList?.FirstOrDefault(t => t.IdType == i.ComponentIdType);
                        i.ComponentTypeList = new ObservableCollection<ComponentType>(ComponentTypeList);
                        i.ColorList = new ObservableCollection<Color>(ListColor);
                        i.IsDelVisible = HasBasicPermission(i.CreatorId);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error processing ComponentsList: " + ex.Message, Category.Exception, Priority.Low);
                    }
                }

                DispatcherInvokeSafe(() =>
                {
                    ComponentsList = new ObservableCollection<ConnectorComponents>(comps);
                    ComponentCaption = GetCaption("EditConnectorAutoHideComponentsHeader", comps.Count);
                });
            }));

            // Task 3: Linked Connectors
            tasks.Add(Task.Run(() =>
            {
                var links = connectorDetails.LinkedConnectorList ?? new List<Connectors>();
                foreach (var i in links)
                {
                    try
                    {
                        i.IsDelVisible = HasLinkPermission(i.CreatorId);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error processing LinkedConnectorList: " + ex.Message, Category.Exception, Priority.Low);
                    }
                }

                DispatcherInvokeSafe(() =>
                {
                    LinkedConnectorList = new ObservableCollection<Connectors>(links);
                    LinkedConnectorCaption = GetCaption("EditConnectorAutoHideLinksHeader", links.Count);
                });
            }));

            // Task 4: Locations
            tasks.Add(Task.Run(() =>
            {
                var locs = connectorDetails.LocationList ?? new List<ConnectorLocation>();
                foreach (var loc in locs)
                {
                    try
                    {
                        bool hasPerm = HasAdvancedPermission(loc.CreatorId);
                        loc.IsDelVisible = hasPerm;
                        loc.IsEditVisible = hasPerm;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error processing LocationList: " + ex.Message, Category.Exception, Priority.Low);
                    }
                }

                DispatcherInvokeSafe(() =>
                {
                    LocationList = new ObservableCollection<ConnectorLocation>(locs);
                    LocationCaption = GetCaption("EditConnectorAutoHideLocationsHeader", locs.Count);
                });
            }));

            // Task 5: Attachments
            tasks.Add(Task.Run(() =>
            {
                var files = connectorDetails.AttachementFilesList ?? new List<ConnectorAttachements>();
                foreach (var f in files)
                {
                    try
                    {
                        f.IsDelVisible = HasBasicPermission(Convert.ToInt32(f.CreatedBy));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Error processing Attachments: " + ex.Message, Category.Exception, Priority.Low);
                    }
                }

                DispatcherInvokeSafe(() =>
                {
                    ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>(files);
                    SelectedConnectorAttachementFiles = ConnectorAttachementFilesList.FirstOrDefault(a => a.Reference == connectorDetails.Ref);
                });
            }));

            // Task 6: Change Log + Drawings
            tasks.Add(Task.Run(() =>
            {
                var logs = connectorDetails.ChangeLogList?.OrderByDescending(i => i.Datetime).ToList() ?? new List<ConnectorLogEntry>();
                var drawings = SCMService.GetDrawingsByConnectorRef_V2590(connectorDetails.IdConnector) ?? new List<ScmDrawing>();

                DispatcherInvokeSafe(() =>
                {
                    ConnectorChangeLogList = new ObservableCollection<ConnectorLogEntry>(logs);
                    ConnectorDrawingList = new ObservableCollection<ScmDrawing>(drawings);
                    DrawingCaption = GetCaption("EditConnectorAutoHideDrawingsHeader", drawings.Count);
                });
            }));

            // Task 7: Status + Workflow
            tasks.Add(Task.Run(() =>
            {
                try
                {
                 
                    var matching = ConnectorStatusTransition.Where(s => s.IdWorkflowStatusFrom == connectorDetails.IdStatus).ToList();
                    var temp = ConnectorStatus.FirstOrDefault(s => s.IdWorkflowStatus == connectorDetails.IdStatus);
                    DispatcherInvokeSafe(() =>
                    {
                        
                        if (ReferenceStatusList == null)
                            ReferenceStatusList = new ObservableCollection<ConnectorWorkflowStatus>();

                        foreach (var match in matching)
                        {
                            var target = ConnectorStatus.FirstOrDefault(i => i.IdWorkflowStatus == match.IdWorkflowStatusTo);
                            if (target != null)
                                ReferenceStatusList.Add(target);
                        }
                        if (temp != null)
                        {
                            if (!ReferenceStatusList.Any(i => i.IdWorkflowStatus == temp?.IdWorkflowStatus))
                                ReferenceStatusList.Add(temp);

                            SelectedReferenceStatus = ReferenceStatusList.FirstOrDefault(s => s.IdWorkflowStatus == connectorDetails.IdStatus);
                        }
                    });
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Error processing Status List: " + ex.Message, Category.Exception, Priority.Low);
                }
            }));

            // Task 8: Properties
            tasks.Add(Task.Run(() => LoadProperties()));

            // Wait for all
            await Task.WhenAll(tasks);
        }        
        private string GetCaption(string resourceKey, int count) => $"{Application.Current.FindResource(resourceKey)}{count}";
        private bool HasBasicPermission(int createdBy) =>        
            (GeosApplication.Instance.ActiveUser.IdUser ==Convert.ToInt32(createdBy) && GeosApplication.Instance.IsSCMREditConnectorBasic)    
            || GeosApplication.Instance.IsSCMPermissionAdmin    
            || GeosApplication.Instance.IsSCMEditConnectorAdvanced;
        private bool HasAdvancedPermission(int createdBy) =>
            (GeosApplication.Instance.ActiveUser.IdUser == createdBy && (GeosApplication.Instance.IsSCMREditConnectorBasic || 
            GeosApplication.Instance.IsSCMEditConnectorAdvanced))            
            || GeosApplication.Instance.IsSCMPermissionAdmin
            || GeosApplication.Instance.IsSCMEditConnectorAdvanced;
        private bool HasLinkPermission(int createdBy) =>
            (GeosApplication.Instance.ActiveUser.IdUser == createdBy && (GeosApplication.Instance.IsSCMREditConnectorBasic || 
            GeosApplication.Instance.IsSCMEditConnectorLinks))
            || GeosApplication.Instance.IsSCMPermissionAdmin
            || GeosApplication.Instance.IsSCMEditConnectorAdvanced;
        private void DispatcherInvokeSafe(Action action)
        {
            Application.Current.Dispatcher.Invoke(() => action());
        }
        #region Attachment
        //[pramod.misal][GEOS2-5755][15.05.2024]  
        private void AddFileAction(object obj)
        {
            try
            {

                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method AddFileAction..."), category: Category.Info, priority: Priority.Low);

                    AddAttachmentsFilesInConnectorView addAttachmentsFilesInConnectorView = new AddAttachmentsFilesInConnectorView();
                    AddAttachmentsFilesInConnectorViewModel addAttachmentsFilesInConnectorViewModel = new AddAttachmentsFilesInConnectorViewModel();
                    EventHandler handle = delegate { addAttachmentsFilesInConnectorView.Close(); };
                    addAttachmentsFilesInConnectorViewModel.RequestClose += handle;
                    addAttachmentsFilesInConnectorViewModel.WindowHeader = Application.Current.FindResource("AddFileHeader").ToString();
                    addAttachmentsFilesInConnectorViewModel.Init(ConnectorAttachementFilesList);
                    addAttachmentsFilesInConnectorViewModel.IsNew = true;
                    addAttachmentsFilesInConnectorView.DataContext = addAttachmentsFilesInConnectorViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addAttachmentsFilesInConnectorView.Owner = Window.GetWindow(ownerInfo);
                    addAttachmentsFilesInConnectorView.ShowDialog();

                    if (addAttachmentsFilesInConnectorViewModel.IsSave == true)
                    {
                        if (ConnectorAttachementFilesList == null)
                            ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>();
                        addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile.IsDelVisible = true;
                        addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile.TransactionOperation = ModelBase.TransactionOperations.Modify;
                        addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile.Reference = Reference;
                        addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile.Idconnector = Convert.ToInt32(IdConnector);
                        ConnectorAttachementFilesList.Add(addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile);
                        SelectedConnectorFile = addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile;
                        ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>(ConnectorAttachementFilesList.OrderByDescending(x => x.ModifiedDate).ToList());
                        IsEnabledCancelButton = true;
                        SelectedConnectorAttachementFiles = ConnectorAttachementFilesList?[0];
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method AddFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

                }


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5755][16.05.2024]  
        private void EditFileAction(object obj)
        {
            try
            {
                TableView detailView = (TableView)obj;

                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction..."), category: Category.Info, priority: Priority.Low);

                    //TableView detailView = (TableView)obj;
                    ConnectorAttachements connectorAttachements = (ConnectorAttachements)detailView.DataControl.CurrentItem;
                    AddAttachmentsFilesInConnectorView addAttachmentsFilesInConnectorView = new AddAttachmentsFilesInConnectorView();
                    AddAttachmentsFilesInConnectorViewModel addAttachmentsFilesInConnectorViewModel = new AddAttachmentsFilesInConnectorViewModel();
                    EventHandler handle = delegate { addAttachmentsFilesInConnectorView.Close(); };
                    addAttachmentsFilesInConnectorViewModel.RequestClose += handle;
                    addAttachmentsFilesInConnectorViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditFileHeader").ToString();
                    addAttachmentsFilesInConnectorViewModel.IsNew = false;
                    addAttachmentsFilesInConnectorViewModel.EditInit(connectorAttachements);
                    addAttachmentsFilesInConnectorView.DataContext = addAttachmentsFilesInConnectorViewModel;
                    var ownerInfo = (detailView as FrameworkElement);
                    addAttachmentsFilesInConnectorView.Owner = Window.GetWindow(ownerInfo);
                    addAttachmentsFilesInConnectorView.ShowDialog();

                    if (addAttachmentsFilesInConnectorViewModel.IsSave == true)
                    {
                        if (ConnectorAttachementFilesList != null)
                            SelectedConnectorAttachementFiles = ConnectorAttachementFilesList.FirstOrDefault(s => s.Idconnectordoc == addAttachmentsFilesInConnectorViewModel.Idconnectordoc);

                        addAttachmentsFilesInConnectorViewModel.SelectedConnectorFile.TransactionOperation = ModelBase.TransactionOperations.Update;
                        SelectedConnectorAttachementFiles.Reference = Reference;
                        SelectedConnectorAttachementFiles.Idconnector = Convert.ToInt32(IdConnector);
                        SelectedConnectorAttachementFiles.IdDocType = addAttachmentsFilesInConnectorViewModel.SelectedAttachmentType.IdDocType;
                        SelectedConnectorAttachementFiles.IdCustomer = Convert.ToInt32(addAttachmentsFilesInConnectorViewModel.SelectedCompany?.Id);
                        SelectedConnectorAttachementFiles.Idconnectordoc = addAttachmentsFilesInConnectorViewModel.Idconnectordoc;
                        SelectedConnectorAttachementFiles.OriginalFileName = addAttachmentsFilesInConnectorViewModel.FileName;
                        SelectedConnectorAttachementFiles.Description = addAttachmentsFilesInConnectorViewModel.Description;
                        SelectedConnectorAttachementFiles.ConnectorAttachementsDocInBytes = addAttachmentsFilesInConnectorViewModel.FileInBytes;
                        SelectedConnectorAttachementFiles.SavedFileName = addAttachmentsFilesInConnectorViewModel.ConnectorAttachmentSavedFileName;
                        SelectedConnectorAttachementFiles.AttachmentType = addAttachmentsFilesInConnectorViewModel.SelectedAttachmentType;
                        SelectedConnectorAttachementFiles.UpdatedDate = addAttachmentsFilesInConnectorViewModel.UpdatedDate;
                        //SelectedConnectorAttachementFiles.ModifiedDate = addAttachmentsFilesInConnectorViewModel.UpdatedDate;
                        SelectedConnectorAttachementFiles.ModifiedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        SelectedConnectorAttachementFiles.AttachmentType.DocumentType = addAttachmentsFilesInConnectorViewModel.SelectedAttachmentType.DocumentType;
                        SelectedConnectorAttachementFiles.CustomerName = addAttachmentsFilesInConnectorViewModel.SelectedCompany.Name;
                        SelectedConnectorAttachementFiles.AttachmentType = addAttachmentsFilesInConnectorViewModel.SelectedAttachmentType;
                        SelectedConnectorAttachementFiles.DocumentType = new Data.Common.DocumentType();
                        SelectedConnectorAttachementFiles.DocumentType = addAttachmentsFilesInConnectorViewModel.SelectedAttachmentType.DocumentType;
                        ConnectorAttachementFilesList.OrderByDescending(s => s.CreatedDate);
                        IsEnabledCancelButton = true;
                    }

                    GeosApplication.Instance.Logger.Log(string.Format("Method EditFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
       
        //[pramod.misal][GEOS2-5755][16.05.2024]  
        private void DeleteFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction..."), category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                  
                    if (ConnectorAttachementDeletedFilesList == null)
                    {
                        ConnectorAttachementDeletedFilesList = new ObservableCollection<ConnectorAttachements>();
                    }
                    if (SelectedConnectorAttachementFiles.TransactionOperation != ModelBase.TransactionOperations.Modify)
                        ConnectorAttachementDeletedFilesList.Add(SelectedConnectorAttachementFiles);

                    ConnectorAttachementFilesList.Remove(SelectedConnectorAttachementFiles);
                    ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>(ConnectorAttachementFilesList);
                    SelectedConnectorAttachementFiles = ConnectorAttachementFilesList.FirstOrDefault();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenPDFDocument(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument..."), category: Category.Info, priority: Priority.Low);

                // Open PDF in another window
                DocumentView documentView = new DocumentView();
                DocumentViewModel documentViewModel = new DocumentViewModel();
                documentViewModel.OpenPdfByOptionWayDetectionSparePart(SelectedConnectorAttachementFiles, obj);


                if (documentViewModel.IsPresent)
                {
                    documentView.DataContext = documentViewModel;
                    documentView.Show();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method OpenPDFDocument()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - ServiceUnexceptedException", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenPDFDocument() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Location
        //[rushikesh.gaikwad][GEOS2-5752][05.08.2024][rdixit][12.08.2024][GEOS2-5752]
        private void AddConnLocation(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddConnLocation()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddLocationViewModel addLocationViewModel = new AddLocationViewModel();
                ConnectorLocation oneconn = new ConnectorLocation();
                AddLocationView addLocationView = new AddLocationView();
                EventHandler handle = delegate { addLocationView.Close(); };
                addLocationViewModel.RequestClose += handle;
                addLocationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddsamplelocationHeader").ToString();
                addLocationViewModel.IsNew = true;
                addLocationView.DataContext = addLocationViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                addLocationView.ShowDialogWindow();
                if (addLocationViewModel.IsSave)
                {
                    ConnectorLocation newLocation = addLocationViewModel.SelectedPlants;
                    if (LocationList == null)
                    {
                        LocationList = new ObservableCollection<ConnectorLocation>();
                    }
                    if (addLocationViewModel.SelectedPlants != null)
                    {
                        newLocation.IsDelVisible = true;
                        newLocation.IsEditVisible = true;
                        newLocation.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                        newLocation.TransactionOperation = ModelBase.TransactionOperations.Modify;
                        LocationList.Add(newLocation);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddConnLocation()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddConnLocation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][12.08.2024][GEOS2-5752]
        private void EditLocationsAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditLocationsAction()...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic && Selectedlocation != null)
            {

                AddLocationView addLocationView = new AddLocationView();
                AddLocationViewModel addLocationViewModel = new AddLocationViewModel();
                EventHandler handle = delegate { addLocationView.Close(); };
                addLocationViewModel.RequestClose += handle;
                addLocationViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditsamplelocationHeader").ToString();
                addLocationView.DataContext = addLocationViewModel;
                addLocationViewModel.EditInit(Selectedlocation);

                if (addLocationViewModel.SelectedPlant.IdCompany == Selectedlocation.Idsite)  //[rushikesh.gaikwad][GEOS2-5752][16.08.2024]
                {
                    addLocationView.ShowDialog();
                }

                if (addLocationViewModel.IsSave)
                {
                    ConnectorLocation newLocation = LocationList.FirstOrDefault(i => i.IdLocationByConnector == Selectedlocation.IdLocationByConnector);
                    if (Selectedlocation.Quantity != addLocationViewModel.SelectedPlants.Quantity ||
                        Selectedlocation.QuantityWithoutWires != addLocationViewModel.SelectedPlants.QuantityWithoutWires ||
                        Selectedlocation.Location != addLocationViewModel.SelectedPlants.Location ||
                        Selectedlocation.IsDamaged != addLocationViewModel.SelectedPlants.IsDamaged ||
                        Selectedlocation.Idsite != addLocationViewModel.SelectedPlants.Idsite)
                    {
                        if (newLocation.TransactionOperation != ModelBase.TransactionOperations.Modify)
                        {
                            newLocation.CountryName = addLocationViewModel.SelectedPlants.CountryName;
                            newLocation.CountryIconUrl = addLocationViewModel.SelectedPlants.CountryIconUrl;
                            newLocation.ShortName = addLocationViewModel.SelectedPlants.ShortName;
                            newLocation.Quantity = addLocationViewModel.SelectedPlants.Quantity;
                            newLocation.QuantityWithoutWires = addLocationViewModel.SelectedPlants.QuantityWithoutWires;
                            newLocation.Location = addLocationViewModel.SelectedPlants.Location;
                            newLocation.IsDamaged = addLocationViewModel.SelectedPlants.IsDamaged;
                            newLocation.Idsite = addLocationViewModel.SelectedPlants.Idsite;
                            newLocation.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                            newLocation.TransactionOperation = ModelBase.TransactionOperations.Update;
                        }
                    }
                }
            }
        }
        #endregion

        #region Image
        //[pramod.misal][GEOS2-5754][27.08.2024]  
        private void DownloadImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadImageAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DownloadImageMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    IsEnabledCancelButton = true;
                    SCMConnectorImage ObjImage = (SCMConnectorImage)obj;
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.Title = "Select path";
                    dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                      "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                      "Portable Network Graphic (*.png)|*.png";
                    dlg.FileName = ObjImage.SavedFileName;

                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        System.IO.File.WriteAllBytes(dlg.FileName, ObjImage.ConnectorsImageInBytes);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method DownloadImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DownloadImageAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-5754][01.07.2024]     
        private void EditImageAction(object obj)
        {
            if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
            {
                try
                {
                    GeosApplication.Instance.Logger.Log("Method EditImageAction()...", category: Category.Info, priority: Priority.Low);

                    AddImageInConnectorView addImageInConnectorView = new AddImageInConnectorView();
                    AddImageInConnectorViewModel addImageInConnectorViewModel = new AddImageInConnectorViewModel();
                    EventHandler handle = delegate { addImageInConnectorView.Close(); };
                    addImageInConnectorViewModel.RequestClose += handle;
                    addImageInConnectorViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditImageHeader").ToString();

                    addImageInConnectorViewModel.IsNew = false;
                    SCMConnectorImage tempObject = new SCMConnectorImage();
                    tempObject = (SCMConnectorImage)obj;
                    addImageInConnectorViewModel.ImageList = ConnectorsImageList;

                    addImageInConnectorViewModel.EditInit(tempObject, ConnectorsImageList);
                    int index_new = ConnectorsImageList.IndexOf(tempObject);

                    addImageInConnectorView.DataContext = addImageInConnectorViewModel;
                    addImageInConnectorView.ShowDialog();

                    if (addImageInConnectorViewModel.IsSave == true)
                    {
                        if (addImageInConnectorViewModel.OldDefaultImage != null)
                        {
                            int index_old = ConnectorsImageList.IndexOf(addImageInConnectorViewModel.OldDefaultImage);
                            ConnectorsImageList.Remove(tempObject);
                            SCMConnectorImage tempSCMConnectorImage_Old = new SCMConnectorImage();
                            tempSCMConnectorImage_Old.IdConnectorImage = addImageInConnectorViewModel.SelectedImage.IdConnectorImage;
                            tempSCMConnectorImage_Old.IdPictureType = addImageInConnectorViewModel.SelectedImage.IdPictureType;
                            tempSCMConnectorImage_Old.OriginalFileName = addImageInConnectorViewModel.SelectedImage.OriginalFileName;
                            tempSCMConnectorImage_Old.Description = addImageInConnectorViewModel.SelectedImage.Description;
                            tempSCMConnectorImage_Old.ConnectorsImageInBytes = addImageInConnectorViewModel.SelectedImage.ConnectorsImageInBytes;
                            tempSCMConnectorImage_Old.Position = addImageInConnectorViewModel.SelectedImage.Position;
                            tempSCMConnectorImage_Old.CreatedBy = addImageInConnectorViewModel.SelectedImage.CreatedBy;
                            tempSCMConnectorImage_Old.ModifiedBy = addImageInConnectorViewModel.SelectedImage.ModifiedBy;
                            tempSCMConnectorImage_Old.isDelVisible = true;
                            tempSCMConnectorImage_Old.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                            ConnectorsImageList.Insert(index_old, tempSCMConnectorImage_Old);
                            ConnectorsImageList.Remove(addImageInConnectorViewModel.OldDefaultImage);
                            SCMConnectorImage tempSCMConnectorImage_New = new SCMConnectorImage();
                            tempSCMConnectorImage_New = addImageInConnectorViewModel.OldDefaultImage;
                            tempSCMConnectorImage_New.TransactionOperation = ModelBase.TransactionOperations.Update;
                            ConnectorsImageList.Insert(index_new, tempSCMConnectorImage_New);
                            SelectedConnectorImage = ConnectorsImageList[index_old];

                            SelectedConnectorImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(tempSCMConnectorImage_Old.ConnectorsImageInBytes);
                            if (tempSCMConnectorImage_Old.OriginalFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                            {
                                System.Drawing.Image test = GetwtgByteArrayToImage(tempSCMConnectorImage_Old.ConnectorsImageInBytes);
                                SelectedConnectorImage.AttachmentImage = ConvertDrawingImageToImageSource(test);
                            }
                            else
                                SelectedConnectorImage.AttachmentImage = GetByteArrayToImage(tempSCMConnectorImage_Old.ConnectorsImageInBytes);


                            SelectedImage = SelectedConnectorImage;

                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        }
                        else
                        {
                            int index = ConnectorsImageList.IndexOf(tempObject);
                            ConnectorsImageList.Remove(tempObject);
                            SCMConnectorImage tempSCMConnectorImage = new SCMConnectorImage();
                            tempSCMConnectorImage.IdConnectorImage = addImageInConnectorViewModel.SelectedImage.IdConnectorImage;
                            tempSCMConnectorImage.IdPictureType = addImageInConnectorViewModel.SelectedImage.IdPictureType;
                            tempSCMConnectorImage.OriginalFileName = addImageInConnectorViewModel.SelectedImage.OriginalFileName;
                            tempSCMConnectorImage.SavedFileName = addImageInConnectorViewModel.SelectedImage.SavedFileName;
                            tempSCMConnectorImage.Description = addImageInConnectorViewModel.SelectedImage.Description;
                            tempSCMConnectorImage.ConnectorsImageInBytes = addImageInConnectorViewModel.SelectedImage.ConnectorsImageInBytes;
                            tempSCMConnectorImage.Position = addImageInConnectorViewModel.SelectedImage.Position;
                            if (tempSCMConnectorImage.Position == 1)
                            {
                                tempSCMConnectorImage.IsRatingVisible = Visibility.Visible;
                            }
                            tempSCMConnectorImage.CreatedBy = addImageInConnectorViewModel.SelectedImage.CreatedBy;
                            tempSCMConnectorImage.ModifiedBy = addImageInConnectorViewModel.SelectedImage.ModifiedBy;
                            tempSCMConnectorImage.isDelVisible = true;
                            tempSCMConnectorImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                            tempSCMConnectorImage.TransactionOperation = ModelBase.TransactionOperations.Update;
                            ConnectorsImageList.Insert(index, tempSCMConnectorImage);
                            SelectedConnectorImage = ConnectorsImageList[index];

                            if (tempSCMConnectorImage.OriginalFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                            {
                                System.Drawing.Image test = GetwtgByteArrayToImage(tempSCMConnectorImage.ConnectorsImageInBytes);
                                SelectedConnectorImage.AttachmentImage = ConvertDrawingImageToImageSource(test);
                            }
                            else
                                SelectedConnectorImage.AttachmentImage = GetByteArrayToImage(tempSCMConnectorImage.ConnectorsImageInBytes);

                            SelectedImage = SelectedConnectorImage;
                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        }
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        IsEnabledCancelButton = true;
                        if (ImagesList != null)
                            ImagesList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.OrderBy(i => i.IdPictureType).ThenBy(i => i.Position).ToList());
                    }
                    GeosApplication.Instance.Logger.Log("Method EditImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method EditImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
        }
        public ImageSource ConvertDrawingImageToImageSource(System.Drawing.Image drawingImage)
        {
            try
            {
                if (drawingImage != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        drawingImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        memoryStream.Position = 0;
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                    }
                    return bitmapImage;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ConvertDrawingImageToImageSource." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }
        public System.Drawing.Image GetwtgByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                if (byteArrayIn != null)
                {
                    GeosApplication.Instance.Logger.Log("Execution started in Method GetwtgByteArrayToImage.", category: Category.Info, priority: Priority.Low);
                    byte[] array;
                    byte a = 1;
                    Array objArray = Array.CreateInstance(a.GetType(), byteArrayIn.Length);
                    Array objArrayDest = Array.CreateInstance(a.GetType(), byteArrayIn.Length - 50); ;
                    byteArrayIn.CopyTo(objArray, 0);
                    Array.Copy(objArray, 50, objArrayDest, 0, byteArrayIn.Length - 50);
                    array = (byte[])objArrayDest;
                    MemoryStream myStream = new MemoryStream();
                    myStream.Write(array, 0, array.Length);
                    System.Drawing.Image Img = System.Drawing.Image.FromStream(myStream);
                    return Img;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method GetwtgByteArrayToImage." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return null;
            }
        }

        //[GEOS2-9203][rdixit][06.10.2025]
        public ImageSource GetByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn != null && byteArrayIn.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(byteArrayIn))
                    {
                        Bitmap bmp = new Bitmap(ms);

                        if (Rotate(bmp))
                        {
                            return Convert1(bmp);
                        }
                        else
                        {
                            GeosApplication.Instance.Logger.Log("Using fallback conversion method", category: Category.Info, priority: Priority.Low);
                            return ByteArrayToImage(byteArrayIn);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log($"Error in Method GetByteArrayToImage: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                    return null;
                }
            }

            GeosApplication.Instance.Logger.Log("Invalid byte array input: null or empty", category: Category.Exception, priority: Priority.Low);
            return null;
        }
        public ImageSource ByteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ByteArrayToImage....", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();

                using (var mem = new MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method ByteArrayToImage." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        //[GEOS2-9203][rdixit][06.10.2025]
        public Boolean Rotate(Bitmap bmp)
        {
            try
            {
                System.Drawing.Imaging.PropertyItem pi = bmp.PropertyItems.Select(x => x).FirstOrDefault(x => x.Id == 0x0112);
                if (pi == null)
                {
                    GeosApplication.Instance.Logger.Log("No rotation property found in image", category: Category.Info, priority: Priority.Low);
                    return false;
                }

                byte o = pi.Value[0];

                if (o == 2) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                if (o == 3) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                if (o == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                if (o == 5) bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
                if (o == 6) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                if (o == 7) bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
                if (o == 8) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);

                GeosApplication.Instance.Logger.Log($"Image rotated successfully with orientation: {o}", category: Category.Info, priority: Priority.Low);
                return true;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in Method Rotate: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                return false;
            }
        }
        //[GEOS2-9203][rdixit][06.10.2025]
        public BitmapImage Convert1(Bitmap src)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                GeosApplication.Instance.Logger.Log("Bitmap converted to BitmapImage successfully", category: Category.Info, priority: Priority.Low);
                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in Method Convert1: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        //[pramod.misal][GEOS2-5754][01.07.2024]  
        private void AddImageAction(object obj)
        {
            try
            {
                if ((GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic))
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Method AddImageAction..."), category: Category.Info, priority: Priority.Low);
                    AddImageInConnectorView addImageInConnectorView = new AddImageInConnectorView();
                    AddImageInConnectorViewModel addImageInConnectorViewModel = new AddImageInConnectorViewModel();
                    EventHandler handle = delegate { addImageInConnectorView.Close(); };
                    addImageInConnectorViewModel.RequestClose += handle;
                    addImageInConnectorViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddImageHeader").ToString();
                    addImageInConnectorViewModel.IsNew = true;
                    addImageInConnectorViewModel.ImageList = ConnectorsImageList;
                    addImageInConnectorViewModel.Init(ConnectorsImageList);
                    addImageInConnectorView.DataContext = addImageInConnectorViewModel;
                    var ownerInfo = (obj as FrameworkElement);
                    addImageInConnectorView.Owner = Window.GetWindow(ownerInfo);
                    addImageInConnectorView.ShowDialog();
                    if (ConnectorsImageList == null)
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>();

                    if (ImagesList == null)
                        ImagesList = new ObservableCollection<SCMConnectorImage>();
                    
                    if (addImageInConnectorViewModel.IsSave == true)
                    {
                        SelectedConnectorImage = new SCMConnectorImage();
                        SelectedConnectorImage = addImageInConnectorViewModel.SelectedImage;

                        if (SelectedConnectorImage.OriginalFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                        {
                            System.Drawing.Image test = GetwtgByteArrayToImage(SelectedConnectorImage.ConnectorsImageInBytes);
                            SelectedConnectorImage.AttachmentImage = ConvertDrawingImageToImageSource(test);
                        }
                        else
                            SelectedConnectorImage.AttachmentImage = GetByteArrayToImage(SelectedConnectorImage.ConnectorsImageInBytes);

                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.OrderBy(x => x.Position).ToList());

                        if (addImageInConnectorViewModel.OldDefaultImage != null)
                        {
                            int index_old = ConnectorsImageList.IndexOf(addImageInConnectorViewModel.OldDefaultImage);
                            ConnectorsImageList.Insert(index_old, addImageInConnectorViewModel.SelectedImage);

                            int index_new = ConnectorsImageList.IndexOf(SelectedConnectorImage) + 1;

                            ConnectorsImageList.Remove(addImageInConnectorViewModel.OldDefaultImage);
                            SCMConnectorImage tempSCMConnectorImage = new SCMConnectorImage();
                            tempSCMConnectorImage.IdConnector = addImageInConnectorViewModel.OldDefaultImage.IdConnector;
                            tempSCMConnectorImage.IdConnectorImage = addImageInConnectorViewModel.OldDefaultImage.IdConnectorImage;
                            tempSCMConnectorImage.SavedFileName = addImageInConnectorViewModel.OldDefaultImage.SavedFileName;
                            tempSCMConnectorImage.Description = addImageInConnectorViewModel.OldDefaultImage.Description;
                            tempSCMConnectorImage.ConnectorsImageInBytes = addImageInConnectorViewModel.OldDefaultImage.ConnectorsImageInBytes;
                            tempSCMConnectorImage.OriginalFileName = addImageInConnectorViewModel.OldDefaultImage.OriginalFileName;
                            tempSCMConnectorImage.Position = addImageInConnectorViewModel.OldDefaultImage.Position;
                            tempSCMConnectorImage.CreatedBy = addImageInConnectorViewModel.OldDefaultImage.CreatedBy;
                            tempSCMConnectorImage.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                            tempSCMConnectorImage.IsDelVisible = true;
                            tempSCMConnectorImage.AttachmentImage = SCMCommon.Instance.GetByteArrayToImage(tempSCMConnectorImage.ConnectorsImageInBytes);
                            tempSCMConnectorImage.TransactionOperation = ModelBase.TransactionOperations.Modify;
                            ConnectorsImageList.Insert(index_new, tempSCMConnectorImage);
                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);

                        }
                        else
                        {
                            addImageInConnectorViewModel.SelectedImage.TransactionOperation = ModelBase.TransactionOperations.Modify;
                            ConnectorsImageList.Add(addImageInConnectorViewModel.SelectedImage);
                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);

                            ImagesList.Add(addImageInConnectorViewModel.SelectedImage);
                            ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList);
                        }

                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        // Sort the list by Position (or any other criteria)
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(
                            ConnectorsImageList.OrderBy(x => x.IdConnectorImage)
                        );

                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.ToList().OrderBy(o => (o.Position)).ToList());
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.OrderBy(i => i.IdPictureType));
                        SelectedConnectorImage = ConnectorsImageList.FirstOrDefault();
                        ConnectorsImageList.Where(x => x.IdPictureType == 0).ToList().ForEach(j => j.IsRatingVisible = Visibility.Visible);
                        ConnectorsImageList.Where(x => x.IdPictureType != 0).ToList().ForEach(j => j.IsRatingVisible = Visibility.Hidden);
                        if (ConnectorsImageList.FirstOrDefault(x => x.IdPictureType == 1) != null)
                            ConnectorsImageList.FirstOrDefault(x => x.IdPictureType == 1).IsBreak = true;
                        if (ConnectorsImageList.FirstOrDefault(x => x.IdPictureType == 2) != null)
                            ConnectorsImageList.FirstOrDefault(x => x.IdPictureType == 2).IsBreak = true;
                  
                        SelectedImage = ConnectorsImageList.FirstOrDefault();
                        //[rdixit][24.09.2024][GEOS2-6468]
                        if (ConnectorsImageList.Count == 1)
                        {
                            ConnectorsImageList[0].IsRatingVisible = Visibility.Visible;
                            ConnectorsImageList[0].Position = 1;
                        }
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        IsEnabledCancelButton = true;
                    }
                    if (ImagesList != null)
                        ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList.OrderBy(i => i.IdPictureType).ThenBy(i => i.Position).ToList());
                    GeosApplication.Instance.Logger.Log("Method AddImageAction()....executed successfully", category: Category.Info, priority: Priority.Low);

                }

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddImageAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }


        }
        private void DetailsImageClickCommandAction(object obj)
        {
            try
            {            
                if (SelectedImage != null)
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    Connectors SelectedConnectors = new Connectors();
                    SelectedConnectors.Ref = SelectedImage.Ref;
                    SelectedConnectors.Description = SelectedImage.Description;
                    SelectedConnectors.ConnectorsImageInBytes = SelectedImage.ConnectorsImageInBytes;
                    ConnectorGridImageView connectorGridImageView = new ConnectorGridImageView();
                    ConnectorGridImageViewModel connectorGridImageViewModel = new ConnectorGridImageViewModel();
                    EventHandler handle = delegate { connectorGridImageView.Close(); };
                    connectorGridImageViewModel.RequestClose += handle;
                    connectorGridImageViewModel.Init(SelectedConnectors);
                    connectorGridImageView.DataContext = connectorGridImageViewModel;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    connectorGridImageView.ShowDialogWindow();
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DetailsImageClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void OpenSelectedImageAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetailsImageClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                ImageOpen = true;
                FlowLayoutControl flc = obj as FlowLayoutControl;
                flc.MaximizedElement = flc.Children[1] as FrameworkElement;
                MaximizedElement = SelectedConnectorImage;

                GeosApplication.Instance.Logger.Log("Method DetailsImageClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method DetailsImageClickCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rdixit][06.05.2024][GEOS2-5384]
        private void OpenContentIamge(object obj)
        {
            if (ImageOpen && SelectedConnectorImage != null)
            {
                ImageOpen = false;
                FlowLayoutControl flc = obj as FlowLayoutControl;
                ImageContainer temp = flc.Children.OfType<ImageContainer>().FirstOrDefault(i => ((SCMConnectorImage)i.DataContext).
                IdConnectorImage == SelectedConnectorImage.IdConnectorImage);
                flc.MaximizedElement = temp;
            }
        }
        private void LoadImageSection(Connectors connectorDetails)
        {
            try
            {
                if (connectorDetails == null) return;

                GeosApplication.Instance.Logger.Log("Method LoadImageSection()....", category: Category.Info, priority: Priority.Low);

                // Step 1: Initialize collection
                ConnectorsImageList = ImagesList = new ObservableCollection<SCMConnectorImage>(connectorDetails.ConnectorImageList);

                if (ConnectorsImageList == null || ConnectorsImageList.Count == 0)
                    return;

                // Step 2: Normalize positions & set image sources
                NormalizeImagePositions(ConnectorsImageList);

                // Step 3: Assign permissions (delete/edit visibility)
                ApplyPermissions(ConnectorsImageList);

                // Step 4: Sort order (first by Position, then by PictureType)
                ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(
                    ConnectorsImageList.OrderBy(o => o.Position).ThenBy(o => o.IdPictureType)
                );

                // Step 5: Apply PictureType-specific UI rules
                ApplyPictureTypeRules(ConnectorsImageList);

                // Step 6: Select the first image
                SelectedConnectorImage = ConnectorsImageList.FirstOrDefault();
                SelectedImage = SelectedConnectorImage;

                // Ensure selected image is position 1
                if (SelectedConnectorImage != null)
                    SelectedConnectorImage.Position = 1;

                GeosApplication.Instance.Logger.Log("Method LoadImageSection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error in method LoadImageSection() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Helpers

        private void NormalizeImagePositions(ObservableCollection<SCMConnectorImage> images)
        {
            var assignedPositions = new HashSet<ulong>();
            foreach (var img in images)
            {
                //[GEOS2-9203][rdixit][06.10.2025]
                try
                {
                    if (img.SavedFileName.EndsWith(".wtg", StringComparison.OrdinalIgnoreCase))
                    {
                        var drawingImg = GetwtgByteArrayToImage(img.ConnectorsImageInBytes);
                        img.AttachmentImage = ConvertDrawingImageToImageSource(drawingImg);
                    }
                    else
                    {
                        img.AttachmentImage = GetByteArrayToImage(img.ConnectorsImageInBytes);
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log($"Error in method NormalizeImagePositions() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        private void ApplyPermissions(IEnumerable<SCMConnectorImage> images)
        {
            foreach (var img in images)
            {
                //[GEOS2-9203][rdixit][06.10.2025]
                try
                {
                    bool hasPerm =
                    (GeosApplication.Instance.ActiveUser.IdUser == img.CreatedBy 
                        && (GeosApplication.Instance.IsSCMREditConnectorBasic 
                        || GeosApplication.Instance.IsSCMEditConnectorAdvanced))
                        || GeosApplication.Instance.IsSCMPermissionAdmin;

                    img.IsDelVisible = hasPerm;
                    img.IsEditVisible = hasPerm;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log($"Error in method ApplyPermissions() - {ex.Message}", category: Category.Exception, priority: Priority.Low);
                }
            }
        }

        private void ApplyPictureTypeRules(ObservableCollection<SCMConnectorImage> images)
        {
            foreach (var img in images)
                img.IsRatingVisible = img.IdPictureType == 0 ? Visibility.Visible : Visibility.Hidden;

            var picType1 = images.FirstOrDefault(x => x.IdPictureType == 1);
            if (picType1 != null) picType1.IsBreak = true;

            var picType2 = images.FirstOrDefault(x => x.IdPictureType == 2);
            if (picType2 != null) picType2.IsBreak = true;


            // Special rule when only one image
            if (images.Count == 1)
            {
                images[0].IsRatingVisible = Visibility.Visible;
                images[0].Position = 1;
            }
        }

        #endregion


        #endregion

        #region Comments
        //[pramod.misal][GEOS2-][07.05.2024]      
        private void AddCommentsCommandAction(object obj)
        {
            try
            {         
                GeosApplication.Instance.Logger.Log("Method AddCommentsCommandAction()...", category: Category.Info, priority: Priority.Low);

                AddEditCommentsViewModel addCommentsViewModel = new AddEditCommentsViewModel();
                AddEditCommentsView addCommentsView = new AddEditCommentsView();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();

                if (addCommentsViewModel.SelectedComment != null)
                {
                    if (ConnectorCommentsList == null)
                        ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>();

                    addCommentsViewModel.SelectedComment.IdConnector = (UInt64)IdConnector;
                    addCommentsViewModel.SelectedComment.IsDelVisible = true;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<ConnectorLogEntry>();

                    AddCommentsList.Add(new ConnectorLogEntry()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        IdConnector = addCommentsViewModel.SelectedComment.IdConnector,
                        IdLogEntryByConnector = addCommentsViewModel.SelectedComment.IdLogEntryByConnector,
                        Datetime = addCommentsViewModel.SelectedComment.Datetime,
                        Comments = addCommentsViewModel.SelectedComment.Comments
                    });

                    ConnectorCommentsList.Add(addCommentsViewModel.SelectedComment);
                    SelectedComment = addCommentsViewModel.SelectedComment;
         
                    try
                    {
                        var sortedConConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>(
                            ConnectorCommentsList.OrderByDescending(x => x.Datetime));

                        ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>();
                        foreach (var item in sortedConConnectorCommentsList)
                        {
                            ConnectorCommentsList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {

                        GeosApplication.Instance.Logger.Log("Get an error in AddCommentsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddCommentsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddCommentsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[pramod.misal][GEOS2-][07.05.2024]  
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
            if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
            {
                ConnectorLogEntry logcomments = (ConnectorLogEntry)obj;
                AddEditCommentsView editCommentsView = new AddEditCommentsView();
                AddEditCommentsViewModel editCommentsViewModel = new AddEditCommentsViewModel();
                EventHandler handle = delegate { editCommentsView.Close(); };
                editCommentsViewModel.RequestClose += handle;
                editCommentsViewModel.WindowHeader = Application.Current.FindResource("EditCommentsHeader").ToString();
                editCommentsViewModel.NewItemComment = SelectedComment.Comments;
                string oldComments = SelectedComment.Comments;
                editCommentsViewModel.IdLogEntryByConnector = SelectedComment.IdLogEntryByConnector;
                editCommentsView.DataContext = editCommentsViewModel;
                editCommentsView.ShowDialog();

                if (editCommentsViewModel.SelectedComment != null)
                {
                    SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                    ConnectorCommentsList.FirstOrDefault(s => s.IdLogEntryByConnector == SelectedComment.IdLogEntryByConnector).Comments = editCommentsViewModel.NewItemComment;
                    ConnectorCommentsList.FirstOrDefault(s => s.IdLogEntryByConnector == SelectedComment.IdLogEntryByConnector).Datetime = GeosApplication.Instance.ServerDateTime;
                    editCommentsViewModel.SelectedComment.IdConnector = Convert.ToUInt64(IdConnector);
             
                    if (UpdatedCommentsList == null)
                        UpdatedCommentsList = new List<ConnectorLogEntry>();
                   
                    UpdatedCommentsList.Add(new ConnectorLogEntry()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdConnector = SelectedComment.IdConnector,
                        Comments = SelectedComment.Comments,
                        Datetime = GeosApplication.Instance.ServerDateTime,
                        IdLogEntryByConnector = SelectedComment.IdLogEntryByConnector
                    });
                    
                    try
                    {
                        ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>(ConnectorCommentsList.OrderByDescending(x => x.Datetime));

                        var sortedConConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>(
                           ConnectorCommentsList.OrderByDescending(x => x.Datetime));

                        ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>();
                        foreach (var item in sortedConConnectorCommentsList)
                        {
                            ConnectorCommentsList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in CommentDoubleClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
        }
        //[pramod.misal][GEOS2-][07.05.2024]  
        public void DeleteCommentRowCommandAction(object parameter)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);
                ConnectorLogEntry commentObject = (ConnectorLogEntry)parameter;

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteProductTypeComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (ConnectorCommentsList != null && ConnectorCommentsList?.Count > 0)
                    {
                        ConnectorLogEntry Comment = (ConnectorLogEntry)commentObject;
                        ConnectorCommentsList.Remove(Comment);

                        if (DeleteCommentsList == null)
                            DeleteCommentsList = new List<ConnectorLogEntry>();
                 
                        DeleteCommentsList.Add(new ConnectorLogEntry()
                        {
                            IdUser = Comment.IdUser,
                            IdConnector = Comment.IdConnector,
                            Comments = Comment.Comments,
                            IdLogEntryByConnector = Comment.IdLogEntryByConnector
                        });
                        ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>(ConnectorCommentsList);                       
                   
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteCommentRowCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void OpenIDrawingPath(object folder)
        {
            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
            info.Arguments = "/n," + "\"" + folder + "\"";
            info.WindowStyle = ProcessWindowStyle.Normal;
            System.Diagnostics.Process.Start(info);
        }
        #endregion

        #region CustomFields
        private void FillCustomFields()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomFields().... "), category: Category.Info, priority: Priority.Low);
                CustomFieldsList = new ObservableCollection<ConnectorProperties>(SCMService.GetConnectorCustomPropertiesByFamily_V2500(Convert.ToUInt32(SelectedFamily.Id)));

                SelectedCustomField = CustomFieldsList?.FirstOrDefault();
                if (CustomFieldsList?.Count > 0)
                    IsCustomFieldsVisible = Visibility.Visible;
                else
                    IsCustomFieldsVisible = Visibility.Hidden;
              
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomFields()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCustomFields() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillCustomFieldFamily()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomFieldFamily().... "), category: Category.Info, priority: Priority.Low);
                List<int> idfamily = new List<int>();
                List<int> idLookupKey = new List<int>();

                foreach (var item in CustomFieldsList)
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

                    CustomFieldsList.Where(x => x.IdConnectorProperty == item).ToList()
                        .ForEach(a => a.CustomFieldComboboxList = new ObservableCollection<LookUpValues>(SCMService.GetAllLookUpValuesRecordByIDLookupkey(lookUpKey)));
                    CustomFieldsList.Where(x => x.IdConnectorProperty == item).ToList()
                     .ForEach(a => a.SelectedcustomField = a.CustomFieldComboboxList.FirstOrDefault(c => c.Value_en == CustomFieldsList.Where(x => x.IdConnectorProperty == item).FirstOrDefault().DefaultValue));
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomFieldFamily()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCustomFieldFamily() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region User Profile
        public void SetUserProfileImage(ObservableCollection<ConnectorLogEntry> ConConnectorCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in ConConnectorCommentsList)
                {
                    item.IsDelVisible = (GeosApplication.Instance.ActiveUser.IdUser == item.IdUser && GeosApplication.Instance.IsSCMREditConnectorBasic)
                                || GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditConnectorAdvanced;

                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/FemaleUser_White.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/MaleUser_White.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/wUnknownGender.png");

                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/FemaleUser_Blue.png");
                            else if (item.People.IdPersonGender == 2)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/MaleUser_Blue.png");
                            else if (item.People.IdPersonGender == null)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.SCM;component/Assets/Images/blueUnknownGender.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[pramod.misal][GEOS2-5391][22.04.2024]
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        //[pramod.misal][GEOS2-5391][22.04.2024]
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert ByteArrayToBitmapImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method ByteArrayToBitmapImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ByteArrayToBitmapImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        #endregion

        #region Sealed
        private void SealedCheckedCommandAction(object obj)
        {
            try
            {
                if (IsUnSealed)
                    IsUnSealed = false;
                GeosApplication.Instance.Logger.Log("Method SealedCheckedCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SealedCheckedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UnSealedCheckedCommandAction(object obj)
        {
            try
            {
                if (IsSealed)
                    IsSealed = false;
                GeosApplication.Instance.Logger.Log("Method UnSealedCheckedCommandAction()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method UnSealedCheckedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Link
        //[rdiixt][27.05.2024][GEOS2-5753]
        private void AddConnLinks(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddConnLinks()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AddNewConnectorViewModel AddNewConn = new AddNewConnectorViewModel();
                AddNewConnectorView AddNewConnView = new AddNewConnectorView();
                EventHandler handle = delegate { AddNewConnView.Close(); };
                AddNewConn.RequestClose += handle;
                AddNewConnView.DataContext = AddNewConn;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                AddNewConnView.ShowDialogWindow();
                if (AddNewConn.IsSave)
                {
                    if (LinkedConnectorList == null)
                        LinkedConnectorList = new ObservableCollection<Connectors>();
                    Connectors newLink = AddNewConn.Connector;
                    if (LinkedConnectorList.Any(i => i.IdLinkdType == newLink.IdLinkdType && i.IdConnector == newLink.IdConnector))
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("LinkedConnectorAlreadyExist").ToString(),
                            newLink.Ref, newLink.LinkdTypeName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;

                    }
                    //pramod.misal 06.06.2024
                    if (AddNewConn.Connector.IdConnector == IdConnector)
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("LinkedSameConnectorAlreadyExist").ToString(),
                            newLink.Ref, newLink.LinkdTypeName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    newLink.IsDelVisible = true;
                    newLink.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                    newLink.TransactionOperation = ModelBase.TransactionOperations.Modify;
                    LinkedConnectorList.Add(newLink);
                    LinkedConnectorList = new ObservableCollection<Connectors>(LinkedConnectorList);

                }
                GeosApplication.Instance.Logger.Log("Method AddConnLinks()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddConnLinks()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdiixt][27.05.2024][GEOS2-5753]
        private void DeleteConLink(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteOtherReference"].ToString(),
                       Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    GeosApplication.Instance.Logger.Log("Method DeleteConLink()...", category: Category.Info, priority: Priority.Low);
                    if (LinkedConnectorList != null)
                    {
                        if (DeletedLinkedConnectorList == null)
                            DeletedLinkedConnectorList = new List<Connectors>();

                        if (SelectedLink != null)
                        {
                            Connectors delconn = SelectedLink;
                            if (SelectedLink.TransactionOperation != ModelBase.TransactionOperations.Modify)
                                DeletedLinkedConnectorList.Add(delconn);
                            LinkedConnectorList.Remove(delconn);
                            LinkedConnectorList = LinkedConnectorList;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteConLink()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteConLink()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Component
        private void DeleteConComponent(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteComponent"].ToString(),
                     Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (DeletedComponentList == null)
                        DeletedComponentList = new List<ConnectorComponents>();
                    if (ComponentsList == null)
                        ComponentsList = new ObservableCollection<ConnectorComponents>();
                    if (SelectedComponents != null)
                    {
                        ConnectorComponents delconn = SelectedComponents;
                        if (SelectedComponents.TransactionOperation != ModelBase.TransactionOperations.Modify)
                            DeletedComponentList.Add(delconn);
                        ComponentsList.Remove(delconn);
                        ComponentsList = ComponentsList;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        private void DeleteLocationsItems(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteLocation"].ToString(),
                     Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (DeletedLocationList == null)
                        DeletedLocationList = new List<ConnectorLocation>();
                    if (LocationList == null)
                        LocationList = new ObservableCollection<ConnectorLocation>();
                    if (Selectedlocation != null)
                    {
                        ConnectorLocation delconn = Selectedlocation;
                        if (Selectedlocation.TransactionOperation != ModelBase.TransactionOperations.Modify)
                            DeletedLocationList.Add(delconn);
                        LocationList.Remove(delconn);
                        LocationList = LocationList;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        private void DeleteConnectorImage(object obj)
        {
            try
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteConnectorImage"].ToString(),
                     Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SCMConnectorImage SCMConnectorImage = (SCMConnectorImage)obj;
                    SelectedConnectorImage = SCMConnectorImage;

                    if (DeletedImageList == null)
                        DeletedImageList = new List<SCMConnectorImage>();

                    if (ConnectorsImageList == null)
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>();
                    if (SelectedConnectorImage != null)
                    {

                        SCMConnectorImage delconn = SelectedConnectorImage;
                        if (SelectedConnectorImage.TransactionOperation != ModelBase.TransactionOperations.Modify)
                            DeletedImageList.Add(delconn);
                        ConnectorsImageList.Remove(delconn);


                        ConnectorsImageList = ConnectorsImageList;

                        // Check if there is at least one item in the list and make the first item's IsRatingVisible visible
                        if (ConnectorsImageList.Count > 0)
                        {
                            // Set IsRatingVisible for the first record in the list
                            ConnectorsImageList[0].IsRatingVisible = Visibility.Visible;
                            ConnectorsImageList[0].Position = 1;
                        }
                        ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);
                        var imageToRemove = ImagesList
                        .FirstOrDefault(img => img.IdConnectorImage == SelectedConnectorImage.IdConnectorImage);
                        ImagesList.Remove(imageToRemove);
                        //ImagesList.Remove(addImageInConnectorViewModel.SelectedImage);
                        ImagesList = new ObservableCollection<SCMConnectorImage>(ImagesList);
                        SelectedImage = ImagesList.FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void AddConnComponent(object obj)
        {
            try
            {
                if (ComponentsList == null)
                    ComponentsList = new ObservableCollection<ConnectorComponents>();
                ConnectorComponents newcomp = new ConnectorComponents();
                newcomp.IsDelVisible = true;
                newcomp.ComponentRef = string.Empty;
                newcomp.ColorList = new ObservableCollection<Color>(ListColor.Select(i => (Data.Common.SCM.Color)i.Clone()).ToList());
                newcomp.Color = newcomp.ColorList?.FirstOrDefault();
                newcomp.ComponentTypeList = new ObservableCollection<ComponentType>(ComponentTypeList.Select(i => (ComponentType)i.Clone()).ToList());
                newcomp.Type = newcomp.ComponentTypeList?.FirstOrDefault();
                newcomp.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;

                newcomp.TransactionOperation = ModelBase.TransactionOperations.Modify;
                ComponentsList.Add(newcomp);
                ComponentsList = new ObservableCollection<ConnectorComponents>(ComponentsList);
            }
            catch (Exception ex)
            {
            }
        }
        //[nsatpute][16.07.2025][GEOS2-8090]
        private void TransferReferenceCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TransferReferenceCommandAction().", category: Category.Info, priority: Priority.Low);

                ConnectorReference selectedReference = SelectedReference;

                TransferReferenceView transferReferenceView = new TransferReferenceView();
                TransferReferenceViewModel transferReferenceViewModel = new TransferReferenceViewModel();
                transferReferenceViewModel.Init(((Emdep.Geos.Data.Common.SCM.ConnectorReference)obj).Reference, Header);
                EventHandler handle = delegate { transferReferenceView.Close(); };
                transferReferenceViewModel.RequestClose += handle;
                transferReferenceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("Transferreferenceviewmodel_Transferreferenceanddrawing").ToString();
                transferReferenceView.DataContext = transferReferenceViewModel;             
                transferReferenceView.ShowDialogWindow();
                GeosApplication.Instance.Logger.Log("Method TransferReferenceCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method TransferReferenceCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Other Reference
        //[rdixit][20.05.2024][GEOS2-5477]
        private void DeleteConReference(object obj)
        {
            try
            {
                //[nsatpute][24.07.2025][GEOS2-8090]
                GeosApplication.Instance.Logger.Log("Method DeleteConReference()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteOtherReference"].ToString(),
                    Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    // Check if the reference exists in OTS before deleting
                    //[nsatpute][24.07.2025][GEOS2-8090]
                    if (!CustomerRererenceExistsInOts(SelectedReference.Reference))
                    {
                        if (DeletedReferencesList == null)
                            DeletedReferencesList = new ObservableCollection<ConnectorReference>();
                        if (ReferencesList == null)
                            ReferencesList = new ObservableCollection<ConnectorReference>();
                        if (SelectedReference != null)
                        {
                            ConnectorReference delconn = SelectedReference;
                            if (SelectedReference.TransactionOperation != ModelBase.TransactionOperations.Modify)
                                DeletedReferencesList.Add(delconn);
                            ReferencesList.Remove(delconn);
                            ReferencesList = ReferencesList;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(Application.Current.Resources["Transferreferenceviewmodel_Thereferencecannotberemove"].ToString(),
                            Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteConReference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(ex.Message, Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }
        //[nsatpute][24.07.2025][GEOS2-8090]
        private bool CustomerRererenceExistsInOts(string reference)
        {
            GeosApplication.Instance.Logger.Log(string.Format("Method CustomerRererenceExistsInOts().... "), category: Category.Info, priority: Priority.Low);

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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



            bool result = false;
            ConcurrentBag<ScmDrawing> lstDrawing = new ConcurrentBag<ScmDrawing>();
            try
            {
                //SCMService = new SCMServiceController("localhost:6699");
                List<Data.Common.Company> lstCompany = SCMService.GetAllCompaniesWithServiceProvider_V2660();
                try
                {
                    foreach (Data.Common.Company company in lstCompany)
                    {
                        try
                        {
                            SCMServiceCompanyWise = new SCMServiceController(company.ServiceProviderUrl);
                            //SCMServiceCompanyWise = new SCMServiceController("localhost:6699");

                            List<ScmDrawing> dr = SCMServiceCompanyWise.GetDrawingsByCustomerRef_V2660(reference);
                            if (dr != null && dr.Count > 0)
                            {
                                result = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log($"Get an error in CustomerRererenceExistsInOts() Method. Parallel processing error: Error processing company {company.Name}: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                catch (AggregateException ae)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    foreach (var ex in ae.InnerExceptions)
                        GeosApplication.Instance.Logger.Log("Get an error in CustomerRererenceExistsInOts() Method. Parallel processing error:" + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CustomerRererenceExistsInOts() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CustomerRererenceExistsInOts() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                throw;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CustomerRererenceExistsInOts() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            return result;
        }
        private void AddConnReference(object obj)
        {
            try
            {
                if (ReferencesList == null)
                    ReferencesList = new ObservableCollection<ConnectorReference>();
                ConnectorReference newconn = new ConnectorReference();
                newconn.CompanyList = SCMService.GetAllCompanyList_V2520();
                newconn.Company = newconn.CompanyList?.FirstOrDefault();
                newconn.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                newconn.IsDelVisible = true;
                newconn.Reference = string.Empty;
                newconn.TransactionOperation = ModelBase.TransactionOperations.Modify;
                ReferencesList.Add(newconn);
                ReferencesList = new ObservableCollection<ConnectorReference>(ReferencesList);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
       
        
        private void EditConnector(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditConnector()...", category: Category.Info, priority: Priority.Low);
                ConnectorSearch updatedConn = new ConnectorSearch();
                ConnectorSearch newConn = new ConnectorSearch();
                newConn.UpdatedChangeLogList = new List<ConnectorLogEntry>();

                if (Originalconnector != null)
                {
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMEditConnectorAdvanced)
                    {
                        #region Reference Validation
                        if (ReferencesList != null)
                        {
                            List<string> incorrectRef = new List<string>();
                            string Already_Exist_Ref = string.Empty;
                            List<ConnectorReference> filteredList = ReferencesList.Where(i => i.TransactionOperation ==
                            ModelBase.TransactionOperations.Modify || i.Reference != i.OldReference).ToList();
                            if (filteredList?.Count > 0)
                                Already_Exist_Ref = SCMService.CheckOtherRefIsValid(string.Join(",", filteredList.Select(i => i?.Reference)));
                            if (!string.IsNullOrEmpty(Already_Exist_Ref))
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReferenceConnectorAlreadyExist").ToString(), Already_Exist_Ref),
                                                                       "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                            foreach (var item in ReferencesList.ToList())
                            {
                                if (string.IsNullOrEmpty(item.Reference))
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ReferenceConnectorEmpty").ToString()),
                                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                                if (item.Company != null)
                                {
                                    if (!string.IsNullOrEmpty(item.Company.PatternForConnectorReferences))
                                    {
                                        if (!System.Text.RegularExpressions.Regex.IsMatch(item.Reference, item.Company.PatternForConnectorReferences))
                                        {
                                            incorrectRef.Add(item.Reference);
                                        }
                                    }
                                }
                            }
                            if (incorrectRef?.Count > 0)
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ConnectorRefPatternCheckMessage").ToString(), string.Join(",", incorrectRef)),
                                    "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                        #endregion
                    }

                    if (IsSealed == false && IsUnSealed == false)
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("SealedBothUnCheckMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }


                    #region properties [rdixit][14.05.2024][GEOS2-5477]
                    if (Originalconnector.SelectedColor?.Id != SelectedColor?.Id)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (Originalconnector.SelectedColor == null)
                            log.Comments = string.Format(Application.Current.FindResource("ColorLogEntryByIdConnector").ToString(),"None", SelectedColor?.Name);
                        else
                            log.Comments = string.Format(Application.Current.FindResource("ColorLogEntryByIdConnector").ToString(),Originalconnector.SelectedColor?.Name, SelectedColor?.Name);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.SelectedFamily?.Id != SelectedFamily?.Id)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (Originalconnector.SelectedFamily == null)
                            log.Comments = string.Format(Application.Current.FindResource("FamilyLogEntryByIdConnector").ToString(),"None", SelectedFamily?.Name);
                        else
                            log.Comments = string.Format(Application.Current.FindResource("FamilyLogEntryByIdConnector").ToString(),Originalconnector.SelectedFamily?.Name, SelectedFamily?.Name);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.SelectedSubFamily?.Id != SelectedSubFamily?.Id)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (Originalconnector.SelectedSubFamily == null)
                            log.Comments = string.Format(Application.Current.FindResource("SubFamilyLogEntryByIdConnector").ToString(),
                           "None", SelectedSubFamily?.Name);
                        else if (SelectedSubFamily == null)
                            log.Comments = string.Format(Application.Current.FindResource("SubFamilyLogEntryByIdConnector").ToString(),
                               Originalconnector.SelectedSubFamily?.Name, "None");
                        else
                            log.Comments = string.Format(Application.Current.FindResource("SubFamilyLogEntryByIdConnector").ToString(),
                                    Originalconnector.SelectedSubFamily?.Name, SelectedSubFamily?.Name);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }


                    if (Originalconnector.NumWays != NumWays)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("WaysLogEntryByIdConnector").ToString(),
                            Originalconnector.NumWays, NumWays);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.SelectedGender?.Id != SelectedGender?.Id)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (Originalconnector.SelectedGender == null)
                            log.Comments = string.Format(Application.Current.FindResource("GenderLogEntryByIdConnector").ToString(),
                           "None", SelectedGender?.Name);
                        else
                            log.Comments = string.Format(Application.Current.FindResource("GenderLogEntryByIdConnector").ToString(),
                                Originalconnector.SelectedGender?.Name, SelectedGender?.Name);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.IsSealed != IsSealed)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (Originalconnector.IsSealed)
                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("SealedToUnSealedLogEntryByIdConnector").ToString());
                        else
                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("UnSealedToSealedLogEntryByIdConnector").ToString());
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.Description != Description)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        if (string.IsNullOrEmpty(Originalconnector.Description))
                            log.Comments = string.Format(Application.Current.FindResource("DescriptionLogEntryByIdConnector").ToString(),
                         "None", Description);
                        else if (string.IsNullOrEmpty(Description))
                            log.Comments = string.Format(Application.Current.FindResource("DescriptionLogEntryByIdConnector").ToString(),
                          Originalconnector.Description, "None");
                        else
                            log.Comments = string.Format(Application.Current.FindResource("DescriptionLogEntryByIdConnector").ToString(),
                                Originalconnector.Description, Description);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.InternalDiameter != InternalDiameter)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("InternalDiameterLogEntryByIdConnector").ToString(),
                            Originalconnector.InternalDiameter, InternalDiameter);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }


                    if (Originalconnector.ExternalDiameter != ExternalDiameter)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("ExternalDiameterLogEntryByIdConnector").ToString(),
                            Originalconnector.ExternalDiameter, ExternalDiameter);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.Sheight != Height)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("HeightLogEntryByIdConnector").ToString(),
                            Originalconnector.Sheight, Height);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.Slength != Length)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("LengthLogEntryByIdConnector").ToString(),
                            Originalconnector.Slength, Length);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.SWidth != Width)
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("WidthLogEntryByIdConnector").ToString(),
                            Originalconnector.SWidth, Width);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }

                    if (Originalconnector.SelectedReferenceStatus?.Name != SelectedReferenceStatus?.ToString())
                    {
                        ConnectorLogEntry log = new ConnectorLogEntry();
                        log.IdConnector = (ulong)IdConnector;
                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        log.Comments = string.Format(Application.Current.FindResource("WorkflowStatusLogEntryByIdConnector").ToString(),
                            Originalconnector.SelectedReferenceStatus?.Name, SelectedReferenceStatus.Name);
                        log.IdLogEntryType = 20;
                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                        newConn.UpdatedChangeLogList.Add(log);
                    }
                    #endregion

                    #region Connector Reference
                    if (ReferencesList != null)
                    {
                        #region Update Ref Connector
                        List<ConnectorReference> UpdatedRefList = ReferencesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList();
                        List<ConnectorReference> OldRefList = Originalconnector.ConnectorReferenceList?.ToList();

                        if (newConn.ReferencesList == null)
                            newConn.ReferencesList = new ObservableCollection<ConnectorReference>();

                        if (UpdatedRefList != null && OldRefList != null)
                        {
                            foreach (var item in UpdatedRefList)
                            {
                                if (!OldRefList.Any(i => item.Reference == i.Reference && i.Company?.Id == item.Company?.Id))
                                {
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    item.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                                    string temp = OldRefList.FirstOrDefault(i => i.Reference == item.OldReference)?.Company?.Name;
                                    if (item.Reference != item.OldReference)
                                    {
                                        //[nsatpute][GEOS2-8090][25.07.2025]
                                        if (!CustomerRererenceExistsInOts(item.OldReference))
                                        {
                                            ConnectorLogEntry log = new ConnectorLogEntry();
                                            log.IdConnector = (ulong)IdConnector;
                                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                            if (string.IsNullOrEmpty(item.Reference))
                                                log.Comments = string.Format(Application.Current.FindResource("ReferenceConnectorRefUpdateChangeLog").ToString(), "None", item.Reference);
                                            else
                                                log.Comments = string.Format(Application.Current.FindResource("ReferenceConnectorRefUpdateChangeLog").ToString(), item.OldReference, item.Reference);
                                            log.IdLogEntryType = 20;
                                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                                            newConn.UpdatedChangeLogList.Add(log);
                                        }
                                        else
                                        {
                                            ReferencesList.FirstOrDefault(x => x.Reference == item.Reference).Reference = item.OldReference;
                                            CustomMessageBox.Show(Application.Current.Resources["Transferreferenceviewmodel_Thereferencecannotbemodifi"].ToString(),
                                                Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                            return;
                                        }
                                    }
                                    if (temp != item.Company?.Name)
                                    {
                                        ConnectorLogEntry log = new ConnectorLogEntry();
                                        log.IdConnector = (ulong)IdConnector;
                                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                        if (string.IsNullOrEmpty(temp))
                                            log.Comments = string.Format(Application.Current.FindResource("ReferenceConnectorCompUpdateChangeLog").ToString(), item.Reference, "None", item.Company?.Name);
                                        else
                                            log.Comments = string.Format(Application.Current.FindResource("ReferenceConnectorCompUpdateChangeLog").ToString(), item.Reference,
                                                   temp, item.Company?.Name);
                                        log.IdLogEntryType = 20;
                                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                                        newConn.UpdatedChangeLogList.Add(log);
                                    }
                                    newConn.ReferencesList.Add(item);
                                }
                            }
                        }
                        #endregion

                        #region Add Ref ChangeLog
                        List<ConnectorReference> NewRefList = ReferencesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList();
                        if (NewRefList?.Count > 0)
                        {
                            foreach (var item in NewRefList)
                            {
                                ConnectorLogEntry log = new ConnectorLogEntry();
                                log.IdConnector = (ulong)IdConnector;
                                log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                log.Comments = string.Format(System.Windows.Application.Current.FindResource("ReferenceConnectorAddChangeLog").ToString(), item.Reference);
                                log.IdLogEntryType = 20;
                                log.Datetime = GeosApplication.Instance.ServerDateTime;
                                newConn.UpdatedChangeLogList.Add(log);
                                newConn.ReferencesList.Add(item);
                            }
                        }
                        #endregion

                    }

                    #region Del Ref Changelog
                    if (DeletedReferencesList?.Count > 0)
                    {
                        newConn.DeletedReferencesList = new ObservableCollection<ConnectorReference>(DeletedReferencesList.Select(i => (ConnectorReference)i.Clone()).ToList());
                        foreach (var item in newConn.DeletedReferencesList)
                        {
                            ConnectorLogEntry log = new ConnectorLogEntry();
                            log.IdConnector = (ulong)IdConnector;
                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("ReferenceConnectorDeleteChangeLog").ToString(), item.Reference);
                            log.IdLogEntryType = 20;
                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                            newConn.UpdatedChangeLogList.Add(log);                           
                        }
                    }
                    #endregion

                    #endregion

                    #region Connector Component [rdixit][GEOS2-5751][21-05-2024]
                    if (ComponentsList != null)
                    {
                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMREditConnectorBasic)
                        {
                            foreach (var item in ComponentsList.ToList())
                            {
                                if (string.IsNullOrEmpty(item.ComponentRef))
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentRefEmpty").ToString()),
                                        "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                      
                        #region Update Comp Connector
                        List<ConnectorComponents> UpdatedCompList = ComponentsList.Where(i => i.TransactionOperation ==
                        ModelBase.TransactionOperations.Add).ToList();

                        List<ConnectorComponents> OldCompList = Originalconnector.ConnectorComponentslist?.ToList();
                        if (newConn.ComponentsList == null)
                            newConn.ComponentsList = new ObservableCollection<ConnectorComponents>();

                        if (UpdatedCompList != null && OldCompList != null)
                        {
                            foreach (var item in UpdatedCompList)
                            {
                                if (!OldCompList.Any(i => item.ComponentRef == i.ComponentRef && i.Color?.Id == item.Color?.Id && i.Type?.IdType == item?.Type?.IdType))
                                {
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    item.CreatorId = GeosApplication.Instance.ActiveUser.IdUser;
                                    ConnectorComponents old = OldCompList.FirstOrDefault(i => i.IdComponentsByConnector == item.IdComponentsByConnector);
                                    if (old?.ComponentRef != item.ComponentRef)
                                    {
                                        ConnectorLogEntry log = new ConnectorLogEntry();
                                        log.IdConnector = (ulong)IdConnector;
                                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                        if (string.IsNullOrEmpty(old?.ComponentRef))
                                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentRefUpdateChangeLog").ToString(), "None", item.ComponentRef);
                                        else
                                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentRefUpdateChangeLog").ToString(), old?.ComponentRef, item?.ComponentRef);
                                        log.IdLogEntryType = 20;
                                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                                        newConn.UpdatedChangeLogList.Add(log);
                                    }
                                    if (old?.Color?.Id != item.Color?.Id)
                                    {
                                        ConnectorLogEntry log = new ConnectorLogEntry();
                                        log.IdConnector = (ulong)IdConnector;
                                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                        log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentColorUpdateChangeLog").ToString(),
                                            item.ComponentRef, old?.Color?.Name, item.Color?.Name);
                                        log.IdLogEntryType = 20;
                                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                                        newConn.UpdatedChangeLogList.Add(log);
                                    }
                                    if (old?.Type?.IdType != item.Type?.IdType)
                                    {
                                        ConnectorLogEntry log = new ConnectorLogEntry();
                                        log.IdConnector = (ulong)IdConnector;
                                        log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                        log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentTypeUpdateChangeLog").ToString(),
                                            item.ComponentRef, old?.Type?.Name, item.Type?.Name);
                                        log.IdLogEntryType = 20;
                                        log.Datetime = GeosApplication.Instance.ServerDateTime;
                                        newConn.UpdatedChangeLogList.Add(log);
                                    }

                                    newConn.ComponentsList.Add(item);
                                }
                            }
                        }
                        #endregion

                        #region Add Ref ChangeLog
                        List<ConnectorComponents> NewCompList = ComponentsList.Where(i => i.TransactionOperation ==
                        ModelBase.TransactionOperations.Modify).ToList();
                        if (NewCompList?.Count > 0)
                        {
                            foreach (var item in NewCompList)
                            {
                                ConnectorLogEntry log = new ConnectorLogEntry();
                                log.IdConnector = (ulong)IdConnector;
                                log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentRefAddChangeLog").ToString(),
                                   item.Type?.Name, item.Color?.Name, item.ComponentRef);
                                log.IdLogEntryType = 20;
                                log.Datetime = GeosApplication.Instance.ServerDateTime;
                                newConn.UpdatedChangeLogList.Add(log);
                                newConn.ComponentsList.Add(item);
                            }
                        }
                        #endregion
                    }

                    #region Del Ref Changelog
                    if (DeletedComponentList?.Count > 0)
                    {
                        newConn.DeletedComponentList = DeletedComponentList;
                        foreach (var item in DeletedComponentList)
                        {
                            ConnectorLogEntry log = new ConnectorLogEntry();
                            log.IdConnector = (ulong)IdConnector;
                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorComponentDeleteChangeLog").ToString(),
                                item.Type?.Name, item.Color?.Name, item.ComponentRef);
                            log.IdLogEntryType = 20;
                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                            newConn.UpdatedChangeLogList.Add(log);                            
                        }
                    }
                    #endregion

                    #endregion

                    #region Linked Connector [rdiixt][27.05.2024][GEOS2-5753]
                    if (LinkedConnectorList != null)
                    {                     
                        #region Add Linked Connector ChangeLog
                        List<Connectors> NewLinkConnList = LinkedConnectorList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList();
                        if (NewLinkConnList?.Count > 0)
                        {
                            if (newConn.LinkedConnectorList == null)
                                newConn.LinkedConnectorList = new ObservableCollection<Connectors>();

                            foreach (var item in NewLinkConnList)
                            {
                                ConnectorLogEntry log = new ConnectorLogEntry();
                                log.IdConnector = (ulong)IdConnector;
                                log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                log.Comments = string.Format(Application.Current.FindResource("LinkedConnectorAddChangeLog").ToString(), item.Ref);
                                log.IdLogEntryType = 20;
                                log.Datetime = GeosApplication.Instance.ServerDateTime;
                                newConn.UpdatedChangeLogList.Add(log);
                                newConn.LinkedConnectorList.Add(item);
                            }
                        }
                        #endregion
                    }

                    #region Del Linked Connector Changelog
                    if (DeletedLinkedConnectorList?.Count > 0)
                    {
                        newConn.DeletedLinkedConnectorList = DeletedLinkedConnectorList;
                        foreach (var item in DeletedLinkedConnectorList)
                        {
                            ConnectorLogEntry log = new ConnectorLogEntry();
                            log.IdConnector = (ulong)IdConnector;
                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            log.Comments = string.Format(Application.Current.FindResource("LinkedConnectorDeleteChangeLog").ToString(), item.Ref);
                            log.IdLogEntryType = 20;
                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                            newConn.UpdatedChangeLogList.Add(log);                            
                        }
                    }
                    #endregion

                    #endregion

                    #region attachemnt add                     
                    if (newConn.ConnectorAttachementFilesList == null)
                        newConn.ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>();
                    //Add  Files 
                    foreach (ConnectorAttachements item in ConnectorAttachementFilesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify))
                    {
                        if (item.Idconnectordoc == 0)
                        {
                            item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdLogEntryType = 20,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesAdd").ToString(), item.OriginalFileName)
                            });
                            newConn.ConnectorAttachementFilesList.Add(item);
                        }                        
                    }

                    //Deleted  Files 
                    if (ConnectorAttachementDeletedFilesList?.Count > 0)
                    {
                        newConn.ConnectorAttachementDeletedFilesList = ConnectorAttachementDeletedFilesList;
                        foreach (var item in ConnectorAttachementDeletedFilesList)
                        {
                            ConnectorAttachements ConnectorAttachements = (ConnectorAttachements)item.Clone();
                            ConnectorAttachements.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            newConn.ConnectorAttachementFilesList.Add(ConnectorAttachements);
                            newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry()
                            {
                                IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                                Datetime = GeosApplication.Instance.ServerDateTime,
                                IdLogEntryType = 20,
                                UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                                Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDelete").ToString(), item.OriginalFileName)
                            });
                        }
                    }
                    List<ConnectorAttachements> UpdatedAttachmentList = ConnectorAttachementFilesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList();
                    List<ConnectorAttachements> OldAttachmentList = Originalconnector.AttachementFilesList?.ToList();

                    //Updated ConnectorAttachement Files                       
                    foreach (ConnectorAttachements item in UpdatedAttachmentList)
                    {
                        ConnectorAttachements temp = OldAttachmentList.FirstOrDefault(i => i.Idconnectordoc == item.Idconnectordoc);
                        if (item.OriginalFileName != temp.OriginalFileName || item.Description != temp.Description || item.IdDocType != temp.IdDocType
                            || item.CustomerName != temp.CustomerName || item.ConnectorAttachementsDocInBytes != temp.ConnectorAttachementsDocInBytes)
                        {
                            if (item.OriginalFileName != temp.OriginalFileName)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                if (string.IsNullOrEmpty(item.OriginalFileName))
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), temp.OriginalFileName, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(temp.OriginalFileName))
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), "None", item.OriginalFileName) });
                                    else
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesNameUpdate").ToString(), temp.OriginalFileName, item.OriginalFileName) });
                                }
                            }
                            if (item.Description != temp.Description)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                if (string.IsNullOrEmpty(item.Description))
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), temp.Description, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(temp.Description))
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), "None", item.Description) });
                                    else
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesDescriptionUpdate").ToString(), temp.Description, item.Description) });
                                }
                            }

                            if (item.IdDocType != temp.IdDocType)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                if (temp.IdDocType == 1)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), "None", item.AttachmentType.DocumentType.Name) });
                                }
                                else if (item.IdDocType == 1)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), item.AttachmentType.DocumentType.Name, "None") });
                                }
                                else if (item.IdDocType != temp.IdDocType)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesTypeUpdate").ToString(), temp.DocumentType.Name, item.DocumentType.Name) });
                                }
                            }

                            if (item.CustomerName != temp.CustomerName)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                if (temp.CustomerName == null)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesCustomerTypeUpdate").ToString(), "None", item.CustomerName) });
                                }
                                else if (item.CustomerName == null)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesCustomerTypeUpdate").ToString(), item.CustomerName, "None") });
                                }
                                else if (item.IdCustomer != temp.IdCustomer)
                                {
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesCustomerTypeUpdate").ToString(), temp.CustomerName, item.CustomerName) });
                                }
                            }
                            if (item.ConnectorAttachementsDocInBytes != temp.ConnectorAttachementsDocInBytes)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("DWOSChangeLogFilesUpdate").ToString(), temp.SavedFileName, item.SavedFileName) });
                            }
                            newConn.ConnectorAttachementFilesList.Add(item);
                        }
                    }

                    #endregion

                    #region Comment section PRM
                    if (newConn.ConnectorCommentsList == null)                    
                        newConn.ConnectorCommentsList = new ObservableCollection<ConnectorLogEntry>();
                    
                    if (AddCommentsList != null && AddCommentsList.Count > 0)
                    {                       
                        foreach (var item in AddCommentsList)
                        {
                            ConnectorLogEntry newItem = new ConnectorLogEntry
                            {
                                Comments = item.Comments,
                                IdUser = item.IdUser,
                                IdConnector = item.IdConnector,
                                Datetime = item.Datetime,
                                IdLogEntryType = item.IdLogEntryType,
                                IdLogEntryByConnector = item.IdLogEntryByConnector,

                                TransactionOperation = ModelBase.TransactionOperations.Add
                            };
                            newConn.ConnectorCommentsList.Add(newItem);
                        }
                    }

                    if (UpdatedCommentsList != null && UpdatedCommentsList.Count > 0)
                    {
                        foreach (var item in UpdatedCommentsList)
                        {
                            ConnectorLogEntry newItem = new ConnectorLogEntry
                            {
                                Comments = item.Comments,
                                IdUser = item.IdUser,
                                IdConnector = item.IdConnector,
                                Datetime = item.Datetime,
                                IdLogEntryType = item.IdLogEntryType,
                                IdLogEntryByConnector = item.IdLogEntryByConnector,

                                TransactionOperation = ModelBase.TransactionOperations.Update
                            };
                            newConn.ConnectorCommentsList.Add(newItem);
                        }
                    }

                    if (DeleteCommentsList != null && DeleteCommentsList.Count > 0)
                    {
                        foreach (var item in DeleteCommentsList)
                        {
                            ConnectorLogEntry newItem = new ConnectorLogEntry
                            {

                                Comments = item.Comments,
                                IdUser = item.IdUser,
                                Datetime = item.Datetime,
                                IdLogEntryType = item.IdLogEntryType,
                                IdLogEntryByConnector = item.IdLogEntryByConnector,

                                TransactionOperation = ModelBase.TransactionOperations.Delete
                            };

                            newConn.ConnectorCommentsList.Add(newItem);
                        }

                    }

                    //REf AddUpdateDeleteCommentsByPO_V2480 SRM MGR

                    #endregion

                    #region ConnectorLocation [rdixit][12.08.2024][GEOS2-5752]

                    #region Add Linked Connector ChangeLog
                    if (newConn.LocationList == null)
                        newConn.LocationList = new ObservableCollection<ConnectorLocation>();
                    if (LocationList != null)
                    {
                        List<ConnectorLocation> NewConnLocationList = LocationList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList();
                        if (NewConnLocationList?.Count > 0)
                        {
                            foreach (var item in NewConnLocationList)
                            {
                                ConnectorLogEntry log = new ConnectorLogEntry();
                                log.IdConnector = (ulong)IdConnector;
                                log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                log.Comments = string.Format(System.Windows.Application.Current.FindResource("LocationConnectorAddChangeLog").ToString(), item.Location);
                                log.IdLogEntryType = 20;
                                log.Datetime = GeosApplication.Instance.ServerDateTime;
                                newConn.UpdatedChangeLogList.Add(log);
                                newConn.LocationList.Add(item);
                            }
                        }
                    }
                    #endregion

                    #region Add Linked Connector ChangeLog
                    if (LocationList != null)
                    {
                        LocationList = new ObservableCollection<ConnectorLocation>(LocationList.Select(i => (ConnectorLocation)i.Clone()).ToList());
                        List<ConnectorLocation> OldCompList = Originalconnector?.LocationList?.ToList();
                        List<ConnectorLocation> UpdatedConnLocationList = LocationList.Where(i => i.TransactionOperation ==
                        ModelBase.TransactionOperations.Update).ToList();
           
                        if (UpdatedConnLocationList?.Count > 0)
                        {                            
                            foreach (var item in UpdatedConnLocationList)
                            {                              
                                newConn.LocationList.Add(item);
                                ConnectorLocation original = OldCompList.FirstOrDefault(i => i.IdLocationByConnector == item.IdLocationByConnector);
                                if (original.Quantity != item.Quantity)
                                {
                                    ConnectorLogEntry log = new ConnectorLogEntry();
                                    log.IdConnector = (ulong)IdConnector;
                                    log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    log.Comments = string.Format(Application.Current.FindResource("ConnectorLocationQuntityUpdateChangeLog").ToString(),
                                        original.Location, original.Quantity, item.Quantity);
                                    log.IdLogEntryType = 20;
                                    log.Datetime = GeosApplication.Instance.ServerDateTime;
                                    newConn.UpdatedChangeLogList.Add(log);
                                }
                                if (original.QuantityWithoutWires != item.QuantityWithoutWires)
                                {
                                    ConnectorLogEntry log = new ConnectorLogEntry();
                                    log.IdConnector = (ulong)IdConnector;
                                    log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    log.Comments = string.Format(Application.Current.FindResource("ConnectorLocationQuntitywithoutUpdateChangeLog").ToString()
                                        , original.Location, original.QuantityWithoutWires, item.QuantityWithoutWires);
                                    log.IdLogEntryType = 20;
                                    log.Datetime = GeosApplication.Instance.ServerDateTime;
                                    newConn.UpdatedChangeLogList.Add(log);
                                }
                                if (original.Location != item.Location)
                                {
                                    ConnectorLogEntry log = new ConnectorLogEntry();
                                    log.IdConnector = (ulong)IdConnector;
                                    log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorLocationLocationUpdateChangeLog").ToString(),
                                        original.Location, original.Location, item.Location);
                                    log.IdLogEntryType = 20;
                                    log.Datetime = GeosApplication.Instance.ServerDateTime;
                                    newConn.UpdatedChangeLogList.Add(log);
                                }
                                if (original.IsDamaged != item.IsDamaged)
                                {
                                    ConnectorLogEntry log = new ConnectorLogEntry();
                                    log.IdConnector = (ulong)IdConnector;
                                    log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorLocationisdamagedUpdateChangeLog").ToString(),
                                        original.Location, original.IsDamaged, item.IsDamaged);
                                    log.IdLogEntryType = 20;
                                    log.Datetime = GeosApplication.Instance.ServerDateTime;
                                    newConn.UpdatedChangeLogList.Add(log);
                                }
                                if (original.Idsite != item.Idsite)
                                {
                                    ConnectorLogEntry log = new ConnectorLogEntry();
                                    log.IdConnector = (ulong)IdConnector;
                                    log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                    log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorLocationSiteUpdateChangeLog").ToString(),
                                        original.Location, original.ShortName, item.ShortName);
                                    log.IdLogEntryType = 20;
                                    log.Datetime = GeosApplication.Instance.ServerDateTime;
                                    newConn.UpdatedChangeLogList.Add(log);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Del location Connector Changelog
                    if (DeletedLocationList?.Count > 0)
                    {
                        newConn.DeletedLocationList = new List<ConnectorLocation>(DeletedLocationList.Select(i => (ConnectorLocation)i.Clone()).ToList());
                        foreach (var item in newConn.DeletedLocationList)
                        {
                            ConnectorLogEntry log = new ConnectorLogEntry();
                            log.IdConnector = (ulong)IdConnector;
                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            log.Comments = string.Format(Application.Current.FindResource("LocationConnectorDeleteChangeLog").ToString(), item.Location);
                            log.IdLogEntryType = 20;
                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                            newConn.UpdatedChangeLogList.Add(log);                        
                        }
                        DeletedLocationList = null;
                    }
                    #endregion

                    newConn?.LocationList?.ToList()?.ForEach(i => i.CreatorId = GeosApplication.Instance.ActiveUser.IdUser);
                    #endregion

                    #region Imges Section

                    //Del Connector Image Changelog
                    if (DeletedImageList?.Count > 0)
                    {
                        newConn.DeletedImageList = new List<SCMConnectorImage>(DeletedImageList.Select(i => (SCMConnectorImage)i.Clone()).ToList());
                        foreach (var item in newConn.DeletedImageList)
                        {
                            item.AttachmentImage = null;
                            ConnectorLogEntry log = new ConnectorLogEntry();
                            log.IdConnector = (ulong)IdConnector;
                            log.IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                            log.Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageDeleteChangeLog").ToString(), item.SavedFileName);
                            log.IdLogEntryType = 20;
                            log.Datetime = GeosApplication.Instance.ServerDateTime;
                            newConn.UpdatedChangeLogList.Add(log);                          
                        }                     
                    }

                    //Add Connector Image
                    newConn.ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.
                        Select(i => (SCMConnectorImage)i.Clone()).ToList());

                    foreach (SCMConnectorImage item in newConn.ConnectorsImageList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify))
                    {
                        item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                        item.Ref = Reference;
                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry()
                        {
                            IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdLogEntryType = 20,
                            UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageAddChangeLog").ToString(), item.OriginalFileName)
                        });                        
                    }

                    List<SCMConnectorImage> UpdatedConnectorsImageList = ConnectorsImageList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList();
                    List<SCMConnectorImage> OldConnectorsImageList = Originalconnector?.ConnectorImageList?.ToList();

                    //update connector Image
                    foreach (SCMConnectorImage item in UpdatedConnectorsImageList)
                    {
                        SCMConnectorImage temp = OldConnectorsImageList.FirstOrDefault(i => i.IdConnectorImage == item.IdConnectorImage);
                        if (item.Description != temp.Description || item.OriginalFileName != temp.SavedFileName || item.Description != temp.Description
                             || item.IdPictureType != temp.IdPictureType)
                        {
                            newConn.ImagesList.Add(item);
                            if (item.OriginalFileName != temp.SavedFileName)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.OldFileName = temp.SavedFileName;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                item.Ref = Reference;
                                if (string.IsNullOrEmpty(item.OriginalFileName))
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageNameUpdate").ToString(), temp.SavedFileName, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(temp.SavedFileName))
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageNameUpdate").ToString(), "None", item.OriginalFileName) });
                                    else
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageNameUpdate").ToString(), temp.SavedFileName, item.OriginalFileName) });
                                }
                            }
                            if (item.Description != temp.Description)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                item.Ref = Reference;
                                if (string.IsNullOrEmpty(item.Description))
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageDescriptionUpdate").ToString(), item.OriginalFileName, temp.Description, "None") });
                                else
                                {
                                    if (string.IsNullOrEmpty(temp.Description))
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageDescriptionUpdate").ToString(), item.OriginalFileName, "None", item.Description) });
                                    else
                                        newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(System.Windows.Application.Current.FindResource("ConnectorImageChangeLogImageDescriptionUpdate").ToString(), item.OriginalFileName, temp.Description, item.Description) });
                                }
                            }
                            //[rdixit][GEOS2-9199][11.09.2025]
                            if (item.IdPictureType != temp.IdPictureType)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                item.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                item.Ref = Reference;
                                if (temp.IdPictureType == 1)
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(Application.Current.FindResource("ConnectorImageChangeLogImageTypeUpdate").ToString(), "WTG", "Sample Image") });
                                else
                                    newConn.UpdatedChangeLogList.Add(new ConnectorLogEntry() { IdUser = (uint)GeosApplication.Instance.ActiveUser.IdUser, IdLogEntryType = 20, Datetime = GeosApplication.Instance.ServerDateTime, UserName = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName, Comments = string.Format(Application.Current.FindResource("ConnectorImageChangeLogImageTypeUpdate").ToString(), "Sample Image", "WTG") });
                            }
                        }
                    }
                    #endregion

                    #region Assign [rdixit][14.05.2024][GEOS2-5477]
                    newConn.Reference = Reference;
                    newConn.IdConnector = IdConnector;
                    newConn.SelectedColor = SelectedColor;
                    newConn.SelectedFamily = SelectedFamily;
                    newConn.SelectedSubFamily = SelectedSubFamily;
                    newConn.SelectedGender = SelectedGender;
                    newConn.NumWays = NumWays;
                    newConn.IsSealed = IsSealed;
                    newConn.Description = Description;
                    newConn.InternalDiameter = InternalDiameter;
                    newConn.ExternalDiameter = ExternalDiameter;
                    newConn.Height = Height;
                    newConn.Width = Width;
                    newConn.Length = Length;
                    newConn.SelectedReferenceStatus = SelectedReferenceStatus == null ? new ConnectorWorkflowStatus() : SelectedReferenceStatus;
                    newConn.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    newConn.ModifiedIn = DateTime.Now;
                    newConn.IdSite = GeosApplication.Instance.ActiveUser.Company.IdCompany;                   
                    #endregion

                }

                //[pramod.misal][GEOS2-5754][28.08.2024]
                ConnectorSearch updatednewConn = new ConnectorSearch();
                updatednewConn.ConnectorsImageList = new ObservableCollection<SCMConnectorImage>();

                if (newConn.ConnectorsImageList != null)
                {
                    foreach (SCMConnectorImage item in newConn.ConnectorsImageList)
                    {
                        updatednewConn.ConnectorsImageList.Add((SCMConnectorImage)item.Clone());
                    }
                }

                newConn.ConnectorsImageList.ToList().ForEach(x => x.AttachmentImage = null);
                //IsUpdate = SCMService.UpdateConnectorStatus_V2520(newConn);//[rdixit][14.05.2024][GEOS2-5477]  
                //[pramod.misal][GEOS2-5754][28.08.2024]
                //[rdixit][GEOS2-9199][11.09.2025]
                newConn = SCMService.UpdateConnectorStatus_V2670(newConn);
                IsUpdated = true;
                //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
                AddCommentsList = new List<ConnectorLogEntry>();
                if (IsUpdated == true)
                {
                    try
                    {
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("ConnectorStatusUpdateMessage").ToString(), Reference), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        #region [rdixit][GEOS2-5479][06.05.2024]
                        List<ConnectorWorkflowTransitions> matchingStatus = ConnectorStatusTransition.Where(s => s.IdWorkflowStatusFrom == SelectedReferenceStatus.IdWorkflowStatus).ToList();
                        ReferenceStatusList = new ObservableCollection<ConnectorWorkflowStatus>();
                        if (matchingStatus != null)
                        {
                            foreach (var match in matchingStatus)
                            {
                                ReferenceStatusList.Add(ConnectorStatus.FirstOrDefault(i => i.IdWorkflowStatus == match.IdWorkflowStatusTo));
                            }
                        }
                        ReferenceStatusList = new ObservableCollection<ConnectorWorkflowStatus>(ReferenceStatusList);
                        #endregion

                        #region Reinitialize fields after update [rdixit][22.05.2024][GEOS2-5477]
                        string conref = Reference;
                        if (ReferencesList?.Count > 0)
                            ReferencesList.ToList().ForEach(i => { i.TransactionOperation = ModelBase.TransactionOperations.Add; i.OldReference = i.Reference; });
                        else
                            ReferencesList = new ObservableCollection<ConnectorReference>();

                        if (ComponentsList?.Count > 0)
                            ComponentsList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Add);
                        else
                            ComponentsList = new ObservableCollection<ConnectorComponents>();

                        if (LinkedConnectorList?.Count > 0)
                            LinkedConnectorList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Add);
                        else
                            LinkedConnectorList = new ObservableCollection<Connectors>();

                        if (ConnectorAttachementFilesList?.Count > 0)
                            ConnectorAttachementFilesList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Add);
                        else
                            ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>();
                        //[rdixit][12.08.2024][GEOS2-5752]
                        if (LocationList?.Count > 0)
                            LocationList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Add);
                        else
                            LocationList = new ObservableCollection<ConnectorLocation>();
                        //[pramod.misal][10.09.2024][GEOS2-5754]
                        if (ConnectorsImageList?.Count > 0)
                        {
                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>(newConn.ConnectorsImageList);
                            //[pramod.misal][GEOS2-5754][28.08.2024]
                            foreach (var item in ConnectorsImageList)
                            {
                                item.AttachmentImage = updatednewConn.ConnectorsImageList.FirstOrDefault(x => x.SavedFileName == item.SavedFileName).AttachmentImage;
                            }
                            ConnectorsImageList.ToList().ForEach(i => i.TransactionOperation = ModelBase.TransactionOperations.Add);
                        }

                        else
                            ConnectorsImageList = new ObservableCollection<SCMConnectorImage>();

                        ImagesList = new ObservableCollection<SCMConnectorImage>(ConnectorsImageList);//https://helpdesk.emdep.com/browse/GEOS2-9567 [rdixit][22.09.2025]
                        DeletedLocationList = new List<ConnectorLocation>();
                        DeletedReferencesList = new ObservableCollection<ConnectorReference>();
                        DeletedComponentList = new List<ConnectorComponents>();
                        DeletedLinkedConnectorList = new List<Connectors>();
                        DeleteCommentsList = new List<ConnectorLogEntry>();
                        ConnectorAttachementDeletedFilesList = new ObservableCollection<ConnectorAttachements>();

                        if (ConnectorChangeLogList == null)
                            ConnectorChangeLogList = new ObservableCollection<ConnectorLogEntry>();

                        LocationCaption = Application.Current.FindResource("EditConnectorAutoHideLocationsHeader").ToString() + LocationList?.Count;
                        LinkedConnectorCaption = Application.Current.FindResource("EditConnectorAutoHideLinksHeader").ToString() + LinkedConnectorList?.Count;
                        ComponentCaption = Application.Current.FindResource("EditConnectorAutoHideComponentsHeader").ToString() + ComponentsList?.Count;
                        ReferenceCaption = Application.Current.FindResource("EditConnectorAutoHideReferencesHeader").ToString() + ReferencesList?.Count;
                        #region UpdatedChangeLog
                        if (newConn.UpdatedChangeLogList != null)
                        {
                            foreach (var item in newConn.UpdatedChangeLogList)
                            {
                                if (ConnectorChangeLogList == null)
                                    ConnectorChangeLogList = new ObservableCollection<ConnectorLogEntry>();

                                ConnectorChangeLogList.Add(new ConnectorLogEntry
                                {
                                    UserName = GeosApplication.Instance.ActiveUser.FullName,
                                    IdConnector = item.IdConnector,
                                    IdUser = item.IdUser,
                                    IdLogEntryType = 20,
                                    Datetime = item.Datetime,
                                    Comments = item.Comments
                                });
                            }
                            ConnectorChangeLogList = new ObservableCollection<ConnectorLogEntry>(ConnectorChangeLogList?.OrderByDescending(i => i.Datetime));
                        }
                        #endregion
                        UpdatedChangeLogList = new List<ConnectorLogEntry>();

                        #endregion


                        Originalconnector.Ref = Reference;
                        Originalconnector.SelectedColor = SelectedColor;
                        Originalconnector.SelectedFamily = SelectedFamily;
                        Originalconnector.SelectedSubFamily = SelectedSubFamily;
                        Originalconnector.NumWays = NumWays;
                        Originalconnector.SelectedGender = SelectedGender;
                        Originalconnector.IsSealed = IsSealed;
                        Originalconnector.Description = Description;
                        Originalconnector.InternalDiameter = InternalDiameter;
                        Originalconnector.ExternalDiameter = ExternalDiameter;
                        Originalconnector.Sheight = Height;
                        Originalconnector.Slength = Length;
                        Originalconnector.SWidth = Width;
                        Originalconnector.SelectedReferenceStatus = SelectedReferenceStatus;

                        Originalconnector.ConnectorReferenceList = ReferencesList?.ToList();
                        Originalconnector.ConnectorComponentslist = ComponentsList?.ToList();
                        Originalconnector.LinkedConnectorList = LinkedConnectorList?.ToList();
                        Originalconnector.LocationList = LocationList?.ToList();
                        Originalconnector.AttachementFilesList = ConnectorAttachementFilesList?.ToList();
                        Originalconnector.ConnectorImageList = ConnectorsImageList?.ToList();
                   
                        #region Load Properties
                        FillCustomFields();
                        FillCustomFieldFamily();
                        LoadProperties();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in method EditConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
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
        

        #region Fill List
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
        private void FillFamily()
        {
            try
            {

                //Service GetAllFamilies Updated with GetAllFamilies_V2450 by [GEOS2-4958][rdixit][20.10.2023]
                //Service GetAllFamilies_V2450 Updated with GetAllFamilies_V2480 by [rdixit][GEOS2-5148,5149,5150][29.01.2024]

                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //if (!string.IsNullOrEmpty(SCMCommon.Instance.Header))
                {
                    AllFamilyList = SCMServiceThreadLocal.GetAllFamilies_V2480(language);
                    ListFamily = new ObservableCollection<Family>(AllFamilyList);
                    //ListFamily = new ObservableCollection<Family>(AllFamilyList.Where(i => SCMCommon.Instance.SelectedTypeList.Any(k => k.IdLookupValue == i.IdType)).ToList());
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
                //[rdixit][01.09.2023][GEOS2-4565] Updated class SubFamily to ConnectorSubFamily Hence versionwise method created using new class
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ListSubfamily = new ObservableCollection<ConnectorSubFamily>(SCMServiceThreadLocal.GetAllSubfamilies_V2480(language));//[rdixit][29.1.2024][GEOS2-5148,5149,5150]             
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
                ListColor = new ObservableCollection<Color>(SCMServiceThreadLocal.GetAllColors(language));
                Color colorToRemove = ListColor.FirstOrDefault(x => x.Name == "None");
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
        private void LoadProperties()
        {
            try
            {
                if (SelectedFamily != null)
                {
                    List<ConnectorProperties> PropertiesForSelectedFamily = SCMService.GetPropertyManager_V2490()?.Where(i => i.IdFamily == SelectedFamily.Id)?.ToList();

                    IsColorVisible = Visibility.Hidden;
                    IsWayVisible = Visibility.Hidden;
                    IsGenderVisible = Visibility.Hidden;
                    IsSealVisible = Visibility.Hidden;
                    IsDiameterVisible = Visibility.Hidden;
                    IsSizeVisible = Visibility.Hidden;
                    IsInternalVisible = Visibility.Hidden;
                    IsExternalVisible = Visibility.Hidden;
                    IsHeightVisible = Visibility.Hidden;
                    IsLengthVisible = Visibility.Hidden;
                    IsWidthVisible = Visibility.Hidden;
                    IsColorEnabled = false;
                    IsWaysEnabled = false;
                    IsGenderEnabled = false;
                    IsSealingEnabled = false;
                    IsInternalEnabled = false;
                    IsExternalEnabled = false;
                    IsLengthEnabled = false;
                    IsWidthEnabled = false;
                    IsHeightEnabled = false;
                    if (PropertiesForSelectedFamily != null)
                    {
                        foreach (var item in PropertiesForSelectedFamily)
                        {
                            if (item.PropertyName == "Color")
                            {
                                IsColorVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsColorEnabled = true;
                                }

                            }
                            else if (item.PropertyName == "Gender")
                            {
                                IsGenderVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsGenderEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Ways")
                            {
                                IsWayVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsWaysEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Sealing")
                            {
                                IsSealVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsSealingEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Internal")
                            {
                                IsDiameterVisible = Visibility.Visible;
                                IsInternalVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsInternalEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "External")
                            {
                                IsDiameterVisible = Visibility.Visible;
                                IsExternalVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsExternalEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Height")
                            {
                                IsSizeVisible = Visibility.Visible;
                                IsHeightVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsHeightEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Length")
                            {
                                IsSizeVisible = Visibility.Visible;
                                IsLengthVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsLengthEnabled = true;
                                }
                            }
                            else if (item.PropertyName == "Width")
                            {
                                IsSizeVisible = Visibility.Visible;
                                IsWidthVisible = Visibility.Visible;
                                if (item.IsEnabled && (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin))
                                {
                                    IsWidthEnabled = true;
                                }
                            }
                        }
                    }
                    if (GeosApplication.Instance.IsSCMREditConnectorBasic || GeosApplication.Instance.IsSCMPermissionAdmin)
                    {
                        IsDescriptionEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method LoadActions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillStatus()
        {
            try
            {
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ConnectorStatus = SCMService.GetAllConnectorStatus();
            
                GeosApplication.Instance.Logger.Log(string.Format("Method FillStatus()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillStatus() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillWorkflowTransitions()
        {
            try
            {
                ISCMService SCMServiceThreadLocal = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ConnectorStatusTransition = SCMService.GetAllWorkflowTransitions();
                GeosApplication.Instance.Logger.Log(string.Format("Method FillWorkflowTransitions()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillWorkflowTransitions() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region WorkFlow
        private void OpenWorkflowDiagramCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()...", category: Category.Info, priority: Priority.Low);
                ConnectorWorkflowDiagramViewModel WorkflowDiagramViewModel = new ConnectorWorkflowDiagramViewModel();
                ConnectorWorkflowDiagramView WorkflowDiagramView = new ConnectorWorkflowDiagramView();
                ConnectorWorkflow();
                EventHandler handle = delegate { WorkflowDiagramView.Close(); };
                WorkflowDiagramViewModel.RequestClose += handle;
                WorkflowDiagramViewModel.StatusList = StatusList;
                WorkflowDiagramViewModel.WorkflowTransitionList = WorkflowTransitionList.OrderByDescending(a => a.IdWorkflowTransition).ToList();
                WorkflowDiagramView.DataContext = WorkflowDiagramViewModel;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                WorkflowDiagramView.ShowDialog();

                GeosApplication.Instance.Logger.Log("Method OpenWorkflowDiagramCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenWorkflowDiagramCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ConnectorWorkflow()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConnectorWorkflow()...", category: Category.Info, priority: Priority.Low);
                StatusList = new List<ConnectorWorkflowStatus>(SCMService.GetAllConnectorStatus());
                WorkflowTransitionList = new ObservableCollection<ConnectorWorkflowTransitions>(SCMService.GetAllWorkflowTransitions());
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
        #endregion  
        
        #region Drawings
        //[rdixit][GEOS2-5389][26.04.2024]
        void OpenArticleByDrawingActionCommand(object obj)
        {
            try
            {
                TextBlock IdDrawing_TextBox = (TextBlock)obj;
                ArticlesbyDrawingView articlesByDrawingView = new ArticlesbyDrawingView();
                ArticlesByDrawingViewModel articlesByDrawingViewModel = new ArticlesByDrawingViewModel();
                EventHandler handle = delegate { articlesByDrawingView.Close(); };
                articlesByDrawingViewModel.RequestClose += handle;
                articlesByDrawingViewModel.Init(Convert.ToUInt32(IdDrawing_TextBox.Text));
                articlesByDrawingView.DataContext = articlesByDrawingViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articlesByDrawingView.Show();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method OpenArticleByDrawingActionCommand() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        void FillDrawingColumns(ObservableCollection<ScmDrawing> DrawingList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillDrawingColumns().... "), category: Category.Info, priority: Priority.Low);
                //[GEOS2-6080][05.12.2024][rdixit]
                List<BandItem> bandsLocal = new List<BandItem>();
                BandItem bandAll = new BandItem() { BandName = "All", BandHeader = "", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                Columns = new ObservableCollection<Column>();
                DtDrawingCopy = new DataTable();
                Column c = new Column();
                bandAll.Columns.Add(new ColumnItem() { ColumnFieldName = "IdDrawing", HeaderText = "IdDrawing", Width = 90, IsVertical = false, DrawingSettings = DrawingSettingsType.IdDrawing, Visible = true });
                bandsLocal.Add(bandAll);
                DtDrawingCopy.Columns.Add("IdDrawing", typeof(int));

                #region Detections
                List<string> Detections = new List<string>();
                var test = DrawingList.Where(item => item?.DetectionList != null).SelectMany(item => item.DetectionList.Select(det => det)).Distinct().ToList();

                Detections = DrawingList.Where(item => item?.DetectionList != null).SelectMany(item => item.DetectionList.Select(det => det.Name?.ToLower())).Distinct().ToList();
                BandItem bandDetection = new BandItem() { BandName = "Detection", MinWidth = 250, BandHeader = "Detection", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandDetection.Columns = new ObservableCollection<ColumnItem>();
                if (Detections?.Count > 0)
                {
                    foreach (var item in Detections)
                    {
                        DtDrawingCopy.Columns.Add(item, typeof(int));
                        bandDetection.Columns.Add(new ColumnItem() { ColumnFieldName = item, HeaderText = item, DrawingSettings = DrawingSettingsType.Default, Width = 50, Visible = true, IsVertical = true });
                    }
                }
                else
                {
                    DtDrawingCopy.Columns.Add("", typeof(int));
                    bandDetection.Columns.Add(new ColumnItem() { ColumnFieldName = "No Detection", HeaderText = "", DrawingSettings = DrawingSettingsType.Default, Width = 50, Visible = true, IsVertical = true });
                }
                bandsLocal.Add(bandDetection);
                List<Tuple<byte, string>> CptypeColumns = DrawingList?.Where(i => i.CptypeName != null)?.Select(i => Tuple.Create(i.IdCPType, i.CptypeName))?.Distinct()?.ToList();

                BandItem bandCptype = new BandItem() { BandName = "CP_Type", MinWidth = 250, BandHeader = "CP Type", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandCptype.Columns = new ObservableCollection<ColumnItem>();

                if (CptypeColumns?.Count > 0)
                {
                    foreach (var item in CptypeColumns)
                    {
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                        bandCptype.Columns.Add(new ColumnItem() { ColumnFieldName = item.Item2, HeaderText = item.Item2, Width = 50, DrawingSettings = DrawingSettingsType.Default, Visible = true, IsVertical = true });
                    }
                }
                else
                {
                    DtDrawingCopy.Columns.Add("", typeof(int));
                    bandCptype.Columns.Add(new ColumnItem() { ColumnFieldName = "No CP_Type", HeaderText = "", DrawingSettings = DrawingSettingsType.Default, Width = 50, Visible = true, IsVertical = true });
                }
                bandsLocal.Add(bandCptype);

                List<Tuple<byte, string>> TemplateColumns = DrawingList?.Where(i => i.TemplateName != null)?.Select(i => Tuple.Create(i.IdTemplate, i.TemplateName))?.Distinct()?.ToList();

                BandItem bandTemplate = new BandItem() { BandName = "Template", MinWidth = 250, BandHeader = "Template", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandTemplate.Columns = new ObservableCollection<ColumnItem>();

                if (TemplateColumns?.Count > 0)
                {
                    foreach (var item in TemplateColumns)
                    {
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                        bandTemplate.Columns.Add(new ColumnItem() { ColumnFieldName = item.Item2, Width = 50, HeaderText = item.Item2, DrawingSettings = DrawingSettingsType.Default, Visible = true, IsVertical = true });
                    }
                }
                else
                {
                    DtDrawingCopy.Columns.Add("", typeof(int));
                    bandTemplate.Columns.Add(new ColumnItem() { ColumnFieldName = "No Template", HeaderText = "", DrawingSettings = DrawingSettingsType.Default, Width = 50, Visible = true, IsVertical = true });
                }
                bandsLocal.Add(bandTemplate);

                BandItem bandLast = new BandItem() { BandName = "", BandHeader = "", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandLast.Columns = new ObservableCollection<ColumnItem>();
                DtDrawingCopy.Columns.Add("Comments", typeof(string));
                DtDrawingCopy.Columns.Add("Path", typeof(string));
                DtDrawingCopy.Columns.Add("Site", typeof(string));
                DtDrawingCopy.Columns.Add("Created By", typeof(string));
                DtDrawingCopy.Columns.Add("Modified By", typeof(string));
                DtDrawingCopy.Columns.Add("Debugged", typeof(bool));
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Comments", HeaderText = "Comments", Width = 200, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Path", HeaderText = "Path", Width = 45, IsVertical = true, DrawingSettings = DrawingSettingsType.Image, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Site", HeaderText = "Site", Width = 150, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Created By", HeaderText = "Created By", Width = 300, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Modified By", HeaderText = "Modified By", Width = 300, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Debugged", HeaderText = "Debugged", Width = 40, IsVertical = true, DrawingSettings = DrawingSettingsType.IsChecked, Visible = true });

                bandsLocal.Add(bandLast);
                #endregion
                Bands = new ObservableCollection<BandItem>(bandsLocal);
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
                    //[GEOS2-6080][05.12.2024][rdixit]
                    dr["Created By"] = DrawingList[i].CreatedBy + " " + DrawingList[i].CreatedIn?.ToShortDateString();
                    dr["Modified By"] = DrawingList[i].ModifiedBy + " " + DrawingList[i].ModifiedIn?.ToShortDateString();
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
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDrawingData() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            DtDrawing = DtDrawingCopy;
        }
        #endregion

        //[rdixit][GEOS2-5390][22.04.2024]
        private void ChangeLogExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeLogExportToExcel()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "SCMHistory_" + Reference + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
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

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ChangeLogExportToExcel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ChangeLogExportToExcel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ImageLeftArrowCommandAction(object obj)
        {
            try
            {
                if (ImagesList?.Count > 1)
                {
                    int i = ImagesList.IndexOf(SelectedImage);
                    i = i - 1;
                    if (i > -1)
                        SelectedImage = ImagesList[i];
                    else
                        SelectedImage = ImagesList.LastOrDefault();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ImageLeftArrowCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ImageRightArrowCommandAction(object obj)
        {
            try
            {
                if (ImagesList?.Count > 1)
                {
                    int i = ImagesList.IndexOf(SelectedImage);
                    i = i + 1;
                    if (i < ImagesList.Count)
                        SelectedImage = ImagesList[i];
                    else
                        SelectedImage = ImagesList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ImageRightArrowCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[rdixit][14.05.2024][GEOS2-5477]
        private void FamilyChangeEvent(object obj)
        {
            try
            {
                if (SelectedFamily != null)
                {
                    ListSubfamily = new ObservableCollection<ConnectorSubFamily>(ListSubfamily.Where(i =>
                    i.IdFamily == SelectedFamily.Id).ToList().Select(j => (ConnectorSubFamily)j.Clone()).ToList());
                    SelectedSubFamily = ListSubfamily?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void CloseTabMethod(object obj)
        {

            try
            {
                if (Originalconnector != null)
                {
                    #region [GEOS2-5477][rdixit][31.05.2024]
                    IsUpdated = false;
                    if (Originalconnector != null)
                    {
                        Connectors oldConn = Originalconnector;
                        if (oldConn != null)
                        {
                            #region Check properties 
                            if (oldConn.SelectedColor?.Id != SelectedColor?.Id || oldConn.SelectedFamily?.Id != SelectedFamily?.Id || oldConn.SelectedSubFamily?.Id != SelectedSubFamily?.Id
                                || oldConn.NumWays != NumWays || oldConn.SelectedGender?.Id != SelectedGender?.Id || oldConn.IsSealed != IsSealed
                                || oldConn.Description != Description || oldConn.InternalDiameter != InternalDiameter || oldConn.ExternalDiameter != ExternalDiameter
                                || oldConn.Sheight != Height || oldConn.Slength != Length || oldConn.SWidth != Width || oldConn.SelectedReferenceStatus?.IdWorkflowStatus != SelectedReferenceStatus?.IdWorkflowStatus)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            #endregion

                            #region Check Reference
                            if (ReferencesList != null)
                            {
                                List<ConnectorReference> UpdatedRefList = ReferencesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList();
                                List<ConnectorReference> OldRefList = oldConn.ConnectorReferenceList?.ToList();
                                if (UpdatedRefList != null && OldRefList != null)
                                {
                                    foreach (var item in UpdatedRefList)
                                    {
                                        if (!OldRefList.Any(i => item.Reference == i.Reference && i.Company?.Id == item.Company?.Id))
                                        {
                                            IsUpdated = true;
                                            goto ShowSaveMsg;
                                        }
                                    }
                                }

                                if (ReferencesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                            }

                            if (DeletedReferencesList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            #endregion

                            #region Component
                            if (ComponentsList != null)
                            {
                                List<ConnectorComponents> UpdatedCompList = ComponentsList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList();
                                List<ConnectorComponents> OldCompList = oldConn.ConnectorComponentslist?.ToList();

                                if (UpdatedCompList != null && OldCompList != null)
                                {
                                    foreach (var item in UpdatedCompList)
                                    {
                                        if (!OldCompList.Any(i => item.ComponentRef == i.ComponentRef && i.Color?.Id == item.Color?.Id && i.Type?.IdType == item?.Type?.IdType))
                                        {
                                            IsUpdated = true;
                                            goto ShowSaveMsg;
                                        }
                                    }
                                }

                                if (ComponentsList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                            }

                            if (DeletedComponentList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            #endregion

                            #region Check Link
                            if (LinkedConnectorList != null)
                            {
                                LinkedConnectorList = new ObservableCollection<Connectors>(LinkedConnectorList.Select(i => (Connectors)i.Clone()).ToList());

                                if (LinkedConnectorList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                            }

                            if (DeletedLinkedConnectorList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            #endregion

                            #region Check Attachment

                            if (ConnectorAttachementFilesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }

                            if (ConnectorAttachementDeletedFilesList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            List<ConnectorAttachements> UpdatedAttachmentList = ConnectorAttachementFilesList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList();
                            List<ConnectorAttachements> OldAttachmentList = oldConn.AttachementFilesList?.ToList();

                            foreach (ConnectorAttachements item in UpdatedAttachmentList)
                            {
                                ConnectorAttachements temp = OldAttachmentList.FirstOrDefault(i => i.Idconnectordoc == item.Idconnectordoc);
                                if (item.OriginalFileName != temp.OriginalFileName || item.Description != temp.Description || item.IdDocType != temp.IdDocType || item.CustomerName != temp.CustomerName
                                    || item.ConnectorAttachementsDocInBytes != temp.ConnectorAttachementsDocInBytes)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                            }
                            #endregion

                            #region Check Comment

                            if (AddCommentsList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }

                            if (DeleteCommentsList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            if (ConnectorCommentsList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList()?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }
                            #endregion

                            #region Location
                            //[rushikesh.gaikwad][GEOS2-5752][21.08.2024]
                            if (LocationList != null)
                            {
                                if (LocationList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                                if (LocationList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                            }

                            if (DeletedLocationList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }

                            #endregion

                            //[pramod.misal][GEOS2-5754][04-09-2024]
                            #region Images 
                            if (ConnectorsImageList != null)
                            {

                                if (ConnectorsImageList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Modify).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }
                                if (ConnectorsImageList.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Update).ToList()?.Count > 0)
                                {
                                    IsUpdated = true;
                                    goto ShowSaveMsg;
                                }

                            }
                            if (DeletedImageList?.Count > 0)
                            {
                                IsUpdated = true;
                                goto ShowSaveMsg;
                            }


                            #endregion


                        }
                    ShowSaveMsg:
                        #endregion

                        if (IsUpdated)
                        {

                            MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ArticleGridUpdateWarning"].ToString()),
                                "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                            if (MessageBoxResult == MessageBoxResult.Yes)
                            {
                                EditConnector(null);
                            }
                            else
                            {
                                //SCMCommon.Instance.IsSavebtn = false;
                                //SCMCommon.Instance.IsCancelbtn = true;
                            }
                        }
                        else
                        {
                            SCMCommon.Instance.IsSavebtn = false;
                            SCMCommon.Instance.IsCancelbtn = true;
                            SCMCommon.Instance.IsSignout = false;
                        }
                        //[rdixit][GEOS2-8327][22.07.2025] Removed Pin-Unpin
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void Dispose()
        {
        }
        #endregion
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        private void DeleteConnector(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteConnector()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteReferenceMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
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
                    var Gridview = SCMCommon.Instance.Tabs[SCMCommon.Instance.TabIndex];
                    if (Gridview is ConnectorDetailViewMode detailVM)
                    {
                        string Reference = detailVM.Reference;
                        Int64 IdConnector = detailVM.IdConnector;
                        Int32 IdUser=GeosApplication.Instance.ActiveUser.IdUser;
                        //SCMService = new SCMServiceController("localhost:6699");
                        bool isDeleted = SCMService.DeleteReference_V2680(Reference, IdConnector, IdUser);
                        if (isDeleted)
                        {
                            // After successful deletion, send notification email
                            SendConnectorDeletedEmail(IdConnector);
                            GeosApplication.Instance.Logger.Log($"Connector {Reference} successfully deleted and email notification sent.", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteConnector() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DeleteConnector() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method DeleteConnector() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SendConnectorDeletedEmail(long connectorId)
        {
           
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendConnectorDeletedEmail()...", category: Category.Info, priority: Priority.Low);
                //SCMService = new SCMServiceController("localhost:6699");
                bool IsEmailSend = SCMService.GetConnectorCreatorEmailAndSendMail_V2680(connectorId);

                GeosApplication.Instance.Logger.Log(string.Format("Method DeleteFileAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Error sending email for connector {connectorId}: {ex.Message}", category: Category.Exception, priority: Priority.Low);
            }
            
        }

    }
}
