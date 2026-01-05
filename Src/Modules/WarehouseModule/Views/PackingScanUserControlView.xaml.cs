using System.Windows.Controls;
using DevExpress.Xpf.Grid;
using DevExpress.XtraGrid.Views.Card;
using System.Windows;

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for PackingScanUserControlViewView.xaml
    /// </summary>
    public partial class PackingScanUserControlView : UserControl
    {
        public PackingScanUserControlView()
        {
            InitializeComponent();
        }
       
    
    private void CardViewName_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            var cardView = sender as DevExpress.Xpf.Grid.CardView;
            if (cardView != null)
            {
                // Set the focused row handle to an invalid value to prevent selection
                cardView.FocusedRowHandle = DevExpress.Xpf.Grid.GridControl.InvalidRowHandle;
            }

        }
    }
}
