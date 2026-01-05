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

namespace Emdep.Geos.Modules.PCM.Views
{
    /// <summary>
    /// Interaction logic for ProductTypeGridImageView.xaml
    /// </summary>
    public partial class ProductTypeGridImageView : DXDialogWindow    
    {
        public ProductTypeGridImageView()
        {
            InitializeComponent();
        }
        private void LayoutImages_OnLoaded(object sender, RoutedEventArgs e)
        {
            layoutImages.MaximizedElement = (FrameworkElement)layoutImages.Children[0];
        }
        private void layoutImages_MaximizedElementChanged(object sender, ValueChangedEventArgs<FrameworkElement> e)
        {
            //if (e.OldValue is FrameworkElement el1) {
            //    el1.Width = 200;
            //    el1.Height = 200;
            //}
            //if (e.NewValue is FrameworkElement el2) {
            //    el2.ClearValue(WidthProperty);
            //    el2.ClearValue(HeightProperty);
            //}
        }
        //private void DialogWindow_StateChanged(object sender, EventArgs e)
        //{

        //    if (WindowState == WindowState.Maximized)
        //    {
        //        foreach (var child in layoutControl.Children)
        //        {
        //            var frameworkElement = child as FrameworkElement;
        //            if (frameworkElement != null)
        //            {
        //                frameworkElement.Height = ActualHeight;
        //            }
        //        }
        //    }
        //    //else if (WindowState==WindowState.Normal)
        //    //{
        //    //    foreach (var child in layoutControl.Children)
        //    //    {
        //    //        var frameworkElement = child as FrameworkElement;
        //    //        if (frameworkElement != null)
        //    //        {
        //    //            frameworkElement.Height = ActualHeight;
        //    //        }
        //    //    }
        //    //}
        //}

    }
}
