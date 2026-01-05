using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Modules.OTM.CommonClass
{
    /// <summary>
    /// [001][pramod.misal][GEOS2-6520][8.10.2024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
    /// </summary>
    ///
    /// <returns></returns>
    public class PO : ModelBase, INotifyPropertyChanged
    {
        #region Fields
        string header;
        ObservableCollection<PORequestDetails> pORequestDetailsList;
        ObservableCollection<PORegisteredDetails> poRegisteredList;
        DateTime startDate;
        DateTime endDate;
        string fromDate;
        string toDate;
        List<object> selectedPlantList;
        private ObservableCollection<TileBarFilters> _filterStatusListOfTile;
        private TileBarFilters _selectedTileBarItem;
        Visibility isVisiblePOReq;
        Visibility isVisiblePORegistred;
        //private Visibility isPOTileBarVisible;
        private ObservableCollection<TileBarFilters> _filterTypeListOfTile;
        private TileBarFilters _selectedTypeTileBarItem;
        #endregion


        #region Constructor
        public PO()
        {

        }
        #endregion

        #region Properties

        public Visibility ISVisiblePOReq
        {
            get { return isVisiblePOReq; }
            set
            {
                isVisiblePOReq = value;
                OnPropertyChanged("ISVisiblePOReq");
            }
        }
        public Visibility ISVisiblePORegistered
        {
            get { return isVisiblePORegistred; }
            set
            {
                isVisiblePORegistred = value;
                OnPropertyChanged("ISVisiblePORegistered");
            }
        }
        //public Visibility IsPOTileBarVisible
        //{
        //    get
        //    {
        //        return isPOTileBarVisible;
        //    }

        //    set
        //    {
        //        isPOTileBarVisible = value;
        //        OnPropertyChanged("IsPOTileBarVisible");
        //    }
        //}

        public ObservableCollection<TileBarFilters> FilterStatusListOfTiles
        {
            get { return _filterStatusListOfTile; }
            set
            {
                _filterStatusListOfTile = value;
                OnPropertyChanged("FilterStatusListOfTiles");
            }
        }


        public TileBarFilters SelectedTileBarItems
        {
            get { return _selectedTileBarItem; }
            set
            {
                _selectedTileBarItem = value;
                OnPropertyChanged("SelectedTileBarItems");
            }
        }

        public string FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        string toDatePOreg;
        string fromDatePOreg;
        public string FromDatePOreg
        {
            get
            {
                return fromDatePOreg; ;
            }

            set
            {
                fromDatePOreg = value;
                OnPropertyChanged("FromDatePOreg");
            }
        }
        public string ToDatePOreg
        {
            get
            {
                return toDatePOreg;
            }

            set
            {
                toDatePOreg = value;
                OnPropertyChanged("ToDatePOreg");
            }
        }


        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }


        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

       
        public List<object> SelectedPlantList
        {
            get { return selectedPlantList; }
            set
            {
                selectedPlantList = value;
                OnPropertyChanged("SelectedPlantList");
            }
        }
        
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

       
        public ObservableCollection<PORequestDetails> PORequestDetailsList
        {
            get { return pORequestDetailsList; }
            set
            {
                pORequestDetailsList = value;
                OnPropertyChanged("PORequestDetailsList");
            }
        }
        public ObservableCollection<PORegisteredDetails> PORegisteredDetailsList
        {
            get { return poRegisteredList; }
            set
            {
                poRegisteredList = value;
                OnPropertyChanged("PORegisteredDetailsList");
            }
        }

        public ObservableCollection<TileBarFilters> FilterTypeListOfTiles
        {
            get { return _filterTypeListOfTile; }
            set
            {
                _filterTypeListOfTile = value;
                OnPropertyChanged("FilterTypeListOfTiles");
            }
        }


        public TileBarFilters SelectedTypeTileBarItems
        {
            get { return _selectedTypeTileBarItem; }
            set
            {
                _selectedTypeTileBarItem = value;
                OnPropertyChanged("SelectedTypeTileBarItems");
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
