using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for PORequestLinkedOffersView.xaml
    /// </summary>
    public partial class PORequestLinkedOffersView  : WinUIDialogWindow
    {
        public PORequestLinkedOffersView()
        {
            InitializeComponent();
        }


        #region old code
        //private void LinkedofferGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var view = sender as TableView;
        //    if (view == null)
        //        return;

        //    var source = e.OriginalSource as DependencyObject;
        //    var hitInfo = view.CalcHitInfo(source);

        //    // Only intercept clicks on rows or row cells
        //    if (hitInfo.HitTest == TableViewHitTest.Row || hitInfo.HitTest == TableViewHitTest.RowCell)
        //    {
        //        e.Handled = true; // Stop default selection behavior
        //        bool iscontrolclicked = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //        // Only toggle selection when Ctrl is pressed
        //        if (!iscontrolclicked)
        //        {
        //            int rowHandle = hitInfo.RowHandle;

        //            if (view.IsRowSelected(rowHandle))
        //            {
        //                view.UnselectRow(rowHandle);

        //            }
        //            else
        //            {
        //                view.SelectRow(rowHandle);
        //            }

        //        }

        //    }
        //}

        #endregion

        /// <summary>
        /// [pramod.misal][GEOS2-9139][07-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9139
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkedofferGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var view = LinkedOffersTableView; // Or: var view = sender as TableView;
            if (view == null)
                return;
            var source = e.OriginalSource as DependencyObject;
            var hitInfo = view.CalcHitInfo(source);            
            if (hitInfo.HitTest == TableViewHitTest.RowCell && hitInfo.Column != null)
            {
                if (hitInfo.Column.FieldName == "") 
                {
                    return; 
                }
            }           
            if (hitInfo.HitTest == TableViewHitTest.Row || hitInfo.HitTest == TableViewHitTest.RowCell)
            {
                
                if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
                {
                    view.ClearSelection(); 
                }                
                int rowHandle = hitInfo.RowHandle;              
                if (!view.IsRowSelected(rowHandle))
                {
                    view.SelectRow(rowHandle);
                }            
                e.Handled = true;
            }
        }



    }
}
