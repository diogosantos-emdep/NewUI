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
using System.Globalization;
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.Modules.HarnessPart.Views
{
    /// <summary>
    /// Interaction logic for ucCarLayout.xaml
    /// </summary>
    public partial class ucCarLayout : UserControl
    {
        public ucCarLayout()
        {
            InitializeComponent();
        }
        //void CarElementMouseEnter(object sender, MouseEventArgs e)
        //{
        //    CarElement = (controlCarInfo)sender;
        //    CarElementPosition = CarElement.GetPosition(layoutCars);

        //    var carDetailsElement = new controlCarInfo(true, true, true);
        //    carDetailsElement.DataContext = CarElement.DataContext;
        //    carDetailsElement.FlowDirection = CarElement.FlowDirection;
        //    carDetailsElement.FontFamily = CarElement.FontFamily;
        //    carDetailsElement.Owner = CarElement;

        //    CarDetailsPopup = new TransparentPopup();
        //    CarDetailsPopup.Child = carDetailsElement;
        //    CarDetailsPopup.PlacementTarget = layoutCars;
        //    CarDetailsPopup.IsOpen = true;
        //    carDetailsElement.UpdateLayout();
        //    if (CarDetailsPopup == null)
        //        return;
        //    Point carDetailsPopupPosition =
        //        PointHelper.Subtract(PointHelper.Add(CarElementPosition, CarElement.ContentOffset), carDetailsElement.ContentOffset);
        //    CarDetailsPopup.HorizontalOffset = carDetailsPopupPosition.X;
        //    CarDetailsPopup.VerticalOffset = carDetailsPopupPosition.Y;
        //    CarDetailsPopup.MakeVisible();

        //    Storyboard.SetTarget(CarDetailsShowingAnimation, carDetailsElement);
        //    CarDetailsShowingAnimation.Begin();
        //}
        //void CarElementMouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (CarDetailsPopup == null)
        //        return;
        //    CarDetailsShowingAnimation.Stop();

        //    CarDetailsPopup.IsOpen = false;
        //    CarDetailsPopup.Child = null;
        //    CarDetailsPopup = null;

        //    CarElement = null;
        //}
        //void OnLayoutUpdated(object sender, EventArgs e)
        //{
        //    if (CarDetailsPopup == null)
        //        return;
        //    Point newPosition = CarElement.GetPosition(layoutCars);
        //    Point offset = PointHelper.Subtract(newPosition, CarElementPosition);
        //    if (offset.X == 0 && offset.Y == 0)
        //        return;
        //    CarElementPosition = newPosition;
        //    CarDetailsPopup.HorizontalOffset += offset.X;
        //    CarDetailsPopup.VerticalOffset += offset.Y;
        //}
    }
    public class CarDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var control = (FlowLayoutControl)container;
            return (DataTemplate)control.Resources[item.GetType().Name + "DataTemplate"];
        }
    }

    public class IsFirstCarConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //foreach (Brand brand in pageCars.Brands)
            //    if (brand.Cars[0] == value)
            //        return true;
            return false;
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }




}
