using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.EditStrategy;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.UI.Helper
{
    public class MyComboBoxEditModify : ComboBoxEditStrategy
    {
        public MyComboBoxEditModify(ComboBoxEdit editor) : base(editor) {

        }
        protected override bool IsEnabledContainer(int controllerIndex)
        {
            var item = base.ItemsProvider.GetItemByControllerIndex(controllerIndex, null);
            if (item is GeosStatus)
                return ((GeosStatus)item).IsEnabled;
            return base.IsEnabledContainer(controllerIndex);
        }
    }

    public class MyComboBoxEdit : ComboBoxEdit
    {
        protected override EditStrategyBase CreateEditStrategy()
        {
            return new MyComboBoxEditModify(this);
        }
    }
}
