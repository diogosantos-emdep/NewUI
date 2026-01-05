using DevExpress.Xpf.Grid.LookUp;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraEditors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Hrm.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AddEditTripsView.xaml
    /// </summary>
    public partial class AddEditTripsView : WinUIDialogWindow
    {
        public AddEditTripsView()
        {
            InitializeComponent();
        }
        //[rdixit][26.09.2024][GEOS2-6476]
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
    }
}
