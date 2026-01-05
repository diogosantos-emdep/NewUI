using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Modules.PCM.ViewModels;
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

namespace Emdep.Geos.Modules.PCM.Views
{
    /// <summary>
    /// Interaction logic for ArticleGridImageView.xaml
    /// </summary>
    public partial class ArticleGridImageView : DXDialogWindow
    {
        public ArticleGridImageView()
        {
            InitializeComponent();
           
        }
        private void LayoutImages_OnLoaded(object sender, RoutedEventArgs e)
        {
            layoutImages.MaximizedElement = (FrameworkElement)layoutImages.Children[0];
        }
    }
}
