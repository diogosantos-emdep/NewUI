using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
    //[Sudhir.Jangra][GEOS2-5983]
    /// <summary>
    /// APM Alert Tile Bar Filters - updated with IsSelected property
    /// </summary>
    public class APMAlertTileBarFilters : ViewModelBase, INotifyPropertyChanged
    {
        private string caption;
        private string entitiesCount;
        private Visibility entitiesCountVisibility;
        private bool isSelected;

        public string Caption
        {
            get
            {
                return caption;
            }

            set
            {
                caption = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Caption"));
            }
        }
        public string EntitiesCount
        {
            get
            {
                return entitiesCount;
            }

            set
            {
                entitiesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntitiesCount"));
            }
        }
        public Visibility EntitiesCountVisibility
        {
            get
            {
                return entitiesCountVisibility;
            }

            set
            {
                entitiesCountVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntitiesCountVisibility"));
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
            }
        }

        public int Id { get; set; }
        public string DisplayText { get; set; }
        public string ImageUri { get; set; }
        public ImageSource Image { get; set; }
        public string BackColor { get; set; }
        public string ForeColor { get; set; }
        public long IdOfferStatusType { get; set; }
        public string FilterCriteria { get; set; }
        public int Height { get; set; }
        public int width { get; set; }
        public string Type { get; set; }
        public ICommand NavigateCommand { get; set; }
        public void Update(int entitiesCount)
        {
            this.EntitiesCount = entitiesCount.ToString();
            DisplayText = string.Format("{0} ({1})", Caption, entitiesCount);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
