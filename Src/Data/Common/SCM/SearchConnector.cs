using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class SearchConnector : ModelBase, IDisposable
    {
        //[rdixit][18.09.2025][GEOS2-]
        ObservableCollection<Connectors> connectorList;
        string header;
        bool allowPaging;
        bool isTableView;
        int resultPages;
        #region Constructor
        public SearchConnector()
        {

        }
        #endregion

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
        public bool AllowPaging
        {
            get
            {
                return allowPaging;
            }

            set
            {
                allowPaging = value;
                OnPropertyChanged("AllowPaging");
            }
        }

        [DataMember]
        public bool IsTableView
        {
            get
            {
                return isTableView;
            }

            set
            {
                isTableView = value;
                OnPropertyChanged("IsTableView");
            }
        }
        [DataMember]
        public int ResultPages
        {
            get
            {
                return resultPages;
            }

            set
            {
                resultPages = value;
                OnPropertyChanged("ResultPages");
            }
        }

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            ConnectorSearch Details = (ConnectorSearch)this.MemberwiseClone();            
            return Details;
        }

        #endregion
    }
}
