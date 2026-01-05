using DevExpress.Data.Filtering;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.APM;
using Emdep.Geos.Modules.APM.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Emdep.Geos.Modules.APM.Views
{
    /// <summary>
    /// Interaction logic for ActionPlansView.xaml
    /// </summary>
    public partial class ActionPlansView : System.Windows.Controls.UserControl
    {
        public ActionPlansView()
        {
            InitializeComponent();
            // Get the ViewModel
            //var viewModel = DataContext as ActionPlansViewModel;
            // Explicitly set the DataContext
            var viewModel = new ActionPlansViewModel();
            DataContext = viewModel;
            // Pass GridControl reference to the ViewModel
            viewModel?.SetGridControl(gridControl, ActionPlansTableView);

        }

        public TableView ActionPlansTableViewProp => ActionPlansTableView;
        public TableView TasksGridTableViewProp => TasksGridTableView;

        private object _previousCheckedItems;//[pallavi.kale][GEOS2-7215][22.07.2025]

        //[pallavi.kale][GEOS2-7215][22.07.2025]
        private void ComboBoxEdit_PopupOpening(object sender, DevExpress.Xpf.Editors.OpenPopupEventArgs e)
        {

            if (sender is ComboBoxEdit comboBox)
            {
                _previousCheckedItems = comboBox.EditValue;
            }
        }
        //[pallavi.kale][GEOS2-7215][22.07.2025]
        private void ComboBoxEdit_PopupClosing(object sender, DevExpress.Xpf.Editors.ClosingPopupEventArgs e)
        {

            if (e.CloseMode == PopupCloseMode.Cancel && sender is ComboBoxEdit comboBox)
            {
                if (_previousCheckedItems != null)
                {
                    comboBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var old = _previousCheckedItems;


                        comboBox.EditValue = null;
                        comboBox.EditValue = old;


                        comboBox.UpdateLayout();
                        comboBox.Focus();
                        Keyboard.ClearFocus();
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }

        }

       
    }
}
