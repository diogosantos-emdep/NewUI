using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{   /// <summary>
    /// [pramod.misal][GEOS2-9293][28-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9293
    /// </summary>
    public class FixedColumnHelper :Behavior<TableView>
    {

        #region Old code
        public GridColumnMenuInfo gridColumnMenuInfo;
        public string OTM_PORequestesGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PORequestesGridSetting.Xml";
        
        public bool IsBusy { get; set; }
   
        public FixedColumnHelper()
        {

        }

        public static void SetParentBand(DependencyObject obj, GridControlBand value)
        {
            obj.SetValue(ParentBandProperty, value);
        }

        public static readonly DependencyProperty ParentBandProperty =
           DependencyProperty.RegisterAttached("ParentBand", typeof(GridControlBand), typeof(FixedColumnHelper), new PropertyMetadata(null));

        public static GridControlBand GetParentBand(DependencyObject obj)
        {
            return (GridControlBand)obj.GetValue(ParentBandProperty);
        }

       

        public static FixedStyle GetFixedState(DependencyObject obj)
           => (FixedStyle)obj.GetValue(FixedStateProperty);
        
        public static readonly DependencyProperty FixedStateProperty =DependencyProperty.RegisterAttached("FixedState", typeof(FixedStyle), typeof(FixedColumnHelper), new PropertyMetadata(FixedStyle.None));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.ShowGridMenu += AssociatedObject_ShowGridMenu;

            // ✅ Initialize ParentBand for all columns
            InitializeOriginalParentBands();


        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.ShowGridMenu -= AssociatedObject_ShowGridMenu;
            base.OnDetaching();
        }
        private void AssociatedObject_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            if (e.MenuType == GridMenuType.Column)
            {
                var lockToLeft = new BarButtonItem() { Content = "Lock To Left", Tag = "left" };
                var lockToRight = new BarButtonItem() { Content = "Lock To Right", Tag = "right" };
                var unlock = new BarButtonItem() { Content = "Unlock", Tag = "none" };

                if (e.MenuInfo.Column.ParentBand == null)
                {
                    if (e.MenuInfo.Column.Fixed == FixedStyle.None)
                        unlock.IsEnabled = false;
                    if (e.MenuInfo.Column.Fixed == FixedStyle.Left)
                        lockToLeft.IsEnabled = false;
                    if (e.MenuInfo.Column.Fixed == FixedStyle.Right)
                        lockToRight.IsEnabled = false;
                }
                else
                {
                    if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.None)
                        unlock.IsEnabled = false;
                    if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.Left)
                        lockToLeft.IsEnabled = false;
                    if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.Right)
                        lockToRight.IsEnabled = false;
                }

                lockToLeft.ItemClick += LockUnlock_ItemClick;
                lockToRight.ItemClick += LockUnlock_ItemClick;
                unlock.ItemClick += LockUnlock_ItemClick;

                e.Customizations.Add(new InsertAction() { Element = new BarItemSeparator(), Index = 7 });
                e.Customizations.Add(new InsertAction() { Element = lockToLeft, Index = 8 });
                e.Customizations.Add(new InsertAction() { Element = lockToRight, Index = 9 });
                e.Customizations.Add(new InsertAction() { Element = unlock, Index = 10 });
                e.Customizations.Add(new InsertAction() { Element = new BarItemSeparator(), Index = 11 });
            }
        }

        private void InitializeOriginalParentBands()
        {
            if (AssociatedObject?.Grid == null)
                return;

            var grid = AssociatedObject.Grid;

            foreach (var col in grid.Columns)
            {
                if (col.ParentBand is GridControlBand parentBand && GetParentBand(col) == null)
                {
                    SetParentBand(col, parentBand);
                }
            }
        }




        public static void SetFixedState(GridColumn column, FixedStyle state)
        {
            column.Tag = state; // simple way to store state
            column.Fixed = state; // also set actual Fixed property
        }

        public static FixedStyle GetFixedState(GridColumn column)
        {
            if (column.Tag is FixedStyle state)
                return state;
            return FixedStyle.None;
        }


        private void LockUnlock_ItemClick(object sender, ItemClickEventArgs e)
        {
            Processing();
            var tag = e.Item.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) return;

            var menuInfo = GridPopupMenu.GetGridMenuInfo(e.Item) as GridColumnMenuInfo;
            gridColumnMenuInfo = menuInfo;
            if (menuInfo == null) return;

            var grid = menuInfo.Grid;
            var column = menuInfo.Column;
         
            GridControlBand targetBand = null;

            switch (tag)
            {
                case "left":
                    SetFixedState(menuInfo.Column, FixedStyle.Left);
                    targetBand = GetOrCreateFixedBand(grid, "fixedLeftBand", "Pinned Left", FixedStyle.Left);
                    MoveColumnToBand(menuInfo.Column, targetBand);
                    
                    break;
                case "right":
                    SetFixedState(menuInfo.Column, FixedStyle.Right);
                    targetBand = GetOrCreateFixedBand(grid, "fixedRightBand", "Pinned Right", FixedStyle.Right);
                    MoveColumnToBand(menuInfo.Column, targetBand);
                    
                    break;
                case "none":
                    SetFixedState(menuInfo.Column, FixedStyle.None);
                    RestoreColumnToOriginalBand(menuInfo.Column);
                    
                    break;
            }

            CloseProcessing();



        }


        private GridControlBand GetOrCreateFixedBand(GridControl grid, string tag, string header, FixedStyle fixedStyle)
        {
            var band = grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == tag);
            if (band == null)
            {
                band = new GridControlBand { Header = header, Tag = tag, Fixed = fixedStyle };
                grid.Bands.Add(band);
            }
            else band.Visible = true;

            return band;
        }

        private void MoveColumnToBand(GridColumn column, GridControlBand fixedBand)
        {
            if (column.ParentBand is GridControlBand currentBand && currentBand != fixedBand)
            {
                if (GetParentBand(column) == null)
                    SetParentBand(column, currentBand);

                currentBand.Columns.Remove(column);
                if (currentBand.Columns.Count == 0)
                    currentBand.Visible = false;
            }

            if (!fixedBand.Columns.Contains(column))
                fixedBand.Columns.Add(column);

            column.Visible = true;
            column.Fixed = fixedBand.Fixed;
        }

        private void RestoreColumnToOriginalBand(GridColumn column)
        {
            if (column.ParentBand is GridControlBand currentBand)
            {
                currentBand.Columns.Remove(column);
                if (currentBand.Columns.Count == 0)
                    currentBand.Visible = false;
            }

            var originalBand = GetParentBand(column);
            if (originalBand==null)
            {

            }
            if (originalBand != null)
            {
                originalBand.Columns.Add(column);
                originalBand.Visible = true;
                SetParentBand(column, null);
            }

            column.Visible = true;
            column.Fixed = FixedStyle.None;
        }


        void CloseProcessing()
        {
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private Action Processing()
        {
            IsBusy = true;
            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new HelperSplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }

        #endregion


        // Use your existing user-specific path
        //private readonly string OTM_PORequestesGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "PORequestesGridSetting.Xml";

        //public static void SetParentBand(DependencyObject obj, GridControlBand value)
        //{
        //    obj.SetValue(ParentBandProperty, value);
        //}

        //public static readonly DependencyProperty ParentBandProperty =
        //    DependencyProperty.RegisterAttached("ParentBand", typeof(GridControlBand), typeof(FixedColumnHelper), new PropertyMetadata(null));

        //public static GridControlBand GetParentBand(DependencyObject obj)
        //{
        //    return (GridControlBand)obj.GetValue(ParentBandProperty);
        //}

        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    this.AssociatedObject.ShowGridMenu += AssociatedObject_ShowGridMenu;
        //}

        //private void AssociatedObject_ShowGridMenu(object sender, GridMenuEventArgs e)
        //{
        //    if (e.MenuType == GridMenuType.Column)
        //    {
        //        var lockToLeft = new BarButtonItem() { Content = "Lock To Left", Tag = "left" };
        //        var lockToRight = new BarButtonItem() { Content = "Lock To Right", Tag = "right" };
        //        var unlock = new BarButtonItem() { Content = "Unlock", Tag = "none" };

        //        if (e.MenuInfo.Column.ParentBand == null)
        //        {
        //            if (e.MenuInfo.Column.Fixed == FixedStyle.None)
        //                unlock.IsEnabled = false;
        //            if (e.MenuInfo.Column.Fixed == FixedStyle.Left)
        //                lockToLeft.IsEnabled = false;
        //            if (e.MenuInfo.Column.Fixed == FixedStyle.Right)
        //                lockToRight.IsEnabled = false;
        //        }
        //        else
        //        {
        //            if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.None)
        //                unlock.IsEnabled = false;
        //            if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.Left)
        //                lockToLeft.IsEnabled = false;
        //            if (e.MenuInfo.Column.ParentBand.Fixed == FixedStyle.Right)
        //                lockToRight.IsEnabled = false;
        //        }

        //        lockToLeft.ItemClick += LockUnlock_ItemClick;
        //        lockToRight.ItemClick += LockUnlock_ItemClick;
        //        unlock.ItemClick += LockUnlock_ItemClick;

        //        e.Customizations.Add(new InsertAction() { Element = new BarItemSeparator(), Index = 7 });
        //        e.Customizations.Add(new InsertAction() { Element = lockToLeft, Index = 8 });
        //        e.Customizations.Add(new InsertAction() { Element = lockToRight, Index = 9 });
        //        e.Customizations.Add(new InsertAction() { Element = unlock, Index = 10 });
        //        e.Customizations.Add(new InsertAction() { Element = new BarItemSeparator(), Index = 11 });
        //    }
        //}

        //private void LockUnlock_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var tag = e.Item.Tag?.ToString();
        //    if (string.IsNullOrEmpty(tag))
        //        return;

        //    var menuInfo = GridPopupMenu.GetGridMenuInfo(e.Item) as GridColumnMenuInfo;
        //    if (menuInfo == null)
        //        return;

        //    var grid = menuInfo.Grid as GridControl;

        //    try
        //    {
        //        switch (tag)
        //        {
        //            case "left":
        //                HandleLock(menuInfo, FixedStyle.Left, "fixedLeftBand");
        //                break;
        //            case "right":
        //                HandleLock(menuInfo, FixedStyle.Right, "fixedRightBand");
        //                break;
        //            case "none":
        //                HandleUnlock(menuInfo);
        //                break;
        //        }

        //        // ✅ Auto-save layout after lock/unlock
        //        SaveGridLayout(grid);

        //        GeosApplication.Instance.Logger.Log("Grid layout auto-saved after lock/unlock.", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error in LockUnlock_ItemClick(): " + ex.Message, category: Category.Exception, priority: Priority.High);
        //    }
        //}

        //private void HandleLock(GridColumnMenuInfo menuInfo, FixedStyle style, string tag)
        //{
        //    if (menuInfo.Column.ParentBand != null)
        //    {
        //        var parentBand = (GridControlBand)menuInfo.Column.ParentBand;
        //        var band = menuInfo.Grid.Bands.FirstOrDefault(b => b.Tag?.ToString() == tag);
        //        if (band == null)
        //        {
        //            band = new GridControlBand() { Header = style == FixedStyle.Left ? "Fixed Left" : "Fixed Right", Tag = tag, Fixed = style };
        //            menuInfo.Grid.Bands.Add(band);
        //        }
        //        else
        //            band.Visible = true;

        //        if (GetParentBand(menuInfo.Column) == null)
        //            SetParentBand(menuInfo.Column, parentBand);

        //        parentBand.Columns.Remove(menuInfo.Column);
        //        if (parentBand.Columns.Count == 0)
        //            parentBand.Visible = false;

        //        band.Columns.Add(menuInfo.Column);
        //    }
        //    else
        //    {
        //        menuInfo.Column.Fixed = style;
        //    }
        //}

        //private void HandleUnlock(GridColumnMenuInfo menuInfo)
        //{
        //    if (menuInfo.Column.ParentBand != null)
        //    {
        //        var parentBand = (GridControlBand)menuInfo.Column.ParentBand;
        //        parentBand.Columns.Remove(menuInfo.Column);

        //        var originalBand = GetParentBand(menuInfo.Column);
        //        if (originalBand != null)
        //        {
        //            originalBand.Columns.Add(menuInfo.Column);
        //            originalBand.Visible = true;
        //            SetParentBand(menuInfo.Column, null);
        //        }

        //        if (parentBand.Columns.Count == 0)
        //            parentBand.Visible = false;
        //    }
        //    else
        //    {
        //        menuInfo.Column.Fixed = FixedStyle.None;
        //    }
        //}

        //private void SaveGridLayout(GridControl grid)
        //{
        //    try
        //    {
        //        if (grid == null) return;

        //        var dir = Path.GetDirectoryName(OTM_PORequestesGridSettingFilePath);
        //        if (!Directory.Exists(dir))
        //            Directory.CreateDirectory(dir);

        //        grid.SaveLayoutToXml(OTM_PORequestesGridSettingFilePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Error saving grid layout: " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}

        //protected override void OnDetaching()
        //{
        //    this.AssociatedObject.ShowGridMenu -= AssociatedObject_ShowGridMenu;
        //    base.OnDetaching();
        //}
    }
}
