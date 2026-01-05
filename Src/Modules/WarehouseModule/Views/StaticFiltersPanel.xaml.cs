using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for StaticFiltersPanel.xaml
    /// </summary>
    public partial class StaticFiltersPanel : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(StaticFiltersPanel), new PropertyMetadata(null));

        public StaticFiltersPanel()
        {
            InitializeComponent();
        }

        private void TileBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
