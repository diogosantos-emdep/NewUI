using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class CatalogueSearchedFilter : ModelBase, IDisposable
    {

        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;

        private string reference;
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }

        private string supplierIds;
        [DataMember]
        public string SupplierIds
        {
            get { return supplierIds; }
            set { supplierIds = value; OnPropertyChanged("SupplierIds"); }
        }

        private string categoryIds;
        [DataMember]
        public string CategoryIds
        {
            get { return categoryIds; }
            set { categoryIds = value; OnPropertyChanged("CategoryIds"); }
        }

        private string conditionalOperator;
        [DataMember]
        public string ConditionalOperator
        {
            get { return conditionalOperator; }
            set { conditionalOperator = value; OnPropertyChanged("ConditionalOperator"); }
        }

        private string stockQuanitty;
        [DataMember]
        public string StockQuanitty
        {
            get { return stockQuanitty; }
            set { stockQuanitty = value; OnPropertyChanged("StockQuanitty"); }
        }


        #endregion
        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
