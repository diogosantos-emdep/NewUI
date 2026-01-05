using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Modules.Hrm.ViewModels;
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

namespace Emdep.Geos.Modules.Hrm.Views
{
    /// <summary>
    /// Interaction logic for AddEditAccommodationView.xaml
    /// </summary>
    public partial class AddEditAccommodationView : WinUIDialogWindow
    {
        public AddEditAccommodationView()
        {
            InitializeComponent();
        }
        private void DepTime_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            if (e.NewValue is TimeSpan)
            {
                DevExpress.Xpf.Editors.TextEdit t = (DevExpress.Xpf.Editors.TextEdit)e.Source;
                TimeSpan newTimeSpan = (TimeSpan)e.NewValue;
                if (newTimeSpan.TotalHours > 24 && (newTimeSpan.TotalMinutes > 0 || newTimeSpan.TotalSeconds > 0))
                {
                    newTimeSpan = new TimeSpan(23, newTimeSpan.Minutes, newTimeSpan.Seconds);
                }
                else if (newTimeSpan.TotalHours > 24)
                {
                    newTimeSpan = new TimeSpan(24, 0, 0);
                }
                t.EditValue = newTimeSpan;
            }
        }
        private void DateEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            var dateEdit = sender as DevExpress.Xpf.Editors.DateEdit;
            if (dateEdit == null)
                return;

            var viewModel = this.DataContext as AddEditAccommodationViewModel;
            if (viewModel == null)
                return;

            if (dateEdit.EditValue == null && viewModel.Fromdate.HasValue)
            {
                // Set EditValue to FromDate temporarily so calendar opens on this date
                dateEdit.EditValue = viewModel.Fromdate.Value;

                // Optionally, you could track this and clear the EditValue if user doesn't pick a date
            }
        }
        private void DateEdit_GotFocus1(object sender, RoutedEventArgs e)
        {
            var dateEdit = sender as DevExpress.Xpf.Editors.DateEdit;
            if (dateEdit == null)
                return;

            var viewModel = this.DataContext as AddEditAccommodationViewModel;
            if (viewModel == null)
                return;

            if (dateEdit.EditValue == null && viewModel.ToDate.HasValue)
            {
                // Set EditValue to FromDate temporarily so calendar opens on this date
                dateEdit.EditValue = viewModel.ToDate.Value;

                // Optionally, you could track this and clear the EditValue if user doesn't pick a date
            }
        }

    }
}
