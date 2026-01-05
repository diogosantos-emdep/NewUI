using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-5303]
    [DataContract]
    public class Samples : ModelBase, IDisposable
    {
        #region fields
        Int64 idConnector;
        string emdepReference;
        string reference;
        Int64 idConnectorImage;
        string savedFileName;
        Int64 idWorkflowStatus;
        WorkflowStatus status;

        Int64 idFamily;
        string familyName;
        Int64 idConnectorSubFamily;
        string subFamilyName;
        DateTime createdIn;
        DateTime? modifiedIn;
        UInt32 createdBy;
        UInt32 modifiedBY;
        string firstName;
        string lastName;
        UInt32 idCompany;
        string plantName;
        string description;
        string connectorsImagePath;
        bool isUpdatedRow;


        string statusName;
        string statusHTMLColor;
        List<WorkflowStatus> statusList;

        UInt32 idLogEntry;
        string logComments;
        UInt32 idType;
        string typeName;
        string drawingName;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set
            {
                idConnector = value;
                OnPropertyChanged("IdConnector");
            }
        }

        [NotMapped]
        [DataMember]
        public string EmdepReference
        {
            get { return emdepReference; }
            set
            {
                emdepReference = value;
                OnPropertyChanged("EmdepReference");
            }
        }

        [NotMapped]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdConnectorImage
        {
            get { return idConnectorImage; }
            set
            {
                idConnectorImage = value;
                OnPropertyChanged("IdConnectorImage");
            }
        }

        [NotMapped]
        [DataMember]
        public string SavedFileName
        {
            get { return savedFileName; }
            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdWorkflowStatus
        {
            get { return idWorkflowStatus; }
            set
            {
                idWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }
        [NotMapped]
        [DataMember]
        public WorkflowStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }



        [NotMapped]
        [DataMember]
        public Int64 IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }

        [NotMapped]
        [DataMember]
        public string FamilyName
        {
            get { return familyName; }
            set
            {
                familyName = value;
                OnPropertyChanged("FamilyName");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdConnectorSubFamily
        {
            get { return idConnectorSubFamily; }
            set
            {
                idConnectorSubFamily = value;
                OnPropertyChanged("IdConnectorSubFamily");
            }
        }

        [NotMapped]
        [DataMember]
        public string SubFamilyName
        {
            get { return subFamilyName; }
            set
            {
                subFamilyName = value;
                OnPropertyChanged("SubFamilyName");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [NotMapped]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }



        [NotMapped]
        [DataMember]
        public UInt32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [NotMapped]
        [DataMember]
        public string PlantName
        {
            get { return plantName; }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }
        }

        [NotMapped]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorsImagePath
        {
            get { return connectorsImagePath; }
            set
            {
                connectorsImagePath = value;
                OnPropertyChanged("ConnectorsImagePath");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdatedRow
        {
            get
            {
                return isUpdatedRow;
            }

            set
            {
                isUpdatedRow = value;
                OnPropertyChanged("IsUpdatedRow");
            }
        }

        [DataMember]
        public string StatusName
        {
            get { return statusName; }
            set
            {
                statusName = value;
                OnPropertyChanged("StatusName");
            }
        }

        [DataMember]
        public string StatusHTMLColor
        {
            get { return statusHTMLColor; }
            set
            {
                statusHTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
            }
        }

        [DataMember]
        public List<WorkflowStatus> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged("StatusList");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 ModifiedBy
        {
            get { return modifiedBY; }
            set
            {
                modifiedBY = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdLogEntry
        {
            get { return idLogEntry; }
            set
            {
                idLogEntry = value;
                OnPropertyChanged("IdLogEntry");
            }
        }

        [NotMapped]
        [DataMember]
        public string LogComment
        {
            get { return logComments; }
            set
            {
                logComments = value;
                OnPropertyChanged("LogComment");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [NotMapped]
        [DataMember]
        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                OnPropertyChanged("TypeName");
            }
        }

        [NotMapped]
        [DataMember]
        public string DrawingName
        {
            get { return drawingName; }
            set
            {
                drawingName = value;
                OnPropertyChanged("DrawingName");
            }
        }
        #endregion

        #region Constructor
        public Samples()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
