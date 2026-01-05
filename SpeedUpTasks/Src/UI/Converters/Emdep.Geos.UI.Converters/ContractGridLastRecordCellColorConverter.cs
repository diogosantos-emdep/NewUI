using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Emdep.Geos.UI.Common;
using System.Globalization;
using Emdep.Geos.Data.Common.Hrm;
using System.Collections.ObjectModel;
using System.Windows;

namespace Emdep.Geos.UI.Converters
{
    public class ContractGridLastRecordCellColorConverter : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// [001][skale][18-09-2019][GEOS2-1710] Memorize Exit Events
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //ulong IdEmployeeContractSituation = (ulong)values[0];
                //var EmployeeContractSituationList = values[1] as ObservableCollection<EmployeeContractSituation>;
                //DateTime value2 = System.Convert.ToDateTime(values[2]);


                //if (IdEmployeeContractSituation == EmployeeContractSituationList[EmployeeContractSituationList.Count-1].IdEmployeeContractSituation&&IdEmployeeContractSituation!=0)
                //{
                //    if (values[2] != System.DBNull.Value)
                //    {
                //        if ((value2 != null && ((DateTime)(value2) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                //            return true;
                //    }
                //}
                //return false;
                //[001] added
                if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                {
                    return null;
                }
                else
                {
                    DateTime Enddate = System.Convert.ToDateTime(values[1]);

                    if (values[0] != null)
                    {
                        if (values[1] != System.DBNull.Value)
                        {
                            if ((Enddate != null && ((DateTime)(Enddate.Date) < Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date)))
                                return true;
                        }
                    }

                }
               
               return false;
            }
            catch
            {
                return false;
            }

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
