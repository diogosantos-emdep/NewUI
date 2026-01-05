using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Emdep.Geos.UI.Helper
{
    public class DTDColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate TemplateName { get; set; }
        public DataTemplate Stages { get; set; }

        public DataTemplate Total { get; set; }

        public DataTemplate DTDDelete { get; set; }
        public DataTemplate Error { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ColumnItem ci = item as ColumnItem;
            if (ci != null)
            {
                if (ci.ColumnFieldName.Contains("Stages_"))
                {
                    return Stages;
                }
                else if (ci.ColumnFieldName == "Template")
                {
                    return TemplateName;
                }
                else if (ci.ColumnFieldName == "Total")
                {
                    return DefaultTemplate;
                }
                else if (ci.ColumnFieldName == "DTDDelete")
                {
                    return DTDDelete;
                }
                else if (ci.ColumnFieldName == "Error")
                {
                    return Error;
                }

            }

            return base.SelectTemplate(item, container);
        }

        public enum DTDSettingType
        {
            DefaultTemplate, TemplateName, Stages, Total , DTDDelete, Error
        }

    }

    
}
