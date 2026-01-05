using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.UI.Helper
{
   public class GridRowTemplateSelector: DependencyObject, INotifyPropertyChanged
    {
        #region public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        public DataTemplate DetailTemplate
        {
            get { return (DataTemplate)GetValue(DetailTemplateProperty); }
            set {
                SetValue(DetailTemplateProperty, value);
                OnPropertyChanged(new PropertyChangedEventArgs("DetailTemplate"));
            }
        }
        public static readonly DependencyProperty DetailTemplateProperty =
            DependencyProperty.Register("DetailTemplate", typeof(DataTemplate), typeof(GridRowTemplateSelector), new PropertyMetadata(null));

        public Style RowStyle
        {
            get { return (Style)GetValue(RowStyleProperty); }
            set { SetValue(RowStyleProperty, value);
                OnPropertyChanged(new PropertyChangedEventArgs("RowStyle"));
            }
        }

        public static readonly DependencyProperty RowStyleProperty =
            DependencyProperty.Register("RowStyle", typeof(Style), typeof(GridRowTemplateSelector), new PropertyMetadata(null));

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value);
                OnPropertyChanged(new PropertyChangedEventArgs("DisplayName"));
            }
        }
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(GridRowTemplateSelector), new PropertyMetadata(null));
    }
}
