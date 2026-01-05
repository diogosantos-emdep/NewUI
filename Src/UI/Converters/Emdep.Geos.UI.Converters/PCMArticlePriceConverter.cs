using DevExpress.Xpf.PivotGrid.Internal;
using DevExpress.Xpf.PivotGrid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using DevExpress.Xpf.Editors.Helpers;
using System.Text.RegularExpressions;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.DataProcessing;
using DevExpress.Xpf.Grid;
using System.Data;
using DevExpress.Xpf.Data;

namespace Emdep.Geos.UI.Converters
{
    public class PCMArticlePriceConverter : MarkupExtension, IMultiValueConverter
    {
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPCMService PCMService = new PCMServiceController("localhost:6699");
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string output = null;
            try
            {
                #region Get Values
                if (values == null || values.Length != 3) return output;
                if (values[0] == null || values[1] == null || values[2] == null) return output;

                ulong articleId = 0;
                string comp = values[1].ToString();
                DataRowView rowdata = (DataRowView)values[2];

                if (values[0] is ulong)
                    articleId = (ulong)values[0];
                else
                    return output;

                if (!GeosApplication.Instance.ArticleCostPriceList.Any(i => i.Alias == comp))
                {
                    var ArticleCostPricesList = PCMService.GetArticleCostPricesByCompany_V2590(comp, GeosApplication.Instance.PCMCurrentCurrency.IdCurrency);
                    GeosApplication.Instance.ArticleCostPriceList.AddRange(ArticleCostPricesList);
                }
                var ArticleCostPrice = GeosApplication.Instance.ArticleCostPriceList.FirstOrDefault(i => i.IdArticle == articleId && i.Alias == comp);
                #endregion
                if (ArticleCostPrice != null)
                {
                    rowdata.Row[comp] = ArticleCostPrice.ArticleCostValue.ToString("F2") ?? null;
                    return ArticleCostPrice.ArticleCostValue.ToString("F2") ?? null;
                }
                else
                {
                    rowdata.Row[comp] = DBNull.Value;
                    return DBNull.Value;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Convert() Method from PCMArticlePriceConverter: " + ex.Message,
                    category: Category.Exception, priority: Priority.Low);
            }
            return output;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
