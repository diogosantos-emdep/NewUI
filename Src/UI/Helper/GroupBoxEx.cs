using DevExpress.Xpf.LayoutControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.UI.Helper
{
   public class GroupBoxEx: GroupBox
    {
        public GroupBoxEx()
        {
            StateChanged += GroupBoxEx_StateChanged;
        }

        private void GroupBoxEx_StateChanged(object sender, DevExpress.Xpf.Core.ValueChangedEventArgs<DevExpress.Xpf.LayoutControl.GroupBoxState> e)
        {
            if (e.NewValue == GroupBoxState.Normal)

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ((DevExpress.Xpf.LayoutControl.GroupBox)sender).State = GroupBoxState.Minimized;
                }));
        }
    }
}
