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
    public class FlyoutContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if ((item as IEnumerable) != null)
            {
                var template = (DataTemplate)Application.Current.Resources["CollectionTemplate"];



                return template;
            }
            return base.SelectTemplate(item, container);
        }

    }
}
