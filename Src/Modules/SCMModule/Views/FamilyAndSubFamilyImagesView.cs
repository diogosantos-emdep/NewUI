using DevExpress.Xpf.Core;
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

namespace Emdep.Geos.Modules.SCM.Views
{
    /// <summary>
    /// Interaction logic for DetectionGridImageView.xaml
    /// </summary>
    public partial class FamilyAndSubFamilyImagesView : DXDialogWindow
    {
        public FamilyAndSubFamilyImagesView()
        {
            InitializeComponent();
        }
        private void LayoutImages_OnLoaded(object sender, RoutedEventArgs e)
        {
            layoutImages.MaximizedElement = (FrameworkElement)layoutImages.Children[0];
        }

        private void layoutImages_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
