using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Emdep.Geos.Data.Common.SRM;
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

namespace Emdep.Geos.Modules.SRM.Views
{
    /// <summary>
    /// Interaction logic for AddReceiversView.xaml
    /// </summary>
    public partial class AddReceiversView : WinUIDialogWindow
    {
        public AddReceiversView()
        {
            InitializeComponent();
        }

        void OnDragRecordOver(object sender, DragRecordOverEventArgs e)
        {
            if (e.IsFromOutside && typeof(Contacts).IsAssignableFrom(e.GetRecordType()))
            {
                var data = (RecordDragDropData)e.Data.GetData(typeof(RecordDragDropData));
                List<Contacts> newRecords = data.Records.OfType<Contacts>().Select(x => new Contacts { IsMainContact=x.IsMainContact }).ToList();
                Contacts temp = newRecords.FirstOrDefault();
                if (temp != null&& temp.IsMainContact==false)
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                }
                
            }
        }
    }
}
