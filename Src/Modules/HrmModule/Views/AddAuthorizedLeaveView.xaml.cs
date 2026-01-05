using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common.Hrm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for AddAuthorizedLeaveView.xaml
    /// </summary>
    public partial class AddAuthorizedLeaveView : WinUIDialogWindow
    {
        public AddAuthorizedLeaveView()
        {
            InitializeComponent();
        }

        private void TableView_ValidateCell(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            if (e.Column.FieldName == "Comments")
            {
                e.IsValid = !string.IsNullOrEmpty(e.Value as string);
            }
        }

        private void TableView_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            var item = e.Row as EmployeeAnnualAdditionalLeave;
            if (string.IsNullOrEmpty(item.Comments))
            {
                e.IsValid = false;
                e.ErrorContent = "You cannot leave the Comments empty";
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;

            }
        }

        private void TableView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ErrorText = "You cannot leave the Comments empty.";
        }
    }


    public class TableViewEx : TableView
    {
        protected override MessageBoxResult DisplayInvalidRowError(IInvalidRowExceptionEventArgs e)
        {
            //var result = base.DisplayInvalidRowError(e);
            var result = System.Windows.MessageBoxResult.Yes;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Grid.CurrentColumn = this.Grid.Columns["Comments"];
            }), DispatcherPriority.Loaded);
            //if (result == MessageBoxResult.Yes)
            //{
            //    Dispatcher.BeginInvoke(new Action(() => {
            //        this.Grid.CurrentColumn = this.Grid.Columns["Comments"];
            //    }), DispatcherPriority.Loaded);
            //}
            //else
            //{
            //    Dispatcher.BeginInvoke(new Action(() => {
            //        this.Grid.CurrentColumn = this.Grid.Columns["Comments"];
            //    }), DispatcherPriority.Loaded);
            //}
            return result;
        }
    }
}
