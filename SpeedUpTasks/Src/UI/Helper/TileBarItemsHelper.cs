using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Xpf.LayoutControl;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
    public class TileBarItemsHelper : ViewModelBase, INotifyPropertyChanged
    {
        //For TileBar
        public string Caption { get; set; }
        public object Tag { get; set; }
        public String GlyphUri { get; set; }
        public string BackColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Id { get; set; }
        public ObservableCollection<TileBarItemsHelper> Children { get; set; }
        public ICommand NavigateCommand { get; set; }
        public bool IsHasChildren { get { return Children != null && Children.Count != 0; } }

        private Visibility visibility;
        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visibility"));
            }
        }

        public TileBarItemsHelper()
        {
            Tag = null;
            Caption = String.Empty;
            Children = new ObservableCollection<TileBarItemsHelper>();
            NavigateCommand = null;
            GlyphUri = String.Empty;
            Height = 65;
            Width = 170;
            Id = 0;

        }

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
    /// <summary>
    /// This Class is using for Layout Groupbox  Maximization and Minimization
    /// </summary>
    public class MyLayoutControl : LayoutControl, IMaximizingContainer
    {
        FrameworkElement IMaximizingContainer.MaximizedElement { get; set; }
    }

}
