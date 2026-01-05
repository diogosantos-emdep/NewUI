using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
   
    [DataContract]
    public class CPProduct : ModelBase, IDisposable
    {
        #region Fields

        string reference;
        string otherReference;       
        string connectorImagePath;
        string connectorEmdepReference;
        byte[] connectorBytes;
        byte idCPType;
        string productTypeName;
        List<CPDetection> lstCPDetection;
        long idCPTypenew;
        string samples;
        string zone;
        string drawingType;    
        int idCP;
        int ways;
        int? idDrawing;
        bool? fullMatchWithDrawing;
        DateTime? sampleReceivedIn;
        int? idDrawingAssignedBy;
        int? idWorkbookOfCpProducts;
        int? idConnector;
        bool notStandard;
        int idAutomaticDrawing;
        byte? idStatusEDS;
        int? idMandatoryRevision;
        byte idEDSStatusBeforeWorkshop;
        int? createdBy;
        int? modifiedBy;
        bool showMessageIn3D;
        #endregion

        #region Constructor
        public CPProduct()
        {
        }

        #endregion

        #region Properties

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public string OtherReference
        {
            get
            {
                return otherReference;
            }

            set
            {
                otherReference = value;
                OnPropertyChanged("OtherReference");
            }
        }


        [NotMapped]
        [DataMember]
        public byte[] ConnectorBytes
        {
            get
            {
                return connectorBytes;
            }

            set
            {
                connectorBytes = value;
                OnPropertyChanged("connectorBytes");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorImagePath
        {
            get
            {
                return connectorImagePath;
            }

            set
            {
                connectorImagePath = value;
                OnPropertyChanged("ConnectorImagePath");
            }
        }

        [NotMapped]
        [DataMember]
        public string ConnectorEmdepReference
        {
            get
            {
                return connectorEmdepReference;
            }

            set
            {
                connectorEmdepReference = value;
                OnPropertyChanged("ConnectorEmdepReference");
            }
        }

     
        [NotMapped]
        [DataMember]
        public byte IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }


        [NotMapped]
        [DataMember]
        public string ProductTypeName
        {
            get
            {
                return productTypeName;
            }

            set
            {
                productTypeName = value;
                OnPropertyChanged("ProductTypeName");
            }
        }

        [NotMapped]
        [DataMember]
        public List<CPDetection> LstCPDetection
        {
            get
            {
                return lstCPDetection;
            }

            set
            {
                lstCPDetection = value;
                OnPropertyChanged("LstCPDetection");
            }
        }

        [NotMapped]
        [DataMember]
        public long IdCPTypeNew
        {
            get
            {
                return idCPTypenew;
            }

            set
            {
                idCPTypenew = value;
                OnPropertyChanged("IdCPTypeNew");
            }
        }



        [NotMapped]
        [DataMember]
        public int IdCP
        {
            get { return idCP; }
            set
            {
                idCP = value;
                OnPropertyChanged("IdCP");
            }
        }

        [NotMapped]
        [DataMember]
        public string Samples
        {
            get { return samples; }
            set
            {
                samples = value;
                OnPropertyChanged("Samples");
            }
        }

        [NotMapped]
        [DataMember]
        public int Ways
        {
            get { return ways; }
            set
            {
                ways = value;
                OnPropertyChanged("Ways");
            }
        }

        [NotMapped]
        [DataMember]
        public int? IdDrawing
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
        public string Zone
        {
            get { return zone; }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }

        [NotMapped]
        [DataMember]
        public bool? FullMatchWithDrawing
        {
            get { return fullMatchWithDrawing; }
            set
            {
                fullMatchWithDrawing = value;
                OnPropertyChanged("FullMatchWithDrawing");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime? SampleReceivedIn
        {
            get { return sampleReceivedIn; }
            set
            {
                sampleReceivedIn = value;
                OnPropertyChanged("SampleReceivedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public int? IdDrawingAssignedBy
        {
            get { return idDrawingAssignedBy; }
            set
            {
                idDrawingAssignedBy = value;
                OnPropertyChanged("IdDrawingAssignedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public int? IdWorkbookOfCpProducts
        {
            get { return idWorkbookOfCpProducts; }
            set
            {
                idWorkbookOfCpProducts = value;
                OnPropertyChanged("IdWorkbookOfCpProducts");
            }
        }

        [NotMapped]
        [DataMember]
        public int? IdConnector
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
        public string DrawingType
        {
            get { return drawingType; }
            set
            {
                drawingType = value;
                OnPropertyChanged("DrawingType");
            }
        }

        [NotMapped]
        [DataMember]
        public bool NotStandard
        {
            get { return notStandard; }
            set
            {
                notStandard = value;
                OnPropertyChanged("NotStandard");
            }
        }

        [NotMapped]
        [DataMember]
        public int IdAutomaticDrawing
        {
            get { return idAutomaticDrawing; }
            set
            {
                idAutomaticDrawing = value;
                OnPropertyChanged("IdAutomaticDrawing");
            }
        }

        [NotMapped]
        [DataMember]
        public byte? IdStatusEDS
        {
            get { return idStatusEDS; }
            set
            {
                idStatusEDS = value;
                OnPropertyChanged("IdStatusEDS");
            }
        }

        [NotMapped]
        [DataMember]
        public int? IdMandatoryRevision
        {
            get { return idMandatoryRevision; }
            set
            {
                idMandatoryRevision = value;
                OnPropertyChanged("IdMandatoryRevision");
            }
        }

        [NotMapped]
        [DataMember]
        public byte IdEDSStatusBeforeWorkshop
        {
            get { return idEDSStatusBeforeWorkshop; }
            set
            {
                idEDSStatusBeforeWorkshop = value;
                OnPropertyChanged("IdEDSStatusBeforeWorkshop");
            }
        }

        [NotMapped]
        [DataMember]
        public int? CreatedBy
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
        public int? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public bool ShowMessageIn3D
        {
            get { return showMessageIn3D; }
            set
            {
                showMessageIn3D = value;
                OnPropertyChanged("ShowMessageIn3D");
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
            return this.MemberwiseClone();
        }

        #endregion
    }
}
