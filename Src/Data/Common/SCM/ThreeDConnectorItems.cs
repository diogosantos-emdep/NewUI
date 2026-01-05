using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.jangra][GEOS2-5205]
    [DataContract]
    public class ThreeDConnectorItems : ModelBase, IDisposable
    {
        #region Fields
        Int64 idConnector;
        string emdepReference;
        string reference;
        Int64 idConnectorImage;
        string savedFileName;
        string connectorsImagePath;
        Int64 idFamily;
        string familyName;
        Int64 idConnectorSubFamily;
        string subFamilyName;
        DateTime creationDate;
        UInt32 idCompany;
        string plantName;
        UInt32 idOT;
        string oTCode;
        UInt32 itemNumber;
        UInt32 idItemOTStatus;
        string itemStatus;
        UInt32? idDrawing;
        string revision;
        int reworks;
        string reworkComment;
        UInt32 idType;
        string typeName;
        UInt32 idPerson;
        string designerFullName;
        UInt32 idStage;
        string reworkStage;
        DateTime? drawingCreatedIn;

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
        public Int64 IdConnectorsSubFamily
        {
            get { return idConnectorSubFamily; }
            set
            {
                idConnectorSubFamily = value;
                OnPropertyChanged("IdConnectorsSubFamily");
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
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
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
        public UInt32 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [NotMapped]
        [DataMember]
        public string OTCode
        {
            get { return oTCode; }
            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 ItemNumber
        {
            get { return itemNumber; }
            set
            {
                itemNumber = value;
                OnPropertyChanged("ItemNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdItemOTStatus
        {
            get { return idItemOTStatus; }
            set
            {
                idItemOTStatus = value;
                OnPropertyChanged("IdItemOTStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public string ItemStatus
        {
            get { return itemStatus; }
            set
            {
                itemStatus = value;
                OnPropertyChanged("ItemStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32? IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }

        [NotMapped]
        [DataMember]
        public string Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                OnPropertyChanged("Revision");
            }
        }

        [NotMapped]
        [DataMember]
        public int Reworks
        {
            get { return reworks; }
            set
            {
                reworks = value;
                OnPropertyChanged("Reworks");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReworkComment
        {
            get { return reworkComment; }
            set
            {
                reworkComment = value;
                OnPropertyChanged("ReworkComment");
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
        public UInt32 IdPerson
        {
            get { return idPerson; }
            set
            {
                idPerson = value;
                OnPropertyChanged("IdPerson");
            }
        }

        [NotMapped]
        [DataMember]
        public string DesignerFullName
        {
            get { return designerFullName; }
            set
            {
                designerFullName = value;
                OnPropertyChanged("DesignerFullName");
            }
        }

        [NotMapped]
        [DataMember]
        public UInt32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [NotMapped]
        [DataMember]
        public string ReworkStage
        {
            get { return reworkStage; }
            set
            {
                reworkStage = value;
                OnPropertyChanged("ReworkStage");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? DrawingCreatedIn
        {
            get { return drawingCreatedIn; }
            set
            {
                drawingCreatedIn = value;
                OnPropertyChanged("DrawingCreatedIn");
            }
        }
        #endregion

        #region Constructor
        public ThreeDConnectorItems()
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
