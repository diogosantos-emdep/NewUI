using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
    public class CustomFilter : ICustomFunctionOperator
    {
        /// <summary>
        /// [000][SP-66][skale][27-06-2019][GEOS2-1589]THRM - Bug: Employees List filter doesn't work
        /// this method create for custom filter
        /// </summary>
        /// <param name="operands"></param>
        /// <returns></returns>
        public object Evaluate(params object[] operands)
        {
            var collection = (IList<string>)operands[0];
            if (collection == null)
                return false;
            var item = (string)operands[1];
            return collection.Contains(item);
        }

        public Type ResultType(params Type[] operands)
        {
            //throw new NotImplementedException();
            return typeof(bool);
        }

        public string Name => "CustomFilter";
    }
}
