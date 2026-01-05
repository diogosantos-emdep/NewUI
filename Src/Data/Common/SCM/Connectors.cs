using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Connectors : ModelBase, IDisposable
    {
        #region Fields  
        private ConnectorSubFamily selectedSubFamily;
        bool isSealed;
        bool isUnSealed;
        List<SCMConnectorImage> connectorImageList;
        List<ConnectorComponents> connectorComponentslist;
        List<ConnectorReference> connectorReferenceList;
        List<Connectors> linkedConnectorList;
        List<ConnectorLocation> locationList;
        List<ConnectorAttachements> attachementFilesList;
        List<ConnectorLogEntry> changeLogList;
        List<ConnectorLogEntry> commentsList;
        List<Components> componentlist;
        private string location;
        private string status;
        private string color;
        private string fileName;
        byte[] connectorsImageInBytes;
        private string subFamily;
        private string family;
        string connectorsImagePath;
        private string description;
        private ConnectorSubFamily selectedSubfamily;
        private string terminal;
        bool? isSealingEnabled;
        string customValue;
        string width;
        string length;
        string height;
        string diameterExternal;
        string ways;
        private Int32 duplicated;
        private Company company;
        private Family selectedfamily;
        string diameterInternal;
        Epc.LookupValue selectedShape;
        Color selectedColor;
        Gender selectedGender;
        private Int64 idConnector;
        private Int32 numWays;
        private string refe;
        string htmlColor;
        int componentIdType;
        ushort componentIdColor;
        private string componentRef;
        string connectorType;     
        string componentTypeName;
        List<Data.Common.SCM.ValueType> selectedValueType;
        List<Company> selectedCompanyList;
        List<Color> selectedColorList;
        List<Gender> selectedGenderList;
        List<Family> selectedfamilyList;
        List<ConnectorSubFamily> selectedSubfamilyList;
        List<Company> companyList;
        ConnectorProperties waysProp;
        ConnectorProperties diameterExternalProp;
        ConnectorProperties diameterInternalProp;
        ConnectorProperties heightProp;
        ConnectorProperties lengthProp;
        ConnectorProperties widthProp;
        private Int64 idFamily;
        private Int32 movable;
        private double internalDiameter;
        private double externalDiameter;
        private double sheight;
        private double slength;
        private double sWidth;
        private Int32 idColor;
        private Int32 idType;
        private Int32 idStatus;
        private Int32 idSubFamily;
        private Int32 idGender;
        private string imagesUrl;
        private string linkdTypeName;
        private int idLinkdType;
        bool isDelVisible;
        private Int32 creatorId;
        int idLinkedConnector;
        ConnectorWorkflowStatus selectedReferenceStatus;
        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        private bool showDeleted;
        #endregion

        #region Properties
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

        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string ComponentTypeName
        {
            get { return componentTypeName; }
            set
            {
                componentTypeName = value;
                OnPropertyChanged("ComponentTypeName");
            }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        [DataMember]
        public int ComponentIdType
        {
            get { return componentIdType; }
            set
            {
                componentIdType = value;
                OnPropertyChanged("ComponentIdType");
            }
        }

        [DataMember]
        public int IdGender
        {
            get { return idGender; }
            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }
        [DataMember]
        public ushort ComponentIdColor
        {
            get { return componentIdColor; }
            set { componentIdColor = value; OnPropertyChanged("ComponentIdColor"); }
        }
        [DataMember]
        public string ComponentRef
        {
            get { return componentRef; }
            set { componentRef = value; OnPropertyChanged("ComponentRef"); }
        }

        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }

        [DataMember]
        public string Ref
        {
            get { return refe; }
            set { refe = value; OnPropertyChanged("Ref"); }
        }

        [DataMember]
        public Int32 NumWays
        {
            get { return numWays; }
            set { numWays = value; OnPropertyChanged("NumWays"); }
        }

      
        [DataMember]
        public Int32 IdColor
        {
            get { return idColor; }
            set { idColor = value; OnPropertyChanged("IdColor"); }
        }

      
        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set { idStatus = value; OnPropertyChanged("IdStatus"); }
        }

        [DataMember]
        public Int32 IdSubFamily
        {
            get { return idSubFamily; }
            set { idSubFamily = value; OnPropertyChanged("IdSubFamily"); }
        }

        [DataMember]
        public Int32 IdType
        {
            get { return idType; }
            set { idType = value; OnPropertyChanged("IdType"); }
        }
        [DataMember]
        public string Family
        {
            get { return family; }
            set { family = value; OnPropertyChanged("Family"); }
        }

        [DataMember]
        public string SubFamily
        {
            get { return subFamily; }
            set { subFamily = value; OnPropertyChanged("SubFamily"); }
        }

        [DataMember]
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged("Color"); }
        }

        [DataMember]
        public string Terminal
        {
            get { return terminal; }
            set { terminal = value; OnPropertyChanged("Terminal"); }
        }

        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }

        [DataMember]
        public byte[] ConnectorsImageInBytes
        {
            get
            {
                return connectorsImageInBytes;
            }
            set
            {
                connectorsImageInBytes = value;
                OnPropertyChanged("ConnectorsImageInBytes");
            }
        }

        [DataMember]
        public string ConnectorsImagePath
        {
            get
            {
                return connectorsImagePath;
            }
            set
            {
                connectorsImagePath = value;
                OnPropertyChanged("ConnectorsImagePath");
            }
        }

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        [DataMember]
        public Int32 Duplicated
        {
            get { return duplicated; }
            set { duplicated = value; OnPropertyChanged("Duplicated"); }
        }

        [DataMember]
        public Company SelectedCompany
        {
            get { return company; }
            set { company = value; OnPropertyChanged("SelectedCompany"); }
        }

        [DataMember]
        public Family SelectedFamily
        {
            get { return selectedfamily; }
            set
            {
                selectedfamily = value;
                OnPropertyChanged("SelectedFamily");
            }
        }

        [DataMember]
        public ConnectorSubFamily SelectedSubfamily
        {
            get { return selectedSubfamily; }
            set
            {
                selectedSubfamily = value;
                OnPropertyChanged("SelectedSubfamily");
            }
        }

    

        [DataMember]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
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
        public Epc.LookupValue SelectedShape
        {
            get { return selectedShape; }
            set
            {
                selectedShape = value;
                OnPropertyChanged("SelectedShape");
            }
        }

        [DataMember]
        public string Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged("Ways");
            }
        }

        [DataMember]
        public string DiameterInternal
        {
            get { return diameterInternal; }
            set
            {
                diameterInternal = value;
                OnPropertyChanged("DiameterInternal");
            }
        }

        [DataMember]
        public string DiameterExternal
        {
            get { return diameterExternal; }
            set
            {
                diameterExternal = value;
                OnPropertyChanged("DiameterExternal");
            }
        }

        [DataMember]
        public string Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        [DataMember]
        public string Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        [DataMember]
        public string Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [DataMember]
        public bool? IsSealingEnabled
        {
            get { return isSealingEnabled; }
            set
            {
                isSealingEnabled = value;
                OnPropertyChanged("IsSealingEnabled");
            }
        }

        [DataMember]
        public List<Data.Common.SCM.ValueType> SelectedValueType
        {
            get { return selectedValueType; }
            set
            {
                selectedValueType = value;
                OnPropertyChanged("SelectedValueType");
            }
        }

        [DataMember]
        public string CustomValue
        {
            get { return customValue; }
            set
            {
                customValue = value;
                OnPropertyChanged("CustomValue");
            }
        }

        [DataMember]
        public string ConnectorType
        {
            get
            {
                return connectorType;
            }

            set
            {
                connectorType = value;
                OnPropertyChanged("ConnectorType");
            }
        }
        #region [GEOS2-5295][rdixit][29.02.2024]
        [DataMember]
        public List<Company> SelectedCompanyList
        {
            get { return selectedCompanyList; }
            set { selectedCompanyList = value; OnPropertyChanged("SelectedCompanyList"); }
        }

        [DataMember]
        public List<Family> SelectedFamilyList
        {
            get { return selectedfamilyList; }
            set
            {
                selectedfamilyList = value;
                OnPropertyChanged("SelectedFamilyList");
            }
        }

        [DataMember]
        public List<ConnectorSubFamily> SelectedSubfamilyList
        {
            get { return selectedSubfamilyList; }
            set
            {
                selectedSubfamilyList = value;
                OnPropertyChanged("SelectedSubfamilyList");
            }
        }

        [DataMember]
        public List<Color> SelectedColorList
        {
            get { return selectedColorList; }
            set
            {
                selectedColorList = value;
                OnPropertyChanged("SelectedColorList");
            }
        }

        [DataMember]
        public List<Gender> SelectedGenderList
        {
            get { return selectedGenderList; }
            set
            {
                selectedGenderList = value;
                OnPropertyChanged("SelectedGenderList");
            }
        }

        [DataMember]
        public ConnectorProperties WaysProp
        {
            get { return waysProp; }
            set
            {
                waysProp = value;
                OnPropertyChanged("WaysProp");
            }
        }

        [DataMember]
        public ConnectorProperties DiameterInternalProp
        {
            get { return diameterInternalProp; }
            set
            {
                diameterInternalProp = value;
                OnPropertyChanged("DiameterInternalProp");
            }
        }

        [DataMember]
        public ConnectorProperties DiameterExternalProp
        {
            get { return diameterExternalProp; }
            set
            {
                diameterExternalProp = value;
                OnPropertyChanged("DiameterExternalProp");
            }
        }

        [DataMember]
        public ConnectorProperties HeightProp
        {
            get { return heightProp; }
            set
            {
                heightProp = value;
                OnPropertyChanged("HeightProp");
            }
        }

        [DataMember]
        public ConnectorProperties LengthProp
        {
            get { return lengthProp; }
            set
            {
                lengthProp = value;
                OnPropertyChanged("LengthProp");
            }
        }

        [DataMember]
        public ConnectorProperties WidthProp
        {
            get { return widthProp; }
            set
            {
                widthProp = value;
                OnPropertyChanged("WidthProp");
            }
        }
        int sealingSearch;
        [DataMember]
        public int SealingSearch
        {
            get { return sealingSearch; }
            set
            {
                sealingSearch = value;
                OnPropertyChanged("SealingSearch");
            }
        }

        [DataMember]
        public Int64 IdFamily
        {
            get { return idFamily; }
            set { idFamily = value; OnPropertyChanged("IdFamily"); }
        }

        [DataMember]
        public Int32 Movable
        {
            get { return movable; }
            set { movable = value; OnPropertyChanged("Movable"); }
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
        public double Sheight
        {
            get { return sheight; }
            set
            {
                sheight = value;
                OnPropertyChanged("Sheight");
            }
        }

        [DataMember]
        public double Slength
        {
            get { return slength; }
            set
            {
                slength = value;
                OnPropertyChanged("Slength");
            }
        }

        [DataMember]
        public double SWidth
        {
            get { return sWidth; }
            set
            {
                sWidth = value;
                OnPropertyChanged("SWidth");
            }
        }

        [DataMember]
        public string LinkdTypeName
        {
            get
            {
                return linkdTypeName;
            }
            set
            {
                linkdTypeName = value;
                OnPropertyChanged("LinkdTypeName");
            }
        }

        [DataMember]
        public string ImagesUrl
        {
            get { return imagesUrl; }
            set
            {
                imagesUrl = value;
                OnPropertyChanged("ImagesUrl");
            }
        }
        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set { isDelVisible = value; OnPropertyChanged("IsDelVisible"); }
        }

        [DataMember]
        public Int32 CreatorId
        {
            get { return creatorId; }
            set { creatorId = value; OnPropertyChanged("CreatorId"); }
        }

        [DataMember]
        public int IdLinkdType
        {
            get { return idLinkdType; }
            set { idLinkdType = value; OnPropertyChanged("IdLinkdType"); }
        }

        [DataMember]
        public int IdLinkedConnector
        {
            get { return idLinkedConnector; }
            set { idLinkedConnector = value; OnPropertyChanged("IdLinkedConnector"); }
        }
        #endregion

        //[rdixit][GEOS2-5802][05.09.2024]
        [DataMember]
        public List<Components> Componentlist
        {
            get { return componentlist; }
            set
            {
                componentlist = value;
                OnPropertyChanged("Componentlist");
            }
        }

        //[rdixit][18.09.2025][GEOS2-]
        [DataMember]
        public List<ConnectorReference> ConnectorReferenceList
        {
            get { return connectorReferenceList; }
            set
            {
                connectorReferenceList = value;
                OnPropertyChanged("ConnectorReferenceList");
            }
        }

        [DataMember]
        public List<ConnectorComponents> ConnectorComponentslist
        {
            get { return connectorComponentslist; }
            set
            {
                connectorComponentslist = value;
                OnPropertyChanged("ConnectorComponentslist");
            }
        }

        [DataMember]
        public List<Connectors> LinkedConnectorList
        {
            get { return linkedConnectorList; }
            set
            {
                linkedConnectorList = value;
                OnPropertyChanged("LinkedConnectorList");
            }
        }

        [DataMember]
        public List<ConnectorLocation> LocationList
        {
            get { return locationList; }
            set
            {
                locationList = value;
                OnPropertyChanged("LocationList");
            }
        }

        [DataMember]
        public List<ConnectorAttachements> AttachementFilesList
        {
            get { return attachementFilesList; }
            set
            {
                attachementFilesList = value;
                OnPropertyChanged("AttachementFilesList");
            }
        }

        [DataMember]
        public List<ConnectorLogEntry> ChangeLogList
        {
            get { return changeLogList; }
            set
            {
                changeLogList = value;
                OnPropertyChanged("ChangeLogList");
            }
        }

        [DataMember]
        public List<ConnectorLogEntry> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged("CommentsList");
            }
        }
        [DataMember]
        public List<SCMConnectorImage> ConnectorImageList
        {
            get { return connectorImageList; }
            set
            {
                connectorImageList = value;
                OnPropertyChanged("SCMConnectorImageList");
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

        //[Rahul.Gadhave][GEOS2-9556][Date:04/11/2025]
        [DataMember]
        public bool ShowDeleted
        {
            get
            {
                return showDeleted;
            }
            set
            {
                showDeleted = value;
                OnPropertyChanged("ShowDeleted");
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            //[rdixit][18.09.2025][GEOS2-8895]
            var ConnectorClone = (Connectors)this.MemberwiseClone();

            if (Componentlist != null)
                ConnectorClone.Componentlist = Componentlist.Select(x => (Components)x.Clone()).ToList();

            if (WidthProp != null)
                ConnectorClone.WidthProp = (ConnectorProperties)WidthProp.Clone();

            // Single object properties
            if (SelectedCompany != null)
                ConnectorClone.SelectedCompany = (Company)SelectedCompany.Clone();

            if (SelectedFamily != null)
                ConnectorClone.SelectedFamily = (Family)SelectedFamily.Clone();

            if (SelectedSubfamily != null)
                ConnectorClone.SelectedSubfamily = (ConnectorSubFamily)SelectedSubfamily.Clone();

            if (SelectedColor != null)
                ConnectorClone.SelectedColor = (Color)SelectedColor.Clone();

            if (SelectedGender != null)
                ConnectorClone.SelectedGender = (Gender)SelectedGender.Clone();

            if (SelectedShape != null)
                ConnectorClone.SelectedShape = (Epc.LookupValue)SelectedShape.Clone();

            if (SelectedValueType != null)
                ConnectorClone.SelectedValueType = SelectedValueType.Select(x => (ValueType)x.Clone()).ToList();

            // List properties
            if (SelectedCompanyList != null)
                ConnectorClone.SelectedCompanyList = SelectedCompanyList
                    .Select(x => (Company)x.Clone()).ToList();

            if (SelectedFamilyList != null)
                ConnectorClone.SelectedFamilyList = SelectedFamilyList
                    .Select(x => (Family)x.Clone()).ToList();

            if (SelectedSubfamilyList != null)
                ConnectorClone.SelectedSubfamilyList = SelectedSubfamilyList
                    .Select(x => (ConnectorSubFamily)x.Clone()).ToList();

            if (SelectedColorList != null)
                ConnectorClone.SelectedColorList = SelectedColorList
                    .Select(x => (Color)x.Clone()).ToList();

            if (SelectedGenderList != null)
                ConnectorClone.SelectedGenderList = SelectedGenderList
                    .Select(x => (Gender)x.Clone()).ToList();

            // ConnectorProperties objects
            if (WaysProp != null)
                ConnectorClone.WaysProp = (ConnectorProperties)WaysProp.Clone();

            if (DiameterInternalProp != null)
                ConnectorClone.DiameterInternalProp = (ConnectorProperties)DiameterInternalProp.Clone();

            if (DiameterExternalProp != null)
                ConnectorClone.DiameterExternalProp = (ConnectorProperties)DiameterExternalProp.Clone();

            if (HeightProp != null)
                ConnectorClone.HeightProp = (ConnectorProperties)HeightProp.Clone();

            if (LengthProp != null)
                ConnectorClone.LengthProp = (ConnectorProperties)LengthProp.Clone();

            if (ConnectorReferenceList != null)
                ConnectorClone.ConnectorReferenceList = ConnectorReferenceList
                    .Select(x => (ConnectorReference)x.Clone())
                    .ToList();

            if (ConnectorComponentslist != null)
                ConnectorClone.ConnectorComponentslist = ConnectorComponentslist
                    .Select(x => (ConnectorComponents)x.Clone())
                    .ToList();

            if (LinkedConnectorList != null)
                ConnectorClone.LinkedConnectorList = LinkedConnectorList
                    .Select(x => (Connectors)x.Clone())
                    .ToList();

            if (LocationList != null)
                ConnectorClone.LocationList = LocationList
                    .Select(x => (ConnectorLocation)x.Clone())
                    .ToList();

            if (AttachementFilesList != null)
                ConnectorClone.AttachementFilesList = AttachementFilesList
                    .Select(x => (ConnectorAttachements)x.Clone())
                    .ToList();

            if (ChangeLogList != null)
                ConnectorClone.ChangeLogList = ChangeLogList
                    .Select(x => (ConnectorLogEntry)x.Clone())
                    .ToList();

            if (CommentsList != null)
                ConnectorClone.CommentsList = CommentsList
                    .Select(x => (ConnectorLogEntry)x.Clone())
                    .ToList();

            return ConnectorClone;
        }
    }
}
