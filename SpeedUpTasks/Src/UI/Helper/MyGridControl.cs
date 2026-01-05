using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class MyGridControl : GridControl
    {
        public static readonly DependencyProperty BandsSourceProperty =
            DependencyProperty.Register("BandsSource", typeof(IList), typeof(MyGridControl), new PropertyMetadata(null, OnBandedSourcePropertyChanged));
        public static readonly DependencyProperty BandTemplateProperty =
            DependencyProperty.Register("BandTemplate", typeof(DataTemplate), typeof(MyGridControl), new PropertyMetadata(null));

        public IList BandsSource
        {
            get { return (IList)GetValue(BandsSourceProperty); }
            set { SetValue(BandsSourceProperty, value); }
        }

        public DataTemplate BandTemplate
        {
            get { return (DataTemplate)GetValue(BandTemplateProperty); }
            set { SetValue(BandTemplateProperty, value); }
        }

        private static void OnBandedSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MyGridControl)d).OnBandedSourceChanged(e);
        }

        private void OnBandedSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Bands.Clear();
            if (BandsSource != null)
            {
                foreach (var b in BandsSource)
                {
                    ContentControl cc = BandTemplate.LoadContent() as ContentControl;
                    if (cc == null)
                        continue;
                    GridControlBand band = cc.Content as GridControlBand;
                    cc.Content = null;
                    if (band == null)
                        continue;
                    band.DataContext = b;

                    //if (b is BandItem)
                    //    band.Name = ((BandItem)b).BandName;
                    Bands.Add(band);
                }
            }
        }
    }
}
