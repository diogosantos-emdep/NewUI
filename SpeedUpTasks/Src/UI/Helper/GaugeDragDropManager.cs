using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Gauges;


namespace Emdep.Geos.UI.Helper
{
    public class GaugeDragDropFactory : DragDropManagerBase.DragDropManagerDropTargetFactory { }

    public class GaugeItemDragDropElementHelperStrategy : BaseDragDropStrategy
    {
        public GaugeItemDragDropElementHelperStrategy(ISupportDragDrop supportDragDrop, DragDropElementHelper helper)
            : base(supportDragDrop, helper) { }
        public override FrameworkElement GetTopVisual(FrameworkElement node)
        {
            return (FrameworkElement)LayoutHelper.GetTopLevelVisual(node);
        }
        public override BaseLocationStrategy CreateLocationStrategy()
        {
            return base.CreateLocationStrategy();
        }
    }
    public class GaugeItemDragDropElementHelper : DragDropElementHelperBounded
    {
        public GaugeItemDragDropElementHelper(ISupportDragDrop supportDragDrop, bool isRelativeMode = true)
            : base(supportDragDrop, isRelativeMode) { }
        protected override BaseDragDropStrategy CreateDragDropStrategy()
        {
            return new GaugeItemDragDropElementHelperStrategy(SupportDragDrop, this);
        }
        protected override Point CorrectDragElementLocation(Point newPos)
        {
            return PointHelper.Subtract(base.CorrectDragElementLocation(newPos), MouseDownPositionCorrection);
        }
        DragDropManagerBase DragDropManager
        {
            get
            {
                return ((DragDropObjectBase)SupportDragDrop).DragDropManagerBase as DragDropManagerBase;
            }
        }
        protected override void StartDragging(Point offset, IndependentMouseEventArgs e)
        {
            var DDManager = DragDropManager as GaugeDragDropManager;
            if (DDManager != null && DDManager.CustomAllowDragEx(e) && DDManager.DraggingRows != null && DDManager.DraggingRows.Count > 0 && DDManager.AllowDrag)
                base.StartDragging(offset, e);
        }
        protected override void EndDragging(IndependentMouseButtonEventArgs e)
        {
            base.EndDragging(e);
            DragDropManagerBase DDManager = DragDropManager;
            if (DDManager != null)
            {
                //DDManager.DraggingRows = null;
                //DDManager.ViewInfo.DraggingRows = null;
                DDManager.ViewInfo.FirstDraggingObject = null;
            }
        }
        protected override Point GetStartDraggingOffset(IndependentMouseEventArgs e, FrameworkElement sourceElement)
        {
            return GetPosition(e, SupportDragDrop.SourceElement);
        }
    }

    public class GaugeDropEventArgs : DropEventArgs
    {
        public GaugeDragDropManager Manager { get; protected set; }
        public GaugeDropEventArgs(GaugeDragDropManager manager, DragDropManagerBase sourceManager, IList dragRows)
            : base(sourceManager, dragRows)
        {
            Manager = manager;
        }
    }
    public delegate void GaugeDroppedEventHandler(object sender, GaugeDropEventArgs e);

    public class GaugeDragDropManager : DragDropManagerBase
    {
        #region inner classes
        public class GaugeDragSource : SupportDragDropBase
        {
            GaugeDragDropManager GaugeDragDropManager { get { return (GaugeDragDropManager)dragDropManager; } }
            protected override FrameworkElement Owner { get { return GaugeDragDropManager.Gauge; } }
            public GaugeDragSource(GaugeDragDropManager dragDropManager)
                : base(dragDropManager)
            {
            }
            protected override FrameworkElement SourceElementCore
            {
                get { return GaugeDragDropManager.Gauge; }
            }
        }
        #endregion

        #region Events
        [Category("Events")]
        GaugeDroppedEventHandler DropEventHandler;
        public event GaugeDroppedEventHandler Drop
        {
            add { DropEventHandler += value; }
            remove { DropEventHandler -= value; }
        }
        #endregion

        public GaugeControlBase Gauge;
        public Point MouseLeftButtonPoint;
        public UIElement PreviousElement = null;
        //public TableDragIndicatorPosition AdornerPosition;

        protected override void OnAttached()
        {
            base.OnAttached();
            Gauge = this.AssociatedObject as GaugeControlBase;

            DragManager.SetAllowMouseMoveSelectionFunc(Gauge, new Func<MouseEventArgs, bool>((e) => this.AllowMouseMoveSelection(e)));
            SetDragDropManager(Gauge, this);
            DragDropHelper = new GaugeItemDragDropElementHelper(CreateDragSource(this));
            DragManager.SetDropTargetFactory(Gauge, new GaugeDragDropFactory());
        }

        protected virtual bool AllowMouseMoveSelection(MouseEventArgs args)
        {
            return false;
        }
        protected virtual ISupportDragDrop CreateDragSource(GaugeDragDropManager GaugeDragDropManager)
        {
            return new GaugeDragSource(GaugeDragDropManager);
        }

        protected override System.Collections.IList ItemsSource
        {
            get { return new List<object>(); }
        }

        protected override void OnDrop(DragDropManagerBase sourceManager, UIElement source, Point pt)
        {
            HideDropMarker();

            GaugeDropEventArgs e = RaiseDropEvent(sourceManager, sourceManager.DraggingRows);

            if (!e.Handled)
                PerformDropToView(sourceManager, source, pt);

            base.OnDrop(sourceManager, source, pt);
        }

        protected virtual GaugeDropEventArgs RaiseDropEvent(DragDropManagerBase sourceManager, IList draggedRows)
        {
            GaugeDropEventArgs e = new GaugeDropEventArgs(this, sourceManager, draggedRows)
            {
                Handled = false,
            };
            if (DropEventHandler != null)
                DropEventHandler(this, e);
            return e;
        }

        protected virtual void PerformDropToView(DragDropManagerBase sourceManager, UIElement source, Point pt)
        {
            //add items here
        }

        public override System.Collections.IList GetSource(object row)
        {
            return base.GetSource(row);
        }

        protected internal virtual bool CustomAllowDragEx(IndependentMouseEventArgs e)
        {
            return CustomAllowDrag(e);
        }
        protected override bool CustomAllowDrag(IndependentMouseEventArgs e)
        {
            DraggingRows = CalcDraggingRows(e);
            StartDragEventArgs startDragArgs = RaiseStartDragEvent(e);
            return startDragArgs != null ? startDragArgs.CanDrag : true;
        }

        public override void OnDragOver(DragDropManagerBase sourceManager, UIElement source, Point pt)
        {
            if (AllowDrop)
            {
                sourceManager.ViewInfo.DropTargetType = DropTargetType.DataArea;
                ShowDropMarker(Gauge, TableDragIndicatorPosition.InRow);
            }
            else
                HideDropMarker();
        }

        protected override System.Collections.IList CalcDraggingRows(DevExpress.Xpf.Core.IndependentMouseEventArgs e)
        {
            return new List<object>();
        }
        protected override bool CanStartDrag(System.Windows.Input.MouseButtonEventArgs e)
        {
            return false;
        }
        protected override DevExpress.Xpf.Grid.DragDrop.StartDragEventArgs RaiseStartDragEvent(DevExpress.Xpf.Core.IndependentMouseEventArgs e)
        {
            return new StartDragEventArgs { CanDrag = AllowDrag };
        }
    }
}