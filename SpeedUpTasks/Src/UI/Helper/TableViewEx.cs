using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.UI.Helper
{
    public class TableViewEx : TableView
    {
        protected override SelectionStrategyBase CreateSelectionStrategy()
        {
            var strategy = base.CreateSelectionStrategy();
            if (strategy is SelectionStrategyCell)
                strategy = new SelectionStrategyCellEx(this);
            return strategy;
        }
    }

    public class SelectionStrategyCellEx : SelectionStrategyCell
    {
        public SelectionStrategyCellEx(TableView owner) : base(owner) { }

        public override void OnAfterMouseLeftButtonDown(IDataViewHitInfo hitInfo)
        {
            if (Keyboard.Modifiers != ModifierKeys.Alt)
                base.OnAfterMouseLeftButtonDown(hitInfo);
            else TableView.ShowEditor();
        }

        public override void ClearSelection()
        {
            base.ClearSelection();
        }
    }
}
