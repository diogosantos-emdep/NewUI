using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common.OTM;
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
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for AddReceiversView.xaml
    /// </summary>
    public partial class AddToAndCcEmailView : WinUIDialogWindow
    {
        public AddToAndCcEmailView()
        {
            InitializeComponent();
        }

        void OnDragRecordOver(object sender, DragRecordOverEventArgs e)
        {
            if (e.IsFromOutside && typeof(People).IsAssignableFrom(e.GetRecordType()))
            {

                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
            
        }
    }
}
