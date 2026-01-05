using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ConnectorGridImageView.xaml
    /// </summary>
    public partial class ConnectorGridImageView : DXDialogWindow
    {
        public ConnectorGridImageView()
        {
            InitializeComponent();
        }
        private void LayoutImages_OnLoaded(object sender, RoutedEventArgs e)
        {
            layoutImages.MaximizedElement = (FrameworkElement)layoutImages.Children[0];
        }

        //[rdixit][GEOS2-9199][11.09.2025]
        private void layoutImages_MaximizedElementChanged(object sender, ValueChangedEventArgs<FrameworkElement> e)
        {
            if (e.NewValue != null)
            {
                if (e.NewValue.DataContext is SCMConnectorImage newImage)
                    newImage.IsPictureMaximize = true;
            }

            if (e.OldValue != null)
            {
                if (e.OldValue.DataContext is SCMConnectorImage oldImage)
                    oldImage.IsPictureMaximize = false;
            }
        }

    }
}
