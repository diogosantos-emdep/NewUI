using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorSearch : ModelBase, IDisposable
    {
        #region Fields
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
        Data.Common.SCM.Color selectedColor;
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
        private  List<SCMConnectorImage> deletedImageList;
        Visibility isCustomFieldsVisible;
        #endregion

        #region Constructor
        public ConnectorSearch()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Visibility IsCustomFieldsVisible
        {
            get { return isCustomFieldsVisible; }
            set
            {
                isCustomFieldsVisible = value;
                OnPropertyChanged("IsCustomFieldsVisible");
            }
        }

        [DataMember]
        public bool IsDescriptionEnabled
        {
            get { return isDescriptionEnabled; }
            set
            {
                isDescriptionEnabled = value;
                OnPropertyChanged("IsDescriptionEnabled");
            }
        }
        [DataMember]
        public int IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        [DataMember]
        public string ReferenceStatus
        {
            get
            {
                return referenceStatus;
            }
            set
            {
                referenceStatus = value;
                OnPropertyChanged("ReferenceStatus");
            }
        }

        [DataMember]
        public string ReferenceStatusHtmlColor
        {
            get
            {
                return referenceStatusHtmlColor;
            }
            set
            {
                referenceStatusHtmlColor = value;
                OnPropertyChanged("ReferenceStatusHtmlColor");
            }
        }
        [DataMember]
        public string CurrentImageCount
        {
            get { return currentImageCount; }
            set
            {
                currentImageCount = value;
                OnPropertyChanged("CurrentImageCount");
            }
        }

        [DataMember]
        public string ImageDescription
        {
            get { return imageDescription; }
            set
            {
                imageDescription = value;
                OnPropertyChanged("ImageDescription");
            }
        }

        [DataMember]
        public SCMConnectorImage SelectedImage
        {
            get { return selectedImage; }
            set
            {
                selectedImage = value;
                if (SelectedImage != null)
                {
                    CurrentImageCount = SelectedImage?.Position + "/" + ImagesList?.Count;
                }
                OnPropertyChanged("SelectedImage");
            }
        }

        [DataMember]
        public ObservableCollection<Connectors> ConnectorList
        {
            get
            {
                return connectorList;
            }

            set
            {
                connectorList = value;
                OnPropertyChanged("ConnectorList");
            }
        }

        [DataMember]
        public ObservableCollection<SCMConnectorImage> ImagesList
        {
            get
            {
                return imagesList;
            }
            set
            {
                imagesList = value;
                OnPropertyChanged("ImagesList");
            }
        }

        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public double InternalDiameter
        {
            get { return internalDiameter; }
            set
            {
                internalDiameter = value;
                OnPropertyChanged("InternalDiameter");
            }
        }

        [DataMember]
        public double ExternalDiameter
        {
            get { return externalDiameter; }
            set
            {
                externalDiameter = value;
                OnPropertyChanged("ExternalDiameter");
            }
        }

        [DataMember]
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [DataMember]
        public double Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        [DataMember]
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [DataMember]
        public Family SelectedFamily
        {
            get { return selectedFamily; }
            set
            {
                selectedFamily = value;
                OnPropertyChanged("SelectedFamily");
            }
        }

        [DataMember]
        public Gender SelectedGender
        {
            get { return selectedGender; }
            set
            {
                selectedGender = value;
                OnPropertyChanged("SelectedGender");
            }
        }

        [DataMember]
        public ConnectorSubFamily SelectedSubFamily
        {
            get { return selectedSubFamily; }
            set
            {
                selectedSubFamily = value;
                OnPropertyChanged("SelectedSubFamily");
            }
        }

        [DataMember]
        public Data.Common.SCM.Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }

        [DataMember]
        public bool IsSealed
        {
            get
            {
                return isSealed;
            }
            set
            {
                isSealed = value;
                OnPropertyChanged("IsSealed");
            }
        }

        [DataMember]
        public bool IsUnSealed
        {
            get
            {
                return isUnSealed;
            }
            set
            {
                isUnSealed = value;
                OnPropertyChanged("IsUnSealed");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorSubFamily> ListSubfamily
        {
            get
            {
                return listSubfamily;
            }
            set
            {
                listSubfamily = value;
                OnPropertyChanged("ListSubfamily");
            }
        }

        [DataMember]
        public Int32 NumWays
        {
            get
            {
                return numWays;
            }
            set
            {
                numWays = value;
                OnPropertyChanged("NumWays");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorReference> ReferencesList
        {
            get { return referencesList; }
            set
            {
                referencesList = value;
                OnPropertyChanged("ReferencesList");
            }
        }

        [DataMember]
        public ConnectorReference SelectedReference
        {
            get { return selectedReference; }
            set
            {
                selectedReference = value;
                OnPropertyChanged("SelectedReference");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorReference> DeletedReferencesList
        {
            get { return deletedReferencesList; }
            set
            {
                deletedReferencesList = value;
                OnPropertyChanged("DeletedReferencesList");
            }
        }
        //[Sudhir.Jangra][GEOS2-5374]
        [DataMember]
        public ObservableCollection<ConnectorProperties> CustomFieldsList
        {
            get { return customFieldsList; }
            set
            {
                customFieldsList = value;
                OnPropertyChanged("CustomFieldsList");
            }
        }

        //[Sudhir.Jangra][GEOS2-5374]
        [DataMember]
        public ConnectorProperties SelectedCustomField
        {
            get { return selectedCustomField; }
            set
            {
                selectedCustomField = value;
                OnPropertyChanged("SelectedCustomField");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorComponents> ComponentsList
        {
            get { return componentsList; }
            set
            {
                componentsList = value;
                OnPropertyChanged("ComponentsList");
            }
        }

        [DataMember]
        public string ReferenceCaption
        {
            get
            {
                return referenceCaption;
            }
            set
            {
                referenceCaption = value;
                OnPropertyChanged("ReferenceCaption");
            }
        }


        [DataMember]
        public string ComponentCaption
        {
            get
            {
                return componentCaption;
            }
            set
            {
                componentCaption = value;
                OnPropertyChanged("ComponentCaption");
            }
        }

        [DataMember]
        public ObservableCollection<Connectors> LinkedConnectorList
        {
            get { return linkedConnectorList; }
            set
            {
                linkedConnectorList = value;
                OnPropertyChanged("LinkedConnectorList");
            }
        }

        //[pramod.misal][GEOS2-5387][08-04-2024]
        [DataMember]
        public ObservableCollection<ConnectorAttachements> ConnectorAttachementFilesList
        {
            get { return connectorAttachementFilesList; }
            set
            {
                connectorAttachementFilesList = value;
                OnPropertyChanged("ConnectorAttachementFilesList");
            }
        }

        //ConnectorAttachementDeletedFilesList

        [DataMember]
        public ObservableCollection<ConnectorAttachements> ConnectorAttachementDeletedFilesList
        {
            get { return connectorAttachementDeletedFilesList; }
            set
            {
                connectorAttachementDeletedFilesList = value;
                OnPropertyChanged("ConnectorAttachementDeletedFilesList");
            }
        }

        [DataMember]
        public ConnectorAttachements SelectedConnectorAttachementFiles
        {
            get { return selectedConnectorAttachementFiles; }
            set
            {
                selectedConnectorAttachementFiles = value;
                OnPropertyChanged("SelectedConnectorAttachementFiles");
            }
        }

        [DataMember]
        public string LinkedConnectorCaption
        {
            get
            {
                return linkedConnectorCaption;
            }
            set
            {
                linkedConnectorCaption = value;
                OnPropertyChanged("LinkedConnectorCaption");
            }
        }

        #region Visibility
        [DataMember]
        public Visibility IsTableView
        {
            get
            {
                return isTableView;
            }

            set
            {
                isTableView = value;
                OnPropertyChanged("IsTableView");
                if (IsTableView == Visibility.Visible)
                    IsCardView = Visibility.Collapsed;
                else
                    IsCardView = Visibility.Visible;
            }
        }

        [DataMember]
        public Visibility IsCardView
        {
            get
            {
                return isCardView;
            }

            set
            {
                isCardView = value;
                OnPropertyChanged("IsCardView");
            }
        }

        [DataMember]
        public Visibility IsEditConnectorView
        {
            get { return isEditConnectorView; }
            set
            {
                isEditConnectorView = value;
                OnPropertyChanged("IsEditConnectorView");
            }
        }
        [DataMember]
        public Visibility IsWayVisible
        {
            get { return isWayVisible; }
            set
            {
                isWayVisible = value;
                OnPropertyChanged("IsWayVisible");
            }
        }

        [DataMember]
        public Visibility IsGenderVisible
        {
            get { return isGenderVisible; }
            set
            {
                isGenderVisible = value;
                OnPropertyChanged("IsGenderVisible");
            }
        }

        [DataMember]
        public Visibility IsSealVisible
        {
            get { return isSealVisible; }
            set
            {
                isSealVisible = value;
                OnPropertyChanged("IsSealVisible");
            }
        }

        [DataMember]
        public Visibility IsDiameterVisible
        {
            get { return isDiameterVisible; }
            set
            {
                isDiameterVisible = value;
                OnPropertyChanged("IsDiameterVisible");
            }
        }

        [DataMember]
        public Visibility IsSizeVisible
        {
            get { return isSizeVisible; }
            set
            {
                isSizeVisible = value;
                OnPropertyChanged("IsSizeVisible");
            }
        }

        [DataMember]
        public Visibility IsColorVisible
        {
            get { return isColorVisible; }
            set
            {
                isColorVisible = value;
                OnPropertyChanged("IsColorVisible");
            }
        }

        [DataMember]
        public Visibility IsInternalVisible
        {
            get { return isInternalVisible; }
            set
            {
                isInternalVisible = value;
                OnPropertyChanged("IsInternalVisible");
            }
        }

        [DataMember]
        public Visibility IsExternalVisible
        {
            get { return isExternalVisible; }
            set
            {
                isExternalVisible = value;
                OnPropertyChanged("IsExternalVisible");
            }
        }

        [DataMember]
        public Visibility IsHeightVisible
        {
            get { return isHeightVisible; }
            set
            {
                isHeightVisible = value;
                OnPropertyChanged("IsHeightVisible");
            }
        }

        [DataMember]
        public Visibility IsLengthVisible
        {
            get { return isLengthVisible; }
            set
            {
                isLengthVisible = value;
                OnPropertyChanged("IsLengthVisible");
            }
        }

        [DataMember]
        public Visibility IsWidthVisible
        {
            get { return isWidthVisible; }
            set
            {
                isWidthVisible = value;
                OnPropertyChanged("IsWidthVisible");
            }
        }

        #endregion

        //[Sudhir.Jangra][GEOS2-5384]
        [DataMember]
        public ObservableCollection<SCMConnectorImage> ConnectorsImageList
        {
            get { return connectorsImageList; }
            set
            {
                connectorsImageList = value;
                OnPropertyChanged("ConnectorsImageList");
            }
        }

        #region Enabled
        [DataMember]
        public bool IsColorEnabled
        {
            get { return isColorEnabled; }
            set
            {
                isColorEnabled = value;
                OnPropertyChanged("IsColorEnabled");
            }
        }
        [DataMember]
        public bool IsWaysEnabled
        {
            get { return isWaysEnabled; }
            set
            {
                isWaysEnabled = value;
                OnPropertyChanged("IsWaysEnabled");
            }
        }

        [DataMember]
        public bool IsGenderEnabled
        {
            get { return isGenderEnabled; }
            set
            {
                isGenderEnabled = value;
                OnPropertyChanged("IsGenderEnabled");
            }
        }

        [DataMember]
        public bool IsSealingEnabled
        {
            get { return isSealingEnabled; }
            set
            {
                isSealingEnabled = value;
                OnPropertyChanged("IsSealingEnabled");
            }
        }

        [DataMember]
        public bool IsInternalEnabled
        {
            get { return isInternalEnabled; }
            set
            {
                isInternalEnabled = value;
                OnPropertyChanged("IsInternalEnabled");
            }
        }

        [DataMember]
        public bool IsExternalEnabled
        {
            get { return isExternalEnabled; }
            set
            {
                isExternalEnabled = value;
                OnPropertyChanged("IsExternalEnabled");
            }
        }

        [DataMember]
        public bool IsLengthEnabled
        {
            get { return isLengthEnabled; }
            set
            {
                isLengthEnabled = value;
                OnPropertyChanged("IsLengthEnabled");
            }
        }

        [DataMember]
        public bool IsHeightEnabled
        {
            get { return isHeightEnabled; }
            set
            {
                isHeightEnabled = value;
                OnPropertyChanged("IsHeightEnabled");
            }
        }

        [DataMember]
        public bool IsWidthEnabled
        {
            get { return isWidthEnabled; }
            set
            {
                isWidthEnabled = value;
                OnPropertyChanged("IsWidthEnabled");
            }
        }
        #endregion

        [DataMember]
        public ObservableCollection<ConnectorLocation> LocationList
        {
            get
            {
                return locationList;
            }

            set
            {
                locationList = value;
                OnPropertyChanged("LocationList");
            }
        }

        [DataMember]
        public string LocationCaption
        {
            get
            {
                return locationCaption;
            }
            set
            {
                locationCaption = value;
                OnPropertyChanged("LocationCaption");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorLogEntry> ConnectorChangeLogList
        {
            get { return connectorChangeLogList; }
            set
            {
                connectorChangeLogList = value;
                OnPropertyChanged("ConnectorChangeLogList");
            }
        }


        //[pramod.misal][GEOS2-5391][22.04.2024]
        [DataMember]
        public ObservableCollection<ConnectorLogEntry> ConnectorCommentsList
        {
            get { return connectorCommentsList; }
            set
            {
                connectorCommentsList = value;
                OnPropertyChanged("ConnectorCommentsList");
            }
        }

        [DataMember]
        public ObservableCollection<ScmDrawing> ConnectorDrawingList
        {
            get { return connectorDrawingList; }
            set
            {
                connectorDrawingList = value;
                OnPropertyChanged("ConnectorDrawingList");
            }
        }

        [DataMember]
        public string DrawingCaption
        {
            get
            {
                return drawingCaption;
            }

            set
            {
                drawingCaption = value;
                OnPropertyChanged("DrawingCaption");
            }
        }

        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }

        [DataMember]
        public ObservableCollection<ConnectorWorkflowStatus> ReferenceStatusList
        {
            get { return referenceStatusList; }
            set
            {
                referenceStatusList = value;
                OnPropertyChanged("ReferenceStatusList");
            }
        }

        [DataMember]
        public ConnectorWorkflowStatus SelectedReferenceStatus
        {
            get { return selectedReferenceStatus; }
            set
            {
                selectedReferenceStatus = value;
                OnPropertyChanged("SelectedReferenceStatus");
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
                OnPropertyChanged("WorkflowTransitionList");
            }
        }

        [DataMember]
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public List<ConnectorLogEntry> UpdatedChangeLogList
        {
            get { return updatedChangeLogList; }
            set
            {
                updatedChangeLogList = value;
                OnPropertyChanged("UpdatedChangeLogList");
            }
        }

        public List<ConnectorLogEntry> AddCommentsList
        {
            get { return addCommentsList; }
            set
            {
                addCommentsList = value;
                OnPropertyChanged("AddCommentsList");

            }
        }
        public List<ConnectorLogEntry> UpdatedCommentsList
        {
            get { return updatedCommentList; }
            set
            {
                updatedCommentList = value;
                OnPropertyChanged("UpdatedCommentsList");
            }
        }
        public List<ConnectorLogEntry> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged("DeleteCommentsList");

            }
        }

        
        [DataMember]
        public ConnectorComponents SelectedComponents
        {
            get { return selectedComponents; }
            set
            {
                selectedComponents = value;
                OnPropertyChanged("SelectedComponents");
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [DataMember]
        public ConnectorLocation Selectedlocation
        {
            get { return selecedLocation; }
            set
            {
                selecedLocation = value;
                OnPropertyChanged("Selectedlocation");
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [DataMember]
        public SCMConnectorImage SelectedConnectorImage
        {
            get { return selectedConnectorImage; }
            set
            {
                selectedConnectorImage = value;
                OnPropertyChanged("Selectedlocation");
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [DataMember]
        public List<ConnectorComponents> DeletedComponentList
        {
            get { return deletedComponentList; }
            set
            {
                deletedComponentList = value;
                OnPropertyChanged("DeletedComponentList");
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [DataMember]
        public List<ConnectorLocation> DeletedLocationList
        {
            get { return deletedLocationList; }
            set
            {
                deletedLocationList = value;
                OnPropertyChanged("DeletedLocationList");
            }
        }
        //[Rahul.Gadhave][GEOS2-5804][Date-5/08/2024]
        [DataMember]
        public List<SCMConnectorImage> DeletedImageList
        {
            get { return deletedImageList; }
            set
            {
                deletedImageList = value;
                OnPropertyChanged("DeletedImageList");
            }
        }
        [DataMember]
        public List<Connectors> DeletedLinkedConnectorList
        {
            get { return deletedLinkedConnectorList; }
            set
            {
                deletedLinkedConnectorList = value;
                OnPropertyChanged("DeletedLinkedConnectorList");
            }
        }

        [DataMember]
        public Connectors SelectedLink
        {
            get { return selectedLink; }
            set
            {
                selectedLink = value;
                OnPropertyChanged("SelectedLink");
            }
        }

        [DataMember]
        public ObservableCollection<ConnectorSearch> ConnectorSearchList
        {
            get { return connectorSearchList; }
            set
            {
                connectorSearchList = value;
                OnPropertyChanged("ConnectorSearchList");
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            ConnectorSearch Details = (ConnectorSearch)this.MemberwiseClone();

            if (ReferencesList != null)
                Details.ReferencesList = new ObservableCollection<ConnectorReference>(ReferencesList.Select(x => (ConnectorReference)x.Clone()).ToList());

            if (ComponentsList != null)
                Details.ComponentsList = new ObservableCollection<ConnectorComponents>(ComponentsList.Select(x => (ConnectorComponents)x.Clone()).ToList());

            if (ConnectorAttachementFilesList != null)
                Details.ConnectorAttachementFilesList = new ObservableCollection<ConnectorAttachements>(ConnectorAttachementFilesList.Select(x => (ConnectorAttachements)x.Clone()).ToList());

            if (LocationList != null)
                Details.LocationList = new ObservableCollection<ConnectorLocation>(LocationList.Select(x => (ConnectorLocation)x.Clone()).ToList());

            //[pramod.misal][GEOS2-5754][29.08.2024]
            if (ConnectorsImageList!= null)
            {
                Details.ConnectorsImageList=new ObservableCollection<SCMConnectorImage>(ConnectorsImageList.Select(x=> (SCMConnectorImage)x.Clone()).ToList());
            }

            return Details;
        }

        #endregion
    }
}
