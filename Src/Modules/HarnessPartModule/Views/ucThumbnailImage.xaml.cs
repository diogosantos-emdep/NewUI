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
using Emdep.Geos.Modules.HarnessPart.Class;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for uctest.xaml
    /// </summary>
    public partial class ucThumbnailImage : UserControl
    {
        public ucThumbnailImage()
        {
            InitializeComponent();
        

            var result = new List<ImageSource>();
            for (int i = 0; i <= 11; i++)
            {
                result.Add(GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con" + i + ".jpg"));
            }
            for (int k = 0; k <= 11; k++)
            {
                result.Add(GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con" + k + ".jpg"));
            }
            List<clsHarnessInfo> objclsImageInfo = new List<clsHarnessInfo>() {
                new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con0.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con1.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con2.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con3.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con4.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con5.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con6.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con7.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con8.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con9.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con10.jpg"), AllvisualAid=result },
                 new clsHarnessInfo { Harnessdescription="ABCd", Harnessname="test2", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con11.jpg"),AllvisualAid=result },
                    new clsHarnessInfo {  Harnessdescription="ABC", Harnessname="test1", VisualAid = GetImage("/Emdep.Geos.Modules.HarnessPart;component/Image/con12.jpg"), AllvisualAid=result },
                         
                
            }; this.DataContext = objclsImageInfo;


        }
        BitmapImage GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));

        }
       
    }
    public class ImageContainer : ContentControlBase
    {
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (Controller.IsMouseLeftButtonDown)
            {
                var layoutControl = Parent as FlowLayoutControl;
                if (layoutControl != null)
                {
                    Controller.IsMouseEntered = false;
                    layoutControl.MaximizedElement = layoutControl.MaximizedElement == this ? null : this;
                }
            }
        }
        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            if (!double.IsNaN(Width) && !double.IsNaN(Height))
                if (e.NewSize.Width != e.PreviousSize.Width)
                    Height = double.NaN;
                else
                    Width = double.NaN;
        }
    }
}
