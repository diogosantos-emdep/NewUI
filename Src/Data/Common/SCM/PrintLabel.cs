using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class PrintLabel : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idConnector;
        private string reference;
        private uint idCustomerType;
        private int quantity;
        private int toPrint;
        private string viewImage;
        private string location;
        private string plant;
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdConnector
        {
            get { return idConnector; }
            set { idConnector = value; OnPropertyChanged("IdConnector"); }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged("Quantity"); }
        }

        [DataMember]
        public int ToPrint
        {
            get { return toPrint; }
            set { toPrint = value; OnPropertyChanged("ToPrint"); }
        }

        [DataMember]
        public string ViewImage
        {
            get { return viewImage; }
            set { viewImage = value; OnPropertyChanged("ViewImage"); }
        }

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }

        [DataMember]
        public string Plant
        {
            get { return plant; }
            set { plant = value; OnPropertyChanged("Plant"); }
        }

        #endregion
        public override string ToString()
        {
            return Reference;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
