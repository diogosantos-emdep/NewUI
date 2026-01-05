using DevExpress.XtraSpreadsheet.API.Native.Implementation;
using DevExpress.XtraSpreadsheet.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Hrm.CommonClass
{
    public static class RangeDataSourceExtension
    {
        public static List<string> ToColList(this RangeDataSource dataSource)
        {
            PropertyDescriptorCollection props = dataSource.GetItemProperties(null);
            List<string> columnList = new List<string>();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                columnList.Add(prop.DisplayName);
            }
            return columnList;
        }
        public static List<object> ToDataList(this RangeDataSource dataSource)
        {
            PropertyDescriptorCollection props = dataSource.GetItemProperties(null);
            object[] values = new object[props.Count];
            List<object> dataList = new List<object>();
            foreach (RangeDataRow dataItem in dataSource)
            {
                dynamic item = new ExpandoObject();
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    ((IDictionary<String, Object>)item).Add(prop.Name, props[i].GetValue(dataItem));
                }
                dataList.Add(item);
            }
            return dataList;
        }
    }
}
