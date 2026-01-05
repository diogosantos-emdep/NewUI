using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.LayoutControl;

namespace Emdep.Geos.UI.Helper
{
    public class FlowLayoutControlEx : FlowLayoutControl
    {
        protected override LayoutProviderBase CreateLayoutProvider()
        {
            return new FlowLayoutProviderEx(this);
        }
    }

    public class FlowLayoutProviderEx : FlowLayoutProvider
    {
        public FlowLayoutProviderEx(IFlowLayoutModel model) : base(model) { }

        protected override Size OnArrange(FrameworkElements items, Rect bounds, Rect viewPortBounds)
        {
            CalculateLayout(items, bounds, CalculateMaximizedElementIndexForArrange,
                delegate (FrameworkElement item) {
                    if (item == Model.MaximizedElement)
                        return CalculateMaximizedElementSize(items, viewPortBounds);
                    else
                        return item.GetDesiredSize();
                },
                delegate (FrameworkElement item, ref FlowLayoutItemPosition itemPosition, ref FlowLayoutItemSize itemSize) {
                    if (item == Model.MaximizedElement)
                        itemPosition.ItemOffset = GetLayerContentStart(viewPortBounds);
                    else
                    {
                        itemSize = GetItemSize(item.DesiredSize);

                        if (StretchContent)
                            itemSize.Width = GetLayerWidth(viewPortBounds);
                    }
                    item.Arrange(GetItemBounds(itemPosition, itemSize));
                });

            var layersCopy = LayerInfos.Select(t => t).ToList();
            CalculateLayout(items, bounds, CalculateMaximizedElementIndexForArrange,
                delegate (FrameworkElement item) {
                    if (item == Model.MaximizedElement)
                        return CalculateMaximizedElementSize(items, viewPortBounds);
                    else if (Model.MaximizedElement != null)
                        return new Size();
                    else
                        return item.GetDesiredSize();
                },
                delegate (FrameworkElement item, ref FlowLayoutItemPosition itemPosition, ref FlowLayoutItemSize itemSize) {
                    var itemIndex = items.IndexOf(item);
                    var layer = layersCopy.FirstOrDefault(l => l.FirstItemIndex <= itemIndex && itemIndex < l.FirstItemIndex + l.SlotCount);
                    var layerBounds = this.GetLayerSpaceBounds(layer, viewPortBounds);
                    Rect newBounds = new Rect();

                    if (item == Model.MaximizedElement)
                    {
                        itemPosition.ItemOffset = GetLayerContentStart(layerBounds);
                        itemSize = GetItemSize(bounds.Size);
                        newBounds = bounds;
                    }
                    else if (Model.MaximizedElement != null)
                    {
                        itemSize = new FlowLayoutItemSize(0, 0);
                    }
                    else
                    {
                        itemSize = GetItemSize(item.DesiredSize);
                        if (StretchContent)
                            itemSize.Width = GetLayerWidth(layerBounds);

                        var oldBounds = GetItemBounds(itemPosition, itemSize);

                        var oldSize = itemSize;
                        var step = GetLayerContentLength(bounds) / layer.SlotCount;
                        if (Orientation == Orientation.Horizontal)
                            itemSize = new FlowLayoutItemSize(itemSize.Width, step);
                        else
                            itemSize = new FlowLayoutItemSize(step, itemSize.Length);

                        itemPosition = new FlowLayoutItemPosition(itemPosition.LayerOffset, GetLayerContentStart(layerBounds) + itemSize.Length * (itemIndex - layer.FirstItemIndex));
                        //itemPosition.ItemOffset + (itemIndex > layer.FirstItemIndex ? itemSize.Length - oldSize.Length : 0)
                        newBounds = GetItemBounds(itemPosition, itemSize);
                    }

                    item.Arrange(newBounds);
                });

            if (ShowLayerSeparators && items.Count != 0)
                AddLayerSeparators(bounds, viewPortBounds);
            return bounds.Size();
        }

        protected override bool IsFlowBreak(FrameworkElement item, Rect bounds, double slotOffset, double slotLength, out bool isHardFlowBreak)
        {

            return base.IsFlowBreak(item, bounds, slotOffset, slotLength, out isHardFlowBreak);
        }
    }
}
