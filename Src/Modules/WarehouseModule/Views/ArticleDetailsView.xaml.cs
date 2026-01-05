using DevExpress.Xpf.WindowsUI;

namespace Emdep.Geos.Modules.Warehouse.Views
{
    /// <summary>
    /// Interaction logic for ArticleDetailsView.xaml
    /// </summary>
    public partial class ArticleDetailsView : WinUIDialogWindow
    {
        public ArticleDetailsView()
        {
            InitializeComponent();
        }

        private void gridControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DevExpress.Xpf.Grid.GridControl obj = (DevExpress.Xpf.Grid.GridControl)sender;

            obj.Columns[1].Width = (obj.ActualWidth * 72) / 100.0;
        }
    }
}
