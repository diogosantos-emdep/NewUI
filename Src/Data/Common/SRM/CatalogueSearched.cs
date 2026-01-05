using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
	//  [nsatpute][12-06-2024] GEOS2-5463
    [DataContract]
    public class CatalogueSearched : ModelBase, IDisposable
    {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Catalogue> catelogueList;
        private string header;

        [DataMember]
        public ObservableCollection<Catalogue> CatalogueList
        {
            get { return catelogueList; }
            set { catelogueList = value;
                OnPropertyChanged("CatelogueList");
            }
        }

        [DataMember]
        public string Header
        {
            get { return header; }
            set { header = value;
                OnPropertyChanged("Header");
            }
        }

        private CatalogueSearchedFilter savedFilter;
        [DataMember]
        public CatalogueSearchedFilter SavedFilter
        {
            get { return savedFilter; }
            set { savedFilter = value; OnPropertyChanged("SavedFilter"); }
        }

        #endregion


        #region Method
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
