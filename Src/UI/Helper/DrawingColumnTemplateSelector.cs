using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
    public class DrawingColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate IdDrawingTemplate { get; set; }
        public DataTemplate HiddenTemplate { get; set; }
        public DataTemplate ImageTemplate { get; set; }//[Sudhir.Jangra][GEOS2-2922][23/03/2023]
        public DataTemplate NameTemplate { get; set; }
        public DataTemplate GroupTemplate { get; set; } //[nsatpute][GEOS2-8090][22.07.2025]
        public DataTemplate IsCheckedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;

            if (ci != null)
            {
                if (ci.DrawingSettings == DrawingSettingsType.Default)
                {
                    return DefaultTemplate;
                }
                else if (ci.DrawingSettings == DrawingSettingsType.IdDrawing)
                {
                    return IdDrawingTemplate;
                }
                else if (ci.DrawingSettings == DrawingSettingsType.Name)
                {
                    return NameTemplate;
                }
                else if (ci.DrawingSettings == DrawingSettingsType.Image)
                {
                    return ImageTemplate;
                }
                else if (ci.DrawingSettings == DrawingSettingsType.IsChecked)
                {
                    return IsCheckedTemplate;
                }
                else if (ci.DrawingSettings == DrawingSettingsType.IsGrouped)
                {
                    return GroupTemplate; //[nsatpute][GEOS2-8090][22.07.2025]
                }
            }
            return base.SelectTemplate(item, container);
        }
    }

    public enum DrawingSettingsType
    {
        Default, IdDrawing, Name, Image, IsChecked, IsGrouped //[nsatpute][GEOS2-8090][22.07.2025]
    }
}

