using DevExpress.Xpf.Core.Native;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Emdep.Geos.UI.Converters
{
    public class PCMBasePriceConverter : MarkupExtension, IMultiValueConverter
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
                ulong? idBasePriceList = 0;
                string fieldName = values[1].ToString();
                DataRowView rowdata = (DataRowView)values[2];

                if (values[0] is ulong)
                    articleId = (ulong)values[0];
                else
                    return output;
                #endregion
                var match = Regex.Match(values[1].ToString(), @"^BPL_(\d+)_(\w+)$");

                if (match.Success)
                {
                    #region
                    idBasePriceList = ulong.TryParse(match.Groups[1].Value, out ulong parsedIdBasePriceList) ? parsedIdBasePriceList : (ulong?)null;
                    if (!GeosApplication.Instance.BasePriceListByItem.Any(i => i.IdBasePriceList == idBasePriceList))
                    {
                        var BasePriceList = PCMService.GetSalesPriceforArticleByIdBasePrice_V2590((ulong)idBasePriceList);
                        GeosApplication.Instance.BasePriceListByItem.AddRange(BasePriceList);
                    }
                    int? idCurrency = int.TryParse(match.Groups[2].Value, out int parsedIdCurrency) ? parsedIdCurrency : (int?)null;
                    double? MaxCost_1 = null;
                    BasePriceListByItem BPL = GeosApplication.Instance.BasePriceListByItem.FirstOrDefault(i => i.IdBasePriceList == idBasePriceList && i.IdArticle == articleId);
                    double Cost_Value = 0;
                    double? finalvalue =null;
                    #endregion

                    if (BPL != null)
                    {
                        foreach (UInt32 idSite in BPL.PlantIdList)
                        {
                            ArticleCostPrice Cost = GeosApplication.Instance.ArticleCostPriceList.FirstOrDefault(a => a.IdCompany ==idSite && a.IdArticle == articleId);
                            if (Cost == null)
                            {
                                Cost = new ArticleCostPrice();
                            }
                            if (MaxCost_1 == null)
                            {
                                MaxCost_1 = Cost.ArticleCostValue;
                                Cost_Value = Cost.ArticleCostValue;
                            }
                            if (Cost_Value < Cost.ArticleCostValue)
                            {
                                MaxCost_1 = Cost.ArticleCostValue;
                                Cost_Value = Cost.ArticleCostValue;
                            }
                        }
                        if (BPL.IdRule == 307)
                        {
                            double? ConversionRate = null;
                            CurrencyConversion CurConversion1 = GeosApplication.Instance.CurrencyConversionList.FirstOrDefault(x => x.Idcurrencyfrom == BPL.IdCurrency && x.Idcurrencyto == idCurrency &&
                            x.LastUpdate.Date == GeosApplication.Instance.CurrencyConversionList.FirstOrDefault().LastUpdate);
                            if (CurConversion1 != null)
                            {
                                if (BPL.IdExchangeRateUpdateType == 1509)
                                    ConversionRate = CurConversion1.ExchangeRate;
                                else
                                    ConversionRate = CurConversion1.LastMonthAVGRate;
                            }
                            if (BPL.IdCurrency == idCurrency)
                                finalvalue = MaxCost_1 + ((MaxCost_1 * (BPL.RuleValue) / 100));
                            if (ConversionRate != null)
                            {
                                finalvalue = BPL.RuleValue * ConversionRate;
                            }
                            else
                                finalvalue = null;

                        }
                        else if (BPL.IdRule == 308)
                        {
                            if (BPL.RuleValue == 0 || BPL.RuleValue <= 0)
                            {
                                finalvalue = null;
                            }
                            else
                            {
                                double? ConversionRate = null;
                                CurrencyConversion CurConversion1 = GeosApplication.Instance.CurrencyConversionList.FirstOrDefault(x => x.Idcurrencyfrom == BPL.IdCurrency && x.Idcurrencyto == idCurrency
                                && x.LastUpdate.Date == GeosApplication.Instance.CurrencyConversionList.FirstOrDefault().LastUpdate);
                                if (CurConversion1 != null)
                                {
                                    if (BPL.IdExchangeRateUpdateType == 1509)
                                        ConversionRate = CurConversion1.ExchangeRate;
                                    else
                                        ConversionRate = CurConversion1.LastMonthAVGRate;
                                }
                                if (ConversionRate != 0 && ConversionRate != null)
                                {
                                    finalvalue = BPL.RuleValue * ConversionRate;
                                }
                                else
                                    finalvalue = null;

                            }
                        }
                        else if (BPL.IdRule == 1518)
                        {
                            finalvalue = 0;
                        }
                        else if (BPL.IdRule == 309)
                        {
                            double? samecur = 0;
                            double? ConversionRate = null;
                            if (BPL.RuleValue == 0)
                            {
                                if (BPL.IdCurrency == idCurrency)
                                    samecur = MaxCost_1;
                            }
                            else
                                samecur = MaxCost_1 + BPL.RuleValue;

                            CurrencyConversion CurConversion1 = GeosApplication.Instance.CurrencyConversionList.FirstOrDefault(x => x.Idcurrencyfrom ==
                            BPL.IdCurrency && x.Idcurrencyto == idCurrency && x.LastUpdate.Date == GeosApplication.Instance.CurrencyConversionList.FirstOrDefault().LastUpdate);
                            if (CurConversion1 != null)
                            {
                                if (BPL.IdExchangeRateUpdateType == 1509)
                                    ConversionRate = CurConversion1.ExchangeRate;
                                else
                                    ConversionRate = CurConversion1.LastMonthAVGRate;
                            }

                            if (ConversionRate != 0 && ConversionRate != null)
                                finalvalue = samecur * ConversionRate;
                            else
                                finalvalue = null;

                        }

                        if (finalvalue != null)
                        {
                            rowdata.Row[fieldName] = finalvalue?.ToString("F2") ?? null;
                            return finalvalue?.ToString("F2") ?? null;
                        }
                        else
                        {
                            rowdata.Row[fieldName] = DBNull.Value;
                            return DBNull.Value;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Convert() Method from PCMArticlePriceConverter: " + ex.Message,
                    category: Category.Exception, priority: Priority.Low);
            }
            return output;
        }


        // The ConvertBack method isn't necessary here if you're not using two-way binding
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
