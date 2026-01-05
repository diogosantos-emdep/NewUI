using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
    [DataContract]
    public class ModuleReference : ModelBase, IDisposable
    {
        #region Fields
        private List<Detection> detections;
        private string numItem;
        private string reference;
        private string type;
        private int quantity;
        private int idConnector;
        private string connector;
        private int idRevisionItem;
        private string ecosNavigateUrl;
        private string connectorImageApiUrl;
        private byte[] connectorImageBytes;
        #endregion Properties

        #region Properties
        [DataMember]
        public string NumItem
        {
            get { return numItem; }
            set { numItem = value; OnPropertyChanged("ProjectName"); }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }

        [DataMember]
        public List<Detection> Detections
        {
            get { return detections; }
            set { detections = value; OnPropertyChanged("Detections"); }
        }

        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged("Quantity"); }
        }
        [DataMember]
        public string Connector
        {
            get { return connector; }
            set { connector = value; OnPropertyChanged("Connector"); }
        }
        [DataMember]
        public int IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }
        [DataMember]
        public int IdRevisionItem
        {
            get { return idRevisionItem; }
            set { idRevisionItem = value; OnPropertyChanged("IdRevisionItem"); }
        }
        [DataMember]
        public string EcosNavigateUrl
        {
            get { return ecosNavigateUrl; }
            set { ecosNavigateUrl = value; OnPropertyChanged("EcosNavigateUrl"); }
        }

        [DataMember]
        public string ConnectorImageApiUrl
        {
            get { return connectorImageApiUrl; }
            set { connectorImageApiUrl = value; OnPropertyChanged("ConnectorImageApiUrl"); }
        }

        [DataMember]
        public byte[] ConnectorImageBytes
        {
            get { return connectorImageBytes; }
            set { connectorImageBytes = value; OnPropertyChanged("ConnectorImageBytes"); }
        }

        #endregion

        #region Constructor
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
